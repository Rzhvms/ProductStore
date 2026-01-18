using Application.Extensions;
using Application.UseCases.Order;
using Application.UseCases.Order.Request;
using Application.UseCases.Order.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.OrderController;

/// <summary>
/// Контроллер по управлению заказами
/// </summary>
[Authorize]
[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderUseCase _orderUseCase;

    public OrderController(IOrderUseCase orderUseCase)
    {
        _orderUseCase = orderUseCase;
    }
    
    /// <summary>
    /// Получить заказ по айди
    /// </summary>
    [HttpGet("{id}")]
    public async Task<GetOrderByIdResponse> GetOrderByIdAsync([FromRoute] Guid id)
    {
        return await _orderUseCase.GetOrderByIdAsync(id);
    }
    
    /// <summary>
    /// Получить список заказов
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetOrderListResponse> GetOrderListAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return await _orderUseCase.GetOrderListAsync(pageNumber, pageSize);
    }

    /// <summary>
    /// Оформить заказ
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var userId = User.GetUserId();
        return await _orderUseCase.CreateOrderAsync(userId);
    }
    
    /// <summary>
    /// Изменить статус заказа
    /// </summary>
    [HttpPatch("{id}")]
    public async Task ChangeOrderStatusAsync([FromRoute] Guid id, ChangeOrderStatusRequest request)
    {
        await _orderUseCase.ChangeOrderStatusAsync(id, request);
    }
}