using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleCreatedEvent(Guid SaleId, string SaleNumber) : INotification;
public record SaleModifiedEvent(Guid SaleId, string SaleNumber) : INotification;
public record SaleCancelledEvent(Guid SaleId, string SaleNumber) : INotification;
public record ItemCancelledEvent(Guid SaleId, Guid ItemId) : INotification;