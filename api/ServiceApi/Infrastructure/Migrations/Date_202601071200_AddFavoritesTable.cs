using Domain.Product;
using Domain.User;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Добавление таблицы избранного
/// </summary>
[Migration(202601071200)]
public class Date_202601071200_AddFavoritesTable : Migration
{
    private readonly string _tbName = nameof(FavoriteProductsModel);
    
    /// <inheritdoc/>
    public override void Up()
    {
        if (!Schema.Table(_tbName).Exists())
        {
            Create.Table(_tbName)
                .WithColumn(nameof(FavoriteProductsModel.Id)).AsGuid().PrimaryKey().NotNullable()
                .WithColumn(nameof(FavoriteProductsModel.ProductId)).AsGuid().NotNullable()
                .WithColumn(nameof(FavoriteProductsModel.UserId)).AsGuid().NotNullable();
        }

        if (Schema.Table(_tbName).Exists() && Schema.Table(nameof(ProductModel)).Exists()
                                           && !Schema.Table(_tbName)
                                               .Constraint("FK_FavoriteProducts_Product").Exists())
        {
            Create.ForeignKey("FK_FavoriteProducts_Product")
                .FromTable(_tbName).ForeignColumn(nameof(FavoriteProductsModel.ProductId))
                .ToTable(nameof(ProductModel)).PrimaryColumn(nameof(ProductModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_tbName).Exists() && Schema.Table(nameof(UserModel)).Exists()
                                           && !Schema.Table(_tbName)
                                               .Constraint("FK_FavoriteProducts_User").Exists())
        {
            Create.ForeignKey("FK_FavoriteProducts_User")
                .FromTable(_tbName).ForeignColumn(nameof(FavoriteProductsModel.UserId))
                .ToTable(nameof(UserModel)).PrimaryColumn(nameof(UserModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }

    /// <inheritdoc/>
    public override void Down()
    {
        Delete.ForeignKey("FK_FavoriteProducts_Product").OnTable(_tbName);
        Delete.ForeignKey("FK_FavoriteProducts_User").OnTable(_tbName);
        
        if (Schema.Table(_tbName).Exists())
        {
            Delete.Table(_tbName);
        }
    }
}