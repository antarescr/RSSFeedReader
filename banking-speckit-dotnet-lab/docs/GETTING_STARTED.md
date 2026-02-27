# 🚀 GUÍA DE INICIO RÁPIDO - API BANCARIA

**Última actualización:** 27 de febrero de 2026

---

## 1. Requisitos Previos

Asegúrate de tener instalado:

```bash
# Verificar .NET SDK 8.0+
dotnet --version

# Debería mostrar: 8.0.x o superior
```

Si no tienes .NET SDK, descárgalo desde [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

---

## 2. Clonar y Preparar el Proyecto

```bash
# Clonar el repositorio
git clone https://github.com/antarescr/RSSFeedReader.git
cd RSSFeedReader/banking-speckit-dotnet-lab

# Navegar a la carpeta del proyecto
cd src/BankingApi
```

---

## 3. Restaurar Dependencias

```bash
# Restaurar paquetes NuGet
dotnet restore

# Debería descargar Serilog, xUnit, Moq, Swashbuckle, etc.
```

---

## 4. Compilar el Proyecto

```bash
# Compilar solución
dotnet build BankingApi.sln

# Si todo está bien, verás: "Build succeeded"
```

---

## 5. Ejecutar Pruebas

```bash
# Correr todas las pruebas unitarias
dotnet test

# Ejemplo esperado:
# Test Run Successful.
# Total tests: 15
# Passed: 15 (100%)
# Failed: 0
```

---

## 6. Iniciar la API

```bash
# Desde src/BankingApi/BankingApi (proyecto principal)
dotnet run

# Esperado:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: http://localhost:5000
# info: Microsoft.Hosting.Lifetime[0]
#       Application started. Press Ctrl+C to exit.
```

---

## 7. Acceder a Swagger (Documentación Interactiva)

Una vez que la API esté corriendo, abre tu navegador:

```
http://localhost:5000/swagger
```

Verás la documentación completa de todos los endpoints con ejemplos.

---

## 8. Primer Request - Listar Cuentas

```bash
# En otra terminal, hacer un GET a las cuentas
curl -X GET http://localhost:5000/api/v1/accounts \
  -H "Content-Type: application/json"

# Respuesta esperada:
{
  "success": true,
  "statusCode": 200,
  "data": [],
  "timestamp": "2026-02-27T10:30:00Z",
  "correlationId": "xyz-123"
}
```

---

## 9. Crear una Cuenta

```bash
# POST - Crear cuenta bancaria
curl -X POST http://localhost:5000/api/v1/accounts \
  -H "Content-Type: application/json" \
  -d '{
    "accountNumber": "ACC-001",
    "accountType": "CHECKING",
    "ownerName": "Juan Pérez",
    "initialBalance": 5000
  }'

# Respuesta exitosa (201 Created):
{
  "success": true,
  "statusCode": 201,
  "data": {
    "id": 1,
    "accountNumber": "ACC-001",
    "accountType": "CHECKING",
    "ownerName": "Juan Pérez",
    "balance": 5000,
    "status": "ACTIVE",
    "createdAt": "2026-02-27T10:30:00Z"
  },
  "timestamp": "2026-02-27T10:30:00Z",
  "correlationId": "abc-456"
}
```

---

## 10. Hacer una Transferencia

```bash
# Primero, crear otra cuenta (destino)
curl -X POST http://localhost:5000/api/v1/accounts \
  -H "Content-Type: application/json" \
  -d '{
    "accountNumber": "ACC-002",
    "accountType": "SAVINGS",
    "ownerName": "María García",
    "initialBalance": 2000
  }'

# Luego, transferir dinero
curl -X POST http://localhost:5000/api/v1/transfers \
  -H "Content-Type: application/json" \
  -H "X-Correlation-ID: transfer-001" \
  -d '{
    "sourceAccountId": 1,
    "targetAccountId": 2,
    "amount": 500,
    "concept": "Pago de servicios"
  }'

# Respuesta:
{
  "success": true,
  "statusCode": 200,
  "data": {
    "id": 1,
    "sourceAccountId": 1,
    "targetAccountId": 2,
    "amount": 500,
    "concept": "Pago de servicios",
    "status": "COMPLETED",
    "completedAt": "2026-02-27T10:35:00Z"
  },
  "timestamp": "2026-02-27T10:35:00Z",
  "correlationId": "transfer-001"
}
```

---

## 11. Ver Logs Estructurados

Los logs se guardan en: `banking-yyyymmdd.txt` en la raíz del proyecto.

```bash
# Ver logs en tiempo real (macOS/Linux)
tail -f banking-20260227.txt

# En Windows
Get-Content banking-20260227.txt -Tail 20 -Wait
```

Ejemplo de log estructurado:
```json
{
  "@t": "2026-02-27T10:35:00.1234567Z",
  "@m": "Transferencia completada: Transfer-1 | 500 de ACC-001 a ACC-002",
  "@l": "Information",
  "CorrelationId": "transfer-001",
  "TransferId": "Transfer-1",
  "Amount": 500
}
```

---

## 12. Estructura del Proyecto

```
src/BankingApi/
├── BankingApi/                      # Proyecto Web API
│   ├── Program.cs                   # Configura servicios y middleware
│   ├── appsettings.json             # Configuración de Serilog, BD
│   ├── Controllers/                 # Endpoints HTTP
│   │   ├── AccountsController.cs
│   │   ├── TransfersController.cs
│   │   └── DepositsController.cs
│   ├── Services/                    # Lógica de aplicación
│   │   ├── AccountService.cs
│   │   ├── TransferService.cs
│   │   └── interfaces/
│   │       ├── IAccountService.cs
│   │       └── ITransferService.cs
│   ├── Domain/                      # Entidades del dominio
│   │   ├── Account.cs
│   │   ├── Transfer.cs
│   │   ├── Deposit.cs
│   │   └── Enums/
│   │       ├── AccountType.cs
│   │       └── TransferStatus.cs
│   ├── Infrastructure/              # Repositorios y acceso a datos
│   │   └── Repositories/
│   │       ├── AccountRepository.cs
│   │       └── TransferRepository.cs
│   ├── Middlewares/                 # Middleware personalizado
│   │   ├── CorrelationIdMiddleware.cs
│   │   └── ErrorHandlingMiddleware.cs
│   └── Extensions/
│       └── ServiceCollectionExtensions.cs
│
├── BankingApi.Domain/               # Proyecto de clases de dominio
├── BankingApi.Application/          # Proyecto de servicios
├── BankingApi.Infrastructure/       # Proyecto de repositorios/datos
│
└── BankingApi.Tests/                # Proyecto de pruebas
    ├── Services/
    │   ├── TransferServiceTests.cs
    │   └── AccountServiceTests.cs
    └── Domain/
        ├── AccountTests.cs
        └── TransferTests.cs
```

---

## 13. Común Problemas y Soluciones

### "Port 5000 already in use"
```bash
# Cambiar puerto en appsettings.json
# O ejecutar en otro puerto:
dotnet run --urls="http://localhost:5001"
```

### "EF Core migrations not found"
```bash
# Si usas Entity Framework Core
cd BankingApi
dotnet ef database update
```

### "Pruebas fallan"
```bash
# Limpiar caché y reconstruir
dotnet clean
dotnet build
dotnet test
```

---

## 14. Próximos Pasos

1. ✅ Lee [SPECKIT-CONSTITUTION.md](../SPECKIT-CONSTITUTION.md) (reglas del proyecto)
2. ✅ Consulta [API_GUIDE.md](./API_GUIDE.md) (detalles de endpoints)
3. ✅ Explora el código en `src/BankingApi/`
4. ✅ Corre las pruebas: `dotnet test`
5. ✅ Accede a Swagger: `http://localhost:5000/swagger`

---

## 💡 Tips Útiles

**Recargar código automáticamente:**
```bash
dotnet watch run
```

**Ejecutar pruebas con cobertura:**
```bash
dotnet test /p:CollectCoverage=true
```

**Publicar en Release:**
```bash
dotnet publish -c Release
```

---

¿Preguntas? Consulta la documentación completa en `docs/` o revisa el código comentado en español.

**¡Bienvenido al equipo SpecKit Banking! 🏦**
