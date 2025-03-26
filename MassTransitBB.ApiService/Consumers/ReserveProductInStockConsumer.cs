using MassTransit;
using MassTransitBB.ApiService.Commands;
using MassTransitBB.ApiService.Database;

public class ReserveProductInStockConsumer : IConsumer<ReserveProductInStock>
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<ReserveProductInStockConsumer> logger;

    public ReserveProductInStockConsumer(OrderDbContext orderDbContext, ILogger<ReserveProductInStockConsumer> logger)
    {
        _orderDbContext = orderDbContext;
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<ReserveProductInStock> context)
    {
        var message = context.Message;
        logger.LogInformation("Reserving product in stock...");

        return Task.CompletedTask;
    }
}
