import { fileURLToPath, URL } from 'node:url'

import { defineConfig, ConfigEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig(({ mode }: ConfigEnv) => {
  return {
    plugins: [
      vue(),
      vueDevTools(),
    ],
    base: mode === "production" ? "/TodoApp/" : "/",
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
      },
    },
  };
})
