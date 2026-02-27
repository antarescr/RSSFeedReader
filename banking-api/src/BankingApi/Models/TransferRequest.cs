namespace BankingApi.Models;

/// <summary>
/// Solicitud de transferencia bancaria.
/// </summary>
public class TransferRequest
{
    /// <summary>
    /// Identificador de la cuenta de origen.
    /// </summary>
    public required string SourceAccountId { get; set; }

    /// <summary>
    /// Identificador de la cuenta de destino.
    /// </summary>
    public required string TargetAccountId { get; set; }

    /// <summary>
    /// Cantidad a transferir (debe ser positiva).
    /// </summary>
    public required decimal Amount { get; set; }

    /// <summary>
    /// Descripción o concepto de la transferencia.
    /// </summary>
    public string? Concept { get; set; }
}
