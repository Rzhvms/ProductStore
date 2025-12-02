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
}