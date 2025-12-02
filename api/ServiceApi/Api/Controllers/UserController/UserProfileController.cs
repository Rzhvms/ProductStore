using Application.Extensions;
using Application.UseCases.UserProfile;
using Application.UseCases.UserProfile.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserController;

/// <summary>
/// Контроллер по управлению пользовательскими данными (Профиль)
/// </summary>
[Authorize]
[ApiController]
[Route("api/user/profile")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileUseCase _userProfileUseCase;
    public UserProfileController(IUserProfileUseCase userProfileUseCase)
    {
        _userProfileUseCase = userProfileUseCase;
    }
    
    /// <summary>
    /// Получить профиль пользователя
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetUserProfileResponse> GetUserProfileAsync()
    {
        var userId = User.GetUserId();
        return await _userProfileUseCase.GetUserProfileAsync(userId);
    }
    
    /// <summary>
    /// Обновить профиль пользователя.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task UpdateUserProfileAsync()
    {
        
    }
}