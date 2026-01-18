using Application.UseCases.User.GetUserList.Response;

namespace Application.UseCases.User.GetUserList;

/// <summary>
/// Получение списка пользователей
/// </summary>
public interface IGetUserListUseCase
{
    /// <summary>
    /// Получить список пользователей
    /// </summary>
    Task<GetUserListResponse> GetUserListAsync();
}