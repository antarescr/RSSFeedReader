# 📚 GUÍA COMPLETA DE API - Banking API v1.0

**Última actualización:** 27 de febrero de 2026

---

## Tabla de Contenidos
1. [Introducción](#introducción)
2. [Autenticación & Seguridad](#autenticación--seguridad)
3. [Convenciones de Respuesta](#convenciones-de-respuesta)
4. [Recursos: Cuentas](#recursos-cuentas)
5. [Recursos: Transferencias](#recursos-transferencias)
6. [Recursos: Depósitos](#recursos-depósitos)
7. [Códigos de Error](#códigos-de-error)
8. [Correlation ID & Trazabilidad](#correlation-id--trazabilidad)
9. [Rate Limiting & Best Practices](#rate-limiting--best-practices)

---

## Introducción

**Base URL:** `http://localhost:5000/api/v1`

**Formato:** `application/json`

**Versión API:** 1.0

### Objetivos de Diseño
- RESTful puro: métodos HTTP semánticos (GET, POST, PUT, DELETE)
- Respuestas consistentes con estructura estándar
- Validaciones estrictas en el servidor
- Logs completos para auditoría

---

## Autenticación & Seguridad

### Nota importante - Laboratorio Educativo ⚠️

Este servidor **NO implementa:**
- ❌ Autenticación (sin JWT, sin clave API)
- ❌ Autorización (sin roles de usuario)
- ❌ HTTPS (solo HTTP para desarrollo)

**Enfoque:** Lógica de negocio y validaciones de dominio.

### Header Recomendado: Correlation ID
```http
X-Correlation-ID: 550e8400-e29b-41d4-a716-446655440000
```

Si no lo envías, se generará automáticamente uno.

---

## Convenciones de Respuesta

### Respuesta Exitosa (2xx)

```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    // Payload del recurso
  },
  "timestamp": "2026-02-27T10:30:00Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### Respuesta con Error (4xx, 5xx)

```json
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "INVALID_REQUEST",
    "message": "Uno o más parámetros son inválidos",
    "details": {
      "field": "amount",
      "reason": "Debe ser mayor a cero"
    }
  },
  "timestamp": "2026-02-27T10:30:45Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### Propiedades Comunes

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `success` | boolean | `true` = éxito, `false` = error |
| `statusCode` | integer | Código HTTP (200, 201, 400, 500, etc.) |
| `data` | object | Recurso o arreglo de recursos |
| `error` | object | Solo si hay error |
| `timestamp` | ISO 8601 | Momento de la respuesta en UTC |
| `correlationId` | UUID | ID de trazabilidad |

---

## Recursos: Cuentas

### 1. Listar Todas las Cuentas

```http
GET /api/v1/accounts
```

**Parámetros de Query (opcionales):**
```
?status=ACTIVE&type=CHECKING&skip=0&limit=10
```

**Respuesta exitosa (200):**
```json
{
  "success": true,
  "statusCode": 200,
  "data": [
    {
      "id": 1,
      "accountNumber": "ACC-001",
      "accountType": "CHECKING",
      "ownerName": "Juan Pérez",
      "balance": 4500.00,
      "status": "ACTIVE",
      "createdAt": "2026-02-27T09:00:00Z",
      "updatedAt": "2026-02-27T10:30:00Z"
    },
    {
      "id": 2,
      "accountNumber": "ACC-002",
      "accountType": "SAVINGS",
      "ownerName": "María García",
      "balance": 2500.00,
      "status": "ACTIVE",
      "createdAt": "2026-02-27T09:15:00Z",
      "updatedAt": "2026-02-27T10:30:00Z"
    }
  ],
  "timestamp": "2026-02-27T10:30:00Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

---

### 2. Crear Nueva Cuenta

```http
POST /api/v1/accounts
Content-Type: application/json
```

**Body:**
```json
{
  "accountNumber": "ACC-003",
  "accountType": "INVESTMENT",
  "ownerName": "Carlos López",
  "initialBalance": 10000.00
}
```

**Validaciones Obligatorias:**
- `accountNumber`: 5-20 caracteres, alfanumérico
- `accountType`: `CHECKING`, `SAVINGS`, `INVESTMENT`
- `ownerName`: 3-100 caracteres
- `initialBalance`: ≥ 0, máximo 999,999,999

**Respuesta exitosa (201):**
```json
{
  "success": true,
  "statusCode": 201,
  "data": {
    "id": 3,
    "accountNumber": "ACC-003",
    "accountType": "INVESTMENT",
    "ownerName": "Carlos López",
    "balance": 10000.00,
    "status": "ACTIVE",
    "createdAt": "2026-02-27T10:35:00Z",
    "updatedAt": "2026-02-27T10:35:00Z"
  },
  "timestamp": "2026-02-27T10:35:00Z",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Errores Posibles:**
```json
// 400 - Número de cuenta duplicado
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "ACCOUNT_NUMBER_DUPLICATE",
    "message": "El número de cuenta ACC-001 ya existe"
  }
}

// 400 - Tipo de cuenta inválido
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "INVALID_ACCOUNT_TYPE",
    "message": "El tipo de cuenta debe ser CHECKING, SAVINGS o INVESTMENT"
  }
}
```

---

### 3. Obtener Cuenta por ID

```http
GET /api/v1/accounts/{id}
```

**Ejemplo:**
```http
GET /api/v1/accounts/1
```

**Respuesta exitosa (200):**
```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "id": 1,
    "accountNumber": "ACC-001",
    "accountType": "CHECKING",
    "ownerName": "Juan Pérez",
    "balance": 4500.00,
    "status": "ACTIVE",
    "createdAt": "2026-02-27T09:00:00Z",
    "updatedAt": "2026-02-27T10:30:00Z"
  },
  "timestamp": "2026-02-27T10:40:00Z",
  "correlationId": "abc-def-123"
}
```

**Error (404):**
```json
{
  "success": false,
  "statusCode": 404,
  "error": {
    "code": "ACCOUNT_NOT_FOUND",
    "message": "La cuenta con ID 999 no existe"
  }
}
```

---

### 4. Actualizar Cuenta

```http
PUT /api/v1/accounts/{id}
Content-Type: application/json
```

**Body:**
```json
{
  "ownerName": "Juan Pedro Pérez",
  "status": "INACTIVE"
}
```

**Campos Actualizables:**
- `ownerName`: string (3-100 caracteres)
- `status`: `ACTIVE`, `INACTIVE`, `SUSPENDED`, `CLOSED`

**Respuesta exitosa (200):**
```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "id": 1,
    "accountNumber": "ACC-001",
    "accountType": "CHECKING",
    "ownerName": "Juan Pedro Pérez",
    "balance": 4500.00,
    "status": "INACTIVE",
    "updatedAt": "2026-02-27T10:45:00Z"
  }
}
```

---

### 5. Eliminar Cuenta (Lógico)

```http
DELETE /api/v1/accounts/{id}
```

**Condiciones:**
- Cuenta debe tener saldo = 0
- No puede estar en transacciones pendientes

**Respuesta exitosa (204 No Content):**
```http
HTTP/1.1 204 No Content
```

**Error (400):**
```json
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "ACCOUNT_HAS_BALANCE",
    "message": "No se puede eliminar una cuenta con saldo positivo"
  }
}
```

---

## Recursos: Transferencias

### 1. Crear Transferencia

```http
POST /api/v1/transfers
Content-Type: application/json
X-Correlation-ID: transfer-12345
```

**Body:**
```json
{
  "sourceAccountId": 1,
  "targetAccountId": 2,
  "amount": 500.00,
  "concept": "Pago de servicios"
}
```

**Validaciones Obligatorias:**
- `sourceAccountId`: cuenta debe existir y estar ACTIVE
- `targetAccountId`: cuenta debe existir y estar ACTIVE
- `amount`: > 0, ≤ 10,000 (límite diario)
- `sourceAccountId` ≠ `targetAccountId`
- Saldo origen ≥ amount
- Máximo 20 transferencias por día (por cuenta origen)
- `concept`: 1-100 caracteres

**Respuesta exitosa (200):**
```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "id": 1,
    "sourceAccountId": 1,
    "targetAccountId": 2,
    "amount": 500.00,
    "concept": "Pago de servicios",
    "status": "COMPLETED",
    "completedAt": "2026-02-27T10:50:00Z",
    "sourceBalanceAfter": 4000.00,
    "targetBalanceAfter": 3000.00
  },
  "timestamp": "2026-02-27T10:50:00Z",
  "correlationId": "transfer-12345"
}
```

**Errores Posibles:**

#### Saldo Insuficiente (400)
```json
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "INSUFFICIENT_FUNDS",
    "message": "Saldo insuficiente para completar la transferencia",
    "details": {
      "available": 300,
      "required": 500,
      "deficit": 200
    }
  }
}
```

#### Cuenta Inactiva (400)
```json
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "ACCOUNT_INACTIVE",
    "message": "La cuenta de origen (ACC-001) está inactiva"
  }
}
```

#### Límite Diario Excedido (400)
```json
{
  "success": false,
  "statusCode": 400,
  "error": {
    "code": "DAILY_LIMIT_EXCEEDED",
    "message": "Límite diario de transferencias excedido",
    "details": {
      "limit": 10000,
      "usedToday": 9700,
      "remaining": 300
    }
  }
}
```

---

### 2. Listar Transferencias

```http
GET /api/v1/transfers?status=COMPLETED&skip=0&limit=20
```

**Parámetros de Query:**
- `status`: `PENDING`, `COMPLETED`, `FAILED`, `CANCELLED`
- `sourceAccountId`: filtrar por cuenta origen
- `skip`: desplazamiento (paginación)
- `limit`: máximo de resultados

**Respuesta:**
```json
{
  "success": true,
  "statusCode": 200,
  "data": [
    {
      "id": 1,
      "sourceAccountId": 1,
      "targetAccountId": 2,
      "amount": 500.00,
      "concept": "Pago de servicios",
      "status": "COMPLETED",
      "createdAt": "2026-02-27T10:50:00Z",
      "completedAt": "2026-02-27T10:50:05Z"
    }
  ],
  "pagination": {
    "skip": 0,
    "limit": 20,
    "total": 1
  }
}
```

---

### 3. Obtener Transferencia por ID

```http
GET /api/v1/transfers/{id}
```

**Respuesta:**
```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "id": 1,
    "sourceAccountId": 1,
    "sourceAccountNumber": "ACC-001",
    "targetAccountId": 2,
    "targetAccountNumber": "ACC-002",
    "amount": 500.00,
    "concept": "Pago de servicios",
    "status": "COMPLETED",
    "createdAt": "2026-02-27T10:50:00Z",
    "completedAt": "2026-02-27T10:50:05Z"
  }
}
```

---

### 4. Historial por Cuenta

```http
GET /api/v1/accounts/{accountId}/transfers?type=incoming
```

**Parámetros:**
- `type`: `incoming`, `outgoing`, `all`
- `skip`, `limit`: paginación

**Respuesta:**
```json
{
  "success": true,
  "statusCode": 200,
  "data": {
    "accountId": 1,
    "accountNumber": "ACC-001",
    "transfers": [
      {
        "id": 1,
        "amount": 500,
        "type": "outgoing",
        "otherParty": "ACC-002 (María García)",
        "status": "COMPLETED",
        "date": "2026-02-27T10:50:00Z"
      }
    ]
  }
}
```

---

## Recursos: Depósitos

### 1. Crear Depósito

```http
POST /api/v1/deposits
Content-Type: application/json
```

**Body:**
```json
{
  "targetAccountId": 1,
  "amount": 2000.00,
  "depositType": "CASH",
  "source": "Depósito en efectivo"
}
```

**Validaciones:**
- `amount`: 0 < amount ≤ 100,000
- `targetAccountId`: cuenta debe existir y estar ACTIVE
- `depositType`: `CASH`, `CHECK`, `WIRE`
- Máximo 1 depósito por minuto

**Respuesta exitosa (201):**
```json
{
  "success": true,
  "statusCode": 201,
  "data": {
    "id": 1,
    "targetAccountId": 1,
    "amount": 2000.00,
    "depositType": "CASH",
    "status": "COMPLETED",
    "newBalance": 6500.00,
    "completedAt": "2026-02-27T11:00:00Z"
  }
}
```

---

### 2. Listar Depósitos

```http
GET /api/v1/deposits?status=COMPLETED&skip=0&limit=20
```

**Respuesta:**
```json
{
  "success": true,
  "statusCode": 200,
  "data": [
    {
      "id": 1,
      "targetAccountId": 1,
      "amount": 2000.00,
      "depositType": "CASH",
      "status": "COMPLETED",
      "createdAt": "2026-02-27T11:00:00Z"
    }
  ]
}
```

---

## Códigos de Error

### Errores de Validación (400 Bad Request)

| Código | Significado |
|--------|-------------|
| `INVALID_REQUEST` | Parámetros inválidos o incompletos |
| `VALIDATION_ERROR` | Error en validación de datos |
| `AMOUNT_INVALID` | Monto inválido (negativo, cero, exceso) |
| `ACCOUNT_NOT_FOUND` | Cuenta no existe |
| `ACCOUNT_INACTIVE` | Cuenta no está activa |
| `ACCOUNT_SUSPENDED` | Cuenta suspendida |
| `INSUFFICIENT_FUNDS` | Saldo insuficiente |
| `DAILY_LIMIT_EXCEEDED` | Límite diario superado |
| `TRANSFER_LIMIT_EXCEEDED` | Número de transferencias excedido |
| `SAME_ACCOUNT_TRANSFER` | Origen y destino son iguales |
| `ACCOUNT_NUMBER_DUPLICATE` | Número de cuenta ya existe |

### Errores de Servidor (500 Internal Server Error)

| Código | Significado |
|--------|-------------|
| `INTERNAL_SERVER_ERROR` | Error no controlado en servidor |
| `DATABASE_ERROR` | Error accediendo a base de datos |
| `SERVICE_UNAVAILABLE` | Servicio no disponible |

---

## Correlation ID & Trazabilidad

### Qué es un Correlation ID

Un **Correlation ID** es un identificador único que vincula todas las operaciones relacionadas en una misma solicitud. Es fundamental para auditoría y debugging.

### Cómo Se Envía

```http
GET /api/v1/accounts/1
X-Correlation-ID: 550e8400-e29b-41d4-a716-446655440000
```

### Cómo Se Recibe

Aparece en toda respuesta:
```json
{
  "correlationId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### Generación Automática

Si NO envías `X-Correlation-ID`, se genera uno automáticamente:
```json
{
  "correlationId": "auto-generated-uuid"
}
```

### Búsqueda en Logs

```bash
# Buscar todas las operaciones con el mismo Correlation ID
grep "550e8400-e29b-41d4-a716-446655440000" banking-*.txt
```

---

## Rate Limiting & Best Practices

### Límites (Lab)
- ⚠️ Este laboratorio **NO implementa rate limiting** formal
- Pero se recomienda: máx 10 requests/segundo por cliente

### Best Practices

✅ **RECOMENDADO:**
1. Incluir siempre `X-Correlation-ID` para trazabilidad
2. Manejar errores 4xx y 5xx correctamente
3. Implementar reintentos exponenciales en clientes
4. Validar siempre la estructura de respuesta
5. Guardar `correlationId` de transferencias críticas

```javascript
// Ejemplo de reintentos en JavaScript
async function transferWithRetry(transferData, maxRetries = 3) {
  for (let i = 0; i < maxRetries; i++) {
    try {
      const response = await fetch('/api/v1/transfers', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'X-Correlation-ID': crypto.randomUUID()
        },
        body: JSON.stringify(transferData)
      });
      
      if (response.ok) return await response.json();
      
      if (response.status === 500) {
        // Reintentar en errores de servidor
        await new Promise(r => setTimeout(r, 1000 * Math.pow(2, i)));
        continue;
      }
      
      // No reintentar en errores de cliente (400)
      return await response.json();
    } catch (error) {
      if (i === maxRetries - 1) throw error;
      await new Promise(r => setTimeout(r, 1000 * Math.pow(2, i)));
    }
  }
}
```

---

## Ejemplos Completos en cURL

### Flujo Completo de Transferencia

```bash
#!/bin/bash

# 1. Crear cuenta origen
ACCOUNT1=$(curl -s -X POST http://localhost:5000/api/v1/accounts \
  -H "Content-Type: application/json" \
  -d '{
    "accountNumber": "ACC-101",
    "accountType": "CHECKING",
    "ownerName": "Origen",
    "initialBalance": 5000
  }' | jq -r '.data.id')

# 2. Crear cuenta destino
ACCOUNT2=$(curl -s -X POST http://localhost:5000/api/v1/accounts \
  -H "Content-Type: application/json" \
  -d '{
    "accountNumber": "ACC-102",
    "accountType": "SAVINGS",
    "ownerName": "Destino",
    "initialBalance": 1000
  }' | jq -r '.data.id')

# 3. Hacer transferencia
TRANSFER_ID=$(curl -s -X POST http://localhost:5000/api/v1/transfers \
  -H "Content-Type: application/json" \
  -H "X-Correlation-ID: my-transfer-001" \
  -d "{
    \"sourceAccountId\": $ACCOUNT1,
    \"targetAccountId\": $ACCOUNT2,
    \"amount\": 500,
    \"concept\": \"Pago de prueba\"
  }" | jq -r '.data.id')

echo "Transferencia creada: $TRANSFER_ID"

# 4. Verificar saldos
echo "Saldo Origen:"
curl -s http://localhost:5000/api/v1/accounts/$ACCOUNT1 | jq '.data.balance'

echo "Saldo Destino:"
curl -s http://localhost:5000/api/v1/accounts/$ACCOUNT2 | jq '.data.balance'
```

---

**¡Esta es la documentación completa de la API! Para más detalles, consulta el código en `src/` o accede a Swagger en `/swagger`.**
