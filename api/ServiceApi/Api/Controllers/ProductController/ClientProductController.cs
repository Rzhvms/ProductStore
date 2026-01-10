using Application.UseCases.Product;
using Application.UseCases.Product.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProductController;

/// <summary>
/// Контроллер по получению продуктов на клиентской части
/// </summary>
[ApiController]
[Route("api/product")]
public class ClientProductController : ControllerBase
{
    private readonly IProductUseCase _productUseCase;
    
    public ClientProductController(IProductUseCase productUseCase)
    {
        _productUseCase = productUseCase;
    }
    
    /// <summary>
    /// Получить продукт
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetProductResponse> GetProductAsync([FromRoute] Guid id)
    {
        return await _productUseCase.GetProductAsync(id);
    }

    /// <summary>
    /// Получить список продуктов
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetProductListResponse> GetProductListAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return await _productUseCase.GetProductListAsync(pageNumber, pageSize);
    }
    
    /// <summary>
    /// Получить список продуктов по айди категории
    /// </summary>
    [HttpGet("{categoryId}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetProductListResponse> GetProductListByCategoryIdAsync(
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize,
        [FromRoute] Guid categoryId)
    {
        return await _productUseCase.GetProductListByCategoryIdAsync(categoryId, pageNumber, pageSize);
    }
}