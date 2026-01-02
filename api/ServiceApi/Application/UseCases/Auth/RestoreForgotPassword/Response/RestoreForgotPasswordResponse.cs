using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.RestoreForgotPassword.Response;

/// <summary>
/// Ответ на запрос о восстановлении пароля
/// </summary>
public record RestoreForgotPasswordResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; init; }
}