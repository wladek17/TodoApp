<script setup lang="ts">
import { useAuthStore } from "@/stores/auth";
import { useRouter } from "vue-router";

const router = useRouter();
const auth = useAuthStore();

function start() {
  if (auth.token) {
    router.push("/tasks");
  } else {
    router.push("/login");
  }
}
</script>

<template>
  <div class="flex items-center justify-center">
    <div class="text-center">
      <h1 class="text-4xl font-bold mb-4">Welcome to TodoApp</h1>
      <p class="text-gray-600 mb-4">
        Organize your tasks, stay focused, and get more done every day.
      </p>
      <button @click="start" class="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700">
        Get Started
      </button>
    </div>
  </div>
</template>
