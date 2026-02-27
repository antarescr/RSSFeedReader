# 🏗️ PLAN TÉCNICO DE IMPLEMENTACIÓN - Banking REST API

**Versión:** 1.0.0  
**Fecha:** 27 de febrero de 2026  
**Basado en:** [spec.md](./spec.md)  
**Audiencia:** Desarrolladores, arquitectos técnicos

---

## 1. VISIÓN GENERAL

Implementar una Banking REST API minimalista en **ASP.NET Core 8** con:
- ✅ 2 endpoints REST únicamente
- ✅ Almacenamiento en-memoria (ConcurrentDictionary)
- ✅ Sin base de datos
- ✅ Arquitectura simplificada en carpetas lógicas
- ✅ Swagger auto-documentado
- ✅ Pruebas unitarias del servicio de transferencias

**Tiempo estimado de implementación:** 4-6 horas  
**Complejidad:** ⭐ Baja (MVP educativo)

---

## 2. STACK TECNOLÓGICO

| Componente | Versión | Propósito |
|-----------|---------|----------|
| **.NET SDK** | 8.0+ | Runtime y compilación |
| **C#** | 12.0 | Lenguaje de programación |
| **ASP.NET Core** | 8.0 | Framework Web |
| **Swashbuckle** | 6.4+ | Swagger/OpenAPI |
| **xUnit** | 2.6+ | Framework de pruebas |
| **Moq** | 4.18+ | Mocking (opcional para tests) |

---

## 3. ESTRUCTURA DE CARPETAS

```
src/BankingApi/
├── BankingApi.csproj                 # Archivo de proyecto
├── Program.cs                        # Entry point, configuración, seed data
│
├── Models/                           # Modelos de datos
│   ├── Account.cs                    # Entidad Cuenta
│   ├── Transfer.cs                   # Entidad Transferencia
│   └── Responses.cs                  # DTOs de respuesta
│
├── Controllers/                      # Endpoints REST
│   └── AccountsController.cs         # 2 endpoints: GET balance, POST transfer
│
├── Services/                         # Lógica de negocio
│   ├── IAccountService.cs            # Interfaz
│   ├── AccountService.cs             # Implementación
│   └── ITransferService.cs           # Interfaz
│   └── TransferService.cs            # Implementación + validaciones
│
├── Exceptions/                       # Excepciones personalizadas
│   ├── InvalidAmountException.cs
│   ├── InsufficientFundsException.cs
│   ├── SameAccountTransferException.cs
│   └── AccountNotFoundException.cs
│
├── appsettings.json                  # Configuración
│
└── BankingApi.Tests/                 # Proyecto de pruebas (si va separado)
    ├── BankingApi.Tests.csproj
    └── Services/
        └── TransferServiceTests.cs

```

---

## 4. PASO A PASO DE IMPLEMENTACIÓN

### FASE 1: Preparación del Proyecto

#### Paso 1.1: Crear estructura de carpetas

```bash
# En la raíz del repositorio
cd src/BankingApi

# Crear carpetas
mkdir Models Controllers Services Exceptions

# Confirmación
ls -la
# Debería mostrar: Models/, Controllers/, Services/, Exceptions/
```

#### Paso 1.2: Crear archivo .csproj

**Archivo:** `src/BankingApi/BankingApi.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <WarningsAsErrors>nullable</WarningsAsErrors>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
  </ItemGroup>

</Project>
```

---

### FASE 2: Modelos de Datos

#### Paso 2.1: Crear `Models/Account.cs`

```csharp
namespace BankingApi.Models
{
    /// <summary>
    /// Entidad que representa una cuenta bancaria.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Identificador único de la cuenta (ej: ACC-001).
        /// </summary>
        public string AccountId { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del titular de la cuenta.
        /// </summary>
        public string AccountOwner { get; set; } = string.Empty;

        /// <summary>
        /// Saldo actual en USD.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Estado de la cuenta (ACTIVE).
        /// </summary>
        public string Status { get; set; } = "ACTIVE";

        /// <summary>
        /// Fecha y hora de creación (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
```

#### Paso 2.2: Crear `Models/Transfer.cs`

```csharp
namespace BankingApi.Models
{
    /// <summary>
    /// Entidad que representa una transferencia bancaria.
    /// </summary>
    public class Transfer
    {
        /// <summary>
        /// Identificador único de la transferencia (ej: TRF-001).
        /// </summary>
        public string TransferId { get; set; } = string.Empty;

        /// <summary>
        /// ID de la cuenta origen.
        /// </summary>
        public string SourceAccountId { get; set; } = string.Empty;

        /// <summary>
        /// ID de la cuenta destino.
        /// </summary>
        public string TargetAccountId { get; set; } = string.Empty;

        /// <summary>
        /// Monto transferido en USD.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Concepto o descripción de la transferencia.
        /// </summary>
        public string Concept { get; set; } = string.Empty;

        /// <summary>
        /// Estado de la transferencia (COMPLETED o FAILED).
        /// </summary>
        public string Status { get; set; } = "COMPLETED";

        /// <summary>
        /// Fecha y hora de completación (UTC).
        /// </summary>
        public DateTime CompletedAt { get; set; }
    }
}
```

#### Paso 2.3: Crear `Models/Responses.cs`

```csharp
namespace BankingApi.Models
{
    /// <summary>
    /// DTO de respuesta de saldo de cuenta.
    /// </summary>
    public class GetBalanceResponse
    {
        public string AccountId { get; set; } = string.Empty;
        public string AccountOwner { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "USD";
    }

    /// <summary>
    /// DTO de solicitud de transferencia.
    /// </summary>
    public class CreateTransferRequest
    {
        public string SourceAccountId { get; set; } = string.Empty;
        public string TargetAccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Concept { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO de respuesta de transferencia.
    /// </summary>
    public class CreateTransferResponse
    {
        public string TransferId { get; set; } = string.Empty;
        public string SourceAccountId { get; set; } = string.Empty;
        public string TargetAccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal SourceBalanceAfter { get; set; }
        public decimal TargetBalanceAfter { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    /// <summary>
    /// Respuesta envolvente estándar para ALL endpoints.
    /// </summary>
    public class ApiResponse<T> where T : class
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public ErrorDetails? Error { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string CorrelationId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Detalles de error.
    /// </summary>
    public class ErrorDetails
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, object>? Details { get; set; }
    }
}
```

---

### FASE 3: Excepciones Personalizadas

#### Paso 3.1: Crear `Exceptions/`

**Archivo:** `Exceptions/InvalidAmountException.cs`
```csharp
namespace BankingApi.Exceptions
{
    /// <summary>
    /// Se lanza cuando el monto de una transferencia es inválido (≤ 0).
    /// </summary>
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException(string message) : base(message) { }
    }
}
```

**Archivo:** `Exceptions/InsufficientFundsException.cs`
```csharp
namespace BankingApi.Exceptions
{
    /// <summary>
    /// Se lanza cuando la cuenta origen no tiene saldo suficiente.
    /// </summary>
    public class InsufficientFundsException : Exception
    {
        public decimal Available { get; set; }
        public decimal Required { get; set; }

        public InsufficientFundsException(string message, decimal available, decimal required) 
            : base(message)
        {
            Available = available;
            Required = required;
        }
    }
}
```

**Archivo:** `Exceptions/SameAccountTransferException.cs`
```csharp
namespace BankingApi.Exceptions
{
    /// <summary>
    /// Se lanza cuando se intenta transferir a la misma cuenta.
    /// </summary>
    public class SameAccountTransferException : Exception
    {
        public SameAccountTransferException(string message) : base(message) { }
    }
}
```

**Archivo:** `Exceptions/AccountNotFoundException.cs`
```csharp
namespace BankingApi.Exceptions
{
    /// <summary>
    /// Se lanza cuando una cuenta no existe.
    /// </summary>
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message) : base(message) { }
    }
}
```

---

### FASE 4: Servicios

#### Paso 4.1: Crear `Services/IAccountService.cs`

```csharp
using BankingApi.Models;

namespace BankingApi.Services
{
    /// <summary>
    /// Interfaz para operaciones sobre cuentas.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Obtiene una cuenta por su ID.
        /// </summary>
        Task<Account?> GetAccountAsync(string accountId);

        /// <summary>
        /// Obtiene todas las cuentas.
        /// </summary>
        Task<IEnumerable<Account>> GetAllAccountsAsync();

        /// <summary>
        /// Obtiene el saldo actual de una cuenta.
        /// </summary>
        Task<decimal> GetBalanceAsync(string accountId);
    }
}
```

#### Paso 4.2: Crear `Services/AccountService.cs`

```csharp
using BankingApi.Models;
using BankingApi.Exceptions;
using System.Collections.Concurrent;

namespace BankingApi.Services
{
    /// <summary>
    /// Implementación del servicio de cuentas.
    /// Mantiene un diccionario en-memoria de cuentas.
    /// </summary>
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Almacenamiento en-memoria de cuentas.
        /// Clave: AccountId, Valor: Account
        /// </summary>
        private readonly ConcurrentDictionary<string, Account> _accounts;

        public AccountService()
        {
            _accounts = new ConcurrentDictionary<string, Account>();
            
            // Seed data: Pre-cargar 3 cuentas
            InitializeAccounts();
        }

        /// <summary>
        /// Inicializa las cuentas seed (datos de prueba).
        /// </summary>
        private void InitializeAccounts()
        {
            var accounts = new[]
            {
                new Account
                {
                    AccountId = "ACC-001",
                    AccountOwner = "Juan Pérez",
                    Balance = 1000.00m,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.UtcNow
                },
                new Account
                {
                    AccountId = "ACC-002",
                    AccountOwner = "María García",
                    Balance = 500.00m,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.UtcNow
                },
                new Account
                {
                    AccountId = "ACC-003",
                    AccountOwner = "Carlos López",
                    Balance = 0.00m,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var account in accounts)
            {
                _accounts.TryAdd(account.AccountId, account);
            }
        }

        public Task<Account?> GetAccountAsync(string accountId)
        {
            var account = _accounts.TryGetValue(accountId, out var result) ? result : null;
            return Task.FromResult(account);
        }

        public Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return Task.FromResult(_accounts.Values.AsEnumerable());
        }

        public async Task<decimal> GetBalanceAsync(string accountId)
        {
            var account = await GetAccountAsync(accountId);
            if (account == null)
                throw new AccountNotFoundException($"La cuenta {accountId} no existe");

            return account.Balance;
        }

        /// <summary>
        /// Actualiza el saldo de una cuenta (usado por TransferService).
        /// INTERNAL: Solo para transacciones.
        /// </summary>
        internal void UpdateBalance(string accountId, decimal newBalance)
        {
            if (_accounts.TryGetValue(accountId, out var account))
            {
                account.Balance = newBalance;
            }
        }

        /// <summary>
        /// Obtiene la instancia de una cuenta para modificación (transacciones).
        /// </summary>
        internal Account? GetAccountDirect(string accountId)
        {
            _accounts.TryGetValue(accountId, out var account);
            return account;
        }
    }
}
```

#### Paso 4.3: Crear `Services/ITransferService.cs`

```csharp
using BankingApi.Models;

namespace BankingApi.Services
{
    /// <summary>
    /// Interfaz para operaciones de transferencias.
    /// </summary>
    public interface ITransferService
    {
        /// <summary>
        /// Procesa una transferencia de dinero entre cuentas.
        /// Aplica todas las validaciones de negocio.
        /// </summary>
        Task<Transfer> ProcessTransferAsync(CreateTransferRequest request);
    }
}
```

#### Paso 4.4: Crear `Services/TransferService.cs`

```csharp
using BankingApi.Models;
using BankingApi.Exceptions;

namespace BankingApi.Services
{
    /// <summary>
    /// Implementación del servicio de transferencias.
    /// Contiene toda la lógica de validación y procesamiento.
    /// </summary>
    public class TransferService : ITransferService
    {
        private readonly IAccountService _accountService;
        private static int _transferCounter = 0;

        public TransferService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Procesa una transferencia con todas las validaciones.
        /// 
        /// Validaciones:
        /// 1. Monto > 0
        /// 2. Cuentas diferentes
        /// 3. Ambas cuentas existen
        /// 4. Saldo suficiente en origen
        /// 5. Atomicidad (actualizar ambas o ninguna)
        /// </summary>
        public async Task<Transfer> ProcessTransferAsync(CreateTransferRequest request)
        {
            // RB-001: Validar monto
            if (request.Amount <= 0)
                throw new InvalidAmountException($"El monto debe ser mayor a cero. Recibido: {request.Amount}");

            // RB-003: Validar que origen ≠ destino
            if (request.SourceAccountId == request.TargetAccountId)
                throw new SameAccountTransferException(
                    $"No se puede transferir a la misma cuenta: {request.SourceAccountId}");

            // RB-004: Validar que ambas cuentas existen
            var sourceAccount = await _accountService.GetAccountAsync(request.SourceAccountId);
            if (sourceAccount == null)
                throw new AccountNotFoundException(
                    $"La cuenta origen {request.SourceAccountId} no existe");

            var targetAccount = await _accountService.GetAccountAsync(request.TargetAccountId);
            if (targetAccount == null)
                throw new AccountNotFoundException(
                    $"La cuenta destino {request.TargetAccountId} no existe");

            // RB-002: Validar saldo suficiente
            if (sourceAccount.Balance < request.Amount)
                throw new InsufficientFundsException(
                    "Saldo insuficiente para completar la transferencia",
                    sourceAccount.Balance,
                    request.Amount);

            // RB-005: Ejecutar transferencia (ATÓMICAMENTE)
            // Actualizar ambas cuentas simultáneamente
            sourceAccount.Balance -= request.Amount;
            targetAccount.Balance += request.Amount;

            // Crear registro de transferencia
            var transfer = new Transfer
            {
                TransferId = $"TRF-{++_transferCounter:D3}",
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.TargetAccountId,
                Amount = request.Amount,
                Concept = request.Concept ?? string.Empty,
                Status = "COMPLETED",
                CompletedAt = DateTime.UtcNow
            };

            return await Task.FromResult(transfer);
        }
    }
}
```

---

### FASE 5: Controllers

#### Paso 5.1: Crear `Controllers/AccountsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using BankingApi.Models;
using BankingApi.Services;
using BankingApi.Exceptions;

namespace BankingApi.Controllers
{
    /// <summary>
    /// Controlador con 2 endpoints:
    /// 1. GET /api/v1/accounts/{accountId}/balance - Consultar saldo
    /// 2. POST /api/v1/transfers - Realizar transferencia
    /// </summary>
    [ApiController]
    [Route("api/v1")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransferService _transferService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            IAccountService accountService,
            ITransferService transferService,
            ILogger<AccountsController> logger)
        {
            _accountService = accountService;
            _transferService = transferService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el saldo actual de una cuenta.
        /// GET /api/v1/accounts/{accountId}/balance
        /// </summary>
        [HttpGet("accounts/{accountId}/balance")]
        [ProducesResponseType(typeof(ApiResponse<GetBalanceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBalanceResponse>>> GetBalance(string accountId)
        {
            var correlationId = HttpContext.TraceIdentifier;
            
            try
            {
                var account = await _accountService.GetAccountAsync(accountId);
                
                if (account == null)
                {
                    var errorResponse = new ApiResponse<GetBalanceResponse>
                    {
                        Success = false,
                        StatusCode = 404,
                        Error = new ErrorDetails
                        {
                            Code = "ACCOUNT_NOT_FOUND",
                            Message = $"La cuenta {accountId} no existe"
                        },
                        CorrelationId = correlationId
                    };
                    return NotFound(errorResponse);
                }

                var response = new ApiResponse<GetBalanceResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Data = new GetBalanceResponse
                    {
                        AccountId = account.AccountId,
                        AccountOwner = account.AccountOwner,
                        Balance = account.Balance,
                        Currency = "USD"
                    },
                    CorrelationId = correlationId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando saldo para cuenta {AccountId}", accountId);
                
                var errorResponse = new ApiResponse<GetBalanceResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Error = new ErrorDetails
                    {
                        Code = "INTERNAL_SERVER_ERROR",
                        Message = "Error interno del servidor"
                    },
                    CorrelationId = correlationId
                };
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Realiza una transferencia de dinero entre cuentas.
        /// POST /api/v1/transfers
        /// </summary>
        [HttpPost("transfers")]
        [ProducesResponseType(typeof(ApiResponse<CreateTransferResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CreateTransferResponse>>> CreateTransfer(
            [FromBody] CreateTransferRequest request)
        {
            var correlationId = HttpContext.TraceIdentifier;

            try
            {
                // Procesar transferencia (incluye todas las validaciones)
                var transfer = await _transferService.ProcessTransferAsync(request);

                // Obtener saldos actualizados
                var sourceBalance = await _accountService.GetBalanceAsync(request.SourceAccountId);
                var targetBalance = await _accountService.GetBalanceAsync(request.TargetAccountId);

                var response = new ApiResponse<CreateTransferResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Data = new CreateTransferResponse
                    {
                        TransferId = transfer.TransferId,
                        SourceAccountId = transfer.SourceAccountId,
                        TargetAccountId = transfer.TargetAccountId,
                        Amount = transfer.Amount,
                        Status = transfer.Status,
                        SourceBalanceAfter = sourceBalance,
                        TargetBalanceAfter = targetBalance,
                        CompletedAt = transfer.CompletedAt
                    },
                    CorrelationId = correlationId
                };

                _logger.LogInformation(
                    "Transferencia completada: {TransferId} | {Monto} de {Origen} a {Destino}",
                    transfer.TransferId, transfer.Amount, request.SourceAccountId, request.TargetAccountId);

                return Ok(response);
            }
            catch (InvalidAmountException ex)
            {
                var errorResponse = new ApiResponse<CreateTransferResponse>
                {
                    Success = false,
                    StatusCode = 400,
                    Error = new ErrorDetails
                    {
                        Code = "INVALID_AMOUNT",
                        Message = ex.Message,
                        Details = new Dictionary<string, object> { { "minimum", 0.01 } }
                    },
                    CorrelationId = correlationId
                };
                return BadRequest(errorResponse);
            }
            catch (SameAccountTransferException ex)
            {
                var errorResponse = new ApiResponse<CreateTransferResponse>
                {
                    Success = false,
                    StatusCode = 400,
                    Error = new ErrorDetails
                    {
                        Code = "SAME_ACCOUNT_TRANSFER",
                        Message = ex.Message,
                        Details = new Dictionary<string, object> { { "accountId", request.SourceAccountId } }
                    },
                    CorrelationId = correlationId
                };
                return BadRequest(errorResponse);
            }
            catch (InsufficientFundsException ex)
            {
                var errorResponse = new ApiResponse<CreateTransferResponse>
                {
                    Success = false,
                    StatusCode = 400,
                    Error = new ErrorDetails
                    {
                        Code = "INSUFFICIENT_FUNDS",
                        Message = ex.Message,
                        Details = new Dictionary<string, object>
                        {
                            { "available", ex.Available },
                            { "required", ex.Required },
                            { "deficit", ex.Required - ex.Available }
                        }
                    },
                    CorrelationId = correlationId
                };
                return BadRequest(errorResponse);
            }
            catch (AccountNotFoundException ex)
            {
                var errorResponse = new ApiResponse<CreateTransferResponse>
                {
                    Success = false,
                    StatusCode = 404,
                    Error = new ErrorDetails
                    {
                        Code = "ACCOUNT_NOT_FOUND",
                        Message = ex.Message
                    },
                    CorrelationId = correlationId
                };
                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando transferencia");

                var errorResponse = new ApiResponse<CreateTransferResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Error = new ErrorDetails
                    {
                        Code = "INTERNAL_SERVER_ERROR",
                        Message = "Error interno del servidor"
                    },
                    CorrelationId = correlationId
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
```

---

### FASE 6: Configuración de Program.cs

#### Paso 6.1: Crear `Program.cs`

```csharp
using BankingApi.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddLogging();

// Registrar servicios de negocio como Singletons
// (Singleton mantiene los datos en-memoria durante toda la vida de la aplicación)
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<ITransferService, TransferService>();

// Agregar Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banking API - MVP",
        Version = "1.0.0",
        Description = "API bancaria minimalista con 2 operaciones"
    });
    
    // Incluir comentarios XML
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "BankingApi.xml");
    if (File.Exists(xmlFile))
    {
        c.IncludeXmlComments(xmlFile);
    }
});

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking API v1.0");
    });
}

// CORS permisivo (solo para desarrollo)
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Log inicio
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🏦 Banking API iniciada. Disponible en http://localhost:5000/swagger");

app.Run();
```

---

### FASE 7: Configuración de appsettings.json

#### Paso 7.1: Crear `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

---

### FASE 8: Pruebas Unitarias

#### Paso 8.1: Crear proyecto de tests

**Archivo:** `BankingApi.Tests.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="xunit" Version="2.6.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.1" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="Moq" Version="4.18.4" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="../BankingApi/BankingApi.csproj" />
  </ItemGroup>

</Project>
```

#### Paso 8.2: Crear `Services/TransferServiceTests.cs`

```csharp
using Xunit;
using BankingApi.Services;
using BankingApi.Models;
using BankingApi.Exceptions;

namespace BankingApi.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para TransferService.
    /// Prueba todas las validaciones de negocio (RB-001 a RB-005).
    /// </summary>
    public class TransferServiceTests
    {
        private readonly IAccountService _accountService;
        private readonly ITransferService _transferService;

        public TransferServiceTests()
        {
            _accountService = new AccountService();
            _transferService = new TransferService(_accountService);
        }

        [Fact]
        public async Task ProcessTransfer_WithValidData_ShouldCompleteSuccessfully()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-001",
                TargetAccountId = "ACC-002",
                Amount = 250.00m,
                Concept = "Pago de servicios"
            };

            var sourceBalanceBefore = await _accountService.GetBalanceAsync("ACC-001");
            var targetBalanceBefore = await _accountService.GetBalanceAsync("ACC-002");

            // ACT
            var transfer = await _transferService.ProcessTransferAsync(request);

            // ASSERT
            Assert.NotNull(transfer);
            Assert.Equal("COMPLETED", transfer.Status);
            Assert.Equal(request.Amount, transfer.Amount);

            // Verificar que los saldos se actualizaron correctamente
            var sourceBalanceAfter = await _accountService.GetBalanceAsync("ACC-001");
            var targetBalanceAfter = await _accountService.GetBalanceAsync("ACC-002");

            Assert.Equal(sourceBalanceBefore - 250.00m, sourceBalanceAfter);
            Assert.Equal(targetBalanceBefore + 250.00m, targetBalanceAfter);
        }

        [Fact]
        public async Task ProcessTransfer_WithInvalidAmount_ShouldThrowInvalidAmountException()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-001",
                TargetAccountId = "ACC-002",
                Amount = -50.00m
            };

            // ACT & ASSERT
            await Assert.ThrowsAsync<InvalidAmountException>(
                () => _transferService.ProcessTransferAsync(request));
        }

        [Fact]
        public async Task ProcessTransfer_WithZeroAmount_ShouldThrowInvalidAmountException()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-001",
                TargetAccountId = "ACC-002",
                Amount = 0.00m
            };

            // ACT & ASSERT
            await Assert.ThrowsAsync<InvalidAmountException>(
                () => _transferService.ProcessTransferAsync(request));
        }

        [Fact]
        public async Task ProcessTransfer_WithInsufficientFunds_ShouldThrowInsufficientFundsException()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-002", // Tiene $500
                TargetAccountId = "ACC-001",
                Amount = 1000.00m // Solicita $1000
            };

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<InsufficientFundsException>(
                () => _transferService.ProcessTransferAsync(request));

            Assert.Equal(500.00m, exception.Available);
            Assert.Equal(1000.00m, exception.Required);
        }

        [Fact]
        public async Task ProcessTransfer_WithSameSourceAndTarget_ShouldThrowSameAccountTransferException()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-001",
                TargetAccountId = "ACC-001", // Misma cuenta
                Amount = 100.00m
            };

            // ACT & ASSERT
            await Assert.ThrowsAsync<SameAccountTransferException>(
                () => _transferService.ProcessTransferAsync(request));
        }

        [Fact]
        public async Task ProcessTransfer_WithNonexistentSourceAccount_ShouldThrowAccountNotFoundException()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-999", // No existe
                TargetAccountId = "ACC-001",
                Amount = 100.00m
            };

            // ACT & ASSERT
            await Assert.ThrowsAsync<AccountNotFoundException>(
                () => _transferService.ProcessTransferAsync(request));
        }

        [Fact]
        public async Task ProcessTransfer_WithNonexistentTargetAccount_ShouldThrowAccountNotFoundException()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-001",
                TargetAccountId = "ACC-999", // No existe
                Amount = 100.00m
            };

            // ACT & ASSERT
            await Assert.ThrowsAsync<AccountNotFoundException>(
                () => _transferService.ProcessTransferAsync(request));
        }

        [Fact]
        public async Task ProcessTransfer_ShouldBeAtomic()
        {
            // ARRANGE
            var request = new CreateTransferRequest
            {
                SourceAccountId = "ACC-001",
                TargetAccountId = "ACC-002",
                Amount = 100.00m
            };

            var sourceBalanceBefore = await _accountService.GetBalanceAsync("ACC-001");
            var targetBalanceBefore = await _accountService.GetBalanceAsync("ACC-002");

            // ACT
            var transfer = await _transferService.ProcessTransferAsync(request);

            // ASSERT - Verificar que ambas cuentas se actualizaron
            var sourceBalanceAfter = await _accountService.GetBalanceAsync("ACC-001");
            var targetBalanceAfter = await _accountService.GetBalanceAsync("ACC-002");

            // La suma total debe mantenerse igual (invariante)
            var totalBefore = sourceBalanceBefore + targetBalanceBefore;
            var totalAfter = sourceBalanceAfter + targetBalanceAfter;

            Assert.Equal(totalBefore, totalAfter);
        }
    }
}
```

---

## 5. COMPILACIÓN Y EJECUCIÓN

### Paso 5.1: Compilar el proyecto

```bash
cd src/BankingApi

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Esperado:
# Build succeeded
```

### Paso 5.2: Ejecutar pruebas

```bash
# Correr tests (desde raíz o carpeta tests)
dotnet test

# Esperado:
# Test Run Successful.
# Total tests: 8
# Passed: 8 (100%)
```

### Paso 5.3: Ejecutar la API

```bash
cd src/BankingApi/BankingApi

# Ejecutar aplicación
dotnet run

# Esperado:
# info: BankingApi.Program[0]
#      🏦 Banking API iniciada. Disponible en http://localhost:5000/swagger
```

### Paso 5.4: Acceder a Swagger

Abre en navegador:
```
http://localhost:5000/swagger
```

Deberías ver:
- 2 endpoints listados
- Documentación XML
- Botones para probar

---

## 6. TESTING MANUAL CON CURL

### Test 1: Consultar Saldo Inicial

```bash
curl -X GET "http://localhost:5000/api/v1/accounts/ACC-001/balance" \
  -H "Content-Type: application/json"
```

**Respuesta esperada:**
```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "accountId": "ACC-001",
    "accountOwner": "Juan Pérez",
    "balance": 1000.00,
    "currency": "USD"
  }
}
```

### Test 2: Transferencia Exitosa

```bash
curl -X POST "http://localhost:5000/api/v1/transfers" \
  -H "Content-Type: application/json" \
  -d '{
    "sourceAccountId": "ACC-001",
    "targetAccountId": "ACC-002",
    "amount": 250.00,
    "concept": "Pago de servicios"
  }'
```

**Respuesta esperada:**
```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "transferId": "TRF-001",
    "status": "COMPLETED",
    "sourceBalanceAfter": 750.00,
    "targetBalanceAfter": 750.00
  }
}
```

### Test 3: Transferencia Fallida (Saldo insuficiente)

```bash
curl -X POST "http://localhost:5000/api/v1/transfers" \
  -H "Content-Type: application/json" \
  -d '{
    "sourceAccountId": "ACC-003",
    "targetAccountId": "ACC-001",
    "amount": 100.00
  }'
```

**Respuesta esperada:**
```json
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "INSUFFICIENT_FUNDS",
    "message": "Saldo insuficiente"
  }
}
```

---

## 7. CHECKLIST DE COMPLETITUD

### Código

- [ ] ✅ Estructura de carpetas creada (Models, Controllers, Services, Exceptions)
- [ ] ✅ Models definidos (Account, Transfer, Responses)
- [ ] ✅ Excepciones personalizadas (4 excepciones)
- [ ] ✅ Services implementados (AccountService, TransferService)
- [ ] ✅ Controllers implementados (2 endpoints)
- [ ] ✅ Program.cs configurado
- [ ] ✅ ConcurrentDictionary para almacenamiento
- [ ] ✅ Seed data cargada (3 cuentas)
- [ ] ✅ Validaciones RB-001 a RB-005 implementadas
- [ ] ✅ Atomicidad garantizada

### Testing

- [ ] ✅ xUnit proyecto creado
- [ ] ✅ 8 tests para TransferService
- [ ] ✅ Happy path: `ProcessTransfer_WithValidData`
- [ ] ✅ Error: Monto inválido (negativo)
- [ ] ✅ Error: Monto cero
- [ ] ✅ Error: Saldo insuficiente
- [ ] ✅ Error: Misma cuenta origen/destino
- [ ] ✅ Error: Cuenta origen no existe
- [ ] ✅ Error: Cuenta destino no existe
- [ ] ✅ Atomicidad verificada
- [ ] ✅ Todos los tests PASAN (100%)

### API

- [ ] ✅ GET /api/v1/accounts/{accountId}/balance funciona
- [ ] ✅ POST /api/v1/transfers funciona
- [ ] ✅ Respuesta 200 con datos correctos
- [ ] ✅ Respuesta 404 para cuenta no existe
- [ ] ✅ Respuesta 400 para errores de validación
- [ ] ✅ Correlation ID presente
- [ ] ✅ Documentación XML cargada

### Swagger

- [ ] ✅ Swagger accesible en /swagger
- [ ] ✅ 2 endpoints documentados
- [ ] ✅ Ejemplos de request/response
- [ ] ✅ Botones "Try it out" funcionales

### Compilación y Ejecución

- [ ] ✅ `dotnet build` sin errores
- [ ] ✅ `dotnet test` - todos PASAN
- [ ] ✅ `dotnet run` inicia correctamente
- [ ] ✅ Seed data (ACC-001, ACC-002, ACC-003) disponible
- [ ] ✅ 7 ejemplos cURL todos PASAN

---

## 8. PUNTOS IMPORTANTES DE IMPLEMENTACIÓN

### Sobre ConcurrentDictionary

```csharp
// ✅ CORRECTO: Maneja concurrencia automáticamente
private readonly ConcurrentDictionary<string, Account> _accounts;

// ❌ INCORRECTO: No es thread-safe
private Dictionary<string, Account> _accounts;
```

### Sobre Singleton

```csharp
// ✅ CORRECTO: Mantiene datos durante toda la aplicación
builder.Services.AddSingleton<IAccountService, AccountService>();

// ❌ INCORRECTO: Crea nueva instancia cada request
builder.Services.AddTransient<IAccountService, AccountService>();
```

### Sobre Seed Data

```csharp
// ✅ CORRECTO: En constructor de AccountService
public AccountService()
{
    _accounts = new ConcurrentDictionary<string, Account>();
    InitializeAccounts(); // Seed data
}

// ✅ RESULTADO: Al iniciar app, 3 cuentas ya existen
```

### Sobre Atomicidad

```csharp
// ✅ CORRECTO: Ambas cuentas se actualizan juntas
sourceAccount.Balance -= amount;
targetAccount.Balance += amount;

// ❌ INCORRECTO: Si falla entre actualizaciones
update(sourceAccount);
// <-- error aquí, targetAccount no actualizada
update(targetAccount);
```

---

## 9. ESTIMACIÓN DE TIEMPO

| Fase | Tarea | Tiempo |
|------|-------|--------|
| 1 | Setup proyecto y carpetas | 15 min |
| 2 | Modelos de datos | 20 min |
| 3 | Excepciones | 10 min |
| 4 | Services | 45 min |
| 5 | Controllers | 30 min |
| 6 | Program.cs y config | 15 min |
| 7 | Tests unitarios | 40 min |
| 8 | Compilación y debugging | 20 min |
| 9 | Testing manual con cURL | 15 min |
| **Total** | | **210 min = 3.5 horas** |

---

## 10. PRÓXIMAS MEJORAS (FUERA DE SCOPE)

Estas características NO están en este MVP pero serían para versiones futuras:

- [ ] Autenticación JWT
- [ ] Base de datos real (SQL Server, PostgreSQL)
- [ ] Historial de transacciones
- [ ] Límites diarios de transferencia
- [ ] Comisiones por transferencia
- [ ] Rate limiting
- [ ] Validación avanzada con FluentValidation
- [ ] Logging estructurado con Serilog
- [ ] Métricas y APM

---

## 11. NOTAS IMPORTANTES

1. **Datos se pierden al reiniciar:** Los datos en-memoria NO persisten. Esto es aceptable para un MVP educativo.

2. **Thread-safe por defecto:** ConcurrentDictionary maneja la concurrencia automáticamente.

3. **Seed data siempre**: Cada vez que inicia la app, las 3 cuentas se crean nuevas.

4. **Sin validación avanzada:** Este MVP usa validación básica. Versiones futuras pueden usar FluentValidation.

5. **CORS permisivo:** El CORS está permitido para desarrollo. En producción, ser más restrictivo.

---

**Siguiente paso:** Seguir este plan paso a paso para implementar la API en 3-4 horas.
