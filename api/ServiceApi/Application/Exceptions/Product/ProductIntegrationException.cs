using Application.Exceptions.Base;

namespace Application.Exceptions.Product;

/// <summary>
/// Кастомная ошибка интеграции продуктов
/// </summary>
public class ProductIntegrationException : BaseException
{
    public ProductIntegrationException(string message) : base(message) { }
}