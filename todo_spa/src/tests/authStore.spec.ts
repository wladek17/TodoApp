import { setActivePinia, createPinia } from "pinia";
import { useAuthStore } from "@/stores/auth";
import { describe, it, expect, beforeEach } from "vitest";

describe("authStore", () => {
  beforeEach(() => {
    localStorage.clear();
    setActivePinia(createPinia());
  });

  it("initializes token and email from localStorage", () => {
    localStorage.setItem("token", "12345");
    localStorage.setItem("userEmail", "test@example.com");

    const auth = useAuthStore();
    auth.initialize();

    expect(auth.token).toBe("12345");
    expect(auth.userEmail).toBe("test@example.com");
  });

  it("clears token and email on logout", () => {
    const auth = useAuthStore();
    auth.token = "12345";
    auth.userEmail = "test@example.com";

    auth.logout();

    expect(auth.token).toBe("");
    expect(auth.userEmail).toBe("");
    expect(localStorage.getItem("token")).toBe(null);
    expect(localStorage.getItem("userEmail")).toBe(null);
  });
});
