using Application.UseCases.UserProfile.Dto.Request;
using Application.UseCases.UserProfile.Dto.Response;

namespace Application.UseCases.UserProfile;

/// <summary>
/// UseCase по управлению профилем пользователя
/// </summary>
public interface IUserProfileUseCase
{
    /// <summary>
    /// Получить профиль пользователя
    /// </summary>
    Task<GetUserProfileResponse> GetUserProfileAsync(Guid userId);

    /// <summary>
    /// Обновить профиль пользователя
    /// </summary>
    Task<UpdateUserProfileResponse> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request);
}