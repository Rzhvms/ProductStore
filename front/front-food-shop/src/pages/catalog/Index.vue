<template>
  <div class="page-container">
    <Header />

    <main class="home">
      <section class="search-section">
        <div class="search-bar">
          <span class="search-icon">🔍</span>
          <input type="text" placeholder="Поиск товаров..." />
          <button class="search-btn">Найти</button>
        </div>
      </section>

      <section class="promo">
        <div class="promo-banner">
          Рекламный баннер / акции
        </div>
      </section>

      <section class="popular">
        <h2>Популярные товары</h2>

        <div class="slider-container">
          <button v-if="canScrollLeft" class="nav-btn left" @click="scrollLeft">‹</button>

          <div class="slider-wrapper" ref="wrapperRef">
            <div
              class="slider"
              ref="sliderRef"
              :style="{ transform: `translateX(-${offset}px)` }"
            >
              <div 
                class="product-card" 
                v-for="product in products" 
                :key="product.id"
                @click="goToProduct(product.id)" 
                style="cursor: pointer;"
              >
                <div class="product-image">
                  <img
                    v-if="product.image"
                    :src="product.image"
                    :alt="product.name"
                    class="img-fluid"
                  />
                  <span v-else>🖼️</span>
                </div>

                <div class="product-name">{{ product.name }}</div>

                <div class="product-bottom">
                  <div class="product-price">₽{{ product.price }}</div>

                  <div v-if="product.count === 0">
                    <button
                      class="add-cart-btn"
                      @click.stop="increment(product)"
                      @mouseenter="hoverBtn = product.id"
                      @mouseleave="hoverBtn = null"
                      @mousedown="activeBtn = product.id"
                      @mouseup="activeBtn = null"
                      :class="{ hover: hoverBtn === product.id, active: activeBtn === product.id }"
                    >
                      Добавить
                    </button>
                  </div>

                  <div v-else class="counter-pill">
                    <button class="counter-btn" @click.stop="decrement(product)"></button>
                    <span class="counter-value">{{ product.count }}</span>
                    <button class="counter-btn" @click.stop="increment(product)"></button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <button v-if="canScrollRight" class="nav-btn right" @click="scrollRight">›</button>
        </div>
      </section>
    </main>

    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { useRouter } from 'vue-router'
import Header from './Header.vue'
import Footer from './Footer.vue'
import { productApi, cartApi } from '@/services/api'

const router = useRouter()
const products = ref([])
const pageNumber = ref(1)
const pageSize = ref(10)

const wrapperRef = ref(null)
const sliderRef = ref(null)
const CARD_WIDTH = 220
const offset = ref(0)

const hoverBtn = ref(null)
const activeBtn = ref(null)

const loadData = async () => {
  try {
    const data = await productApi.getList(pageNumber.value, pageSize.value)
    products.value = data.productList?.map(p => ({
      ...p,
      count: 0
    })) || []
  } catch (error) {
    console.error('Ошибка загрузки товаров:', error)
    products.value = []
  }
}

const goToProduct = (id) => router.push(`/catalog/product/${id}`)

const increment = async (product) => {
  product.count++
  await cartApi.add(product.id, product.count)
}

const decrement = async (product) => {
  if (product.count > 0) {
    product.count--
    await cartApi.add(product.id, product.count)
  } else {
    await cartApi.remove(product.id)
  }
}

const maxOffset = computed(() => {
  if (!wrapperRef.value || !sliderRef.value || products.value.length === 0) return 0
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
  loadData()
  window.addEventListener('resize', handleResize)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', handleResize)
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
  padding: 8px 20px;
  cursor: pointer;
}

/* PROMO */
.promo { margin-bottom: 60px; }
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
.popular h2 { margin-bottom: 20px; }
.slider-container { display: flex; align-items: center; }
.nav-btn {
  font-size: 32px;
  background: none;
  border: none;
  cursor: pointer;
  color: #888;
}
.nav-btn:hover { color: #ff8800; }

.slider-wrapper { overflow: hidden; flex: 1; margin: 0 10px; }
.slider {
  display: flex;
  gap: 20px;
  transition: transform 0.3s ease;
}

.product-card {
  border-radius: 16px;
  background: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  box-shadow: 0 3px 10px rgba(0,0,0,0.05);
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
  font-size: 14px;
  font-weight: 500;
  text-align: center;
  margin-bottom: 8px;
}

.product-bottom {
  display: flex;
  justify-content: space-between;
  width: 100%;
}

.product-price {
  font-weight: 600;
  color: #ff8800;
}

/* Кнопка Добавить */
.add-cart-btn {
  background: #ff8800;
  color: #fff;
  border: none;
  border-radius: 16px;
  padding: 6px 10px;
  font-size: 12px;
  cursor: pointer;
}
.add-cart-btn.hover { background: #ffa533; }
.add-cart-btn.active { background: #cc6600; }

/* Капсула счётчика */
.counter-pill {
  display: flex;
  align-items: center;
  justify-content: space-between;
  border: 1px solid #ff8800;
  border-radius: 16px;
  height: 30px;
  padding: 0 4px;
  min-width: 78px;
}

.counter-btn {
  position: relative;
  width: 20px;
  height: 20px;
  background: #ff8800;
  border: none;
  border-radius: 50%;
  cursor: pointer;
}

/* - */
.counter-btn:first-child::before {
  content: "";
  position: absolute;
  left: 50%;
  top: 50%;
  width: 10px;
  height: 2px;
  background: #fff;
  transform: translate(-50%, -50%);
}

/* + */
.counter-btn:last-child::before,
.counter-btn:last-child::after {
  content: "";
  position: absolute;
  left: 50%;
  top: 50%;
  width: 10px;
  height: 2px;
  background: #fff;
  transform: translate(-50%, -50%);
}

.counter-btn:last-child::after {
  transform: translate(-50%, -50%) rotate(90deg);
}
</style>
