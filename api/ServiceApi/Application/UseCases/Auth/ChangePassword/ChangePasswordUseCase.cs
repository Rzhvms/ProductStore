using Application.Ports.Repositories;
using Application.Ports.Services;
using Application.UseCases.Auth.ChangePassword.Request;

namespace Application.UseCases.Auth.ChangePassword;

/// <inheritdoc/>
internal class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptographyService _cryptoService;
    private readonly IAuthRepository _authRepository;
    
    public ChangePasswordUseCase(
        IUserRepository userRepository,
        ICryptographyService cryptoService,
        IAuthRepository authRepository)
    {
        _userRepository = userRepository;
        _cryptoService = cryptoService;
        _authRepository = authRepository;
    }
    
    /// <inheritdoc />
    public async Task ExecuteAsync(Guid id, ChangePasswordRequest request)
    {
        var user = await _authRepository.GetUserByUserIdAsync(id);
        // Хэшируем новый пароль
        var newPasswordHash = _cryptoService.HashPassword(request.Password, user!.Salt);

        await _userRepository.ChangeUserPasswordAsync(user.Id, newPasswordHash, user.Salt);
    }
}