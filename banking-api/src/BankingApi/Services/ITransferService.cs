using BankingApi.Models;

namespace BankingApi.Services;

/// <summary>
/// Interfaz para ejecutar transferencias bancarias.
/// </summary>
public interface ITransferService
{
    /// <summary>
    /// Ejecuta una transferencia entre dos cuentas.
    /// Implementa todas las validaciones de negocio (RB-001 a RB-005).
    /// </summary>
    Task<TransferResponse> TransferAsync(TransferRequest request);
}
