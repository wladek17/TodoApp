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
  await server.delete(`/api/tasks/${id}`);
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
  <div>
    <div
      style="
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1rem;
      "
    >
      <div><strong>Logged in as:</strong> {{ auth.userEmail }}</div>
      <button @click="logout">Logout</button>
    </div>

    <h1>Your Tasks</h1>

    <ul style="padding: 0">
      <li
        v-for="task in tasks"
        :key="task.id"
        style="
          display: flex;
          align-items: center;
          justify-content: space-between;
          margin-bottom: 0.5rem;
        "
      >
        <label style="flex: 1">
          <input type="checkbox" v-model="task.isCompleted" @change="updateTask(task)" />
          <span :class="{ 'line-through': task.isCompleted }" style="margin-left: 1rem">{{
            task.title
          }}</span>
        </label>
        <button @click="deleteTask(task.id)" style="margin-left: 1rem">🗑</button>
      </li>
    </ul>

    <form @submit.prevent="createTask" style="margin-top: 1rem">
      <input v-model="newTaskTitle" placeholder="New task..." />
      <button type="submit">Add Task</button>
    </form>
  </div>
</template>

<style scoped>
.line-through {
  text-decoration: line-through;
}
</style>
