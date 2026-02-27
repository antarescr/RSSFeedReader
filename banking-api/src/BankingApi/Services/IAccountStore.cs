using BankingApi.Models;

namespace BankingApi.Services;

/// <summary>
/// Interfaz para acceder y gestionar cuentas bancarias.
/// </summary>
public interface IAccountStore
{
    /// <summary>
    /// Obtiene una cuenta por su identificador.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <returns>Account si existe, null en caso contrario.</returns>
    Task<Account?> GetAccountAsync(string accountId);

    /// <summary>
    /// Verifica si una cuenta existe.
    /// </summary>
    Task<bool> AccountExistsAsync(string accountId);

    /// <summary>
    /// Actualiza el saldo de una cuenta.
    /// </summary>
    Task UpdateBalanceAsync(string accountId, decimal newBalance);

    /// <summary>
    /// Obtiene todas las cuentas (para seed data).
    /// </summary>
    Task<IEnumerable<Account>> GetAllAccountsAsync();
}
