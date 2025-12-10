using System.Data;
using Application.Ports.Repositories;
using Dapper;
using Domain.ExtensionModels;
using Domain.User;

namespace Infrastructure.Repositories.User;

/// <inheritdoc />
internal class UserProfileRepository : IUserProfileRepository
{
    private IDbConnection _dbConnection;
    
    public UserProfileRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    /// <inheritdoc />
    public async Task<GetUserProfileModel> GetUserProfileAsync(Guid userId)
    {
        var sql = $@"SELECT u.""{nameof(UserModel.Name)}"", u.""{nameof(UserModel.LastName)}"", u.""{nameof(UserModel.Phone)}"", u.""{nameof(UserModel.Email)}"",
                    up.""{nameof(UserProfileModel.Gender)}"", up.""{nameof(UserProfileModel.BirthDate)}"", up.""{nameof(UserProfileModel.About)}""
                    FROM ""{nameof(UserModel)}"" u
                    LEFT JOIN ""{nameof(UserProfileModel)}"" up ON u.""{nameof(UserModel.Id)}"" = up.""{nameof(UserProfileModel.UserId)}""
                    WHERE u.""{nameof(UserModel.Id)}"" = @Id";
        
        var res = await _dbConnection.QuerySingleOrDefaultAsync<GetUserProfileModel>(sql, new { Id = userId });
        return res;
    }
    
    /// <inheritdoc />
    public async Task UpdateUserProfileAsync(Guid id, GetUserProfileModel model)
    {
        var current = await GetUserProfileAsync(id);
        if (current == null)
            throw new Exception($"User {id} not found");
        
        current.Name = model.Name ?? current.Name;
        current.LastName = model.LastName ?? current.LastName;
        current.Phone = model.Phone ?? current.Phone;
        current.Email = model.Email ?? current.Email;

        current.Gender = model.Gender ?? current.Gender;
        current.BirthDate = model.BirthDate ?? current.BirthDate;
        current.About = model.About ?? current.About;
        
        var sqlUser = $@"UPDATE ""{nameof(UserModel)}""
                        SET ""{nameof(UserModel.Name)}"" = @Name, 
                            ""{nameof(UserModel.LastName)}"" = @LastName, 
                            ""{nameof(UserModel.Phone)}"" = @Phone, 
                            ""{nameof(UserModel.Email)}"" = @Email
                        WHERE ""{nameof(UserModel.Id)}"" = @Id";

        await _dbConnection.ExecuteAsync(sqlUser, new
        {
            Id = id,
            current.Name,
            current.LastName,
            current.Phone,
            current.Email
        });
        
        var sqlUserProfile = $@"UPDATE ""{nameof(UserProfileModel)}""
                            SET ""{nameof(UserProfileModel.Gender)}"" = @Gender, 
                                ""{nameof(UserProfileModel.BirthDate)}"" = @BirthDate, 
                                ""{nameof(UserProfileModel.About)}"" = @About
                            WHERE ""{nameof(UserProfileModel.UserId)}"" = @Id";
        
        await _dbConnection.ExecuteAsync(sqlUserProfile, new
        {
            Id = id,
            current.Gender,
            current.BirthDate,
            current.About
        });
    }
}