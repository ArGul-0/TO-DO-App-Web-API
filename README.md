# 🚀 ToDoApp — ASP.NET Core Web API

![Release](https://img.shields.io/github/v/release/ArGul-0/TO-DO-App-Web-API)
![Downloads](https://img.shields.io/github/downloads/ArGul-0/TO-DO-App-Web-API/total)
![Last Commit](https://img.shields.io/github/last-commit/ArGul-0/TO-DO-App-Web-API)
![Repo Size](https://img.shields.io/github/repo-size/ArGul-0/TO-DO-App-Web-API)
![Top Language](https://img.shields.io/github/languages/top/ArGul-0/TO-DO-App-Web-API)

A modern and production-ready ToDo Web API built with **ASP.NET Core**, **Clean Architecture**, **JWT Authentication**, **PostgreSQL**, and **Docker**.

This project was designed as a scalable backend foundation for a notes / task management platform with secure authentication, domain separation, infrastructure isolation, and automated unit testing.

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
* Update notes
* Delete notes
* Get all notes for authenticated user
* Get note by ID
* One-to-many relationship between users and notes

## 👤 Users System

* User registration
* User login
* Get info about current authenticated user
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

## 🧪 Automated Testing

The project includes a dedicated unit testing suite built with **xUnit**, **FluentAssertions**, and **Moq**.

### Tested Features

* User registration
* User authentication
* Note creation
* Note retrieval
* Note updating
* Note deletion
* Friend requests
* Friend request acceptance
* Friend request rejection
* Friend removal
* Friendship retrieval
* Validation and error scenarios

### Testing Stack

| Technology       | Purpose              |
| ---------------- | -------------------- |
| xUnit            | Test framework       |
| FluentAssertions | Readable assertions  |
| Moq              | Mocking dependencies |

Tests primarily target **Application Layer use cases and handlers**, isolating business logic from external infrastructure.

---

# 🏗 Architecture

* Clean Architecture
* Domain-Driven Design principles
* CQRS-like Use Cases / Handlers
* Repository Pattern
* Unit Of Work Pattern
* DTO separation
* Value Objects

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
├── ToDoApp.Application.Tests
│   └── Users               → User use case tests
│   └── Notes               → Note use case tests
│   └── Friendships         → Friendship use case tests
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

# 🧪 Testing Architecture

Unit tests are separated from the production application and focus primarily on the **Application Layer**.

```text
ToDoApp.Application.Tests
│
├── Users
│   ├── CreateUserHandlerTests
│   └── LoginUserHandlerTests
│
├── Notes
│   ├── CreateNoteHandlerTests
│   ├── GetNoteHandlerTests
│   ├── GetNotesHandlerTests
│   ├── UpdateNoteHandlerTests
│   └── DeleteNoteHandlerTests
│
└── Friendships
    ├── SendFriendRequestHandlerTests
    ├── AcceptFriendRequestHandlerTests
    ├── RejectFriendRequestHandlerTests
    ├── RemoveFriendHandlerTests
    └── GetFriendsHandlerTests
```

The tests use mocks to isolate application logic from repositories, unit-of-work implementations, logging, and other external dependencies.

This allows individual use cases to be tested independently and deterministically.

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
| xUnit                 | Unit testing      |
| FluentAssertions      | Test assertions   |
| Moq                   | Mocking           |

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
| GET    | `/Users/Me`   | Get info about current authenticated user |

---

## Notes

| Method | Endpoint      | Description                       |
| ------ | ------------- | --------------------------------- |
| GET    | `/Notes`      | Get all notes for authorized user |
| GET    | `/Notes/{id}` | Get note by ID                    |
| GET    | `/Notes/Me`   | Get all notes for authenticated user |
| POST   | `/Notes`      | Create new note                   |
| PUT    | `/Notes/{id}` | Update note                       |
| DELETE | `/Notes/{id}` | Delete note                       |

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

# 🧪 Running Tests

Run the complete test suite with:

```bash
dotnet test
```

To run tests with detailed output:

```bash
dotnet test --verbosity normal
```

---

# 📚 Swagger UI

Swagger is enabled automatically.

After launching the application open:

```text
localhost/
```

or

```text
localhost:5000/
```

---

# 🗃️ Seq (logs)

Seq is enabled automatically.

After launching the application open:

```text
localhost:5341
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

Developed by **ArGul**.

If you like this project — consider giving it a ⭐ on GitHub.
