namespace CosmosDbPOC.Api.Products;

public record CreateProduct
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public decimal Value { get; init; }
}
