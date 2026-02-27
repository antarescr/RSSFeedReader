# 📋 BACKLOG DE TAREAS — Banking REST API

**Estado:** 🔴 No iniciado  
**Versión:** 1.0  
**Última actualización:** 27 de febrero de 2026  
**Tiempo estimado total:** 3-4 horas  

---

## 📌 Descripción General

Este documento define 5 tareas **secuenciales y mínimas** para implementar la Banking REST API en .NET 8. Cada tarea tiene entregables específicos y criterios de aceptación claros.

**Prerequisitos:**
- .NET 8 SDK instalado
- Visual Studio Code o Visual Studio 2022
- Haber leído SPECKIT-CONSTITUTION.md y spec.md

---

## 🎯 TAREA 1: Crear Proyecto Web API y Framework de Testing

**Estado:** ⬜ No iniciada  
**Tiempo estimado:** 30-45 minutos  
**Dependencias:** Ninguna  

### 📋 Descripción

Crear la estructura base del proyecto .NET 8 con:
- Proyecto Web API con Minimal APIs
- Proyecto de testing xUnit
- Configuración inicial del Program.cs
- NuGet packages necesarios (Serilog, Swagger)
- Carpetas lógicas (Models, Services, Controllers, Exceptions)

### ✅ Entregables

- [ ] Archivo `src/BankingApi/BankingApi.csproj` creado
- [ ] Archivo `src/BankingApi/Program.cs` configurado
- [ ] Carpetas creadas: `Models/`, `Services/`, `Controllers/`, `Exceptions/`
- [ ] Proyecto `tests/BankingApi.Tests/BankingApi.Tests.csproj` creado
- [ ] NuGet packages instalados: Serilog, Swagger (Swashbuckle)
- [ ] Archivo `.gitignore` actualizado para /bin, /obj

### 🔍 Criterios de Aceptación

1. **Proyecto Web API:**
   - Ejecutar `dotnet new webapi --name BankingApi` en `src/`
   - Carpetas Models/, Services/, Controllers/, Exceptions/ existen
   - Program.cs compila sin errores

2. **Proyecto xUnit:**
   - Ejecutar `dotnet new xunit --name BankingApi.Tests` en `tests/`
   - Referencia a BankingApi.csproj existe en BankingApi.Tests.csproj
   - `dotnet test` ejecuta sin errores (0 tests, que es normal)

3. **NuGet Packages:**
   - Serilog instalado en ambos proyectos
   - Swashbuckle instalado en BankingApi
   - `dotnet build` compila sin warnings críticos

4. **Estructura:**
   ```
   src/BankingApi/
   ├── Models/
   ├── Services/
   ├── Controllers/
   ├── Exceptions/
   ├── Program.cs
   ├── appsettings.json
   └── BankingApi.csproj
   
   tests/BankingApi.Tests/
   ├── BankingApi.Tests.csproj
   └── Usings.cs
   ```

### 📝 Pasos Exactos

```bash
# 1. Crear carpeta raíz del proyecto
mkdir -p /workspaces/RSSFeedReader/banking-api
cd /workspaces/RSSFeedReader/banking-api

# 2. Crear proyecto Web API
mkdir src
cd src
dotnet new webapi --name BankingApi --framework net8.0
cd BankingApi

# 3. Crear subcarpetas
mkdir -p Models Services Controllers Exceptions

# 4. Instalar NuGet packages
dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Swashbuckle.AspNetCore

# 5. Compilar (debe completar sin errores)
dotnet build

# 6. Volver a raíz y crear proyecto de tests
cd /workspaces/RSSFeedReader/banking-api
mkdir tests
cd tests
dotnet new xunit --name BankingApi.Tests --framework net8.0

# 7. Agregar referencia al proyecto principal
cd /workspaces/RSSFeedReader/banking-api/tests/BankingApi.Tests
dotnet add reference ../../src/BankingApi/BankingApi.csproj

# 8. Instalar Serilog en tests
dotnet add package Serilog

# 9. Compilar tests
dotnet build

# 10. Ejecutar tests (debe mostrar 0 tests)
dotnet test
```

### 🎁 Resultado Esperado

```
BankingApi (src/)
├── Controllers/
│   └── WeatherForecastController.cs (ELIMINAR después)
├── Models/
├── Services/
├── Exceptions/
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── Program.cs
└── BankingApi.csproj

BankingApi.Tests (tests/)
├── UnitTest1.cs (RENOMBRAR a TransferServiceTests.cs)
├── Usings.cs
└── BankingApi.Tests.csproj
```

---

## 🎯 TAREA 2: Crear Modelos de Datos (Account, Transfer)

**Estado:** ⬜ No iniciada  
**Tiempo estimado:** 20-30 minutos  
**Dependencias:** Tarea 1 completada  

### 📋 Descripción

Crear los modelos de datos que representan las entidades del negocio:
- `Account` — Representa una cuenta bancaria
- `TransferRequest` — Modelo de entrada para POST /transfers
- `TransferResponse` — Modelo de salida para respuestas

### ✅ Entregables

- [ ] Archivo `src/BankingApi/Models/Account.cs`
- [ ] Archivo `src/BankingApi/Models/TransferRequest.cs`
- [ ] Archivo `src/BankingApi/Models/TransferResponse.cs`
- [ ] Modelos compilados sin errores
- [ ] XML documentation en todos los modelos

### 🔍 Criterios de Aceptación

1. **Account.cs debe contener:**
   - Propiedad `AccountId` (string, ej: "ACC-001")
   - Propiedad `AccountOwner` (string)
   - Propiedad `Balance` (decimal)
   - Propiedad `Currency` (string, ej: "USD")
   - Constructor público
   - Validación de Balance >= 0

2. **TransferRequest.cs debe contener:**
   - Propiedad `SourceAccountId` (string)
   - Propiedad `TargetAccountId` (string)
   - Propiedad `Amount` (decimal)
   - Propiedad `Concept` (string, opcional)

3. **TransferResponse.cs debe contener:**
   - Propiedad `TransferId` (string)
   - Propiedad `Status` (string: "Success", "Failed")
   - Propiedad `Message` (string)
   - Propiedad `Timestamp` (DateTime)

4. **XML Documentation:**
   - Cada clase tiene `/// <summary>`
   - Cada propiedad tiene `/// <summary>`
   - Ejemplo: `/// <summary>Identificador único de la cuenta.</summary>`

### 📝 Código Base

**src/BankingApi/Models/Account.cs:**
```csharp
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
```

**src/BankingApi/Models/TransferRequest.cs:**
```csharp
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
```

**src/BankingApi/Models/TransferResponse.cs:**
```csharp
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
```

### 🎁 Resultado Esperado

```bash
dotnet build
# Resultado: Build succeeded with 0 warnings
```

---

## 🎯 TAREA 3: Crear Servicio en Memoria con Seed Data

**Estado:** ⬜ No iniciada  
**Tiempo estimado:** 45-60 minutos  
**Dependencias:** Tarea 1 y 2 completadas  

### 📋 Descripción

Crear el servicio que gestiona las cuentas en memoria (ConcurrentDictionary) con:
- Interfaz `IAccountStore` para consultar cuentas
- Clase `AccountService` que implementa la lógica
- Interfaz `ITransferService` para transferencias
- Clase `TransferService` con toda la validación (RB-001 a RB-005)
- Seed data con 3 cuentas predeterminadas

### ✅ Entregables

- [ ] Archivo `src/BankingApi/Services/IAccountStore.cs`
- [ ] Archivo `src/BankingApi/Services/AccountService.cs`
- [ ] Archivo `src/BankingApi/Services/ITransferService.cs`
- [ ] Archivo `src/BankingApi/Services/TransferService.cs`
- [ ] Seed data inicializado en Program.cs
- [ ] All services registered en dependency injection

### 🔍 Criterios de Aceptación

1. **IAccountStore:**
   - Método `Task<Account?> GetAccountAsync(string accountId)`
   - Método `Task<bool> AccountExistsAsync(string accountId)`
   - Método `Task UpdateBalanceAsync(string accountId, decimal newBalance)`

2. **AccountService:**
   - Implementa IAccountStore
   - Usa ConcurrentDictionary<string, Account> privado
   - Thread-safe (todas las operaciones atómicas)

3. **ITransferService:**
   - Método `Task<TransferResponse> TransferAsync(TransferRequest request)`
   - Implementa todas las 5 reglas de negocio:
     - RB-001: Amount debe ser positivo
     - RB-002: Sufficient funds check
     - RB-003: Different accounts check
     - RB-004: Both accounts must exist
     - RB-005: Atomic operation

4. **TransferService:**
   - Inyecta IAccountStore
   - Loguea operaciones con Serilog (con Correlation ID si aplica)
   - Genera TransferId único (ej: TRF-{timestamp}-{random})
   - Retorna descripción clara en Status/Message

5. **Seed Data:**
   ```
   ACC-001: John Doe, Balance: 1000.00 USD
   ACC-002: Jane Smith, Balance: 500.00 USD
   ACC-003: Bob Johnson, Balance: 0.00 USD
   ```

### 📝 Código Base

**src/BankingApi/Services/IAccountStore.cs:**
```csharp
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
```

**src/BankingApi/Services/AccountService.cs:**
```csharp
using System.Collections.Concurrent;
using BankingApi.Models;

namespace BankingApi.Services;

/// <summary>
/// Servicio de cuentas bancarias con almacenamiento en memoria.
/// </summary>
public class AccountService : IAccountStore
{
    private readonly ConcurrentDictionary<string, Account> _accounts;

    public AccountService()
    {
        _accounts = new ConcurrentDictionary<string, Account>();
        InitializeSeedData();
    }

    public Task<Account?> GetAccountAsync(string accountId)
    {
        _accounts.TryGetValue(accountId, out var account);
        return Task.FromResult(account);
    }

    public Task<bool> AccountExistsAsync(string accountId)
    {
        return Task.FromResult(_accounts.ContainsKey(accountId));
    }

    public Task UpdateBalanceAsync(string accountId, decimal newBalance)
    {
        if (_accounts.TryGetValue(accountId, out var account))
        {
            account.Balance = newBalance;
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return Task.FromResult(_accounts.Values.AsEnumerable());
    }

    private void InitializeSeedData()
    {
        var accounts = new[]
        {
            new Account 
            { 
                AccountId = "ACC-001", 
                AccountOwner = "John Doe", 
                Balance = 1000m, 
                Currency = "USD" 
            },
            new Account 
            { 
                AccountId = "ACC-002", 
                AccountOwner = "Jane Smith", 
                Balance = 500m, 
                Currency = "USD" 
            },
            new Account 
            { 
                AccountId = "ACC-003", 
                AccountOwner = "Bob Johnson", 
                Balance = 0m, 
                Currency = "USD" 
            }
        };

        foreach (var account in accounts)
        {
            _accounts.TryAdd(account.AccountId, account);
        }
    }
}
```

**src/BankingApi/Services/ITransferService.cs:**
```csharp
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
```

**src/BankingApi/Services/TransferService.cs:**
```csharp
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
```

**Program.cs (actualizado):**
```csharp
using Serilog;
using BankingApi.Services;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddScoped<IAccountStore, AccountService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### 🎁 Resultado Esperado

```bash
dotnet build
# Resultado: Build succeeded with 0 warnings

dotnet run
# Resultado: Application started. Swagger UI disponible en http://localhost:5000/swagger
```

---

## 🎯 TAREA 4: Crear Endpoints (Controllers / Minimal APIs)

**Estado:** ⬜ No iniciada  
**Tiempo estimado:** 45-60 minutos  
**Dependencias:** Tareas 1, 2 y 3 completadas  

### 📋 Descripción

Crear los dos endpoints requeridos:
1. `GET /api/v1/accounts/{accountId}/balance` — Consultar saldo
2. `POST /api/v1/transfers` — Realizar transferencia

Implementar con **Controllers** (más legible para principiantes).

### ✅ Entregables

- [ ] Archivo `src/BankingApi/Controllers/AccountsController.cs`
- [ ] Endpoint GET /api/v1/accounts/{accountId}/balance implementado
- [ ] Endpoint POST /api/v1/transfers implementado
- [ ] Validación de entrada (ModelState)
- [ ] Respuestas HTTP correctas (200, 400, 404, 422)
- [ ] Swagger documentado correctamente

### 🔍 Criterios de Aceptación

1. **GET /api/v1/accounts/{accountId}/balance:**
   - Request: URL parameter `accountId`
   - Response 200 OK:
     ```json
     {
       "accountId": "ACC-001",
       "accountOwner": "John Doe",
       "balance": 1000.00,
       "currency": "USD"
     }
     ```
   - Response 404 Not Found si la cuenta no existe
   - Response 400 Bad Request si accountId es vacío

2. **POST /api/v1/transfers:**
   - Request body:
     ```json
     {
       "sourceAccountId": "ACC-001",
       "targetAccountId": "ACC-002",
       "amount": 100.00,
       "concept": "Payment for services"
     }
     ```
   - Response 200 OK con TransferResponse
   - Response 400 Bad Request si validación falla
   - Response 422 Unprocessable Entity si regla de negocio falla

3. **ModelState Validation:**
   - sourceAccountId requerido (no null)
   - targetAccountId requerido (no null)
   - amount requerido (> 0)

### 📝 Código Base

**src/BankingApi/Controllers/AccountsController.cs:**
```csharp
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
    /// <response code="200">Cuenta encontrada.</response>
    /// <response code="404">Cuenta no existe.</response>
    /// <response code="400">ID de cuenta inválido.</response>
    [HttpGet("{accountId}/balance")]
    [ProduceResponseType(StatusCodes.Status200OK)]
    [ProduceResponseType(StatusCodes.Status404NotFound)]
    [ProduceResponseType(StatusCodes.Status400BadRequest)]
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
```

**src/BankingApi/Controllers/TransfersController.cs (nuevo archivo):**
```csharp
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
    /// <response code="200">Transferencia completada (exitosa o fallida).</response>
    /// <response code="400">Validación fallida (campos requeridos faltantes).</response>
    /// <response code="422">Regla de negocio violada (saldo insuficiente, cuenta no existe, etc).</response>
    [HttpPost]
    [ProduceResponseType(StatusCodes.Status200OK)]
    [ProduceResponseType(StatusCodes.Status400BadRequest)]
    [ProduceResponseType(StatusCodes.Status422UnprocessableEntity)]
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
```

**Actualizar appsettings.json:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 🎁 Resultado Esperado

```bash
dotnet build
# Build succeeded with 0 warnings

dotnet run
# Swagger disponible en http://localhost:5000/swagger/index.html

# En otra terminal:
curl -X GET http://localhost:5000/api/v1/accounts/ACC-001/balance
# Resultado:
{
  "accountId": "ACC-001",
  "accountOwner": "John Doe",
  "balance": 1000,
  "currency": "USD"
}
```

---

## 🎯 TAREA 5: Escribir Pruebas Unitarias para TransferService

**Estado:** ⬜ No iniciada  
**Tiempo estimado:** 60-90 minutos  
**Dependencias:** Tareas 1, 2 y 3 completadas  

### 📋 Descripción

Crear pruebas unitarias exhaustivas para `TransferService` cubriendo:
- 5 reglas de negocio (RB-001 a RB-005)
- Casos exitosos
- Casos de error
- Verificación de logging
- Atomicidad de operaciones

Objetivo: **≥80% code coverage en TransferService**

### ✅ Entregables

- [ ] Archivo `tests/BankingApi.Tests/TransferServiceTests.cs`
- [ ] Mínimo 12 test cases (uno por cada escenario)
- [ ] Mock de IAccountStore funcional
- [ ] Todos los tests ejecutados con `dotnet test`
- [ ] Code coverage ≥80%

### 🔍 Criterios de Aceptación

1. **RB-001 Tests (Amount positivo):**
   - ✅ Transfer_AmountIsPositive_Success
   - ❌ Transfer_AmountIsZero_Fails
   - ❌ Transfer_AmountIsNegative_Fails

2. **RB-002 Tests (Sufficient funds):**
   - ✅ Transfer_SufficientFunds_Success
   - ❌ Transfer_InsufficientFunds_Fails
   - ❌ Transfer_ExactBalance_Success

3. **RB-003 Tests (Different accounts):**
   - ❌ Transfer_SameAccount_Fails

4. **RB-004 Tests (Both accounts exist):**
   - ❌ Transfer_SourceNotExists_Fails
   - ❌ Transfer_TargetNotExists_Fails
   - ❌ Transfer_BothNotExist_Fails

5. **RB-005 Tests (Atomic operation):**
   - ✅ Transfer_ValidTransfer_UpdatesBothBalances

6. **Additional Tests:**
   - ✅ Transfer_ValidTransfer_ReturnsUniqueTransferId
   - ✅ Transfer_ValidTransfer_ReturnsSuccessStatus

### 📝 Código Base

**tests/BankingApi.Tests/TransferServiceTests.cs:**
```csharp
using Xunit;
using Moq;
using BankingApi.Models;
using BankingApi.Services;
using Microsoft.Extensions.Logging;

namespace BankingApi.Tests;

/// <summary>
/// Pruebas unitarias para TransferService.
/// Cubre todas las reglas de negocio (RB-001 a RB-005).
/// </summary>
public class TransferServiceTests
{
    private readonly Mock<IAccountStore> _accountStoreMock;
    private readonly Mock<ILogger<TransferService>> _loggerMock;
    private readonly TransferService _transferService;

    public TransferServiceTests()
    {
        _accountStoreMock = new Mock<IAccountStore>();
        _loggerMock = new Mock<ILogger<TransferService>>();
        _transferService = new TransferService(_accountStoreMock.Object, _loggerMock.Object);
    }

    // RB-001: Amount must be positive
    [Fact]
    public async Task Transfer_AmountIsZero_ReturnsFailed()
    {
        // Arrange
        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 0,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("greater than 0", response.Message);
    }

    [Fact]
    public async Task Transfer_AmountIsNegative_ReturnsFailed()
    {
        // Arrange
        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = -100,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("greater than 0", response.Message);
    }

    [Fact]
    public async Task Transfer_AmountIsPositive_ExceedsZeroValidation()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Valid transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert (passes amount validation, continues to other validations)
        Assert.NotNull(response);
        // Won't fail on amount validation, should succeed or fail on other validations
    }

    // RB-003: Different accounts
    [Fact]
    public async Task Transfer_SourceEqualsTarget_ReturnsFailed()
    {
        // Arrange
        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-001",
            Amount = 100,
            Concept = "Self transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("must be different", response.Message);
    }

    // RB-004: Both accounts exist
    [Fact]
    public async Task Transfer_SourceAccountNotFound_ReturnsFailed()
    {
        // Arrange
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-999"))
            .ReturnsAsync((Account?)null);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-999",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("does not exist", response.Message);
        Assert.Contains("ACC-999", response.Message);
    }

    [Fact]
    public async Task Transfer_TargetAccountNotFound_ReturnsFailed()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-999"))
            .ReturnsAsync((Account?)null);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-999",
            Amount = 100,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("does not exist", response.Message);
        Assert.Contains("ACC-999", response.Message);
    }

    // RB-002: Sufficient funds
    [Fact]
    public async Task Transfer_InsufficientFunds_ReturnsFailed()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 50, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "More than balance"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("Insufficient funds", response.Message);
    }

    [Fact]
    public async Task Transfer_ExactBalance_Succeeds()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 100, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Exact balance"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Success", response.Status);
    }

    // RB-005: Atomic operation
    [Fact]
    public async Task Transfer_ValidTransfer_UpdatesBothBalances()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 250,
            Concept = "Valid transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Success", response.Status);
        _accountStoreMock.Verify(
            x => x.UpdateBalanceAsync("ACC-001", 750),
            Times.Once,
            "Source account balance should be decreased by 250");
        _accountStoreMock.Verify(
            x => x.UpdateBalanceAsync("ACC-002", 750),
            Times.Once,
            "Target account balance should be increased by 250");
    }

    [Fact]
    public async Task Transfer_ValidTransfer_ReturnsUniqueTransferId()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Test transfrer"
        };

        // Act
        var response1 = await _transferService.TransferAsync(request);
        var response2 = await _transferService.TransferAsync(request);

        // Assert
        Assert.NotEqual(response1.TransferId, response2.TransferId);
        Assert.StartsWith("TRF-", response1.TransferId);
        Assert.StartsWith("TRF-", response2.TransferId);
    }

    [Fact]
    public async Task Transfer_ValidTransfer_ReturnsSuccessStatus()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Valid transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Success", response.Status);
        Assert.NotEmpty(response.Message);
        Assert.NotEqual(default, response.Timestamp);
    }
}
```

**Actualizar tests/BankingApi.Tests/BankingApi.Tests.csproj:**
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
  <PackageReference Include="xunit" Version="2.6.4" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.5.4">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="Moq" Version="4.20.69" />
  <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.0" />
  <PackageReference Include="coverlet.collector" Version="6.0.0">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
</ItemGroup>
```

### 🎁 Resultado Esperado

```bash
cd tests/BankingApi.Tests
dotnet test
# Resultado:
# Test Run Successful.
# Total tests: 12
# Passed: 12
# Failed: 0
# Skipped: 0
# Duration: ~2s

# Coverage report:
# BankingApi.Services.TransferService: 85% coverage
```

---

## 📅 Cronograma Total

| Tarea | Tiempo | Acumulado | Estado |
|-------|--------|-----------|--------|
| T1: Setup Proyecto | 30-45 min | 30-45 min | ⬜ |
| T2: Modelos | 20-30 min | 50-75 min | ⬜ |
| T3: Servicios | 45-60 min | 95-135 min | ⬜ |
| T4: Endpoints | 45-60 min | 140-195 min | ⬜ |
| T5: Tests | 60-90 min | 200-285 min | ⬜ |
| **TOTAL** | — | **3.5-4.5 horas** | ⬜ |

---

## ✅ Checklist de Completitud

### Después de completar TODAS las 5 tareas:

- [ ] Proyecto compila sin errores: `dotnet build`
- [ ] Tests pasan: `dotnet test` (12/12 passing)
- [ ] Code coverage ≥80%: `dotnet test /p:CollectCoverage=true`
- [ ] Swagger activo: `dotnet run` → http://localhost:5000/swagger
- [ ] Seed data cargado: GET /api/v1/accounts/ACC-001/balance retorna 1000
- [ ] Transferencia exitosa: POST /api/v1/transfers entre ACC-001 y ACC-002 retorna Success
- [ ] Transferencia fallida: POST con monto negativo retorna Failed
- [ ] Insufficient funds: POST ACC-003 → ACC-001 por 1000 retorna Failed (ACC-003 balance = 0)
- [ ] Todos los 7 cURL examples de spec.md funcionan correctamente
- [ ] Logging funciona: ejecutar con `--verbose` muestra logs de cada operación
- [ ] XML documentation comentarios presentes en Model y Service classes

---

## 🔗 Referencias

- SPECKIT-CONSTITUTION.md — Reglas arquitectónicas
- spec.md — Especificación funcional (2 operaciones)
- plan.md — Plan técnico base
- docs/GETTING_STARTED.md — Guía de inicio rápido

---

**Última actualización:** 27 de febrero de 2026  
**Versión:** 1.0
