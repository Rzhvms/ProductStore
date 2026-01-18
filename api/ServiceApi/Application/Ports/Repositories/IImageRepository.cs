using Domain.Image;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для работы с файлами
/// </summary>
public interface IImageRepository
{
    /// <summary>
    /// Добавить изображение
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    Task<Guid> CreateAsync(ImageModel image);
    
    /// <summary>
    /// Получить изображение по его Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ImageModel> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Получить все изображения по Id продукта
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<IEnumerable<ImageModel>> GetAllByProductIdAsync(Guid productId);
    
    /// <summary>
    /// Получить основное фото продукта
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<ImageModel?> GetMainImageAsync(Guid productId);
    
    /// <summary>
    /// Обновить изображение 
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    Task<ImageModel> UpdateAsync(ImageModel image);
    
    /// <summary>
    /// Удалить изображение по его Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
}