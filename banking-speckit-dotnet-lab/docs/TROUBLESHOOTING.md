# 🔧 GUÍA DE RESOLUCIÓN DE PROBLEMAS (Troubleshooting)

**Última actualización:** 27 de febrero de 2026

---

## Tabla de Contenidos
1. [Problemas de Setup](#problemas-de-setup)
2. [Problemas de Compilación](#problemas-de-compilación)
3. [Problemas de Ejecución](#problemas-de-ejecución)
4. [Problemas de Testing](#problemas-de-testing)
5. [Problemas de API](#problemas-de-api)
6. [Problemas de Logging](#problemas-de-logging)
7. [Performance y Debugging](#performance-y-debugging)

---

## Problemas de Setup

### ❌ ".NET SDK not found"

**Síntoma:**
```bash
$ dotnet --version
The term 'dotnet' is not recognized
```

**Solución:**
1. Descargar .NET 8 SDK: https://dotnet.microsoft.com/download
2. Instalar siguiendo instrucciones del SO
3. Reiniciar terminal
4. Verificar: `dotnet --version` (debe mostrar 8.0.x+)

**Verificación Adicional:**
```bash
# En Windows
where dotnet

# En macOS/Linux
which dotnet
```

---

### ❌ "No such file or directory - Git"

**Síntoma:**
```bash
$ git clone https://...
bash: git: command not found
```

**Solución:**

#### Windows
- Descargar Git: https://git-scm.com/download/win
- Ejecutar instalador
- Reiniciar terminal

#### macOS
```bash
# Opción 1: Xcode Command Line Tools
xcode-select --install

# Opción 2: Homebrew
brew install git
```

#### Linux (Ubuntu/Debian)
```bash
sudo apt update
sudo apt install git
```

---

### ❌ "Cannot find path 'src/BankingApi'"

**Síntoma:**
```bash
cd src/BankingApi
cd: can't cd to src/BankingApi: No such file or directory
```

**Causa:** Estás en el directorio incorrecto.

**Solución:**
```bash
# Verifica dónde estás
pwd

# Debes estar en:
# /path/to/RSSFeedReader/banking-speckit-dotnet-lab

# Si estás en RSSFeedReader root:
cd banking-speckit-dotnet-lab
cd src/BankingApi

# Lista el contenido para verificar
ls -la
```

---

## Problemas de Compilación

### ❌ "The project file could not be loaded"

**Síntoma:**
```
error MSB4066: The attribute "Version" in element "Project" is unrecognized
```

**Causa:** Archivo `.csproj` corrupto o mal formateado.

**Solución:**
1. Verifica que el `.csproj` está bien formado (XML válido)
2. Usa editor con soporte XML (VS Code, Visual Studio)
3. Reconstruye proyecto limpio:
```bash
dotnet clean
rm -rf bin obj  # macOS/Linux
rmdir /s bin obj  # Windows
dotnet restore
dotnet build
```

---

### ❌ "Missing NuGet packages"

**Síntoma:**
```
error NU1101: Unable to find package Microsoft.AspNetCore.App
```

**Solución:**
```bash
# Limpiar y restaurar
dotnet clean
dotnet nuget locals all --clear
dotnet restore --no-cache
dotnet build
```

**Si persiste:**
```bash
# Verificar conectividad a NuGet.org
curl -I https://api.nuget.org/v3/index.json

# Si da timeout, intenta con proxy corporativo:
dotnet nuget add source https://api.nuget.org/v3/index.json --name nuget.org
```

---

### ❌ "Duplicate namespace/class declaration"

**Síntoma:**
```
error CS0101: The namespace 'BankingApi.Domain' already contains a definition for 'Account'
```

**Causa:** Dos archivos definen la misma clase.

**Solución:**
```bash
# Busca duplicados
find . -name "Account.cs" | sort

# Elimina el archivo duplicado erróneo
rm path/to/duplicate/Account.cs
```

---

### ❌ "CS0103: The name 'X' does not exist in the current context"

**Síntoma:**
```
error CS0103: The name 'ITransferService' does not exist
```

**Causa:** Falta `using` statement o interfaz no definida.

**Solución:**
1. Añade el `using` statment al archivo:
```csharp
using BankingApi.Services;  // Asegúrate del namespace correcto
```

2. Verifica que la interfaz existe:
```bash
find . -name "*.cs" -exec grep -l "interface ITransferService" {} \;
```

---

## Problemas de Ejecución

### ❌ "Port 5000 already in use"

**Síntoma:**
```
System.IO.IOException: Failed to bind to address http://[::1]:5000: Address already in use
```

**Solución - Opción 1:** Cambiar puerto en línea de comandos
```bash
dotnet run --urls="http://localhost:5001"
```

**Solución - Opción 2:** Cambiar en `appsettings.json`
```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5001"
      }
    }
  }
}
```

**Solución - Opción 3:** Matar proceso que usa el puerto
```bash
# macOS/Linux
sudo lsof -i :5000
kill -9 <PID>

# Windows (PowerShell admin)
Get-Process -Id (Get-NetTCPConnection -LocalPort 5000).OwningProcess | Stop-Process -Force
```

---

### ❌ "ApplicationDbContext does not exist"

**Síntoma:**
```
error CS0246: The type or namespace name 'BankingContext' could not be found
```

**Causa:** Class `BankingContext` (DbContext) no existe aún.

**Solución Temporal (para lab):**
Crea en `Infrastructure/Database/BankingContext.cs`:
```csharp
using Microsoft.EntityFrameworkCore;
using BankingApi.Domain;

namespace BankingApi.Infrastructure.Database
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options) { }
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
```

---

### ❌ "An assembly with the same identity already exists"

**Síntoma:**
```
System.Reflection.AmbiguousMatchException: An assembly with the same name 'BankingApi' already exists
```

**Causa:** Proyectos con mismo nombre pero rutas diferentes.

**Solución:**
```bash
# Limpia completamente
dotnet clean
rm -rf **/bin **/obj
cd src/BankingApi
dotnet build
```

---

## Problemas de Testing

### ❌ "No test assemblies found to run"

**Síntoma:**
```bash
$ dotnet test
No test assemblies found
```

**Causa:** `BankingApi.Tests.csproj` no existe o no está configurado.

**Solución:**
1. Crea archivo `BankingApi.Tests.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Moq" Version="4.18.4" />
    <PackageReference Include="xunit" Version="2.6.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.1" />
    <PackageReference Include="FluentAssertions" Version="6.11.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="../BankingApi/BankingApi.csproj" />
  </ItemGroup>
</Project>
```

2. Crea carpeta `BankingApi.Tests` en `src/BankingApi/`

---

### ❌ "xUnit test framework not available"

**Síntoma:**
```
xunit.runner.visualstudio not found
```

**Solución:**
```bash
cd BankingApi.Tests
dotnet add package xunit --version 2.6.2
dotnet add package xunit.runner.visualstudio --version 2.5.1
dotnet test
```

---

### ❌ "A collection of assertions completed with one or more failures"

**Síntoma:**
```
FluentAssertions exception con múltiples errores
```

**Causa:** Test tiene múltiples assertion failures.

**Solución:**
Dibuja las assertions una por una:
```csharp
// ❌ INCORRECTO (falla en primera, no ve las demás)
result.Should().NotBeNull();
result.Status.Should().Be(TransferStatus.Completed);
result.Amount.Should().Be(500);

// ✅ CORRECTO (ve todos los errores)
result.Should().NotBeNull();
using (new AssertionScope())
{
    result.Status.Should().Be(TransferStatus.Completed);
    result.Amount.Should().Be(500);
    result.SourceAccountId.Should().Be(1);
}
```

---

### ❌ "Test setup fails with NullReferenceException"

**Síntoma:**
```
NullReferenceException: Object reference not set to an instance of an object.
```

**Causa:** Mock no configurado correctamente.

**Solución:**
```csharp
// ❌ INCORRECTO
var mockRepository = new Mock<IAccountRepository>();
var service = new AccountService(mockRepository); // ← Falta .Object

// ✅ CORRECTO
var mockRepository = new Mock<IAccountRepository>();
var service = new AccountService(mockRepository.Object);

// ✅ CON CONFIGURACIÓN
mockRepository
    .Setup(x => x.GetAsync(It.IsAny<int>()))
    .ReturnsAsync(new Account { Id = 1, Balance = 5000 });
```

---

## Problemas de API

### ❌ "Cannot POST /api/v1/transfers - 404 Not Found"

**Síntoma:**
```json
{
  "success": false,
  "statusCode": 404,
  "error": { "code": "NOT_FOUND", "message": "The endpoint does not exist" }
}
```

**Causa:** Controller no existe o ruta mal escrita.

**Solución:**
1. Verifica que `TransfersController.cs` existe en `Controllers/`
2. Verifica ruta del endpoint:
```csharp
[ApiController]
[Route("api/v1/[controller]")]  // ← Debe generar /api/v1/transfers
public class TransfersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TransferRequest request)
    {
        // ...
    }
}
```

3. Verifica que el servicio está registrado en `Program.cs`:
```csharp
builder.Services.AddScoped<ITransferService, TransferService>();
```

---

### ❌ "400 Bad Request - Invalid JSON"

**Síntoma:**
```bash
curl -X POST http://localhost:5000/api/v1/transfers \
  -H "Content-Type: application/json" \
  -d '{"sourceAccountId": 1, "targetAccountId": 2, "amount": 500'  # ← JSON incompleto

# Respuesta
{
  "success": false,
  "statusCode": 400,
  "error": { "code": "INVALID_JSON", "message": "Invalid JSON format" }
}
```

**Solución:**
Valida el JSON:
```bash
# ✅ CORRECTO
echo '{"sourceAccountId": 1, "targetAccountId": 2, "amount": 500, "concept": "test"}' | jq .

# Usa comillas correctamente en bashas
curl -X POST http://localhost:5000/api/v1/transfers \
  -H "Content-Type: application/json" \
  -d '{
    "sourceAccountId": 1,
    "targetAccountId": 2,
    "amount": 500,
    "concept": "test"
  }'
```

---

### ❌ "411 Length Required"

**Síntoma:**
```
411 Length Required
```

**Causa:** Header `Content-Length` faltante (raro en clientes modernos).

**Solución:**
Asegúrate de enviar `Content-Length` o usar `-d` con curl:
```bash
# Curl automáticamente añade Content-Length
curl -X POST http://localhost:5000/api/v1/transfers \
  -H "Content-Type: application/json" \
  -d '{"sourceAccountId": 1, ...}'
```

---

### ❌ "CORS error - Access-Control-Allow-Origin"

**Síntoma:**
```
Access to XMLHttpRequest at 'http://localhost:5000/api/v1/accounts' 
from origin 'http://localhost:3000' has been blocked by CORS policy
```

**Solución:**
Configura CORS en `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()      // ⚠️ Solo para desarrollo
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

app.UseCors();  // Añade antes de MapControllers
```

---

## Problemas de Logging

### ❌ "No log files appear"

**Síntoma:**
```bash
ls -la | grep banking
# No aparece banking-20260227.txt
```

**Causa:** Serilog no configurado en `Program.cs`.

**Solución:**
Configura Serilog en `Program.cs`:
```csharp
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "BankingApi")
    .WriteTo.Console()
    .WriteTo.File(
        "logs/banking-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// ... resto del setup
```

---

### ❌ "Logs are not structured JSON"

**Síntoma:**
```
2026-02-27 10:30:00 [INF] Transfer completed
# Texto plano, no JSON
```

**Causa:** `outputTemplate` en Serilog no es JSON.

**Solución:**
Usa JSON formatter:
```csharp
.WriteTo.File(
    new JsonFormatter(),  // ← Añade JSON formatter
    "logs/banking-.json", // ← Cambiar a .json
    rollingInterval: RollingInterval.Day
)
```

Instala paquete:
```bash
dotnet add package Serilog.Formatting.Compact
```

---

### ❌ "Correlation ID no aparece en logs"

**Síntoma:**
```
2026-02-27 10:30:00 [INF] Transfer completed  # Sin Correlation ID
```

**Causa:** Middleware no implementado o LogContext no usado.

**Solución:**
1. Crea middleware `CorrelationIdMiddleware.cs`:
```csharp
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    
    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers
            .TryGetValue("X-Correlation-ID", out var value)
            ? value.ToString()
            : Guid.NewGuid().ToString();
        
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            context.Response.Headers.Add("X-Correlation-ID", correlationId);
            await _next(context);
        }
    }
}
```

2. Regístralo en `Program.cs`:
```csharp
app.UseMiddleware<CorrelationIdMiddleware>();  // Antes de MapControllers
```

3. Include in Serilog config:
```csharp
.Enrich.FromLogContext()  // ← Esto captura LogContext.PushProperty
```

---

## Performance y Debugging

### ⚠️ "API muy lenta (>1 segundo por request)"

**Diagnóstico:**
```bash
# Medir tiempo de request
time curl -X GET http://localhost:5000/api/v1/accounts

# Con más detalles
curl -w "@curl-format.txt" -o /dev/null -s http://localhost:5000/api/v1/accounts
```

**Causas Comunes:**
1. **N+1 Queries:** Entity Framework cargando relaciones sin `.Include()`
   ```csharp
   // ❌ LENTO: n+1 queries
   var accounts = _context.Accounts.ToList();
   foreach (var acc in accounts) {
       var balance = acc.Transfers.Count(); // ← Otra query por cada account
   }
   
   // ✅ RÁPIDO: eager load
   var accounts = _context.Accounts
       .Include(a => a.Transfers)
       .ToList();
   ```

2. **Logging excesivo:** Cambiar nivel a `Warning`
   ```csharp
   .MinimumLevel.Warning()  // En producción
   ```

3. **Middlewares inútiles:** Deshabilita los no necesarios

4. **Base de datos loca:** Verifica queries con `SQL Profiler`

---

### 🎯 "Debug en VS Code"

Crea `.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "program": "${workspaceFolder}/src/BankingApi/BankingApi/bin/Debug/net8.0/BankingApi.dll",
      "args": [],
      "stopAtEntry": false,
      "serverReadyAction": {
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
        "uriFormat": "{0}",
        "action": "openExternally"
      }
    }
  ]
}
```

Luego: `F5` para debuggear con breakpoints.

---

### 💡 Ver historial completo de logs

```bash
# Último 100 líneas
tail -n 100 banking-20260227.txt

# En tiempo real
tail -f banking-20260227.txt

# Filtrar por nivel
grep "\[ERR\]" banking-*.txt

# Buscar por Correlation ID
grep "550e8400-e29b-41d4-a716-446655440000" banking-*.txt

# Análisis con jq (si logs son JSON)
cat banking-20260227.json | jq '.[] | select(.Level == "Error")'
```

---

## 🆘 Reportar un Problema

Si tu problema no está listado:

1. **Recopila información:**
   ```bash
   dotnet --version
   git --version
   uname -a  # macOS/Linux
   ```

2. **Limpias y reintentas:**
   ```bash
   dotnet clean
   dotnet build
   dotnet test
   ```

3. **Revisa los logs:**
   ```bash
   tail -n 200 banking-*.txt
   ```

4. **Busca en documentación:**
   - SPECKIT-CONSTITUTION.md
   - docs/GETTING_STARTED.md
   - docs/API_GUIDE.md

5. **Abre un issue en GitHub**

---

**¿Aún hay problemas? Contacta al arquitecto de software. ¡No estás solo!** 🤝
