using Newtonsoft.Json;

namespace CosmosDbPOC.Infrastructure.Database.Models;

public class ProductModel
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
