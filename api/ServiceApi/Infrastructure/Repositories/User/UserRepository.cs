using System.Data;
using Application.Ports.Repositories;
using Dapper;
using Domain.ExtensionModels;
using Domain.Order;
using Domain.User;
using Infrastructure.Repositories.Auth;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.User;

/// <inheritdoc />
internal class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<AuthRepository> _logger;
    
    public UserRepository(IDbConnection dbConnection, ILogger<AuthRepository> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<UserModel> GetUserInfoAsync(Guid id)
    {
        var sql = $@"
                SELECT ""{nameof(UserModel.Email)}"",
                       ""{nameof(UserModel.Phone)}"",
                       ""{nameof(UserModel.Name)}"",
                       ""{nameof(UserModel.LastName)}"",
                       ""{nameof(UserModel.CreateAt)}"",
                       ""{nameof(UserModel.UpdateAt)}""
                FROM ""{nameof(UserModel)}""
                WHERE ""{nameof(UserModel.Id)}"" = @Id";
        
        var response = await _dbConnection.QueryFirstAsync<UserModel>(sql, new { Id = id });
        return response;
    }

    /// <inheritdoc />
    public async Task<(Guid, DateTime)> UpdateUserInfoAsync(Guid id, string? phone, string? name, string? lastName)
    {
        var sql = "";
        var now = DateTime.UtcNow;
        
        if (phone != null)
        {
            sql = $@"
            UPDATE ""{nameof(UserModel)}""
            SET ""{nameof(UserModel.Phone)}"" = @Phone,
                ""{nameof(UserModel.UpdateAt)}"" = @Time
            WHERE ""{nameof(UserModel.Id)}"" = @Id";
        }
        
        if (name != null)
        { 
            sql = $@"
            UPDATE ""{nameof(UserModel)}""
            SET ""{nameof(UserModel.Name)}"" = @Name,
                ""{nameof(UserModel.UpdateAt)}"" = @Time
            WHERE ""{nameof(UserModel.Id)}"" = @Id";
        }
        
        if (lastName != null)
        {
            sql = $@"
            UPDATE ""{nameof(UserModel)}""
            SET ""{nameof(UserModel.LastName)}"" = @LastName,
                ""{nameof(UserModel.UpdateAt)}"" = @Time
            WHERE ""{nameof(UserModel.Id)}"" = @Id";
        }
        
        await _dbConnection.ExecuteAsync(sql, new { Id = id, Phone = phone, Name = name, LastName = lastName, Time = now });
        var response = (id, now);
        return response;
    }
    
    /// <inheritdoc />
    public async Task ChangeUserPasswordAsync(Guid id, string passwordHash, string salt)
    {
        var now = DateTime.UtcNow;
        var sql = $@"
            UPDATE ""{nameof(UserModel)}""
            SET ""{nameof(UserModel.Password)}"" = @PasswordHash,
                ""{nameof(UserModel.Salt)}"" = @Salt,
                ""{nameof(UserModel.UpdateAt)}"" = @Time
            WHERE ""{nameof(UserModel.Id)}"" = @Id";
        
        await _dbConnection.ExecuteAsync(sql, new { Id = id, PasswordHash = passwordHash, Salt = salt, Time = now });
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid id)
    {
        var sql = $@"DELETE FROM ""{nameof(UserModel)}""
                    WHERE ""{nameof(UserModel.Id)}"" = @Id";
        
        await _dbConnection.ExecuteAsync(sql, new { Id = id });
    }

    /// <inheritdoc />
    public async Task<List<UserExtensionModel>> GetUserListAsync()
    {
        var sqlGetUserInfo = $@"SELECT 
                                u.""{nameof(UserModel.Id)}"",
                                u.""{nameof(UserModel.Email)}"",
                                u.""{nameof(UserModel.Name)}"",
                                u.""{nameof(UserModel.LastName)}"",
                                u.""{nameof(UserModel.Phone)}"",
                                p.""{nameof(UserProfileModel.BirthDate)}"",
                                a.""{nameof(UserAddressModel.City)}"",
                                COUNT(o.""{nameof(OrderModel.CustomerId)}"") as ""OrdersCount""
                                FROM ""{nameof(UserModel)}"" u 
                                LEFT JOIN ""{nameof(UserProfileModel)}"" p 
                                    ON u.""{nameof(UserModel.Id)}"" = p.""{nameof(UserProfileModel.UserId)}"" 
                                LEFT JOIN ""{nameof(UserAddressModel)}"" a 
                                    ON u.""{nameof(UserModel.Id)}"" = a.""{nameof(UserAddressModel.UserId)}""
                                LEFT JOIN ""{nameof(OrderModel)}"" o 
                                    ON o.""{nameof(OrderModel.CustomerId)}"" = u.""{nameof(UserModel.Id)}""
                                GROUP BY 
                                    u.""{nameof(UserModel.Id)}"",
                                    u.""{nameof(UserModel.Email)}"",
                                    u.""{nameof(UserModel.Name)}"",
                                    u.""{nameof(UserModel.LastName)}"",
                                    u.""{nameof(UserModel.Phone)}"",
                                    p.""{nameof(UserProfileModel.BirthDate)}"",
                                    a.""{nameof(UserAddressModel.City)}""";

        var response = await _dbConnection.QueryAsync<UserExtensionModel>(sqlGetUserInfo);
        return response.AsList();
    }
}