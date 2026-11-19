namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Marks append-only entities so persistence can reject updates and deletes.</summary>
internal interface IImmutableEntity;
