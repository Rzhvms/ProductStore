<template>
  <AdminLayout>
    <template #default>
      <div class="gray-panel">

        <!-- Общий поиск -->
        <div class="search-row">
          <input
            type="text"
            placeholder="Поиск по логину, ФИО или городу"
            v-model="searchQuery"
          />
        </div>

        <!-- Таблица пользователей -->
        <div class="users-grid">

          <!-- Заголовки -->
          <div class="grid-header-row">
            <div class="header-cell col-id">№ID</div>
            <div class="header-cell col-email">ПОЧТА</div>
            <div class="header-cell col-name">Фамилия, имя, отчество</div>
            <div class="header-cell col-city">Город</div>
            <div class="header-cell col-order">Заказ №</div>
          </div>

          <!-- Строки данных -->
          <div
            class="grid-data-row"
            v-for="user in filteredUsers"
            :key="user.id"
          >
            <div class="data-cell col-id">{{ user.id }}</div>
            <div class="data-cell col-email bold">{{ user.email }}</div>
            <div class="data-cell col-name bold">{{ user.name }}</div>
            <div class="data-cell col-city bold">{{ user.city }}</div>
            <div class="data-cell col-order">
              <button class="order-btn">Заказ {{ user.orderId }}</button>
            </div>
          </div>

        </div>

      </div>
    </template>
  </AdminLayout>
</template>

<script setup>
import { ref, computed } from "vue";
import AdminLayout from "./AdminLayout.vue";
import "./admin.css";

// Поиск
const searchQuery = ref("");

// Тестовые пользователи
const users = ref([
  { id: 1, email: "anyemail@gmail.com", name: "Павел Николаевич Ивлев", city: "Санкт-Петербург", orderId: "123" },
  { id: 2, email: "anyemail@gmail.com", name: "Павел Николаевич Ивлев", city: "Екатеринбург", orderId: "15" },
]);

// Фильтрация по поиску
const filteredUsers = computed(() => {
  if (!searchQuery.value) return users.value;
  const q = searchQuery.value.toLowerCase();
  return users.value.filter(
    user =>
      user.email.toLowerCase().includes(q) ||
      user.name.toLowerCase().includes(q) ||
      user.city.toLowerCase().includes(q)
  );
});
</script>