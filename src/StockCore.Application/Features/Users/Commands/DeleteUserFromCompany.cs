using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;

namespace StockCore.Application.Features.Users.Commands
{
    public static class DeleteUserFromCompany
    {
        public record Command(Guid UserId) : IRequest<Result<Response>>;
        public record Response();
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("User id cannot be empty");
            }
        }

        public class BlockUserHandler : IRequestHandler<Command, Result<Response>>
        {
            private readonly IAppDbContext _appDbContext;

            public BlockUserHandler(IAppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == request.UserId);

                return await user.AsMaybe().ToResult($"User with Id: {request.UserId} was not found")
                    .Ensure(user => !user.IsAcceptedByCompany, $"User with Id {request.UserId} is already deleted from company")
                    .Tap(user => user.DeleteUserFromCompany())
                    .Tap(async _ => await _appDbContext.SaveChangesAsync(cancellationToken))
                    .Map(user => new Response());
            }
        }
    }

}
