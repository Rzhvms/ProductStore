using Application.UseCases.Category.Dto.Request;
using Application.UseCases.Category.Dto.Response;

namespace Application.UseCases.Category;

/// <summary>
/// UseCase категорий
/// </summary>
public interface ICategoryUseCase
{
    /// <summary>
    /// Создать категорию
    /// </summary>
    Task<CreateCategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);

    /// <summary>
    /// Получить категорию
    /// </summary>
    Task<GetCategoryResponse> GetCategoryAsync(Guid id);

    /// <summary>
    /// Получить список категорий
    /// </summary>
    Task<List<GetCategoryResponse>> GetCategoryListAsync();
    
    /// <summary>
    /// Обновить категорию
    /// </summary>
    Task UpdateCategoryAsync(UpdateCategoryRequest request, Guid id);

    /// <summary>
    /// Удалить категорию
    /// </summary>
    Task DeleteCategoryAsync(Guid id);
}