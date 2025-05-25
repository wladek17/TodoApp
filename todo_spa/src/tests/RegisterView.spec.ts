import { mount } from "@vue/test-utils";
import RegisterView from "@/views/RegisterView.vue";
import { describe, it, expect, beforeEach, vi } from "vitest";
import { createPinia, setActivePinia } from "pinia";
import { createRouter, createWebHistory } from "vue-router";
import server from "@/api/server";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', component: { template: '<div>Home</div>' } },
    { path: '/login', component: { template: '<div>Login</div>' } },
    { path: '/register', component: { template: '<div>Register</div>' } },
  ]
})

describe("RegisterView", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it("renders full name, email and password fields", () => {
    const wrapper = mount(RegisterView, {
      global: {
        plugins: [router],
      },
    });

    expect(wrapper.find('input[placeholder="Your full name"]').exists()).toBe(true);
    expect(wrapper.find('input[placeholder="Email"]').exists()).toBe(true);
    expect(wrapper.find('input[placeholder="Password"]').exists()).toBe(true);
  });

  it("shows error message when registration fails", async () => {
    vi.spyOn(server, "post").mockRejectedValue(new Error("Registration failed"));

    const wrapper = mount(RegisterView, {
      global: {
        plugins: [router],
      },
    });

    await wrapper.find('input[placeholder="Your full name"]').setValue("Full name");
    await wrapper.find('input[placeholder="Email"]').setValue("wrongemail@example.com");
    await wrapper.find('input[placeholder="Password"]').setValue("wrongpass");

    await wrapper.find("form").trigger("submit");

    expect(wrapper.text()).toContain("Registration failed");
  });
});
