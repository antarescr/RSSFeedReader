# 📋 DECISIONES ARQUITECTÓNICAS (ADRs)

**Archivo de Registro de Decisiones de Arquitectura**

---

## ADR-001: Separación Lingüística Español/Inglés

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Proyecto con múltiples stakeholders hispanohablantes pero estándares de desarrollo en inglés.

### Decisión
- **Documentación:** ESPAÑOL
- **Código Fuente:** INGLÉS
- **API Routes:** INGLÉS
- **Base de Datos:** INGLÉS

### Justificación
1. Accesibilidad: documentación hispana para equipo hispanohablante
2. Compatibilidad: código en inglés es estándar internacional
3. Mantenibilidad: comentarios en español en código
4. Consistencia: nombres técnicos siempre inglés

### Alternativas Rechazadas
- ❌ Todo en español: incompatible con ecosistema .NET
- ❌ Todo en inglés: confuso para documentación de negocio

### Impacto
- ✅ Baja: ya establecido en SPECKIT-CONSTITUTION
- ✅ Requiere revisor de código que valide convención

---

## ADR-002: Arquitectura de Capas (Layered Architecture)

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Necesidad de modular código para mantenibilidad y escalabilidad.

### Decisión
```
Presenta tion Layer (Controllers)
     ↓
Application Layer (Services)
     ↓
Domain Layer (Entities, Business Rules)
     ↓
Infrastructure Layer (Repositories)
```

### Justificación
1. **Separation of Concerns:** cada capa tiene responsabilidad clara
2. **Testability:** fácil de mockear capas inferiores
3. **Reusability:** lógica de dominio reutilizable
4. **SOLID:** adhiere a DIP (Dependency Inversion Principle)

### Estructura de Proyectos
```
BankingApi/                          (Presentation)
BankingApi.Application/              (Application)
BankingApi.Domain/                   (Domain)
BankingApi.Infrastructure/           (Infrastructure)
BankingApi.Tests/                    (Unit Tests)
```

### Alternativas Rechazadas
- ❌ Monolito único: difícil de mantener
- ❌ Microservicios: prematura para lab educativo

### Impacto
- ⚠️ Moderado: requiere 4+ proyectos
- ✅ Payoff alto en mantenibilidad

---

## ADR-003: Domain-Driven Design (DDD) para Validaciones

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Necesidad de validaciones de negocio robustas y reutilizables.

### Decisión
- Todas las validaciones van en el **Domain Layer**
- Las entidades de dominio tienen constructores privados con factory methods
- Excepciones de dominio específicas (`InsufficientFundsException`, `DomainException`)

### Ejemplo
```csharp
// ✅ CORRECTO: Validación en constructo de entidad
public class Transfer : Entity
{
    public Transfer(Account source, Account target, decimal amount, string concept)
    {
        if (amount <= 0)
            throw new DomainException("Monto inválido");
        
        if (source.Balance < amount)
            throw new InsufficientFundsException("Saldo insuficiente");
        
        // ... resto de lógica
    }
}

// ❌ INCORRECTO: Validación en Controller
[HttpPost]
public IActionResult CreateTransfer(TransferDTO dto)
{
    if (dto.Amount <= 0) // ❌ Lógica de negocio en web layer
        return BadRequest();
}
```

### Justificación
1. Reglas de negocio centralizadas
2. Reutilizable en CLI, APIs, trabajos batch
3. Imposible crear instancias inválidas
4. Facilita testing de reglas

### Impacto
- ⚠️ Alto: refactorización de código
- ✅ Mejora expresividad de código

---

## ADR-004: Serilog para Logging Estructurado

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Necesidad de logs auditables y buscables para operaciones financieras críticas.

### Decisión
- **Librería:** Serilog
- **Formato:** JSON estructurado
- **Enrichers:** Correlation ID en todo log
- **Sinks:** Console + archivo rotatório
- **Nivel mínimo:** Information (en producción)

### Configuración Requerida
```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("CorrelationId", context?.TraceIdentifier)
    .WriteTo.Console()
    .WriteTo.File("logs/banking-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
```

### Justificación
1. **JSON estructurado:** buscable con grep, jq
2. **Correlation ID:** trazabilidad de requests
3. **Rendimiento:** mejor que log4net para async
4. **Community:** activamente mantenida

### Alternativas Rechazadas
- ❌ Log4Net: más antiguo, menos modern
- ❌ NLog: válido pero Serilog más popular
- ❌ Console.WriteLine: no es suficiente

### Impacto
- ✅ Bajo: fácil de integrar
- ✅ Alto valor en debugging

---

## ADR-005: xUnit para Testing

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Elegir framework de testing para .NET.

### Decisión
- **Framework:** xUnit
- **Mocking:** Moq
- **Assertions:** FluentAssertions
- **Cobertura requerida:** ≥80%
- **Patrón:** Arrange-Act-Assert (AAA)

### Ejemplo
```csharp
public class TransferServiceTests
{
    [Fact]
    public async Task ProcessTransfer_WithValidData_ShouldCompleteSuccessfully()
    {
        // ARRANGE
        var sourceAccount = new Account(id: 1, balance: 5000m);
        var transferService = new TransferService(mockRepository.Object);
        
        // ACT
        var result = await transferService.ProcessAsync(request);
        
        // ASSERT
        result.Should().NotBeNull();
        result.Status.Should().Be(TransferStatus.Completed);
    }
}
```

### Justificación
1. **Modern:** built para .NET moderno (parallelizable)
2. **Clean:** sintaxis clara, sin setup pesado
3. **Ecosystem:** compatible con Moq, FluentAssertions
4. **Industry:** usado por ASP.NET Core mismo

### Alternativas Rechazadas
- ❌ MSTest: más verbose
- ❌ NUnit: antiguo, menos modern

### Impacto
- ✅ Bajo: fácil adopción
- ✅ Mejor velocidad de tests

---

## ADR-006: Cobertura Mínima 80% de Pruebas

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Balance entre cobertura de código y velocidad de desarrollo.

### Decisión
- **Mínimo requerido:** 80% de cobertura
- **Excepciones permitidas:** setup/configuration code
- **Excluir:** controllers simples (solo orquestación)
- **Prioridad:** Domain + Application layers

### Medir Cobertura
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Justificación
1. **80/20 rule:** 80% cobertura captura 95% de bugs
2. **No perfectionism:** 100% es excesivo
3. **Business logic:** enfoque en Domain
4. **Escalable:** mantener a medida que crece

### Alternativas Rechazadas
- ❌ Sin cobertura: riesgo alto
- ❌ 100% cobertura: time sink sin ROI

### Impacto
- ⚠️ Moderado: requiere disciplina
- ✅ Payoff alto en confiabilidad

---

## ADR-007: Swagger/OpenAPI v3.0 Mandatorio

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Documentación de API debe ser livingdoc (viva, actualizada automáticamente).

### Decisión
- **Librería:** Swashbuckle (OpenAPI v3.0)
- **Documentación XML:** obligatoria en métodos públicos
- **Endpoint:** `/swagger` accesible en desarrollo
- **Ejemplos:** Request/Response ejemplificados

### Configuración
```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banking API",
        Version = "1.0.0"
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

app.UseSwagger();
app.UseSwaggerUI();
```

### Justificación
1. **Living Doc:** siempre sincronizado con código
2. **Interactive:** probar endpoints desde navegador
3. **Estándar:** OpenAPI es industria-estándar
4. **Client-Gen:** generar clientes automáticamente

### Impacto
- ✅ Bajo: Swashbuckle es plug-and-play
- ✅ Alto valor en usabilidad

---

## ADR-008: Sin Autenticación ni Autorización (Lab)

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Este es un **laboratorio educativo**, no producción.

### Decisión
- ❌ NO implementar autenticación (sin JWT)
- ❌ NO implementar autorización (sin roles)
- ❌ NO implementar HTTPS
- ✅ ENFOQUE en lógica de negocio

### Justificación
1. **Scope reducido:** enfoque en dominio
2. **Aprendizaje:** conceptos de seguridad son otro curso
3. **Velocidad:** acelera el desarrollo inicial
4. **Lab:** explícitamente marcado como educativo

### Nota de Seguridad
⚠️ **JAMÁS usar este código en producción sin añadir:**
- OpenID Connect / OAuth2
- JWT tokens
- HTTPS/TLS
- Rate limiting
- Input sanitization avanzada

### Impacto
- ✅ Alto: simplifica considerablemente código
- ⚠️ Crítico: documenación clara de limitaciones

---

## ADR-009: Validaciones Estrictas en Dominio

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Operaciones financieras requieren garantías de integridad.

### Decisión
- Cada entidad validaleglas de negocio en constructor
- Excepciones específicas (`InsufficientFundsException`) vs genéricas
- Máximos y mínimos definidos como constantes
- Validaciones nunca a null (parámetros requeridos)

### Reglas Ejemplo (Transferencias)
```csharp
public class Transfer : Entity
{
    public Transfer(Account source, Account target, decimal amount, string concept)
    {
        if (amount <= 0)
            throw new DomainException("Monto debe ser > 0");
        
        if (source.Id == target.Id)
            throw new DomainException("No se puede transferir a la misma cuenta");
        
        if (source.Balance < amount)
            throw new InsufficientFundsException($"Saldo insuficiente: {source.Balance}");
        
        // Limit validations
        const decimal MaxTransferAmount = 10_000m;
        if (amount > MaxTransferAmount)
            throw new DomainException($"Excede límite: {MaxTransferAmount}");
    }
}
```

### Justificación
1. **Invariantes:** impossibilidad de estado inválido
2. **Predecible:** comportamiento consistente
3. **Auditable:** todas las fallos registradas
4. **Testeable:** fácil escribir test cases

### Impacto
- ⚠️ Moderado: verbosidad inicial
- ✅ Alto payoff en robustez

---

## ADR-010: Correlation ID en Todos los Logs

**Fecha:** 27 de febrero de 2026  
**Autor:** Arquitecto SpecKit  
**Estado:** ✅ ACEPTADA

### Contexto
Sistemas distribuidos requieren trazabilidad de request end-to-end.

### Decisión
- Header `X-Correlation-ID` en requests
- Generado automáticamente si no presente
- Incluido en TODOS los logs
- Retornado en TODAS respuestas

### Implementación
```csharp
// Middleware
app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers
        .TryGetValue("X-Correlation-ID", out var value)
        ? value.ToString()
        : Guid.NewGuid().ToString();
    
    using (LogContext.PushProperty("CorrelationId", correlationId))
    {
        context.Response.Headers.Add("X-Correlation-ID", correlationId);
        await next();
    }
});
```

### Búsqueda en Logs
```bash
grep "abc-123-def-456" banking-*.txt
# Muestra TODAS operaciones de ese request
```

### Justificación
1. **Debugging:** reconstruir exact request flow
2. **Auditoría:** cumplimiento regulatorio
3. **Monitoring:** alertas vinculadas a request
4. **SLA:** tracking response times por request

### Impacto
- ✅ Bajo: overhead mínimo
- ✅ Valor incalculable en soporte

---

## Próximas Decisiones Pendientes

- [ ] **ADR-011:** Estrategia de versionamiento API (v1.0, v1.1, v2.0)
- [ ] **ADR-012:** Concurrencia (optimistic locking, pessimistic)
- [ ] **ADR-013:** Caché (Redis sí/no)
- [ ] **ADR-014:** Event Sourcing o CRUD puro
- [ ] **ADR-015:** Escalabilidad horizontal (load balancing)

---

**Documento vivo:** se actualiza cada sprint con nuevas decisiones arquitectónicas.
