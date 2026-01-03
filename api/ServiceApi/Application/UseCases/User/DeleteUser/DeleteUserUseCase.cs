using Application.Ports.Repositories;

namespace Application.UseCases.User.DeleteUser;

/// <inheritdoc/>
internal class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserRepository _userRepository;
    
    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    /// <inheritdoc/>
    public async Task ExecuteAsync(Guid id)
    {
        await _userRepository.DeleteUserAsync(id);
    }
}