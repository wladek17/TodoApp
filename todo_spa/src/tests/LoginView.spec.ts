import { mount } from "@vue/test-utils";
import LoginView from "@/views/LoginView.vue";
import { describe, it, expect, beforeEach, vi } from "vitest";
import { createPinia, setActivePinia } from "pinia";
import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "@/stores/auth";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", component: { template: "<div>Home</div>" } },
    { path: "/register", component: { template: "<div>Register</div>" } },
  ],
});

describe("LoginView", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it("renders email and password fields", () => {
    const wrapper = mount(LoginView, {
      global: {
        plugins: [createPinia(), router],
      },
    });

    expect(wrapper.find('input[placeholder="Email"]').exists()).toBe(true);
    expect(wrapper.find('input[placeholder="Password"]').exists()).toBe(true);
  });

  it("shows error when login fails", async () => {
    const wrapper = mount(LoginView, {
      global: {
        plugins: [createPinia(), router],
      },
    });

    const auth = useAuthStore();
    auth.login = vi.fn().mockRejectedValue(new Error("Invalid credentials"));

    await wrapper.find('input[placeholder="Email"]').setValue("wrongemail@example.com");
    await wrapper.find('input[placeholder="Password"]').setValue("wrongpass");

    await wrapper.find("form").trigger("submit");

    expect(wrapper.text()).toContain("Login failed");
  });
});
