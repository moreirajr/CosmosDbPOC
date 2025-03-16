using CosmosDbPOC.Infrastructure.Database;
using CosmosDbPOC.Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CosmosDbPOC.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["CosmosDb:ConnectionString"];
        var acccountKey = configuration["CosmosDb:AccountKey"];

        var options = new CosmosClientOptions
        {
            ApplicationName = "CosmosDbPOC.Api",
            HttpClientFactory = () => new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }),
            ConnectionMode = ConnectionMode.Gateway
        };

        var cosmoClient = new CosmosClient(connectionString, acccountKey, options);

        services.AddSingleton(cosmoClient);
        services.AddSingleton<ICosmosContext, CosmosContext>();
        services.AddSingleton<ICosmosDbRepository, CosmosDbRepository>();

        return services;
    }
}
