<template>
  <AdminLayout>
    <template #default>
      <div class="admin-wrapper">
        
        <div class="chart-card">
          <div class="chart-header">
            <div>
              <!-- Теперь заголовок корректно отображает выбранный период -->
              <h3>Сводка: {{ periodLabels[selectedPeriod] }}</h3>
              <p class="chart-subtitle">Статистика по: {{ currentMetricTitle }}</p>
            </div>
            <div class="date-select-wrapper">
              <!-- Диапазон дат теперь динамический -->
              <span class="period-range-label">{{ currentPeriodDatesRange }}</span>
              <select v-model="selectedPeriod" class="custom-select">
                <option v-for="(label, key) in periodLabels" :key="key" :value="key">{{ label }}</option>
              </select>
            </div>
          </div>

          <div class="chart-tabs">
             <div class="chart-tab" :class="{ active: activeTab === 'views' }" @click="activeTab = 'views'">
                <div class="tab-label">Всего просмотров</div><div class="tab-val">45.2 тыс.</div>
              </div>
               <div class="chart-tab" :class="{ active: activeTab === 'visitors' }" @click="activeTab = 'visitors'">
                <div class="tab-label">Посетителей</div><div class="tab-val">12.5 тыс.</div>
              </div>
               <div class="chart-tab" :class="{ active: activeTab === 'orders' }" @click="activeTab = 'orders'">
                <div class="tab-label">Заказов</div><div class="tab-val">843</div>
              </div>
               <div class="chart-tab" :class="{ active: activeTab === 'revenue' }" @click="activeTab = 'revenue'">
                <div class="tab-label">Выручка</div><div class="tab-val">2.1 млн ₽</div>
              </div>
          </div>

          <div class="chart-area">
             <!-- Добавил отступы (padding) через viewBox для подписей осей -->
             <svg class="chart-svg" viewBox="-50 -10 1050 240" preserveAspectRatio="none">
              <defs>
                <linearGradient :id="'gradient-' + activeTab" x1="0" x2="0" y1="0" y2="1">
                  <stop offset="0%" :stop-color="chartColor" stop-opacity="0.2"/>
                  <stop offset="100%" :stop-color="chartColor" stop-opacity="0"/>
                </linearGradient>
              </defs>

              <!-- СЕТКА И ОСЬ Y -->
              <g class="grid-lines">
                <!-- Рисуем 5 линий сетки и подписи к ним -->
                <g v-for="(val, i) in yAxisValues" :key="i">
                  <!-- Линия -->
                  <line 
                    x1="0" 
                    :y1="i * 50" 
                    x2="1000" 
                    :y2="i * 50" 
                    stroke="#eee" 
                    stroke-width="1" 
                    stroke-dasharray="4"
                  />
                  <!-- Текст оси Y (слева) -->
                  <text 
                    x="-10" 
                    :y="i * 50 + 4" 
                    text-anchor="end" 
                    class="axis-text"
                  >
                    {{ val }}
                  </text>
                </g>
              </g>

              <!-- График -->
              <path :d="currentChartData.path" fill="none" :stroke="chartColor" stroke-width="3" vector-effect="non-scaling-stroke" class="chart-line-anim"/>
              <path :d="currentChartData.fill" :fill="'url(#gradient-' + activeTab + ')'" stroke="none" class="chart-fill-anim"/>

              <!-- ОСЬ X -->
              <g class="x-axis">
                <text 
                  v-for="(label, i) in xAxisLabels" 
                  :key="i"
                  :x="(i * (1000 / (xAxisLabels.length - 1)))" 
                  y="225" 
                  text-anchor="middle" 
                  class="axis-text"
                >
                  {{ label }}
                </text>
              </g>

            </svg>
          </div>
        </div>

        <div class="promotions-section">
          <div class="section-header">
            <h2>Акции и баннеры</h2>
            <button class="add-btn" @click="openModal('create')">+ Добавить акцию</button>
          </div>

          <div class="promo-grid">
            <div class="promo-card" v-for="promo in promotions" :key="promo.id">
              <div class="promo-image" :style="{ backgroundColor: promo.color }">
                <span class="promo-img-text">{{ promo.valueType === 'percent' ? '%' : '₽' }}</span>
                
                <div class="status-dropdown-wrapper">
                   <div class="promo-status" :class="promo.status" @click.stop="toggleStatusMenu(promo.id)">
                    {{ getStatusLabel(promo.status) }} ▾
                   </div>
                   <div v-if="activeStatusMenuId === promo.id" class="status-menu">
                     <div @click="changeStatus(promo.id, 'active')">Действует</div>
                     <div @click="changeStatus(promo.id, 'archived')">Архив</div>
                     <div class="danger" @click="askDeletePromo(promo.id)">Удалить</div>
                   </div>
                </div>
              </div>
              
              <div class="promo-content">
                <h3 class="promo-title">
                  {{ promo.title }} 
                  <span class="title-percent">
                    — {{ promo.value }} {{ promo.valueType === 'percent' ? '%' : '₽' }}
                  </span>
                </h3>
                <p class="promo-desc">{{ promo.description }}</p>
                
                <div class="promo-details-badges">
                  <span class="badge-type">{{ promo.benefitType === 'discount' ? 'Скидка' : 'Бонус' }}</span>
                  <span class="badge-target">{{ getTargetLabel(promo.targetType) }}</span>
                </div>

                <div class="promo-meta">
                  <span>📅 {{ formatDate(promo.dateStart) }} — {{ formatDate(promo.dateEnd) }}</span>
                </div>
              </div>

              <div class="promo-actions">
                <button class="action-btn edit" @click="openModal('edit', promo)">Редактировать</button>
              </div>
            </div>
          </div>
        </div>

        <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
          <div class="modal-content large-modal">
            <h3>{{ modalMode === 'create' ? 'Новая акция' : 'Редактирование' }}</h3>
            
            <div class="form-row">
              <div class="form-group main-col">
                <label>Заголовок</label>
                <input v-model="form.title" placeholder="Например: Осенний ценопад" :class="{ 'input-error': errors.title }"/>
                <span v-if="errors.title" class="input-error-text">{{ errors.title }}</span>
              </div>
              
              <div class="form-group small-col">
                <label>Размер выгоды</label>
                <div class="value-input-group">
                  <input 
                    type="number" 
                    v-model="form.value" 
                    min="1"
                    placeholder="1"
                    :class="{ 'input-error': errors.value }"
                  />
                  <div class="value-type-switch">
                    <button 
                      :class="{ active: form.valueType === 'percent' }" 
                      @click="form.valueType = 'percent'"
                      title="Проценты"
                    >%</button>
                    <button 
                      :class="{ active: form.valueType === 'fixed' }" 
                      @click="form.valueType = 'fixed'"
                      title="Валюта"
                    >₽</button>
                  </div>
                </div>
                <span v-if="errors.value" class="input-error-text">{{ errors.value }}</span>
              </div>
            </div>
            
            <div class="form-group">
              <label>Описание</label>
              <textarea v-model="form.description" placeholder="Условия акции..." rows="2" :class="{ 'input-error': errors.description }"></textarea>
              <span v-if="errors.description" class="input-error-text">{{ errors.description }}</span>
            </div>

            <div class="form-row">
              <div class="form-group half">
                <label>Тип акции</label>
                <select v-model="form.benefitType">
                  <option value="discount">Скидка</option>
                  <option value="bonus">Бонусы</option>
                </select>
              </div>
              
              <div class="form-group half">
                <label>Область действия</label>
                <select v-model="form.targetType" @change="resetTargets">
                  <option value="all">Весь каталог</option>
                  <option value="category">Категории</option>
                  <option value="subcategory">Подкатегории</option>
                  <option value="product">Товары</option>
                </select>
              </div>
            </div>

            <div class="form-group" v-if="form.targetType !== 'all'">
              <label>Поиск и выбор {{ getTargetLabel(form.targetType).toLowerCase() }}</label>
              
              <div class="search-select-wrapper">
                <input 
                  type="text" 
                  v-model="searchQuery" 
                  class="search-input"
                  :placeholder="`Начните вводить название...`"
                  @focus="isSearchFocused = true"
                  @blur="blurSearch"
                />
                <div class="search-dropdown" v-if="isSearchFocused && filteredTargets.length > 0">
                  <div 
                    v-for="item in filteredTargets" 
                    :key="item.id" 
                    class="search-item"
                    @mousedown.prevent="selectTarget(item)"
                  >
                    {{ item.name }}
                  </div>
                </div>
                <div class="search-dropdown" v-if="isSearchFocused && filteredTargets.length === 0 && searchQuery">
                  <div class="search-item no-result">Ничего не найдено</div>
                </div>
              </div>

              <div class="selected-tags-area" v-if="form.selectedItems.length > 0">
                <div class="tag-item" v-for="item in form.selectedItems" :key="item.id">
                  <span>{{ item.name }}</span>
                  <button class="remove-tag" @click="removeTarget(item.id)">×</button>
                </div>
                <div class="clear-all" @click="form.selectedItems = []">Очистить всё</div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group half">
                <label>Дата начала</label>
                <input type="date" v-model="form.dateStart" :class="{ 'input-error': errors.dateStart }" />
                <span v-if="errors.dateStart" class="input-error-text">{{ errors.dateStart }}</span>
              </div>
              <div class="form-group half">
                <label>Дата окончания</label>
                <input type="date" v-model="form.dateEnd" />
              </div>
            </div>

            <div class="modal-actions">
              <button class="primary-btn" @click="savePromotion">
                {{ modalMode === 'create' ? 'Создать' : 'Сохранить' }}
              </button>
              <button class="secondary-btn" @click="closeModal">Отмена</button>
            </div>
          </div>
        </div>
        <!-- НОВОЕ: Модальное окно подтверждения удаления -->
        <div v-if="showDeleteModal" class="modal-overlay" @click.self="showDeleteModal = false">
          <div class="modal-content confirm-modal">
            <div class="confirm-icon">
              <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#ff4d4f" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path>
                <line x1="12" y1="9" x2="12" y2="13"></line>
                <line x1="12" y1="17" x2="12.01" y2="17"></line>
              </svg>
            </div>
            <h3>Удалить акцию?</h3>
            <p class="confirm-text">Это действие нельзя будет отменить. Акция будет удалена из списка и перестанет действовать.</p>
            <div class="modal-actions">
              <button class="primary-btn danger-btn" @click="confirmDelete">Удалить</button>
              <button class="secondary-btn" @click="showDeleteModal = false">Отмена</button>
            </div>
          </div>
        </div>
      </div>
    </template>
  </AdminLayout>
</template>

<script setup>
import { ref, computed, reactive, watch } from 'vue';
import AdminLayout from './AdminLayout.vue';
import './admin.css';

const errors = reactive({
  title: '',
  description: '',
  value: '',
  dateStart: ''
});

const periodLabels = { 
  day: 'День', 
  week: 'Неделя', 
  month: 'Месяц', 
  halfYear: 'Полгода', 
  year: 'Год',
  year3: '3 года',
  all: 'Все время'
};

const selectedPeriod = ref('month');
const activeTab = ref('views');
const currentMetricTitle = computed(() => ({ views: 'Просмотрам', visitors: 'Посетителям', orders: 'Заказам', revenue: 'Выручке' }[activeTab.value]));

// Цвета графика
const chartColor = computed(() => {
  switch(activeTab.value) { case 'views': return '#FF7A00'; case 'visitors': return '#2196F3'; case 'orders': return '#4CAF50'; case 'revenue': return '#9C27B0'; default: return '#333'; }
});

// Моковые данные для графика (SVG Paths)
const chartDataMock = {
  views: { path: "M0,120 Q200,80 400,100 T600,90 T800,40 T1000,70", fill: "M0,120 Q200,80 400,100 T600,90 T800,40 T1000,70 V200 H0 Z" },
  visitors: { path: "M0,150 Q200,130 400,140 T600,100 T800,110 T1000,90", fill: "M0,150 Q200,130 400,140 T600,100 T800,110 T1000,90 V200 H0 Z" },
  orders: { path: "M0,180 Q200,160 400,140 T600,100 T800,80 T1000,20", fill: "M0,180 Q200,160 400,140 T600,100 T800,80 T1000,20 V200 H0 Z" },
  revenue: { path: "M0,100 Q200,120 400,150 T600,100 T800,50 T1000,80", fill: "M0,100 Q200,120 400,150 T600,100 T800,50 T1000,80 V200 H0 Z" }
};
const currentChartData = computed(() => chartDataMock[activeTab.value]);

// === ИСПРАВЛЕНИЕ: Логика диапазонов дат ===
const currentPeriodDatesRange = computed(() => {
   const now = new Date();
   const start = new Date();
   const f = new Intl.DateTimeFormat('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' });

   if(selectedPeriod.value === 'day') return f.format(now);
   
   if(selectedPeriod.value === 'week') start.setDate(now.getDate() - 7);
   else if(selectedPeriod.value === 'month') start.setMonth(now.getMonth() - 1);
   else if(selectedPeriod.value === 'halfYear') start.setMonth(now.getMonth() - 6);
   else if(selectedPeriod.value === 'year') start.setFullYear(now.getFullYear() - 1);
   else if(selectedPeriod.value === 'year3') start.setFullYear(now.getFullYear() - 3);
   else if(selectedPeriod.value === 'all') start.setFullYear(now.getFullYear() - 10);
   
   return `${f.format(start)} — ${f.format(now)}`;
});

// === ИСПРАВЛЕНИЕ: Подписи оси X (динамические) ===
const xAxisLabels = computed(() => {
  switch(selectedPeriod.value) {
    case 'day': return ['00:00', '06:00', '12:00', '18:00', '23:59'];
    case 'week': return ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'];
    case 'month': return ['1', '8', '15', '22', '29'];
    case 'halfYear': return ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн'];
    case 'year': return ['Янв', 'Апр', 'Июл', 'Окт'];
    default: return ['Нач.', 'Сер.', 'Кон.'];
  }
});

// === ИСПРАВЛЕНИЕ: Подписи оси Y (динамические) ===
// Значения идут сверху вниз (от max к 0)
const yAxisValues = computed(() => {
  switch(activeTab.value) {
    case 'views': return ['60k', '45k', '30k', '15k', '0'];
    case 'visitors': return ['20k', '15k', '10k', '5k', '0'];
    case 'orders': return ['1000', '750', '500', '250', '0'];
    case 'revenue': return ['3M', '2.2M', '1.5M', '0.7M', '0'];
    default: return ['100', '75', '50', '25', '0'];
  }
});

const database = {
  categories: Array.from({length: 20}, (_, i) => ({ id: i, name: `Категория ${i+1}` })),
  subcategories: Array.from({length: 50}, (_, i) => ({ id: i, name: `Подкатегория ${i+1}` })),
  products: Array.from({length: 100}, (_, i) => ({ id: i, name: `Товар №${i+1} (Артикул ${1000+i})` }))
};
database.categories[0].name = "Электроника"; database.categories[1].name = "Одежда";
database.products[0].name = "iPhone 15 Pro Max 256GB"; database.products[1].name = "Samsung Galaxy S24";

const promotions = ref([
  { 
    id: 1, 
    title: 'Распродажа телефонов', 
    description: 'Скидка на флагманы', 
    benefitType: 'discount',
    value: 15,
    valueType: 'percent',
    dateStart: '2025-12-01',
    dateEnd: '2025-12-30', 
    status: 'active',
    color: '#FFE0B2',
    targetType: 'product',
    selectedItems: [{ id: 0, name: 'iPhone 15 Pro Max 256GB' }, { id: 1, name: 'Samsung Galaxy S24' }]
  },
  { 
    id: 2, 
    title: 'Бонус за любой заказ', 
    description: 'Дарим 500 рублей на счет', 
    benefitType: 'bonus',
    value: 500,
    valueType: 'fixed',
    dateStart: '2026-01-01',
    dateEnd: '2026-02-15', 
    status: 'active',
    color: '#C8E6C9',
    targetType: 'all',
    selectedItems: []
  }
]);

const showModal = ref(false);
const modalMode = ref('create');
const editingId = ref(null);

const showDeleteModal = ref(false);
const promoIdToDelete = ref(null);

const form = reactive({
  title: '',
  description: '',
  benefitType: 'discount',
  value: 10,
  valueType: 'percent',
  dateStart: '',
  dateEnd: '',
  targetType: 'all',
  selectedItems: []
});

const searchQuery = ref('');
const isSearchFocused = ref(false);

watch(() => form.value, (newVal) => {
  if (form.valueType === 'percent') {
    if (newVal > 100) form.value = 100;
  }
  if (newVal < 0) form.value = 0;
});

watch(() => form.valueType, (newType) => {
  if (newType === 'percent' && form.value > 100) {
    form.value = 100;
  }
});

const filteredTargets = computed(() => {
  if (!searchQuery.value) return [];
  
  let source = [];
  if (form.targetType === 'category') source = database.categories;
  else if (form.targetType === 'subcategory') source = database.subcategories;
  else if (form.targetType === 'product') source = database.products;

  const query = searchQuery.value.toLowerCase();
  
  return source.filter(item => 
    item.name.toLowerCase().includes(query) && 
    !form.selectedItems.find(selected => selected.id === item.id)
  ).slice(0, 10);
});

function selectTarget(item) {
  form.selectedItems.push(item);
  searchQuery.value = '';
}

function removeTarget(id) {
  form.selectedItems = form.selectedItems.filter(item => item.id !== id);
}

function resetTargets() {
  form.selectedItems = [];
  searchQuery.value = '';
}

function blurSearch() {
  setTimeout(() => { isSearchFocused.value = false; }, 200);
}

function openModal(mode, promoData = null) {
  modalMode.value = mode;
  showModal.value = true;
  searchQuery.value = '';

  errors.title = '';
  errors.description = '';
  errors.value = '';
  errors.dateStart = '';

  if (mode === 'edit' && promoData) {
    editingId.value = promoData.id;
    form.title = promoData.title;
    form.description = promoData.description;
    form.benefitType = promoData.benefitType;
    form.value = promoData.value;
    form.valueType = promoData.valueType;
    form.dateStart = promoData.dateStart;
    form.dateEnd = promoData.dateEnd;
    form.targetType = promoData.targetType || 'all';
    form.selectedItems = JSON.parse(JSON.stringify(promoData.selectedItems || []));
  } else {
    editingId.value = null;
    form.title = '';
    form.description = '';
    form.benefitType = 'discount';
    form.value = 10;
    form.valueType = 'percent';
    form.dateStart = new Date().toISOString().split('T')[0];
    form.dateEnd = '';
    form.targetType = 'all';
    form.selectedItems = [];
  }
}

function closeModal() {
  showModal.value = false;
  activeStatusMenuId.value = null;
}

function savePromotion() {

  errors.title = '';
  errors.description = '';
  errors.value = '';
  errors.dateStart = '';

  let isValid = true;

  if (!form.title || !form.title.trim()) {
    errors.title = 'Пожалуйста, добавьте название акции.';
    isValid = false;
  }
  
  if (!form.description || !form.description.trim()) {
    errors.description = 'Пожалуйста, добавьте описание акции.';
    isValid = false;
  }

  if (form.value <= 0) {
    errors.value = 'Значение должно быть больше нуля.';
    isValid = false;
  }

  if (!form.dateStart) {
    errors.dateStart = 'Пожалуйста, добавьте дату начала акции.';
    isValid = false;
  } else {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const [year, month, day] = form.dateStart.split('-').map(Number);
    const startDate = new Date(year, month - 1, day);
    if (startDate < today) {
      errors.dateStart = 'Дата начала акции не может быть в прошлом.';
      isValid = false;
    }
  }

  if (!isValid) return;

  const dataToSave = {
    title: form.title,
    description: form.description,
    benefitType: form.benefitType,
    value: form.value,
    valueType: form.valueType,
    dateStart: form.dateStart,
    dateEnd: form.dateEnd || 'Бессрочно',
    color: form.valueType === 'fixed' ? '#C8E6C9' : '#FFE0B2', 
    targetType: form.targetType,
    selectedItems: JSON.parse(JSON.stringify(form.selectedItems))
  };

  if (modalMode.value === 'create') {
    promotions.value.push({ ...dataToSave, id: Date.now(), status: 'active' });
  } else {
    const index = promotions.value.findIndex(p => p.id === editingId.value);
    if (index !== -1) {
      promotions.value[index] = { ...promotions.value[index], ...dataToSave };
    }
  }
  closeModal();
}

const activeStatusMenuId = ref(null);
function toggleStatusMenu(id) { activeStatusMenuId.value = activeStatusMenuId.value === id ? null : id; }
function changeStatus(id, s) { const p = promotions.value.find(x => x.id === id); if(p) p.status = s; activeStatusMenuId.value = null; }
function askDeletePromo(id) {
  promoIdToDelete.value = id;
  showDeleteModal.value = true;
  activeStatusMenuId.value = null;
}

function confirmDelete() {
  if (promoIdToDelete.value !== null) {
    promotions.value = promotions.value.filter(p => p.id !== promoIdToDelete.value);
  }
  showDeleteModal.value = false;
  promoIdToDelete.value = null;
}
function getStatusLabel(s) { return s === 'active' ? 'Активна' : (s === 'archived' ? 'Архив' : s); }
function formatDate(s) { if(!s) return '...'; const p = s.split('-'); return p.length<3?s:`${p[2]}.${p[1]}.${p[0]}`; }
function getTargetLabel(type) {
  const labels = { all: 'Весь каталог', category: 'Категории', subcategory: 'Подкатегории', product: 'Товары' };
  if (type === 'product') return 'Товара';
  return labels[type] || type;
}
</script>

<style scoped>
.admin-wrapper { padding: 20px 40px; min-height: 90vh; font-family: 'Inter', sans-serif; color: #333; }
.custom-select { padding: 8px 12px; border: 1px solid #ddd; border-radius: 6px; background: #f9f9f9; font-weight: 600; color: #555; cursor: pointer; outline: none; }
.chart-card { background: #fff; border-radius: 12px; padding: 25px; box-shadow: 0 4px 20px rgba(0,0,0,0.03); margin-bottom: 40px; border: 1px solid #eee; }
.chart-header { display: flex; justify-content: space-between; margin-bottom: 25px; }
.chart-header h3 { margin: 0; font-size: 20px; font-weight: 600; }
.chart-subtitle { margin: 5px 0 0; color: #888; font-size: 14px; }
.chart-tabs { display: flex; border-bottom: 1px solid #eee; }
.chart-tab { flex: 1; padding: 15px; border-right: 1px solid #eee; cursor: pointer; transition: all 0.2s; position: relative; }
.chart-tab:last-child { border-right: none; }
.chart-tab:hover { background: #fdfdfd; }
.chart-tab.active { background: #fffcf8; }
.chart-tab.active::after { content: ''; position: absolute; top: -1px; left: 0; width: 100%; height: 3px; background: #FF7A00; }
.tab-label { font-size: 13px; color: #999; text-transform: uppercase; margin-bottom: 8px; }
.tab-val { font-size: 24px; font-weight: 700; color: #333; display: flex; align-items: center; gap: 5px; }
.chart-area { position: relative; height: 280px; margin: 20px 0; border-left: 1px solid #eee; border-bottom: 1px solid #eee; }
.chart-svg { width: 100%; height: 100%; overflow: visible; }
.chart-line-anim, .chart-fill-anim { transition: d 0.5s ease, stroke 0.3s ease, fill 0.3s ease; }
/* Стили для текста осей */
.axis-text { font-size: 11px; fill: #999; font-family: sans-serif; }

.date-select-wrapper { display: flex; flex-direction: column; align-items: flex-end; justify-content: center; gap: 6px; }
.period-range-label { font-size: 13px; color: #888; font-weight: 500; letter-spacing: -0.01em; }

.section-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
.section-header h2 { margin: 0; font-size: 22px; }
.add-btn { background: #FF7A00; color: white; border: none; padding: 10px 20px; border-radius: 8px; cursor: pointer; font-weight: 600; }
.add-btn:hover { background: #e06600; }
.promo-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 20px; }
.promo-card { background: #fff; border-radius: 12px; overflow: visible; box-shadow: 0 2px 10px rgba(0,0,0,0.05); display: flex; flex-direction: column; transition: transform 0.2s; position: relative; }
.promo-card:hover { transform: translateY(-3px); z-index: 5; }
.promo-image { height: 140px; position: relative; display: flex; justify-content: center; align-items: center; border-radius: 12px 12px 0 0; }
.promo-img-text { font-size: 40px; color: rgba(0,0,0,0.1); font-weight: 900; }
.status-dropdown-wrapper { position: absolute; top: 10px; right: 10px; }
.promo-status { padding: 4px 12px; border-radius: 20px; font-size: 11px; font-weight: bold; text-transform: uppercase; cursor: pointer; user-select: none; background: #fff; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }
.promo-status.active { color: #2E7D32; border: 1px solid #C8E6C9; }
.promo-status.archived { color: #546E7A; border: 1px solid #CFD8DC; }
.status-menu { position: absolute; top: 25px; right: 0; background: white; border-radius: 8px; box-shadow: 0 5px 15px rgba(0,0,0,0.15); min-width: 120px; overflow: hidden; z-index: 10; border: 1px solid #eee; }
.status-menu div { padding: 8px 12px; font-size: 13px; cursor: pointer; transition: background 0.1s; }
.status-menu div:hover { background: #f5f5f5; }
.status-menu div.danger { color: #ff4d4f; }

.promo-content { padding: 15px; flex-grow: 1; }
.promo-title { margin: 0 0 8px 0; font-size: 16px; font-weight: 600; display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap;}
.title-percent { color: #FF7A00; font-weight: 700; font-size: 16px; white-space: nowrap; }
.promo-desc { margin: 0 0 15px 0; font-size: 13px; color: #666; line-height: 1.4; }
.promo-details-badges { display: flex; gap: 8px; margin-bottom: 12px; flex-wrap: wrap; }
.badge-type { background: #f0f0f0; padding: 2px 8px; border-radius: 4px; font-size: 11px; color: #555; }
.badge-target { background: #E3F2FD; color: #1565C0; padding: 2px 8px; border-radius: 4px; font-size: 11px; font-weight: 600; }
.promo-meta { font-size: 12px; color: #999; }
.promo-actions { border-top: 1px solid #f0f0f0; padding: 10px 15px; }
.action-btn.edit { color: #589BF2; background: none; border: none; cursor: pointer; font-size: 13px; font-weight: 500; }

.modal-overlay { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.4); z-index: 1000; display: flex; justify-content: center; align-items: center; }
.modal-content { background: white; padding: 30px; border-radius: 12px; width: 450px; display: flex; flex-direction: column; max-height: 90vh; }
.modal-content.large-modal { width: 600px; }
.modal-content h3 { margin-top: 0; }

.form-group { margin-bottom: 15px; }
.form-row { display: flex; gap: 15px; }
.form-group.half { flex: 1; }
.form-group.main-col { flex: 2; }
.form-group.small-col { flex: 1; }
.form-group label { display: block; margin-bottom: 5px; font-size: 13px; font-weight: 600; color: #444; }
.form-group input, .form-group textarea, .form-group select { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px; box-sizing: border-box; font-size: 14px; }
.form-group select { background: white; }

.value-input-group { display: flex; gap: 0; }
.value-input-group input { border-top-right-radius: 0; border-bottom-right-radius: 0; border-right: none; }
.value-type-switch { display: flex; }
.value-type-switch button {
  border: 1px solid #ddd; background: #f9f9f9; cursor: pointer; padding: 0 12px; font-weight: 600; color: #777; transition: all 0.2s;
}
.value-type-switch button:first-child { border-right: none; }
.value-type-switch button:last-child { border-top-right-radius: 6px; border-bottom-right-radius: 6px; }
.value-type-switch button.active { background: #FF7A00; color: white; border-color: #FF7A00; }

.search-select-wrapper { position: relative; }
.search-input { padding-right: 30px; }
.search-icon { position: absolute; right: 10px; top: 50%; transform: translateY(-50%); color: #aaa; pointer-events: none; }
.search-dropdown {
  position: absolute; top: 100%; left: 0; width: 100%;
  background: white; border: 1px solid #ddd; border-radius: 6px;
  box-shadow: 0 5px 15px rgba(0,0,0,0.1);
  max-height: 200px; overflow-y: auto; z-index: 100;
  margin-top: 5px;
}
.search-item { padding: 10px 12px; font-size: 13px; cursor: pointer; border-bottom: 1px solid #f5f5f5; }
.search-item:last-child { border-bottom: none; }
.search-item:hover { background: #f0f7ff; color: #000; }
.search-item.no-result { color: #999; cursor: default; }

.selected-tags-area {
  display: flex; flex-wrap: wrap; gap: 8px; margin-top: 10px; 
  padding: 10px; background: #f8f9fa; border-radius: 8px; border: 1px solid #eee;
  max-height: 120px; overflow-y: auto;
}
.tag-item {
  background: white; border: 1px solid #ddd; border-radius: 4px;
  padding: 4px 8px; font-size: 12px; display: flex; align-items: center; gap: 6px;
  box-shadow: 0 1px 2px rgba(0,0,0,0.05);
}
.remove-tag {
  background: none; border: none; color: #999; font-size: 14px; font-weight: bold; cursor: pointer; padding: 0; line-height: 1;
}
.remove-tag:hover { color: #ff4d4f; }
.clear-all {
  font-size: 11px; color: #666; text-decoration: underline; cursor: pointer; align-self: center; margin-left: auto;
}
.clear-all:hover { color: #333; }

.modal-actions { display: flex; gap: 10px; margin-top: auto; padding-top: 20px; }
.primary-btn { flex: 1; background: #FF7A00; color: white; border: none; padding: 12px; border-radius: 6px; cursor: pointer; font-weight: 600; }
.primary-btn:hover { background: #e06600; }
.secondary-btn { flex: 1; background: #f0f0f0; color: #333; border: none; padding: 12px; border-radius: 6px; cursor: pointer; }
.secondary-btn:hover { background: #e0e0e0; }
.input-error {
  border-color: #ff4d4f !important;
  background-color: #fff1f0 !important; 
}

.input-error:focus {
  border-color: #ff7875 !important;
  box-shadow: 0 0 0 2px rgba(255, 77, 79, 0.2);
}

.input-error-text {
  display: block;
  font-size: 11px;
  color: #ff4d4f;
  margin-top: 4px;
  font-weight: 500;
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-3px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>