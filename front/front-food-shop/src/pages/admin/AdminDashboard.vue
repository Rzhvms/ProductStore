<template>
  <AdminLayout>
    <template #default>
      <div class="admin-page">
        <h1 class="page-title">Главная</h1>

        <div class="dashboard-grid">
          <div class="dashboard-card">
            <h2>Общая статистика</h2>
            <p>Тут появятся ключевые показатели.</p>
          </div>

          <div class="dashboard-card">
            <h2>Активность пользователей</h2>
            <p>Графики, таблицы, события и т.д.</p>
          </div>

          <div class="dashboard-card">
            <h2>Системные уведомления</h2>
            <p>Логи, алерты и техническая информация.</p>
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

.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 20px;
}

.dashboard-card {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0,0,0,0.05);
}
</style>
