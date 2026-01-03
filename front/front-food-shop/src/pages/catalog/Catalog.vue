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
            :key="i"
          >
            <div
              class="image-placeholder"
              :style="{ backgroundImage: p.image ? `url(${p.image})` : '' }"
            />

            <div class="card-info">
              <p class="product-name">{{ p.name }}</p>
              <p class="product-category">
                {{ p.category }}
                <span v-if="p.subcategory"> / {{ p.subcategory }}</span>
              </p>
              <p class="product-price">{{ p.price }} ₽</p>
            </div>
          </div>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import Header from './Header.vue'
import Sidebar from './Sidebar.vue'

const searchQuery = ref('')

const activeFilters = ref({
  category: null,
  subcategory: null
})

const products = ref([
  {
    name: 'Манго сушёное',
    price: '450',
    image: '',
    category: 'Фрукты',
    subcategory: 'Тропические'
  },
  {
    name: 'Морковь свежая',
    price: '120',
    image: '',
    category: 'Овощи',
    subcategory: 'Корнеплоды'
  },
  {
    name: 'Апельсиновый сок',
    price: '250',
    image: '',
    category: 'Напитки',
    subcategory: 'Соки'
  }
])

const applyFilters = (filters) => {
  activeFilters.value = filters
}

const resetFilters = () => {
  activeFilters.value = {
    category: null,
    subcategory: null
  }
}

const filteredProducts = computed(() => {
  return products.value.filter(p => {
    const matchesSearch =
      !searchQuery.value ||
      p.name.toLowerCase().includes(searchQuery.value.toLowerCase())

    const matchesCategory =
      !activeFilters.value.category ||
      p.category === activeFilters.value.category

    const matchesSubcategory =
      !activeFilters.value.subcategory ||
      p.subcategory === activeFilters.value.subcategory

    return matchesSearch && matchesCategory && matchesSubcategory
  })
})
</script>

<style scoped>
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

.product-card {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.image-placeholder {
  width: 100%;
  aspect-ratio: 1 / 1;
  background: linear-gradient(135deg, #e0e0e0 0%, #f0f0f0 100%);
  border-radius: 25px;
  margin-bottom: 10px;
  background-size: cover;
  background-position: center;
}

.card-info {
  text-align: center;
}

.product-name {
  font-weight: 800;
  margin: 5px 0 0;
}

.product-category {
  font-size: 13px;
  color: #555;
}

.product-price {
  font-weight: 500;
}
</style>
