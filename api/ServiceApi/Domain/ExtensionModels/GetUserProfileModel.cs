namespace Domain.ExtensionModels;

/// <summary>
/// Дополнительная модель для получения профиля пользователя
/// </summary>
public record GetUserProfileModel
{
    /// <summary>
    /// Имя
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; init; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? BirthDate { get; init; }
    
    /// <summary>
    /// Пол
    /// </summary>
    public string? Gender { get; init; }
    
    /// <summary>
    /// Почта
    /// </summary>
    public string? Email { get; init; }
    
    /// <summary>
    /// Телефон
    /// </summary>
    public string? Phone { get; init; }
    
    /// <summary>
    /// О себе
    /// </summary>
    public string? About { get; init; }
    
    // /// <summary>
    // /// Путь до аватара
    // /// </summary>
    // public string? AvatarUrl { get; init; }
}