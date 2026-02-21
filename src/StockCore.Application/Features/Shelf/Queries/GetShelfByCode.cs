using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common;
using StockCore.Application.Common.DTOs;
using StockCore.Application.Common.Interfaces;

namespace StockCore.Application.Features.Shelf.Queries
{
    public static class GetShelfByCode
    {
        public record Query(string ShelfCode): IRequest<Result<Response>>;
        public record Response(Guid ShelfId,Guid RackId,int Level,string SheflCode,bool IsFull);

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.ShelfCode).NotEmpty().WithMessage("Shelf code cannot be empty");
            }
        }

        // --- Handler ---

        public class Handler(IAppDbContext appDbContext) : IRequestHandler<Query, Result<Response>> { 
            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var shelf = await appDbContext.Shelfs.FirstOrDefaultAsync(shelf => shelf.ShelfCode.ToLower() == request.ShelfCode.ToLower());

                if (shelf == null)
                {
                    return Result.Failure<Response>("Shelf with this code doesn't exist");
                }

                var shelfDto = new Response(shelf.Id, shelf.RackId, shelf.Level, shelf.ShelfCode, shelf.IsFull);

                return Result.Success<Response>(shelfDto);
            }
        }
    }
}
