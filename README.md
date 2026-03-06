# Asisya — REST API + SPA

Solución full-stack sobre el modelo de datos Northwind, implementando una API REST en .NET y una SPA en React con autenticación JWT.

---

## Arquitectura

```
asisya/
├── backend/
│   ├── asisya/                  # Capa de presentación (API) — controladores, Program.cs
│   ├── asisya.Application/      # Capa de aplicación — servicios, DTOs, mappers, interfaces
│   ├── asisya.Domain/           # Capa de dominio — entidades, interfaces de repositorio
│   ├── asisya.Infrastructure/   # Capa de infraestructura — EF Core, repositorios, JWT
│   └── asisya.Tests/            # Pruebas unitarias e integración
└── frontend/                    # SPA React + Vite + Tailwind
```

### Decisiones arquitectónicas

- **Arquitectura limpia en capas**: separación estricta entre Domain, Application, Infrastructure y API. Las capas internas no dependen de las externas.
- **DTOs y mapeo explícito**: ninguna entidad del dominio se expone directamente en la API. Se usa `ProductMapper` / `CategoryMapper` con métodos estáticos.
- **Repository pattern**: `IProductRepository`, `ICategoryRepository`, etc. abstraen el acceso a datos; EF Core vive únicamente en Infrastructure.
- **JWT stateless**: tokens firmados con HS256. Todos los endpoints excepto `POST /Auth/login` requieren `[Authorize]`.
- **Batch inserts**: `POST /Product` acepta un `count` y persiste en lotes de 1000 registros, vaciando el ChangeTracker entre lotes para soportar 100 000+ productos sin agotar memoria.
- **Base de datos**: PostgreSQL en producción; SQLite en memoria para las pruebas de integración (via `CustomWebApplicationFactory`).

---

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/) + [pnpm](https://pnpm.io/)
- [PostgreSQL 16+](https://www.postgresql.org/) **o** Docker

---

## Ejecución con Docker Compose

```bash
docker compose up --build
```

| Servicio   | URL                   |
| ---------- | --------------------- |
| API        | http://localhost:8080 |
| Frontend   | http://localhost:3000 |
| PostgreSQL | localhost:5432        |

---

## Ejecución local (sin Docker)

### 1. Base de datos

```bash
# Con una instancia PostgreSQL local corriendo:
# La app aplica las migraciones automáticamente al arrancar.
# Ajusta la cadena de conexión en backend/asisya/appsettings.json si es necesario.
```

### 2. Backend

```bash
cd backend
dotnet restore
dotnet run --project asisya
# API disponible en:
#   http://localhost:5153
#   https://localhost:7273
# Swagger UI: http://localhost:5153/swagger
```

### 3. Frontend

```bash
cd frontend
pnpm install
pnpm dev
# SPA disponible en http://localhost:5173
# El archivo frontend/.env apunta la API a http://localhost:5153
```

**Credenciales de demo**: `admin` / `admin123`

---

## Endpoints principales

| Método | Ruta           | Auth | Descripción                                   |
| ------ | -------------- | ---- | --------------------------------------------- |
| POST   | /Auth/login    | ❌   | Obtener token JWT                             |
| GET    | /Products      | ✅   | Listar con paginación, búsqueda y filtros     |
| GET    | /Products/{id} | ✅   | Detalle con foto de categoría en Base64       |
| POST   | /Products      | ✅   | Crear un producto con datos específicos       |
| POST   | /Product       | ✅   | Generar N productos aleatorios (carga masiva) |
| PUT    | /Products/{id} | ✅   | Actualizar producto (PATCH semántico)         |
| DELETE | /Products/{id} | ✅   | Eliminar producto                             |
| GET    | /Category      | ✅   | Listar categorías                             |
| POST   | /Category      | ✅   | Crear categoría                               |

### Ejemplo — carga masiva de 100 000 productos

```bash
# Variables de entorno según dónde se ejecute:
# Local:  BASE=http://localhost:5153
# Docker: BASE=http://localhost:8080

# 1. Obtener token
TOKEN=$(curl -s -X POST $BASE/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' | jq -r .accessToken)

# 2. Crear categorías
curl -X POST $BASE/Category \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"categoryName":"SERVIDORES","description":"Servidores físicos"}'

curl -X POST $BASE/Category \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"categoryName":"CLOUD","description":"Servicios cloud"}'

# 3. Generar 100 000 productos asociados a la categoría 1
curl -X POST $BASE/Product \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"count":100000,"categoryID":1}'
```

---

## Escalabilidad horizontal en cloud

Para escalar horizontalmente en un entorno cloud se recomienda:

1. **Stateless por diseño**: la API no guarda estado en memoria. Múltiples instancias pueden correr sin coordinación.
2. **Load balancer**: AWS ALB / GCP Load Balancer / Azure Front Door distribuye tráfico entre pods/instancias.
3. **Kubernetes (EKS / GKE / AKS)**: definir un `Deployment` con `replicas: N` y un `HorizontalPodAutoscaler` basado en CPU/requests.
4. **Base de datos gestionada**: RDS (PostgreSQL), Cloud SQL o Azure Database for PostgreSQL con réplicas de lectura para queries pesadas.
5. **Carga masiva asíncrona**: para inserciones de millones de registros, publicar mensajes a una cola (SQS / Pub/Sub / Service Bus) y procesarlos con workers independientes que usen `COPY` de PostgreSQL o `BulkInsert`.
6. **Cache**: agregar Redis (ElastiCache / Memorystore) para cachear resultados de `GET /Products` con filtros frecuentes.
7. **CDN**: servir el frontend desde CloudFront / Cloud CDN / Azure CDN para reducir latencia global.

---

## CI/CD

El pipeline `.github/workflows/ci.yml` ejecuta en cada push/PR a `main`:

- `dotnet build` + `dotnet test` (con PostgreSQL en servicio)
- `docker build` del backend
- `pnpm lint` + `pnpm build` del frontend
- `docker build` del frontend
