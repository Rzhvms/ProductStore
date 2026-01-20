<template>
  <router-view />
</template>

<script setup>
  import { useAuthStore } from './stores/auth'
  import { onMounted } from 'vue'

  const authStore = useAuthStore();

  onMounted(() => {
    if (authStore.isAuthenticated) {
      authStore.setToken(authStore.accessToken);
      authStore.startRefreshTimer();
    }
  });
</script>

<style>
body {
  margin: 0;
  padding: 0;
  font-family: "Libre Franklin", ui-sans-serif, system-ui;
  box-sizing: border-box;
}
</style>