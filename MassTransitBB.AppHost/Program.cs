using Arshid.Aspire.ApiDocs.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var db = ConfigureDatabase(builder);

/*var serviceBus = builder.AddAzureServiceBus("messaging").RunAsEmulator(
    emulator =>
    {
        emulator.WithHostPort(7777);
    });*/

var messaging = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin();

var apiService = builder.AddProject<Projects.MassTransitBB_ApiService>("apiservice")
    .WithScalar()
    .WithOpenApi()
    .WithReference(db).WaitFor(db)
    .WithReference(messaging).WaitFor(messaging);


builder.AddProject<Projects.MassTransitBB_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();

static IResourceBuilder<IResourceWithConnectionString> ConfigureDatabase(IDistributedApplicationBuilder builder)
{
    IResourceBuilder<IResourceWithConnectionString>? fairPlayDbResource;
    if (Convert.ToBoolean(builder.Configuration["UseDatabaseContainer"]))
    {
        var sqlPassword = builder.AddParameter("db-password", secret: true);
        fairPlayDbResource = builder.AddSqlServer("sqldb", password: sqlPassword)
            .WithDataVolume()
            .AddDatabase("OrderDb");
    }
    else
    {
        fairPlayDbResource = builder.AddConnectionString("OrderDb");
    }
    return fairPlayDbResource;
}