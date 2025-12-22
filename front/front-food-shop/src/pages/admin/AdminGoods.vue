<template>
  <div class="admin-panel">
    <!-- Хедер -->
    <header class="header">
      <div class="logo-area">
        <div class="logo-icon">
          <!-- SVG Логотипа (имитация буквы F) -->
          <svg viewBox="0 0 24 24" width="30" height="30" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M4 6h16M4 12h16M4 18h8" stroke="black" />
            <circle cx="12" cy="12" r="10" stroke="black" />
          </svg>
        </div>
        <h1 class="brand-name">FANTECH - PANEL</h1>
      </div>
      <button class="admin-btn">Администратор</button>
    </header>

    <div class="content-wrapper">
      <!-- Боковое меню -->
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

      <!-- Основная область -->
      <main class="main-content">
        <!-- Верхняя панель управления (Фильтры + Кнопка) -->
        <div class="controls-area">
          <div class="filters-box">
            <!-- Поиск -->
            <div class="filter-group search-group">
              <label>Поиск</label>
              <input type="text" placeholder="По ключевому имени" v-model="filters.search" />
            </div>

            <!-- Цена -->
            <div class="filter-group price-group">
              <label>Стоимость, ₽</label>
              <div class="price-inputs">
                <input type="number" placeholder="От" v-model="filters.priceFrom" />
                <span class="dash">—</span>
                <input type="number" placeholder="До" v-model="filters.priceTo" />
              </div>
            </div>
          </div>

          <!-- Кнопка добавления -->
          <button class="add-product-btn">Добавить товар</button>
        </div>

        <!-- Сетка товаров -->
        <div class="products-grid">
          <div class="product-card" v-for="n in 6" :key="n">
            <div class="image-placeholder">
              <!-- Иконка редактирования (карандаш) -->
              <button class="edit-btn">
                <svg viewBox="0 0 24 24" width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                  <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                </svg>
              </button>
            </div>
            <div class="card-info">
              <p class="product-name">Ключевое имя</p>
              <p class="product-price">Цена₽</p>
            </div>
          </div>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue';

// Данные для меню
const menuItems = ref([
  { name: 'На страницу магазина', active: false },
  { name: 'Товары', active: true },
  { name: 'Категории', active: false },
  { name: 'Пользователи', active: false },
  { name: 'Заказы', active: false },
  { name: 'Статистика', active: false },
]);

// Реактивное состояние для фильтров
const filters = reactive({
  search: '',
  priceFrom: null,
  priceTo: null,
});
</script>

<style scoped>
/* Основные шрифты и сброс */
.admin-panel {
  font-family: 'Roboto', -apple-system, BlinkMacSystemFont, "Segoe UI", Helvetica, Arial, sans-serif;
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
  gap: 15px;
}

.brand-name {
  font-size: 24px;
  font-weight: 800;
  text-transform: uppercase;
  margin: 0;
}

.admin-btn {
  background: white;
  border: 2px solid #000;
  padding: 8px 20px;
  font-weight: 700;
  border-radius: 8px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.admin-btn:hover {
  background: #000;
  color: #fff;
}

/* --- LAYOUT --- */
.content-wrapper {
  display: flex;
  flex: 1;
  padding: 20px 40px;
  gap: 40px;
}

/* --- SIDEBAR --- */
.sidebar {
  width: 220px;
  flex-shrink: 0;
  padding-top: 10px;
}

.sidebar ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.sidebar li {
  margin-bottom: 10px;
}

.nav-item {
  width: 100%;
  text-align: left;
  background: #f9f9f9; /* Светло-серый фон как на скрине */
  border: none;
  padding: 12px 15px;
  font-weight: 700;
  font-size: 14px;
  cursor: pointer;
  border-radius: 4px; /* Легкое скругление */
  transition: background 0.2s;
}

/* Активная кнопка (как "Товары") */
.nav-item.active {
  background: #fff;
  border: 1px solid #000;
  box-shadow: 0 2px 5px rgba(0,0,0,0.05);
}

.nav-item:not(.active):hover {
  background: #eee;
}

/* --- MAIN CONTENT --- */
.main-content {
  flex: 1;
}

/* --- CONTROLS / FILTERS --- */
.controls-area {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: 30px;
  gap: 20px;
}

.filters-box {
  background-color: #f8f9fa; /* Серый фон блока фильтров */
  padding: 20px 30px;
  border-radius: 12px;
  display: flex;
  gap: 60px; /* Расстояние между "Поиск" и "Стоимость" */
  flex: 1;
  align-items: center;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.filter-group label {
  font-weight: 800;
  font-size: 15px;
}

.filter-group input {
  padding: 10px 15px;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  font-size: 14px;
  outline: none;
  background: #fff;
}

.search-group input {
  width: 250px;
}

.price-inputs {
  display: flex;
  align-items: center;
  gap: 10px;
}

.price-inputs input {
  width: 80px;
}

.add-product-btn {
  background: #fff;
  border: 1px solid #000;
  padding: 12px 20px;
  border-radius: 8px;
  font-weight: 700;
  cursor: pointer;
  white-space: nowrap;
  height: fit-content;
  align-self: flex-start; /* Выравнивание по верху */
}

.add-product-btn:hover {
  background: #000;
  color: #fff;
}

/* --- GRID --- */
.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 20px;
}

/* --- PRODUCT CARD --- */
.product-card {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.image-placeholder {
  width: 100%;
  aspect-ratio: 1 / 1; /* Квадрат */
  background: linear-gradient(135deg, #e0e0e0 0%, #f0f0f0 100%);
  border-radius: 25px; /* Сильное скругление как на макете */
  position: relative;
  margin-bottom: 10px;
  box-shadow: 0 4px 10px rgba(0,0,0,0.05);
}

.edit-btn {
  position: absolute;
  top: 15px;
  right: 15px;
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 0;
  opacity: 0.7;
}

.edit-btn:hover {
  opacity: 1;
}

.card-info {
  text-align: center;
}

.product-name {
  font-weight: 800;
  margin: 5px 0;
  font-size: 15px;
}

.product-price {
  font-weight: 800;
  margin: 0;
  font-size: 15px;
}

/* Адаптивность для мобильных (опционально) */
@media (max-width: 900px) {
  .content-wrapper {
    flex-direction: column;
  }
  .sidebar {
    width: 100%;
    display: flex;
    overflow-x: auto;
  }
  .sidebar ul {
    display: flex;
    gap: 10px;
  }
  .nav-item {
    white-space: nowrap;
  }
  .controls-area {
    flex-direction: column;
  }
  .filters-box {
    flex-direction: column;
    align-items: flex-start;
    gap: 20px;
    width: 100%;
    box-sizing: border-box;
  }
}
</style>