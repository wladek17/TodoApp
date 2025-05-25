import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import { useAuthStore } from "@/stores/auth";
import LoginView from "../views/LoginView.vue";
import TasksView from "../views/TasksView.vue";
import RegisterView from "../views/RegisterView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {
      path: "/about",
      name: "about",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/AboutView.vue"),
    },
    {
      path: "/login",
      component: LoginView,
    },
    {
      path: "/tasks",
      component: TasksView,
      beforeEnter: () => {
        const auth = useAuthStore();
        if (!auth.token) return "/login";
      },
    },
    {
      path: "/register",
      component: RegisterView,
    },
  ],
});

export default router;
