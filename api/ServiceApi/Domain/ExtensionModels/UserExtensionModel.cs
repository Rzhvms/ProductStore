namespace Domain.ExtensionModels;

/// <summary>
/// Вспомогательная модель с данными пользователя
/// </summary>
public record UserExtensionModel
{
    /// <summary>
    /// Айди пользователя
    /// </summary>
    public Guid? Id { get; init; }
    
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
    /// Почта
    /// </summary>
    public string? Email { get; init; }
    
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? Phone { get; init; }
    
    /// <summary>
    /// Город
    /// </summary>
    public string? City { get; init; }
    
    /// <summary>
    /// Количество заказов
    /// </summary>
    public int OrdersCount { get; init; }
}