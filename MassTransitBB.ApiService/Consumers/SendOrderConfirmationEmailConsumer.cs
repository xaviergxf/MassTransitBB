using MassTransit;
using MassTransitBB.ApiService.Commands;
using MassTransitBB.ApiService.Database;

public class SendOrderConfirmationEmailConsumer : IConsumer<SendOrderConfirmationEmail>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<SendOrderConfirmationEmailConsumer> logger;

    public SendOrderConfirmationEmailConsumer(OrderDbContext orderDbContext, ILogger<SendOrderConfirmationEmailConsumer> logger)
    {
        _orderDbContext = orderDbContext;
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<SendOrderConfirmationEmail> context)
    {
        var message = context.Message;
        logger.LogInformation("Sending order confirmation email...");

        return Task.CompletedTask;
    }
}
