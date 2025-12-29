using Application.Exceptions;
using Application.Ports.Repositories;
using Application.Ports.Services;
using Application.UseCases.Auth.RestoreForgotPassword.Response;
using Application.UseCases.Auth.VerifyEmail.Cache;
using Application.UseCases.Auth.VerifyEmail.Request;
using Domain.User;
using Microsoft.Extensions.Caching.Memory;

namespace Application.UseCases.Auth.RestoreForgotPassword;

/// <inheritdoc/>
internal class RestoreForgotPasswordUseCase : IRestoreForgotPasswordUseCase
{
    private readonly IMemoryCache _memoryCache;
    private readonly IAuthRepository _authRepository;
    private readonly IAuthTokenService _authTokenService;
    
    public RestoreForgotPasswordUseCase(
        IMemoryCache memoryCache, 
        IAuthRepository authRepository,
        IAuthTokenService authTokenService)
    {
        _memoryCache = memoryCache;
        _authRepository = authRepository;
        _authTokenService = authTokenService;
    }

    /// <inheritdoc />
    public async Task<RestoreForgotPasswordResponse> ExecuteAsync(VerifyEmailRequest request)
    {
        var user = await _authRepository.GetUserByEmailAsync(request.Email);
        
        if (user == null)
            throw new VerifyEmailException("User not found");
        
        if (!user.IsEmailConfirmed)
            throw new VerifyEmailException("Email not confirmed");
        
        var cacheKey = $"EmailVerification:{request.Email}";

        if (!_memoryCache.TryGetValue<EmailVerificationCacheEntry>(cacheKey, out var entry))
            throw new VerifyEmailException("PIN not found or expired");
        
        if (entry.ExpiresAt < DateTime.UtcNow)
            throw new VerifyEmailException("PIN expired");

        if (request.Pin != entry.PinCode)
        {
            entry.Attempts++;

            // можно ограничить попытки
            if (entry.Attempts >= 5)
            {
                _memoryCache.Remove(cacheKey);
                throw new VerifyEmailException("Too many attempts");
            }

            throw new VerifyEmailException("Invalid PIN");
        }

        var accessToken = await _authTokenService.GenerateAccessToken(user, isRestoringPassword: true);
        
        // Успешный ввод: удаляем PIN из кэша
        _memoryCache.Remove(cacheKey);

        return new RestoreForgotPasswordResponse
        {
            AccessToken = accessToken,
        };
    }
    
    /// <summary>
    /// Создаёт новый refresh token с параметрами из AuthTokenService.
    /// </summary>
    private async Task<RefreshTokenModel> CreateRefreshTokenAsync()
    {
        var lifetimeMinutes = await _authTokenService.GetRefreshTokenLifetimeInMinutes();
        return new RefreshTokenModel
        {
            Value = await _authTokenService.GenerateRefreshToken(),
            Active = true,
            ExpirationDate = DateTime.UtcNow.AddMinutes(lifetimeMinutes)
        };
    }
}