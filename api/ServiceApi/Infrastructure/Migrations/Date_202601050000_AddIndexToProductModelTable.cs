using Domain.Product;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Добавление составного индекса для таблицы ProductModel
/// (Id, CategoryId)
/// </summary>
[Migration(202601050000)]
public class Date_202601050000_AddIndexToProductModelTable : Migration
{
    private readonly string _tableName = nameof(ProductModel);
    private readonly string _idColumnName = nameof(ProductModel.Id);
    private readonly string _categoryIdColumnName = nameof(ProductModel.CategoryId);
    private readonly string _indexName = "IX_ProductModel_Id_CategoryId";
    
    public override void Up()
    {
        if (Schema.Table(_tableName).Exists() && !Schema.Table(_tableName).Index(_indexName).Exists())
        {
            Create.Index(_indexName)
                .OnTable(_tableName)
                .OnColumn(_idColumnName).Ascending()
                .OnColumn(_categoryIdColumnName).Ascending();
        }
    }

    public override void Down()
    {
        if (Schema.Table(_tableName).Exists() && Schema.Table(_tableName).Index(_indexName).Exists())
        {
            Delete.Index(_indexName).OnTable(_tableName);
        }
    }
}