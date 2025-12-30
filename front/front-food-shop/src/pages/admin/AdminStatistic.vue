<template>
  <AdminLayout>
    <template #default>
      
      <!-- ============================================= -->
      <!-- VIEW 1: СПИСОК ТОВАРОВ (ГЛАВНАЯ) -->
      <!-- ============================================= -->
      <div v-if="!selectedProduct" class="stats-page">
        
        <div class="header-row">
          <h1 class="page-title">Статистика товаров</h1>
          <div class="total-badge">Всего товаров: {{ products.length }}</div>
        </div>

        <!-- ПАНЕЛЬ УПРАВЛЕНИЯ (Поиск, Фильтр, Сортировка) -->
        <div class="controls-panel">
          
          <!-- Левая часть: Поиск + Кнопка Фильтров -->
          <div class="left-controls">
            <div class="search-input-wrapper">
              <span class="search-icon">🔍</span>
              <input 
                type="text" 
                placeholder="Поиск товара..." 
                v-model="searchQuery" 
              />
            </div>
            
            <button 
              class="control-btn" 
              :class="{ active: showFilters }" 
              @click="showFilters = !showFilters"
            >
              <span class="filter-icon">⚙</span> Фильтры
            </button>
          </div>

          <!-- Правая часть: Сортировка (Выпадающий список) -->
          <div class="right-controls">
            <span class="sort-label">Сортировка:</span>
            <select v-model="currentSort" class="sort-select">
              <option value="name_asc">По названию (А → Я)</option>
              <option value="name_desc">По названию (Я → А)</option>
              <option value="sales_desc">По продажам (Много → Мало)</option>
              <option value="sales_asc">По продажам (Мало → Много)</option>
              <option value="reviews_desc">По отзывам (Много → Мало)</option>
              <option value="reviews_asc">По отзывам (Мало → Много)</option>
            </select>
          </div>
        </div>

        <!-- Выпадающая панель фильтров -->
        <transition name="slide-fade">
          <div v-if="showFilters" class="filters-drawer">
            <div class="filter-group">
              <label>Категория:</label>
              <select v-model="filterCategory">
                <option value="">Все категории</option>
                <option value="Электроника">Электроника</option>
                <option value="Одежда">Одежда</option>
                <option value="Книги">Книги</option>
              </select>
            </div>
            <div class="filter-group">
              <label>Минимальный рейтинг:</label>
              <input type="number" min="0" max="5" step="0.5" v-model.number="filterRatingMin" placeholder="0" />
            </div>
            <button class="clear-filters" @click="resetFilters">Сбросить всё</button>
          </div>
        </transition>

        <!-- СПИСОК КАРТОЧЕК -->
        <div class="products-list">
          <div 
            class="product-stat-card" 
            v-for="product in sortedAndFilteredProducts" 
            :key="product.id"
            @click="openProductStats(product)"
          >
            <!-- Иконка и Название -->
            <div class="card-left">
              <div class="card-icon">📦</div>
              <div class="card-info">
                <h3 class="p-name">{{ product.name }}</h3>
                <div class="p-module">
                  Категория: <span class="highlight">{{ product.category }}</span> • {{ product.subcategory }}
                </div>
              </div>
            </div>

            <!-- Статистика справа -->
            <div class="card-stats">
              <div class="stat-group">
                <div class="stars-row">
                  <span v-for="n in 5" :key="n" class="star" :class="{ filled: n <= Math.round(product.rating) }">★</span>
                </div>
                <div class="score-val">{{ product.rating }}</div>
              </div>
              
              <div class="stat-group reviews">
                <span class="icon">💬</span> {{ product.reviewsCount.toLocaleString() }} отзывов
              </div>

              <div class="sales-badge">
                Продаж: {{ product.sales.toLocaleString() }}
              </div>
            </div>
          </div>
          
          <!-- Если ничего не найдено -->
          <div v-if="sortedAndFilteredProducts.length === 0" class="empty-state">
            По вашему запросу товары не найдены.
          </div>
        </div>

      </div>

      <!-- ============================================= -->
      <!-- VIEW 2: ДЕТАЛЬНАЯ СТАТИСТИКА (Ваш старый код) -->
      <!-- ============================================= -->
      <div v-else class="detail-view">
        
        <!-- Хлебные крошки / Назад -->
        <div class="back-nav" @click="closeProductStats">
          <span class="back-arrow">←</span> Назад к списку
        </div>

        <div class="detail-header">
          <h2>{{ selectedProduct.name }}</h2>
          <div class="total-score">
            Общая оценка: <strong>{{ selectedProduct.rating }}</strong>
          </div>
        </div>

        <!-- 1. БЛОК ГРАФИКА -->
        <div class="chart-card">
          <div class="chart-header">
            <h3>За последние 28 дней ваши товары набрали <strong>1254 просмотра</strong></h3>
            <div class="date-select">Последние 28 дней ⌄</div>
          </div>

          <div class="chart-tabs">
            <div class="chart-tab active">
              <div class="tab-label">Просмотры</div>
              <div class="tab-val">1,3 тыс. ⌄</div>
              <div class="tab-sub">Ниже обычного (на 293)</div>
            </div>
            <div class="chart-tab">
              <div class="tab-label">Посетители</div>
              <div class="tab-val">593 чел. ^</div>
              <div class="tab-sub">Выше обычного</div>
            </div>
            <div class="chart-tab">
              <div class="tab-label">Заказы</div>
              <div class="tab-val">146 заказ</div>
              <div class="tab-sub">В среднем диапазоне</div>
            </div>
          </div>

          <div class="chart-area">
            <div class="grid-line" style="top: 0%"><span>75</span></div>
            <div class="grid-line" style="top: 33%"><span>50</span></div>
            <div class="grid-line" style="top: 66%"><span>25</span></div>
            <div class="grid-line" style="top: 100%"><span>0</span></div>
            <svg class="chart-svg" viewBox="0 0 1000 200" preserveAspectRatio="none">
              <path d="M0,150 L100,120 L200,160 L300,100 L400,110 L500,80 L600,140 L700,50 L800,100 L900,110 L1000,80" 
                    fill="none" stroke="#589BF2" stroke-width="3" vector-effect="non-scaling-stroke"/>
              <path d="M0,150 L100,120 L200,160 L300,100 L400,110 L500,80 L600,140 L700,50 L800,100 L900,110 L1000,80 V200 H0 Z" 
                    fill="rgba(88, 155, 242, 0.1)" stroke="none"/>
            </svg>
          </div>

          <div class="chart-dates">
            <span>15 окт.</span><span>20 окт.</span><span>25 окт.</span><span>30 окт.</span><span>5 нояб.</span><span>10 нояб.</span>
          </div>

          <button class="details-btn">Подробнее</button>
        </div>

        <!-- 2. БЛОК ОТЗЫВОВ -->
        <div class="reviews-section">
          <div class="reviews-header-row">
            <h3>💬 Отзывы <span class="count">11 111</span></h3>
            <button class="orange-btn">Написать отзыв (Admin)</button>
          </div>

          <div class="sort-row small">
            <span class="sort-link active">Дате ↑</span>
            <span class="sort-link">Рейтингу</span>
            <span class="sort-link">Полезности</span>
          </div>

          <div class="review-card" v-for="review in mockReviews" :key="review.id">
            <div class="review-top">
              <div class="user-info">
                <div class="avatar-circle">👤</div>
                <span class="user-name">{{ review.author }}</span>
              </div>
              <div class="review-score">
                Общая оценка: <strong>{{ review.score }}</strong>
              </div>
            </div>

            <div class="review-criteria">
              <div class="criteria-item">
                <span>❓ Качество товара</span>
                <div class="stars-mini">
                  <span v-for="n in 5" :key="n" class="star" :class="{ filled: n <= review.quality }">★</span>
                </div>
              </div>
              <div class="criteria-item">
                <span>🚚 Скорость доставки</span>
                <div class="stars-mini">
                  <span v-for="n in 5" :key="n" class="star" :class="{ filled: n <= review.delivery }">★</span>
                </div>
              </div>
            </div>

            <div class="review-text">{{ review.text }}</div>

            <div class="review-footer">
              <span class="date">Дата публикации: {{ review.date }}</span>
              <div class="likes">Был ли отзыв полезен? 👍 👎</div>
            </div>
          </div>
        </div>

      </div>
    </template>
  </AdminLayout>
</template>

<script setup>
import { ref, computed } from 'vue';
import AdminLayout from './AdminLayout.vue';
import './admin.css';

// === СОСТОЯНИЕ (STATE) ===
const searchQuery = ref('');
const selectedProduct = ref(null);
const showFilters = ref(false);

// Фильтры
const filterCategory = ref('');
const filterRatingMin = ref(0);

// Сортировка (по умолчанию "По продажам - много")
const currentSort = ref('sales_desc');

// === ДАННЫЕ (MOCK DATA) ===
const products = ref([
  { id: 1, name: 'iPhone 14 Pro Max 256GB', category: 'Электроника', subcategory: 'Телефоны', rating: 4.8, reviewsCount: 1250, sales: 5400 },
  { id: 2, name: 'Футболка Cotton Basic White', category: 'Одежда', subcategory: 'Футболки', rating: 3.6, reviewsCount: 11263, sales: 8900 },
  { id: 3, name: 'Наушники Sony WH-1000XM5', category: 'Электроника', subcategory: 'Аудио', rating: 4.9, reviewsCount: 850, sales: 1200 },
  { id: 4, name: 'Роман "Мастер и Маргарита"', category: 'Книги', subcategory: 'Классика', rating: 5.0, reviewsCount: 300, sales: 450 },
  { id: 5, name: 'Кроссовки Nike Air Force 1', category: 'Одежда', subcategory: 'Обувь', rating: 4.2, reviewsCount: 2100, sales: 3200 },
  { id: 6, name: 'Акула Блохэй', category: 'Игрушки', subcategory: 'Мягкие', rating: 4.9, reviewsCount: 15000, sales: 25000 },
  { id: 7, name: 'Зубная щетка', category: 'Красота', subcategory: 'Уход', rating: 4.1, reviewsCount: 50, sales: 120 },
]);

const mockReviews = ref([
  { id: 101, author: 'Алексей Смирнов', score: 4.0, quality: 4, delivery: 5, date: '13.05.2025', text: 'Товар пришел быстро, качество соответствует описанию.' },
  { id: 102, author: 'Аноним', score: 3.0, quality: 3, delivery: 2, date: '10.05.2025', text: 'Ожидал большего. Материал кажется дешевым.' },
  { id: 103, author: 'Мария Иванова', score: 5.0, quality: 5, delivery: 5, date: '01.05.2025', text: 'Все супер! Очень довольна покупкой.' },
]);

// === ЛОГИКА ФИЛЬТРАЦИИ И СОРТИРОВКИ ===
const resetFilters = () => {
  filterCategory.value = '';
  filterRatingMin.value = 0;
  searchQuery.value = '';
};

const sortedAndFilteredProducts = computed(() => {
  let result = [...products.value];

  // 1. Поиск
  if (searchQuery.value) {
    const q = searchQuery.value.toLowerCase();
    result = result.filter(p => p.name.toLowerCase().includes(q));
  }

  // 2. Фильтр по категории
  if (filterCategory.value) {
    result = result.filter(p => p.category === filterCategory.value);
  }

  // 3. Фильтр по рейтингу
  if (filterRatingMin.value > 0) {
    result = result.filter(p => p.rating >= filterRatingMin.value);
  }

  // 4. Сортировка
  result.sort((a, b) => {
    switch (currentSort.value) {
      case 'name_asc': return a.name.localeCompare(b.name);
      case 'name_desc': return b.name.localeCompare(a.name);
      case 'sales_desc': return b.sales - a.sales; // Больше -> Меньше
      case 'sales_asc': return a.sales - b.sales;  // Меньше -> Больше
      case 'reviews_desc': return b.reviewsCount - a.reviewsCount;
      case 'reviews_asc': return a.reviewsCount - b.reviewsCount;
      default: return 0;
    }
  });

  return result;
});

// Навигация
function openProductStats(prod) {
  selectedProduct.value = prod;
  window.scrollTo({ top: 0, behavior: 'smooth' });
}
function closeProductStats() {
  selectedProduct.value = null;
}
</script>

<style scoped>
/* Основной контейнер без лишних отступов */
.stats-page {
  width: 100%;
  box-sizing: border-box;
}

.header-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 25px;
}
.page-title { margin: 0; font-size: 26px; font-weight: 700; color: #333; }
.total-badge { background: #EAF4FF; color: #589BF2; padding: 5px 12px; border-radius: 20px; font-weight: 600; font-size: 14px; }

/* === ПАНЕЛЬ УПРАВЛЕНИЯ === */
.controls-panel {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  gap: 20px;
  flex-wrap: wrap;
}
.left-controls {
  display: flex;
  gap: 15px;
  flex: 1;
  min-width: 300px;
}
.right-controls {
  display: flex;
  align-items: center;
  gap: 10px;
}

/* Поиск */
.search-input-wrapper {
  flex-grow: 1;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  display: flex;
  align-items: center;
  padding: 0 15px;
  height: 42px;
  transition: border-color 0.2s;
}
.search-input-wrapper:focus-within { border-color: #589BF2; }
.search-input-wrapper input { border: none; background: transparent; width: 100%; outline: none; font-size: 14px; }
.search-icon { opacity: 0.5; margin-right: 10px; }

/* Кнопка Фильтров */
.control-btn {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 0 20px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  height: 44px;
  transition: all 0.2s;
}
.control-btn:hover { background: #f9f9f9; }
.control-btn.active { background: #FF7A00; border-color: #FF7A00; color: white; } /* Оранжевый активный */

/* Сортировка */
.sort-label { font-size: 14px; color: #666; font-weight: 500; }
.sort-select {
  padding: 10px 30px 10px 15px;
  border-radius: 8px;
  border: 1px solid #ddd;
  background: #fff;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  outline: none;
  /* Кастомная стрелочка */
  appearance: none;
  background-image: url("data:image/svg+xml;charset=US-ASCII,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20width%3D%22292.4%22%20height%3D%22292.4%22%3E%3Cpath%20fill%3D%22%23333%22%20d%3D%22M287%2069.4a17.6%2017.6%200%200%200-13-5.4H18.4c-5%200-9.3%201.8-12.9%205.4A17.6%2017.6%200%200%200%200%2082.2c0%205%201.8%209.3%205.4%2012.9l128%20127.9c3.6%203.6%207.8%205.4%2012.8%205.4s9.2-1.8%2012.8-5.4L287%2095c3.5-3.5%205.4-7.8%205.4-12.8%200-5-1.9-9.2-5.5-12.8z%22%2F%3E%3C%2Fsvg%3E");
  background-repeat: no-repeat;
  background-position: right 10px top 50%;
  background-size: 10px;
}

/* Выпадающая панель фильтров */
.filters-drawer {
  background: #f8f9fa;
  border: 1px solid #eee;
  padding: 20px;
  border-radius: 8px;
  margin-bottom: 20px;
  display: flex;
  gap: 20px;
  align-items: flex-end;
}
.filter-group { display: flex; flex-direction: column; gap: 5px; }
.filter-group label { font-size: 12px; font-weight: 700; color: #777; }
.filter-group select, .filter-group input { padding: 8px; border: 1px solid #ddd; border-radius: 6px; min-width: 150px; }
.clear-filters { background: none; border: none; color: #FF5252; cursor: pointer; text-decoration: underline; padding-bottom: 10px; }

/* Анимация фильтров */
.slide-fade-enter-active, .slide-fade-leave-active { transition: all 0.3s ease; }
.slide-fade-enter-from, .slide-fade-leave-to { transform: translateY(-10px); opacity: 0; }


/* === СПИСОК КАРТОЧЕК === */
.product-stat-card {
  background: #fff;
  border-radius: 12px;
  padding: 20px;
  margin-bottom: 12px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  cursor: pointer;
  border: 1px solid transparent;
  box-shadow: 0 2px 4px rgba(0,0,0,0.03);
  transition: all 0.2s;
}
.product-stat-card:hover {
  border-color: #589BF2;
  transform: translateY(-2px);
  box-shadow: 0 8px 16px rgba(88, 155, 242, 0.1);
}

.card-left { display: flex; align-items: center; gap: 15px; }
.card-icon {
  width: 48px; height: 48px;
  background: #EAF4FF;
  border-radius: 10px;
  display: flex; align-items: center; justify-content: center;
  font-size: 24px; color: #589BF2;
}
.card-info { flex-grow: 1; }
.p-name { margin: 0 0 5px 0; font-size: 17px; font-weight: 600; color: #333; }
.p-module { font-size: 13px; color: #888; }
.highlight { color: #333; font-weight: 500; }

.card-stats { display: flex; align-items: center; gap: 30px; }
.stat-group { display: flex; flex-direction: column; align-items: flex-end; }
.reviews { flex-direction: row; gap: 5px; color: #777; font-size: 14px; }
.stars-row { display: flex; gap: 2px; }
.star { color: #ddd; font-size: 16px; }
.star.filled { color: #589BF2; }
.score-val { font-size: 18px; font-weight: 700; color: #333; margin-top: 2px; }

.sales-badge {
  background: #f5f5f5;
  color: #555;
  padding: 6px 10px;
  border-radius: 6px;
  font-size: 13px;
  font-weight: 500;
}

.empty-state { text-align: center; padding: 40px; color: #999; font-style: italic; }


/* === ДЕТАЛЬНАЯ СТРАНИЦА (VIEW 2) === */
.detail-view { animation: fadeIn 0.3s ease; }
@keyframes fadeIn { from { opacity: 0; transform: translateY(5px); } to { opacity: 1; transform: translateY(0); } }

.back-nav { color: #589BF2; cursor: pointer; font-size: 15px; margin-bottom: 20px; display: inline-flex; align-items: center; gap: 5px; }
.back-nav:hover { text-decoration: underline; }

.detail-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
.detail-header h2 { margin: 0; font-size: 24px; }
.total-score { font-size: 18px; color: #589BF2; }

/* Блок Графика */
.chart-card { background: #fff; border-radius: 12px; padding: 25px; box-shadow: 0 4px 20px rgba(0,0,0,0.05); margin-bottom: 30px; border: 1px solid #eee; }
.chart-header { display: flex; justify-content: space-between; margin-bottom: 25px; }
.chart-header h3 { margin: 0; font-size: 18px; font-weight: 500; }
.date-select { font-weight: 600; cursor: pointer; color: #333; }

.chart-tabs { display: flex; border-bottom: 1px solid #eee; }
.chart-tab { flex: 1; padding: 15px; border-right: 1px solid #eee; cursor: pointer; }
.chart-tab:last-child { border-right: none; }
.chart-tab.active { border-top: 3px solid #589BF2; background: #fafcff; }
.tab-label { font-size: 14px; color: #888; margin-bottom: 5px; }
.tab-val { font-size: 20px; font-weight: 700; color: #333; }
.tab-sub { font-size: 12px; color: #aaa; margin-top: 5px; }

.chart-area { position: relative; height: 250px; margin: 20px 0; background: #F8FBFF; border-left: 1px solid #eee; border-bottom: 1px solid #eee; }
.grid-line { position: absolute; width: 100%; border-top: 1px dashed #e0e0e0; left: 0; }
.grid-line span { position: absolute; right: -30px; top: -8px; font-size: 11px; color: #999; }
.chart-svg { width: 100%; height: 100%; }
.chart-dates { display: flex; justify-content: space-between; padding: 0 20px; color: #999; font-size: 12px; }
.details-btn { margin-top: 20px; background: #fff; border: 1px solid #ddd; padding: 8px 16px; border-radius: 20px; font-weight: 700; cursor: pointer; }

/* Блок Отзывов */
.reviews-section { background: #fff; border-radius: 12px; }
.reviews-header-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px; }
.count { color: #999; font-weight: 400; font-size: 16px; }
.orange-btn { background: #FF7A00; color: white; border: none; padding: 8px 20px; border-radius: 6px; cursor: pointer; font-weight: 600; }

.sort-row.small { display: flex; gap: 15px; margin-bottom: 15px; font-size: 14px; }
.sort-link { color: #589BF2; cursor: pointer; border-bottom: 1px dashed transparent; }
.sort-link.active { color: #333; cursor: default; font-weight: 700; }

.review-card { background: #FAFAFA; padding: 20px; border-radius: 8px; margin-bottom: 15px; }
.review-top { display: flex; justify-content: space-between; margin-bottom: 15px; }
.user-info { display: flex; align-items: center; gap: 10px; }
.avatar-circle { width: 36px; height: 36px; background: #fff; border-radius: 50%; display: flex; align-items: center; justify-content: center; border: 1px solid #eee; }
.user-name { font-weight: 600; font-size: 15px; }
.review-score { color: #589BF2; font-size: 14px; }

.review-criteria { margin-bottom: 15px; }
.criteria-item { display: flex; align-items: center; gap: 15px; font-size: 13px; margin-bottom: 5px; }
.stars-mini .star { font-size: 14px; }

.review-text { font-size: 14px; line-height: 1.5; color: #444; margin-bottom: 15px; }
.review-footer { display: flex; justify-content: space-between; font-size: 12px; color: #999; }
.likes { cursor: pointer; }
</style>