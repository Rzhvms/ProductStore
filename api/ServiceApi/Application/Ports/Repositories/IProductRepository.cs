using Domain.ExtensionModels;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий по управлению продуктами
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Создать продукт
    /// </summary>
    Task<Guid> CreateProductAsync(ResultProductModel model);

    /// <summary>
    /// Получить продукт
    /// </summary>
    Task<ResultProductModel?> GetProductAsync(Guid id);
    
    /// <summary>
    /// Получить список продуктов
    /// </summary>
    Task<IEnumerable<ResultProductModel>> GetProductListAsync(int pageNumber, int pageSize);
    
    /// <summary>
    /// Получить список продуктов
    /// </summary>
    Task<IEnumerable<ResultProductModel>> GetProductListByCategoryIdAsync(Guid categoryId, int pageNumber, int pageSize);
    
    /// <summary>
    /// Изменить продукт
    /// </summary>
    Task UpdateProductAsync(Guid id, ResultProductModel model);
    
    /// <summary>
    /// Удалить продукт
    /// </summary>
    Task DeleteProductAsync(Guid id);
}