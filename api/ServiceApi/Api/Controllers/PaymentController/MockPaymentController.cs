using Application.UseCases.Payment;
using Domain.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.PaymentController;

/// <summary>
/// Контроллер-заглушка для оплаты
/// </summary>
[ApiController]
[Route("api/mock-payments")]
public class MockPaymentController : ControllerBase
{
    private readonly IPaymentUseCase _paymentUseCase;

    public MockPaymentController(IPaymentUseCase paymentUseCase)
    { 
        _paymentUseCase = paymentUseCase;   
    }

    /// <summary>
    /// Произвести оплату
    /// </summary>
    [HttpPost("{paymentId}/pay")]
    public async Task<IActionResult> PayAsync([FromRoute] Guid paymentId)
    {
        await _paymentUseCase.PayAsync(paymentId);
        return Ok();
    }

    /// <summary>
    /// Отменить оплату
    /// </summary>
    [HttpPost("{paymentId}/cancel")]
    public async Task<IActionResult> FailAsync([FromRoute] Guid paymentId)
    {
        await _paymentUseCase.CancelAsync(paymentId);
        return Ok();
    }

    /// <summary>
    /// Получить платеж по айди
    /// </summary>
    [HttpGet("{paymentId}")]
    public async Task<PaymentModel?> GetPaymentByIdAsync([FromRoute] Guid paymentId)
    {
        return await _paymentUseCase.GetPaymentByIdAsync(paymentId);
    }
}