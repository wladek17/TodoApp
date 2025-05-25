<script setup lang="ts">
import { RouterView, RouterLink } from "vue-router";
import { useAuthStore } from "@/stores/auth";

const auth = useAuthStore();
</script>

<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <header class="bg-white shadow px-6 py-4 flex justify-between items-center">
      <RouterLink to="/" class="text-xl font-bold text-blue-600">TodoApp</RouterLink>
      <nav class="space-x-4">
        <RouterLink to="/" class="hover:underline">Home</RouterLink>
        <RouterLink to="/tasks" class="hover:underline">Tasks</RouterLink>
        <RouterLink to="/about" class="hover:underline">About</RouterLink>
        <span v-if="auth.token" class="text-gray-500">| {{ auth.userEmail }}</span>
      </nav>
    </header>

    <main class="flex-1 p-6">
      <RouterView />
    </main>

    <footer class="bg-gray-100 text-center text-gray-500 py-4">
      &copy; {{ new Date().getFullYear() }} TodoApp
    </footer>
  </div>
</template>
