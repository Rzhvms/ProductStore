using Domain.ExtensionModels;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для управления профилем пользователя
/// </summary>
public interface IUserProfileRepository
{
    /// <summary>
    /// Получить информацию о профиле пользователя
    /// </summary>
    Task<GetUserProfileModel> GetUserProfileAsync(Guid userId);

    /// <summary>
    /// Обновить информацию о пользователе
    /// </summary>
    Task UpdateUserProfileAsync(Guid id, GetUserProfileModel model);
}