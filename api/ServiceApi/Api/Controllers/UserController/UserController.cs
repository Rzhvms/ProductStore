using Application.Extensions;
using Application.UseCases.User.ChangeUserPassword;
using Application.UseCases.User.ChangeUserPassword.Request;
using Application.UseCases.User.ChangeUserPassword.Response;
using Application.UseCases.User.DeleteUser;
using Application.UseCases.User.GetUserInfo;
using Application.UseCases.User.GetUserInfo.Request;
using Application.UseCases.User.GetUserInfo.Response;
using Application.UseCases.User.UpdateUserInfo;
using Application.UseCases.User.UpdateUserInfo.Request;
using Application.UseCases.User.UpdateUserInfo.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserController;

/// <summary>
/// Контроллер по управлению пользовательскими данными
/// </summary>
[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IGetUserInfoUseCase _getUserInfoUseCase;
    private readonly IUpdateUserInfoUseCase _updateUserInfoUseCase;
    private readonly IChangeUserPasswordUseCase _changeUserPasswordUseCase;
    private readonly IDeleteUserUseCase _deleteUserUseCase;
    
    public UserController(
        IGetUserInfoUseCase getUserInfoUseCase,
        IUpdateUserInfoUseCase updateUserInfoUseCase,
        IChangeUserPasswordUseCase changeUserPasswordUseCase,
        IDeleteUserUseCase deleteUserUseCase)
    {
        _getUserInfoUseCase = getUserInfoUseCase;
        _updateUserInfoUseCase = updateUserInfoUseCase;
        _changeUserPasswordUseCase = changeUserPasswordUseCase;
        _deleteUserUseCase = deleteUserUseCase;
    }

    /// <summary>
    /// Получить базовую информацию о пользователе (UserModel)
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetUserInfoResponse> GetUserInfoAsync()
    {
        var id = User.GetUserId();
        return await _getUserInfoUseCase.ExecuteAsync(new GetUserInfoRequest { Id = id });
    }

    /// <summary>
    /// Изменить базовую информацию о пользователе (UserModel) (Phone, Name, LastName)
    /// </summary>
    [HttpPatch("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<UpdateUserInfoResponse> UpdateUserInfoAsync(UpdateUserInfoRequest request)
    {
        var id = User.GetUserId();
        return await _updateUserInfoUseCase.ExecuteAsync(id, request);
    }

    /// <summary>
    /// Изменить пароль
    /// </summary>
    [HttpPatch("me/password")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ChangeUserPasswordResponse> ChangeUserPasswordAsync(ChangeUserPasswordRequest request)
    {
        var id = User.GetUserId();
        return await _changeUserPasswordUseCase.ExecuteAsync(id, request);
    }

    /// <summary>
    /// Удаление пользователя
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeleteUserAsync()
    {
        var id = User.GetUserId();
        await _deleteUserUseCase.ExecuteAsync(id);
    }
}