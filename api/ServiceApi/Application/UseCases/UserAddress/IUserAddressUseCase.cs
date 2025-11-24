using Application.UseCases.UserAddress.Dto.Request;
using Application.UseCases.UserAddress.Dto.Response;

namespace Application.UseCases.UserAddress;

/// <summary>
/// UseCase для управления адресом пользователя
/// </summary>
public interface IUserAddressUseCase
{
    /// <summary>
    /// Получить адрес пользователя
    /// </summary>
    Task<GetUserAddressResponse> GetUserAddressAsync(Guid userId);
    
    /// <summary>
    /// Добавить адрес пользователя
    /// </summary>
    Task<AddUserAddressResponse> AddUserAddressAsync(Guid userId, AddUserAddressRequest request);

    /// <summary>
    /// Обновить существующий адрес пользователя
    /// </summary>
    Task<UpdateUserAddressResponse> UpdateUserAddressAsync(Guid userId, UpdateUserAddressRequest request);

    /// <summary>
    /// Удалить адрес пользователя
    /// </summary>
    Task<DeleteUserAddressResponse> DeleteUserAddressAsync(Guid userId);
}