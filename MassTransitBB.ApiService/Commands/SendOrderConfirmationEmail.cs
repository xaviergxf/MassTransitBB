namespace MassTransitBB.ApiService.Commands;

public record SendOrderConfirmationEmail(Guid OrderId, string Email);