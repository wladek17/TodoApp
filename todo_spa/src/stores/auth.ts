import { defineStore } from "pinia";
import server from "@/api/server";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: "",
    userEmail: "",
  }),
  actions: {
    async login(email: string, password: string) {
      const response = await server.post("/auth/login", { email, password });
      this.token = response.data.token;
      this.userEmail = email;
      localStorage.setItem("token", this.token);
      localStorage.setItem("userEmail", email);
      server.defaults.headers.common["Authorization"] = `Bearer ${this.token}`;
    },
    logout() {
      this.token = "";
      this.userEmail = "";
      localStorage.removeItem("token");
      localStorage.removeItem("userEmail");
      delete server.defaults.headers.common["Authorization"];
    },
    initialize() {
      const savedToken = localStorage.getItem("token");
      const savedEmail = localStorage.getItem("userEmail");
      if (savedToken) {
        this.token = savedToken;
        server.defaults.headers.common["Authorization"] = `Bearer ${savedToken}`;
      }
      if (savedEmail) {
        this.userEmail = savedEmail;
      }
    },
  },
});
