<template>
  <aside class="sidebar-container">
    <!-- КНОПКА КАТАЛОГА -->
    <div class="catalog-btn" @click="isCatalogOpen = !isCatalogOpen">
      <div class="burger-icon">
        <span />
        <span />
        <span />
      </div>
      <span>Каталог</span>
      <span class="main-arrow" :class="{ rotated: isCatalogOpen }">▼</span>
    </div>

    <!-- КАТЕГОРИИ -->
    <Transition name="fade">
      <div v-if="isCatalogOpen" class="catalog-list">
        <div
          v-for="cat in categories"
          :key="cat.name"
          class="category-item"
        >
          <!-- КАТЕГОРИЯ -->
          <div
            class="category-title"
            :class="{ active: activeCategory === cat.name }"
            @click="selectCategory(cat.name)"
          >
            {{ cat.name }}
          </div>

          <!-- ПОДКАТЕГОРИИ -->
          <div
            v-if="cat.sub && activeCategory === cat.name"
            class="subcategory-list"
          >
            <div
              v-for="sub in cat.sub"
              :key="sub"
              class="subcategory-item"
              :class="{ active: activeSubcategory === sub }"
              @click.stop="selectSubcategory(sub)"
            >
              {{ sub }}
            </div>
          </div>
        </div>
      </div>
    </Transition>
  </aside>
</template>

<script setup>
import { ref, watch } from 'vue'

const emit = defineEmits(['update:filters'])

const props = defineProps({
  filters: {
    type: Object,
    required: true
  }
})

const isCatalogOpen = ref(true)
const activeCategory = ref(null)
const activeSubcategory = ref(null)

/* те же категории, что в админке */
const categories = [
  { name: 'Фрукты', sub: ['Цитрусовые', 'Ягоды', 'Тропические'] },
  { name: 'Овощи', sub: ['Корнеплоды', 'Листовые', 'Бобовые'] },
  { name: 'Напитки', sub: ['Соки', 'Чай', 'Кофе'] }
]

const selectCategory = (name) => {
  activeCategory.value = name
  activeSubcategory.value = null
}

const selectSubcategory = (sub) => {
  activeSubcategory.value = sub
}

/* ⬅️ СИНХРОНИЗАЦИЯ СО СБРОСОМ ИЗ CATALOG */
watch(
  () => props.filters,
  (newFilters) => {
    activeCategory.value = newFilters.category
    activeSubcategory.value = newFilters.subcategory
  },
  { immediate: true, deep: true }
)

/* ⬆️ ОТПРАВКА ФИЛЬТРОВ В CATALOG */
watch([activeCategory, activeSubcategory], () => {
  emit('update:filters', {
    category: activeCategory.value,
    subcategory: activeSubcategory.value
  })
})
</script>

<style scoped>
.sidebar-container {
  width: 280px;
  background: #fff;
  border-right: 1px solid #eee;
  display: flex;
  flex-direction: column;
}

.catalog-btn {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 18px 20px;
  font-weight: 800;
  cursor: pointer;
}

.catalog-btn:hover {
  background: #f7f7f7;
}

.burger-icon span {
  display: block;
  width: 18px;
  height: 2px;
  background: #333;
  margin-bottom: 3px;
}

.main-arrow {
  margin-left: auto;
  transition: 0.3s;
  font-size: 12px;
}

.main-arrow.rotated {
  transform: rotate(180deg);
}

.catalog-list {
  padding: 10px 0;
}

.category-item {
  padding: 0 20px;
}

.category-title {
  padding: 10px 0;
  font-weight: 700;
  cursor: pointer;
}

.category-title.active {
  color: #f57c00;
}

.subcategory-list {
  padding-left: 15px;
  margin-bottom: 10px;
}

.subcategory-item {
  padding: 6px 0;
  font-size: 14px;
  cursor: pointer;
  color: #555;
}

.subcategory-item.active {
  color: #f57c00;
  font-weight: 700;
}
</style>
