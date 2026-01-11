using Application.UseCases.Auth.CreateUser.Response;

namespace Application.UseCases.Auth.Register.Response;

/// <summary>
/// Выходная модель с успешным созданием пользователя
/// </summary>
public record CreateUserSuccessResponse : CreateUserResponse
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; internal set; }
}