using System.Data;
using Application.Ports.Repositories;
using Dapper;
using Domain.User;

namespace Infrastructure.Repositories.User;

/// <inheritdoc />
internal class UserAddressRepository : IUserAddressRepository
{
    private readonly IDbConnection _dbConnection;

    public UserAddressRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <inheritdoc />
    public async Task<UserAddressModel?> GetUserAddressByUserIdAsync(Guid userId)
    {
        var sql = $@"SELECT * FROM ""{nameof(UserAddressModel)}"" 
                    WHERE ""{nameof(UserAddressModel.UserId)}"" = @UserId";
        
        var result = await _dbConnection.QuerySingleOrDefaultAsync<UserAddressModel>(sql, new { UserId = userId });
        return result;
    }
    
    /// <inheritdoc />
    public async Task<Guid> AddUserAddressAsync(UserAddressModel? model)
    {
        var sql = $@"INSERT INTO ""{nameof(UserAddressModel)}"" VALUES 
                    (@Id, @UserId, @Country, @Region, @City, @Street, @House, @Apartment, @PostalCode, @IsDefault, @Floor, @Entrance)";

        var addressId = Guid.NewGuid();
        await _dbConnection.ExecuteAsync(sql, new
        {
            Id = addressId,
            UserId = model.UserId,
            Country = model.Country,
            Region = model.Region,
            City = model.City,
            Street = model.Street,
            House = model.House,
            Apartment = model.Apartment,
            PostalCode = model.PostalCode,
            IsDefault = model.IsDefault ?? false,
            Floor = model.Floor,
            Entrance = model.Entrance
        });
        
        return addressId;
    }

    /// <inheritdoc />
    public async Task UpdateUserAddressAsync(UserAddressModel? model)
    {
        var sql = $@"UPDATE ""{nameof(UserAddressModel)}""
                    SET ""{nameof(UserAddressModel.Country)}"" = @Country,
                        ""{nameof(UserAddressModel.Region)}"" = @Region,
                        ""{nameof(UserAddressModel.City)}"" = @City,
                        ""{nameof(UserAddressModel.Street)}"" = @Street,
                        ""{nameof(UserAddressModel.House)}"" = @House,
                        ""{nameof(UserAddressModel.Apartment)}"" = @Apartment,
                        ""{nameof(UserAddressModel.PostalCode)}"" = @PostalCode,
                        ""{nameof(UserAddressModel.IsDefault)}"" = @IsDefault,
                        ""{nameof(UserAddressModel.Floor)}"" = @Floor,
                        ""{nameof(UserAddressModel.Entrance)}"" = @Entrance,
                    WHERE ""{nameof(UserAddressModel.UserId)}"" = @UserId";

        var checkIsDefaultSql = $@"SELECT ""{nameof(UserAddressModel.IsDefault)}"" FROM ""{nameof(UserAddressModel)}"" 
                                         WHERE ""{nameof(UserAddressModel.UserId)}"" = @UserId";
        var checkIsDefault = _dbConnection.QuerySingleOrDefault<bool>(checkIsDefaultSql, new { UserId = model.UserId });
        
        await _dbConnection.ExecuteAsync(sql, new
        {
            Country = model.Country,
            Region = model.Region,
            City = model.City,
            Street = model.Street,
            House = model.House,
            Apartment = model.Apartment,
            PostalCode = model.PostalCode,
            IsDefault = checkIsDefault,
            UserId = model.UserId,
            Floor = model.Floor,
            Entrance = model.Entrance
        });
    }

    /// <inheritdoc />
    public async Task DeleteUserAddressAsync(Guid userId)
    {
        var sql = $@"DELETE FROM ""{nameof(UserAddressModel)}"" WHERE ""{nameof(UserAddressModel.UserId)}"" = @UserId";
        await _dbConnection.ExecuteAsync(sql, new { UserId = userId });
    }
}