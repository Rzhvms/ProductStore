using Domain.Product;

namespace Application.Ports.Repositories;

public interface ICategoryRepository
{
    /// <summary>
    /// Создать категорию
    /// </summary>
    Task<Guid> CreateCategoryAsync(string categoryName, Guid? parentId);

    /// <summary>
    /// Получить категорию
    /// </summary>
    Task<CategoryModel?> GetCategoryAsync(Guid categoryId);

    /// <summary>
    /// Получить список категорий
    /// </summary>
    Task<IEnumerable<CategoryModel?>> GetCategoryListAsync();
    
    /// <summary>
    /// Обновить категорию
    /// </summary>
    Task UpdateCategoryAsync(CategoryModel category);

    /// <summary>
    /// Удалить категорию
    /// </summary>
    Task DeleteCategoryAsync(Guid id);
}