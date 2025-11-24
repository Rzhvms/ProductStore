namespace Application.UseCases.UserAddress.Dto.Response;

/// <summary>
/// Выходная модель добавления адреса пользователя
/// </summary>
public record AddUserAddressResponse
{
    /// <summary>
    /// Идентификатор адреса
    /// </summary>
    public Guid AddressId { get; init; }
}