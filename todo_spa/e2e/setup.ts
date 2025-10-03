import { request } from "@playwright/test";

export async function ensureE2EUserExists() {
  const api = await request.newContext();
  const apiUrl = process.env.VITE_API_URL || "https://localhost:7262/api";

  const login = await api.post(`${apiUrl}/auth/login`, {
    data: {
      email: "e2e@example.com",
      password: "_f0Z3$3{bBw*",
    },
  });

  console.log("Login status:", login.status(), await login.text());

  if (login.status() !== 200) {
    const register = await api.post(`${apiUrl}/auth/register`, {
      data: {
        fullName: "E2E User",
        email: "e2e@example.com",
        password: "_f0Z3$3{bBw*",
      },
    });

    console.log("Register status:", register.status(), await register.text());
  }

  await api.dispose();
}
