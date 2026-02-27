# 📋 ESPECIFICACIÓN CONSTITUCIONAL - API BANCARIA EMPRESARIAL
**Versión:** 1.0  
**Fecha:** 27 de febrero de 2026  
**Propósito:** Documento fundamental que establece las reglas, políticas y estándares del proyecto.

---

## 1. POLÍTICA DE IDIOMA

### Regla Fundacional: Separación Lingüística Estricta

| Aspecto | Idioma | Justificación |
|--------|--------|---------------|
| **Documentación** | Español | Accesibilidad para equipos hispanos |
| **Código Fuente** | Inglés | Estándar internacional de desarrollo |
| **Clases & Métodos** | Inglés | Compatibilidad con .NET Framework |
| **Variables** | Inglés | Convención C# |
| **Enums** | Inglés | Identificadores técnicos |
| **API Routes** | Inglés | Endpoints públicos universales |
| **Base de Datos** | Inglés | Esquema agnóstico al idioma |
| **Comentarios de Código** | Español | Facilitar comprensión del equipo |

**Ejemplo de Cumplimiento:**
```csharp
/// <summary>
/// Procesa una transferencia bancaria entre cuentas.
/// </summary>
public async Task<TransferResponse> ProcessTransfer(TransferRequest request)
{
    // Validar que la cuenta origen tiene fondos suficientes
    if (sourceAccount.Balance < request.Amount)
        throw new InsufficientFundsException("Saldo insuficiente");
    
    return await _transferService.ExecuteAsync(request);
}
```

---

## 2. ESTRUCTURA DE CARPETAS

### Estructura Mandatoria Desde Raíz del Repositorio

```
banking-speckit-dotnet-lab/
├── src/                              # 🔵 TODO el código fuente
│   └── BankingApi/
│       ├── BankingApi.sln           # Solución .NET
│       ├── BankingApi/              # Proyecto principal (WebApi)
│       │   ├── Program.cs
│       │   ├── appsettings.json
│       │   ├── Controllers/         # Controladores REST
│       │   ├── Services/            # Lógica de negocio
│       │   ├── Domain/              # Entidades de dominio
│       │   ├── Infrastructure/      # Acceso a datos, logging
│       │   ├── Middlewares/         # Middleware personalizado
│       │   └── Extensions/          # Métodos de extensión
│       ├── BankingApi.Domain/       # Proyecto de dominio (entidades, interfaces)
│       ├── BankingApi.Application/  # Proyecto de aplicación (servicios)
│       ├── BankingApi.Infrastructure/ # Proyecto de infraestructura (datos, logging)
│       ├── BankingApi.Tests/        # Proyecto de pruebas unitarias
│       └── BankingApi.IntegrationTests/ # Pruebas de integración
├── docs/                            # Documentación (en español)
│   ├── API_GUIDE.md
│   ├── GETTING_STARTED.md
│   └── TROUBLESHOOTING.md
├── .github/                         # Configuración de GitHub
├── .specify/                        # Configuración de SpecKit
│   └── memory/
│       ├── project.md               # Contexto del proyecto
│       └── decisions.md             # ADRs (Architectural Decision Records)
├── SPECKIT-CONSTITUTION.md          # 📌 Este documento
├── README.md
└── .gitignore
```

**Regla Crítica:** ❌ Ningún código fuente fuera de `src/BankingApi/`

---

## 3. POLÍTICA DE SEGURIDAD (Laboratorio)

### Alcance de Seguridad Reducida
Este es un **entorno de aprendizaje**. Se omiten deliberadamente:

| Componente | Estado | Razón |
|-----------|--------|-------|
| **Autenticación** | ❌ NO IMPLEMENTAR | Focus en lógica de negocio |
| **Autorización** | ❌ NO IMPLEMENTAR | Sin roles de usuario |
| **HTTPS** | ❌ NO REQUERIDO | HTTP suficiente para lab |
| **Cifrado de Datos** | ❌ NO IMPLEMENTAR | Texto plano aceptado |
| **CORS** | ⚠️ PERMISIVO | Permitir todos los orígenes |

### Enfoque en Seguridad de Negocio

✅ **OBLIGATORIO Implementar:**
- Validaciones estrictas en transacciones (ej: saldo suficiente, montos válidos)
- Reglas de negocio robustas (ej: no permitir transferencias negativas)
- Logging de todas las operaciones críticas
- Correlation IDs para trazabilidad completa

**Ejemplo de Validación de Negocio:**
```csharp
public class TransferValidator
{
    public ValidationResult Validate(TransferRequest request)
    {
        if (request.Amount <= 0)
            return ValidationResult.Fail("La cantidad debe ser mayor a cero");
        
        if (request.SourceAccountId == request.TargetAccountId)
            return ValidationResult.Fail("No se puede transferir a la misma cuenta");
        
        if (request.Amount > 1_000_000)
            return ValidationResult.Fail("Límite diario excedido");
        
        return ValidationResult.Success();
    }
}
```

---

## 4. LOGGING ESTRUCTURADO Y TRAZABILIDAD

### Política de Logging Obligatorio

Todas las operaciones críticas DEBEN ser registradas con **Correlation ID** para trazabilidad.

### Niveles de Log
```
[TRACE]   Detalles más granulares (entrada/salida de métodos internos)
[DEBUG]   Información de depuración (valores de variables)
[INFO]    Eventos importantes (login, transferencia iniciada)
[WARN]    Situaciones inusuales (saldo bajo, timeout)
[ERROR]   Errores que afectan funcionamiento
[FATAL]   Fallos críticos que detienen la aplicación
```

### Implementación Requerida
- **Librería:** Serilog con enrichers para Correlation ID
- **Formato:** JSON Estructurado
- **Destinos:** Console + Archivo rotatório

**Ejemplo de Implementación:**
```csharp
// Program.cs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("CorrelationId", httpContext?.TraceIdentifier)
    .WriteTo.Console()
    .WriteTo.File("logs/banking-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Uso en servicios
logger.LogInformation(
    "Transferencia procesada: {TransferId} | Origen: {Source} | Destino: {Destination} | Monto: {Amount}",
    transferId, sourceAccount, targetAccount, amount
);
```

### Middleware para Correlation ID
```csharp
app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers.ContainsKey("X-Correlation-ID")
        ? context.Request.Headers["X-Correlation-ID"].ToString()
        : Guid.NewGuid().ToString();
    
    using (LogContext.PushProperty("CorrelationId", correlationId))
    {
        context.Response.Headers.Add("X-Correlation-ID", correlationId);
        await next();
    }
});
```

---

## 5. VALIDACIONES ESTRICTAS EN EL DOMINIO

### Principio: Domain-Driven Design (DDD)

Todas las validaciones de negocio DEBEN residir en la capa **Domain**, no en controllers ni en UI.

### Validaciones Obligatorias por Dominio

#### 5.1 Cuentas Bancarias (`Account` Entity)
✅ VALIDAR:
- Número de cuenta: alfanumérico, 10-20 caracteres
- Saldo: nunca negativo
- Tipo de cuenta: SAVINGS, CHECKING, INVESTMENT
- Estado: ACTIVE, INACTIVE, SUSPENDED, CLOSED
- No eliminar cuentas con saldo > 0
- No permitir operaciones en cuentas inactivas

#### 5.2 Transferencias (`Transfer` Entity)
✅ VALIDAR:
- Cantidad > 0
- No transferir a la misma cuenta
- Cuenta origen debe existir y estar ACTIVE
- Cuenta destino debe existir
- Saldo origen >= cantidad + comisiones
- Límite diario: máx $10,000 por cuenta
- Límite número de transferencias: máx 20 por día
- Concepto: máx 100 caracteres, sin caracteres especiales peligrosos

#### 5.3 Depósitos (`Deposit` Entity)
✅ VALIDAR:
- Cantidad > 0
- Máximo $100,000 por depósito
- Cuenta destino estar ACTIVE
- Tipo de depósito válido (CASH, CHECK, WIRE)

#### 5.4 Retiros (`Withdrawal` Entity)
✅ VALIDAR:
- Cantidad > 0
- Saldo suficiente
- Retiro máximo: $5,000 por transacción
- Máximo 3 retiros por día
- Intervalo mínimo: 1 hora entre retiros

### Implementación Correcta (DDD)

```csharp
// ❌ INCORRECTO: Validación en Web API
[HttpPost("transfer")]
public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
{
    if (request.Amount < 0) // ❌ LÓGICA EN CONTROLLER
        return BadRequest("Monto inválido");
    
    await _transferService.ProcessAsync(request);
    return Ok();
}

// ✅ CORRECTO: Validación en Entidad de Dominio
public class Transfer : Entity
{
    public Transfer(Account source, Account target, decimal amount, string concept)
    {
        if (amount <= 0)
            throw new DomainException("La cantidad debe ser mayor a cero");
        
        if (source.Id == target.Id)
            throw new DomainException("No se puede transferir a la misma cuenta");
        
        if (source.Balance < amount)
            throw new InsufficientFundsException($"Saldo insuficiente: {source.Balance}");
        
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Target = target ?? throw new ArgumentNullException(nameof(target));
        Amount = amount;
        Concept = concept ?? string.Empty;
        Status = TransferStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }
    
    public Account Source { get; private set; }
    public Account Target { get; private set; }
    public decimal Amount { get; private set; }
    public string Concept { get; private set; }
    public TransferStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
}

// Servicio de Aplicación: orquestación
public class TransferService : ITransferService
{
    private readonly ITransferRepository _repository;
    private readonly ILogger<TransferService> _logger;
    
    public async Task<TransferResponse> ProcessAsync(TransferRequest request)
    {
        try
        {
            var source = await _repository.GetAccountAsync(request.SourceAccountId);
            var target = await _repository.GetAccountAsync(request.TargetAccountId);
            
            // Crear instancia: constructor valida reglas de negocio
            var transfer = new Transfer(source, target, request.Amount, request.Concept);
            
            // Persistir
            await _repository.SaveTransferAsync(transfer);
            
            _logger.LogInformation(
                "Transferencia completada: {TransferId} | {Amount} de {Source} a {Target}",
                transfer.Id, transfer.Amount, source.Number, target.Number
            );
            
            return new TransferResponse { Id = transfer.Id, Status = "COMPLETED" };
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Error de lógica de negocio: {Message}", ex.Message);
            throw;
        }
    }
}
```

---

## 6. ESTÁNDARES DE CÓDIGO C#

### Principios SOLID

| Principio | Descripción | Ejemplo |
|-----------|-------------|---------|
| **S**ingle Responsibility | Una clase = una responsabilidad | `TransferValidator` solo valida transferencias |
| **O**pen/Closed | Abierto extensión, cerrado modificación | Usar interfaces `ITransferService` |
| **L**iskov Substitution | Subclases reemplazan base sin fallos | `CreditAccount` como `Account` |
| **I**nterface Segregation | Interfaces específicas, no generales | `ITransferRepository`, no `IRepository<T>` |
| **D**ependency Inversion | Depender de abstracciones, no concretos | Inyectar `ILogger` no `ConsoleLogger` |

### Convenciones de Nomenclatura Obligatorias

```csharp
// ✅ Clases: PascalCase
public class CustomerAccount { }

// ✅ Métodos: PascalCase
public async Task<TransferResponse> ProcessTransfer() { }

// ✅ Variables locales: camelCase
var accountBalance = 5000m;

// ✅ Constantes: UPPER_SNAKE_CASE
private const int MAX_TRANSFER_AMOUNT = 1_000_000;

// ✅ Propiedades: PascalCase
public decimal Balance { get; private set; }

// ✅ Campos privados: _camelCase
private readonly ILogger _logger;

// ✅ Interfaces: IPascalCase
public interface ITransferService { }

// ✅ Enums: PascalCase (elementos también)
public enum TransferStatus
{
    Pending,
    Completed,
    Failed,
    Cancelled
}
```

### Clean Code - Reglas Obligatorias

1. **Nombres Descriptivos**
   ```csharp
   ❌ int d; // ¿Qué es d?
   ✅ int daysSinceLastTransaction;
   ```

2. **Métodos Pequeños (máx 20 líneas)**
   ```csharp
   ❌ UniversalProcessMethod() { ... 100 líneas ... }
   ✅ ProcessTransfer() { ... 15 líneas, llama a métodos específicos ... }
   ```

3. **No Parámetros Booleanos**
   ```csharp
   ❌ public void ProcessAccount(bool isActive) { }
   ✅ public void ProcessActiveAccount() { } // o ProcessInactiveAccount()
   ```

4. **Early Returns (evitar anidación)**
   ```csharp
   ❌ if (isValid) { if (hasBalance) { if (isActive) { /* logica */ } } }
   ✅ if (!isValid) return;
      if (!hasBalance) return;
      if (!isActive) return;
      /* logica */
   ```

5. **Máxima Complejidad Ciclomática: 5**
   ```csharp
   ✅ Si un método requiere >5 condiciones, es demasiado complejo.
      Refactorizar en métodos más pequeños.
   ```

---

## 7. PRUEBAS UNITARIAS OBLIGATORIAS

### Cobertura Requerida: **≥80%**

### Framework de Testing
- **Framework:** xUnit
- **Mocking:** Moq
- **Assertions:** FluentAssertions

### Estructura de Pruebas: Arrange-Act-Assert (AAA)

```csharp
// ✅ CORRECTO
[Fact]
public async Task ProcessTransfer_WithValidData_ShouldCompleteSuccessfully()
{
    // ARRANGE
    var sourceAccount = new Account(id: 1, balance: 5000m);
    var targetAccount = new Account(id: 2, balance: 1000m);
    var transferService = new TransferService(
        mockRepository: _mockRepository.Object,
        mockLogger: _mockLogger.Object
    );
    
    var request = new TransferRequest
    {
        SourceAccountId = 1,
        TargetAccountId = 2,
        Amount = 500m,
        Concept = "Pago de servicios"
    };
    
    // ACT
    var result = await transferService.ProcessAsync(request);
    
    // ASSERT
    Assert.NotNull(result);
    Assert.Equal(TransferStatus.Completed, result.Status);
    Assert.Equal(500m, result.Amount);
    _mockRepository.Verify(x => x.SaveTransferAsync(It.IsAny<Transfer>()), Times.Once);
}

// ❌ INCORRECTO (sin estructura clara)
[Fact]
public void TestTransfer()
{
    var svc = new TransferService(repo, logger);
    var req = new TransferRequest { Amount = 500 };
    var res = svc.Process(req);
    if (res.Status == "OK") { Assert.True(true); }
}
```

### Casos de Prueba Obligatorios

#### Transferencias (`TransferService.Tests`)
- ✅ Happy Path: transferencia válida se completa
- ✅ Saldo insuficiente: lanza `InsufficientFundsException`
- ✅ Monto inválido: lanza `DomainException`
- ✅ Misma cuenta origen/destino: lanza `DomainException`
- ✅ Cuenta origen no existe: lanza `AccountNotFoundException`
- ✅ Cuenta origen inactiva: lanza `InactiveAccountException`
- ✅ Excede límite diario: lanza `DailyLimitExceededException`

#### Cuentas (`AccountRepository.Tests`)
- ✅ Crear cuenta nueva: se guarda correctamente
- ✅ Obtener cuenta por ID: retorna cuenta válida
- ✅ Actualizar balance: persiste cambios
- ✅ Cuenta inexistente: lanza excepción

---

## 8. SWAGGER Y DOCUMENTACIÓN API

### Swagger Mandatorio

✅ **OBLIGATORIO:**
- OpenAPI 3.0 habilitado
- Todos los endpoints documentados
- Ejemplos de Request/Response
- Códigos HTTP documentados

### Configuración Requerida en `Program.cs`

```csharp
// Agregar Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banking API - Laboratorio SpecKit",
        Version = "1.0.0",
        Description = "API bancaria empresarial para propósitos educativos",
        Contact = new OpenApiContact
        {
            Name = "Equipo SpecKit",
            Email = "team@speckit.banking"
        }
    });
    
    // Incluir archivos XML de documentación
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Verificar Swagger esté disponible en producción
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "Banking API v1.0");
});
```

### Documentación XML Obligatoria

```csharp
/// <summary>
/// Procesa una transferencia bancaria entre dos cuentas.
/// </summary>
/// <param name="request">Solicitud de transferencia con cuentas origen/destino y monto</param>
/// <returns>Respuesta con ID de transferencia y estado</returns>
/// <exception cref="InsufficientFundsException">El saldo origen es insuficiente</exception>
/// <exception cref="DomainException">Error en las reglas de negocio (ej: monto inválido)</exception>
[HttpPost("transfers")]
[ProducesResponseType(typeof(TransferResponse), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public async Task<IActionResult> PostTransfer([FromBody] TransferRequest request)
{
    var result = await _transferService.ProcessAsync(request);
    return Ok(result);
}
```

---

## 9. DEFINITION OF DONE (DoD) - Criterios de Aceptación

**Ninguna tarea se considera COMPLETADA hasta que cumple TODOS estos criterios:**

### Antes de Crear PR

- [ ] ✅ Código compila sin errores
- [ ] ✅ Código cumple estándares SOLID y Clean Code
- [ ] ✅ Pruebas unitarias escritas y TODAS PASAN
- [ ] ✅ Cobertura de código ≥80%
- [ ] ✅ Sin comentarios `TODO` o `FIXME` sin resolver
- [ ] ✅ Documentación XML en métodos públicos
- [ ] ✅ Validaciones de dominio implementadas
- [ ] ✅ Logging estructurado en operaciones críticas
- [ ] ✅ Correlation IDs en flujos transversales

### Durante Revisión (Code Review)

- [ ] ✅ Mínimo 1 aprobación de otro miembro del equipo
- [ ] ✅ Pruebas unitarias tienen buena cobertura
- [ ] ✅ Nombre de variables es descriptivo
- [ ] ✅ No hay duplicación de código (`DRY` - Don't Repeat Yourself)
- [ ] ✅ Métodos no exceden 20 líneas
- [ ] ✅ Complejidad ciclomática ≤ 5

### Antes de Merge a Main

- [ ] ✅ Todos los comentarios de revisión están resueltos
- [ ] ✅ Branch está actualizado a main (`git rebase`)
- [ ] ✅ Pipeline CI/CD ✅ PASA (compilación + tests)
- [ ] ✅ Sin conflictos de merge

### Después del Merge

- [ ] ✅ Documentación en `docs/` está actualizada
- [ ] ✅ Swagger captura nuevos endpoints
- [ ] ✅ README.md actualizado si hay cambios estructurales
- [ ] ✅ Changelog (CHANGELOG.md) actualizado

---

## 10. FLUJO DE DESARROLLO

### Rama Principal: `main`
- Siempre compilable y deployable
- Protegida: requiere PR + revisión + CI/CD pass
- Tags de versión semántica: `v1.0.0`, `v1.1.0`, etc.

### Ramas de Feature: `feature/description`
```bash
git checkout -b feature/implement-transfer-validation
# ... desarrollar ...
git push origin feature/implement-transfer-validation
# Crear PR
```

### Naming Commits
```
✅ CORRECTO:
  - feat: Implement transfer validation
  - fix: Correct insufficient funds exception
  - docs: Add API guide for transfers
  - test: Add unit tests for TransferService

❌ INCORRECTO:
  - fixed stuff
  - update codes
  - blablabla
```

---

## 11. HERRAMIENTAS Y VERSIONES

| Herramienta | Versión Mínima | Propósito |
|-------------|-----------------|----------|
| **.NET SDK** | 8.0 | Runtime y compilación |
| **C#** | 12.0 | Lenguaje de programación |
| **xUnit** | 2.6+ | Framework de testing |
| **Moq** | 4.18+ | Mocking en pruebas |
| **Serilog** | 3.1+ | Logging estructurado |
| **Swashbuckle** | 6.4+ | Swagger/OpenAPI |
| **Entity Framework Core** | 8.0+ | ORM (opcional) |

---

## 12. MATRIZ DE RESPONSABILIDADES (RACI)

| Área | Responsable | Accountable | Consultar | Informar |
|------|-------------|-------------|-----------|----------|
| Arquitectura | Arquitecto | Lead técnico | Team | Stakeholders |
| Código fuente | Developer | Code reviewer | QA | Team |
| Pruebas | Developer + QA | QA Lead | Team | Team |
| Documentación | Developer | PM | Arquitecto | Team |
| Deploy | DevOps | Tech Lead | Developer | Team |

---

## 13. CONVENCIONES DE API REST

### Endpoints Obligatorios por Recurso

#### Cuentas
```
GET    /api/v1/accounts              # Listar todas
POST   /api/v1/accounts              # Crear nueva
GET    /api/v1/accounts/{id}         # Obtener por ID
PUT    /api/v1/accounts/{id}         # Actualizar
DELETE /api/v1/accounts/{id}         # Eliminar (lógico)
```

#### Transferencias
```
GET    /api/v1/transfers             # Listar todas
POST   /api/v1/transfers             # Crear nueva
GET    /api/v1/transfers/{id}        # Obtener por ID
GET    /api/v1/transfers/account/{id} # Por cuenta origen
```

#### Depósitos
```
GET    /api/v1/deposits              # Listar
POST   /api/v1/deposits              # Crear
GET    /api/v1/deposits/{id}         # Obtener
```

### Estructura de Respuesta Estándar

```json
// Éxito
{
  "success": true,
  "statusCode": 200,
  "data": { /* payload */ },
  "timestamp": "2026-02-27T10:30:00Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}

// Error
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "INSUFFICIENT_FUNDS",
    "message": "Saldo insuficiente para completar la transferencia",
    "details": {
      "available": 1000,
      "required": 5000
    }
  },
  "timestamp": "2026-02-27T10:30:00Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

---

## 14. MANTENIMIENTO CONSTITUCIONAL

### Revisión y Actualización
- **Cadencia:** Cada sprint (cada 2 semanas)
- **Criterio:** Cambios arquitectónicos requieren enmienda constitucional
- **Votación:** Consenso del equipo técnico
- **Historia:** Cada cambio documentado con fecha y justificación

### Enmiendas Registradas
| Versión | Fecha | Cambio | Autor |
|---------|-------|--------|-------|
| 1.0 | 2026-02-27 | Constitución inicial | Arquitecto SpecKit |

---

## 📌 REFERENCIAS Y LINKS ÚTILES

- [SOLID Principles in C#](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles)
- [Clean Code by Robert C. Martin](https://www.oreilly.com/library/view/clean-code-a/9780136083238/)
- [Domain-Driven Design](https://vaughnvernon.com/domain-driven-design/)
- [Serilog Documentation](https://serilog.net/)
- [OpenAPI/Swagger](https://swagger.io/resources/articles/best-practices-in-api-design/)

---

## ✅ CHECKLIST DE ADOPCIÓN INICIAL

Como arquitecto, confirma que el proyecto inicia con:

- [ ] Estructura de carpetas `src/BankingApi/` creada
- [ ] `.csproj` configurados con estándares SOLID
- [ ] Logging con Serilog + Correlation ID implementado
- [ ] Pruebas unitarias (xUnit) en setup inicial
- [ ] Swagger habilitado y accesible en `/swagger`
- [ ] Documentación API en `docs/API_GUIDE.md`
- [ ] Definition of Done publicada en equipo
- [ ] Rama `main` protegida en GitHub
- [ ] Este documento revisado por todo el equipo

---

**DOCUMENTO CONSTITUCIONAL EFECTIVO A PARTIR DE:** 27 de febrero de 2026

**"Con código limpio y validaciones fuertes, construimos sistemas confiables."** 🏛️
