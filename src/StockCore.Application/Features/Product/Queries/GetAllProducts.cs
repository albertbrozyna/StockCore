using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common;
using StockCore.Application.Common.Interfaces;

namespace StockCore.Application.Features.Product.Queries
{
    public static class GetAllProducts
    {

        public record Filters(string? Name, decimal? MinPrice, decimal? MaxPrice);
        public record Pagination(int Page, int PageSize);
        public record Sorting(string? SortBy, bool IsDescending = false);

        public record Query(Filters Filters, Pagination Pagination, Sorting Sorting)
            : IRequest<Result<PagedList<ProductResponse>>>;

        // Response includes basic product info and a list of its attributes
        public record ProductResponse(
            Guid Id,
            string Name,
            decimal Price,
            string Code,
            List<AttributeResponse> Attributes);

        public record AttributeResponse(string Name, string Value, string Type);

        // --- Validator ---

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Pagination.Page).GreaterThan(0);
                RuleFor(x => x.Pagination.PageSize).InclusiveBetween(1, 100);
                RuleFor(x => x.Sorting.SortBy)
                    .Must(x => string.IsNullOrEmpty(x) || new[] { "name", "price", "code" }.Contains(x.ToLower()))
                    .WithMessage("Invalid sort property.");
            }
        }

        // --- Handler ---

        public class Handler(IAppDbContext context) : IRequestHandler<Query, Result<PagedList<ProductResponse>>>
        {
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