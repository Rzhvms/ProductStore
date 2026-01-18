using System.Text.Json.Serialization;
using Domain.ExtensionModels;

namespace Application.UseCases.User.GetUserList.Response;

/// <summary>
/// Выходная модель получения списка всех порльзователей
/// </summary>
public record GetUserListResponse
{
    [JsonPropertyName("userList")]
    public List<UserExtensionModel> UserList { get; init; }
}