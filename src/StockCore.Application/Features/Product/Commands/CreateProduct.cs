using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;
using StockCore.Domain.Entities;

namespace StockCore.Application.Features.Product.Commands
{
    public static class CreateProduct
    {


        public record Command(string Name, decimal Price, string Code, string Description, IList<Attribute> Attributes) : IRequest<Result<Response>>;
        public record Response(Guid ProductId);

        public record Attribute(string AttributeName, string AttributeType, string Value);



        public class Handler(IAppDbContext appDbContext, IUserDetails userDetails) : IRequestHandler<Command, Result<Response>>
        {
            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Initialize the main product entity
                var product = new Domain.Entities.Product(
                    Guid.NewGuid(),
                    userDetails.TenantId,
                    request.Name,
                    request.Price,
                    request.Description,
                    request.Code);

                // Extract unique attribute names to minimize database roundtrips
                var attrNames = request.Attributes
                    .Select(a => a.AttributeName)
                    .Distinct()
                    .ToList();

                // Fetch existing definitions for this tenant in a single query 
                var existingDefs = await appDbContext.AttributeDefinitions
                    .Where(x => x.TenantId == userDetails.TenantId && attrNames.Contains(x.Name))
                    .ToListAsync(cancellationToken);

                foreach (var attrRequest in request.Attributes)
                {
                    // Lookup the definition in the locally cached list
                    var definition = existingDefs.FirstOrDefault(x => x.Name == attrRequest.AttributeName);

                    // If the definition doesn't exist yet, create a new one
                    if (definition == null)
                    {
                        definition = new AttributeDefinition(
                            Guid.NewGuid(),
                            attrRequest.AttributeName,
                            attrRequest.AttributeType,
                            userDetails.TenantId);

                        appDbContext.AttributeDefinitions.Add(definition);

                        // Add to the local list to handle potential duplicates within the same request
                        existingDefs.Add(definition);
                    }

                    // Map the specific value to the product and its definition
                    var attributeValue = new ProductAttributeValue(
                        Guid.NewGuid(),
                        product.Id,
                        definition.Id,
                        attrRequest.Value,
                        userDetails.TenantId
                    );

                    appDbContext.ProductAttributeValues.Add(attributeValue);
                }

                // Add the product and commit all changes in a single transaction
                appDbContext.Products.Add(product);
                await appDbContext.SaveChangesAsync(cancellationToken);

                return Result.Success(new Response(product.Id));
            }
        }

    }
}
