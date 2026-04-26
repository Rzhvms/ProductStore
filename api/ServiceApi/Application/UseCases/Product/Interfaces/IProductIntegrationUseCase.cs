using Domain.Product;

namespace Application.UseCases.Product.Interfaces;

/// <summary>
/// UseCase для экспорта и импорта продуктов
/// </summary>
public interface IProductIntegrationUseCase
{
    /// <summary>
    /// Экспортировать товар по идентификатору.
    /// </summary>ъ
    Task<ExportJsonFile> ExportProductByIdAsync(Guid id);

    /// <summary>
    /// Экспортировать все товары.
    /// </summary>
    Task<ExportJsonFile> ExportProductListAsync();

    /// <summary>
    /// Импортировать товар.
    /// </summary>
    Task ImportProductAsync(Stream jsonStream);

    /// <summary>
    /// Импортировать список товаров.
    /// </summary>
    Task ImportProductListAsync(Stream jsonStream);
}