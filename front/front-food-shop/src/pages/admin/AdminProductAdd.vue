<template>
  <AdminLayout>
    <div class="product-page">
      
      <div class="page-header">
        <h2>Добавление товара</h2>
      </div>

      <div class="main-content">
        
        <div class="left-col">
           <div class="upload-zone">
              <span class="upload-text">Загрузите обложку товара</span>
              <button class="btn-circle-orange" @click="openFileDialog">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M12 5V19M5 12H19" stroke="#FF7A00" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
              </button>
           </div>
        </div>

        <div class="right-col">
          
          <div class="card info-card">
          
            <div class="form-group">
                <label>Название и граммовка:</label>
                <div class="row-inputs">
                    <input 
                      v-model="form.title" 
                      type="text" 
                      placeholder="Введите название товара" 
                      class="input-main"
                    >
                    
                    <input 
                        v-model="form.weight"
                        @input="validateNumber(form, 'weight', $event)" 
                        placeholder="0" 
                        class="input-short"
                    >
                    <div class="control-box dropdown-wrapper">
                        <button
                          class="sort-btn-styled"
                          :class="{ 'is-active': showUnitMenu }"
                          @click.stop="showUnitMenu = !showUnitMenu"
                        >
                          <span>{{ unitDict[form.unit] || form.unit }}</span>
                          <img v-if="!showUnitMenu" src="../../assets/arrow-down.svg" />
                          <img v-else src="../../assets/arrow-down.svg" style="transform: rotate(180deg); filter: invert(100%) sepia(0%) saturate(7500%) hue-rotate(74deg) brightness(1210%) contrast(113%);" />
                        </button>
                        <div v-if="showUnitMenu" class="custom-dropdown-menu sort-menu-design form-dd">
                            <template v-for="(val, key, index) in unitDict" :key="key">
                                <div
                                  class="sort-item"
                                  @click="form.unit = key; showUnitMenu = false"
                                >
                                  <div class="radio-indicator" :class="{ selected: form.unit === key }"></div>
                                  <span class="sort-text">{{ val }}</span>
                                </div>
                                <div v-if="index < Object.keys(unitDict).length - 1" class="dd-divider"></div>
                            </template>
                            
                        </div>
                    </div>
                    
                </div>
            </div>

            <div class="form-group">
              <label>Артикул и бренд:</label>
              <div class="row-inputs">
                <input 
                  v-model="form.brand" 
                  placeholder="Бренд"
                  style="width: 50%;"
                >
                <input 
                    v-model="form.article" 
                    placeholder="Артикул"
                    style="width: 50%;"
                    @input="validateNumber(form, 'article', $event)"
                >
              </div>
            </div>

            <div class="form-group">
                <label>Категория и подкатегория:</label>
    
    <!-- Обертка с click-outside -->
    <div class="custom-select-container" v-click-outside="() => isCategorySelectorOpen = false">
  
  <!-- Заголовок -->
  <div 
    class="select-header" 
    :class="{ 'is-open': isCategorySelectorOpen }" 
    @click="isCategorySelectorOpen = !isCategorySelectorOpen"
  >
    <span>{{ selectedCategoryLabel }}</span>
    <img src="../../assets/arrow-down.svg" />
  </div>
  
  <!-- Тело селектора -->
  <div v-if="isCategorySelectorOpen" class="select-body">
    
    <!-- Поиск -->
    <div class="select-search-wrapper">
      <img src="../../assets/search-normal.svg" alt="search" class="search-icon-sm" />
      <input 
        type="text" 
        v-model="categorySearchQuery" 
        placeholder="Введите название категории..." 
        class="select-search-input"
      />
    </div>
    
    <!-- Список категорий -->
    <div class="select-list">
      
      <template v-for="cat in filteredCategories" :key="cat.categoryName">
        
        <!-- Родительская категория (папка) -->
        <div 
          class="select-option parent-option" 
          @click.stop="toggleCategory(cat.categoryName)"
        >
          <span class="option-text font-bold">{{ cat.categoryName }}</span>
          <img 
            src="../../assets/arrow-down.svg" 
            class="cat-arrow" 
            :class="{ 'is-expanded': expandedCategory === cat.categoryName }"
          />
        </div>
        
        <!-- Подкатегории -->
        <template v-if="expandedCategory === cat.categoryName">
          <label 
            v-for="sub in cat.subs" 
            :key="sub" 
            class="select-option sub-option"
            @click.stop="selectSubcategory(sub)"
          >
            <input 
              type="radio" 
              name="subcategory" 
              :value="sub" 
              v-model="form.subcategory"
            />
            <div 
              class="radio-indicator" 
              :class="{ selected: form.subcategory === sub }"
            ></div>
            <span class="option-text">{{ sub.categoryName }}</span>
          </label>
        </template>
        
      </template>
      
      <!-- Пусто -->
      <div v-if="filteredCategories.length === 0" class="empty-select">
        Ничего не найдено
      </div>
      
    </div>
  </div>
  
  <!-- Кнопка "Посмотреть все" -->
    <div 
      class="show-all-link" 
      v-if="hasMoreCategories"
      @click="showAllCategories = true"
    >
      Посмотреть все ({{ categories.length - visibleCategoriesCount }} ещё)
      <img src="../../assets/arrow-down.svg" />
    </div>
    <div 
      class="show-all-link" 
      v-else-if="showAllCategories && !categorySearch"
      @click="showAllCategories = false"
      style="margin-top: 8px;"
    >
      Свернуть
      <img src="../../assets/arrow-down.svg" />
    </div>
  </div>
            </div>

            <div class="form-group price-group">
                <div class="row-inputs wrap-on-mobile">
                    
                    <div class="input-group">
                      <span class="label-in">Цена:</span>
                      <input 
                        v-model="form.price" 
                        placeholder="0"
                        @input="validateNumber(form, 'price', $event)" 
                        class="input-price"
                      >
                    </div>
                    
                    <div class="input-group">
                      <span class="label-in">За:</span>
                      <div class="control-box dropdown-wrapper">
                        <button
                          class="sort-btn-styled"
                          :class="{ 'is-active': showPriceUnitMenu }"
                          @click.stop="showPriceUnitMenu = !showPriceUnitMenu"
                        >
                          <span>{{ priceUnitsDict[form.priceUnit] }}</span>
                          <img v-if="!showPriceUnitMenu" src="../../assets/arrow-down.svg" />
                          <img v-else src="../../assets/arrow-down.svg" style="transform: rotate(180deg); filter: invert(100%) sepia(0%) saturate(7500%) hue-rotate(74deg) brightness(1210%) contrast(113%);" />
                        </button>
                        <div v-if="showPriceUnitMenu" class="custom-dropdown-menu sort-menu-design form-dd">
                            <template v-for="(val, key, index) in priceUnitsDict" :key="key">
                                <div
                                  class="sort-item"
                                  @click="form.priceUnit = key; showPriceUnitMenu = false"
                                >
                                  <div class="radio-indicator" :class="{ selected: form.priceUnit === key }"></div>
                                  <span class="sort-text">{{ val }}</span>
                                </div>
                                <div v-if="index < Object.keys(priceUnitsDict).length - 1" class="dd-divider"></div>
                            </template>
                        </div>
                      </div>
                    </div>
                    
                    <div class="input-group ml-auto">
                      <span class="label-in">В наличии</span>
                      <div class="counter">
                          <button @click="form.stock > 0 ? form.stock-- : null">-</button>
                          <input 
                            v-model="form.stock" 
                            @input="validateNumber(form, 'stock', $event)" 
                            class="input-short-counter"
                          >
                          <button @click="form.stock++">+</button>
                      </div>
                    </div>
                    
                    <div class="control-box dropdown-wrapper">
                        <button
                          class="sort-btn-styled"
                          :class="{ 'is-active': showStockUnitMenu }"
                          @click.stop="showStockUnitMenu = !showStockUnitMenu"
                        >
                          <span>{{ stockUnitsDict[form.stockUnit] }}</span>
                          <img v-if="!showStockUnitMenu" src="../../assets/arrow-down.svg" />
                          <img v-else src="../../assets/arrow-down.svg" style="transform: rotate(180deg); filter: invert(100%) sepia(0%) saturate(7500%) hue-rotate(74deg) brightness(1210%) contrast(113%);" />
                        </button>
                        <div v-if="showStockUnitMenu" class="custom-dropdown-menu sort-menu-design form-dd right-align">
                            <template v-for="(val, key, index) in stockUnitsDict" :key="key">
                                <div
                                  class="sort-item"
                                  @click="form.stockUnit = key; showStockUnitMenu = false"
                                >
                                  <div class="radio-indicator" :class="{ selected: form.stockUnit === key }"></div>
                                  <span class="sort-text">{{ val }}</span>
                                </div>
                                <div v-if="index < Object.keys(stockUnitsDict).length - 1" class="dd-divider"></div>
                            </template>
                        </div>
                    </div>

                </div>
            </div>

          </div> 
        </div> </div> <div class="full-width-section">
        <h2>О товаре</h2>
        
        <div class="card content-card">
            
            <div class="form-group">
              <h3 class="section-title">Пищевая ценность <span class="subtitle">на 100г продукта</span></h3>
              <div class="nutrition-grid">
                
                <div class="nut-item">
                    <span class="nut-label">Жиры:</span>
                    <input 
                        v-model="form.nutrition.fats"
                        @input="validateNumber(form.nutrition, 'fats', $event)"
                        class="nut-input"
                        placeholder="0"
                    >
                    <span class="nut-unit">г</span>
                </div>

                <div class="nut-item">
                    <span class="nut-label">Углеводы:</span>
                    <input 
                        v-model="form.nutrition.carbs"
                        @input="validateNumber(form.nutrition, 'carbs', $event)"
                        class="nut-input"
                        placeholder="0"
                    >
                    <span class="nut-unit">г</span>
                </div>

                <div class="nut-item">
                    <span class="nut-label">Белки:</span>
                    <input 
                        v-model="form.nutrition.protein"
                        @input="validateNumber(form.nutrition, 'protein', $event)"
                        class="nut-input"
                        placeholder="0"
                    >
                    <span class="nut-unit">г</span>
                </div>

                <div class="nut-item wide">
                    <span class="nut-label">Энергетическая ценность:</span>
                    <input 
                        v-model="form.nutrition.energy"
                        @input="validateNumber(form.nutrition, 'energy', $event)"
                        class="nut-input"
                        placeholder="0"
                    >
                    <span class="nut-unit">кКал /</span>
                    
                    <input 
                        v-model="form.nutrition.joules" 
                        @input="validateNumber(form.nutrition, 'joules', $event)" 
                        class="nut-input"
                        placeholder="0"
                    >
                    <span class="nut-unit">кДж</span>
                </div>
              </div>
            </div>

            <div class="form-group">
              <h3 class="section-title">Состав</h3>
              <textarea 
                  v-model="form.description" 
                  rows="1"
                  placeholder="Введите состав товара"
                  class="simple-textarea"
              ></textarea>
            </div>

            <div class="form-group">
              <div class="header-with-action">
                  <h3 class="section-title">Характеристики</h3>
                  <button class="add-circle-btn" @click="addAttr">
                    <img src="../../assets/add-circle.svg" />
                  </button>
              </div>
              
              <div class="attrs-list">
                  <div v-for="(attr, i) in form.attributes" :key="i" class="attr-row-edit">
                      <input v-model="attr.name" placeholder="Наименование" class="input-bg">
                      <input v-model="attr.value" placeholder="Значение" class="input-bg">
                      <button class="icon-btn danger" @click="removeAttr(i)">
                        <img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/>
                      </button>
                  </div>
              </div>
            </div>

        </div>
      </div>

      <div class="footer-actions">
          <button class="btn-cancel" @click="cancel">Отменить</button>
          <button class="btn-add" @click="addProduct">Добавить</button>
      </div>

    </div>
    <input type="file" ref="fileInputRef" @change="handleFileChange" style="display: none;" />
  </AdminLayout>
</template>

<script setup>
import { ref, computed, onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router'; // Для навигации после добавления
import AdminLayout from './AdminLayout.vue';
// Используем объектный экспорт из вашего api.js
import { adminProductApi, categoryApi } from '@/services/api.js';

const router = useRouter();

// --- DATA ---

const isLoading = ref(false);
const categoriesRaw = ref([]); // Сюда придут данные из API

// Состояние UI
const showCatDropdown = ref(false);
const expandedCategory = ref(null);
const isCategorySelectorOpen = ref(false);
const categorySearchQuery = ref('');
const showUnitMenu = ref(false);
const showPriceUnitMenu = ref(false);
const showStockUnitMenu = ref(false);

// Словари (оставляем для UI)
const unitDict = { 'kg': 'Килограмм', 'g': 'Грамм', 'mg': 'Миллиграмм', 'l': 'Литр', 'ml': 'Миллилитр' };
const priceUnitsDict = { 'st': 'Шт', 'kg': 'Кг', 'g': 'Гр', 'mg': 'Мг', 'l': 'Л', 'ml': 'Мл' };
const stockUnitsDict = { 'kg': 'Килограмм', 'st': 'Штук', 'g': 'Грамм', 'mg': 'Миллиграмм', 'l': 'Литр', 'ml': 'Миллилитр' };

// Логика формы
const form = ref({
  title: '',
  weight: '',
  unit: 'kg',
  price: '',
  priceUnit: 'st',
  stock: 0,
  stockUnit: 'kg',
  categoryId: null,    // Добавили поле для ID категории
  subcategoryName: '', // Только для отображения в UI
  nutrition: {
    fats: '',
    carbs: '',
    protein: '',
    energy: '',
    joules: ''
  },
  brand: '',
  article: '',
  description: '',
  attributes: [
    { name: '', value: '' }
  ],
  images: []
});

const fileInputRef = ref(null);
// --- API FETCHING ---

const loadCategories = async () => {
  try {
    const data = await categoryApi.get();
    categoriesRaw.value = data; // Сохраняем плоский список
  } catch (error) {
    console.error("Ошибка загрузки категорий:", error.message);
  }
};

onMounted(() => {
  loadCategories();
});

// --- COMPUTED ---

// Преобразуем плоский список категорий в дерево для верстки
const categoriesTree = computed(() => {
  const mainCats = categoriesRaw.value.filter(c => !c.parentCategoryId);
  return mainCats.map(parent => ({
    categoryId: parent.categoryId,
    categoryName: parent.categoryName,
    subs: categoriesRaw.value.filter(c => c.parentCategoryId === parent.categoryId) // Теперь тут объекты {id, name...}
  }));
});

const filteredCategories = computed(() => {
  const query = categorySearchQuery.value.toLowerCase().trim();
  if (!query) return categoriesTree.value;
  
  return categoriesTree.value
    .map(cat => ({
      ...cat,
      subs: cat.subs.filter(sub => 
        sub.categoryName.toLowerCase().includes(query) || 
        cat.categoryName.toLowerCase().includes(query)
      )
    }))
    .filter(cat => cat.subs.length > 0 || cat.categoryName.toLowerCase().includes(query));
});

const selectedCategoryLabel = computed(() => {
  if (!form.value.categoryId) return 'Выбрать категорию и подкатегорию из списка';
  // Ищем категорию по ID
  const sub = categoriesRaw.value.find(c => c.categoryId === form.value.categoryId);
  if (!sub) return 'Выбрать категорию...';
  const parent = categoriesRaw.value.find(c => c.categoryId === sub.parentCategoryId);
  return parent ? `${parent.categoryName} - ${sub.categoryName}` : sub.categoryName;
});

// --- METHODS ---

const toggleCategory = (catName) => {
  expandedCategory.value = expandedCategory.value === catName ? null : catName;
};

const selectSubcategory = (sub) => {
  form.value.categoryId = sub.categoryId; // Сохраняем ID для сервера
  form.value.subcategoryName = sub.categoryName; 
  isCategorySelectorOpen.value = false; // Закрываем дропдаун
};

const validateNumber = (obj, key, event) => {
  let val = event.target.value.replace(/[^0-9.]/g, '');
  if ((val.match(/\./g) || []).length > 1) val = val.slice(0, -1);
  obj[key] = val;
};

const addAttr = () => form.value.attributes.push({ name: '', value: '' });
const removeAttr = (index) => form.value.attributes.splice(index, 1);

const addProduct = async () => {
  if (!form.value.title || !form.value.categoryId) {
    return alert('Заполните название и выберите категорию');
  }

  isLoading.value = true;
  try {
    const characteristics = {};

    characteristics.grammage = {
      weight: form.value.weight,
      unit: form.value.unit
    }

    characteristics.priceUnit = form.value.priceUnit;
    characteristics.stockUnit = form.value.stockUnit;

    if (form.value.brand) characteristics.brand = form.value.brand;
    if (form.value.article) characteristics.article = form.value.article;


    if (form.value.nutrition) {
      characteristics.nutrition = JSON.stringify(form.value.nutrition);
    };

    form.value.attributes.forEach(attr => {
      if (attr.name && attr.value) characteristics[attr.name] = attr.value;
    });

    await adminProductApi.create(
      form.value.title,
      null,
      form.value.description,
      parseFloat(form.value.price) || 0,
      parseInt(form.value.stock) || 0,
      form.value.categoryId,
      characteristics
    );

    router.push('/admin/products');
  } catch (error) {
    console.error(error);
  } finally {
    isLoading.value = false;
  }
};

const cancel = () => {
  if(confirm('Отменить добавление?')) {
    router.back();
  }
};

const vClickOutside = {
  mounted(el, binding) {
    el.clickOutsideEvent = function(event) {
      if (!(el === event.target || el.contains(event.target))) {
        binding.value(event, el);
      }
    };
    document.body.addEventListener('click', el.clickOutsideEvent);
  },
  unmounted(el) {
    document.body.removeEventListener('click', el.clickOutsideEvent);
  }
};

const openFileDialog = () => {
  fileInputRef.value.click();
};

const handleFileChange = async (e) => {
  const file = e.target.files[0];
  if (!file) return;
  
  const formData = new FormData();
  formData.append('file', file);
  
  try {
    // const response = await adminProductApi.addImage(form.id, file);
    form.value.images.push(response.imageId);
  } catch (error) {
    console.error(error);
  }
};

const removeImage = (index) => {
  form.value.images.splice(index, 1);
};
</script>
<style scoped lang="scss">
@import './admin.css';

/* --- Variables --- */
$primary-orange: #FF7F00;
$text-dark: #333;
$border-color: #E0E0E0;
$bg-light: #f9f9f9;

/* --- Layout --- */
.product-page {
  font-family: 'Inter', sans-serif; /* Или ваш шрифт */
  margin: 0 auto;
  color: $text-dark;
  padding-bottom: 80px; /* место для футера */
}

.page-header h2 {
    font-size: 24px;
    font-weight: 700;
    margin-bottom: 24px;
}

.main-content {
  display: flex;
  gap: 32px;
  margin-bottom: 30px;
}

.left-col { 
    flex: 0 0 35%; 
    display: flex;
    flex-direction: column;
}

.right-col { 
    flex: 1; 
    background-color: $bg-light; 
    padding: 32px; 
    border-radius: 24px; 
}

/* --- Upload Zone (Left Col) --- */
.upload-zone {
    flex: 1;
    background-color: $bg-light;
    border-radius: 24px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    min-height: 400px;
    gap: 16px;
    text-align: center;
}

.upload-text {
    color: #9CA3AF;
    font-size: 16px;
    font-weight: 500;
}

.btn-circle-orange {
    width: 48px;
    height: 48px;
    border-radius: 50%;
    border: 1px solid #FF7A00;
    background: transparent;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    transition: all 0.2s;
}

.btn-circle-orange:hover {
    background: rgba(255, 122, 0, 0.1);
}

/* --- Form Elements --- */
.form-group {
  margin-bottom: 24px;
  
  label {
    display: block;
    font-weight: 600;
    margin-bottom: 12px;
    font-size: 16px;
  }
}

.row-inputs {
  display: flex;
  gap: 12px;
  align-items: center;
}

.input-group {
    display: flex;
    align-items: center;
    gap: 12px;
}

input, select, textarea {
  border: none;
  outline: none;
  padding: 12px 16px;
  border-radius: 12px;
  font-size: 15px;
  background: #FFF;
}

.input-main { flex: 1; }
.input-short { width: 80px; text-align: center; }

/* Custom Select Styling */
.custom-select-wrapper {
    position: relative;
    min-width: 140px;
}

.custom-select-wrapper.full-width {
    width: 100%;
}

.select-trigger {
    background: #FFF;
    height: 44px;
    border-radius: 12px;
    padding: 0 16px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    cursor: pointer;
    font-size: 15px;
    color: #111827;
}

.grey-text { color: #9CA3AF; }

.select-trigger .arrow {
    font-size: 10px;
    color: #6B7280;
}

.custom-dropdown-menu {
    position: absolute;
    top: calc(100% + 4px);
    left: 0;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0,0,0,0.1);
    z-index: 10;
    min-width: 100%;
    overflow: hidden;
    padding: 8px 0;
}

.sort-item {
    padding: 10px 16px;
    display: flex;
    align-items: center;
    gap: 10px;
    cursor: pointer;
}

.sort-item:hover { background-color: #F3F4F6; }

/* Category specific styles */
.dd-search-container { padding: 0 12px 8px 12px; }
.dd-search-input { width: 100%; background: #f9f9f9; border: 1px solid #E5E7EB; box-sizing: border-box; }
.dd-divider { height: 1px; background: #E5E7EB; margin-bottom: 4px; }
.parent-cat { justify-content: space-between; font-weight: 600; }
.sub-cat-list { background: #f9f9f9; }
.sub-item { padding-left: 32px; font-size: 14px; }

/* Counter */
.counter {
  background-color: #fff;
  display: flex;
  padding: 0 12px;
  height: 44px;
  border-radius: 12px;
  align-items: center;
  gap: 8px;
}

.counter button {
  font-size: 20px;
  color: #FF7A00;
  background: none;
  border: none;
  cursor: pointer;
  padding: 0 8px;
}

.input-short-counter {
    width: 30px;
    padding: 0;
    text-align: center;
    font-weight: 600;
}

.ml-auto { margin-left: auto; }

/* --- Full Width Section --- */
.full-width-section {
    margin-top: 40px;
}

.section-title {
    font-size: 18px;
    font-weight: 600;
    margin-bottom: 16px;
    display: flex;
    align-items: center;
    gap: 8px;
}

.subtitle {
    font-size: 14px;
    color: #6B7280;
    font-weight: 400;
}

.content-card {
    background: $bg-light;
    padding: 32px;
    border-radius: 24px;
}

/* Nutrition */
.nutrition-grid {
    display: flex;
    gap: 20px;
    flex-wrap: wrap;
}

.nut-item {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 15px;
}

.nut-label { color: #374151; }

.nut-input {
    width: 60px;
    text-align: center;
    background: #FFF;
    border-radius: 12px;
}

.nut-unit { color: #6B7280; }

/* Textarea */
.simple-textarea {
    width: 100%;
    resize: none;
    border: none;
    border-radius: 12px;
    padding: 16px;
    font-family: "Libre Franklin", ui-sans-serif, system-ui;
    box-sizing: border-box;
}

/* Attributes */
.header-with-action {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 12px;
    margin-bottom: 16px;
}

.header-with-action h3 { margin: 0; }


.add-circle-btn { display: flex; align-items: center; justify-content: center; border-radius: 16px; background-color: #fff; border: none; cursor: pointer; color: #6B7280; transition: background 0.2s; padding: 12px 18px; }

.attrs-list {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.attr-row-edit {
    display: grid;
    grid-template-columns: 1fr 1fr 40px;
    gap: 16px;
    align-items: center;
}

.input-bg {
    background: #FFF;
}

.icon-btn.danger {
    background: none;
    height: 44px;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    border: none;
    cursor: pointer;
}

.icon-btn.danger:hover { color: #EF4444; background: rgba(239, 68, 68, 0.1); }

/* --- Footer --- */
.footer-actions {
    display: flex;
    justify-content: flex-end;
    gap: 16px;
    margin-top: 40px;
}

.btn-cancel {
    padding: 12px 32px;
    border-radius: 12px;
    border: none;
    background: #F3F4F6;
    color: #374151;
    font-weight: 600;
    cursor: pointer;
}

.btn-add {
    padding: 12px 32px;
    border-radius: 12px;
    border: none;
    background: $primary-orange;
    color: white;
    font-weight: 600;
    cursor: pointer;
}

/* Utilities */
.radio-indicator {
    width: 16px;
    height: 16px;
    border-radius: 50%;
    border: 2px solid #E5E7EB;
}
.radio-indicator.selected {
    border-color: $primary-orange;
    background: $primary-orange;
    box-shadow: inset 0 0 0 3px #fff;
}

.sort-btn-styled { background-color: #fff; }
/* Custom Select */
.custom-select-container { border: 1px solid #E5E7EB; border-radius: 12px; overflow: hidden; }
.select-header { background-color: #F6F6F6; padding: 14px 16px; cursor: pointer; display: flex; justify-content: space-between; align-items: center; font-size: 14px; color: #6B7280; }
.select-header.is-open { background-color: #FF7A00; color: white; border-radius: 12px 12px 0 0; }
.select-header.is-open img { transform: rotate(180deg); filter: invert(100%) sepia(0%) saturate(7500%) hue-rotate(74deg) brightness(1210%) contrast(113%); }
.select-body { border: 1px solid #E5E7EB; border-top: none; border-radius: 0 0 12px 12px; padding: 16px; background: #f9f9f9; }
.select-search-wrapper { position: relative; margin-bottom: 12px; }
.search-icon-sm { position: absolute; left: 0; top: 2px; }
.select-search-input { background: #f9f9f9; width: 100%; border: none; border-bottom: 1px solid #E5E7EB; padding: 6px 36px; font-size: 14px; outline: none; }
.select-list { max-height: 200px; overflow-y: auto; display: flex; flex-direction: column; gap: 12px; margin-bottom: 12px; }
.select-option { display: flex; align-items: center; gap: 10px; cursor: pointer; font-size: 14px; color: #4B5563; border-bottom: 1px solid #dddddd; padding: 10px;}
.select-option input { display: none; }
.radio-custom { width: 18px; height: 18px; border-radius: 50%; background: #E5E7EB; position: relative; transition: all 0.2s; }
.select-option input:checked + .radio-custom { background: #FF7A00; border: 4px solid #FFD8B3; }
.show-more-link { color: #FF7A00; font-size: 13px; cursor: pointer; text-align: center; margin-top: 10px; }
.select-option.sub-option { display: flex; align-items: center; ;}
</style>