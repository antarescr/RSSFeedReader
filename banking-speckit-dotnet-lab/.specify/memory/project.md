## 📋 INFORMACIÓN DEL PROYECTO

**Nombre:** Banking API - SpecKit Laboratory Edition
**Versión:** 1.0.0
**Estado:** 🚧 En desarrollo
**Propósito:** API bancaria empresarial educativa en .NET

---

## 🏗️ Arquitectura

- **Tipo:** REST API
- **Framework:** ASP.NET Core 8.0
- **Patrón:** Layered Architecture + Domain-Driven Design
- **Testing:** xUnit + Moq + FluentAssertions
- **Logging:** Serilog con correlation IDs
- **Documentación:** Swagger/OpenAPI 3.0

---

## 📍 Ubicación del Código

**REGLA CRÍTICA:** Todo el código fuente DEBE estar en:
```
src/BankingApi/
├── BankingApi/                 (Capa Presentación)
├── BankingApi.Domain/          (Capa Dominio)
├── BankingApi.Application/     (Capa Aplicación)
├── BankingApi.Infrastructure/  (Capa Infraestructura)
└── BankingApi.Tests/           (Pruebas)
```

---

## 🌐 Política de Idioma

| Aspecto | Idioma | Nota |
|--------|--------|------|
| **Documentación Oficial** | Español | README.md, docs/, SPECKIT-CONSTITUTION.md |
| **Código Fuente** | Inglés | Todas las clases, métodos, variables |
| **Comentarios en Código** | Español | Para facilitar comprensión del equipo |
| **API Routes** | Inglés | `/api/v1/accounts`, `/api/v1/transfers` |
| **Base de Datos** | Inglés | Esquemas, tablas, columnas |
| **Nombres de Métodos** | Inglés | `GetAccount()`, `ProcessTransfer()` |
| **Enums** | Inglés | `AccountStatus`, `TransferStatus` |
| **Excepciones** | Inglés | `InsufficientFundsException` |

### Ejemplo de Cumplimiento

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

## 📚 Documentación Principal

1. **SPECKIT-CONSTITUTION.md** — Documento fundacional con todas las reglas
2. **README.md** — Hoja de ruta e información general
3. **docs/GETTING_STARTED.md** — Guía de setup e inicio rápido
4. **docs/API_GUIDE.md** — Referencia completa de API REST
5. **docs/TROUBLESHOOTING.md** — Solución de problemas
6. **.specify/memory/decisions.md** — Decisiones arquitectónicas (ADRs)

---

## ✅ Estándares Obligatorios

### Código
- ✅ SOLID principles
- ✅ Clean Code (máximo 20 líneas por método)
- ✅ Domain-Driven Design para lógica de negocio
- ✅ Naming: PascalCase (clases), camelCase (variables)
- ✅ Documentación XML en métodos públicos

### Testing
- ✅ Mínimo 80% cobertura de código
- ✅ Patrón Arrange-Act-Assert
- ✅ xUnit + Moq + FluentAssertions
- ✅ Validaciones de dominio testeadas

### Logging
- ✅ Serilog con formatter JSON estructurado
- ✅ Correlation ID en todos los logs
- ✅ Niveles: Trace, Debug, Info, Warn, Error, Fatal
- ✅ Operaciones críticas logeadas siempre

### API
- ✅ REST puro (GET, POST, PUT, DELETE)
- ✅ Estructura de respuesta consistente (success, statusCode, data, error)
- ✅ Swagger/OpenAPI habilitado en `/swagger`
- ✅ HTTP status codes correctos (200, 201, 400, 404, 500)

---

## 🔐 Seguridad (Laboratorio)

### ❌ NO Implementado (Deliberadamente)
- Autenticación (sin JWT)
- Autorización (sin roles)
- HTTPS (HTTP suficiente)
- Rate limiting formal
- Cifrado de datos

### ✅ SÍ Implementado
- Validaciones estrictas de negocio
- Logging completo para auditoría
- Correlation IDs para trazabilidad
- Excepciones específicas de dominio

⚠️ **ADVERTENCIA:** Nunca usar en producción sin agregar:
- OpenID Connect / OAuth2
- JWT tokens
- HTTPS/TLS
- Web Application Firewall
- Rate limiting y throttling

---

## 🎯 Definition of Done (DoD)

Antes de marcar tarea como HECHA:

### Desarrollo
- [ ] Código compila sin errores
- [ ] Código sigue SOLID & Clean Code
- [ ] Pruebas unitarias hacen PASS
- [ ] Cobertura ≥80%
- [ ] Documentación XML en métodos públicos
- [ ] Validaciones de dominio implementadas
- [ ] Logging en operaciones críticas

### Code Review
- [ ] Mínimo 1 aprobación
- [ ] Sin comentarios TODO sin resolver
- [ ] Métodos < 20 líneas
- [ ] Complejidad ciclomática ≤ 5
- [ ] Documentación actualizada

### Merge
- [ ] Branch actualizado a main
- [ ] CI/CD ✅ PASA
- [ ] Swagger captura cambios
- [ ] README actualizado (si aplica)

---

## 🚀 Próximos Pasos

### Phase 1 (Actual)
- [x] Estructura base del proyecto
- [x] Constitución y reglas establecidas
- [x] Documentación completa
- [ ] Implementar Domain Entities (Account, Transfer, Deposit)
- [ ] Implementar Services y Controllers
- [ ] Tests unitarios ≥80% cobertura

### Phase 2
- [ ] Entity Framework Core + Migrations
- [ ] Validación avanzada (FluentValidation)
- [ ] Caché (Redis)

### Phase 3
- [ ] Event Sourcing
- [ ] CQRS pattern
- [ ] Microservicios

---

## 📊 Métricas de Calidad

| Métrica | Objetivo | Actual |
|---------|----------|--------|
| Cobertura de tests | ≥80% | Pending |
| Duración promedio compilación | <15s | - |
| Duración promedio tests | <5s | - |
| Endpoints documentados en Swagger | 100% | 0% (pending) |
| Métodos sin documentación XML | 0% | Pending |

---

## 🛠️ Tecnologías (Locked Versions)

| Tecnología | Versión | Tipo |
|-----------|---------|------|
| **.NET SDK** | 8.0+ | Runtime |
| **C#** | 12.0 | Lenguaje |
| **ASP.NET Core** | 8.0 | Framework |
| **Serilog** | 3.1+ | Logging |
| **xUnit** | 2.6+ | Testing |
| **Moq** | 4.18+ | Mocking |
| **FluentAssertions** | 6.11+ | Assertions |
| **Swashbuckle** | 6.4+ | Swagger |
| **Entity Framework Core** | 8.0+ | ORM (opcional) |

---

## 👥 Equipo

| Rol | Responsable | Notas |
|-----|-------------|-------|
| Arquitecto | SpecKit Team | Decisiones de diseño |
| Lead Técnico | - | Revisiones de código |
| Desarrolladores | - | Implementación |
| QA | - | Testing |

---

## 📞 Contactos y Documentación

- **Preguntas sobre arquitectura:** Ver `.specify/memory/decisions.md`
- **API endpoints:** Ver `docs/API_GUIDE.md`
- **Inicio rápido:** Ver `docs/GETTING_STARTED.md`
- **Problemas técnicos:** Ver `docs/TROUBLESHOOTING.md`
- **Reglas generales:** Ver `SPECKIT-CONSTITUTION.md`

---

## 📅 Histórico

| Fecha | Evento | Responsable |
|-------|--------|-------------|
| 2026-02-27 | Creación de Constitución v1.0 | Arquitecto SpecKit |
| 2026-02-27 | Setup inicial de estructura | Arquitecto SpecKit |
| - | Implementación de Domain Entities | TBD |
| - | Implementation de Services | TBD |
| - | Primeras pruebas unitarias | TBD |

---

## ✨ Valores del Proyecto

- 🎯 **Claridad:** Código legible y auto-explicativo
- 🧪 **Confiabilidad:** Tests cubriendo toda lógica crítica
- 📊 **Trazabilidad:** Logging completo para auditoría
- 🏛️ **Arquitectura:** Diseño limpio y mantenible
- 📚 **Documentación:** Viva y siempre actualizada
- 🤝 **Colaboración:** Code reviews constructivas

**Mantra:** _"Con código limpio y validaciones fuertes, construimos sistemas confiables."_