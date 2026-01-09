using Domain.User;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Добавляем таблицу UserModel, ClaimModel и RefreshTokenModel
/// </summary>
[Migration(202511011900)]
public class Date_202511011900_AddTables_UserDal_ClaimDal_RefreshTokenDal : Migration
{
    private readonly string _userTableName = nameof(UserModel);
    private readonly string _claimTableName = nameof(ClaimModel);
    private readonly string _tokenTableName = nameof(RefreshTokenModel);
    
    public override void Up()
    {
        if (!Schema.Table(_userTableName).Exists())
        {
            Create.Table(_userTableName)
                .WithColumn(nameof(UserModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(UserModel.Email)).AsString(50).NotNullable().Unique()
                .WithColumn(nameof(UserModel.Phone)).AsString(20).Nullable()
                .WithColumn(nameof(UserModel.Password)).AsString().NotNullable()
                .WithColumn(nameof(UserModel.Salt)).AsString().NotNullable()
                .WithColumn(nameof(UserModel.Name)).AsString(50).Nullable()
                .WithColumn(nameof(UserModel.LastName)).AsString(50).Nullable()
                .WithColumn(nameof(UserModel.CreateAt)).AsDateTime().NotNullable()
                .WithColumn(nameof(UserModel.UpdateAt)).AsDateTime().Nullable()
                .WithColumn(nameof(UserModel.IsEmailConfirmed)).AsBoolean().NotNullable();
            
            Create.Index("IX_UserModel_Email_Unique").OnTable(_userTableName).OnColumn(nameof(UserModel.Email)).Unique();
        }
        
        if (!Schema.Table(_claimTableName).Exists())
        {
            Create.Table(_claimTableName)
                .WithColumn(nameof(ClaimModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(ClaimModel.UserId)).AsGuid()
                .WithColumn(nameof(ClaimModel.Type)).AsString().Nullable()
                .WithColumn(nameof(ClaimModel.Value)).AsString().Nullable();
        }
        
        if (!Schema.Table(_tokenTableName).Exists())
        {
            Create.Table(_tokenTableName)
                .WithColumn(nameof(RefreshTokenModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(RefreshTokenModel.UserId)).AsGuid()
                .WithColumn(nameof(RefreshTokenModel.Value)).AsString().Nullable()
                .WithColumn(nameof(RefreshTokenModel.Active)).AsBoolean().Nullable()
                .WithColumn(nameof(RefreshTokenModel.ExpirationDate)).AsDateTime().Nullable();
        }

        if (Schema.Table(_claimTableName).Exists() && Schema.Table(_userTableName).Exists() 
                                                   && !Schema.Table(_claimTableName)
                                                       .Constraint("fk_UserDal_ClaimId_ClaimDal").Exists())
        {
            Create.ForeignKey("fk_UserDal_ClaimId_ClaimDal")
                .FromTable(_claimTableName).ForeignColumn(nameof(ClaimModel.UserId))
                .ToTable(_userTableName).PrimaryColumn(nameof(UserModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_tokenTableName).Exists() && Schema.Table(_userTableName).Exists()
                                                   && !Schema.Table(_tokenTableName)
                                                       .Constraint("fk_UserDal_RefreshToken_RefreshTokenDal").Exists())
        {
            Create.ForeignKey("fk_UserDal_RefreshToken_RefreshTokenDal")
                .FromTable(_tokenTableName).ForeignColumn(nameof(RefreshTokenModel.UserId))
                .ToTable(_userTableName).PrimaryColumn(nameof(UserModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_UserDal_ClaimId_ClaimDal").OnTable(_claimTableName);
        Delete.ForeignKey("fk_UserDal_RefreshToken_RefreshTokenDal").OnTable(_tokenTableName);
        
        if (Schema.Table(_userTableName).Exists() && Schema.Table(_userTableName).Index("IX_UserModel_Email_Unique").Exists())
        {
            Delete.Index("IX_UserModel_Email_Unique").OnTable(_userTableName);
        }
        
        if (Schema.Table(_claimTableName).Exists())
        {
            Delete.Table(_claimTableName);
        }
        
        if (Schema.Table(_tokenTableName).Exists())
        {
            Delete.Table(_tokenTableName);
        }
        
        if (Schema.Table(_userTableName).Exists())
        {
            Delete.Table(_userTableName);
        }
    }
}