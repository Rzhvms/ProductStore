namespace Application.UseCases.Auth.VerifyEmail.Cache;

/// <summary>
/// Запись проверки почты в MemoryCache
/// </summary>
public record EmailVerificationCacheEntry()
{
    /// <summary>
    /// Пинкод
    /// </summary>
    public string PinCode { get; set; }
    
    /// <summary>
    /// Время жизни
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Попытки
    /// </summary>
    public int Attempts { get; set; }
}