# 🏦 BANKING API - SpecKit Laboratory Edition

**Versión:** 1.0.0  
**Estado:** 🚧 En Desarrollo  
**Propósito:** API bancaria empresarial para laboratorio educativo en .NET

---

## 📌 Resumen Ejecutivo

Una API REST bancaria modular, robusta y bien documentada construida con **.NET 8** siguiendo principios **SOLID**, **Domain-Driven Design** y **Clean Code**.

### Características Principales
- ✅ Gestión de Cuentas Bancarias (CRUD)
- ✅ Transferencias interbancarias con validaciones estrictas
- ✅ Depósitos y Retiros
- ✅ Logging estructurado con **Serilog**
- ✅ Trazabilidad completa con **Correlation ID**
- ✅ Documentación automática con **Swagger/OpenAPI**
- ✅ Pruebas unitarias obligatorias (**xUnit**)
- ✅ Validaciones de dominio robustas

### Decisiones de Arquitectura  
- 🏗️ **Arquitectura:** Layered (4 capas: Presentation, Application, Domain, Infrastructure)
- 📚 **DDD:** Domain-Driven Design para lógica de negocio
- 🧪 **Testing:** Pruebas unitarias + integración (cobertura ≥80%)
- 📖 **Documentación:** Español (docs) + Inglés (código)

---

## 🚀 Inicio Rápido

### Requisitos
- **.NET SDK 8.0+** — [Descargar](https://dotnet.microsoft.com/download)
- **Git** — Para clonar el repositorio

### 5 Minutos para Empezar

```bash
# 1. Clonar
git clone https://github.com/antarescr/RSSFeedReader.git
cd RSSFeedReader/banking-speckit-dotnet-lab/src/BankingApi

# 2. Restaurar dependencias
dotnet restore

# 3. Compilar
dotnet build BankingApi.sln

# 4. Ejecutar pruebas
dotnet test

# 5. Iniciar la API
dotnet run

# 6. Abrir Swagger
# → http://localhost:5000/swagger
```

---

## 📖 Documentación Completa

| Documento | Propósito |
|-----------|----------|
| [spec.md](./spec.md) | 📋 **Especificación Funcional MVP** — 2 operaciones únicamente |
| [plan.md](./plan.md) | 🛠️ **Plan Técnico de Implementación** — Paso a paso con código completo |
| [tasks.md](./tasks.md) | 📋 **Backlog de Tareas** — 5 tareas secuenciales para ejecutar (3.5-4.5h) |
| [SPECKIT-CONSTITUTION.md](./SPECKIT-CONSTITUTION.md) | 📋 **Documento Fundamental** — Reglas globales, estándares, DoD |
| [docs/GETTING_STARTED.md](./docs/GETTING_STARTED.md) | 🚀 **Guía de Inicio** — Setup, instalación, primeros requests |
| [docs/API_GUIDE.md](./docs/API_GUIDE.md) | 📚 **Referencia de API** — Todos los endpoints, ejemplos cURL |
| [.specify/memory/decisions.md](./.specify/memory/decisions.md) | 🏗️ **Decisiones Arquitectónicas** — ADRs, justificaciones |
| [.specify/memory/project.md](./.specify/memory/project.md) | 🔧 **Contexto Técnico** — Información general del proyecto |

### Lee primero:
1. ✅ **SPECKIT-CONSTITUTION.md** — Entiende las reglas arquitectónicas
2. ✅ **spec.md** — Entiende la funcionalidad (2 operaciones)
3. ✅ **plan.md** — Entiende el plan de implementación (paso a paso)
4. ✅ **tasks.md** — Ejecuta las 5 tareas secuenciales
5. ✅ **docs/GETTING_STARTED.md** — Setup del proyecto
6. ✅ **docs/API_GUIDE.md** — Referencia API detallada

---

## 📁 Estructura del Proyecto

```
banking-speckit-dotnet-lab/
├── SPECKIT-CONSTITUTION.md              # 📋 DOCUMENTO MANDATORIO
├── README.md                            # ← Estás aquí
│
├── src/BankingApi/                      # 🔵 TODO EL CÓDIGO FUENTE
│   ├── BankingApi.sln                   # Solución .NET
│   │
│   ├── BankingApi/                      # Proyecto PRINCIPAL (Web API)
│   │   ├── Program.cs                   # Entry point, configuración
│   │   ├── appsettings.json             # Config dev (logging, BD)
│   │   ├── Controllers/                 # 🌐 REST Endpoints
│   │   │   ├── AccountsController.cs
│   │   │   ├── TransfersController.cs
│   │   │   └── DepositsController.cs
│   │   ├── Services/                    # 📦 Lógica de Aplicación
│   │   │   ├── AccountService.cs
│   │   │   ├── TransferService.cs
│   │   │   └── Interfaces/
│   │   ├── Domain/                      # 🎯 Entidades + Reglas de Negocio
│   │   │   ├── Entities/
│   │   │   │   ├── Account.cs           # Validaciones de Cuenta
│   │   │   │   ├── Transfer.cs          # Validaciones de Transferencia
│   │   │   │   └── Deposit.cs           # Validaciones de Depósito
│   │   │   ├── Enums/
│   │   │   │   ├── AccountStatus.cs
│   │   │   │   ├── TransferStatus.cs
│   │   │   │   └── DepositType.cs
│   │   │   └── Exceptions/
│   │   │       ├── DomainException.cs
│   │   │       ├── InsufficientFundsException.cs
│   │   │       └── AccountNotFoundException.cs
│   │   ├── Infrastructure/              # 💾 Acceso a Datos
│   │   │   ├── Repositories/
│   │   │   │   ├── AccountRepository.cs
│   │   │   │   └── TransferRepository.cs
│   │   │   └── Database/
│   │   │       └── BankingContext.cs
│   │   ├── Middlewares/                 # 🔗 Cross-cutting
│   │   │   ├── CorrelationIdMiddleware.cs
│   │   │   └── ErrorHandlingMiddleware.cs
│   │   └── Extensions/
│   │       └── ServiceCollectionExtensions.cs
│   │
│   ├── BankingApi.Domain/               # Proyecto de Dominio
│   ├── BankingApi.Application/          # Proyecto de Aplicación
│   ├── BankingApi.Infrastructure/       # Proyecto de Infraestructura
│   │
│   └── BankingApi.Tests/                # 🧪 Tests Unitarios
│       ├── Services/
│       │   ├── TransferServiceTests.cs
│       │   └── AccountServiceTests.cs
│       └── Domain/
│           ├── AccountTests.cs
│           └── TransferTests.cs
│
├── docs/                                # 📖 DOCUMENTACIÓN
│   ├── API_GUIDE.md                     # Referencia completa de API
│   ├── GETTING_STARTED.md               # Guía de inicio rápido
│   └── TROUBLESHOOTING.md               # Resolución de problemas
│
├── .github/                             # GitHub Actions, config
├── .specify/                            # SpecKit memory
│   └── memory/
│       ├── project.md                   # Contexto del proyecto
│       └── decisions.md                 # ADRs archivo
│
└── .gitignore, .vscode/, etc.
```

**Regla Crítica:** ❌ Ningún código fuente fuera de `src/BankingApi/`

---

## 🛠️ Tecnologías y Versiones

| Componente | Versión | Propósito |
|-----------|---------|----------|
| **.NET SDK** | 8.0+ | Runtime y compilación |
| **C#** | 12.0 | Lenguaje de programación |
| **Serilog** | 3.1+ | Logging estructurado |
| **Swashbuckle** (Swagger) | 6.4+ | OpenAPI documentation |
| **xUnit** | 2.6+ | Framework de testing |
| **Moq** | 4.18+ | Mocking en pruebas |
| **FluentAssertions** | 6.11+ | Assertions fluidas |
| **Entity Framework Core** | 8.0+ | ORM (opcional) |

---

## 🚀 Comandos Útiles

### Desarrollo
```bash
# Ejecutar en modo watch (auto-reload al guardar)
dotnet watch run

# Compilar en Release
dotnet build -c Release

# Limpiar artefactos
dotnet clean
```

### Testing
```bash
# Correr todas las pruebas
dotnet test

# Correr con cobertura de código
dotnet test /p:CollectCoverage=true

# Correr un test específico
dotnet test --filter "NamespaceName.ClassName.MethodName"

# Ver logs detallados de tests
dotnet test -v detailed
```

### Publicación
```bash
# Publicar en Release
dotnet publish -c Release -o ./publish

# Crear paquete NuGet (si aplicable)
dotnet pack -c Release
```

---

## 📊 Estructura de Respuesta API

Toda respuesta sigue este formato consistente:

### ✅ Éxito (2xx)
```json
{
  "success": true,
  "statusCode": 200,
  "data": { /* payload */ },
  "timestamp": "2026-02-27T10:30:00Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### ❌ Error (4xx, 5xx)
```json
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "INSUFFICIENT_FUNDS",
    "message": "Saldo insuficiente para completar la transferencia",
    "details": { "available": 1000, "required": 5000 }
  },
  "timestamp": "2026-02-27T10:30:45Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

---

## 📋 Definition of Done (DoD)

**Ninguna tarea se marca como COMPLETADA sin que cumpla:**

### Código
- [ ] Compila sin errores
- [ ] Sigue estándares SOLID & Clean Code
- [ ] Pruebas unitarias ≥80% cobertura
- [ ] Documentación XML en métodos públicos
- [ ] Validaciones de dominio implementadas
- [ ] Logging en operaciones críticas

### Code Review
- [ ] Mínimo 1 aprobación
- [ ] Documentación actualizada
- [ ] Métodos < 20 líneas
- [ ] Complejidad ciclomática ≤ 5
- [ ] Sin duplicación de código

### Merge
- [ ] Branch actualizado a main
- [ ] CI/CD ✅ PASA
- [ ] Swagger captura cambios
- [ ] README actualizado (si procede)

---

## 📚 Conceptos Clave

### Domain-Driven Design (DDD)
Toda lógica de negocio reside en la capa **Domain**, no en controllers o UI.

```csharp
// ✅ CORRECTO: Validaciones en Entidad
public class Transfer : Entity
{
    public Transfer(Account source, Account target, decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Monto inválido");
        // ... más validaciones
    }
}

// ❌ INCORRECTO: Validaciones en Controller
[HttpPost] public IActionResult Create(TransferDTO dto)
{
    if (dto.Amount <= 0) return BadRequest(); // ❌ Lógica fuera del dominio
}
```

### Logging Estructurado
Todos los logs incluyen **Correlation ID** para trazabilidad:

```csharp
_logger.LogInformation(
    "Transferencia procesada: {TransferId} | {Monto} de {Origen} a {Destino}",
    transfer.Id, transfer.Amount, source.Number, target.Number
);
```

### SOLID en C#

| Principio | Ejemplo |
|-----------|---------|
| **S** Single Responsibility | `TransferValidator` solo valida |
| **O** Open/Closed | Extender con `ITransferService` |
| **L** Liskov Substitution | `CreditAccount` se usa como `Account` |
| **I** Interface Segregation | `ITransferRepository`, no `IRepository<T>` |
| **D** Dependency Inversion | Inyectar `ILogger` no `ConsoleLogger` |

---

## 🔐 Seguridad (Laboratorio)

⚠️ **Este es un entorno de APRENDIZAJE. NO implementa:**
- ❌ Autenticación (sin JWT)
- ❌ Autorización (sin roles)
- ❌ HTTPS
- ❌ Rate limiting formal

✅ **Pero SÍ implementa:**
- ✅ Validaciones estrictas de negocio
- ✅ Logging completo de operaciones
- ✅ Correlation ID para auditoría
- ✅ Excepciones específicas de dominio

⚠️ **JAMÁS usar en producción sin:** OAuth2, JWT, HTTPS, Web Application Firewall.

---

## 🐛 Troubleshooting

### "Port 5000 already in use"
```bash
dotnet run --urls="http://localhost:5001"
```

### "Pruebas fallan"
```bash
dotnet clean && dotnet build && dotnet test
```

### "Swagger no muestra endpoints"
- Verifica que hay comentarios XML en métodos públicos
- Genera XML: Ver `csproj` tiene `<GenerateDocumentationFile>true</GenerateDocumentationFile>`

### Ver logs en tiempo real
```bash
tail -f banking-*.txt         # macOS/Linux
Get-Content banking-*.txt -Tail 20 -Wait  # Windows
```

---

## 👥 Contribuciones

1. Lee [SPECKIT-CONSTITUTION.md](./SPECKIT-CONSTITUTION.md) — Reglas obligatorias
2. Crea rama: `git checkout -b feature/descriptive-name`
3. Commits semánticos: `feat:`, `fix:`, `docs:`, `test:`
4. Push: `git push origin feature/descriptive-name`
5. Pull Request con descripción clara
6. Mínimo 1 aprobación + CI/CD ✅

---

## 📞 Contacto & Soporte

| Aspecto | Contacto |
|--------|----------|
| **Arquitectura** | Revisar [.specify/memory/decisions.md](./.specify/memory/decisions.md) |
| **API** | Acceder a `/swagger` o leer [docs/API_GUIDE.md](./docs/API_GUIDE.md) |
| **Problemas** | Consultar [docs/TROUBLESHOOTING.md](./docs/TROUBLESHOOTING.md) |
| **Inicio** | Leer [docs/GETTING_STARTED.md](./docs/GETTING_STARTED.md) |

---

## 📅 Roadmap

### Phase 1 (Actual) ✅
- [x] Estructura base de proyecto
- [x] Constitución y reglas
- [x] Documentación completa
- [ ] Implementar Domain Entities (Account, Transfer, Deposit)
- [ ] Implementar Services (AccountService, TransferService)
- [ ] Implementar Controllers REST
- [ ] Pruebas unitarias (≥80% cobertura)

### Phase 2 (Próximo Sprint)
- [ ] Entity Framework Core integration
- [ ] Database migrations
- [ ] Validación avanzada (FluentValidation)
- [ ] Caché (Redis opcional)

### Phase 3 (Futuro)
- [ ] Event Sourcing
- [ ] CQRS pattern
- [ ] Microservicios
- [ ] GraphQL API

---

## 📜 Licencia

[Especificar Licencia: MIT, Apache 2.0, etc.]

---

## 🎓 Recursos Educativos

- [Clean Code - Robert C. Martin](https://www.oreilly.com/library/view/clean-code-a/9780136083238/)
- [SOLID Principles](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles)
- [Domain-Driven Design - Eric Evans](https://domainlanguage.com/ddd/)
- [ASP.NET Core Best Practices](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)
- [xUnit Testing Patterns](https://xunit.net/docs/getting-started/netcore)

---

## ✅ Checklist de Adopción

- [ ] Leer SPECKIT-CONSTITUTION.md
- [ ] Clonar y compilar el proyecto
- [ ] Ejecutar `dotnet test` con éxito
- [ ] Acceder a `http://localhost:5000/swagger`
- [ ] Revisar estructura en `src/BankingApi/`
- [ ] Estudiar ejemplos de Controllers y Services
- [ ] Escribir 1 test unitario de ejemplo
- [ ] Crear 1 feature branch y PR

---

**¡Bienvenido al laboratorio de Banking API con SpecKit! 🏦**

`"Con código limpio y validaciones fuertes, construimos sistemas confiables."`

---

**Última actualización:** 27 de febrero de 2026  
**Versión:** 1.0.0  
**Estado:** 🚧 En desarrollo activo
