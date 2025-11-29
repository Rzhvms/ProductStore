using Application.Exceptions.Base;

namespace Application.Exceptions;

/// <summary>
/// Ошибка верификации пользователя
/// </summary>
public class VerifyEmailException : BaseException
{
    public VerifyEmailException(string message) : base(message) { }
}