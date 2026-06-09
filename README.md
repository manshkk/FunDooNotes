# Fundoo Notes API

## Overview

Fundoo Notes is a RESTful Notes Management API inspired by Google Keep. The application allows users to register, authenticate using JWT, manage notes, organize them with labels, archive notes, pin notes, assign colors, and manage trash operations.

The project is developed using ASP.NET Core 8 Web API with a layered architecture following Repository Pattern and Entity Framework Core.

---

# Features

## User Management

* User Registration
* User Login
* JWT Authentication
* Get User Profile
* Forgot Password
* Reset Password via Email

## Notes Management

* Create Note
* Get All Notes
* Get Note By Id
* Update Note
* Move Note to Trash

## Trash Module

* Get Trashed Notes
* Restore Note from Trash
* Permanently Delete Note

## Archive Module

* Archive Note
* Unarchive Note
* Get Archived Notes

## Pin Module

* Pin Note
* Unpin Note

## Color Module

* Change Note Color

## Label Module

* Create Label
* Update Label
* Delete Label
* Add Label to Note
* Remove Label from Note

---

# Technology Stack

## Backend

* ASP.NET Core 8 Web API
* C#

## Database

* Microsoft SQL Server
* Entity Framework Core

## Authentication

* JWT (JSON Web Token)

## Email Service

* MailKit
* MimeKit

## API Testing

* Swagger UI
* Postman

## Version Control

* Git
* GitHub

---

# Project Architecture

```text
FundooNotes
│
├── Controllers
│
├── BusinessLayer
│   ├── Interfaces
│   └── Services
│
├── RepositoryLayer
│   ├── Interfaces
│   ├── Services
│   └── Context
│
├── ModelLayer
│   ├── Entities
│   └── DTOs
│
└── SQL Server Database
```

## Architecture Layers

### Controller Layer

Handles HTTP requests and responses.

### Business Layer

Contains business logic and validations.

### Repository Layer

Handles database operations using Entity Framework Core.

### Model Layer

Contains Entities and DTOs.

---

# Database Tables

## Users

Stores user information and authentication details.

## Notes

Stores note details and metadata.

## Labels

Stores user-created labels.

## NoteLabels

Maintains the many-to-many relationship between Notes and Labels.

---

# Authentication

JWT Authentication is implemented for securing APIs.

Protected endpoints require a valid JWT token.

### Authorization Header

```text
Authorization: Bearer <JWT_TOKEN>
```

---

# Getting Started

## Clone Repository

```bash
git clone <repository-url>
```

## Navigate to Project

```bash
cd FundooNotes
```

## Restore Packages

```bash
dotnet restore
```

## Configure Database

Update the connection string in:

```text
appsettings.json
```

## Apply Migrations

```powershell
Update-Database
```

## Run Application

```bash
dotnet run
```

## Open Swagger

```text
https://localhost:<port>/swagger
```

---

# API Testing

## Using Swagger

Swagger UI is integrated into the project and can be used to test all endpoints.

```text
https://localhost:<port>/swagger
```

## Using Postman

### Step 1

Login using:

```http
POST /api/User/login
```

### Step 2

Copy the JWT token from the response.

### Step 3

Open the Authorization tab in Postman.

```text
Type: Bearer Token
```

Paste the JWT token.

### Step 4

Execute any protected API endpoint.

---

# API Endpoints

## User APIs

| Method | Endpoint                  |
| ------ | ------------------------- |
| POST   | /api/User/register        |
| POST   | /api/User/login           |
| GET    | /api/User/profile         |
| POST   | /api/User/forgot-password |
| POST   | /api/User/reset-password  |

## Note APIs

| Method | Endpoint       |
| ------ | -------------- |
| POST   | /api/Note      |
| GET    | /api/Note      |
| GET    | /api/Note/{id} |
| PUT    | /api/Note/{id} |
| DELETE | /api/Note/{id} |

## Trash APIs

| Method | Endpoint                 |
| ------ | ------------------------ |
| GET    | /api/Note/trash          |
| PATCH  | /api/Note/{id}/restore   |
| DELETE | /api/Note/{id}/permanent |

## Archive APIs

| Method | Endpoint                 |
| ------ | ------------------------ |
| GET    | /api/Note/archive        |
| PATCH  | /api/Note/{id}/archive   |
| PATCH  | /api/Note/{id}/unarchive |

## Pin APIs

| Method | Endpoint             |
| ------ | -------------------- |
| PATCH  | /api/Note/{id}/pin   |
| PATCH  | /api/Note/{id}/unpin |

## Color APIs

| Method | Endpoint             |
| ------ | -------------------- |
| PATCH  | /api/Note/{id}/color |

## Label APIs

| Method | Endpoint                    |
| ------ | --------------------------- |
| POST   | /api/Label                  |
| PUT    | /api/Label/{id}             |
| DELETE | /api/Label/{id}             |
| POST   | /api/Label/add-to-note      |
| DELETE | /api/Label/remove-from-note |

---

# Design Patterns Used

* Repository Pattern
* Dependency Injection
* Layered Architecture
* Separation of Concerns

---

# Author

**Manish Kumar Kaushal**

ASP.NET Core Backend Developer


