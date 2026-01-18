using Application.UseCases.Image.Dto;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Image.Interfaces;

/// <summary>
/// Юзкейс для работы с изображениями
/// </summary>
public interface IImageUseCase
{
    /// <summary>
    /// Добавить файл к продукту
    /// </summary>
    Task AddProductImageAsync(Guid productId, IFormFile file, bool isMain);

    /// <summary>
    /// Удалить файл продукта
    /// </summary>
    Task DeleteProductImageAsync(Guid productId, Guid imageId);

    /// <summary>
    /// Получить файлы привязанные к продукту
    /// </summary>
    Task<IEnumerable<ProductImageResponse>> GetProductImagesAsync(Guid productId);
}