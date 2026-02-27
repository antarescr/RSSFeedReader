using Microsoft.AspNetCore.Mvc;
using BankingApi.Models;
using BankingApi.Services;

namespace BankingApi.Controllers;

/// <summary>
/// Controlador para operaciones de transferencia bancaria.
/// </summary>
[ApiController]
[Route("api/v1/transfers")]
public class TransfersController : ControllerBase
{
    private readonly ITransferService _transferService;
    private readonly ILogger<TransfersController> _logger;

    public TransfersController(
        ITransferService transferService,
        ILogger<TransfersController> logger)
    {
        _transferService = transferService;
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta una transferencia bancaria de una cuenta a otra.
    /// </summary>
    /// <param name="request">Detalles de la transferencia.</param>
    /// <returns>Resultado de la operación de transferencia.</returns>
    [HttpPost]
    public async Task<ActionResult<TransferResponse>> Transfer([FromBody] TransferRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid transfer request: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Processing transfer from {Source} to {Target} amount {Amount}",
            request.SourceAccountId, request.TargetAccountId, request.Amount);

        var response = await _transferService.TransferAsync(request);

        // Si fue exitosa, retornar 200. Si falló, retornar 422.
        if (response.Status == "Success")
        {
            return Ok(response);
        }
        else
        {
            _logger.LogWarning("Transfer failed: {Message}", response.Message);
            return UnprocessableEntity(response);
        }
    }
}
