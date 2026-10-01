# TaskHub

TaskHub is a multi-tenant task management platform built with **.NET 10** using a **Modular Monolith** architecture and **Clean Architecture** principles.

The project is designed as a practical backend project for exploring scalable application architecture, tenant isolation, authentication, authorization, CQRS, and Entity Framework Core.

## 🚀 Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- MediatR
- FluentValidation
- JWT Authentication
- Swagger / OpenAPI
- Dependency Injection

## 🏗️ Architecture

TaskHub follows a **Modular Monolith** architecture.

The application is divided into independent business modules while running as a single deployable application.

```text
TaskHub
│
├── BuildingBlocks
│   ├── Application
│   ├── Domain
│   └── Infrastructure
│
├── Host
│
├── Modules
│   ├── Identity
│   ├── Tenancy
│   └── Tasks
│
└── Tests
Module Structure

Each module follows Clean Architecture boundaries:

Module
│
├── API
├── Application
├── Domain
└── Infrastructure

Dependency direction:

API
 ↓
Application
 ↓
Domain

Infrastructure
 ↓
Application
 ↓
Domain

The Application layer does not depend on Infrastructure.

🔐 Multi-Tenancy

TaskHub supports a hybrid multi-tenant storage strategy.

A tenant can use either a shared database or a dedicated database.

Shared Database

Multiple tenants can share the same database.

Tenant-specific data is isolated using TenantId.

┌──────────────────────────────┐
│        Shared Database       │
│                              │
│ Tenant A → Tasks             │
│ Tenant B → Tasks             │
│ Tenant C → Tasks             │
│                              │
│ Data isolated by TenantId    │
└──────────────────────────────┘
Dedicated Database

A tenant can have its own database.

Tenant A
   ↓
Database A

Tenant B
   ↓
Database B

Tenant C
   ↓
Database C

The tenant's storage strategy determines which database is used.

Tenant Resolution

The current tenant is resolved from the request context.

The application validates the tenant before accessing tenant-specific data.

HTTP Request
     │
     ▼
Tenant Resolution
     │
     ▼
Tenant Context
     │
     ├── TenantId
     ├── Storage Type
     └── Connection String
     │
     ▼
Tenant-aware DbContext

This helps prevent tenant data from being accidentally accessed across tenants.

🔑 Authentication & Authorization

TaskHub uses JWT Bearer Authentication.

The JWT contains information such as:

sub
email
name
role
jti

The sub claim represents the authenticated user's identifier.

Authorization is based on the authenticated user's roles.

🧩 Modules
Identity

Responsible for authentication and user management.

Responsibilities include:

User registration
Login
Password credentials
Roles
JWT generation
User authentication
Tenancy

Responsible for tenant management and tenant infrastructure.

Responsibilities include:

Tenant registration
Tenant membership
Tenant storage configuration
Tenant resolution
Shared database tenants
Dedicated database tenants
Tenant database provisioning
Tasks

Responsible for task-management functionality.

Responsibilities include:

Creating tasks
Updating tasks
Assigning tasks
Completing tasks
Retrieving tenant tasks
Tenant-aware persistence
🧠 CQRS

TaskHub uses CQRS with MediatR.

Commands are responsible for changing application state.

Examples:

CreateTaskCommand
UpdateTaskCommand
CompleteTaskCommand

Queries are responsible for reading data.

Examples:

GetTaskByIdQuery
GetTasksQuery

Conceptually:

             ┌───────────────┐
             │     API       │
             └───────┬───────┘
                     │
              ┌──────▼──────┐
              │   MediatR   │
              └──────┬──────┘
                     │
             ┌───────┴────────┐
             │                │
        Commands           Queries
             │                │
             ▼                ▼
          Write             Read
🗄️ Database & EF Core

Entity Framework Core is used for persistence.

The solution contains multiple DbContext instances owned by their respective modules.

Examples:

IdentityDbContext
TenantRegistryDbContext
TenantDataDbContext

Migrations are generated per module.

Example:

Add-Migration InitialIdentity `
    -Context IdentityDbContext `
    -Project TaskHub.Modules.Identity.Infrastructure `
    -StartupProject TaskHub.Host

Update the database:

Update-Database `
    -Context IdentityDbContext `
    -Project TaskHub.Modules.Identity.Infrastructure `
    -StartupProject TaskHub.Host
⚙️ Configuration

The application uses SQL Server.

Example connection string:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskHub;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

Example JWT configuration:

{
  "JWT": {
    "Issuer": "TaskHub",
    "Audience": "TaskHub",
    "ExpirationMinutes": 60
  }
}

Do not commit real secrets, passwords, JWT signing keys, or production connection strings to source control.

▶️ Running the Project
1. Clone the repository
git clone <your-taskhub-repository-url>
2. Configure the database

Update the connection string in:

src/Host/TaskHub.Host/appsettings.json
3. Apply migrations

From Visual Studio's Package Manager Console:

Update-Database

Or update each module's DbContext individually.

4. Run the application
dotnet run --project src/Host/TaskHub.Host
📚 API Documentation

When the application is running, Swagger/OpenAPI can be used to explore and test the API.

/swagger

Authentication can be tested using the Authorize button with a JWT Bearer token.

🧪 Testing

The solution contains a dedicated test structure:

Tests

The goal is to test application behavior independently from infrastructure concerns.

🎯 Architecture Goals

The project is built to practice and demonstrate:

Modular Monolith architecture
Clean Architecture
CQRS
MediatR
Domain-driven design concepts
Repository pattern
Unit of Work
Dependency Injection
JWT Authentication
Role-based Authorization
Multi-Tenancy
Shared Database tenancy
Dedicated Database tenancy
Tenant isolation
Database provisioning
EF Core migrations
Validation
Separation of concerns
🗺️ Future Improvements

Possible future improvements include:

Refresh Token rotation
More granular permissions
Advanced task filtering
Pagination and sorting
Audit logging
Distributed caching
Integration tests
Docker support
CI/CD pipeline
👨‍💻 Author

Lotfy Khattab

Backend Developer focused on C# / .NET, backend architecture, and scalable application development.

⭐ Project Purpose

TaskHub is primarily a learning and portfolio project focused on understanding how a real-world .NET backend can be structured using modular architecture and multi-tenancy.

The goal is not only to make the application work, but also to understand why each architectural decision exists and how the different parts of the system interact with each other.