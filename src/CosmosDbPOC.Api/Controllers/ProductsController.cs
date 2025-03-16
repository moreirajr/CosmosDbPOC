using CosmosDbPOC.Api.Products;
using CosmosDbPOC.Infrastructure.Database.Models;
using CosmosDbPOC.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDbPOC.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(ICosmosDbRepository repository) : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> PostAsync([FromBody] CreateProduct request, CancellationToken cancellationToken)
    {
        var newProduct = new ProductModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Value = request.Value
        };

        await repository.SaveAsync(newProduct, cancellationToken);

        return Created(HttpContext.Request.Path, newProduct.Id);
    }

    [HttpGet("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAsync(Guid productId, CancellationToken cancellationToken)
    {
        var result = await repository.GetByIdAsync(productId.ToString(), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllAsync([FromQuery] string category, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(category, cancellationToken);
        return Ok(result);
    }
}
