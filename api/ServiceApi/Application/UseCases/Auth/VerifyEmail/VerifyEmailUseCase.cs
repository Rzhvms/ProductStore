using Application.Exceptions;
using Application.Ports.Repositories;
using Application.UseCases.Auth.VerifyEmail.Cache;
using Application.UseCases.Auth.VerifyEmail.Request;
using Application.UseCases.Auth.VerifyEmail.Response;
using Microsoft.Extensions.Caching.Memory;

namespace Application.UseCases.Auth.VerifyEmail;

/// <inheritdoc />
public class VerifyEmailUseCase : IVerifyEmailUseCase
{
    private readonly IMemoryCache _memoryCache;
    private readonly IAuthRepository _authRepository;
    
    public VerifyEmailUseCase(IMemoryCache memoryCache, IAuthRepository authRepository)
    {
        _memoryCache = memoryCache;
        _authRepository = authRepository;
    }
    public async Task<VerifyEmailResponse> ExecuteAsync(VerifyEmailRequest request)
    {
        var user = await _authRepository.GetUserByEmailAsync(request.Email);
        
        if (user == null)
            throw new VerifyEmailException("User not found");
        
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


        // Обновляем пользователя
        user.IsEmailConfirmed = true;
        await _authRepository.UpdateUserAsync(user);
        
        // Успешный ввод: удаляем PIN из кэша
        _memoryCache.Remove(cacheKey);

        return new VerifyEmailResponse();
    }

}