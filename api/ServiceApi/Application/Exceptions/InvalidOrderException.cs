using Application.Exceptions.Base;

namespace Application.Exceptions;

/// <summary>
/// Общая ошибка, связанная с валидацией и обработкой заказа
/// </summary>
public class InvalidOrderException : BaseException
{
    public InvalidOrderException(string message) : base(message) { }
    public InvalidOrderException(string message, Exception innerException) : base(message, innerException) { }
}