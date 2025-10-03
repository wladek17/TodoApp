import axios from "axios";
import { useAuthStore } from "@/stores/auth";

const server = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

server.interceptors.request.use((config) => {
  const auth = useAuthStore();
  if (auth.token) {
    config.headers.Authorization = `Bearer ${auth.token}`;
  }
  return config;
});

export default server;
