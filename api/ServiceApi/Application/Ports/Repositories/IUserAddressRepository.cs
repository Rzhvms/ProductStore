using Domain.User;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий по управлению адресом пользователя
/// </summary>
public interface IUserAddressRepository
{
    /// <summary>
    /// Получить адрес пользователя
    /// </summary>
    Task<UserAddressModel?> GetUserAddressByUserIdAsync(Guid userId);
    
    /// <summary>
    /// Добавить адрес пользователя
    /// </summary>
    Task<Guid> AddUserAddressAsync(UserAddressModel? model);
    
    /// <summary>
    /// Обновить существующий адрес пользователя
    /// </summary>
    Task UpdateUserAddressAsync(UserAddressModel? model);

    /// <summary>
    /// Удалить адрес пользователя
    /// </summary>
    Task DeleteUserAddressAsync(Guid userId);
}