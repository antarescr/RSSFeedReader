namespace BankingApi.Models;

/// <summary>
/// Representa una cuenta bancaria en el sistema.
/// </summary>
public class Account
{
    /// <summary>
    /// Identificador único de la cuenta (ej: ACC-001).
    /// </summary>
    public required string AccountId { get; set; }

    /// <summary>
    /// Nombre del propietario de la cuenta.
    /// </summary>
    public required string AccountOwner { get; set; }

    /// <summary>
    /// Saldo actual de la cuenta en la moneda especificada.
    /// Debe ser mayor o igual a 0.
    /// </summary>
    public required decimal Balance { get; set; }

    /// <summary>
    /// Código de moneda (ej: USD, EUR, MXN).
    /// </summary>
    public required string Currency { get; set; }
}
