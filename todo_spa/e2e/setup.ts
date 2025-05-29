import { request } from "@playwright/test";

export async function ensureE2EUserExists() {
  const api = await request.newContext();

  const login = await api.post("http://localhost:5173/api/auth/login", {
    data: {
      email: "e2e@example.com",
      password: "_f0Z3$3{bBw*",
    },
  });

  if (login.status() !== 200) {
    await api.post("http://localhost:5173/api/auth/register", {
      data: {
        fullName: "E2E User",
        email: "e2e@example.com",
        password: "_f0Z3$3{bBw*",
      },
    });
  }

  await api.dispose();
}
