# 🚀 ToDoApp — ASP.NET Core Web API

A modern and production-ready ToDo Web API built with **ASP.NET Core**, **Clean Architecture**, **JWT Authentication**, **PostgreSQL**, and **Docker**.

This project was designed as a scalable backend foundation for a notes / task management platform with secure authentication, domain separation, and infrastructure isolation.

---

# ✨ Features

## 🔐 Authentication & Security

* JWT Authentication
* JWT stored in secure HttpOnly Cookies
* Argon2 password hashing
* Authorization using `.RequireAuthorization()`
* Global exception handling middleware
* Secure cookie policy configuration
* HTTPS support

## 📝 Notes System

* Create notes
* Get all notes for authenticated user
* Get note by ID
* One-to-many relationship between users and notes

## 👤 Users System

* User registration
* User login
* Get all users
* Get user by ID

## 🤝 Friendship System

* Send friend requests
* Accept friend requests
* Reject friend requests
* Remove friends
* View all friends
* View incoming friend requests
* Friendship status management (`Pending`, `Accepted`, `Rejected`)

## 🏗 Architecture

* Clean Architecture
* Domain-Driven Design principles
* CQRS-like Use Cases / Handlers
* Repository Pattern
* Unit Of Work Pattern
* DTO separation
* Value Objects

## ⚙️ Infrastructure

* PostgreSQL + Entity Framework Core
* EF Core migrations
* Docker support
* Docker Compose for development and production
* Nginx reverse proxy configuration
* Health Check endpoint
* Swagger / OpenAPI documentation
* Structured logging with Serilog

---

# 🧱 Project Structure

```text
ToDoApp
│
├── ToDoApp.Domain          → Core business entities and value objects
├── ToDoApp.Application     → Use cases, DTOs, interfaces, business logic
├── ToDoApp.Infrastructure  → Database, repositories, authentication, persistence
├── ToDoApp.WebApi          → Minimal API endpoints and application entry point
│
├── nginx                   → Nginx reverse proxy configuration
├── docker-compose.dev.yaml
├── docker-compose.prod.yaml
└── Dockerfile
```

---

# 🧠 Architecture Overview

The project follows a layered Clean Architecture approach:

## Domain Layer

Contains:

* Entities
* Value Objects
* Core business rules

This layer has **zero dependencies** on external frameworks.

---

## Application Layer

Contains:

* Use Cases
* DTOs
* Repository Interfaces
* Validation
* Result/Error abstractions

Business logic lives here.

---

## Infrastructure Layer

Contains:

* Entity Framework Core
* PostgreSQL integration
* JWT token generation
* Argon2 password hashing
* Repository implementations
* Dependency Injection configuration
* Nginx

---

## Web API Layer

Contains:

* Minimal API endpoints
* Authentication setup
* Middleware configuration
* Swagger
* HTTP concerns

---

# 🛠 Tech Stack

| Technology            | Description       |
| --------------------- | ----------------- |
| ASP.NET Core          | Web API framework |
| .NET 10               | Runtime           |
| Entity Framework Core | ORM               |
| PostgreSQL            | Database          |
| JWT                   | Authentication    |
| Argon2                | Password hashing  |
| Docker                | Containerization  |
| Nginx                 | Reverse proxy     |
| Serilog               | Logging           |
| Swagger/OpenAPI       | API documentation |

---

# 🔑 Authentication Flow

1. User registers or logs in
2. Server generates JWT token
3. JWT is stored in secure HttpOnly cookie
4. Authorized endpoints validate token automatically
5. User identity extracted from claims

---

# 📌 API Endpoints

## Authentication

| Method | Endpoint         | Description       |
| ------ | ---------------- | ----------------- |
| POST   | `/Auth/Register` | Register new user |
| POST   | `/Auth/Login`    | Login user        |

---

## Users

| Method | Endpoint      | Description    |
| ------ | ------------- | -------------- |
| GET    | `/Users`      | Get all users  |
| GET    | `/Users/{id}` | Get user by ID |

---

## Notes

| Method | Endpoint               | Description                       |
| ------ | ---------------------- | --------------------------------- |
| GET    | `/Notes`               | Get all notes for authorized user |
| GET    | `/Notes/{id}`          | Get note by ID                    |
| POST   | `/Notes`               | Create new note                   |
| PUT    | `/Notes/{id}`          | Update note                       |
| DELETE | `/Notes/{id}`          | Delete note                       |

---

## Friendships

| Method | Endpoint                     | Description                  |
| ------ | ---------------------------- | ---------------------------- |
| GET    | `/Friends`                   | Get all friends              |
| GET    | `/Friends/Incoming`          | Get incoming friend requests |
| POST   | `/Friends/{friendId}`        | Send friend request          |
| PUT    | `/Friends/{friendId}/Accept` | Accept friend request        |
| PUT    | `/Friends/{friendId}/Reject` | Reject friend request        |
| DELETE | `/Friends/{friendId}`        | Remove friend                |

## Health Check

| Method | Endpoint  |
| ------ | --------- |
| GET    | `/health` |

---

# 🐳 Running With Docker

## Development

```bash
docker compose -f docker-compose.dev.yaml up
```

---

## Production

```bash
docker compose -f docker-compose.prod.yaml up -d
```

---

# ⚡ Local Development

## 1. Clone Repository

```bash
git clone <repository-url>
cd ToDoApp
```

---

## 2. Run Dev-Container

```bash
docker compose -f docker-compose.dev.yaml up --build
```

---

## 3. Run Application

```bash
dotnet run --project ToDoApp.WebApi
```

---

# 📚 Swagger UI

Swagger is enabled automatically.

After launching the application:

```text
https://localhost/
```

or

```text
http://localhost:5000/
```

---

# 🗃️ Seq (logs)

Seq is enabled automatically.

After launching the application:

```text
https://localhost/5341
```

User - Admin
Password - Admin

---

# 🔒 Security Notes

The application uses:

* Secure HttpOnly cookies
* Strict SameSite policy
* HTTPS-only cookies
* Argon2 password hashing
* JWT validation
* Centralized exception handling

---

# 📄 License

This project is licensed under the MIT License.

See `LICENSE.txt` for more information.

---

# 👨‍💻 Author

Developed by ArGul.

If you like this project — consider giving it a ⭐ on GitHub.
