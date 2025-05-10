<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/auth";

const email = ref("");
const password = ref("");
const error = ref("");

const router = useRouter();
const auth = useAuthStore();

async function handleLogin() {
  try {
    await auth.login(email.value, password.value);
    router.push("/tasks");
  } catch {
    error.value = "Login failed";
  }
}
</script>

<template>
  <form @submit.prevent="handleLogin">
    <input v-model="email" placeholder="Email" />
    <input v-model="password" type="password" placeholder="Password" />
    <button type="submit">Login</button>
    <p v-if="error" class="text-red-500">{{ error }}</p>
  </form>
</template>
