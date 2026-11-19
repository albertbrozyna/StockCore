using System.Reflection;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Infrastructure.Persistence.Conventions;

internal static class StronglyTypedIdScanner
{
    public static IEnumerable<Type> FindIdTypes(Assembly assembly) =>
        assembly.GetTypes().Where(t => t is { IsValueType: true, IsAbstract: false } && typeof(IStronglyTypedId<Guid>).IsAssignableFrom(t));
}
