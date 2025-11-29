using Application.Ports.Repositories;
using Application.UseCases.UserAddress.Dto.Request;
using Application.UseCases.UserAddress.Dto.Response;
using Domain.User;

namespace Application.UseCases.UserAddress;

/// <inheritdoc />
internal class UserAddressUseCase : IUserAddressUseCase
{
    private readonly IUserAddressRepository _userAddressRepository;

    public UserAddressUseCase(IUserAddressRepository userAddressRepository)
    {
        _userAddressRepository = userAddressRepository;
    }

    public async Task<GetUserAddressResponse> GetUserAddressAsync(Guid userId)
    {
        var response = await _userAddressRepository.GetUserAddressByUserIdAsync(userId);
        if (response == null)
        {
            return new GetUserAddressResponse();
        }

        return new GetUserAddressResponse()
        {
            Id = response.Id,
            UserId = response.UserId,
            Country = response.Country,
            Region = response.Region,
            City = response.City,
            Street = response.Street,
            House = response.House,
            Apartment = response.Apartment,
            PostalCode = response.PostalCode,
            IsDefault = response.IsDefault
        };
    }

    /// <inheritdoc />
    public async Task<AddUserAddressResponse> AddUserAddressAsync(Guid userId, AddUserAddressRequest request)
    {
        var model = new UserAddressModel()
        {
            UserId = userId,
            Country = request.Country,
            Region = request.Region,
            City = request.City,
            Street = request.Street,
            House = request.House,
            Apartment = request.Apartment,
            PostalCode = request.PostalCode,
            IsDefault = request.IsDefault
        };

        var addressId = await _userAddressRepository.AddUserAddressAsync(model);
        return new AddUserAddressResponse()
        {
            AddressId = addressId
        };
    }

    /// <inheritdoc />
    public async Task<UpdateUserAddressResponse> UpdateUserAddressAsync(Guid userId, UpdateUserAddressRequest request)
    {
        var model = new UserAddressModel()
        {
            UserId = userId,
            Country = request.Country,
            Region = request.Region,
            City = request.City,
            Street = request.Street,
            House = request.House,
            Apartment = request.Apartment,
            PostalCode = request.PostalCode,
            IsDefault = request.IsDefault
        };

        await _userAddressRepository.UpdateUserAddressAsync(model);
        return new UpdateUserAddressResponse();
    }

    /// <inheritdoc />
    public async Task<DeleteUserAddressResponse> DeleteUserAddressAsync(Guid userId)
    {
        await _userAddressRepository.DeleteUserAddressAsync(userId);
        return new DeleteUserAddressResponse();
    }
}