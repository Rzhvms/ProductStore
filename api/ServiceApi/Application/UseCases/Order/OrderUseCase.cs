using Application.Exceptions.Cart;
using Application.Exceptions.Order;
using Application.Ports.Repositories;
using Application.Ports.Services.Payment;
using Application.UseCases.Order.Request;
using Application.UseCases.Order.Response;
using Domain.ExtensionModels;
using Domain.Order;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Order;

/// <inheritdoc/>
public class OrderUseCase : IOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<OrderUseCase> _logger;
    private readonly IPaymentService _paymentService;
    
    public OrderUseCase(
        IOrderRepository orderRepository, 
        ICartRepository cartRepository,
        ILogger<OrderUseCase> logger,
        IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _logger = logger;
        _paymentService = paymentService;
    }
    
    /// <inheritdoc/>
    public async Task<GetOrderByIdResponse> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        return new GetOrderByIdResponse
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            State = order.State,
            TotalSum = order.TotalSum,
            DeliveryId = order.DeliveryId,
            OrderDate = order.OrderDate,
            ProductListInOrder = order.ProductListInOrder
        };
    }

    /// <inheritdoc/>
    public async Task<GetOrderListResponse> GetOrderListAsync(int pageNumber, int pageSize)
    {
        var orderList = await _orderRepository.GetOrderListAsync(pageNumber, pageSize);
        return new GetOrderListResponse
        {
            OrderList = orderList
        };
    }

    /// <inheritdoc/>
    public async Task<CreateOrderResponse> CreateOrderAsync(Guid userId)
    {
        try
        {
            // Получаем корзину пользователя
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart.Items.Count < 1)
            { 
                _logger.LogError("Incorrect products count in cart");
                throw new InvalidCartException("Invalid cart. You can't place an order");
            }
        
            var totalSum = cart.Items.Sum(i => i.Price * i.Quantity);
            
            // Создаем заказ в БД
            var orderModel = new OrderModelExtension
            {
                CustomerId = userId,
                State = OrderStateEnum.Created,
                TotalSum = totalSum,
                ProductIdWithQuantityList = cart.Items.Select(item => new ProductWithQuantityModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                })
            };
        
            var orderId = await _orderRepository.CreateOrderAsync(orderModel);
            if (orderId == Guid.Empty)
            {
                _logger.LogError("Order was created incorrectly. OrderId is invalid");
                throw new InvalidOrderException("Invalid order.");
            }
            
            // Создаем платеж
            var payment = await _paymentService.CreatePaymentAsync(orderId, userId, totalSum);
            
            // Очищаем корзину
            await _cartRepository.ClearCartAsync(userId);

            return new CreateOrderResponse
            {
                OrderId = orderId,
                PaymentUrl = payment.PaymentUrl
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, "An error occurred when placing an order");
            throw new InvalidOrderException("An error occured", e);
        }
    }

    /// <inheritdoc/>
    public async Task ChangeOrderStatusAsync(Guid id, ChangeOrderStatusRequest request)
    {
        await _orderRepository.ChangeOrderStatusAsync(id, request.State);
    }
}