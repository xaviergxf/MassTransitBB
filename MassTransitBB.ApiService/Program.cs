using Azure.Messaging.ServiceBus;
using MassTransit;
using MassTransit.Configuration;
using MassTransitBB.ApiService;
using MassTransitBB.ApiService.Database;
using MassTransitBB.ApiService.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
//builder.AddAzureServiceBusClient("messaging");
builder.AddRabbitMQClient("messaging");

builder.AddSqlServerDbContext<OrderDbContext>("OrderDb");

builder.Services.Configure<MassTransitHostOptions>(options =>
{
    //options.WaitUntilStarted = true;
});
builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(includeNamespace: false));
    x.AddConsumers(typeof(Program).Assembly);
    x.AddEntityFrameworkOutbox<OrderDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);

        o.UseSqlServer();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        var configService = context.GetRequiredService<IConfiguration>();
        var connectionString = configService.GetConnectionString("messaging");
        if(connectionString is not null)
            cfg.Host(connectionString);

        cfg.ConfigureEndpoints(context);
    });
    x.AddSagaStateMachine<OrderSaga, OrderState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.AddDbContext<DbContext, OrderDbContext>((provider, builder) =>
            {
                builder.UseSqlServer();
            });
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Servers = [];
    });
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<OrderDbContext>().Database.EnsureDeletedAsync();
    await scope.ServiceProvider.GetRequiredService<OrderDbContext>().Database.MigrateAsync();
}


app.MapPost("/orders", async (OrderDto order, OrderDbContext orderDbContext, [FromServices] IPublishEndpoint publishEndpoint, CancellationToken cancellationToken) =>
{
    var orderTracked = orderDbContext.Orders.Add(new Order(order.ConfirmationEmail, order.OrderItems.Select(s => new OrderItem(s.ProductId, s.Quantity)),
        new PaymentInfo(order.PaymentInfo.CardNumber, order.PaymentInfo.ExpiryDate, order.PaymentInfo.CVV)));
    await publishEndpoint.Publish(new OrderSubmittedEvent(orderTracked.Entity.Id), cancellationToken);
    await orderDbContext.SaveChangesAsync(cancellationToken);
})
.WithName("CreateOrder");

app.MapDefaultEndpoints();

app.Run();