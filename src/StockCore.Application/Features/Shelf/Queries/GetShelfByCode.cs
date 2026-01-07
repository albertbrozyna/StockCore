using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common;
using StockCore.Application.Common.DTOs;
using StockCore.Application.Common.Interfaces;

namespace StockCore.Application.Features.Product.Queries
{
    public static class GetShelfByCode
    {
        public record Query(string ShelfCode): IRequest<Result<PagedList<ProductResponse>>>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.ShelfCode).NotEmpty().WithMessage("Shelf code cannot be empty");
            }
        }

        // --- Handler ---

        public class Handler(IAppDbContext context) : IRequestHandler<Query, Task<Result<PagedList<ProductResponse>>>>
            public async Task<Result<PagedList<ProductResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Start with AsNoTracking and include related data safely through projection
                var query = context.Products.AsNoTracking();


                // Filtering
                if (!string.IsNullOrWhiteSpace(request.Filters.Name))
                    query = query.Where(x => x.Name.Contains(request.Filters.Name));

                if (request.Filters.MinPrice.HasValue)
                    query = query.Where(x => x.Price >= request.Filters.MinPrice.Value);

                if (request.Filters.MaxPrice.HasValue)
                    query = query.Where(x => x.Price <= request.Filters.MaxPrice.Value);

                // Sorting
                query = request.Sorting.SortBy?.ToLower() switch
                {
                    "name" => request.Sorting.IsDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                    "price" => request.Sorting.IsDescending ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                    "code" => request.Sorting.IsDescending ? query.OrderByDescending(x => x.Code) : query.OrderBy(x => x.Code),
                    _ => query.OrderBy(x => x.Id)
                };

                // Projection - This joins ProductAttributeValue and AttributeDefinition efficiently
                var projectedQuery = query.Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Code,
                    p.AttributeValues.Select(av => new AttributeResponse(
                        av.AttributeDefinition.Name, // Pulling name from the definition table
                        av.Value,
                        av.AttributeDefinition.DataType
                    )).ToList()
                ));

                // Execute Paged List
                var pagedResult = await PagedList<ProductResponse>.CreateAsync(
                    projectedQuery,
                    request.Pagination.Page,
                    request.Pagination.PageSize
                );

                return Result.Success(pagedResult);
            }
        }
    }
}