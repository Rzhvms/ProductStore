<template>
  <div class="admin-panel">
    <!-- Хедер -->
    <header class="header">
      <div class="logo-area">
        <div class="logo-icon">
          <svg viewBox="0 0 24 24" width="30" height="30" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M4 6h16M4 12h16M4 18h8" stroke="black" stroke-linecap="round" />
            <circle cx="12" cy="12" r="10" stroke="black" />
          </svg>
        </div>
        <h1 class="brand-name">FANTECH — PANEL</h1>
      </div>
      <button class="user-btn">Никнейм</button>
    </header>

    <div class="content-wrapper">
      <!-- Сайдбар -->
      <aside class="sidebar">
        <nav>
          <ul>
            <li v-for="item in menuItems" :key="item.name">
              <button 
                class="nav-item" 
                :class="{ active: item.active }"
              >
                {{ item.name }}
              </button>
            </li>
          </ul>
        </nav>
      </aside>

      <!-- Основной контент -->
      <main class="main-content">
        <!-- Большая серая панель -->
        <div class="gray-panel">
          
          <!-- Блок фильтров -->
          <div class="filters-row">
            <div class="filter-group">
              <label>Поиск по логину:</label>
              <input type="text" placeholder="Введите логин:" v-model="filters.login" />
            </div>
            <div class="filter-group">
              <label>Поиск по ФИО:</label>
              <input type="text" placeholder="Введите ФИО:" v-model="filters.name" />
            </div>
            <div class="filter-group">
              <label>Поиск по городу:</label>
              <input type="text" placeholder="Введите город:" v-model="filters.city" />
            </div>
          </div>

          <!-- Таблица пользователей (Grid Layout) -->
          <div class="users-grid">
            
            <!-- Заголовки (выглядят как отдельные блоки) -->
            <div class="grid-header-row">
              <div class="header-cell col-id">№ID</div>
              <div class="header-cell col-email">ПОЧТА</div>
              <div class="header-cell col-name">Фамилия, имя, отчество</div>
              <div class="header-cell col-city">Город</div>
              <div class="header-cell col-order">Заказ №</div>
            </div>

            <!-- Строки данных -->
            <div class="grid-data-row" v-for="user in users" :key="user.id">
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
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue';

// Меню
const menuItems = ref([
  { name: 'На страницу магазина', active: false },
  { name: 'Новости', active: false },
  { name: 'Товары', active: false },
  { name: 'Категории', active: false },
  { name: 'Бренды', active: false },
  { name: 'Пользователи', active: true }, // Активно
  { name: 'Статистика', active: false },
]);

// Фильтры
const filters = reactive({
  login: '',
  name: '',
  city: ''
});

// Данные пользователей (как на скрине)
const users = ref([
  {
    id: 1,
    email: 'anyemail@gmail.com',
    name: 'Павел Николаевич Ивлев',
    city: 'Санкт-Петербург',
    orderId: '123'
  },
  {
    id: 2,
    email: 'anyemail@gmail.com',
    name: 'Павел Николаевич Ивлев',
    city: 'Екатеринбург',
    orderId: '15'
  }
]);
</script>

<style scoped>
.admin-panel {
  font-family: 'Inter', 'Roboto', sans-serif;
  color: #000;
  background-color: #fff;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* --- HEADER & SIDEBAR (Общие стили) --- */
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 40px;
  border-bottom: 1px solid #eee;
}

.logo-area { display: flex; align-items: center; gap: 12px; }
.brand-name { font-size: 20px; font-weight: 700; text-transform: uppercase; margin: 0; }
.user-btn {
  background: white; border: 1px solid #000; padding: 8px 25px;
  font-weight: 600; border-radius: 6px; font-size: 14px; cursor: pointer;
}
.user-btn:hover { background: #000; color: #fff; }

.content-wrapper { display: flex; flex: 1; padding: 30px 40px; gap: 40px; }
.sidebar { width: 240px; flex-shrink: 0; background-color: #f6f6f6; border-radius: 8px; padding: 20px 0; height: fit-content; min-height: 400px; }
.sidebar ul { list-style: none; padding: 0; margin: 0; }
.nav-item {
  width: 100%; text-align: left; background: transparent; border: none;
  padding: 12px 25px; font-weight: 700; font-size: 14px; cursor: pointer; color: #000;
}
.nav-item.active {
  background: #fff; box-shadow: 0 2px 8px rgba(0,0,0,0.05);
  border-radius: 6px; margin: 0 10px; width: calc(100% - 20px);
}

/* --- MAIN CONTENT (SPECIFIC) --- */
.main-content { flex: 1; }

.gray-panel {
  background-color: #f6f6f6; /* Основной серый фон */
  border-radius: 12px;
  padding: 30px;
  min-height: 600px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.03);
}

/* Фильтры */
.filters-row {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr; /* Три колонки */
  gap: 30px;
  margin-bottom: 40px;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.filter-group label {
  font-weight: 700;
  font-size: 14px;
}

.filter-group input {
  padding: 12px 15px;
  border: none;
  border-radius: 6px;
  font-size: 13px;
  background: #fff;
  box-shadow: 0 2px 4px rgba(0,0,0,0.02);
  outline: none;
}

/* СЕТКА ТАБЛИЦЫ */
/* Настройка колонок: ID фикс, Email средний, Имя широкое, Город средний, Заказ фикс */
.grid-header-row,
.grid-data-row {
  display: grid;
  grid-template-columns: 60px 1.5fr 2fr 1.2fr 120px;
  gap: 15px; /* Отступы между колонками */
  align-items: center;
}

/* Хедер таблицы */
.grid-header-row {
  margin-bottom: 20px;
}

.header-cell {
  background: #fff;
  padding: 12px 10px;
  border-radius: 6px;
  font-weight: 700;
  font-size: 13px;
  text-align: center;
  box-shadow: 0 2px 4px rgba(0,0,0,0.02);
  text-transform: uppercase;
}

/* Строки данных */
.grid-data-row {
  margin-bottom: 15px;
  padding: 0 5px; /* Небольшой отступ внутри */
}

.data-cell {
  font-size: 14px;
  text-align: center;
}

.data-cell.bold {
  font-weight: 700;
}

/* Специфичное выравнивание текста, если нужно */
.data-cell.col-email,
.data-cell.col-name {
  text-align: left;
  padding-left: 10px;
}

/* Кнопка заказа */
.order-btn {
  background: #fff;
  border: none;
  padding: 8px 15px;
  border-radius: 20px; /* Сильное скругление */
  font-weight: 700;
  font-size: 13px;
  cursor: pointer;
  box-shadow: 0 2px 5px rgba(0,0,0,0.05);
  width: 100%;
}

.order-btn:hover {
  background: #000;
  color: #fff;
}

/* Адаптив */
@media (max-width: 1100px) {
  .filters-row {
    grid-template-columns: 1fr;
    gap: 15px;
  }
  
  .grid-header-row, .grid-data-row {
    grid-template-columns: 50px 1fr 1fr 1fr 100px;
    font-size: 12px;
  }
}
</style>