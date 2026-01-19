<template>
  <div class="page-container">
    <Header />

    <main class="home">
      <!-- ПОИСК -->
      <section class="search-section">
        <div class="search-bar">
          <span class="search-icon">🔍</span>
          <input type="text" placeholder="Поиск товаров..." />
          <button class="search-btn">Найти</button>
        </div>
      </section>

      <!-- ПРОМО БАННЕРЫ -->
      <section class="promo" v-if="promotions.length > 0">
        <div class="promo-container">
          <button class="promo-nav prev" @click="prevPromo" v-if="promotions.length > 1">‹</button>
          
          <div class="promo-wrapper">
            <div 
              class="promo-track"
              :style="{ transform: `translateX(-${currentPromoIndex * 100}%)` }"
            >
              <div 
                class="promo-banner" 
                v-for="(promo, index) in promotions" 
                :key="promo.id"
                :style="{ backgroundColor: promo.color || '#e5e5e5' }"
              >
                <div class="promo-content">
                  <h3>{{ promo.title }}</h3>
                  <p>{{ promo.description }}</p>
                  <div class="promo-tag" v-if="promo.benefitType === 'discount'">
                    Скидка {{ promo.value }}{{ promo.valueType === 'percent' ? '%' : '₽' }}
                  </div>
                  <div class="promo-tag" v-else>
                    Бонус {{ promo.value }}₽
                  </div>
                </div>
                <div class="promo-img-placeholder">
                  🎁
                </div>
              </div>
            </div>
          </div>

          <button class="promo-nav next" @click="nextPromo" v-if="promotions.length > 1">›</button>

          <div class="promo-dots" v-if="promotions.length > 1">
            <span 
              v-for="(promo, index) in promotions" 
              :key="'dot-'+promo.id"
              class="dot"
              :class="{ active: currentPromoIndex === index }"
              @click="setPromo(index)"
            ></span>
          </div>
        </div>
      </section>

      <!-- ПОПУЛЯРНЫЕ ТОВАРЫ (СЛАЙДЕР) -->
      <section class="popular">
        <h2>Популярные товары</h2>

        <div class="slider-container">
          <button 
            class="nav-btn left" 
            @click="scrollLeft" 
            :disabled="!canScrollLeft"
            :class="{ disabled: !canScrollLeft }"
          >‹</button>

          <div class="slider-window" ref="wrapperRef">
            <div
              class="slider-track"
              ref="sliderRef"
              :style="{ transform: `translateX(-${offset}px)` }"
            >
              <div 
                class="product-card" 
                v-for="product in products" 
                :key="product.id"
                @click="goToProduct(product.id)" 
              >
                <div class="product-image">
                  <img
                    v-if="product.image"
                    :src="product.image"
                    :alt="product.name"
                  />
                  <span v-else>🖼️</span>
                </div>

                <div class="product-info">
                  <div class="product-name">{{ product.name }}</div>
                  <div class="product-bottom">
                    <div class="product-price">₽{{ product.price }}</div>

                    <!-- Логика добавления в корзину -->
                    <div class="cart-actions" @click.stop>
                      <button
                        v-if="product.count === 0"
                        class="add-cart-btn"
                        @click="increment(product)"
                      >
                        Добавить
                      </button>

                      <div v-else class="counter-pill">
                        <button class="counter-btn minus" @click="decrement(product)"></button>
                        <span class="counter-value">{{ product.count }}</span>
                        <button class="counter-btn plus" @click="increment(product)"></button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <button 
            class="nav-btn right" 
            @click="scrollRight" 
            :disabled="!canScrollRight"
            :class="{ disabled: !canScrollRight }"
          >›</button>
        </div>
      </section>
    </main>

    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { useRouter } from 'vue-router'
import Header from './Header.vue'
import Footer from './Footer.vue'
import { productApi, cartApi } from '@/services/api'
import { getActivePromotions } from '@/services/promotionsService' // Импортируем наш сервис

const router = useRouter()
const products = ref([])
const promotions = ref([])
const currentPromoIndex = ref(0)
let promoInterval = null

// --- Параметры слайдера товаров ---
const wrapperRef = ref(null)
const sliderRef = ref(null)
const offset = ref(0)
const maxOffset = ref(0)

// Настройки ширины: карточка 220px + отступ 20px
const CARD_WIDTH = 220 
const GAP = 20
const STEP = CARD_WIDTH + GAP

// --- ЗАГРУЗКА ДАННЫХ ---
const loadData = async () => {
  // 1. Загружаем акции
  promotions.value = getActivePromotions()
  startPromoRotation()

  // 2. Загружаем товары
  try {
    const data = await productApi.getList(1, 10) // page 1, size 10
    products.value = data.productList?.map(p => ({
      ...p,
      count: 0 // В реальном проекте здесь нужно сверяться с текущей корзиной
    })) || []
    
    await nextTick()
    calculateSliderMetrics()
  } catch (error) {
    console.error('Ошибка загрузки товаров:', error)
    products.value = []
  }
}

// --- ЛОГИКА БАННЕРОВ ---
const startPromoRotation = () => {
  if (promotions.value.length > 1) {
    promoInterval = setInterval(() => {
      nextPromo()
    }, 5000)
  }
}

const nextPromo = () => {
  currentPromoIndex.value = (currentPromoIndex.value + 1) % promotions.value.length
}

const prevPromo = () => {
  currentPromoIndex.value = currentPromoIndex.value === 0 
    ? promotions.value.length - 1 
    : currentPromoIndex.value - 1
}

const setPromo = (index) => {
  currentPromoIndex.value = index
  // Сбрасываем таймер при ручном переключении
  clearInterval(promoInterval)
  startPromoRotation()
}

// --- ЛОГИКА СЛАЙДЕРА ТОВАРОВ ---
const calculateSliderMetrics = () => {
  if (!wrapperRef.value || !sliderRef.value) return
  
  const trackWidth = sliderRef.value.scrollWidth
  const containerWidth = wrapperRef.value.clientWidth
  
  // Максимальный сдвиг = полная длина ленты - видимая область
  maxOffset.value = Math.max(0, trackWidth - containerWidth)
  
  // Если после ресайза мы улетели слишком далеко, возвращаем назад
  if (offset.value > maxOffset.value) {
    offset.value = maxOffset.value
  }
}

const canScrollLeft = computed(() => offset.value > 0)
const canScrollRight = computed(() => offset.value < maxOffset.value) // Исправлено для точности

const scrollLeft = () => {
  const newOffset = offset.value - STEP
  offset.value = Math.max(0, newOffset)
}

const scrollRight = () => {
  const newOffset = offset.value + STEP
  // Округляем до maxOffset, если шаг превышает остаток
  offset.value = Math.min(newOffset, maxOffset.value)
}

// --- КОРЗИНА И НАВИГАЦИЯ ---
const goToProduct = (id) => router.push(`/catalog/product/${id}`)

const increment = async (product) => {
  product.count++
  try {
    await cartApi.add(product.id, product.count)
  } catch (e) {
    console.error(e)
    product.count-- // Откат при ошибке
  }
}

const decrement = async (product) => {
  if (product.count > 0) {
    product.count--
    try {
      if (product.count === 0) {
        await cartApi.remove(product.id)
      } else {
        await cartApi.add(product.id, product.count)
      }
    } catch (e) {
      console.error(e)
      product.count++ // Откат при ошибке
    }
  }
}

// --- Lifecycle ---
const handleResize = () => {
  calculateSliderMetrics()
}

onMounted(() => {
  loadData()
  window.addEventListener('resize', handleResize)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', handleResize)
  if (promoInterval) clearInterval(promoInterval)
})
</script>

<style scoped>
.page-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.home {
  flex: 1;
  padding: 40px 48px;
  background: #fafafa;
}

/* SEARCH */
.search-section { margin-bottom: 40px; }
.search-bar {
  display: flex;
  align-items: center;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 30px;
  padding: 8px 20px;
  gap: 12px;
  max-width: 800px;
  margin: 0 auto;
}
.search-icon { color: #999; }
.search-bar input {
  flex: 1;
  border: none;
  outline: none;
  font-size: 16px;
}
.search-btn {
  background: #ff8800;
  color: #fff;
  border: none;
  border-radius: 20px;
  padding: 8px 24px;
  cursor: pointer;
  transition: background 0.2s;
}
.search-btn:hover { background: #e07700; }

/* PROMO BANNER STYLES */
.promo { margin-bottom: 50px; }
.promo-container {
  position: relative;
  height: 240px;
  border-radius: 20px;
  /* overflow: hidden; <- Убираем отсюда, переносим в wrapper */
  max-width: 1200px;
  margin: 0 auto;
  box-shadow: 0 10px 25px rgba(0,0,0,0.08);
}

/* Окно просмотра */
.promo-wrapper {
  width: 100%;
  height: 100%;
  overflow: hidden; /* Скрываем то, что вылезает за границы */
  border-radius: 20px; /* Скругление переносим сюда */
  position: relative;
}

/* Длинная лента, которая двигается */
.promo-track {
  display: flex;
  height: 100%;
  transition: transform 0.5s cubic-bezier(0.25, 1, 0.5, 1); /* Плавная анимация */
}

/* Сам баннер */
.promo-banner {
  min-width: 100%; /* Каждый баннер занимает 100% ширины окна */
  height: 100%;
  padding: 40px 60px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: relative; /* Возвращаем relative */
  /* Убираем opacity и absolute, так как теперь это flex-элементы */
}

.promo-content {
  max-width: 60%;
  color: #333;
}

.promo-content h3 {
  font-size: 32px;
  font-weight: 800;
  margin-bottom: 12px;
  line-height: 1.2;
}

.promo-content p {
  font-size: 18px;
  margin-bottom: 20px;
  opacity: 0.8;
}

.promo-tag {
  display: inline-block;
  background: #fff;
  color: #333;
  padding: 8px 16px;
  border-radius: 30px;
  font-weight: 700;
  box-shadow: 0 4px 10px rgba(0,0,0,0.1);
}

.promo-img-placeholder {
  font-size: 100px;
  opacity: 0.8;
  animation: float 3s ease-in-out infinite;
}

@keyframes float {
  0% { transform: translateY(0px); }
  50% { transform: translateY(-15px); }
  100% { transform: translateY(0px); }
}

/* Стрелки навигации */
.promo-nav {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: rgba(255,255,255,0.7);
  border: none;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  font-size: 24px;
  cursor: pointer;
  z-index: 10; /* Поднимаем выше контента */
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.2s;
}
.promo-nav:hover { background: #fff; }
.promo-nav.prev { left: 20px; }
.promo-nav.next { right: 20px; }

/* Точки */
.promo-dots {
  position: absolute;
  bottom: 20px;
  left: 50%;
  transform: translateX(-50%);
  display: flex;
  gap: 8px;
  z-index: 10;
}
.dot {
  width: 10px;
  height: 10px;
  background: rgba(0,0,0,0.2);
  border-radius: 50%;
  cursor: pointer;
  transition: all 0.3s;
}
.dot.active {
  background: #fff;
  transform: scale(1.2);
}
/* POPULAR SLIDER */
.popular h2 { margin-bottom: 24px; font-size: 28px; }

.slider-container { 
  display: flex; 
  align-items: center; 
  position: relative;
}

.nav-btn {
  font-size: 32px;
  background: #fff;
  border: 1px solid #eee;
  width: 44px;
  height: 44px;
  border-radius: 50%;
  cursor: pointer;
  color: #333;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2;
  box-shadow: 0 4px 10px rgba(0,0,0,0.05);
  transition: all 0.2s;
  flex-shrink: 0;
}
.nav-btn:hover:not(.disabled) { color: #ff8800; border-color: #ff8800; }
.nav-btn.disabled { opacity: 0.3; cursor: default; }

.slider-window { 
  overflow: hidden; 
  flex: 1; 
  margin: 0 20px; 
}

.slider-track {
  display: flex;
  gap: 20px; /* Соответствует переменной GAP в JS */
  transition: transform 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94);
  padding: 10px 5px; /* Немного padding чтобы тени не обрезались */
}

.product-card {
  /* Фиксируем ширину, чтобы расчеты JS совпадали с CSS */
  min-width: 220px;
  width: 220px;
  
  border-radius: 16px;
  background: #fff;
  display: flex;
  flex-direction: column;
  box-shadow: 0 4px 12px rgba(0,0,0,0.04);
  padding: 16px;
  cursor: pointer;
  transition: transform 0.2s, box-shadow 0.2s;
}

.product-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 20px rgba(0,0,0,0.08);
}

.product-image {
  width: 100%;
  height: 160px;
  background: #f5f5f5;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 16px;
  overflow: hidden;
}

.product-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.product-info {
  display: flex;
  flex-direction: column;
  flex: 1;
}

.product-name {
  font-size: 15px;
  font-weight: 500;
  line-height: 1.4;
  margin-bottom: 12px;
  flex: 1; /* Чтобы прибить цену к низу */
}

.product-bottom {
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 36px;
}

.product-price {
  font-size: 18px;
  font-weight: 700;
  color: #333;
}

/* Кнопки */
.add-cart-btn {
  background: #ffefe0;
  color: #ff8800;
  border: none;
  border-radius: 12px;
  padding: 8px 16px;
  font-weight: 600;
  font-size: 13px;
  cursor: pointer;
  transition: all 0.2s;
}
.add-cart-btn:hover { background: #ff8800; color: #fff; }

/* Капсула счётчика */
.counter-pill {
  display: flex;
  align-items: center;
  background: #ff8800;
  border-radius: 12px;
  height: 32px;
  padding: 0 2px;
}

.counter-value {
  color: #fff;
  font-weight: 600;
  min-width: 24px;
  text-align: center;
  font-size: 14px;
}

.counter-btn {
  width: 28px;
  height: 28px;
  background: transparent;
  border: none;
  cursor: pointer;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.counter-btn::before, .counter-btn::after {
  content: "";
  position: absolute;
  background: #fff;
  border-radius: 2px;
}

/* Minus */
.counter-btn.minus::before { width: 10px; height: 2px; }

/* Plus */
.counter-btn.plus::before { width: 10px; height: 2px; }
.counter-btn.plus::after { width: 2px; height: 10px; }

</style>