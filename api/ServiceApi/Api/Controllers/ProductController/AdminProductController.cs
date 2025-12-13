using Application.UseCases.Product;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProductController;

/// <summary>
/// Контроллер по управлению продуктами на админ панели
/// </summary>
[ApiController]
[Route("api/admin-product")]
[Authorize]
public class AdminProductController : ControllerBase
{
    private readonly IProductUseCase _productUseCase;
    
    public AdminProductController(IProductUseCase productUseCase)
    {
        _productUseCase = productUseCase;
    }
    
    /// <summary>
    /// Создать продукт
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateProductResponse>> CreateProductAsync(CreateProductRequest request)
    {
        return await _productUseCase.CreateProductAsync(request);
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
    public async Task<GetProductListResponse> GetProductListAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return await _productUseCase.GetProductListAsync(pageNumber, pageSize);
    }
    
    /// <summary>
    /// Получить список продуктов по айди категории
    /// </summary>
    [HttpGet("{categoryId}/list")]
    public async Task<GetProductListResponse> GetProductListByCategoryIdAsync(
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize,
        [FromRoute] Guid categoryId)
    {
        return await _productUseCase.GetProductListByCategoryIdAsync(categoryId, pageNumber, pageSize);
    }

    /// <summary>
    /// Изменить продукт
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<UpdateProductResponse> UpdateProductAsync([FromRoute] Guid id, UpdateProductRequest request)
    {
        return await _productUseCase.UpdateProductAsync(id, request);
    }
    
    /// <summary>
    /// Удалить продукт
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<DeleteProductResponse> DeleteProductAsync([FromRoute] Guid id)
    {
        return await _productUseCase.DeleteProductAsync(id);
    }
}