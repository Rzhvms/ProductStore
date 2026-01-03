<template>
  <Header />

  <main class="home">
    <!-- SEARCH -->
    <section class="search-section">
      <div class="search-bar">
        <span class="search-icon">🔍</span>
        <input type="text" placeholder="Поиск товаров..." />
        <button class="search-btn">Найти</button>
      </div>
    </section>

    <!-- PROMO -->
    <section class="promo">
      <div class="promo-banner">
        Рекламный баннер / акции
      </div>
    </section>

    <!-- POPULAR PRODUCTS -->
    <section class="popular">
      <h2>Популярные товары</h2>

      <div class="slider-container">
        <!-- LEFT -->
        <button
          v-if="canScrollLeft"
          class="nav-btn left"
          @click="scrollLeft"
        >
          ‹
        </button>

        <div class="slider-wrapper" ref="wrapperRef">
          <div
            class="slider"
            ref="sliderRef"
            :style="{ transform: `translateX(-${offset}px)` }"
          >
            <div class="product-card" v-for="n in 10" :key="n">
              <div class="product-image">🖼️</div>
              <div class="product-name">
                Название товара заглушка {{ n }}
              </div>
              <div class="product-bottom">
                <div class="product-price">₽999</div>
                <button class="add-cart-btn">Добавить</button>
              </div>
            </div>
          </div>
        </div>

        <!-- RIGHT -->
        <button
          v-if="canScrollRight"
          class="nav-btn right"
          @click="scrollRight"
        >
          ›
        </button>
      </div>
    </section>
  </main>

  <Footer />
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import Header from './Header.vue'
import Footer from './Footer.vue'

const wrapperRef = ref(null)
const sliderRef = ref(null)

const CARD_WIDTH = 220 // 200 + gap 20
const offset = ref(0)

const maxOffset = computed(() => {
  if (!wrapperRef.value || !sliderRef.value) return 0
  return Math.max(
    sliderRef.value.scrollWidth - wrapperRef.value.clientWidth,
    0
  )
})

const canScrollLeft = computed(() => offset.value > 0)
const canScrollRight = computed(() => offset.value < maxOffset.value)

const scrollLeft = () => {
  offset.value = Math.max(offset.value - CARD_WIDTH, 0)
}

const scrollRight = () => {
  offset.value = Math.min(offset.value + CARD_WIDTH, maxOffset.value)
}

const handleResize = () => {
  if (offset.value > maxOffset.value) {
    offset.value = maxOffset.value
  }
}

onMounted(() => {
  window.addEventListener('resize', handleResize)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', handleResize)
})
</script>

<style scoped>
.home {
  padding: 40px 48px;
  background: #fafafa;
}

/* SEARCH */
.search-section {
  margin-bottom: 40px;
}
.search-bar {
  display: flex;
  align-items: center;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 30px;
  padding: 8px 20px;
  gap: 12px;
}
.search-icon {
  color: #999;
}
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
  padding: 8px 20px;
  cursor: pointer;
}

/* PROMO */
.promo {
  margin-bottom: 60px;
}
.promo-banner {
  height: 220px;
  background: #e5e5e5;
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  color: #555;
}

/* POPULAR */
.popular h2 {
  margin-bottom: 20px;
}

.slider-container {
  display: flex;
  align-items: center;
  position: relative;
}

.nav-btn {
  font-size: 32px;
  background: none;
  border: none;
  cursor: pointer;
  color: #888;
  z-index: 2;
}

.nav-btn:hover {
  color: #ff8800;
}

.slider-wrapper {
  overflow: hidden;
  flex: 1;
  margin: 0 10px;
}

.slider {
  display: flex;
  gap: 20px;
  transition: transform 0.3s ease;
}

.product-card {
  width: 200px;
  flex-shrink: 0;
  border-radius: 16px;
  background: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  box-shadow: 0 3px 10px rgba(0, 0, 0, 0.05);
  padding: 12px;
}

.product-image {
  width: 200px;
  height: 200px;
  background: #eee;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 48px;
  margin-bottom: 12px;
}

.product-name {
  width: 100%;
  font-size: 14px;
  font-weight: 500;
  line-height: 18px;
  height: 36px;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  text-align: center;
  margin-bottom: 8px;
}

.product-bottom {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.product-price {
  font-weight: 600;
  color: #ff8800;
}

.add-cart-btn {
  background: #ff8800;
  color: #fff;
  border: none;
  border-radius: 16px;
  padding: 6px 10px;
  font-size: 12px;
  cursor: pointer;
}
</style>