using Application.Ports.Repositories;
using Application.UseCases.UserProfile.Dto.Request;
using Application.UseCases.UserProfile.Dto.Response;
using Domain.ExtensionModels;

namespace Application.UseCases.UserProfile;

/// <inheritdoc />
internal class UserProfileUseCase : IUserProfileUseCase
{
    private readonly IUserProfileRepository _repository;
    
    public UserProfileUseCase(IUserProfileRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task<GetUserProfileResponse> GetUserProfileAsync(Guid userId)
    {
        var profile = await _repository.GetUserProfileAsync(userId);
        var response = new GetUserProfileResponse()
        {
            Name = profile.Name,
            LastName = profile.LastName,
            BirthDate = profile.BirthDate,
            Gender = profile.Gender,
            Email = profile.Email,
            Phone = profile.Phone,
            About = profile.About
        };
        return response;
    }

    /// <inheritdoc />
    public async Task<UpdateUserProfileResponse> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
    {
        var model = new GetUserProfileModel()
        {
            Name = request.Name,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Email = request.Email,
            Phone = request.Phone,
            About = request.About
        };
        await _repository.UpdateUserProfileAsync(userId, model);
        return new UpdateUserProfileResponse();
    }
}