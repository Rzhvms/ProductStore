using Application.Exceptions.Base;

namespace Application.Exceptions.User;

/// <summary>
/// Кастомные ошибки при создании пользователя
/// </summary>
public class UpdateUserException : BaseException
{
    public UpdateUserException(string message) : base(message) { }
    public UpdateUserException(string message, Exception innerException) : base(message, innerException) { }
}