namespace MassTransitBB.ApiService.Commands;

public record ReserveProductInStock(IEnumerable<Guid> ProductIds);