using CosmosDbPOC.Infrastructure.Database;
using CosmosDbPOC.Infrastructure.Database.Models;

namespace CosmosDbPOC.Infrastructure.Repositories;

public interface ICosmosDbRepository
{
    Task SaveAsync(ProductModel product, CancellationToken cancellationToken);
    Task<ProductModel?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<ProductModel>> GetAllAsync(string category, CancellationToken cancellationToken);
}

internal class CosmosDbRepository(ICosmosContext context) : ICosmosDbRepository
{
    public async Task SaveAsync(ProductModel product, CancellationToken cancellationToken)
    {
        await context.UpsertItemAsync(product, cancellationToken);
    }

    public async Task<ProductModel?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await context.GetItemAsync<ProductModel>(id, id, cancellationToken);
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync(string category, CancellationToken cancellationToken)
    {
        return await context.GetAllAsync<ProductModel>(
            "SELECT * FROM p WHERE p.Category = @category",
            "@category",
            category,
            cancellationToken);
    }
}
