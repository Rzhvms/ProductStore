using Application.UseCases.Category;
using Application.UseCases.Category.Dto.Request;
using Application.UseCases.Category.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.CategoryController;

/// <summary>
/// Категории
/// </summary>
[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryUseCase _categoryUseCase;

    public CategoryController(ICategoryUseCase categoryUseCase)
    {
        _categoryUseCase = categoryUseCase;
    }

    /// <summary>
    /// Создать категорию
    /// </summary>
    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<CreateCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var response = await _categoryUseCase.CreateCategoryAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Получить категорию
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetCategoryResponse>> GetCategoryAsync([FromRoute] Guid id)
    {
        var response = await _categoryUseCase.GetCategoryAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Получить список категорий
    /// </summary>
    [HttpGet("list")]
    public async Task<List<GetCategoryResponse>> GetCategoryListAsync()
    {
        var response = (await _categoryUseCase.GetCategoryListAsync()).ToList();
        return response;
    }

    /// <summary>
    /// Обновить категорию
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateCategoryAsync(UpdateCategoryRequest request, [FromRoute] Guid id)
    {
        await _categoryUseCase.UpdateCategoryAsync(request, id);
        return Ok();
    }

    /// <summary>
    /// Удалить категорию
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategoryAsync([FromRoute] Guid id)
    {
        await _categoryUseCase.DeleteCategoryAsync(id);
        return Ok();
    }
}