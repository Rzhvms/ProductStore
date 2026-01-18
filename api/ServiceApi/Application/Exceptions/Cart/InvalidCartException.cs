using Application.Exceptions.Base;

namespace Application.Exceptions.Cart;

/// <summary>
/// Ошибка в корзине
/// </summary>
public class InvalidCartException : BaseException
{
    public InvalidCartException(string message) : base(message) { }
}