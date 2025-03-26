namespace MassTransitBB.ApiService.Commands;
public record ProcessCreditCardPayment(Guid OrderId, string CardNumber, string ExpiryDate, string Cvv);