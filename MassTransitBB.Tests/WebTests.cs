using Aspire.Hosting;
using MassTransit.Testing;
using Microsoft.AspNetCore.Routing;

namespace MassTransitBB.Tests;
/*
public class WebTests: IAsyncLifetime
{
    private IDistributedApplicationTestingBuilder _appHost;
    private DistributedApplication _app;
    private IServiceProvider _serviceProvider;

    public async Task InitializeAsync()
    {
        _appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.MassTransitBB_AppHost>();
        _appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        _app = await _appHost.BuildAsync();
        _serviceProvider = _app.Services;
    }

    public async Task DisposeAsync()
    {
        await _app.DisposeAsync();
    }


    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        var resourceNotificationService = _app.Services.GetRequiredService<ResourceNotificationService>();
        await _app.StartAsync();

        // Act
        var httpClient = _app.CreateHttpClient("webfrontend");
        await resourceNotificationService.WaitForResourceAsync("webfrontend", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var response = await httpClient.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task OrderSubmitted_should_be_consumed()
    {
        var harness = _serviceProvider.GetRequiredService<ITestHarness>();

        await harness.Start();

        var client = harness.GetRequestClient<SubmitOrder>();

        await client.GetResponse<OrderSubmitted>(new
        {
            OrderId = InVar.Id,
            OrderNumber = "123"
        });

        Assert.True(await harness.Sent.Any<OrderSubmitted>());

        Assert.True(await harness.Consumed.Any<SubmitOrder>());

        var consumerHarness = harness.GetConsumerHarness<SubmitOrderConsumer>();

        Assert.True(await consumerHarness.Consumed.Any<SubmitOrder>());

        // test side effects of the SubmitOrderConsumer here
    }
}*/
