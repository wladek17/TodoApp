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
mysql_main (port 3307) - for development
mysql_test (port 3308) - for tests

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


### Run with Docker (Development)
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

### Production Build (Docker + GHCR)
Production image is built via GitHub Actions and published to GitHub Container Registry (GHCR).

Build and Run Locally:
```bash
docker-compose -f docker-compose.prod.yml up -d --build
```

Or pull from GHCR:
```bash
docker-compose -f docker-compose.prod.yml pull
docker-compose -f docker-compose.prod.yml up -d
```

Backend + Frontend will run at `http://localhost:8080`
Database (MySQL): port 3310

### Docker Compose (Production):

docker-compose.prod.yml example:
```bash
services:
  todoapp:
    image: ghcr.io/wladek17/todoapp:prod
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - ConnectionStrings__DefaultConnection=server=db;port=3306;database=todo;user=root;password=Password123!
    depends_on:
      - db

  db:
    image: mysql:8
    environment:
      - MYSQL_ROOT_PASSWORD=Password123!
      - MYSQL_DATABASE=todo
    volumes:
      - mysql_data:/var/lib/mysql

volumes:
  mysql_data:

```

---

### CI/CD (GitHub Actions)
- Pull Request to dev branch: runs unit tests
- Push to dev branch: runs unit + integration + e2e tests
- Push to master branch: runs unit + integration + e2e tests, builds and pushes production Docker image -> ghcr.io/wladek17/todoapp:prod
- Deploys frontend to GitHub Pages -> https://wladek17.github.io/TodoApp/

### Useful Docker Commands
Start containers
```bash
docker compose up -d
```

Restart containers
```bash
docker compose restart
```

Stop containers
```bash
docker compose down
```

View logs
```bash
docker logs todoapp
```

Rebuild containers (after code changes)
```bash
docker compose up -d --build
```

Pull latest production image from GHCR
```bash
docker pull ghcr.io/wladek17/todoapp:prod
```

Build production image manually
```bash
docker build -t todoapp:prod -f Dockerfile.prod .
```

Push production image to GHCR
```bash
docker push ghcr.io/wladek17/todoapp:prod
```

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
