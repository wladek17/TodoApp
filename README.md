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
- **SQLite** as the database
- **Entity Framework Core**
- **.NET Identity** for user management
- **JWT (JSON Web Token)** authentication
- **xUnit** + **FluentAssertions**
- **Unit/Integration/E2E tests**
- **Swagger** with JWT support

---

## Project Setup

### Backend (.NET 8 WebAPI)
```bash
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
