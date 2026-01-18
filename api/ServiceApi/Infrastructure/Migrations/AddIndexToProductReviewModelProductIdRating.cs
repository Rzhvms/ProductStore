using Domain.Product;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Добавление индекса для таблицы ProductReviewModel
/// </summary>
[Migration(202601100100)]
public class AddIndexToProductReviewModelProductIdRating : Migration
{
    private readonly string _tableName = nameof(ProductReviewModel);
    private readonly string _indexName = "IX_ProductReviewModel_ProductId_Rating";
    private readonly string _productIdColumnName = nameof(ProductReviewModel.ProductId);
    private readonly string _ratingColumnName = nameof(ProductReviewModel.Rating);
    private readonly string _isVisibleColname = nameof(ProductReviewModel.IsVisible);
    
    /// <inheritdoc/>
    public override void Up()
    {
        if (Schema.Table(_tableName).Exists() && !Schema.Table(_tableName).Index(_indexName).Exists())
        {
            var sql = $@"CREATE INDEX IF NOT EXISTS ""{_indexName}""
                ON ""{_tableName}"" (""{_productIdColumnName}"")
                INCLUDE (""{_ratingColumnName}"")
                WHERE ""{_isVisibleColname}"" = TRUE";
            
            Execute.Sql(sql);
        }
    }

    /// <inheritdoc/>
    public override void Down()
    {
        if (Schema.Table(_tableName).Exists() && Schema.Table(_tableName).Index(_indexName).Exists())
        {
            Delete.Index(_indexName).OnTable(_tableName);
        }
    }
}