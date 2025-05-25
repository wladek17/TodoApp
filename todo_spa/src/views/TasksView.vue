<script setup lang="ts">
import { ref, onMounted } from "vue";
import server from "@/api/server";
import { useAuthStore } from "@/stores/auth";
import { useRouter } from "vue-router";

interface TodoTask {
  id: number;
  title: string;
  isCompleted: boolean;
}

const tasks = ref<TodoTask[]>([]);
const newTaskTitle = ref("");

async function fetchTasks() {
  const response = await server.get("/tasks");
  tasks.value = response.data;
}

async function createTask() {
  const response = await server.post("/tasks", {
    title: newTaskTitle.value,
    isCompleted: false,
  });
  tasks.value.push(response.data);
  newTaskTitle.value = "";
}

async function updateTask(task: TodoTask) {
  await server.put(`/tasks/${task.id}`, task);
}

async function deleteTask(id: number) {
  await server.delete(`/tasks/${id}`);
  tasks.value = tasks.value.filter((t) => t.id !== id);
}

const auth = useAuthStore();
const router = useRouter();

function logout() {
  auth.logout();
  router.push("/login");
}

onMounted(fetchTasks);
</script>

<template>
  <div class="bg-gray-50 p-6">
    <div class="max-w-2xl mx-auto bg-white p-6 rounded-xl">
      <div class="flex justify-between items-center mb-6">
        <div class="text-gray-600"><strong>Logged in as:</strong> {{ auth.userEmail }}</div>
        <button @click="logout" class="text-red-600 hover:underline">Logout</button>
      </div>

      <h1 class="text-2xl font-bold text-gray-800 mb-4">Your Tasks</h1>

      <ul class="space-y-3 mb-6">
        <li
          v-for="task in tasks"
          :key="task.id"
          class="flex items-center justify-between bg-gray-100 rounded px-4 py-2"
        >
          <label class="flex-1 flex items-center space-x-3">
            <input type="checkbox" v-model="task.isCompleted" @change="updateTask(task)" />
            <span :class="{ 'line-through text-gray-400': task.isCompleted }">
              {{ task.title }}
            </span>
          </label>
          <button @click="deleteTask(task.id)" class="hover:text-red-700" title="Delete">
            &#10008;
          </button>
        </li>
      </ul>

      <form @submit.prevent="createTask" class="flex space-x-2">
        <input
          v-model="newTaskTitle"
          placeholder="New task..."
          class="flex-1 px-3 py-2 border rounded-md"
        />
        <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700">
          Add
        </button>
      </form>
    </div>
  </div>
</template>
