<template>
  <div class="app-layout">
    <Sidebar />
    <MainContent />
  </div>
</template>

<script setup>
import Sidebar from './pages/catalog/Sidebar.vue';
import MainContent from './pages/catalog/MainContent.vue';
</script>

<style scoped>
body {
  margin: 0;
  padding: 0;
  font-family: 'Inter', sans-serif;
  box-sizing: border-box;
}

.app-layout {
  display: flex;
  height: 100vh;
  overflow: hidden;
}
</style>