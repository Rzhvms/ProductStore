namespace Application.UseCases.User.DeleteUser;

/// <summary>
/// Use Case удаления пользователя
/// </summary>
public interface IDeleteUserUseCase
{
    /// <summary>
    /// Выполнение логики удаления пользователя
    /// </summary>
    Task ExecuteAsync(Guid id);
}