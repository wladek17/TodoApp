# Todo App

Todo App built with **Vue 3 + TypeScript** and **.NET 8 WebAPI**.  
It supports user authentication with JWT, task CRUD operations, and includes unit and e2e tests using **Vitest**, **xUnit**, and **Playwright**.

---

## Tech Stack

### Frontend
- **Vue 3** + **TypeScript** + **Vite**
- **Pinia** for state management
- **Vue Router**
- **Axios**
- **Tailwind CSS**
- **Vitest** for unit tests
- **Playwright** for end-to-end tests

### Backend
- **.NET 8 WebAPI**
- **MySQL** as the database (via Docker Compose)
- **Entity Framework Core** with **Pomelo.EntityFrameworkCore.MySql**
- **.NET Identity** for user management
- **JWT (JSON Web Token)** authentication
- **xUnit** + **FluentAssertions**
- **Unit/Integration/E2E tests**
- **Swagger** with JWT support

---

## Project Setup

### Database (MySQL with Docker Compose)
Make sure Docker is installed. Start MySQL containers:

```bash
docker compose up -d
```

This starts two databases:
mysql_main (port 3307) � for development
mysql_test (port 3308) � for tests

Stop containers:
```bash
docker compose down
```

### Backend (.NET 8 WebAPI)
```bash
cd TodoApi
dotnet restore
dotnet ef database update
dotnet run
```
The API will be available at `https://localhost:7262/swagger/index.html`

### Frontend (Vue 3)
```bash
cd todo_spa
yarn install
yarn dev
```
The frontend will run at `http://localhost:5173`


### Run with Docker
Make sure Docker is installed.

Start all services (backend, frontend, MySQL main & test):
```bash
docker compose up --build
```

Backend (ASP.NET Core WebAPI + Swagger): https://localhost:7262/
Frontend (Vue 3 + Vite): http://localhost:5173/

Two MySQL databases run in containers:
mysql_main -> todo (main DB, port 3307)
mysql_test -> todo_test (test DB, port 3308)

Stop and remove containers:
```bash
docker compose down
```

---

## Testing

### Frontend Tests

#### E2E Tests (Playwright)

Make sure both the **backend** and **frontend** are running before running e2e tests.

Install Playwright Browsers:
```bash
yarn playwright install
```

Run E2E Tests:
```bash
yarn test:e2e
```

Open Last HTML Report:
```bash
yarn playwright show-report
```

#### Run Unit Tests:
```bash
yarn test:unit
```

---

### Backend Tests

Make sure mysql_test container is running before executing backend tests:

```bash
docker compose up -d mysql_test
```

#### Unit Tests
```bash
cd TodoApi.Tests
dotnet test --filter FullyQualifiedName~UnitTests
```

#### Integration Tests
```bash
cd TodoApi.Tests
dotnet test --filter FullyQualifiedName~IntegrationTests
```

#### E2E Tests
```bash
cd TodoApi.Tests
dotnet test --filter FullyQualifiedName~E2E
```

Test GitHub Actions workflow
