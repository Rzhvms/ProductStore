using Application.Exceptions.Cart;
using Application.Exceptions.Order;
using Application.Ports.Repositories;
using Application.UseCases.Cart.Dto.Response;
using Domain.Payment;

namespace Application.UseCases.Cart;

/// <inheritdoc/>
public class CartUseCase : ICartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IPaymentRepository _paymentRepository;

    public CartUseCase(ICartRepository cartRepository, IPaymentRepository paymentRepository)
    {
        _cartRepository = cartRepository;
        _paymentRepository = paymentRepository;
    }

    /// <inheritdoc/>
    public async Task<GetCartResponse> GetCartAsync(Guid userId)
    {
        return await _cartRepository.GetCartAsync(userId);
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateItemAsync(Guid userId, CartItemDto item)
    {
        if (item.Quantity <= 0) 
            throw new InvalidCartException("Quantity must be >= 1.");
        
        await _cartRepository.AddOrUpdateItemAsync(userId, item);
    }

    /// <inheritdoc/>
    public async Task RemoveItemAsync(Guid userId, Guid productId)
    {
        await _cartRepository.RemoveItemAsync(userId, productId);   
    }

    /// <inheritdoc/>
    public async Task ClearCartAsync(Guid userId)
    {
        await _cartRepository.ClearCartAsync(userId);   
    }

    /// <inheritdoc/>
    public async Task ClearCartAfterPaymentAsync(Guid userId, Guid paymentId)
    {
        var paymentModel = await _paymentRepository.GetByIdAsync(paymentId);
        
        // Очищаем корзину, только если заказ полностью оплачен
        if (paymentModel?.Status != PaymentStatusEnum.Paid)
        {
            throw new InvalidOrderException("Payment is not paid.");
        }
        await _cartRepository.ClearCartAsync(userId);
    }
}