# API de Gestión Personal Inteligente

## 📋 Descripción

API RESTful desarrollada en **.NET 8** para la gestión de tareas y notas personales con **categorización automática** basada en palabras clave (keywords).

### Características Principales

- ✅ **CRUD completo** para Tareas y Notas
- ✅ **Categorización inteligente** automática por keywords (español/inglés)
- ✅ **Filtros avanzados** (fecha, prioridad, estado)
- ✅ **Paginación** de resultados
- ✅ **Eliminación temporal** (Soft Delete)
- ✅ **Documentación automática** con Swagger
- ✅ **Tests unitarios** con xUnit (40 tests pasando)

---

## 🛠️ Tecnologías

| Categoría | Tecnología |
|-----------|------------|
| **Framework** | .NET 8 Web API |
| **Lenguaje** | C# |
| **ORM** | Entity Framework Core |
| **Base de datos (Dev)** | SQLite |
| **Autenticación** | JWT (Bearer Tokens) |
| **Hash de contraseñas** | BCrypt |
| **Documentación** | Swagger/OpenAPI |
| **Testing** | xUnit + Moq |
| **Patrón de arquitectura** | Clean Architecture |

---

## 🏗️ Arquitectura

```
ApiGestionPersonal/
├── src/
│   ├── ApiGestionPersonal.Api/        # Capa de presentación (Controllers)
│   ├── ApiGestionPersonal.Application/# Capa de aplicación (Commands, Queries, DTOs)
│   ├── ApiGestionPersonal.Domain/      # Capa de dominio (Entities, Enums)
│   └── ApiGestionPersonal.Infrastructure/ # Capa de infraestructura (DB, Repositorios)
└── tests/
    └── ApiGestionPersonal.Tests/       # Tests unitarios
```

### Capas de Clean Architecture

1. **API Layer** - Controladores HTTP, Middleware, Swagger
2. **Application Layer** - CQRS con MediatR, Commands, Queries, DTOs
3. **Domain Layer** - Entidades, Enums, reglas de negocio
4. **Infrastructure Layer** - EF Core, Repositories, Servicios (JWT, Categorización)

---

## 📡 Endpoints

### Autenticación
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/Auth/register` | Registrar nuevo usuario |
| POST | `/api/Auth/login` | Iniciar sesión y obtener token |

### Tareas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Tasks` | Listar tareas (con filtros y paginación) |
| GET | `/api/Tasks/{id}` | Obtener tarea por ID |
| POST | `/api/Tasks` | Crear nueva tarea (auto-categorización) |
| PUT | `/api/Tasks/{id}` | Actualizar tarea |
| DELETE | `/api/Tasks/{id}` | Eliminar tarea (soft delete) |

### Notas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Notes` | Listar notas (con filtros y paginación) |
| GET | `/api/Notes/{id}` | Obtener nota por ID |
| POST | `/api/Notes` | Crear nueva nota |
| PUT | `/api/Notes/{id}` | Actualizar nota |
| DELETE | `/api/Notes/{id}` | Eliminar nota (soft delete) |

### Categorías
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Categories` | Listar categorías |
| POST | `/api/Categories` | Crear categoría |

---

## 🔐 Seguridad

- Todos los endpoints (excepto Auth) requieren **token JWT**
- Contraseñas almacenadas con **hash BCrypt**
- Los secretos se gestionan con **User Secrets** en desarrollo
- Tokens JWT con expiración configurable

---

## 🔍 Filtros Disponibles

### Tareas
- `Page` / `PageSize` - Paginación
- `FechaDesde` / `FechaHasta` - Filtrar por fecha
- `Prioridad` - Filtrar por prioridad (Baja, Media, Alta)
- `Completada` - Filtrar por estado

### Notas
- `Page` / `PageSize` - Paginación
- `CategoriaId` - Filtrar por categoría

---

## 📊 Categorización Automática

La API categoriza automáticamente según keywords encontrados en el contenido:

| Categoría | Keywords (Español) | Keywords (Inglés) |
|-----------|-------------------|-------------------|
| **trabajo** | reunión, oficina, proyecto | meeting, work, job, office, project |
| **compras** | comprar, tienda | buy, shop, store |
| **prioridad-alta** | urgente, importante | urgent, important, critical |
| **comunicación** | llamar, teléfono | call, phone, message |
| **personal** | casa, familia, amigos | home, family, friend |
| **General** | (por defecto) | (default) |

---

## 🚀 Cómo Ejecutar

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 o VS Code

### Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/Osunita/api-gestion-personal.git
cd api-gestion-personal

# 2. Restaurar paquetes
dotnet restore

# 3. Ejecutar la API
dotnet run --project src/ApiGestionPersonal.Api

# 4. Abrir Swagger
# Navegar a: http://localhost:5138
```

### Primeros Pasos

1. Ejecutar la API
2. Abrir Swagger UI
3. Click en **POST /api/Auth/register**
4. Ingresar email y password
5. Copiar el **token** devuelto
6. Click en **Authorize** y pegar el token como `Bearer <token>`
7. Ya puedes usar los endpoints de Tasks, Notes y Categories

