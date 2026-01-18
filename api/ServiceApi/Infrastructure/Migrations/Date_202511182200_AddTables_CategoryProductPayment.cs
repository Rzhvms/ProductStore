using Domain.Delivery;
using Domain.Image;
using Domain.Order;
using Domain.Payment;
using Domain.Product;
using Domain.Provider;
using Domain.User;
using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(202511182200)]
public class Date_202511182200_AddTables_CategoryProductPayment : Migration
{
    private readonly string _paymentTb = nameof(PaymentModel);
    private readonly string _categoryTb = nameof(CategoryModel);
    private readonly string _productTb = nameof(ProductModel);
    private readonly string _imageTb = nameof(ImageModel);
    private readonly string _productReviewTb = nameof(ProductReviewModel);
    
    /// <inheritdoc />
    public override void Up()
    {
        if (!Schema.Table(_paymentTb).Exists())
        {
            Create.Table(_paymentTb)
                .WithColumn(nameof(PaymentModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(PaymentModel.OrderId)).AsGuid().NotNullable()
                .WithColumn(nameof(PaymentModel.UserId)).AsGuid().NotNullable()
                .WithColumn(nameof(PaymentModel.Amount)).AsDecimal().NotNullable()
                .WithColumn(nameof(PaymentModel.Currency)).AsString().NotNullable()
                .WithColumn(nameof(PaymentModel.PaymentMethod)).AsInt16().NotNullable()
                .WithColumn(nameof(PaymentModel.Status)).AsInt16().NotNullable()
                .WithColumn(nameof(PaymentModel.PaymentSystemId)).AsString().Nullable()
                .WithColumn(nameof(PaymentModel.PaymentUrl)).AsString().Nullable()
                .WithColumn(nameof(PaymentModel.IsRefunded)).AsBoolean().Nullable()
                .WithColumn(nameof(PaymentModel.RefundAmount)).AsDecimal().Nullable()
                .WithColumn(nameof(PaymentModel.CreatedAt)).AsDateTime().NotNullable()
                .WithColumn(nameof(PaymentModel.UpdatedAt)).AsDateTime().Nullable();
        }
        
        if (!Schema.Table(_categoryTb).Exists())
        {
            Create.Table(_categoryTb)
                .WithColumn(nameof(CategoryModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(CategoryModel.Name)).AsString(100).NotNullable()
                .WithColumn(nameof(CategoryModel.ParentId)).AsGuid().Nullable();
        }
        
        if (!Schema.Table(_imageTb).Exists())
        {
            Create.Table(_imageTb)
                .WithColumn(nameof(ImageModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(ImageModel.ProductId)).AsGuid().NotNullable()
                .WithColumn(nameof(ImageModel.ObjectPath)).AsString().NotNullable()
                .WithColumn(nameof(ImageModel.IsMain)).AsBoolean().NotNullable();
            
            Create.ForeignKey($"fk_{_imageTb}_product")
                .FromTable(_imageTb).ForeignColumn(nameof(ImageModel.ProductId))
                .ToTable(_productTb).PrimaryColumn("Id")
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.Index($"ix_{_imageTb}_productid")
                .OnTable(_imageTb)
                .OnColumn(nameof(ImageModel.ProductId))
                .Ascending();
            Execute.Sql(
                $@"CREATE UNIQUE INDEX ux_{_imageTb}_main 
                ON ""{_imageTb}"" (""ProductId"") WHERE ""IsMain"" = true;");

        }
        
        if (!Schema.Table(_productReviewTb).Exists())
        {
            Create.Table(_productReviewTb)
                .WithColumn(nameof(ProductReviewModel.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(ProductReviewModel.UserId)).AsGuid().NotNullable()
                .WithColumn(nameof(ProductReviewModel.ProductId)).AsGuid().NotNullable()
                .WithColumn(nameof(ProductReviewModel.Rating)).AsInt32().NotNullable()
                .WithColumn(nameof(ProductReviewModel.Message)).AsString(300).Nullable()
                .WithColumn(nameof(ProductReviewModel.CreatedAt)).AsDateTime().NotNullable()
                .WithColumn(nameof(ProductReviewModel.IsVisible)).AsBoolean().WithDefaultValue(true).NotNullable();
        }

        if (Schema.Table(_paymentTb).Exists() && Schema.Table(nameof(OrderModel)).Exists()
                                              && !Schema.Table(_paymentTb)
                                                  .Constraint("FK_Payment_Order").Exists())
        {
            Create.ForeignKey("FK_Payment_Order")
                .FromTable(_paymentTb).ForeignColumn(nameof(PaymentModel.OrderId))
                .ToTable(nameof(OrderModel)).PrimaryColumn(nameof(OrderModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_paymentTb).Exists() && Schema.Table(nameof(UserModel)).Exists()
                                              && !Schema.Table(_paymentTb)
                                                  .Constraint("FK_Payment_User").Exists())
        {
            Create.ForeignKey("FK_Payment_User")
                .FromTable(_paymentTb).ForeignColumn(nameof(PaymentModel.UserId))
                .ToTable(nameof(UserModel)).PrimaryColumn(nameof(UserModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);   
        }

        if (Schema.Table(_categoryTb).Exists() && !Schema.Table(_categoryTb)
                .Constraint("FK_CategoryParent_CategoryId").Exists())
        {
            Create.ForeignKey("FK_CategoryParent_CategoryId")
                .FromTable(_categoryTb).ForeignColumn(nameof(CategoryModel.ParentId))
                .ToTable(_categoryTb).PrimaryColumn(nameof(CategoryModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_imageTb).Exists() && Schema.Table(nameof(ProductModel)).Exists()
                                                   && !Schema.Table(_imageTb)
                                                       .Constraint("FK_ProductImage_Product").Exists())
        {
            Create.ForeignKey("FK_ProductImage_Product")
                .FromTable(_imageTb).ForeignColumn(nameof(ImageModel.ProductId))
                .ToTable(nameof(ProductModel)).PrimaryColumn(nameof(ProductModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_productReviewTb).Exists() && Schema.Table(nameof(UserModel)).Exists()
                                                    && !Schema.Table(_productReviewTb)
                                                        .Constraint("FK_ProductReview_User").Exists())
        {
            Create.ForeignKey("FK_ProductReview_User")
                .FromTable(_productReviewTb).ForeignColumn(nameof(ProductReviewModel.UserId))
                .ToTable(nameof(UserModel)).PrimaryColumn(nameof(UserModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_productReviewTb).Exists() && Schema.Table(nameof(ProductModel)).Exists()
                                                    && !Schema.Table(_productReviewTb)
                                                        .Constraint("FK_ProductReview_Product").Exists())
        {
            Create.ForeignKey("FK_ProductReview_Product")
                .FromTable(_productReviewTb).ForeignColumn(nameof(ProductReviewModel.ProductId))
                .ToTable(nameof(ProductModel)).PrimaryColumn(nameof(ProductModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }

        if (Schema.Table(_categoryTb).Exists() && Schema.Table(nameof(ProductModel)).Exists()
                                               && !Schema.Table(nameof(ProductModel))
                                                   .Constraint("FK_Product_Category").Exists())
        {
            Create.ForeignKey("FK_Product_Category")
                .FromTable(nameof(ProductModel)).ForeignColumn(nameof(ProductModel.CategoryId))
                .ToTable(_categoryTb).PrimaryColumn(nameof(CategoryModel.Id))
                .OnDeleteOrUpdate(System.Data.Rule.Cascade);
        }
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete.ForeignKey("FK_ProductReview_Product").OnTable(_productReviewTb);
        Delete.ForeignKey("FK_ProductReview_User").OnTable(_productReviewTb);
        Delete.ForeignKey("FK_ProductImage_Product").OnTable(_imageTb);
        Delete.ForeignKey("FK_CategoryParent_CategoryId").OnTable(_categoryTb);
        Delete.ForeignKey("FK_Payment_User").OnTable(_paymentTb);
        Delete.ForeignKey("FK_Payment_Order").OnTable(_paymentTb);
        
        if (Schema.Table(_productReviewTb).Exists())
        {
            Delete.Table(_productReviewTb);
        }
        
        if (Schema.Table(_imageTb).Exists())
        {
            Delete.Table(_imageTb);
        }
        
        if (Schema.Table(_categoryTb).Exists())
        {
            Delete.Table(_categoryTb);
        }
        
        if (Schema.Table(_paymentTb).Exists())
        {
            Delete.Table(_paymentTb);
        }
    }
}