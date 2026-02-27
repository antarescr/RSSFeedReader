# 📝 CHANGELOG - Banking API

Todos los cambios notables de este proyecto están documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/) y este proyecto cumple con [Versionamiento Semántico](https://semver.org/lang/es/).

---

## [1.0.0] - 2026-02-27

### 🎉 Iniciado (Unreleased)

#### ✅ Agregado

- 📋 **Constitución del Proyecto (SPECKIT-CONSTITUTION.md)** — Documento fundamental que establece:
  - Política de idioma: Español (documentación) / Inglés (código)
  - Estructura mandatoria de carpetas bajo `src/BankingApi/`
  - Estándares de seguridad para laboratorio educativo
  - Logging estructurado con Serilog y Correlation IDs
  - Validaciones estrictas en capa Domain (DDD)
  - Estándares SOLID y Clean Code
  - Pruebas unitarias obligatorias (≥80% cobertura)
  - Swagger/OpenAPI v3.0 habilitado
  - Definition of Done clara

- 📚 **Documentación Completa**
  - [spec.md](./spec.md) — **Especificación Funcional MVP** — 2 operaciones, validaciones, seed data, casos de uso, ejemplos cURL
  - [plan.md](./plan.md) — **Plan Técnico de Implementación** — Stack, arquitectura, fases, código modular, testing
  - [tasks.md](./tasks.md) — **Backlog de Tareas** — 5 tareas secuenciales (380+ líneas), entregables, criterios de aceptación, código completo, 12 test cases
  - [README.md](./README.md) — Visión general y quick start
  - [docs/GETTING_STARTED.md](./docs/GETTING_STARTED.md) — Guía de inicio rápido en 5 minutos
  - [docs/API_GUIDE.md](./docs/API_GUIDE.md) — Referencia completa de endpoints REST con ejemplos cURL
  - [docs/TROUBLESHOOTING.md](./docs/TROUBLESHOOTING.md) — 30+ problemas comunes y soluciones
  - [.specify/memory/decisions.md](./.specify/memory/decisions.md) — 10 ADRs (Architectural Decision Records)

- 🏗️ **Estructura de Carpetas Base**
  - `src/BankingApi/` — Ubicación obligatoria del código fuente
  - `docs/` — Documentación del proyecto en español
  - `.specify/memory/` — Contexto arquitectónico (SpecKit)

- 🎯 **Arquitectura Definida**
  - Layered Architecture (4 capas)
  - Domain-Driven Design para lógica de negocio
  - Separación clara: Presentation → Application → Domain → Infrastructure

- 📋 **Políticas Establecidas**
  - Nomenclatura de commits: `feat:`, `fix:`, `docs:`, `test:`
  - Rama principal protegida: `main` requiere PR + revisión + CI/CD
  - Estructura de respuesta API estándar (success, statusCode, data, error, correlationId)
  - Validaciones obligatorias de transacciones financieras

- 🝥️ **Decisiones Arquitectónicas (ADRs)**
  - ADR-001: Separación lingüística Español/Inglés
  - ADR-002: Arquitectura de Capas
  - ADR-003: Domain-Driven Design
  - ADR-004: Serilog para logging estructurado
  - ADR-005: xUnit para testing
  - ADR-006: Cobertura mínima 80%
  - ADR-007: Swagger/OpenAPI v3.0 mandatorio
  - ADR-008: Sin autenticación/autorización (lab)
  - ADR-009: Validaciones estrictas en dominio
  - ADR-010: Correlation ID en todos los logs

#### ⚠️ NOTAS IMPORTANTES

- Este es un **proyecto educativo/laboratorio**
  - NO implementa autenticación (sin JWT)
  - NO implementa autorización (sin roles)
  - NO usa HTTPS (HTTP suficiente)
  - Enfoque COMPLETAMENTE en lógica de negocio

- Estructura de carpetas es **OBLIGATORIA**
  - Todo código en `src/BankingApi/`
  - Nada afuera de esa carpeta

- Documentación es **VIVA**
  - Se actualiza cada sprint
  - Código debe mantener documentación sincronizada

---

## [0.0.0] - Pre-Release

### 🚧 Preparación

- Creación de estructura inicial del repositorio
- Setup de GitHub (rama main protegida)
- Configuración de SpecKit memory

---

## 📋 Plantilla para Próximas Versiones

Cuando se implemente v1.1.0, v2.0.0, etc., usar esta estructura:

```markdown
## [X.Y.Z] - YYYY-MM-DD

### ✅ Agregado
- Nueva funcionalidad A
- Nueva funcionalidad B

### 🔄 Cambiado
- Comportamiento existente cambió de X a Y
- Refactorización de módulo Z

### ❌ Removido
- Característica deprecated A

### 🐛 Corregido
- Bug #123: Descripción
- Bug #456: Descripción

### 🔒 Seguridad
- Parche de seguridad para vulnerability X

### ⚠️ Deprecado
- Método X será removido en v3.0.0, usar Y en su lugar
```

---

## 🔮 Roadmap Futuro

### v1.1.0 (Próxima Iteración)
- [ ] Implementación de Domain Entities (Account, Transfer, Deposit)
- [ ] Implementación de Services (AccountService, TransferService)
- [ ] Implementación de Controllers REST
- [ ] Pruebas unitarias ≥80% cobertura
- [ ] Database setup (Entity Framework Core)

### v1.2.0 (Following)
- [ ] Validación avanzada (FluentValidation)
- [ ] Caché (Redis, opcional)
- [ ] Filtros y búsqueda avanzada
- [ ] Paginación en endpoints

### v2.0.0 (Future)
- [ ] Autenticación + Autorización (real, no lab)
- [ ] HTTPS/TLS
- [ ] Rate limiting production-grade
- [ ] Event Sourcing
- [ ] CQRS pattern
- [ ] Microservicios

### v3.0.0+ (Visión a Largo Plazo)
- [ ] GraphQL API
- [ ] Distributed tracing (Jaeger/Zipkin)
- [ ] Service mesh (Istio)
- [ ] Escalabilidad horizontal con K8s

---

## 📊 Compatibilidad

| Versión | .NET SDK | C# | Estado |
|---------|----------|----|---------
| 1.0.0+ | 8.0+ | 12.0+ | ✅ In Development |

---

## 🔗 Links Útiles

- [Changelog Format](https://keepachangelog.com/)
- [Semantic Versioning](https://semver.org/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [SPECKIT-CONSTITUTION.md](./SPECKIT-CONSTITUTION.md)
- [ADRs](./.specify/memory/decisions.md)

---

## 📞 Comentarios & Preguntas

Si tienes preguntas sobre cambios:
1. Consulta [SPECKIT-CONSTITUTION.md](./SPECKIT-CONSTITUTION.md)
2. Revisa [ADRs](./.specify/memory/decisions.md)
3. Lee [docs/API_GUIDE.md](./docs/API_GUIDE.md)
4. Contacta al arquitecto de software

---

**Última actualización:** 2026-02-27  
**Versión Actual:** 1.0.0 (Inicialización)  
**Mantener Actualizado:** Se recomienda actualizar este archivo con cada PR merge
