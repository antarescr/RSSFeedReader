namespace BankingApi.Models;

/// <summary>
/// Respuesta de operación de transferencia.
/// </summary>
public class TransferResponse
{
    /// <summary>
    /// Identificador único de la transferencia generado por el sistema.
    /// </summary>
    public required string TransferId { get; set; }

    /// <summary>
    /// Estado de la transferencia: "Success" o "Failed".
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Mensaje descriptivo del resultado de la operación.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Timestamp de cuándo se procesó la transferencia.
    /// </summary>
    public required DateTime Timestamp { get; set; }
}
