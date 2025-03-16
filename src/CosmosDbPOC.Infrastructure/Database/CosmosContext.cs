using CosmosDbPOC.Infrastructure.Exceptions;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace CosmosDbPOC.Infrastructure.Database;

public interface ICosmosContext
{
    Task UpsertItemAsync<T>(T item, CancellationToken cancellationToken) where T : class;
    Task<T?> GetItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken) where T : class;
    Task<IEnumerable<T>> GetAllAsync<T>(string queryDefinition, string parameterName, string parameterValue, CancellationToken cancellationToken) where T : class;
}

public class CosmosContext(CosmosClient client) : ICosmosContext
{
    public const string DatabaseId = "CosmosPoc";
    public const string ContainerId = "Products";
    private readonly Microsoft.Azure.Cosmos.Database Database = client.GetDatabase(DatabaseId);

    public async Task UpsertItemAsync<T>(T item, CancellationToken cancellationToken) where T : class
    {
        var SUCCESS = new[] { HttpStatusCode.OK, HttpStatusCode.Created };
        var container = GetContainer();
        var result = await container.UpsertItemAsync(item, cancellationToken: cancellationToken);
        if (!SUCCESS.Contains(result.StatusCode))
            throw new CosmosDbOperationFailedException("Failed to save item.");
    }

    public async Task<T?> GetItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken) where T : class
    {
        var container = GetContainer();
        var result = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey), cancellationToken: cancellationToken);
        return result;
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(string queryDefinition, string parameterName, string parameterValue, CancellationToken cancellationToken) where T : class
    {
        var container = GetContainer();
        var query = container.GetItemQueryIterator<T>(new QueryDefinition(queryDefinition).WithParameter(parameterName, parameterValue));
        var items = new List<T>();

        do
        {
            var result = await query.ReadNextAsync(cancellationToken);
            items.AddRange(result);
        } while (query.HasMoreResults);

        return items;
    }

    private Container GetContainer() => Database.GetContainer(ContainerId);
}
