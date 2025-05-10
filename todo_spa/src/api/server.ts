import axios from "axios";
import { useAuthStore } from "@/stores/auth";

const server = axios.create({
  baseURL: "https://localhost:7262/api",
});

server.interceptors.request.use((config) => {
  const auth = useAuthStore();
  if (auth.token) {
    config.headers.Authorization = `Bearer ${auth.token}`;
  }
  return config;
});

export default server;
