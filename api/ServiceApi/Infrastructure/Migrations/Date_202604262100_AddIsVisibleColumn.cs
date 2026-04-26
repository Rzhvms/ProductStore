using Domain.Product;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Добавление колонки IsVisible в таблицы ProductModel и CategoryModel
/// </summary>
[Migration(202604262100)]
public class Date_202604262100_AddIsVisibleColumn : Migration
{
    private readonly string _isVisibleProduct = nameof(ProductModel.IsVisible);
    private readonly string _isVisibleCategory = nameof(CategoryModel.IsVisible);

    
    public override void Up()
    {
        if (!Schema.Table(nameof(ProductModel)).Column(_isVisibleProduct).Exists())
        {
            Alter.Table(nameof(ProductModel))
                .AddColumn(_isVisibleProduct)
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true);
        }

        if (!Schema.Table(nameof(CategoryModel)).Column(_isVisibleCategory).Exists())
        {
            Alter.Table(nameof(CategoryModel))
                .AddColumn(_isVisibleCategory)
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true);
        }
    }
    
    public override void Down()
    {
        if (Schema.Table(nameof(ProductModel)).Column(_isVisibleProduct).Exists())
        {
            Delete.Column(_isVisibleProduct).FromTable(nameof(ProductModel));
        }

        if (Schema.Table(nameof(CategoryModel)).Column(_isVisibleCategory).Exists())
        {
            Delete.Column(_isVisibleCategory).FromTable(nameof(CategoryModel));
        }
    }
}