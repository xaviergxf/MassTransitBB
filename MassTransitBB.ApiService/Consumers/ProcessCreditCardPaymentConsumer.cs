using MassTransit;
using MassTransitBB.ApiService.Commands;
using MassTransitBB.ApiService.Database;
using MassTransitBB.ApiService.Events;

public class ProcessCreditCardPaymentConsumer : IConsumer<ProcessCreditCardPayment>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<ProcessCreditCardPaymentConsumer> logger;

    public ProcessCreditCardPaymentConsumer(OrderDbContext orderDbContext, ILogger<ProcessCreditCardPaymentConsumer> logger)
    {
        _orderDbContext = orderDbContext;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessCreditCardPayment> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing credit card payment...");
        await context.Publish(new OrderPayedEvent(message.OrderId), context.CancellationToken);
    }
}
