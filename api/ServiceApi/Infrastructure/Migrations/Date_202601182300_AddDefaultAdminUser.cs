using System.Collections;
using Application.Ports.Repositories;
using Application.Ports.Services;
using Application.UseCases.Auth.Register.Request;
using Domain.User;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Добавление дефолтного администратора
/// </summary>
[Migration(202601182300)]
public class Date_202601182300_AddDefaultAdminUser : Migration
{
    private readonly IAuthRepository _authRepository;
    private readonly ICryptographyService _cryptographyService;

    public Date_202601182300_AddDefaultAdminUser(IAuthRepository authRepository, ICryptographyService cryptographyService)
    {
        _authRepository = authRepository;
        _cryptographyService = cryptographyService;
    }
    
    /// <inheritdoc/>
    public override void Up()
    {
        CreateUser().GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public override void Down()
    {
        // нету
    }

    private async Task CreateUser()
    {
        var salt = _cryptographyService.GenerateSalt();
        var now = DateTime.UtcNow;
        
        var claims = new List<Claim>
        {
            new Claim { Type = "role", Value = "admin" },
        };

        var userId = Guid.NewGuid();
        var user = new UserModel
        {
            Id = userId,
            Email = "admin@admin.admin",
            Password = _cryptographyService.HashPassword("defaultAdmin123", salt),
            Salt = salt,
            Claims = ToClaims(claims),
            CreateAt = now,
            UpdateAt = now
        };

        await _authRepository.CreateUserAsync(user);
        await _authRepository.UpdateUserForFinalRegistrationAsync(userId, "admin", "admin",
            "male", "89000000000", now);
    }
    
    /// <summary>
    /// Преобразует список claim из запроса в доменные объекты.
    /// </summary>
    private static IList<ClaimModel> ToClaims(IList<Claim>? requestClaims)
    {
        if (requestClaims == null || requestClaims.Count == 0)
            return new List<ClaimModel>();

        return requestClaims
            .Select(c => new ClaimModel
            {
                Type = c.Type,
                Value = c.Value
            })
            .ToList();
    }
}