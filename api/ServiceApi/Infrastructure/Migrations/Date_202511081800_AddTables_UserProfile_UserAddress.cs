using Domain.User;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Добавляем таблицу UserProfileModel и UserAddressModel
/// </summary>
[Migration(202511081800)]
public class Date_202511081800_AddTables_UserProfile_UserAddress : Migration
{
    private readonly string _profileTbName = nameof(UserProfileModel);
    private readonly string _addressTbName = nameof(UserAddressModel);
    private readonly string _userTbName = nameof(UserModel);
    
    /// <inheritdoc />
    public override void Up()
    {
        if (!Schema.Table(_profileTbName).Exists())
        {
            Create.Table(_profileTbName)
                .WithColumn(nameof(UserProfileModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(UserProfileModel.UserId)).AsGuid().NotNullable().Unique()
                .WithColumn(nameof(UserProfileModel.AvatarUrl)).AsString().Nullable()
                .WithColumn(nameof(UserProfileModel.BirthDate)).AsDateTime().Nullable()
                .WithColumn(nameof(UserProfileModel.Gender)).AsString().Nullable()
                .WithColumn(nameof(UserProfileModel.About)).AsString().Nullable();
        }

        if (!Schema.Table(_addressTbName).Exists())
        {
            Create.Table(_addressTbName)
                .WithColumn(nameof(UserAddressModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(UserAddressModel.UserId)).AsGuid().NotNullable().Unique()
                .WithColumn(nameof(UserAddressModel.Country)).AsString().Nullable()
                .WithColumn(nameof(UserAddressModel.Region)).AsString().Nullable()
                .WithColumn(nameof(UserAddressModel.City)).AsString().Nullable()
                .WithColumn(nameof(UserAddressModel.Street)).AsString().Nullable() 
                .WithColumn(nameof(UserAddressModel.House)).AsString().Nullable()
                .WithColumn(nameof(UserAddressModel.Apartment)).AsString().Nullable()
                .WithColumn(nameof(UserAddressModel.PostalCode)).AsString().Nullable()
                .WithColumn(nameof(UserAddressModel.IsDefault)).AsBoolean().NotNullable();
        }

        if (Schema.Table(_profileTbName).Exists() && Schema.Table(_userTbName).Exists()
                                                  && !Schema.Table(_profileTbName)
                                                      .Constraint("FK_UserProfile_User").Exists())
        {
            Create.ForeignKey("FK_UserProfile_User")
                .FromTable(_profileTbName).ForeignColumn(nameof(UserProfileModel.UserId))
                .ToTable(_userTbName).PrimaryColumn(nameof(UserModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_addressTbName).Exists() && Schema.Table(_userTbName).Exists()
                                                  && !Schema.Table(_addressTbName)
                                                      .Constraint("FK_UserAddress_User").Exists())
        {
            Create.ForeignKey("FK_UserAddress_User")
                .FromTable(_addressTbName).ForeignColumn(nameof(UserAddressModel.UserId))
                .ToTable(_userTbName).PrimaryColumn(nameof(UserModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete.ForeignKey("FK_UserAddress_User").OnTable(_addressTbName);
        Delete.ForeignKey("FK_UserProfile_User").OnTable(_profileTbName);

        if (Schema.Table(_addressTbName).Exists())
        {
            Delete.Table(_addressTbName);
        }

        if (Schema.Table(_profileTbName).Exists())
        {
            Delete.Table(_profileTbName);
        }
    }
}