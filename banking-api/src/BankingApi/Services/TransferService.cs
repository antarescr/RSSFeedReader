using BankingApi.Models;
using Microsoft.Extensions.Logging;

namespace BankingApi.Services;

/// <summary>
/// Servicio de transferencias con validaciones de negocio.
/// </summary>
public class TransferService : ITransferService
{
    private readonly IAccountStore _accountStore;
    private readonly ILogger<TransferService> _logger;

    public TransferService(IAccountStore accountStore, ILogger<TransferService> logger)
    {
        _accountStore = accountStore;
        _logger = logger;
    }

    public async Task<TransferResponse> TransferAsync(TransferRequest request)
    {
        var transferId = GenerateTransferId();
        var timestamp = DateTime.UtcNow;

        _logger.LogInformation("Starting transfer {TransferId} from {Source} to {Target} amount {Amount}",
            transferId, request.SourceAccountId, request.TargetAccountId, request.Amount);

        // RB-001: Amount must be positive
        if (request.Amount <= 0)
        {
            var message = "Amount must be greater than 0.";
            _logger.LogWarning("Transfer {TransferId} failed: {Message}", transferId, message);
            return new TransferResponse
            {
                TransferId = transferId,
                Status = "Failed",
                Message = message,
                Timestamp = timestamp
            };
        }

        // RB-003: Different accounts
        if (request.SourceAccountId == request.TargetAccountId)
        {
            var message = "Source and target accounts must be different.";
            _logger.LogWarning("Transfer {TransferId} failed: {Message}", transferId, message);
            return new TransferResponse
            {
                TransferId = transferId,
                Status = "Failed",
                Message = message,
                Timestamp = timestamp
            };
        }

        // RB-004: Both accounts must exist
        var sourceAccount = await _accountStore.GetAccountAsync(request.SourceAccountId);
        if (sourceAccount == null)
        {
            var message = $"Source account {request.SourceAccountId} does not exist.";
            _logger.LogWarning("Transfer {TransferId} failed: {Message}", transferId, message);
            return new TransferResponse
            {
                TransferId = transferId,
                Status = "Failed",
                Message = message,
                Timestamp = timestamp
            };
        }

        var targetAccount = await _accountStore.GetAccountAsync(request.TargetAccountId);
        if (targetAccount == null)
        {
            var message = $"Target account {request.TargetAccountId} does not exist.";
            _logger.LogWarning("Transfer {TransferId} failed: {Message}", transferId, message);
            return new TransferResponse
            {
                TransferId = transferId,
                Status = "Failed",
                Message = message,
                Timestamp = timestamp
            };
        }

        // RB-002: Sufficient funds
        if (sourceAccount.Balance < request.Amount)
        {
            var message = $"Insufficient funds. Available: {sourceAccount.Balance}, Required: {request.Amount}";
            _logger.LogWarning("Transfer {TransferId} failed: {Message}", transferId, message);
            return new TransferResponse
            {
                TransferId = transferId,
                Status = "Failed",
                Message = message,
                Timestamp = timestamp
            };
        }

        // RB-005: Atomic operation
        var newSourceBalance = sourceAccount.Balance - request.Amount;
        var newTargetBalance = targetAccount.Balance + request.Amount;

        await _accountStore.UpdateBalanceAsync(request.SourceAccountId, newSourceBalance);
        await _accountStore.UpdateBalanceAsync(request.TargetAccountId, newTargetBalance);

        var successMessage = $"Transfer completed successfully. From: {request.SourceAccountId}, To: {request.TargetAccountId}, Amount: {request.Amount}";
        _logger.LogInformation("Transfer {TransferId} completed successfully", transferId);

        return new TransferResponse
        {
            TransferId = transferId,
            Status = "Success",
            Message = successMessage,
            Timestamp = timestamp
        };
    }

    private static string GenerateTransferId()
    {
        return $"TRF-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..8]}";
    }
}
