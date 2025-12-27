# Help Desk - Ticket Management System

Full-stack application for incident management with JWT authentication, ticket CRUD and advanced filtering.

**Stack**: .NET 8 + Angular 21 | EF Core + Bootstrap 5 | Clean Architecture

---

## Prerequisites

- **.NET SDK 8.0+**
- **Node.js 18+**
- **Visual Studio 2022 / VS Code**

---

## Quick Start

### Backend
```bash
cd Backend
dotnet restore
dotnet ef database update --project HelpDesk.Api
dotnet run --project HelpDesk.Api
```
**API**: `http://localhost:5264` | **Swagger**: `http://localhost:5264/swagger/index.html`

### Frontend
```bash
cd Frontend
npm install
npm start
```
**App**: `http://localhost:4200`

### Credentials
```
Username: admin
Password: 123456
```

---

## Features

| Feature | Description |
|---------|------------|
| **Authentication** | JWT (8h) + FluentValidation + Auth Guard |
| **CRUD Tickets** | Create/Read/Update/Delete with validations |
| **Filtering** | Status (0-2) + Priority (0-2) in real-time |
| **Pagination** | 5 items/page with navigation |
| **Validations** | Backend (FluentValidation) + Frontend (Reactive Forms) |
| **Testing** | xUnit (5) + Jasmine (7 tests) |
| **Logging** | Structured Serilog |
| **API Docs** | Swagger at `/swagger/index.html` |

---

## API Endpoints

```
POST   /api/auth/login              # Login
GET    /api/tickets?page=1&pageSize=5&status=0&priority=1 # Get with pagination and priority
POST   /api/tickets                 # Create
PUT    /api/tickets/{id}            # Update
DELETE /api/tickets/{id}            # Delete
```

**Status**: 0=New, 1=InProgress, 2=Resolved | **Priority**: 0=Low, 1=Medium, 2=High

---

## Testing

### Backend Tests (xUnit + Moq)
```bash
cd Backend
dotnet test
```

Tests included:
- Valid login
- Invalid password
- Non-existent user
- Inactive user
- General coverage

File: `HelpDesk.Tests/AuthServiceTests.cs` (5 tests)

### Frontend Tests (Jasmine + Karma)
```bash
cd Frontend
npm test
```

Tests included:
- TicketService: getTickets (default, with status, with priority=0)
- TicketService: createTicket
- TicketService: updateTicket
- TicketService: deleteTicket
- App: render router outlet

Files: 
- `src/app/services/ticket.service.spec.ts` (7 tests)
- `src/app/app.spec.ts` (1 smoke test)

**Total**: 13 tests passing
**Coverage**: Auth + CRUD + Routing + Filtering

---

## Stack

**Backend**: .NET 8, EF Core, FluentValidation, Serilog, Swagger, xUnit, Moq  
**Frontend**: Angular 21, Bootstrap 5, RxJS, TypeScript, Jasmine, Karma

---

**Status**: Completed and ready for production

