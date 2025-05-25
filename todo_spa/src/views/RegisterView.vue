<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/auth";
import server from "@/api/server";

const fullName = ref("");
const email = ref("");
const password = ref("");
const error = ref("");
const router = useRouter();
const auth = useAuthStore();

const register = async () => {
  error.value = "";
  try {
    await server.post("/auth/register", {
      fullName: fullName.value,
      email: email.value,
      password: password.value,
    });

    await auth.login(email.value, password.value);
    router.push("/tasks");
  } catch {
    error.value = "Registration failed";
  }
};
</script>

<template>
  <div class="flex items-center justify-center">
    <div class="w-full max-w-md bg-white p-6 rounded-md">
      <h1 class="text-2xl font-bold text-center mb-6">Create an account</h1>

      <form @submit.prevent="register" class="space-y-4">
        <div>
          <label class="flex font-medium mb-1">Full Name</label>
          <input
            v-model="fullName"
            required
            placeholder="Your full name"
            class="w-full px-4 py-2 border border-gray-300 rounded-md"
          />
        </div>

        <div>
          <label class="flex font-medium mb-1">Email</label>
          <input
            v-model="email"
            type="email"
            required
            placeholder="Email"
            class="w-full px-4 py-2 border border-gray-300 rounded-md"
          />
        </div>

        <div>
          <label class="flex font-medium mb-1">Password</label>
          <input
            v-model="password"
            type="password"
            required
            placeholder="Password"
            class="w-full px-4 py-2 border border-gray-300 rounded-md"
          />
        </div>

        <button
          type="submit"
          class="w-full bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded-md"
        >
          Register
        </button>

        <p v-if="error" class="text-red-600 text-center mt-2">{{ error }}</p>
      </form>

      <p class="text-center text-gray-600 mt-4">
        Already have an account?
        <router-link to="/login" class="text-blue-600 hover:underline">Login</router-link>
      </p>
    </div>
  </div>
</template>
