using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Warehouse.Api.Application;
using Warehouse.Api.Application.Abstractions;
using Warehouse.Api.Application.Abstractions.Messaging;
using Warehouse.Api.Domain.SharedKernel;
using Warehouse.Api.Domain.SharedKernel.Ids;

namespace Warehouse.UnitTests.Application;

public sealed class DispatcherPipelineTests
{
    private sealed record Add(int A, int B) : ICommand<int>;

    private sealed record Throw : ICommand<int>;

    private sealed record Get(int Id) : IQuery<string>;

    private sealed class AddHandler : ICommandHandler<Add, int>
    {
        public Task<Result<int>> HandleAsync(Add command, CancellationToken ct) =>
            Task.FromResult(command.A < 0
                ? Result.Failure<int>(new Error("Test.Negative", "neg", ErrorType.Conflict))
                : Result.Success(command.A + command.B));
    }

    private sealed class ThrowHandler : ICommandHandler<Throw, int>
    {
        public Task<Result<int>> HandleAsync(Throw command, CancellationToken ct) => throw new InvalidOperationException("boom");
    }

    private sealed class GetHandler : IQueryHandler<Get, string>
    {
        public Task<Result<string>> HandleAsync(Get query, CancellationToken ct) => Task.FromResult(Result.Success($"item-{query.Id}"));
    }

    private sealed class AddValidator : AbstractValidator<Add>
    {
        public AddValidator() => RuleFor(x => x.B).GreaterThanOrEqualTo(0);
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public List<string> Calls { get; } = [];

        public Task<int> SaveChangesAsync(CancellationToken ct) => Record("save", 1);

        public Task BeginTransactionAsync(CancellationToken ct) => Record("begin", 0);

        public Task CommitAsync(CancellationToken ct) => Record("commit", 0);

        public Task RollbackAsync(CancellationToken ct) => Record("rollback", 0);

        private Task<int> Record(string call, int value)
        {
            Calls.Add(call);
            return Task.FromResult(value);
        }
    }

    private sealed class FakeTenant(bool resolved) : ITenantContext
    {
        public TenantId TenantId { get; } = TenantId.New();

        public bool IsResolved { get; } = resolved;
    }

    private static (IDispatcher Dispatcher, FakeUnitOfWork Uow) Build(bool tenantResolved = true)
    {
        var uow = new FakeUnitOfWork();
        var services = new ServiceCollection();
        services.AddSingleton(NullLoggerFactory.Instance);
        services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger<>), typeof(NullLogger<>));
        services.AddSingleton<IUnitOfWork>(uow);
        services.AddSingleton<ITenantContext>(new FakeTenant(tenantResolved));
        services.AddApplication();
        services.AddHandlersFromAssembly(typeof(DispatcherPipelineTests).Assembly);
        return (services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<IDispatcher>(), uow);
    }

    [Fact]
    public async Task Command_runs_through_pipeline_and_commits()
    {
        var (dispatcher, uow) = Build();

        var result = await dispatcher.SendAsync(new Add(2, 3), CancellationToken.None);

        result.Value.ShouldBe(5);
        uow.Calls.ShouldBe(["begin", "save", "commit"]);
    }

    [Fact]
    public async Task Failed_command_rolls_back()
    {
        var (dispatcher, uow) = Build();

        var result = await dispatcher.SendAsync(new Add(-1, 3), CancellationToken.None);

        result.Error.Code.ShouldBe("Test.Negative");
        uow.Calls.ShouldBe(["begin", "rollback"]);
    }

    [Fact]
    public async Task Exception_rolls_back_and_propagates()
    {
        var (dispatcher, uow) = Build();

        await Should.ThrowAsync<InvalidOperationException>(() => dispatcher.SendAsync(new Throw(), CancellationToken.None));

        uow.Calls.ShouldBe(["begin", "rollback"]);
    }

    [Fact]
    public async Task Validation_failure_short_circuits_before_transaction()
    {
        var (dispatcher, uow) = Build();

        var result = await dispatcher.SendAsync(new Add(1, -5), CancellationToken.None);

        result.Error.Code.ShouldBe("Validation.Failed");
        uow.Calls.ShouldBeEmpty();
    }

    [Fact]
    public async Task Unresolved_tenant_is_forbidden()
    {
        var (dispatcher, uow) = Build(tenantResolved: false);

        var result = await dispatcher.SendAsync(new Add(1, 1), CancellationToken.None);

        result.Error.Type.ShouldBe(ErrorType.Forbidden);
        uow.Calls.ShouldBeEmpty();
    }

    [Fact]
    public async Task Query_skips_transaction_behavior()
    {
        var (dispatcher, uow) = Build();

        var result = await dispatcher.QueryAsync(new Get(7), CancellationToken.None);

        result.Value.ShouldBe("item-7");
        uow.Calls.ShouldBeEmpty();
    }
}
