<template>
  <AdminLayout>
    <template #default>
      <div class="admin-page">
        <h1 class="page-title">Статистика</h1>

        <div class="stats-container">
          <div class="stats-block">
            <h2>Графики и аналитика</h2>
            <p>Здесь в будущем будут графики загрузки, активности, роста и т.д.</p>
          </div>

          <div class="stats-block">
            <h2>История изменений</h2>
            <p>Например: когда обновлялись категории, какие действия делались админами.</p>
          </div>
        </div>
      </div>
    </template>
  </AdminLayout>
</template>

<script setup>
import AdminLayout from './AdminLayout.vue'
import './admin.css'
</script>

<style scoped>
.admin-page {
  padding: 20px;
}

.page-title {
  margin-bottom: 20px;
  font-size: 28px;
  font-weight: bold;
}

.stats-container {
  display: flex;
  flex-direction: column;
  gap: 25px;
}

.stats-block {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0,0,0,0.05);
}
</style>
