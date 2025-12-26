# Help Desk - Sistema de Gestión de Tickets

## Descripción General

Aplicación full-stack para gestión de incidencias (Help Desk) desarrollada con:
- **Backend**: .NET 8 Core, Clean Architecture, EF Core + SQLite
- **Frontend**: Angular 17, Bootstrap 5, Reactive Forms

Una prueba técnica que implementa autenticación JWT, CRUD de tickets, filtrado avanzado y buenas prácticas de software.

---

## Requisitos Previos

### Backend
- **.NET SDK 8.0+** ([descargar](https://dotnet.microsoft.com/en-us/download))
- **Visual Studio 2022 / VS Code** con C# extension

### Frontend
- **Node.js 18+** ([descargar](https://nodejs.org/))
- **Angular CLI 17+** (`npm install -g @angular/cli`)

---

## Instalación y Ejecución

### Backend

```bash
# Navegar a la carpeta del backend
cd Backend

# Restaurar dependencias
dotnet restore

# Aplicar migraciones (crear BD SQLite)
dotnet ef database update --project HelpDesk.Api

# Ejecutar servidor en puerto 5000
dotnet run --project HelpDesk.Api
```

**API disponible en**: `http://localhost:5000`
**Swagger disponible en**: `http://localhost:5000/swagger/index.html`

### Frontend

```bash
# Navegar a la carpeta del frontend
cd Frontend/angular-help-desk

# Instalar dependencias
npm install

# Ejecutar en puerto 4200
ng serve

# O para producción
ng build --configuration production
```

**Aplicación disponible en**: `http://localhost:4200`

---

## Credenciales de Prueba

```
Usuario: admin
Contraseña: 123456
```

---

## Estructura del Proyecto

```
help-desk/
├── Backend/
│   ├── HelpDesk.Domain/           # Entidades principales
│   ├── HelpDesk.Application/      # Lógica de negocio
│   ├── HelpDesk.Infrastructure/   # Persistencia y autenticación
│   ├── HelpDesk.Api/              # Controllers y configuración
│   └── HelpDesk.Tests/            # Tests unitarios
├── Frontend/
│   └── angular-help-desk/         # Aplicación Angular
├── docs/                          # Documentación adicional
└── README.md                      # Este archivo
```

---

## Características Implementadas

### Autenticación
- Login con usuario preconfigurado
- Tokens JWT
- Auth Guard en rutas protegidas
- Interceptor para inyectar token automáticamente

### Gestión de Tickets
- CRUD completo (Crear, Leer, Actualizar, Eliminar)
- Estados: Nuevo, En Progreso, Resuelto
- Prioridades: Baja, Media, Alta
- Filtrado por estado y prioridad
- Paginación en listados

### Interfaz Web
- Formularios reactivos validados
- Tabla responsiva con Bootstrap
- Mensajes de error y éxito visuales
- Diseño limpio y responsive

### Testing
- Tests unitarios en backend (xUnit)
- Tests en frontend (Jasmine)
- Cobertura de casos principales

---

## Decisiones Técnicas

### Backend

| Decisión | Justificación |
|----------|--------------|
| **Clean Architecture** | Separación clara de capas para mantenibilidad |
| **SQLite** | Base de datos ligera, portátil, sin dependencias externas |
| **EF Core** | ORM estándar en .NET, migrations automáticas |
| **FluentValidation** | Validaciones reutilizables y expresivas |
| **JWT** | Autenticación stateless y escalable |
| **Serilog** | Logging estructurado en archivos |
| **Swagger** | Documentación automática de API |

### Frontend

| Decisión | Justificación |
|----------|--------------|
| **Angular 17** | Framework moderno con Signals y standalone components |
| **Reactive Forms** | Control total de validaciones y estado de formularios |
| **Bootstrap 5** | CSS framework rápido y responsive sin overhead |
| **RxJS** | Manejo de asincronía y estado reactivo |
| **Auth Guard** | Protección de rutas con validación de token |
| **Interceptor JWT** | Inyección automática de token en requests |

---

## API Endpoints

### Autenticación
```
POST /api/auth/login
  Body: { "username": "admin", "password": "123456" }
  Response: { "token": "jwt_token", "user": { ... } }

POST /api/auth/logout
  Headers: Authorization: Bearer {token}
```

### Tickets
```
GET /api/tickets?page=1&pageSize=10&status=Nuevo&priority=Alta
  Headers: Authorization: Bearer {token}
  Response: { "items": [...], "total": 10, "page": 1 }

POST /api/tickets
  Headers: Authorization: Bearer {token}
  Body: { "titulo": "...", "descripcion": "...", "prioridad": "Alta", ... }

PUT /api/tickets/{id}
  Headers: Authorization: Bearer {token}
  Body: { "titulo": "...", "estado": "EnProgreso", ... }

DELETE /api/tickets/{id}
  Headers: Authorization: Bearer {token}
```

---

## Ejecución de Tests

### Backend
```bash
cd Backend
dotnet test
```

### Frontend
```bash
cd Frontend/angular-help-desk
npm test
```

---

## Extras

- [ ] Docker Compose para containerizar
- [ ] Búsqueda por texto en tickets

---

## Tecnologías Utilizadas

### Backend
- **.NET 8 Core**
- **Entity Framework Core**
- **FluentValidation**
- **Serilog**
- **Swagger/OpenAPI**
- **xUnit + Moq**

### Frontend
- **Angular 17**
- **Bootstrap 5**
- **RxJS**
- **TypeScript**
- **Jasmine + Karma**

---

La prioridad ha sido la **calidad del código** sobre la cantidad de features, buscando un producto mantenible y profesional.

