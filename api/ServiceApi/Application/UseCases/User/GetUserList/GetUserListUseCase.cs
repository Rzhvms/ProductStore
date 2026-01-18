using Application.Ports.Repositories;
using Application.UseCases.User.GetUserList.Response;

namespace Application.UseCases.User.GetUserList;

/// <inheritdoc/>
internal class GetUserListUseCase : IGetUserListUseCase
{
    private readonly IUserRepository _userRepository;
    
    public GetUserListUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    /// <inheritdoc/>
    public async Task<GetUserListResponse> GetUserListAsync()
    {
        var response = await _userRepository.GetUserListAsync();
        return new GetUserListResponse()
        {
            UserList = response
        };
    }
}