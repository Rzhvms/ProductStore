namespace Application.UseCases.Auth.VerifyEmail.Request;

/// <summary>
/// Запрос на верификацию
/// </summary>
public record VerifyEmailRequest()
{
    /// <summary>
    /// Почта 
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Пинкод 
    /// </summary>
    public required string Pin { get; init; }
}