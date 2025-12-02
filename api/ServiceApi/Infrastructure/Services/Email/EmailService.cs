using System.Security.Cryptography;
using Application.Ports.Services.Email;
using Application.UseCases.Auth.VerifyEmail.Cache;
using Infrastructure.Services.Email.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Email;

/// <inheritdoc />
public class EmailService : IEmailService
{
    private readonly IEmailClient _client;
    private readonly IMailHelper _mailHelper;
    private readonly IMemoryCache _memoryCache;

    public EmailService(IEmailClient smtpClient, IMailHelper mailHelper, IMemoryCache memoryCache)
    {
        _client = smtpClient;
        _mailHelper = mailHelper;
        _memoryCache = memoryCache;
    }


    /// <inheritdoc />
    private async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = _mailHelper.CreateMimeMessage(email, subject, message);
        await _client.SendAsync(emailMessage);
    }
    
    /// <summary>
    /// Сгенерировать пинкод и отправить письмо
    /// </summary>
    public async Task SendVerificationEmail(string email)
    {
        var pinCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

        var cacheKey = GetEmailCacheKey(email);
        SetVerificationCacheEntry(cacheKey, new EmailVerificationCacheEntry
        {
            PinCode = pinCode,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            Attempts = 0
        });
        var message = $"Ваш код подтверждения: {pinCode}";
        await SendEmailAsync(email, "Подтверждение регистрации", message);
    }

    /// <inheritdoc />
    public string GetEmailCacheKey(string email)
    {
        return $"EmailVerification:{email}";
    }

    /// <summary>
    /// Получить кэш верификации
    /// </summary>
    private void SetVerificationCacheEntry(string cacheKey, EmailVerificationCacheEntry cacheEntry)
    {
        _memoryCache.Set(cacheKey, cacheEntry, TimeSpan.FromMinutes(5));
    }
}