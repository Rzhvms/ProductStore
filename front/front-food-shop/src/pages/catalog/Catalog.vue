<template>
  <div class="catalog-page">
    <Header />

    <div class="catalog-body">
      <Sidebar
          :filters="activeFilters"
          @update:filters="applyFilters"
      />

      <main class="catalog-content">
        <!-- КНОПКА НАЗАД -->
        <div v-if="activeFilters.category" class="back-row">
          <button class="back-btn" @click="resetFilters">
            ← Все товары
          </button>

          <span class="current-filter">
            {{ activeFilters.category }}
            <span v-if="activeFilters.subcategory">
              / {{ activeFilters.subcategory }}
            </span>
          </span>
        </div>

        <!-- SEARCH -->
        <div class="search-row">
          <input
              type="text"
              placeholder="Поиск по названию товара"
              v-model="searchQuery"
          />
        </div>

        <!-- PRODUCTS -->
        <div class="products-grid">
          <div
              class="product-card"
              v-for="(p, i) in filteredProducts"
              :key="p.id ?? i"
              @click="goToProduct(p.id)"
              @keydown.enter="goToProduct(p.id)"
              role="button"
              tabindex="0"
              :aria-label="`Открыть товар ${p.name}`"
          >
            <div class="product-image">
              <img
                  v-if="p.image"
                  :src="p.image"
                  :alt="p.name"
                  class="img-fluid"
                  loading="lazy"
              />
              <div v-else class="placeholder-icon">🖼️</div>
            </div>

            <div class="product-name">{{ p.name }}</div>

            <div class="product-bottom">
              <div class="product-price">{{ formatPrice(p.price) }} ₽</div>

              <div v-if="p.count === 0">
                <button
                    class="add-cart-btn"
                    @click.stop="increment(p)"
                    @mouseenter="hoverBtn = p.id"
                    @mouseleave="hoverBtn = null"
                    @mousedown="activeBtn = p.id"
                    @mouseup="activeBtn = null"
                    :class="{ hover: hoverBtn === p.id, active: activeBtn === p.id }"
                    :disabled="isPending(p)"
                >
                  Добавить
                </button>
              </div>

              <div v-else class="counter-pill" @click.stop>
                <button class="counter-btn" @click.stop="decrement(p)" :disabled="isPending(p)"></button>
                <span class="counter-value">{{ p.count }}</span>
                <button class="counter-btn" @click.stop="increment(p)" :disabled="isPending(p)"></button>
              </div>
            </div>
          </div>
        </div>

        <!-- LOADING / EMPTY -->
        <div v-if="isLoading" class="loading">Загрузка...</div>
        <div v-else-if="!isLoading && products.length === 0" class="empty-state">
          <p>Товары не найдены</p>
        </div>

        <!-- PAGINATION (минимальная) -->
        <div class="pagination" v-if="totalPages > 1">
          <button
              class="pagination-btn"
              :disabled="pageNumber === 1"
              @click="changePage(pageNumber - 1)"
          >
            ← Назад
          </button>

          <span class="page-info">
            Страница {{ pageNumber }} из {{ totalPages }}
          </span>

          <button
              class="pagination-btn"
              :disabled="pageNumber === totalPages"
              @click="changePage(pageNumber + 1)"
          >
            Вперед →
          </button>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import Header from './Header.vue'
import Sidebar from './Sidebar.vue'
import api from '@/api/instance'

const router = useRouter()

const searchQuery = ref('')

const activeFilters = ref({
  category: null,
  categoryId: null
})

const products = ref([])

const pageNumber = ref(1)
const pageSize = ref(12)
const totalItems = ref(0)

const isLoading = ref(false)

const hoverBtn = ref(null)
const activeBtn = ref(null)

const pendingIds = ref([])

/* ================= helpers ================= */

const addPending = (id) => {
  if (!pendingIds.value.includes(id)) pendingIds.value.push(id)
}
const removePending = (id) => {
  const i = pendingIds.value.indexOf(id)
  if (i !== -1) pendingIds.value.splice(i, 1)
}
const isPending = (p) => pendingIds.value.includes(p.id)

const formatPrice = (price) =>
    Number(price ?? 0).toLocaleString('ru-RU', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    })

const totalPages = computed(() =>
    Math.max(1, Math.ceil(totalItems.value / pageSize.value))
)

/* ================= API ================= */

let requestId = 0
const loadProducts = async () => {
  isLoading.value = true
  const rid = ++requestId

  try {
    const params = {
      pageNumber: pageNumber.value,
      pageSize: pageSize.value
    }

    if (searchQuery.value.trim()) {
      params.search = searchQuery.value.trim()
    }

    const res = await api.get('product/list', { params })
    if (rid !== requestId) return

    const list = Array.isArray(res.data?.productList)
        ? res.data.productList
        : []

    products.value = list.map(p => ({
      ...p,
      count: Number.isInteger(p.count) ? p.count : 0
    }))

    totalItems.value = res.data?.totalCount ?? list.length
  } catch (e) {
    console.error(e)
    products.value = []
    totalItems.value = 0
  } finally {
    isLoading.value = false
  }
}

/* ================= FILTERING ================= */

const filteredProducts = computed(() => {
  const q = searchQuery.value.toLowerCase().trim()
  const categoryId = activeFilters.value.categoryId

  return products.value.filter(p => {
    const matchSearch =
        !q || p.name?.toLowerCase().includes(q)

    const matchCategory =
        !categoryId || String(p.categoryId) === String(categoryId)

    return matchSearch && matchCategory
  })
})

/* ================= WATCHERS ================= */

let searchTimer = null
watch(searchQuery, () => {
  pageNumber.value = 1
  clearTimeout(searchTimer)
  searchTimer = setTimeout(loadProducts, 400)
})

/* ================= EVENTS ================= */

const applyFilters = (filters) => {
  activeFilters.value = {
    category: filters.category ?? null,
    categoryId: filters.categoryId ?? null
  }
  pageNumber.value = 1
  // ❗ API НЕ дергаем
}

const resetFilters = () => {
  activeFilters.value = { category: null, categoryId: null }
  pageNumber.value = 1
}

const changePage = (page) => {
  if (page < 1 || page > totalPages.value) return
  pageNumber.value = page
  loadProducts()
}

const goToProduct = (id) => {
  router.push(`/catalog/product/${id}`)
}

/* ================= CART ================= */

const increment = async (p) => {
  if (isPending(p)) return
  const prev = p.count
  p.count++
  addPending(p.id)

  try {
    await api.post('cart/items', {
      productId: p.id,
      quantity: p.count
    })
  } catch {
    p.count = prev
  } finally {
    removePending(p.id)
  }
}

const decrement = async (p) => {
  if (isPending(p) || p.count <= 0) return
  const prev = p.count
  p.count--
  addPending(p.id)

  try {
    await api.post('cart/items', {
      productId: p.id,
      quantity: p.count
    })
  } catch {
    p.count = prev
  } finally {
    removePending(p.id)
  }
}

onMounted(loadProducts)
</script>


<style scoped>
/* (стили оставлены без изменений) */
.catalog-page {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.catalog-body {
  display: flex;
  flex: 1;
  overflow: hidden;
}

.catalog-content {
  flex: 1;
  padding: 30px 40px;
  overflow-y: auto;
}

/* BACK */
.back-row {
  display: flex;
  align-items: center;
  gap: 15px;
  margin-bottom: 20px;
}

.back-btn {
  border: none;
  background: #f57c00;
  color: #fff;
  padding: 8px 14px;
  border-radius: 8px;
  font-weight: 700;
  cursor: pointer;
}

.current-filter {
  font-weight: 700;
  color: #555;
}

/* SEARCH */
.search-row {
  margin-bottom: 30px;
}

.search-row input {
  width: 100%;
  max-width: 400px;
  padding: 12px 15px;
  border-radius: 8px;
  border: 1px solid #ddd;
}

/* GRID */
.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 20px;
}

/* === Новая карточка (по примеру слайдера) === */
.product-card {
  border-radius: 16px;
  background: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  box-shadow: 0 3px 10px rgba(0,0,0,0.05);
  padding: 12px;
  cursor: pointer;
  transition: transform 0.12s ease, box-shadow 0.12s ease;
  outline: none;
}

.product-card:hover {
  transform: translateY(-6px);
  box-shadow: 0 12px 24px rgba(0,0,0,0.08);
}

.product-card:active {
  transform: translateY(-2px);
}

/* Изображение */
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
  overflow: hidden;
}

.product-image img.img-fluid {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

/* Название */
.product-name {
  font-size: 14px;
  font-weight: 500;
  text-align: center;
  margin-bottom: 8px;
  color: #222;
  min-height: 38px; /* чтобы карточки были ровнее */
}

/* Нижняя панель (цена + кнопки) */
.product-bottom {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  gap: 8px;
}

/* Цена */
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
  padding: 6px 12px;
  font-size: 13px;
  cursor: pointer;
  transition: background 0.12s ease, transform 0.08s ease;
}
.add-cart-btn.hover { background: #ffa533; }
.add-cart-btn.active { background: #cc6600; transform: translateY(1px); }
.add-cart-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Капсула счётчика */
.counter-pill {
  display: flex;
  align-items: center;
  justify-content: space-between;
  border: 1px solid #ff8800;
  border-radius: 16px;
  height: 32px;
  padding: 0 6px;
  min-width: 92px;
  gap: 8px;
}

.counter-value {
  min-width: 24px;
  text-align: center;
  font-weight: 600;
}

/* Кнопки + и - */
.counter-btn {
  position: relative;
  width: 28px;
  height: 28px;
  background: #ff8800;
  border: none;
  border-radius: 50%;
  cursor: pointer;
}
.counter-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* - */
.counter-btn:first-child::before {
  content: "";
  position: absolute;
  left: 50%;
  top: 50%;
  width: 12px;
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
  width: 12px;
  height: 2px;
  background: #fff;
  transform: translate(-50%, -50%);
}
.counter-btn:last-child::after {
  transform: translate(-50%, -50%) rotate(90deg);
}

.loading,
.empty-state {
  text-align: center;
  margin: 40px 0;
  color: #666;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;
  margin-top: 32px;
  padding-top: 20px;
  border-top: 1px solid #eee;
}

.pagination-btn {
  padding: 10px 18px;
  background-color: #f57c00;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}
.pagination-btn:disabled {
  background: #ccc;
  cursor: not-allowed;
}
.page-info {
  font-size: 14px;
  color: #555;
}
</style>
