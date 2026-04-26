using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProductController;

/// <summary>
/// Контроллер для импорта и экспорта товаров
/// </summary>
[ApiController]
[Route("api/integration/product")]
public class ProductIntegrationController : ControllerBase
{
    private readonly IProductIntegrationUseCase _productIntegrationUseCase;

    public ProductIntegrationController(IProductIntegrationUseCase productIntegrationUseCase)
    {
        _productIntegrationUseCase = productIntegrationUseCase;
    }

    /// <summary>
    /// Экспортировать товар по идентификатору.
    /// </summary>
    [HttpGet("{id:guid}/export")]
    public async Task<IActionResult> ExportProductByIdAsync([FromRoute] Guid id)
    {
        var file = await _productIntegrationUseCase.ExportProductByIdAsync(id);
        return File(file.Content, "application/json", file.FileName);
    }

    /// <summary>
    /// Экспортировать все товары.
    /// </summary>
    [HttpGet("list/export")]
    public async Task<IActionResult> ExportProductListAsync()
    {
        var file = await _productIntegrationUseCase.ExportProductListAsync();
        return File(file.Content, "application/json", file.FileName);
    }

    /// <summary>
    /// Импортировать товар.
    /// </summary>
    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportProductAsync([FromForm] ImportProductRequest request)
    {
        await using var stream = request.File.OpenReadStream();
        await _productIntegrationUseCase.ImportProductAsync(stream);
        return Ok();
    }

    /// <summary>
    /// Импортировать список товаров.
    /// </summary>
    [HttpPost("list/import")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportProductListAsync([FromForm] ImportProductRequest request)
    {
        await using var stream = request.File.OpenReadStream();
        await _productIntegrationUseCase.ImportProductListAsync(stream);
        return Ok();
    }
}