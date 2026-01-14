using Application.UseCases.Image.Interfaces;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;
using Application.UseCases.Product.Interfaces;
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
    private readonly IImageUseCase _imageUseCase;
    
    public AdminProductController(IProductUseCase productUseCase, IImageUseCase imageUseCase)
    {
        _productUseCase = productUseCase;
        _imageUseCase = imageUseCase;
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
    public async Task<GetAdminProductResponse> GetProductAsync([FromRoute] Guid id)
    {
        return await _productUseCase.GetAdminProductAsync(id);
    }
    /// <summary>
    /// Получить список продуктов
    /// </summary>
    [HttpGet("list")]
    public async Task<GetAdminProductListResponse> GetProductListAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return await _productUseCase.GetAdminProductListAsync(pageNumber, pageSize);
    }
    
    /// <summary>
    /// Получить список продуктов по айди категории
    /// </summary>
    [HttpGet("{categoryId}/list")]
    public async Task<GetAdminProductListResponse> GetProductListByCategoryIdAsync(
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize,
        [FromRoute] Guid categoryId)
    {
        return await _productUseCase.GetAdminProductListByCategoryIdAsync(categoryId, pageNumber, pageSize);
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
    
    
    /// <summary>
    /// Загрузить картинку продукта
    /// </summary>
    [HttpPost("{id}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadProductImageAsync(
        [FromRoute] Guid id,
        IFormFile file, 
        bool isMain = true)
    {
        await _imageUseCase.AddProductImageAsync(id, file, isMain);
        return Ok();
    }
    
    /// <summary>
    /// Удалить картинку продукта
    /// </summary>
    [HttpDelete("{id}/images/{imageId}")]
    public async Task<IActionResult> DeleteProductImageAsync(
        Guid id,
        Guid imageId)
    {
        await _imageUseCase.DeleteProductImageAsync(id, imageId);
        return Ok();
    }
}