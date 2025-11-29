using Application.Exceptions;
using Application.Ports.Repositories;
using Application.Ports.Services.Email;
using Application.UseCases.Auth.ResendPinCode.Request;

namespace Application.UseCases.Auth.ResendPinCode;

/// <summary>
/// Переотправить письмо верификации
/// </summary>
public class ResendPinCodeUseCase : IResendPinCodeUseCase
{
    private readonly IEmailService _emailService;
    private readonly IAuthRepository _authRepository;

    public ResendPinCodeUseCase(IEmailService emailService, IAuthRepository authRepository)
    {
        _emailService = emailService;
        _authRepository = authRepository;
    }
    
    public async Task ExecuteAsync(ResendPinCodeRequest request)
    {
        var user = await _authRepository.GetUserByEmailAsync(request.Email);
        
        if (user == null)
            throw new VerifyEmailException("User not found");
        
        await _emailService.SendVerificationEmail(request.Email);
    }
    
}