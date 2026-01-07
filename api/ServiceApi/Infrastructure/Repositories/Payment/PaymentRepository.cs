using System.Data;
using Application.Ports.Repositories;
using Dapper;
using Domain.Payment;

namespace Infrastructure.Repositories.Payment;

/// <inheritdoc/>
public class PaymentRepository : IPaymentRepository
{
    private readonly IDbConnection _connection;

    public PaymentRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public Task CreateAsync(PaymentModel payment)
    {
        var sql = $@"INSERT INTO ""{nameof(PaymentModel)}"" VALUES
                     (@Id, @OrderId, @UserId, @Amount, @Currency, @PaymentMethod,
                      @Status, @PaymentSystemId, @PaymentUrl, @IsRefunded, 
                      @RefundAmount, @CreatedAt, @UpdatedAt)";

        return _connection.ExecuteAsync(sql, new
        {
            payment.Id,
            payment.OrderId,
            payment.UserId,
            payment.Amount,
            payment.Currency,
            PaymentMethod = (int)payment.PaymentMethod,
            Status = (int)payment.Status,
            payment.PaymentSystemId,
            payment.PaymentUrl,
            payment.IsRefunded,
            payment.RefundAmount,
            payment.CreatedAt,
            payment.UpdatedAt
        });
    }

    /// <inheritdoc/>
    public Task UpdateStatusAsync(Guid paymentId, PaymentStatusEnum status)
    {
        var sql = $@"UPDATE ""{nameof(PaymentModel)}""
                    SET ""{nameof(PaymentModel.Status)}"" = @Status,
                        ""{nameof(PaymentModel.UpdatedAt)}"" = @UpdatedAt
                    WHERE ""{nameof(PaymentModel.Id)}"" = @Id";

        var updatedAt = DateTime.UtcNow;
        return _connection.ExecuteAsync(sql, new
        {
            Id = paymentId,
            Status = (int)status,
            UpdatedAt = updatedAt
        });
    }

    /// <inheritdoc/>
    public Task<PaymentModel?> GetByIdAsync(Guid paymentId)
    {
        var sql = $@"SELECT * FROM ""{nameof(PaymentModel)}""
                    WHERE ""{nameof(PaymentModel.Id)}"" = @Id";
        
        return _connection.QueryFirstOrDefaultAsync<PaymentModel>(sql, new { Id = paymentId });
    }
}
