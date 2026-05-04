<template>
  <AdminLayout>
    <template #default>
      
      <div v-if="!selectedProduct" class="stats-page">
        
        <div class="header-row">
          <div class="header-left-group">
            <img src="../../assets/chart.svg" class="folder-icon" style="filter: invert(50%) sepia(65%) saturate(2806%) hue-rotate(1deg) brightness(103%) contrast(105%)" />
            <h1 class="page-title">Статистика товаров</h1>
          </div>          
          <div class="total-badge">Всего товаров: {{ products.length }}</div>
        </div>

        <div class="controls-panel">
          <div class="left-controls">
            <div class="search-box-styled card">
              <img src="../../assets/search-normal.svg" alt="search" class="search-icon" />
              <input type="text" placeholder="Поиск товара..." v-model="searchQuery">
              <img src="../../assets/close-circle.svg" class="clear-circle" @click="searchQuery = ''" />
            </div>
          </div>
          <div class="right-controls">
            <button class="tool-btn" @click="toggleFilters" :class="{ 'active': isFiltered }">
              <img src="../../assets/filter.svg" alt="filter" />
              <span>Фильтры</span>
            </button>
            <div class="control-box dropdown-wrapper">
              <button class="sort-btn-styled card" :class="{ 'is-active': showSortDropdown || sortOption }" @click.stop="showSortDropdown = !showSortDropdown">
                <img v-if="!(showSortDropdown || sortOption)" src="../../assets/drop down button.svg" />
                <img v-else src="../../assets/drop down button(1).svg" />
                <span class="sort-btn-text">{{ buttonSortLabel }}</span>
              </button>
              <div v-if="showSortDropdown" class="custom-dropdown-menu sort-menu-design right-align">
                <div class="sort-group-label">По алфавиту</div>
                <div class="sort-item" @click="setSortOption('name-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'name-asc' }"></div><span class="sort-text">От А до Я</span></div>
                <div class="sort-item" @click="setSortOption('name-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'name-desc' }"></div><span class="sort-text">От Я до А</span></div>
                <div class="dd-divider"></div>
                <div class="sort-group-label">Цена</div>
                <div class="sort-item" @click="setSortOption('price-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'price-asc' }"></div><span class="sort-text">Сначала дешевле</span></div>
                <div class="sort-item" @click="setSortOption('price-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'price-desc' }"></div><span class="sort-text">Сначала дороже</span></div>
                <div class="dd-divider"></div>
                <div class="sort-group-label">Рейтинг</div>
                <div class="sort-item" @click="setSortOption('rating-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'rating-desc' }"></div><span class="sort-text">Высокий рейтинг</span></div>
                <div class="sort-item" @click="setSortOption('rating-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'rating-asc' }"></div><span class="sort-text">Низкий рейтинг</span></div>
              </div>
            </div>
          </div>
        </div>

        <transition name="slide-fade">
          <div v-if="showFilters" class="filters-drawer">
            <div class="filter-group">
              <label>Категория:</label>
              <select v-model="filterCategory">
                <option value="">Все категории</option>
                <option v-for="cat in categoriesList" :key="cat.categoryId" :value="cat.categoryName">
                  {{ cat.categoryName }}
                </option>
              </select>
            </div>
            <div class="filter-group">
              <label>Минимальный рейтинг:</label>
              <input type="number" min="0" max="5" step="0.5" v-model.number="filterRatingMin" placeholder="0" />
            </div>
            <button class="clear-filters" @click="resetFilters">Сбросить всё</button>
          </div>
        </transition>
        <div v-if="sortOption" class="active-filters">
            <span class="filter-tag">{{ sortLabel }} <button class="tag-remove" @click="sortOption = ''">×</button></span>
          </div>
        <div class="products-list">
          <div v-if="isLoading" style="text-align: center; padding: 20px; color: #666;">Загрузка данных...</div>
          
          <template v-else>
            <div class="product-row" v-for="(p, i) in filteredProducts" :key="p.id" @click="openProductStats(p)">
              <div class="row-image">
                <div class="img-placeholder" :style="{ backgroundImage: p.image?.url ? `url(${p.image.url})` : '' }"></div>
              </div>
              <div class="row-content">
                <div class="row-main">
                  <h2 class="p-name">{{ p.name }}</h2>
                  <div class="rat-pri">
                    <div class="row-rating">
                      <div class="stars-wrapper">
                        <svg v-for="star in 5" :key="star" class="star-icon" width="24" height="24" viewBox="0 0 24 24">
                          <defs>
                            <linearGradient :id="'grad-' + p.id + '-' + star">
                              <stop offset="0%" stop-color="#FF7A00" />
                              <stop :offset="calculateOffset(p.rating, star)" stop-color="#FF7A00" />
                              <stop :offset="calculateOffset(p.rating, star)" stop-color="#E5E7EB" />
                              <stop offset="100%" stop-color="#E5E7EB" />
                            </linearGradient>
                          </defs>
                          <path :fill="'url(#grad-' + p.id + '-' + star + ')'" d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" />
                        </svg>
                      </div>
                      <span class="rating-value">{{ p.rating }}</span>
                    </div>
                    <div class="row-replies">
                      <img src="../../assets/messages-3.svg"/>
                      <span class="replies-count">{{ p.reviewsCount }}</span>
                    </div>
                    <div class="row-price">{{ p.price }} ₽</div>
                  </div>      
                </div>
                <div class="row-meta">
                  <span class="p-cat">Категория: {{ p.categoryName || 'Без категории' }}</span>
                </div>
              </div>
            </div>
            <div v-if="filteredProducts.length === 0" style="text-align:center; padding: 20px; color: #999;">
                Товары не найдены
            </div>
          </template>
        </div>
      </div>

      <div v-else class="detail-view">
        
        <div class="breadcrumbs">
          <span class="crumb-link" @click="closeProductStats">Статистика</span> 
          <span class="crumb-arrow"> > </span>
          <span class="crumb-current">{{ selectedProduct.name }}</span>
        </div>

        <h1 class="detail-title">{{ selectedProduct.name }}</h1>

        <div class="date-controls-row">
          <div class="period-text">Статистика за период с <span class="d-val">05.12.2025</span> по <span class="d-val">05.01.2026</span></div>
          <div class="date-dropdown-btn">За последний месяц ⌄</div>
        </div>

        <div class="metrics-tabs">
          <div class="metric-tab active">
            <div class="m-label">Заказы</div>
            <div class="m-value-row">
              <span class="m-val">{{ selectedProduct.mockSales }}</span>
              <span class="m-arrow up">↑</span>
            </div>
            <div class="m-sub">Больше на 26%</div>
          </div>

          <div class="metric-tab">
            <div class="m-label">Добавление в избранное</div>
            <div class="m-value-row">
              <span class="m-val">{{ Math.round(selectedProduct.mockSales / 3) }}</span>
              <span class="m-arrow down">↓</span>
            </div>
            <div class="m-sub">Меньше на 26%</div>
          </div>

          <div class="metric-tab">
            <div class="m-label">Просмотры</div>
            <div class="m-value-row">
              <span class="m-val">{{ (selectedProduct.mockSales * 4.5).toFixed(0) }}</span>
              <span class="m-arrow up">↑</span>
            </div>
            <div class="m-sub">Больше на 26%</div>
          </div>
        </div>

        <div class="big-chart-area">
          <div style="width:100%; height:100%; display:flex; align-items:center; justify-content:center; color:#ccc;">
             График активности (нет API)
          </div>
        </div>

        <div class="rating-section card-box">
          <div class="rating-header">
            <h3>Общая оценка</h3>
            <div class="rating-total-stars">
              <div class="stars-fixed">
                <span class="star-f filled">★</span>
                <span class="star-f filled">★</span>
                <span class="star-f filled">★</span>
                <span class="star-f filled">★</span>
                <span class="star-f half">★</span>
              </div>
              <span class="big-rating-num">{{ selectedProduct.rating }}</span>
            </div>
          </div>
          <div class="rating-sub">Количество оценок <span class="count-gray">{{ selectedProduct.reviewsCount }}</span></div>

          <div class="rating-bars">
            <div class="bar-row" v-for="n in 5" :key="n">
              <div class="stars-label">
                 <span v-for="s in (6-n)" :key="s" class="star-mini filled">★</span>
                 <span v-for="e in (n-1)" :key="e" class="star-mini">★</span>
              </div>
              <div class="progress-track">
                <div class="progress-fill" :style="{ width: getStarPercent(6-n) + '%' }"></div>
              </div>
              <div class="bar-count">{{ getStarCount(6-n) }}</div>
            </div>
          </div>
        </div>

        <div class="reviews-wrapper">
          <div class="reviews-title-row">
            <h2>Отзывы <span class="reviews-total">{{ reviews.length }}</span></h2>
          </div>

          <div class="reviews-sort-row">
            <span class="rs-label">Сортировка по:</span>
            
            <button 
              class="rs-link" 
              :class="{ active: currentSort.field === 'date' }" 
              @click="toggleSortReviews('date')"
            >
              Дате 
              <span v-if="currentSort.field === 'date'">{{ currentSort.direction === 'asc' ? '↑' : '↓' }}</span>
            </button>

            <button 
              class="rs-link" 
              :class="{ active: currentSort.field === 'rating' }" 
              @click="toggleSortReviews('rating')"
            >
              Рейтингу
              <span v-if="currentSort.field === 'rating'">{{ currentSort.direction === 'asc' ? '↑' : '↓' }}</span>
            </button>

            <button 
              class="rs-link" 
              :class="{ active: currentSort.field === 'useful' }" 
              @click="toggleSortReviews('useful')"
            >
              Полезности
              <span v-if="currentSort.field === 'useful'">{{ currentSort.direction === 'asc' ? '↑' : '↓' }}</span>
            </button>
          </div>

          <div v-if="sortedReviews.length === 0" style="color: #999; padding: 20px 0;">
             Отзывов пока нет.
          </div>

          <div v-for="review in sortedReviews" :key="review.id" class="review-item card-box">
            
            <div v-if="review.type === 'admin_reply'" class="review-body">
               <h4 style="margin: 0 0 10px;">{{ review.title }}</h4>
               <p class="review-text-content">{{ review.text }}</p>
               <div class="admin-reply-block">
                 <div class="reply-line"></div>
                 <div class="reply-content">{{ review.quote }}</div>
               </div>
            </div>

            <div v-else>
                <div class="user-header">
                  <div class="u-avatar-placeholder">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#ccc" stroke-width="2">
                      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                      <circle cx="12" cy="7" r="4"></circle>
                    </svg>
                  </div>
                  <span class="u-name">{{ review.author }}</span>
                  <div class="u-stars">
                     <svg v-for="i in 5" :key="i" width="18" height="18" viewBox="0 0 24 24" class="star-mini">
                        <defs>
                          <linearGradient :id="'stat-r-grad-' + review.id + '-' + i">
                            <stop offset="0%" stop-color="#FF7A00"/>
                            <stop :offset="calculateOffset(review.rating, i)" stop-color="#FF7A00"/>
                            <stop :offset="calculateOffset(review.rating, i)" stop-color="#D1D5DB"/>
                            <stop offset="100%" stop-color="#D1D5DB"/>
                          </linearGradient>
                        </defs>
                        <path :fill="'url(#stat-r-grad-' + review.id + '-' + i + ')'" d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z"/>
                      </svg>
                     <span class="u-score">{{ review.rating }}</span>
                  </div>
                </div>
                 <div class="review-body">
                  <p class="review-text-content">{{ review.text }}</p>
                </div>
            </div>

            <div class="review-footer">
              <span class="pub-date">Дата публикации: {{ review.date }}</span>
              <div class="review-actions">
                <button class="act-btn" @click="toggleReviewVisibility(review)">
                    <span class="icon-eye">{{ review.isHidden ? '👁' : '👁‍🗨' }}</span> 
                    {{ review.isHidden ? 'Показать комментарий' : 'Скрыть комментарий' }}
                </button>
                <button class="act-btn"><span class="icon-reply">↩</span> Ответить</button>
                <button class="act-btn delete" @click="deleteReview(review.id)"><span class="icon-trash">🗑</span> Удалить</button>
              </div>
            </div>
          </div>

        </div>

      </div>
    </template>
  </AdminLayout>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import AdminLayout from './AdminLayout.vue';
import { adminProductApi, categoryApi, reviewApi, getProductImages } from '@/services/api';
import './admin.css';

// === СОСТОЯНИЕ (STATE) ===
const searchQuery = ref('');
const selectedProduct = ref(null);
const showFilters = ref(false);
const showSortDropdown = ref(false);
const sortOption = ref(''); 
const filterCategory = ref('');
const filterRatingMin = ref(0);
const isLoading = ref(false);

const products = ref([]);
const categoriesList = ref([]);
const categoriesMap = ref({});

const reviews = ref([]);
const currentSort = ref({
  field: 'date',
  direction: 'desc'
});

const getRating = async (id) => {
  const reviews = await reviewApi.getList(id)
  const reviewsData = reviews.productReviewList
  if (!reviewsData.length) return 0
  return reviewsData.reduce((acc, review) => acc + review.rating, 0) / reviewsData.length
}

const getImage = async (id) => {
    const images = await getProductImages(id)
    if (!images || images.length === 0) return null
    return images.find(i => i.isMain) ?? images[0] ?? null
}

// === ЗАГРУЗКА ДАННЫХ ===
const loadData = async () => {
  isLoading.value = true;
  try {
    const catsData = await categoryApi.get();
    categoriesList.value = catsData;
    catsData.forEach(c => {
      categoriesMap.value[c.categoryId] = c.categoryName;
    });

    const prodResponse = await adminProductApi.get(1, 100);
    const rawProducts = prodResponse.productList || [];
    products.value = await Promise.all(rawProducts.map(async (p) => {
      const revData = await reviewApi.getList(p.id);
      const reviews = revData.productReviewList;
      const mockRating = await getRating(p.id);
      const mockReviews = reviews.length;
      const mockSales = Math.floor(Math.random() * 5000) + 100;
      
      const stars = {};
      stars[5] = reviews.filter(r => r.rating === 5).length;
      stars[4] = reviews.filter(r => r.rating === 4).length;
      stars[3] = reviews.filter(r => r.rating === 3).length;
      stars[2] = reviews.filter(r => r.rating === 2).length;
      stars[1] = reviews.filter(r => r.rating === 1).length;

      return {
        id: p.id,
        name: p.name,
        price: p.price,
        categoryId: p.categoryId,
        categoryName: categoriesMap.value[p.categoryId] || 'Не указана',
        description: p.description,
        image: await getImage(p.id),
        rating: mockRating,
        reviewsCount: mockReviews,
        mockSales: mockSales,
        starCounts: stars,
        reviews: reviews
      };
    }));
    if (localStorage.getItem('productId')) {
      const productId = localStorage.getItem('productId');
      const product = products.value.find(p => p.id === productId);
      if (product) {
        selectedProduct.value = product;
        localStorage.removeItem('productId');
      }
    }
  } catch (error) {
    console.error("Ошибка загрузки статистики:", error);
  } finally {
    isLoading.value = false;
  }
};

onMounted(() => {
  loadData();
});

// === МЕТОДЫ ===

const getStarPercent = (star) => {
  if (!selectedProduct.value || !selectedProduct.value.starCounts) return 0;
  const count = selectedProduct.value.starCounts[star] || 0;
  const total = selectedProduct.value.reviewsCount || 1; 
  return (count / total) * 100;
};

const getStarCount = (star) => {
   if (!selectedProduct.value || !selectedProduct.value.starCounts) return 0;
   return selectedProduct.value.starCounts[star] || 0;
}

const resetFilters = () => {
  filterCategory.value = '';
  filterRatingMin.value = 0;
  searchQuery.value = '';
  sortOption.value = '';
};

async function openProductStats(prod) {
  selectedProduct.value = prod;
  window.scrollTo({ top: 0, behavior: 'smooth' });
  isLoading.value = true;
  try {
    const revData = await reviewApi.getList(prod.id);
    reviews.value = revData.productReviewList;
  } catch (e) {
    console.error('Load data error:', e);
    error.value = e.message || 'Ошибка загрузки данных';
    reviews.value = [];
  } finally {
    isLoading.value = false;
  }
}
function closeProductStats() {
  selectedProduct.value = null;
  localStorage.removeItem('productId');
}

function toggleFilters() {
  showFilters.value = !showFilters.value;
}

const toggleSortReviews = (field) => {
  if (currentSort.value.field !== field) {
    currentSort.value = { field, direction: 'asc' };
    return;
  }
  if (currentSort.value.direction === 'asc') {
    currentSort.value.direction = 'desc';
  } else {
    currentSort.value = { field: 'date', direction: 'desc' };
  }
};

const sortedReviews = computed(() => {
  if (!reviews.value.length) return [];
  if (!currentSort.value.field) return reviews.value;
  return [...reviews.value].sort((a, b) => {
    const modifier = currentSort.value.direction === 'asc' ? 1 : -1;
    if (currentSort.value.field === 'date') {
      // Парсим дату формата dd.mm.yy или dd.mm.yyyy
      const getTimestamp = (dateStr) => {
        if (!dateStr) return 0;
        const parts = dateStr.split('.');
        // Простейший парсинг для формата DD.MM.YYYY
        if (parts.length === 3) {
             const day = parseInt(parts[0], 10);
             const month = parseInt(parts[1], 10) - 1;
             const year = parseInt(parts[2].length === 2 ? '20'+parts[2] : parts[2], 10);
             return new Date(year, month, day).getTime();
        }
        return 0;
      };
      return (getTimestamp(a.date) - getTimestamp(b.date)) * modifier;
    }

    if (currentSort.value.field === 'rating') {
      return (a.rating - b.rating) * modifier;
    }

    if (currentSort.value.field === 'useful') {
      return ((a.useful || 0) - (b.useful || 0)) * modifier;
    }

    return 0;
  });
});

const toggleReviewVisibility = async (review) => {
  review.isHidden = !review.isHidden;
  await reviewApi.changeVisibility(review.id, review.isHidden);
};

const deleteReview = async (reviewId) => {
  if (!confirm('Удалить комментарий?')) return;
  reviews.value = reviews.value.filter(r => r.id !== reviewId);
  await reviewApi.delete(reviewId);
};

function setSortOption(opt) {
  sortOption.value = opt;
  showSortDropdown.value = false;
}

const buttonSortLabel = computed(() => {
  if (!sortOption.value) return 'Сортировка';
  if (sortOption.value.includes('name')) return 'По названию';
  if (sortOption.value.includes('price')) return 'По цене';
  if (sortOption.value.includes('rating')) return 'По рейтингу';
  return 'Сортировка';
});

const sortLabel = computed(() => {
  const labels = {
    'name-asc': 'От А до Я', 'name-desc': 'От Я до А',
    'price-asc': 'Сначала дешевле', 'price-desc': 'Сначала дороже',
    'rating-desc': 'Высокий рейтинг', 'rating-asc': 'Низкий рейтинг'
  };
  return labels[sortOption.value] || 'Сортировка';
});

const filteredProducts = computed(() => {
  let result = [...products.value];
  if (searchQuery.value) {
    const q = searchQuery.value.toLowerCase();
    result = result.filter(p => p.name.toLowerCase().includes(q));
  }
  if (filterCategory.value) {
    result = result.filter(p => p.categoryName === filterCategory.value);
  }
  if (filterRatingMin.value > 0) {
    result = result.filter(p => p.rating >= filterRatingMin.value);
  }
  if (sortOption.value) {
    result.sort((a, b) => {
      switch (sortOption.value) {
        case 'name-asc': return a.name.localeCompare(b.name);
        case 'name-desc': return b.name.localeCompare(a.name);
        case 'price-asc': return a.price - b.price;
        case 'price-desc': return b.price - a.price;
        case 'rating-asc': return (a.rating || 0) - (b.rating || 0);
        case 'rating-desc': return (b.rating || 0) - (a.rating || 0);
        default: return 0;
      }
    });
  }
  return result;
});

const calculateOffset = (rating, starIndex) => {
  if (!rating) return '0%';
  if (rating >= starIndex) return '100%';
  if (rating <= starIndex - 1) return '0%';
  return ((rating % 1) * 100) + '%';
};
</script>

<style scoped>
/* Глобальный сброс для этого компонента */
* { box-sizing: border-box; }

/* === LIST VIEW STYLES (Existing) === */
.stats-page { width: 100%; }
.header-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 25px; }
.page-title { margin: 0; font-size: 26px; font-weight: 700; color: #333; }
.total-badge { background: #ffaa; color: #ff7a00; padding: 5px 12px; border-radius: 20px; font-weight: 600; font-size: 14px; }

.controls-panel { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; gap: 20px; flex-wrap: wrap; }
.left-controls { display: flex; gap: 15px; flex: 1; min-width: 300px; }
.right-controls { display: flex; align-items: center; gap: 10px; }

.search-box-styled { display: flex; align-items: center; background-color: #f9f9f9; border-radius: 16px; padding: 12px 24px; width: 100%; }
.search-box-styled input { border: none; background: transparent; outline: none; font-size: 14px; color: #374151; margin-left: 8px; width: 100%; }
.tool-btn { padding: 12px 24px; border-radius: 8px; border: none; background: #F9F9F9; color: #555; display: flex; align-items: center; gap: 8px; cursor: pointer; font-size: 14px; transition: 0.2s; }
.tool-btn.active { background: #FF7A00; color: #fff; }
.tool-btn.active img { filter: brightness(0) invert(1); }

/* Sort & Filter Styles */
.filters-drawer { background: #f8f9fa; border: 1px solid #eee; padding: 20px; border-radius: 8px; margin-bottom: 20px; display: flex; gap: 20px; align-items: flex-end; }
.filter-group { display: flex; flex-direction: column; gap: 5px; }
.clear-filters { background: none; border: none; color: #FF5252; cursor: pointer; text-decoration: underline; padding-bottom: 10px; }
.active-filters { display: flex; gap: 8px; margin-bottom: 15px; }
.filter-tag { display: inline-flex; align-items: center; gap: 6px; background: #FFF7ED; color: #FF7A00; padding: 6px 12px; border-radius: 20px; font-size: 13px; font-weight: 500; }
.tag-remove { background: none; border: none; color: #FF7A00; font-size: 16px; cursor: pointer; }

/* Product Row Styles */
.products-list { display: flex; flex-direction: column; gap: 16px; }
.product-row { display: flex; align-items: center; background: #F9F9F9; padding: 16px 24px; border-radius: 20px; gap: 20px; cursor: pointer; transition: background 0.2s; }
.product-row:hover { background: #F2F2F2; }
.img-placeholder { width: 80px; height: 80px; background-size: cover; background-position: center; background-color: #fff; border-radius: 12px; }
.row-content { flex: 1; display: flex; flex-direction: column; gap: 8px; }
.row-main { display: flex; align-items: center; gap: 20px; justify-content: space-between; }
.p-name { margin: 0; font-size: 24px; font-weight: 600; color: #333; }
.rat-pri { display: flex; align-items: center; justify-content: space-between; gap: 100px; }
.row-rating { display: flex; align-items: center; gap: 8px; }
.stars-wrapper { display: flex; }
.rating-value { color: #FF7A00; font-weight: 600; font-size: 24px; }
.row-replies { display: flex; align-items: center; gap: 8px; font-size: 18px; font-weight: 600; color: #7a7a7a; }
.row-price { font-size: 18px; font-weight: 700; color: #333; }
.row-meta { display: flex; gap: 4px; font-size: 18px; color: #7a7a7a; flex-direction: column; }


/* ========================================================= */
/* === DETAIL VIEW STYLES (NEW - КАК НА СКРИНШОТЕ) === */
/* ========================================================= */

.detail-view {
  font-family: 'Inter', sans-serif; /* Подставьте свой шрифт */
  color: #333;
}

/* 1. Навигация и Шапка */
.breadcrumbs {
  font-size: 14px;
  color: #FF7A00;
  margin-bottom: 12px;
  cursor: pointer;
}
.crumb-arrow { color: #aaa; margin: 0 5px; }
.crumb-current { color: #ffb160; cursor: default; }

.detail-title {
  font-size: 32px;
  font-weight: 700;
  margin: 0 0 20px 0;
}

.date-controls-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}
.period-text {
  font-size: 14px;
  color: #666;
}
.d-val { color: #333; font-weight: 500; }
.date-dropdown-btn {
  background: #F3F4F6;
  padding: 8px 16px;
  border-radius: 8px;
  font-size: 14px;
  color: #555;
  cursor: pointer;
}

/* 2. Табы метрик */
.metrics-tabs {
  display: flex;
  border-bottom: 2px solid #FF7A00;
  margin-bottom: 20px;
}
.metric-tab {
  flex: 1;
  padding: 16px;
  border-top: 2px solid transparent;
  cursor: pointer;
}
/* Активный таб имитируем верхнюю оранжевую линию, если нужно, но на макете подчеркивание общее */
/* На макете заголовки просто текстом. Сделаем структуру: */
.metric-tab {
  border-bottom: none; 
  position: relative;
}

.m-label { font-size: 14px; color: #666; margin-bottom: 8px; }
.m-value-row { display: flex; align-items: center; gap: 8px; margin-bottom: 8px; }
.m-val { font-size: 32px; font-weight: 700; color: #FF7A00; }
.m-arrow { font-size: 20px; }
.m-arrow.up { color: #FF7A00; }
.m-arrow.down { color: #dcbfa6; transform: rotate(180deg); display: inline-block; } /* Примерный цвет */

.m-sub { font-size: 14px; color: #999; }

/* 3. График */
.big-chart-area {
  background: #FAFAFA; /* Серый фон как на макете */
  border-radius: 12px;
  height: 300px;
  margin-bottom: 30px;
}

/* 4. Рейтинг */
.card-box {
  background: #FAFAFA;
  padding: 24px;
  border-radius: 12px;
  margin-bottom: 24px;
}

.rating-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 5px; }
.rating-header h3 { margin: 0; font-size: 18px; font-weight: 600; }
.rating-total-stars { display: flex; align-items: center; gap: 10px; }
.stars-fixed { color: #E5E7EB; font-size: 20px; }
.stars-fixed .filled { color: #FF7A00; }
.big-rating-num { font-size: 20px; font-weight: 700; color: #FF7A00; }

.rating-sub { font-size: 14px; color: #999; margin-bottom: 15px; }
.count-gray { float: right; }

.rating-bars { display: flex; flex-direction: column; gap: 8px; }
.bar-row { display: flex; align-items: center; gap: 15px; }
.stars-label { width: 100px; display: flex; justify-content: flex-end; gap: 2px; }
.star-mini { color: #E5E7EB; font-size: 14px; }
.star-mini.filled { color: #FF7A00; }

.progress-track { flex: 1; height: 6px; background: #E5E7EB; border-radius: 3px; overflow: hidden; }
.progress-fill { height: 100%; background: #FF7A00; border-radius: 3px; }
.bar-count { width: 30px; text-align: right; color: #999; font-size: 14px; }


/* 5. Отзывы */
.reviews-wrapper { margin-top: 40px; }
.reviews-title-row { margin-bottom: 20px; }
.reviews-title-row h2 { font-size: 20px; font-weight: 700; display: flex; gap: 15px; }
.reviews-total { color: #555; font-weight: 400; }

.reviews-sort-row { display: flex; gap: 20px; margin-bottom: 20px; font-size: 14px; color: #FF7A00; }
.rs-label { color: #333; font-weight: 600; }
.rs-link { cursor: pointer; }
.rs-link.active { text-decoration: none; } /* Можно добавить стрелку */

.review-item { margin-bottom: 20px; }
.review-header { margin-bottom: 10px; }
.review-title { font-weight: 700; font-size: 16px; margin-bottom: 5px; }

/* Стили для ответа админа (цитата) */
.admin-reply-block { display: flex; margin-top: 15px; gap: 12px; }
.reply-line { width: 4px; background: #FF7A00; border-radius: 2px; flex-shrink: 0; }
.reply-content { font-size: 14px; color: #888; line-height: 1.5; }

.review-text-content { font-size: 14px; color: #555; line-height: 1.6; }

/* Футер отзыва */
.review-footer { margin-top: 15px; display: flex; justify-content: space-between; align-items: center; border-top: 1px solid #eee; padding-top: 15px; }
.pub-date { font-size: 13px; color: #aaa; }
.review-actions { display: flex; gap: 20px; }
.act-btn { background: none; border: none; font-size: 13px; color: #777; cursor: pointer; display: flex; align-items: center; gap: 6px; }
.act-btn:hover { color: #FF7A00; }
.act-btn.delete { color: #FF5252; }

/* Пользовательский хедер */
.user-header { display: flex; align-items: center; gap: 10px; margin-bottom: 15px; }
.u-avatar-placeholder { width: 32px; height: 32px; background: #eee; border-radius: 50%; display: flex; align-items: center; justify-content: center; color: #999; }
.u-name { font-weight: 700; font-size: 15px; }
.u-stars { display: flex; align-items: center; gap: 5px; margin-left: auto; }
.u-score { font-weight: 700; color: #FF7A00; margin-left: 5px; }
.header-left-group{ display: flex; align-items: center; gap: 12px; }
</style>