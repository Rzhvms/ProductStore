using Application.Extensions;
using Application.UseCases.User.ChangeUserPassword;
using Application.UseCases.User.ChangeUserPassword.Request;
using Application.UseCases.User.ChangeUserPassword.Response;
using Application.UseCases.User.GetUserInfo;
using Application.UseCases.User.GetUserInfo.Request;
using Application.UseCases.User.GetUserInfo.Response;
using Application.UseCases.User.UpdateUserInfo;
using Application.UseCases.User.UpdateUserInfo.Request;
using Application.UseCases.User.UpdateUserInfo.Response;
using Application.UseCases.UserAddress;
using Application.UseCases.UserAddress.Dto.Request;
using Application.UseCases.UserAddress.Dto.Response;
using Application.UseCases.UserProfile;
using Application.UseCases.UserProfile.Dto.Request;
using Application.UseCases.UserProfile.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserController;

/// <summary>
/// Контроллер по изменению данных пользователя со стороны админа
/// </summary>
[Authorize]
[Route("api/admin/user-info")]
[ApiController]
public class UserAdminController : ControllerBase
{
    private readonly IGetUserInfoUseCase _getUserInfoUseCase;
    private readonly IUpdateUserInfoUseCase _updateUserInfoUseCase;
    private readonly IChangeUserPasswordUseCase _changeUserPasswordUseCase;
    private readonly IUserProfileUseCase _userProfileUseCase;
    private IUserAddressUseCase _userAddressUseCase;
    
    public UserAdminController(
        IGetUserInfoUseCase getUserInfoUseCase,
        IUpdateUserInfoUseCase updateUserInfoUseCase,
        IChangeUserPasswordUseCase changeUserPasswordUseCase,
        IUserProfileUseCase userProfileUseCase,
        IUserAddressUseCase userAddressUseCase)
    {
        _getUserInfoUseCase = getUserInfoUseCase;
        _updateUserInfoUseCase = updateUserInfoUseCase;
        _changeUserPasswordUseCase = changeUserPasswordUseCase;
        _userProfileUseCase = userProfileUseCase;
        _userAddressUseCase = userAddressUseCase;
    }

    /// <summary>
    /// Получить базовую информацию о пользователе (UserModel)
    /// </summary>
    [HttpGet("base-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetUserInfoResponse> GetUserInfoAsync(Guid id)
    {
        return await _getUserInfoUseCase.ExecuteAsync(new GetUserInfoRequest { Id = id });
    }

    /// <summary>
    /// Изменить базовую информацию о пользователе (UserModel) (Phone, Name, LastName)
    /// </summary>
    [HttpPatch("base-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<UpdateUserInfoResponse> UpdateUserInfoAsync(Guid id, UpdateUserInfoRequest request)
    {
        return await _updateUserInfoUseCase.ExecuteAsync(id, request);
    }

    /// <summary>
    /// Изменить пароль
    /// </summary>
    [HttpPatch("base-info/password")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ChangeUserPasswordResponse> ChangeUserPasswordAsync(Guid id, ChangeUserPasswordRequest request)
    {
        return await _changeUserPasswordUseCase.ExecuteAsync(id, request);
    }
    
    /// <summary>
    /// Получить профиль пользователя
    /// </summary>
    [HttpGet("profile-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetUserProfileResponse> GetUserProfileAsync(Guid userId)
    {
        return await _userProfileUseCase.GetUserProfileAsync(userId);
    }
    
    /// <summary>
    /// Обновить профиль пользователя.
    /// </summary>
    [HttpPatch("profile-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<UpdateUserProfileResponse> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
    {
        return await _userProfileUseCase.UpdateUserProfileAsync(userId, request);
    }
    
    /// <summary>
    /// Получить адрес пользователя
    /// </summary>
    [HttpGet("address-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetUserAddressResponse> GetUserAddressAsync(Guid userId)
    {
        return await _userAddressUseCase.GetUserAddressAsync(userId);
    }

    /// <summary>
    /// Добавить новый адрес пользователя
    /// </summary>
    [HttpPost("address-info")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<AddUserAddressResponse> AddUserAddressAsync(Guid userId, AddUserAddressRequest request)
    {
        return await _userAddressUseCase.AddUserAddressAsync(userId, request);
    }

    /// <summary>
    /// Обновить существующий адрес пользователя по идентификатору
    /// </summary>
    [HttpPatch("address-info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<UpdateUserAddressResponse> UpdateUserAddressAsync(Guid userId, UpdateUserAddressRequest request)
    {
        return await _userAddressUseCase.UpdateUserAddressAsync(userId, request);
    }

    /// <summary>
    /// Удалить адрес пользователя по идентификатору
    /// </summary>
    [HttpDelete("address-info")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<DeleteUserAddressResponse> DeleteUserAddressAsync(Guid userId)
    {
        return await _userAddressUseCase.DeleteUserAddressAsync(userId);
    }
}