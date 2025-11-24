using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.Register.Request;

/// <summary>
/// Входная модель завершения регистрации
/// </summary>
public record ContinueRegisterRequest : IValidatableObject
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    [JsonPropertyName("userId")]
    [Required]
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Имя
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public required string Name { get; init; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    [JsonPropertyName("lastName")]
    [Required]
    public required string LastName { get; init; }
    
    /// <summary>
    /// Номер телефона
    /// </summary>
    [MaxLength(20)]
    [JsonPropertyName("phone")]
    [Required]
    public required string Phone { get; init; }
    
    /// <summary>
    /// Пол
    /// </summary>
    [JsonPropertyName("gender")]
    [Required]
    public required string Gender { get; init; }
    
    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!IsValidGender(Gender))
        {
            yield return new ValidationResult(
                "Gender must be either 'Male' or 'Female'",
                [nameof(Gender)]);
        }
    }

    /// <summary>
    /// Проверка введенного значения пола
    /// </summary>
    private static bool IsValidGender(string gender)
    {
        return gender.ToLower() is "male" or "female";
    }
}