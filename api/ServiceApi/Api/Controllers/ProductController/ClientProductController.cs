using Application.UseCases.Image.Dto;
using Application.UseCases.Image.Interfaces;
using Application.UseCases.Product.Dto.Response;
using Application.UseCases.Product.Interfaces;
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
    private readonly IImageUseCase _imageUseCase;
    
    public ClientProductController(IProductUseCase productUseCase, IImageUseCase imageUseCase)
    {
        _productUseCase = productUseCase;
        _imageUseCase = imageUseCase;
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

    [HttpGet("{id}/images")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<ProductImageResponse>> GetProductImagesByProductId([FromRoute] Guid id)
    {
        return await _imageUseCase.GetProductImagesAsync(id);
    }
}