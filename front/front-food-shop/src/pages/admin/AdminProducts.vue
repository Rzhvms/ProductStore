<template>
  <AdminLayout>
    <div class="content-wrapper">
      <div class="page-header">
        <div class="title-section">
          <div class="icon-box orange-box">
            <img src="../../assets/box.svg" alt="Logo" class="icon-orange" />
          </div>
          <h1>Товары</h1>
        </div>

        <div class="card-actions">
          <div class="search-box-styled card">
            <img src="../../assets/search-normal.svg" alt="search" class="search-icon" />
            <input type="text" placeholder="Поиск товара..." v-model="searchQuery">
            <img src="../../assets/close-circle.svg" class="clear-circle" @click="searchQuery = ''" />
          </div>

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

          <button class="add-circle-btn card" @click="router.push('/admin/products/add')">
            <img src="../../assets/add-circle.svg" />
          </button>
        </div>
      </div>
      <div v-if="sortOption" class="active-filters">
        <span class="filter-tag">{{ sortLabel }} <button class="tag-remove" @click="sortOption = ''">×</button></span>
      </div>
      <div class="products-list">
        <div class="product-row" v-for="(p, i) in filteredProducts" :key="p.id" @click="openProduct(p.id)">
  
          <div class="row-image">
            <div class="img-placeholder" :style="{ backgroundImage: p.image ? `url(${p.image})` : '' }"></div>
          </div>

          <div class="row-content">
            
            <div class="row-main">
              <h2 class="p-name">{{ p.name }}</h2>
              <div class="rat-pri">
                <div class="row-rating">
                  <div class="stars-wrapper">
                    <svg v-for="star in 5" :key="star" class="star-icon" width="32" height="32" viewBox="0 0 24 24">
                      <defs>
                        <linearGradient :id="'grad-' + p.id + '-' + star">
                          <stop offset="0%" stop-color="#FF7A00" />
                          <stop :offset="calculateOffset(p.rating, star)" stop-color="#FF7A00" />
                          <stop :offset="calculateOffset(p.rating, star)" stop-color="#E5E7EB" />
                          <stop offset="100%" stop-color="#E5E7EB" />
                        </linearGradient>
                      </defs>
                      <path 
                        :fill="'url(#grad-' + p.id + '-' + star + ')'"
                        d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" 
                      />
                    </svg>
                  </div>
                  <span class="rating-value">{{ p.rating }}</span>
                </div>

                <div class="row-price">{{ p.price }} ₽</div>
              </div>      
            </div>

            <!-- Контейнер: категория, подкатегория -->
            <div class="row-meta">
              <span class="p-cat">Категория: {{ getCategoryName(p.categoryId) }}</span>
              <span class="p-sub" v-if="getCategoryName(p.categoryId) !== 'Не указана'">Подкатегория: {{ getSubcategoryName(p.categoryId) }}</span>
            </div>

          </div>

        </div>
        
        <div v-if="filteredProducts.length === 0" style="text-align:center; padding: 20px; color: #999;">
            Товары не найдены
        </div>
      </div>
    </div>

    <div class="filters-overlay" v-if="showFilters" @click.self="closeFilters">
      <div class="filters-sidebar">
        <div class="sidebar-header">
          <h2>Фильтры</h2>
          <button class="close-btn" @click="closeFilters">×</button>
        </div>

        <div class="sidebar-content">
          <!-- Цена -->
          <div class="filter-block">
            <label class="f-label">Цена</label>
            <div class="range-inputs">
              <div class="input-wrap">
                <span>От:</span>
                <input :value="tempFilters.priceFrom" placeholder="0" @input="validateInput(tempFilters, 'priceFrom', $event)" @change="checkLimits(tempFilters, 'priceFrom', 0, 1000000)">
              </div>
              <div class="input-wrap">
                <span>До:</span>
                <input :value="tempFilters.priceTo" @input="validateInput(tempFilters, 'priceTo', $event)" placeholder="1000000" @change="checkLimits(tempFilters, 'priceTo', 0, 1000000)">
              </div>
            </div>
          </div>

          <!-- Рейтинг -->
          <div class="filter-block">
            <label class="f-label">Рейтинг</label>
            <div class="range-inputs">
              <div class="input-wrap">
                <span>От:</span>
                <input :value="tempFilters.ratingFrom" placeholder="0" @input="validateInput(tempFilters, 'ratingFrom', $event)" @change="checkLimits(tempFilters, 'ratingFrom', 0, 5)">
              </div>
              <div class="input-wrap">
                <span>До:</span>
                <input :value="tempFilters.ratingTo" placeholder="5" @input="validateInput(tempFilters, 'ratingTo', $event)" @change="checkLimits(tempFilters, 'ratingTo', 0, 5)">
              </div>
            </div>
          </div>

          <!-- Категории -->
          <div class="filter-block">
            <label class="f-label">Категория</label>
            <input 
              type="text" 
              class="cat-search" 
              v-model="categorySearch"
              placeholder="Поиск категории или подкатегории"
            >
            
            <div class="cat-tree">
              <div v-for="cat in filteredCategories" :key="cat.name" class="cat-group">
                <div class="cat-parent" @click="toggleCategoryExpand(cat.name)">
                  <label class="checkbox-row" @click.stop>
                    <input 
                      type="checkbox" 
                      :checked="isCategoryFullySelected(cat)"
                      :indeterminate.prop="isCategoryPartiallySelected(cat)"
                      @change="toggleCategory(cat)"
                    >
                    <span class="check-box"></span>
                  </label>
                  <span class="cat-name">{{ cat.name }}</span>
                  <span class="chevron" :class="{ expanded: expandedCategories.includes(cat.name) }">›</span>
                </div>
                <transition name="slide">
                  <div class="cat-children" v-show="expandedCategories.includes(cat.name)">
                    <div class="cat-child" v-for="sub in cat.sub" :key="sub">
                      <label class="checkbox-row">
                        <input 
                          type="checkbox" 
                          :value="sub" 
                          v-model="tempFilters.selectedSubcategories"
                        >
                        <span class="check-box"></span>
                        <span>{{ sub }}</span>
                      </label>
                    </div>
                  </div>
                </transition>
              </div>

              <!-- Пустой результат поиска -->
              <div v-if="filteredCategories.length === 0" class="no-results">
                Ничего не найдено
              </div>
            </div>

            <!-- Кнопка "Посмотреть все" -->
            <div 
              class="show-all-link" 
              v-if="hasMoreCategories"
              @click="showAllCategories = true"
            >
              Посмотреть все ({{ categories.length - visibleCategoriesCount }} ещё)
            </div>
            <div 
              class="show-all-link" 
              v-else-if="showAllCategories && !categorySearch"
              @click="showAllCategories = false"
            >
              Свернуть
            </div>
          </div>
        </div>

        <div class="sidebar-footer">
          <button class="reset-btn" @click="resetFilters">Сбросить</button>
          <button class="apply-btn" @click="applyFilters">Применить</button>
        </div>
      </div>
    </div>
  </AdminLayout>
</template>

<script setup>
import { reactive, ref, computed, watch, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import AdminLayout from './AdminLayout.vue'
import { adminProductApi, categoryApi } from '@/services/api.js'

/* --- STATE --- */
const products = ref([])
const categories = ref([])
const rawCategories = ref([])
const isLoading = ref(false)

const router = useRouter()

const tempFilters = reactive({ 
  priceFrom: null, 
  priceTo: null,
  ratingFrom: null,
  ratingTo: null,
  selectedSubcategories: [] 
});

const filters = reactive({ 
  priceFrom: null, 
  priceTo: null,
  ratingFrom: null,
  ratingTo: null,
  selectedSubcategories: [] 
})

const showFilters = ref(false);
const categorySearch = ref('');
const showAllCategories = ref(false);
const visibleCategoriesCount = 3;
const expandedCategories = ref([]);
const sortOption = ref('');
const searchQuery = ref('');
const showSortDropdown = ref(false);
const showAddProductDialog = ref(false);
const isFiltered = ref(false);

/* --- INITIAL DATA FETCHING --- */
const loadData = async () => {
  isLoading.value = true
  try {
    const catsResponse = await categoryApi.get()
    rawCategories.value = catsResponse
    categories.value = transformCategories(catsResponse)

    const productsResponse = await adminProductApi.get(1, 100)
    products.value = productsResponse.productList
  } catch (error) {
    console.error("Ошибка при загрузке данных:", error.message)
    alert(error.message)
  } finally {
    isLoading.value = false
  }
}

const transformCategories = (rawCats) => {
  const mainCats = rawCats.filter(c => !c.parentCategoryId)
  return mainCats.map(parent => ({
    id: parent.categoryId,
    name: parent.categoryName,
    sub: rawCats.filter(c => c.parentCategoryId === parent.categoryId).map(s => ({
      id: s.categoryId,
      name: s.categoryName
    }))
  }))
}

const getCategoryName = (id) => {
  const cat = rawCategories.value.find(c => c.categoryId === id);
  if (!cat) return 'Не указана';

  if (cat.parentCategoryId) {
    const parent = rawCategories.value.find(c => c.categoryId === cat.parentCategoryId);
    return parent ? parent.categoryName : cat.categoryName;
  }
  
  return cat.categoryName;
}

const getSubcategoryName = (id) => {
  const cat = rawCategories.value.find(c => c.categoryId === id);
  return cat ? cat.categoryName : 'Не указана';
}

onMounted(() => {
  loadData()
})

/* --- MODAL LOGIC --- */
const showAddModal = ref(false)
const showEditModal = ref(false)
const editingId = ref(null)

/* --- METHODS --- */
const validateInput = (obj, key, event) => {
  let val = event.target.value.replace(/[^0-9.]/g, '');
  if ((val.match(/\./g) || []).length > 1) val = val.slice(0, -1);
  event.target.value = val;
  obj[key] = val;
};

const checkLimits = (obj, key, minLimit, maxLimit) => {
    if (obj[key] === '' || obj[key] === null) return;
    let val = parseInt(obj[key]);
    if (isNaN(val)) { obj[key] = ''; return; }
    if (val < minLimit) val = minLimit;
    if (val > maxLimit) val = maxLimit;
    obj[key] = val;
};

// Фильтры
const openFilters = () => {
  Object.assign(tempFilters, filters);
  tempFilters.selectedSubcategories = [...filters.selectedSubcategories];
  showFilters.value = true;
}

const closeFilters = () => { showFilters.value = false; categorySearch.value = ''; }
const toggleFilters = () => showFilters.value ? closeFilters() : openFilters();

const applyFilters = () => {
  Object.assign(filters, tempFilters);
  filters.selectedSubcategories = [...tempFilters.selectedSubcategories];
  showFilters.value = false;
  isFiltered.value = true;
}

const resetFilters = () => {
  const empty = { priceFrom: null, priceTo: null, ratingFrom: null, ratingTo: null, selectedSubcategories: [] };
  Object.assign(tempFilters, empty);
  Object.assign(filters, empty);
  searchQuery.value = '';
  isFiltered.value = false;
  showFilters.value = false;
}

const isCategoryFullySelected = (cat) => cat.sub.length > 0 && cat.sub.every(sub => tempFilters.selectedSubcategories.includes(sub));
const isCategoryPartiallySelected = (cat) => {
  const selectedCount = cat.sub.filter(sub => tempFilters.selectedSubcategories.includes(sub)).length;
  return selectedCount > 0 && selectedCount < cat.sub.length;
}

const toggleCategory = (cat) => {
  if (isCategoryFullySelected(cat)) {
    tempFilters.selectedSubcategories = tempFilters.selectedSubcategories.filter(sub => !cat.sub.includes(sub));
  } else {
    cat.sub.forEach(sub => { if (!tempFilters.selectedSubcategories.includes(sub)) tempFilters.selectedSubcategories.push(sub); });
  }
}
const toggleCategoryExpand = (catName) => {
  const index = expandedCategories.value.indexOf(catName);
  index > -1 ? expandedCategories.value.splice(index, 1) : expandedCategories.value.push(catName);
}

const setSortOption = (opt) => { sortOption.value = opt; showSortDropdown.value = false; }

const deleteProduct = async (id) => {
  if (!confirm('Удалить этот товар?')) return
  try {
    await adminProductApi.delete(id)
    await loadData()
  } catch (error) {
    alert(error.message)
  }
}

const openProduct = (id) => {
  router.push(`/admin/products/${id}`)
}

/* --- COMPUTED --- */
const buttonSortLabel = computed(() => {
  if (!sortOption.value) return 'Сортировка';
  if (sortOption.value.includes('name')) return 'По алфавиту';
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

const filteredCategories = computed(() => {
  const search = categorySearch.value.toLowerCase().trim();
  let cats = [...categories.value];
  if (search) {
    cats = cats.map(c => {
      const catMatches = c.name.toLowerCase().includes(search);
      const subMatches = c.sub.filter(s => s.toLowerCase().includes(search));
      if (catMatches || subMatches.length > 0) return { ...c, sub: catMatches ? c.sub : subMatches };
      return null;
    }).filter(Boolean);
  } else if (!showAllCategories.value) {
    cats = cats.slice(0, visibleCategoriesCount);
  }
  return cats;
});

const hasMoreCategories = computed(() => !showAllCategories.value && !categorySearch.value && categories.value.length > visibleCategoriesCount);

const filteredProducts = computed(() => {
  let prods = [...products.value];
  if (searchQuery.value) {
    prods = prods.filter(p => p.name.toLowerCase().includes(searchQuery.value.toLowerCase()));
  }
  if (filters.priceFrom !== null && filters.priceFrom !== '') {
    prods = prods.filter(p => Number(String(p.price).replace(/\s/g, '')) >= filters.priceFrom);
  }
  if (filters.priceTo !== null && filters.priceTo !== '') {
    prods = prods.filter(p => Number(String(p.price).replace(/\s/g, '')) <= filters.priceTo);
  }
  if (filters.selectedSubcategories.length > 0) {
    prods = prods.filter(p => filters.selectedSubcategories.includes(p.subcategory));
  }

  if (sortOption.value) {
    const getPrice = (p) => Number(String(p.price).replace(/\s/g, ''));
    switch(sortOption.value) {
      case 'name-asc': prods.sort((a, b) => a.name.localeCompare(b.name, 'ru')); break;
      case 'name-desc': prods.sort((a, b) => b.name.localeCompare(a.name, 'ru')); break;
      case 'price-asc': prods.sort((a, b) => getPrice(a) - getPrice(b)); break;
      case 'price-desc': prods.sort((a, b) => getPrice(b) - getPrice(a)); break;
      case 'rating-asc': prods.sort((a, b) => (a.rating || 0) - (b.rating || 0)); break;
      case 'rating-desc': prods.sort((a, b) => (b.rating || 0) - (a.rating || 0)); break;
    }
  }
  return prods;
});

const calculateOffset = (rating, starIndex) => {
  if (!rating) return '0%';
  if (rating >= starIndex) return '100%';
  if (rating <= starIndex - 1) return '0%';
  return ((rating % 1) * 100) + '%';
};

watch(showAddProductDialog, (val) => {
  if(val) { openAddModal(); showAddProductDialog.value = false; }
})
</script>

<style scoped>
@import './admin.css';

/* RESET & BASE */
* { box-sizing: border-box; font-family: 'Inter', sans-serif; }
body { margin: 0; background: #fff; }

.page-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: #ffffff;
}

/* --- HEADER --- */
.top-header {
  height: 60px;
  display: flex;
  align-items: center;
  padding: 0 40px;
  border-bottom: 1px solid #eee;
  background: #fff;
}
.logo {
  color: #FF8C00;
  font-weight: 800;
  font-size: 20px;
  letter-spacing: 0.5px;
  margin-right: 60px;
}
.top-nav {
  display: flex;
  gap: 30px;
  flex: 1;
}
.nav-link {
  text-decoration: none;
  color: #999;
  font-size: 14px;
  font-weight: 500;
}
.nav-link.active { color: #FF8C00; }
.avatar {
  width: 32px; height: 32px;
  background: #f0f0f0;
  border-radius: 50%;
  display: flex; align-items: center; justify-content: center;
  font-size: 18px;
  cursor: pointer;
}

/* --- MAIN CONTENT --- */
.content-wrapper {
  padding: 20px 40px;
  margin: 0 auto;
  width: 100%;
}

/* Page Header & Toolbar */
.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 30px;
}
.title-section {
  display: flex;
  align-items: center;
  gap: 12px;
}
.icon-box {
  width: 24px; height: 24px;
  display: flex; align-items: center; justify-content: center;
  color: #FF8C00;
}
h1 {
  font-size: 28px;
  font-weight: 700;
  margin: 0;
  color: #333;
}

.toolbar {
  display: flex;
  gap: 12px;
  align-items: center;
}
.tool-btn {
  padding: 12px 24px;
  border-radius: 8px;
  border: none;
  background: #F9F9F9;
  color: #555;
  display: flex; align-items: center; gap: 8px;
  cursor: pointer;
  font-size: 14px;
  transition: 0.2s;
}
.tool-btn:hover { background: #eee; }
.tool-btn.active {
  background: #FF8C00;
  color: #fff;
  img {
    filter: invert(100%) sepia(0%) saturate(8%) hue-rotate(199deg) brightness(1003%) contrast(104%);
  }
}

.search-input {
  position: relative;
  width: 280px;
}
.search-input input {
  width: 100%;
  height: 40px;
  border-radius: 8px;
  border: none;
  background: #F5F5F5;
  padding-left: 40px;
  padding-right: 15px;
  font-size: 14px;
  outline: none;
}

.add-btn-circle {
  width: 40px; height: 40px;
  border-radius: 50%;
  border: 1px solid #ddd;
  background: #fff;
  display: flex; align-items: center; justify-content: center;
  cursor: pointer;
  color: #555;
}
.add-btn-circle:hover { background: #f9f9f9; }

/* --- SORT DROPDOWN --- */
.sort-wrapper { position: relative; }
.sort-dropdown {
  position: absolute;
  top: 50px;
  right: 0;
  width: 220px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.1);
  padding: 15px;
  z-index: 100;
}
.sort-section { margin-bottom: 15px; }
.sort-section:last-child { margin-bottom: 0; }
.sort-group-title {
  font-size: 12px; color: #999; margin-bottom: 8px;
}
.radio-item {
  display: flex; align-items: center; gap: 10px;
  font-size: 14px; color: #333;
  margin-bottom: 8px;
  cursor: pointer;
}
.radio-item input { display: none; }
.radio-custom {
  width: 18px; height: 18px;
  border: 2px solid #ddd;
  border-radius: 50%;
  position: relative;
}
.radio-item input:checked + .radio-custom {
  border-color: #FF8C00;
}
.radio-item input:checked + .radio-custom::after {
  content: '';
  position: absolute;
  top: 3px; left: 3px;
  width: 8px; height: 8px;
  background: #FF8C00;
  border-radius: 50%;
}

/* --- PRODUCT LIST --- */
.products-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.product-row {
  display: flex;
  align-items: center;
  background: #F9F9F9;
  padding: 16px 24px;
  border-radius: 20px;
  gap: 20px;
  cursor: pointer;
  transition: background 0.2s;
}

.product-row:hover { 
  background: #F2F2F2; 
}

/* Изображение */
.row-image {
  flex-shrink: 0;
}

.img-placeholder {
  width: 80px;
  height: 80px;
  background-size: cover;
  background-position: center;
  background-color: #fff;
  border-radius: 12px;
}

/* Контейнер контента */
.row-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

/* Название, рейтинг, цена */
.row-main {
  display: flex;
  align-items: center;
  gap: 20px;
  justify-content: space-between;
}

.p-name {
  margin: 0;
  font-size: 24px;
  font-weight: 600;
  color: #333;
}

.row-rating {
  display: flex;
  align-items: center;
  gap: 8px;
}

.stars-wrapper {
  display: flex;
}

.rating-value {
  color: #FF7A00;
  font-weight: 600;
  font-size: 24px;
}

.row-price {
  font-size: 18px;
  font-weight: 700;
  color: #333;
}

.rat-pri {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 100px;
}

/* Категория, подкатегория */
.row-meta {
  display: flex;
  gap: 4px;
  font-size: 18px;
  color: #7a7a7a;
  flex-direction: column;
}

.p-cat, .p-sub {
  color: #888;
  font-size: 18px;
}

/* ===== ЕДИНЫЕ СТИЛИ ЧЕКБОКСОВ ===== */
.checkbox-row {
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  font-size: 14px;
  color: #444;
  user-select: none;
}

.checkbox-row input[type="checkbox"] {
  position: absolute;
  opacity: 0;
  width: 0;
  height: 0;
  pointer-events: none;
}

.check-box {
  position: relative;
  width: 20px;
  height: 20px;
  border: 2px solid #ddd;
  border-radius: 5px;
  background: #fff;
  flex-shrink: 0;
  transition: all 0.2s ease;
}

/* Галочка */
.check-box::after {
  content: '';
  position: absolute;
  left: 6px;
  top: 2px;
  width: 5px;
  height: 10px;
  border: solid white;
  border-width: 0 2.5px 2.5px 0;
  transform: rotate(45deg);
  opacity: 0;
  transition: opacity 0.2s ease;
}

/* Выбранное состояние */
.checkbox-row input[type="checkbox"]:checked + .check-box {
  background: #FF7A00;
  border-color: #FF7A00;
}

.checkbox-row input[type="checkbox"]:checked + .check-box::after {
  opacity: 1;
}

/* Частично выбранное состояние (indeterminate) */
.checkbox-row input[type="checkbox"]:indeterminate + .check-box {
  background: #FF7A00;
  border-color: #FF7A00;
}

.checkbox-row input[type="checkbox"]:indeterminate + .check-box::after {
  left: 4px;
  top: 7px;
  width: 10px;
  height: 2.5px;
  border: none;
  background: white;
  transform: none;
  opacity: 1;
}

/* Ховер на чекбокс */
.checkbox-row:hover .check-box {
  border-color: #FF7A00;
}

/* ===== FILTERS SIDEBAR ===== */
.filters-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.4);
  z-index: 90000;
  display: flex;
  justify-content: flex-end;
}

.filters-sidebar {
  width: 400px;
  max-width: 100%;
  background: #fff;
  height: 100%;
  display: flex;
  flex-direction: column;
  animation: slideIn 0.3s ease;
}

@keyframes slideIn {
  from { transform: translateX(100%); }
  to { transform: translateX(0); }
}

.sidebar-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 30px;
  border-bottom: 1px solid #eee;
}

.sidebar-header h2 {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
}

.close-btn {
  background: none;
  border: none;
  font-size: 28px;
  cursor: pointer;
  color: #999;
  line-height: 1;
  padding: 0;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 6px;
  transition: all 0.2s;
}

.close-btn:hover {
  background: #f5f5f5;
  color: #333;
}

.sidebar-content {
  flex: 1;
  overflow-y: auto;
  padding: 20px 30px;
}

.filter-block {
  margin-bottom: 28px;
}

.f-label {
  display: block;
  font-weight: 600;
  margin-bottom: 14px;
  font-size: 15px;
  color: #333;
}

.range-inputs {
  display: flex;
  gap: 15px;
}

.input-wrap {
  flex: 1;
  display: flex;
  align-items: center;
  background: #F5F5F5;
  border-radius: 10px;
  padding: 10px 14px;
  transition: all 0.2s;
}

.input-wrap:focus-within {
  background: #fff;
  box-shadow: 0 0 0 2px #FF7A00;
}

.input-wrap span {
  color: #999;
  font-size: 14px;
  margin-right: 8px;
}

.input-wrap input {
  border: none;
  background: transparent;
  width: 100%;
  outline: none;
  font-weight: 500;
  font-size: 14px;
}

/* Поиск категорий */
.cat-search {
  width: 100%;
  padding: 12px 16px;
  border: none;
  background: #F5F5F5;
  border-radius: 10px;
  font-size: 14px;
  margin-bottom: 16px;
  outline: none;
  transition: all 0.2s;
}

.cat-search:focus {
  background: #fff;
  box-shadow: 0 0 0 2px #FF7A00;
}

.cat-search::placeholder {
  color: #aaa;
}

/* Дерево категорий */
.cat-tree {
  max-height: 320px;
  overflow-y: auto;
}

.cat-tree::-webkit-scrollbar {
  width: 6px;
}

.cat-tree::-webkit-scrollbar-track {
  background: #f5f5f5;
  border-radius: 3px;
}

.cat-tree::-webkit-scrollbar-thumb {
  background: #ddd;
  border-radius: 3px;
}

.cat-tree::-webkit-scrollbar-thumb:hover {
  background: #ccc;
}

.cat-group {
  margin-bottom: 6px;
}

.cat-parent {
  display: flex;
  align-items: center;
  padding: 12px 14px;
  background: #F8F8F8;
  border-radius: 10px;
  cursor: pointer;
  gap: 10px;
  transition: background 0.2s;
}

.cat-parent:hover {
  background: #f0f0f0;
}

.cat-name {
  flex: 1;
  font-weight: 500;
  font-size: 14px;
  color: #333;
}

.chevron {
  font-size: 18px;
  color: #999;
  transition: transform 0.25s ease;
  font-weight: bold;
}

.chevron.expanded {
  transform: rotate(90deg);
}

.cat-children {
  padding-left: 44px;
  padding-top: 8px;
  overflow: hidden;
}

.cat-child {
  padding: 6px 0;
}

.cat-child .checkbox-row span:last-child {
  font-size: 14px;
  color: #555;
}

/* Анимация раскрытия категорий */
.slide-enter-active,
.slide-leave-active {
  transition: all 0.25s ease;
}

.slide-enter-from,
.slide-leave-to {
  opacity: 0;
  max-height: 0;
  padding-top: 0;
}

.slide-enter-to,
.slide-leave-from {
  opacity: 1;
  max-height: 500px;
}

/* Пустой результат поиска */
.no-results {
  padding: 24px;
  text-align: center;
  color: #999;
  font-size: 14px;
}

/* Кнопка "Посмотреть все" */
.show-all-link {
  margin-top: 14px;
  color: #FF7A00;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
  display: inline-block;
}

.show-all-link:hover {
  text-decoration: underline;
}

/* Футер сайдбара */
.sidebar-footer {
  display: flex;
  gap: 14px;
  padding: 20px 30px;
  border-top: 1px solid #eee;
  background: #fff;
}

.reset-btn {
  flex: 1;
  height: 48px;
  border-radius: 10px;
  border: none;
  background: #F5F5F5;
  font-size: 15px;
  font-weight: 600;
  color: #555;
  cursor: pointer;
  transition: all 0.2s;
}

.reset-btn:hover {
  background: #eee;
}

.apply-btn {
  flex: 1;
  height: 48px;
  border-radius: 10px;
  border: none;
  background: #FF7A00;
  color: white;
  font-size: 15px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.apply-btn:hover {
  background: #e66e00;
}

.apply-btn:active {
  transform: scale(0.98);
}

/* ===== MODAL ===== */
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  z-index: 100000;
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-content {
  background: white;
  padding: 30px;
  border-radius: 16px;
  width: 500px;
  max-width: 90vw;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 10px 40px rgba(0,0,0,0.2);
}

.modal-body label {
  display: block;
  margin-top: 15px;
  margin-bottom: 5px;
  font-weight: 500;
  font-size: 13px;
  color: #333;
}

.modal-body input,
.modal-body select,
.modal-body textarea {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 8px;
  outline: none;
  font-size: 14px;
  transition: border-color 0.2s;
}

.modal-body input:focus,
.modal-body select:focus,
.modal-body textarea:focus {
  border-color: #FF7A00;
}

.modal-footer {
  margin-top: 25px;
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.save-btn {
  background: #FF8C00;
  color: white;
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.2s;
}

.save-btn:hover {
  background: #e67e00;
}

.cancel-btn {
  background: #eee;
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.2s;
}

.cancel-btn:hover {
  background: #ddd;
}

/* ===== SCROLLBAR GENERAL ===== */
.sidebar-content::-webkit-scrollbar {
  width: 6px;
}

.sidebar-content::-webkit-scrollbar-track {
  background: #f5f5f5;
}

.sidebar-content::-webkit-scrollbar-thumb {
  background: #ddd;
  border-radius: 3px;
}

.sidebar-content::-webkit-scrollbar-thumb:hover {
  background: #ccc;
}

.active-filters { display: flex; gap: 8px; margin-bottom: 15px; flex-wrap: wrap; }
.card-filters-row { margin-bottom: 12px; display: flex; flex-wrap: wrap; gap: 8px; }
.filter-tag { display: inline-flex; align-items: center; gap: 6px; background: #FFF7ED; color: #FF7A00; padding: 6px 12px; border-radius: 20px; font-size: 13px; font-weight: 500; }
.tag-remove { background: none; border: none; color: #FF7A00; font-size: 16px; cursor: pointer; }

</style>