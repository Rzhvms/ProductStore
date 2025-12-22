<template>
  <div class="admin-panel">
    <!-- Хедер -->
    <header class="header">
      <div class="logo-area">
        <div class="logo-icon">
          <!-- Лого -->
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
        
        <!-- Верхняя панель управления -->
        <div class="top-bar">
          <div class="filters-container">
            <!-- Поиск -->
            <div class="filter-group">
              <label>Поиск:</label>
              <input type="text" placeholder="По ключевому имени:" v-model="filters.search" />
            </div>

            <!-- Бренд -->
            <div class="filter-group">
              <label>Бренд:</label>
              <input type="text" placeholder="Введите название бренда:" v-model="filters.brand" />
            </div>

            <!-- Стоимость -->
            <div class="filter-group price-group">
              <label>Стоимость, ₽</label>
              <div class="price-inputs">
                <input type="number" placeholder="От" v-model="filters.priceFrom" />
                <span class="dash">—</span>
                <input type="number" placeholder="До" v-model="filters.priceTo" />
              </div>
            </div>
          </div>

          <button class="action-btn">Редактировать</button>
        </div>

        <!-- Рабочая область с категориями -->
        <div class="categories-workspace">
          
          <!-- Левая колонка: Список родительских категорий -->
          <div class="category-list-card">
            <ul>
              <li 
                v-for="(cat, index) in parentCategories" 
                :key="index"
                class="cat-item"
                :class="{ 'cat-active': cat.name === 'Электроника' }"
              >
                <span>{{ cat.name }}</span>
                <!-- Стрелочка для активного элемента -->
                <svg v-if="cat.name === 'Электроника'" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <polyline points="9 18 15 12 9 6"></polyline>
                </svg>
              </li>
            </ul>
          </div>

          <!-- Правая колонка: Сетка подкатегорий -->
          <div class="subcategories-grid">
            <div class="sub-card" v-for="card in subCategoryCards" :key="card.title">
              <h3 class="card-title">{{ card.title }}</h3>
              <ul class="sub-list">
                <li v-for="link in card.items" :key="link">{{ link }}</li>
              </ul>
            </div>
          </div>

        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue';

// Меню навигации
const menuItems = ref([
  { name: 'На страницу магазина', active: false },
  { name: 'Новости', active: false },
  { name: 'Товары', active: false },
  { name: 'Категории', active: true }, // Активный пункт
  { name: 'Бренды', active: false },
  { name: 'Пользователи', active: false },
  { name: 'Статистика', active: false },
]);

// Состояние фильтров
const filters = reactive({
  search: '',
  brand: '',
  priceFrom: null,
  priceTo: null
});

// Данные для левого списка (Родительские категории)
const parentCategories = ref([
  { name: 'Категория 1' },
  { name: 'Категория 2' },
  { name: 'Категория 3' },
  { name: 'Электроника' }, // Активная
  { name: 'Категория 5' },
  { name: 'Категория 6' },
  { name: 'Категория 7' },
]);

// Данные для карточек справа
const subCategoryCards = ref([
  {
    title: 'Телефоны',
    items: ['Бюджетные', 'Камера-фоны', 'Тяжелый люкс', 'Аксессуары']
  },
  {
    title: 'Компьютеры',
    items: ['Для офисной работы', 'Для игр', 'High-end сборки']
  },
  {
    title: 'Наушники и колонки',
    items: ['Полноразмерные', 'IEM-наушники', 'Саунд-бары', 'Вкладыши', 'TWS-наушники', 'Умные колонки']
  },
  {
    title: 'Ноутбуки',
    items: ['Под-категория 2', 'Под-категория 3', 'Под-категория 4', 'Под-категория 5', 'Под-категория 6', 'Под-категория 7']
  },
  {
    title: 'Телевизоры',
    items: ['Под-категория 2', 'Под-категория 3', 'Под-категория 4', 'Под-категория 5', 'Под-категория 6', 'Под-категория 7']
  }
]);
</script>

<style scoped>
/* Сброс и шрифты */
.admin-panel {
  font-family: 'Inter', 'Roboto', sans-serif; /* Более современный шрифт */
  color: #000;
  background-color: #fff;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* --- HEADER --- */
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 40px;
  border-bottom: 1px solid #eee;
}

.logo-area {
  display: flex;
  align-items: center;
  gap: 12px;
}

.brand-name {
  font-size: 20px;
  font-weight: 700;
  text-transform: uppercase;
  margin: 0;
  letter-spacing: 0.5px;
}

.user-btn {
  background: white;
  border: 1px solid #000;
  padding: 8px 25px;
  font-weight: 600;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.user-btn:hover {
  background: #000;
  color: #fff;
}

/* --- LAYOUT --- */
.content-wrapper {
  display: flex;
  flex: 1;
  padding: 30px 40px;
  gap: 40px;
  background-color: #fff;
}

/* --- SIDEBAR --- */
.sidebar {
  width: 240px;
  flex-shrink: 0;
  background-color: #f6f6f6; /* Светло-серый фон сайдбара */
  border-radius: 8px;
  padding: 20px 0;
  height: fit-content;
  min-height: 400px;
}

.sidebar ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.nav-item {
  width: 100%;
  text-align: left;
  background: transparent;
  border: none;
  padding: 12px 25px;
  font-weight: 700;
  font-size: 14px;
  cursor: pointer;
  color: #000;
}

/* Активный пункт меню (Категории) */
.nav-item.active {
  background: #fff;
  box-shadow: 0 2px 8px rgba(0,0,0,0.05);
  border-radius: 6px;
  margin: 0 10px;
  width: calc(100% - 20px); /* Отступы по бокам */
}

.nav-item:not(.active):hover {
  opacity: 0.7;
}

/* --- MAIN CONTENT --- */
.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 30px;
}

/* --- TOP BAR (FILTERS) --- */
.top-bar {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 20px;
}

.filters-container {
  background-color: #f6f6f6;
  padding: 20px;
  border-radius: 12px;
  display: flex;
  gap: 30px;
  flex: 1;
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.filter-group label {
  font-weight: 700;
  font-size: 14px;
}

.filter-group input {
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 13px;
  outline: none;
  background: #fff;
  min-width: 200px;
}

.price-inputs {
  display: flex;
  align-items: center;
  gap: 10px;
}

.price-inputs input {
  min-width: 60px;
  width: 80px;
}

.action-btn {
  background: #fff;
  border: 1px solid #000;
  padding: 12px 25px;
  border-radius: 8px;
  font-weight: 700;
  cursor: pointer;
  white-space: nowrap;
  height: fit-content;
  align-self: center; /* Центрируем по вертикали относительно блока фильтров */
}

.action-btn:hover {
  background: #000;
  color: #fff;
}

/* --- WORKSPACE (GRID) --- */
.categories-workspace {
  display: grid;
  grid-template-columns: 240px 1fr; /* Левая колонка фикс, правая резина */
  gap: 30px;
  align-items: start;
}

/* Левая карточка (список) */
.category-list-card {
  background: #f6f6f6;
  border-radius: 8px;
  padding: 15px 10px;
  /* box-shadow: 0 4px 12px rgba(0,0,0,0.08); */
}

.category-list-card ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.cat-item {
  padding: 12px 20px;
  font-weight: 700;
  font-size: 14px;
  cursor: pointer;
  margin-bottom: 5px;
  border-radius: 6px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

/* Активная категория (Электроника) */
.cat-item.cat-active {
  background: #fff;
  box-shadow: 0 2px 5px rgba(0,0,0,0.05);
}

/* Правая сетка подкатегорий */
.subcategories-grid {
  display: grid;
  /* Адаптивная сетка: колонки мин 200px */
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 20px;
}

.sub-card {
  background: #f6f6f6; /* Светлый фон как на макете */
  border-radius: 8px;
  padding: 20px;
  /* box-shadow: 0 2px 8px rgba(0,0,0,0.05); */
  height: fit-content;
}

.card-title {
  margin: 0 0 15px 0;
  font-size: 15px;
  font-weight: 800;
}

.sub-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.sub-list li {
  margin-bottom: 12px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  color: #000;
}

.sub-list li:hover {
  text-decoration: underline;
}

/* Адаптивность */
@media (max-width: 1024px) {
  .categories-workspace {
    grid-template-columns: 1fr; /* Одна колонка на узких экранах */
  }
  .filters-container {
    flex-direction: column;
    align-items: stretch;
  }
  .filters-container input {
    width: 100%;
  }
  .top-bar {
    flex-direction: column;
  }
}
</style>