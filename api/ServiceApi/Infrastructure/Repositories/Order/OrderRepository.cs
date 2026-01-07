using System.Data;
using Application.Exceptions;
using Application.Ports.Repositories;
using Dapper;
using Domain.ExtensionModels;
using Domain.Order;
using Domain.Product;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Order;

/// <inheritdoc/>
internal class OrderRepository : IOrderRepository
{
    private readonly IDbConnection _connection;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    /// <inheritdoc/>
    public async Task<OrderModelExtension?> GetOrderByIdAsync(Guid orderId)
    {
        var getOrderSql = $@"SELECT opm.""{nameof(OrderProductModel.OrderId)}"",
                                    om.""{nameof(OrderModel.CustomerId)}"",
                                    om.""{nameof(OrderModel.State)}"",
                                    om.""{nameof(OrderModel.DeliveryId)}"",
                                    om.""{nameof(OrderModel.TotalSum)}"",
                                    om.""{nameof(OrderModel.OrderDate)}"",
                                    json_agg(
                                        json_build_object(
                                            'ProductId', opm.""{nameof(OrderProductModel.ProductId)}"",
                                            'Quantity', opm.""{nameof(OrderProductModel.Quantity)}""
                                        )
                                    ) as ""ProductListInOrder""
                                FROM ""{nameof(OrderModel)}"" om
                                JOIN ""{nameof(OrderProductModel)}"" opm ON
                                om.""{nameof(OrderModel.Id)}"" = opm.""{nameof(OrderProductModel.OrderId)}""
                                WHERE om.""{nameof(OrderModel.Id)}"" = @Id
                                GROUP BY opm.""{nameof(OrderProductModel.OrderId)}"",
                                    om.""{nameof(OrderModel.CustomerId)}"",
                                    om.""{nameof(OrderModel.State)}"",
                                    om.""{nameof(OrderModel.DeliveryId)}"",
                                    om.""{nameof(OrderModel.TotalSum)}"",
                                    om.""{nameof(OrderModel.OrderDate)}""";
        
        var order = await _connection.QueryFirstOrDefaultAsync<OrderModelExtension>(getOrderSql, new
        {
            Id = orderId
        });
        return order;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrderModel>> GetOrderListAsync(int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        var getOrderInfoSql = $@"SELECT * FROM ""{nameof(OrderModel)}""
                     ORDER BY ""{nameof(OrderModel.OrderDate)}"" DESC
                     OFFSET @Offset LIMIT @PageSize";
        
        var orderList = await _connection.QueryAsync<OrderModel>(getOrderInfoSql, new {offset, pageSize});
        return orderList;
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateOrderAsync(OrderModelExtension orderModel)
    {
        var transaction = _connection.BeginTransaction();
        try
        {
            var orderId = await InsertOrderAsync(orderModel, transaction);
            
            await InsertOrderProductsAsync(orderId, orderModel, transaction);
            await DecreaseProductQuantitiesAsync(orderModel, transaction);
            
            transaction.Commit();
            
            return orderId;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e.Message, "An error occurred when placing an order");
            throw new InvalidOrderException("An error occured", e);
        }
    }

    /// <inheritdoc/>
    public async Task ChangeOrderStatusAsync(Guid orderId, OrderStateEnum orderState)
    {
        var sql = $@"UPDATE ""{nameof(OrderModel)}""
                    SET ""{nameof(OrderModel.State)}"" = @State
                    WHERE ""{nameof(OrderModel.Id)}"" = @Id";

        await _connection.ExecuteAsync(sql, new
        {
            Id = orderId,
            State = orderState
        });
    }

    /// <summary>
    /// Создание записи о заказе в таблице OrderModel
    /// </summary>
    private async Task<Guid> InsertOrderAsync(OrderModelExtension orderModel, IDbTransaction transaction)
    {
        var insertOrderModelSql = $@"INSERT INTO ""{nameof(OrderModel)}""
                VALUES (@Id, @CustomerId, @State, @DeliveryId, @TotalSum, @OrderDate)";
        
        var orderId = Guid.NewGuid();
        await _connection.ExecuteAsync(insertOrderModelSql, new
        {
            Id = orderId,
            CustomerId = orderModel.CustomerId,
            State = orderModel.State,
            DeliveryId = orderModel.DeliveryId,
            TotalSum = orderModel.TotalSum,
            OrderDate = DateTime.UtcNow
        }, transaction);
        
        return orderId;
    }

    /// <summary>
    /// Добавляем запись в таблицу связку OrderProductModel
    /// </summary>
    private async Task InsertOrderProductsAsync(Guid orderId, OrderModelExtension orderModel, IDbTransaction transaction)
    {
        var insertOrderProductSql = $@"INSERT INTO ""{nameof(OrderProductModel)}"" 
                VALUES (@Id, @OrderId, @ProductId, @Quantity)";
        
        var orderProducts = orderModel.ProductIdWithQuantityList!
            .Select(p => new
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = p.ProductId,
                Quantity = p.Quantity
            });
        
        await _connection.ExecuteAsync(insertOrderProductSql, orderProducts, transaction);
    }

    /// <summary>
    /// Уменьшаем значение Quantity у продуктов
    /// </summary>
    private async Task DecreaseProductQuantitiesAsync(OrderModelExtension orderModel, IDbTransaction transaction)
    {
        var updateProductQuantitySql = $@"
                UPDATE ""{nameof(ProductModel)}""
                SET ""{nameof(ProductModel.Quantity)}"" = 
                    ""{nameof(ProductModel.Quantity)}"" - @Quantity
                WHERE ""{nameof(ProductModel.Id)}"" = @ProductId
                AND ""{nameof(ProductModel.Quantity)}"" >= @Quantity";

        foreach (var item in orderModel.ProductIdWithQuantityList!)
        {
            var affectedRows = await _connection.ExecuteAsync(updateProductQuantitySql, new 
                { 
                    ProductId = item.ProductId, 
                    Quantity = item.Quantity
                },
                transaction
            );

            if (affectedRows == 0)
            {
                _logger.LogInformation("Not enough products for {ProductId} in store.", item.ProductId);
                throw new InvalidOrderException($"Not enough products in store. ProductId={item.ProductId}");
            }
        }
    }

}