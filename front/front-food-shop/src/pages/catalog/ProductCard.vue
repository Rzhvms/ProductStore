<template>
  <Header />
  
  <div v-if="loading" class="loading-state container">
    Загрузка...
  </div>

  <div v-else-if="error" class="error-state container">
    {{ error }}
  </div>

  <div v-else class="product-page" style="padding: 24px 48px;">
    <main class="main-content container">
      <div class="breadcrumbs">
          <span 
            class="crumb-link" 
            :class="{ active: currentView === 'categories' }"
            @click="goToCatalog"
          >
            Каталог
          </span>
          <span v-if="parentCategoryName" class="separator">›</span>
          <span 
            v-if="parentCategoryName"
            class="crumb-link"
            @click="goToCategory(parentCategoryName)"
          >
            {{ parentCategoryName }}
          </span>
          <span v-if="categoryName" class="separator">›</span>
          <span 
            v-if="categoryName" 
            class="crumb-link"
          >
            {{ categoryName }}
          </span>
          <span v-if="product" class="separator">›</span>
          <span 
            v-if="product" 
            class="crumb-link active"
          >
            {{ product.name }}
          </span>
        </div>
      <section class="product-hero">
        <div class="gallery-wrapper">
          <div class="thumbnails-col">
  <button
    class="thumb-nav up"
    @click="scrollUp"
    :disabled="thumbIndex === 0"
    aria-label="Scroll up"
  >
    <svg class="thumb-svg up" viewBox="0 0 64 64">
      <path
        fill-rule="evenodd"
        clip-rule="evenodd"
        d="M31.9997 0.557766C31.3342 5.11258 28.6898 7.19061 25.3513 9.05733C19.8436 12.1369 12 19.9825 12 32.554C12 45.1254 19.8436 52.971 25.3513 56.0506C28.6898 57.9173 31.3342 59.9953 31.9997 64.5501V64.554C32 64.552 32 64.552 32 64.552C32 64.552 32 64.552 32 64.552V64.5501C32.6658 59.9953 35.3102 57.9173 38.6487 56.0506C44.1564 52.971 52 45.1254 52 32.554C52 19.9825 44.1564 12.1369 38.6487 9.05733C35.3102 7.19061 32.6658 5.11258 32.0003 0.557766Z"
        fill="white"
      />
      <path d="M23.428 24.214C23.818 23.824 24.452 23.824 24.842 24.214L31.842 31.214C32.233 31.605 32.233 32.238 31.842 32.628L24.842 39.628C24.452 40.019 23.818 40.019 23.428 39.628C23.037 39.238 23.037 38.605 23.428 38.214L29.721 31.921L23.428 25.628C23.037 25.238 23.037 24.605 23.428 24.214Z" fill="#302E33"/>
    </svg>
  </button>

  <div class="thumbs-viewport">
    <div
      class="thumbs-list"
      :style="{ transform: `translateY(-${thumbIndex * ITEM_HEIGHT}px)` }"
    >
      <div
        v-for="(img, index) in images"
        :key="index"
        class="thumb-item"
        :class="{ active: activeImageIndex === index }"
        @click="activeImageIndex = index"
      >
        <img :src="img.url" alt="thumbnail" />
      </div>
    </div>
  </div>

  <button
    class="thumb-nav down"
    @click="scrollDown"
    :disabled="thumbIndex >= maxIndex"
    aria-label="Scroll down"
  >
    <svg class="thumb-svg down" viewBox="0 0 64 64">
      <path
        fill-rule="evenodd"
        clip-rule="evenodd"
        d="M31.9997 0.557766C31.3342 5.11258 28.6898 7.19061 25.3513 9.05733C19.8436 12.1369 12 19.9825 12 32.554C12 45.1254 19.8436 52.971 25.3513 56.0506C28.6898 57.9173 31.3342 59.9953 31.9997 64.5501V64.554C32 64.552 32 64.552 32 64.552C32 64.552 32 64.552 32 64.552V64.5501C32.6658 59.9953 35.3102 57.9173 38.6487 56.0506C44.1564 52.971 52 45.1254 52 32.554C52 19.9825 44.1564 12.1369 38.6487 9.05733C35.3102 7.19061 32.6658 5.11258 32.0003 0.557766Z"
        fill="white"
      />
      <path d="M23.428 24.214C23.818 23.824 24.452 23.824 24.842 24.214L31.842 31.214C32.233 31.605 32.233 32.238 31.842 32.628L24.842 39.628C24.452 40.019 23.818 40.019 23.428 39.628C23.037 39.238 23.037 38.605 23.428 38.214L29.721 31.921L23.428 25.628C23.037 25.238 23.037 24.605 23.428 24.214Z" fill="#302E33"/>
    </svg>
  </button>
</div>
          <div class="main-image" v-if="images.length > 0">
            <img :src="images[activeImageIndex].url" alt="main" />
          </div>
        </div>

        <div class="product-info">
          <h1 class="product-title">
            {{ product.name }}
            <span v-if="displayWeight" class="weight">, {{ displayWeight }}</span>
          </h1>

          <div class="row-rating">
                <div class="stars-wrapper">
                  <svg 
                    v-for="star in 5" 
                    :key="star" 
                    class="star-icon" 
                    width="32" 
                    height="32" 
                    viewBox="0 0 24 24"
                  >
                    <defs>
                      <linearGradient :id="'grad-' + (product.id || 'new') + '-' + star">
                        <stop offset="0%" stop-color="#FF7A00" />
                        <stop :offset="calculateOffset(product.rating, star)" stop-color="#FF7A00" />
                        <stop :offset="calculateOffset(product.rating, star)" stop-color="#E5E7EB" />
                        <stop offset="100%" stop-color="#E5E7EB" />
                      </linearGradient>
                    </defs>
                    <path 
                      :fill="'url(#grad-' + (product.id || 'new') + '-' + star + ')'" 
                      d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" 
                    />
                  </svg>  
                  <h3 class="rating-value" style="margin-left: 20px;">{{ product.rating || 0 }}</h3>
                </div>
                <h3 class="rating-value">({{ reviewsCount }} отзывов)</h3>
                <h3 class="article">Артикул: {{ article }}</h3>
              </div>

          <div class="tags-row">
            <div v-if="categoryName" class="tag-group">
              <span class="label">Категория:</span>
              <span class="tag">{{ parentCategoryName }} - {{ categoryName }}</span>
            </div>
            <div v-if="brand" class="tag-group">
              <span class="label">Бренд:</span>
              <span class="tag">{{ brand }}</span>
            </div>
          </div>

          <div class="price-action-row">
            <h2 class="price">
              {{ formatPrice(product.price) }} / {{ priceUnit }}
            </h2>
            
            <button v-if="!isInCart"
              class="btn-primary" 
              @click="handleAddToCart" 
              :disabled="addingToCart || product.quantity === 0"
            >
              {{ addingToCart ? 'Добавляем...' : 'В корзину' }}
            </button>
            <div v-else class="counter">
              <button @click="decrementStock">-</button>
              <div class="input-short-counter">{{ cartItemCount }}{{ stockUnit }}</div>
              <button @click="incrementStock">+</button>
            </div>
            <button 
              class="btn-favorite" 
              :class="{ active: isFavorite }" 
              @click="toggleFavorite"
            >
              {{ isFavorite ? '♥' : '♡' }}
            </button>
          </div>
        </div>
      </section>

      <section class="about-section">
        <h2>О товаре</h2>
        <div class="about-card">
          <div v-if="nutrition" class="nutrition-block">
            <h3>Пищевая ценность <span class="sub">на 100г продукта</span></h3>
            <div class="nutrition-grid">
              <div class="nutri-item">Белки: {{ nutrition.protein || 0 }}г</div>
              <div class="nutri-item">Жиры: {{ nutrition.fats || 0 }}г</div>
              <div class="nutri-item">Углеводы: {{ nutrition.carbs || 0 }}г</div>
              <div v-if="nutrition.energy" class="nutri-item">Энергетическая ценность: {{ nutrition.energy }} ккал/{{ nutrition.joules }} кДж</div>
            </div>
          </div>

          <div v-if="product.description" class="composition-block">
            <h3>Описание / Состав</h3>
            <p>{{ product.description }}</p>
          </div>

          <div v-if="customSpecs.length > 0" class="specs-block">
            <h3>Характеристики</h3>
            <ul class="specs-list">
              <li v-for="(spec, index) in customSpecs" :key="index" class="spec-item">
                <span class="spec-name">{{ spec.name }}</span>
                <span class="spec-dots"></span>
                <span class="spec-value">{{ spec.value }}</span>
              </li>
            </ul>
          </div>
        </div>
      </section>

      <section class="reviews-section" id="reviews">
        <div class="reviews-header">
          <h2>Отзывы <span class="count">0</span></h2>
          <button class="btn-primary-outline" @click="showReviewModal = true">Написать отзыв</button>
        </div>
        <div v-if="reviewsCount!==0" class="sort-controls">
            <span class="sort-label">Сортировка по:</span>
            
            <button 
              class="btn-sort" 
              :class="{ active: currentSort.field === 'date' }" 
              @click="toggleSort('date')"
            >
              Дате 
              <span v-if="currentSort.field === 'date'">
                {{ currentSort.direction === 'asc' ? '↑' : '↓' }}
              </span>
            </button>

            <button 
              class="btn-sort" 
              :class="{ active: currentSort.field === 'rating' }" 
              @click="toggleSort('rating')"
            >
              Рейтингу
              <span v-if="currentSort.field === 'rating'">
                {{ currentSort.direction === 'asc' ? '↑' : '↓' }}
              </span>
            </button>

            <button 
              class="btn-sort" 
              :class="{ active: currentSort.field === 'useful' }" 
              @click="toggleSort('useful')"
            >
              Полезности
              <span v-if="currentSort.field === 'useful'">
                {{ currentSort.direction === 'asc' ? '↑' : '↓' }}
              </span>
            </button>
          </div>
        <div v-if="reviewsCount!==0" class="reviews-list">
           <div v-for="review in sortedReviews" :key="review.id" class="review-card">
              <div class="review-content">
                <div class="review-user-header">
                <div class="user-info">
                  <div class="avatar-circle">
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#ccc" stroke-width="2">
                      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                      <circle cx="12" cy="7" r="4"></circle>
                    </svg>
                  </div>
                  <span class="user-name">{{ review.author }}</span>
                </div>

                <div class="stars-row">
                  <svg v-for="i in 5" :key="i" width="20" height="20" viewBox="0 0 24 24" class="star-mini">
                    <defs>
                      <linearGradient :id="'r-grad-' + review.id + '-' + i">
                        <stop offset="0%" stop-color="#FF7A00"/>
                        <stop :offset="calculateOffset(review.rating, i)" stop-color="#FF7A00"/>
                        <stop :offset="calculateOffset(review.rating, i)" stop-color="#D1D5DB"/>
                        <stop offset="100%" stop-color="#D1D5DB"/>
                      </linearGradient>
                    </defs>
                    <path 
                      :fill="'url(#r-grad-' + review.id + '-' + i + ')'" 
                      d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z"
                    />
                  </svg>
                  <span class="rating-num">{{ review.rating.toString().replace('.', ',') }}</span>
                </div>
              </div>

              <p class="review-text">{{ review.text }}</p>
              </div>
              <div class="review-footer">
              <span class="pub-date">Дата публикации: {{ review.date }}</span>
                <div class="useful-actions">
                  <span>Был ли отзыв полезен?</span>
                  <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#ccc" stroke-width="2">
                    <!--like (thumb up)-->
                    <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z"/>
                  </svg>
                  <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#ccc" stroke-width="2">
                    <!--dislike (thumb down)-->
                    <path d="M10 15v4a3 3 0 0 0 3 3l4-9V2H5.72a2 2 0 0 0-2 1.7l-1.38 9a2 2 0 0 0 2 2.3zm7-13h2.67A2.31 2.31 0 0 1 22 4v7a2.31 2.31 0 0 1-2.33 2H17"/>
                  </svg>
                </div>
              </div>
           </div>
        </div>
        <span v-else class="empty-text" style="text-align: center">Пока никто не оставил отзыв на этот товар. Поделитесь своим мнением первым!</span>
      </section>
    </main>

    <Teleport to="body">
      <div v-if="showReviewModal" class="modal-overlay" @click.self="closeModal">
        <div class="modal-content">
          <button class="modal-close" @click="closeModal">✕</button>
          <h2>Ваш отзыв</h2>
          
          <div class="form-group">
            <label>Оценка</label>
            <div class="stars-selector">
              <span 
                v-for="star in 5" 
                :key="star" 
                class="star-input" 
                :class="{ filled: star <= newReview.rating }"
                @click="newReview.rating = star"
              >★</span>
            </div>
            <span v-if="reviewErrors.rating" class="error-msg">{{ reviewErrors.rating }}</span>
          </div>

          <div class="form-group">
            <label>Текст отзыва</label>
            <textarea 
              v-model="newReview.text" 
              placeholder="Поделитесь впечатлениями о товаре"
              :class="{ 'error-border': reviewErrors.text }"
            ></textarea>
            <span v-if="reviewErrors.text" class="error-msg">{{ reviewErrors.text }}</span>
          </div>

          <button class="btn-primary full-width" @click="submitReview">Опубликовать</button>
        </div>
      </div>
    </Teleport>
  </div>
  <Footer />
</template>

<script setup>
import { ref, computed, onMounted, reactive } from 'vue'
import { useRoute } from 'vue-router'
import Header from './Header.vue'
import Footer from './Footer.vue'
import { productApi, cartApi, getCategory, reviewApi, favoriteApi, getProductImages } from '@/services/api.js'

const route = useRoute()
const loading = ref(true)
const error = ref(null)
const addingToCart = ref(false)
const isFavorite = ref(false)
const activeImageIndex = ref(0)
const product = ref(null)
const categoryName = ref(null)
const categoryId = ref(null)
const parentCategoryName = ref(null)
const parentCategoryId = ref(null)
const isInCart = ref(false)
const cartItemCount = ref(0)
const reviews = ref([]);
const reviewsCount = computed(() => reviews.value.length);
const images = ref([]);

const thumbIndex = ref(0);
const ITEM_HEIGHT = 90; // Высота картинки (80px) + gap (10px)
const VISIBLE_COUNT = 4; // Сколько картинок видно одновременно

const maxIndex = computed(() => {
  if (!product.value?.images) return 0;
  // Максимальный сдвиг = общее кол-во минус видимые
  return Math.max(0, product.value.images.length - VISIBLE_COUNT);
});

const scrollUp = () => {
  if (thumbIndex.value > 0) thumbIndex.value--;
};

const scrollDown = () => {
  if (thumbIndex.value < maxIndex.value) thumbIndex.value++;
};

const currentSort = ref({
  field: null,
  direction: null
});

const unitDict = {
  'кг': 'Килограмм',
  'г': 'Грамм',
  'л': 'Литр',
  'мл': 'Миллилитр'
};

const priceUnitsDict = {
  'st': 'шт',
  'kg': 'кг',
  'l': 'л'
};

const stockUnitsDict = {
  'kg': 'кг',
  'st': 'шт',
  'l': 'л'
};

// Логика отзывов
const showReviewModal = ref(false)
const newReview = reactive({
  rating: 0,
  text: ''
})
const reviewErrors = reactive({
  rating: '',
  text: ''
})

const loadCart = async () => {
  try {
    const cartData = await cartApi.get()
    return cartData.items || []
  } catch (e) {
    console.error("Ошибка загрузки корзины", e)
    return []
  }
}

function calculateOffset(rating, starIndex) {
  if (!rating) return '0%';
  const val = rating - (starIndex - 1);
  if (val >= 1) return '100%';
  if (val <= 0) return '0%';
  return `${val * 100}%`;
}

function toggleSort(field) {
  if (currentSort.value.field !== field) {
    currentSort.value = { field, direction: 'asc' };
    return;
  }

  if (currentSort.value.direction === 'asc') {
    currentSort.value.direction = 'desc';
  } else {
    currentSort.value = { field: null, direction: null };
  }
}

const sortedReviews = computed(() => {
  if (!currentSort.value.field) return reviews.value;

  return [...reviews.value].sort((a, b) => {
    const modifier = currentSort.value.direction === 'asc' ? 1 : -1;
    
    if (currentSort.value.field === 'date') {
      const getTimestamp = (dateStr) => {
        if (!dateStr) return 0;
        
        const [day, month, year] = dateStr.split('.');
        return new Date(2000 + Number(year), Number(month) - 1, Number(day)).getTime();
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

const closeModal = () => {
  showReviewModal.value = false
  newReview.rating = 0
  newReview.text = ''
  reviewErrors.rating = ''
  reviewErrors.text = ''
}

const submitReview = async () => {
  reviewErrors.rating = newReview.rating === 0 ? 'Пожалуйста, поставьте оценку' : ''
  reviewErrors.text = newReview.text.trim().length < 5 ? 'Напишите хотя бы пару слов (минимум 5 символов)' : ''

  if (!reviewErrors.rating && !reviewErrors.text) {
    await reviewApi.post(product.value.id, newReview.rating, newReview.text)
    closeModal()
  }
}

const characteristics = computed(() => product.value?.characteristics || {})

const displayWeight = computed(() => {
  const g = characteristics.value.grammage
  if (g && g.weight) return `${g.weight}${g.unit || 'г'}`
  return null
})

const priceUnit = computed(() => priceUnitsDict[characteristics.value.priceUnit || 'st'])
const stockUnit = computed(() => stockUnitsDict[characteristics.value.stockUnit || 'kg'])
const brand = computed(() => characteristics.value.brand)
const article = computed(() => characteristics.value.article)

const nutrition = computed(() => {
  if (!characteristics.value.nutrition) return null
  try {
    return typeof characteristics.value.nutrition === 'string' 
      ? JSON.parse(characteristics.value.nutrition) 
      : characteristics.value.nutrition
  } catch (e) { return null }
})

const customSpecs = computed(() => {
  const ignoredKeys = ['grammage', 'nutrition', 'brand', 'article', 'priceUnit', 'stockUnit', 'images']
  return Object.entries(characteristics.value)
    .filter(([key]) => !ignoredKeys.includes(key))
    .map(([key, value]) => ({ name: key, value }))
})

const formatPrice = (value) => {
  return new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB', minimumFractionDigits: 0 }).format(value)
}

// Синхронизация count из корзины
const syncCartCount = async () => {
  const cartItems = await loadCart()
  const id = route.params.id
  const item = cartItems.find(i => i.productId === id)
  cartItemCount.value = item ? item.quantity : 0
  isInCart.value = cartItemCount.value > 0
}

// Добавить 1
const incrementStock = async () => {
  if (!product.value) return
  cartItemCount.value++
  try {
    await cartApi.add(product.value.id, cartItemCount.value)
  } catch (e) {
    console.error(e)
    cartItemCount.value--
  }
  isInCart.value = cartItemCount.value > 0
}

// Убрать 1
const decrementStock = async () => {
  if (!product.value || cartItemCount.value <= 0) return
  cartItemCount.value--
  try {
    if (cartItemCount.value === 0) {
      await cartApi.remove(product.value.id)
    } else {
      await cartApi.add(product.value.id, cartItemCount.value)
    }
  } catch (e) {
    console.error(e)
    cartItemCount.value++
  }
  isInCart.value = cartItemCount.value > 0
}

const handleAddToCart = async () => {
  if (!product.value) return;
  addingToCart.value = true;
  try {
    await cartApi.add(product.value.id, 1);
  } catch (err) { alert(err.message || 'Ошибка'); }
  finally { addingToCart.value = false; }

  // После добавления обновляем count
  await syncCartCount()
}


// ---------- DATA LOADING ----------
const loadData = async () => {
  loading.value = true
  try {
    const id = route.params.id
    const data = await productApi.get(id)
    product.value = data
    if (data.categoryId) {
        categoryId.value = data.categoryId
        const catData = await getCategory(data.categoryId)
        categoryName.value = catData.categoryName || catData.name
        parentCategoryId.value = catData.parentCategoryId
        const parentCatData = await getCategory(catData.parentCategoryId)
        parentCategoryName.value = parentCatData.categoryName || parentCatData.name
    }

    // Теперь синхронизируем корзину
    await syncCartCount()

    const favoritesData = await favoriteApi.get()
    const favoriteProducts = favoritesData.productList
    isFavorite.value = favoriteProducts.some(item => item.productId === id)

    const reviewsData = await reviewApi.getList(id)
    reviews.value = reviewsData.productReviewList

    const imagesData = await getProductImages(id)
    images.value = imagesData.map(i => ({id: i.id, url: i.url}))
  } catch (err) {
    error.value = "Ошибка: " + err.message
  } finally {
    loading.value = false
  }
}

onMounted(loadData)
</script>

<style lang="scss" scoped>
.thumbnails-col {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
  width: 80px;

  .thumb-nav {
    background: none;
    border: none;
    cursor: pointer;
    width: 40px;
    height: 40px;
    padding: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: opacity 0.3s;

    &:disabled {
      opacity: 0.3;
      cursor: not-allowed;
    }

    .thumb-svg {
      width: 30px;
      height: 30px;
      filter: drop-shadow(0 2px 4px rgba(0,0,0,0.1));
      
      &.up { transform: rotate(-90deg); }   // Поворот стрелки вверх
      &.down { transform: rotate(90deg); }  // Поворот стрелки вниз
    }
  }

  .thumbs-viewport {
    overflow: hidden;
    position: relative;
  }

  .thumbs-list {
    display: flex;
    flex-direction: column;
    gap: 10px;
    transition: transform 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  }

  .thumb-item {
    width: 80px;
    height: 80px;
    border-radius: 15px;
    overflow: hidden;
    cursor: pointer;
    border: 2px solid transparent;
    transition: border-color 0.3s, transform 0.2s;
    flex-shrink: 0;

    img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    &.active {
      border-color: #FF7A00;
    }

    &:hover {
      transform: scale(1.02);
    }
  }
}

h2 {
  font-size: 24px;
  font-weight: 600;
}

h3 {
  font-size: 22px;
  font-weight: 600;
}

span {
  font-size: 18px;
  font-weight: 600;
}

.input-short-counter {
    width: 40px;
    text-align: center;
    border: none;
    outline: none;
    font-size: 14px;
    font-weight: 600;
    background: transparent;
}

.counter {
  background-color: #fff;
  display: flex;
  padding: 12px 18px;
  border-radius: 16px;
  align-items: center;
  height: 48px;
  width: 200px;
  justify-content: space-between;
  border: 1px solid #ddd;
}

.counter button {
  font-size: 24px;
  color: #FF7A00;
  background: none;
  border: none;
  outline: none;
  cursor: pointer;
}

.review-card {
    background-color: #F9F9F9;
    border-radius: 12px;
    padding: 24px;
    margin-bottom: 20px;
}

.review-header-title h4 {
    margin: 0 0 12px 0;
    color: #4B5563;
    font-size: 18px;
    font-weight: 600;
}

/* Стили текста */
.review-text {
    font-size: 14px;
    line-height: 1.5;
    color: #374151;
    margin-bottom: 16px;
}

/* Блок цитаты (оранжевая полоска) */
.review-quote {
    display: flex;
    gap: 16px;
    margin-bottom: 16px;
}

.quote-bar {
    width: 4px;
    background-color: #FF7A00;
    border-radius: 2px;
    flex-shrink: 0;
}

.review-quote p {
    font-size: 14px;
    color: #9CA3AF; /* Светло-серый текст цитаты */
    line-height: 1.5;
    margin: 0;
}

/* Хедер юзера */
.review-user-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 16px;
}

.user-info {
    display: flex;
    align-items: center;
    gap: 12px;
}

.avatar-circle {
    width: 32px;
    height: 32px;
    border-radius: 50%;
    background-color: #fff;
    border: 1px solid #E5E7EB;
    display: flex;
    align-items: center;
    justify-content: center;
}

.user-name {
    font-weight: 700;
    font-size: 18px;
    color: #374151;
}

.stars-row {
    display: flex;
    align-items: center;
    gap: 4px;
}

.rating-num {
    font-weight: 700;
    font-size: 18px;
    color: #FF7A00;
    margin-left: 8px;
}

/* Футер карточки */
.review-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-top: 8px;
}

.pub-date {
    font-size: 13px;
    color: #9CA3AF;
}

.review-actions {
    display: flex;
    gap: 24px;
}

.breadcrumbs { font-size: 14px; color: #9E9E9E; padding: 20px; padding-bottom: 0; }
.crumb-link { cursor: pointer; transition: color 0.2s; }
.crumb-link:hover { color: #FFA84C; }
.crumb-link.active { color: #FF7A00; font-weight: 500; cursor: default; }
.separator { margin: 0 8px; color: #CCC; }

.row-rating {
  display: flex;
  align-items: center;
  gap: 8px;
  justify-content: space-between;
}

.stars-wrapper {
  display: flex;
  align-items: center;
}

.rating-value {
  color: #FF7A00;
  font-weight: 600;
  font-size: 24px;
}
.tag {
	background: #555;
	color: #fff;
	padding: 4px 18px;
	border-radius: 24px;
}


$bg-color: #ffffff;
$bg-secondary: #f6f6f6;
$primary-orange: #ffa84c;
$text-dark: #222;
$text-grey: #888;
$border-radius: 24px;
$container-max-width: 1920px;

.loading-state, .error-state { padding: 100px 20px; text-align: center; font-size: 20px; color: $text-grey; }
.container { max-width: $container-max-width; margin: 0 auto; padding: 0 20px; width: 100%; box-sizing: border-box; display: flex; flex-direction: column; }
a { text-decoration: none; color: inherit; }
ul { list-style: none; padding: 0; margin: 0; }
button { cursor: pointer; border: none; background: none; font-family: inherit; transition: 0.2s;}

.btn-primary {
  background-color: $primary-orange; color: white; padding: 12px 24px; border-radius: 16px; font-weight: 600; font-size: 16px;
  &.full-width { width: 100%; margin-top: 10px; }
  &:hover { opacity: 0.9; }
  &:disabled { background-color: #ccc; cursor: not-allowed; }
}

.btn-primary-outline {
  border: 1px solid $primary-orange; color: #fff; padding: 12px 18px; border-radius: 16px; font-weight: 600; background-color: #ff7a00;
  &:hover { background: rgba($primary-orange, 0.05); }
}

.btn-favorite {
  width: 60px; height: 48px; border-radius: 16px; border: 1px solid #ddd; background: white; font-size: 30px; color: #ccc;
  display: flex; align-items: center; justify-content: center;
  &.active { color: #ffa84c; }
}

.reviews-link { color: $primary-orange; border-bottom: 1px $primary-orange; margin-left: 5px; font-weight: 600; }

.product-hero {
  display: grid; grid-template-columns: 1fr 1fr; gap: 40px;
  .gallery-wrapper {
    display: flex; gap: 20px;
    .thumbnails { display: flex; flex-direction: column; gap: 10px; .thumb { width: 140px; height: 140px; background-color: $bg-secondary; border-radius: $border-radius; cursor: pointer; &.active { border: 2px solid $primary-orange; } } }
    .main-image { flex-grow: 1; background-color: $bg-secondary; border-radius: $border-radius; min-height: 400px; }
  }
  .product-info {
    background-color: $bg-secondary; border-radius: $border-radius; display: flex; flex-direction: column; justify-content: center; gap: 40px; width: -webkit-fill-available; padding-left: 48px; padding-right: 48px;
    .product-title { font-size: 28px; font-weight: bold; margin-bottom: 20px; line-height: 1.2; .weight { color: $text-grey; font-size: 28px; } }
    .rating-row { display: flex; align-items: center; gap: 15px; margin-bottom: 25px; font-size: 14px; .stars { color: $primary-orange; } .rating-val { color: $primary-orange; font-weight: bold; } .article { margin-left: auto; color: $text-dark; } }
    .tags-row {
      display: flex; align-items: center; gap: 30px; margin-bottom: 40px; font-size: 14px; justify-content: space-between;
      .tag-group { display: flex; align-items: center;  .label { color: $text-dark; margin-right: 8px; } .tag { background: #666; color: white; padding: 4px 12px;} }
    }
    .price-action-row { display: flex; align-items: center; gap: 20px; .price { font-size: 24px; font-weight: bold; } }
  }
}

.about-section {
  margin-bottom: 60px; h2 { margin-bottom: 20px; font-size: 24px; }
  .about-card {
    background-color: $bg-secondary; padding: 24px; border-radius: $border-radius; display: flex; flex-direction: column; gap: 16px;
    h3 { font-size: 22px; margin-bottom: 15px; margin-top: 25px; &:first-child { margin-top: 0; } .sub { font-size: 18px; color: $text-grey; font-weight: 600; } }
    .nutrition-grid { display: flex; gap: 30px; font-size: 16px; color: $text-dark; margin-bottom: 25px; font-weight: 400; }
    .composition-block p { font-size: 16px; line-height: 1.5; color: $text-dark; margin-bottom: 25px; max-width: 800px; }
    .specs-list {
      max-width: 900px;
      .spec-item { display: flex; align-items: baseline; margin-bottom: 10px; font-size: 18px; .spec-name { color: $text-grey; } .spec-dots { flex: 1; border-bottom: 1px dotted #ccc; margin: 0 10px; } .spec-value { color: $text-dark; font-weight: 600; width: 40%; } }
    }
  }
}

.sort-controls {
    display: flex;
    align-items: center;
    gap: 15px;
    font-size: 14px;
}

.sort-label {
    font-weight: 600;
    color: #111827;
}

.btn-sort {
    background: none;
    border: none;
    cursor: pointer;
    font-size: 18px;
    color: #FF7A00; 
    padding: 0;
    font-weight: 500;
}

.btn-sort:hover { color: #ffa84c; text-decoration: underline; }

.btn-sort.active {
    font-weight: 700;
}

.useful-actions {
    display: flex;
    align-items: center;
    gap: 8px;
}

.reviews-section {
  display: flex; flex-direction: column; gap: 40px;
  .reviews-header { display: flex; justify-content: space-between; align-items: center; h2 { font-size: 24px; .count { color: $text-grey; font-weight: normal; margin-left: 10px; } } }
  .reviews-list { background-color: $bg-secondary; padding: 40px; border-radius: $border-radius; .empty-text { color: $text-grey; font-style: italic; } }
}

/* Modal Styles */
.modal-overlay {
  position: fixed; top: 0; left: 0; width: 100%; height: 100%;
  background: rgba(0,0,0,0.5); display: flex; align-items: center; justify-content: center; z-index: 1000;
}
.modal-content {
  background: white; padding: 30px; border-radius: 20px; width: 100%; max-width: 450px; position: relative;
  h2 { margin-bottom: 20px; font-size: 22px; }
}
.modal-close { position: absolute; top: 20px; right: 20px; font-size: 20px; color: #888; }
.form-group {
  margin-bottom: 20px;
  label { display: block; margin-bottom: 8px; font-weight: 500; font-size: 14px; }
  textarea {
    width: 100%; height: 120px; padding: 12px; border: 1px solid #ddd; border-radius: 12px;
    font-family: inherit; resize: none; box-sizing: border-box;
    &:focus { outline: none; border-color: $primary-orange; }
    &.error-border { border-color: #ff4d4d; }
  }
}
.stars-selector {
  font-size: 30px; display: flex; gap: 5px; cursor: pointer;
  .star-input { color: #eee; transition: 0.1s; &:hover { transform: scale(1.1); } &.filled { color: $primary-orange; } }
}
.error-msg { color: #ff4d4d; font-size: 12px; margin-top: 5px; display: block; }
</style>