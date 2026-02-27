using Microsoft.AspNetCore.Mvc;
using BankingApi.Models;
using BankingApi.Services;

namespace BankingApi.Controllers;

/// <summary>
/// Controlador para operaciones de cuentas bancarias.
/// </summary>
[ApiController]
[Route("api/v1/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountStore _accountStore;
    private readonly ITransferService _transferService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(
        IAccountStore accountStore,
        ITransferService transferService,
        ILogger<AccountsController> logger)
    {
        _accountStore = accountStore;
        _transferService = transferService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene el saldo de una cuenta específica.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta (ej: ACC-001)</param>
    /// <returns>Información de la cuenta con su saldo actual.</returns>
    [HttpGet("{accountId}/balance")]
    public async Task<ActionResult<Account>> GetBalance(string accountId)
    {
        if (string.IsNullOrWhiteSpace(accountId))
        {
            _logger.LogWarning("Invalid account ID: empty or null");
            return BadRequest(new { error = "Account ID cannot be empty." });
        }

        var account = await _accountStore.GetAccountAsync(accountId);

        if (account == null)
        {
            _logger.LogWarning("Account {AccountId} not found", accountId);
            return NotFound(new { error = $"Account {accountId} not found." });
        }

        _logger.LogInformation("Retrieved balance for account {AccountId}: {Balance}", accountId, account.Balance);
        return Ok(account);
    }
}
