using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;
using StockCore.Domain.Entities;

namespace StockCore.Application.Features.Warehouse.Commands
{
    public static class CreateWarehouse
    {
        public record Command(string Name) : IRequest<Result<Response>>;
        public record Response(Guid WarehouseId);

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Warehouse name cannot be empty");
            }
        }

        public class Handler(IAppDbContext appDbContext, IUserDetails userDetails) : IRequestHandler<Command, Result<Response>>
        {
            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var warehouseExist = await appDbContext.Warehouses.FirstOrDefaultAsync(w => w.Name == request.Name,cancellationToken);

                if (warehouseExist != null)
                {
                    return Result.Failure<Response>($"Warehouse with name {request.Name} already exist");
                }


                // Initialize the main product entity
                var warehouse = new Domain.Entities.Warehouse(
                        Guid.NewGuid(),
                        userDetails.TenantId,
                        request.Name
                    );

                await appDbContext.SaveChangesAsync(cancellationToken);

                return Result.Success(new Response(warehouse.Id));
            }
        }

    }
}
