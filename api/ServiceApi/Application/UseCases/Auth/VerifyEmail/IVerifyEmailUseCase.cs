using Application.UseCases.Auth.VerifyEmail.Request;
using Application.UseCases.Auth.VerifyEmail.Response;

namespace Application.UseCases.Auth.VerifyEmail;

/// <summary>
/// Верификация почты 
/// </summary>
public interface IVerifyEmailUseCase
{
    /// <summary>
    /// 
    /// </summary>
    Task<VerifyEmailResponse> ExecuteAsync(VerifyEmailRequest request);
}