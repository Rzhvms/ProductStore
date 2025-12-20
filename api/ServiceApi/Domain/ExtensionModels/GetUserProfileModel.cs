namespace Domain.ExtensionModels;

/// <summary>
/// Дополнительная модель для получения профиля пользователя
/// </summary>
public record GetUserProfileModel
{
    /// <summary>
    /// Имя
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? BirthDate { get; set; }
    
    /// <summary>
    /// Пол
    /// </summary>
    public string? Gender { get; set; }
    
    /// <summary>
    /// Почта
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Телефон
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// О себе
    /// </summary>
    public string? About { get; set; }
    
    // /// <summary>
    // /// Путь до аватара
    // /// </summary>
    // public string? AvatarUrl { get; init; }
}