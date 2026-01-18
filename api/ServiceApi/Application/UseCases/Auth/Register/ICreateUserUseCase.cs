using Application.UseCases.Auth.CreateUser.Response;
using Application.UseCases.Auth.Register.Request;
using Application.UseCases.Auth.Register.Response;

namespace Application.UseCases.Auth.Register;

/// <summary>
/// Интерфейс для Use Case создания нового пользователя.
/// Определяет контракт выполнения операции регистрации.
/// </summary>
public interface ICreateUserUseCase
{
    /// <summary>
    /// Создает нового пользователя, если email ещё не зарегистрирован.
    /// </summary>
    Task<CreateUserResponse> ExecuteAsync(CreateUserRequest request);

    /// <summary>
    /// Завершение регистрации
    /// </summary>
    Task<ContinueRegisterResponse> ContinueRegisterAsync(ContinueRegisterRequest request);
}