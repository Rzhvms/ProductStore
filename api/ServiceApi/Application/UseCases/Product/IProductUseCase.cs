using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;

namespace Application.UseCases.Product;

/// <summary>
/// UseCase по управлению продуктами на админ панели
/// </summary>
public interface IProductUseCase
{
    /// <summary>
    /// Создать продукт
    /// </summary>
    Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request);

    /// <summary>
    /// Получить продукт
    /// </summary>
    Task<GetProductResponse> GetProductAsync(Guid id);
    
    /// <summary>
    /// Получить список продуктов
    /// </summary>
    Task<GetProductListResponse> GetProductListAsync(int pageNumber, int pageSize);
    
    /// <summary>
    /// Получить список продуктов
    /// </summary>
    Task<GetProductListResponse> GetProductListByCategoryIdAsync(Guid categoryId, int pageNumber, int pageSize);
    
    /// <summary>
    /// Изменить продукт
    /// </summary>
    Task<UpdateProductResponse> UpdateProductAsync(Guid id, UpdateProductRequest request);
    
    /// <summary>
    /// Удалить продукт
    /// </summary>
    Task<DeleteProductResponse> DeleteProductAsync(Guid id);
}