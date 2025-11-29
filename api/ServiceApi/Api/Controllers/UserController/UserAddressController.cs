using Application.Extensions;
using Application.UseCases.UserAddress;
using Application.UseCases.UserAddress.Dto.Request;
using Application.UseCases.UserAddress.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserController;

/// <summary>
/// Контроллер по управлению пользовательскими данными (Адрес)
/// </summary>
[Authorize]
[ApiController]
[Route("api/user/address")]
public class UserAddressController : ControllerBase
{
    private IUserAddressUseCase _userAddressUseCase;
    
    public UserAddressController(IUserAddressUseCase userAddressUseCase)
    {
        _userAddressUseCase = userAddressUseCase;
    }
    
    /// <summary>
    /// Получить адрес пользователя
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetUserAddressResponse> GetUserAddressAsync()
    {
        var userId = User.GetUserId();
        return await _userAddressUseCase.GetUserAddressAsync(userId);
    }

    /// <summary>
    /// Добавить новый адрес пользователя
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<AddUserAddressResponse> AddUserAddressAsync(AddUserAddressRequest request)
    {
        var userId = User.GetUserId();
        return await _userAddressUseCase.AddUserAddressAsync(userId, request);
    }

    /// <summary>
    /// Обновить существующий адрес пользователя по идентификатору
    /// </summary>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<UpdateUserAddressResponse> UpdateUserAddressAsync(UpdateUserAddressRequest request)
    {
        var userId = User.GetUserId();
        return await _userAddressUseCase.UpdateUserAddressAsync(userId, request);
    }

    /// <summary>
    /// Удалить адрес пользователя по идентификатору
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<DeleteUserAddressResponse> DeleteUserAddressAsync()
    {
        var userId = User.GetUserId();
        return await _userAddressUseCase.DeleteUserAddressAsync(userId);
    }
}