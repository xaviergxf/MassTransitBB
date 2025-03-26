using MassTransit;
using MassTransitBB.ApiService.Commands;
using MassTransitBB.ApiService.Database;
using MassTransitBB.ApiService.Events;
using Microsoft.EntityFrameworkCore;


public class OrderSubmittedConsumer : IConsumer<OrderSubmittedEvent>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<OrderSubmittedConsumer> logger;

    public OrderSubmittedConsumer(OrderDbContext orderDbContext, ILogger<OrderSubmittedConsumer> logger)
    {
        _orderDbContext = orderDbContext;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
    {
        var message = context.Message;
        var order = await _orderDbContext.Orders.Where(p => p.Id == message.OrderId)
            .Include(p=>p.OrderItems)
            .SingleOrDefaultAsync(context.CancellationToken);
        if (order is null)
            return;
        logger.LogInformation("Processing order");
        await context.Publish(new ProcessCreditCardPayment(order.Id, order.PaymentInfo.CardNumber, order.PaymentInfo.ExpiryDate, order.PaymentInfo.CVV), context.CancellationToken);
        await context.Publish(new ReserveProductInStock(order.OrderItems.Select(s=>s.ProductId).ToArray()), context.CancellationToken);
        await context.Publish(new SendOrderConfirmationEmail(order.Id, order.ConfirmationEmail), context.CancellationToken);

        await _orderDbContext.SaveChangesAsync(context.CancellationToken);
    }
}