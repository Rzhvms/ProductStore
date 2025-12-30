<template>
  <AdminLayout>
    <template #default>
      <div class="admin-wrapper">
        
        <!-- === ХЛЕБНЫЕ КРОШКИ === -->
        <div class="breadcrumbs">
          <span 
            class="crumb-link" 
            :class="{ active: currentView === 'categories' }"
            @click="goHome"
          >
            Категории
          </span>
          <span v-if="selectedCategory" class="separator">›</span>
          <span 
            v-if="selectedCategory" 
            class="crumb-link"
            :class="{ active: currentView === 'subcategories' }"
            @click="goToCategory(selectedCategory)"
          >
            {{ selectedCategory.name }}
          </span>
          <span v-if="selectedSubcategory" class="separator">›</span>
          <span 
            v-if="selectedSubcategory" 
            class="crumb-link active"
          >
            {{ selectedSubcategory }}
          </span>
        </div>

        <!-- ================================================== -->
        <!-- VIEW 1: КАТАЛОГ (ГЛАВНАЯ) -->
        <!-- ================================================== -->
        <div v-if="currentView === 'categories'">
          
          <div class="view-header catalog-header">
            <div class="header-left-group">
              <img src="../../assets/folder.svg" class="folder-icon" />
              <h1 class="catalog-title">Каталог</h1>
            </div>

            <div class="header-right-group">
              <div class="control-box search-box-styled">
                <img src="../../assets/search-normal.svg" />
                <input type="text" placeholder="Поиск..." v-model="searchQuery">
              </div>

              <div class="control-box dropdown-wrapper">
                <button 
                  class="sort-btn-styled" 
                  :class="{ 'is-active': showSortDropdown || sortOption }"
                  @click.stop="showSortDropdown = !showSortDropdown"
                >
                  <img v-if="!(showSortDropdown || sortOption)" src="../../assets/drop down button.svg" />
                  <img v-else src="../../assets/drop down button(1).svg" />
                  <span>{{ sortOption ? sortLabel : 'Сортировка' }}</span>
                </button>
                
                <div v-if="showSortDropdown" class="custom-dropdown-menu sort-menu-design">
                  <div class="sort-group-label">По алфавиту</div>
                  <div class="sort-item" @click="setSortOption('name-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'name-asc' }"></div><span class="sort-text">От А до Я</span></div>
                  <div class="sort-item" @click="setSortOption('name-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'name-desc' }"></div><span class="sort-text">От Я до А</span></div>
                  <div class="dd-divider"></div>
                  <div class="sort-group-label">По количеству<br>подкатегорий</div>
                  <div class="sort-item" @click="setSortOption('count-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'count-desc' }"></div><span class="sort-text">Сначала больше</span></div>
                  <div class="sort-item" @click="setSortOption('count-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'count-asc' }"></div><span class="sort-text">Сначала меньше</span></div>
                </div>
              </div>

              <div class="control-box dropdown-wrapper">
                <button class="add-circle-btn" @click.stop="toggleAddMenu">
                  <img src="../../assets/add-circle.svg" />
                </button>
                <div v-if="showAddMenu" class="custom-dropdown-menu right-align">
                  <div class="dd-item" @click="openCategoryModal">Добавить категорию</div>
                  <div class="dd-item" @click="openSubSidebar">Добавить подкатегорию</div>
                </div>
              </div>
            </div>
          </div>

          <div v-if="sortOption" class="active-filters">
            <span class="filter-tag">{{ sortLabel }} <button class="tag-remove" @click="sortOption = ''">×</button></span>
          </div>

          <div class="list-container">
            <div v-for="cat in sortedCategories" :key="cat.id" class="category-item-wrapper">
              <div class="list-row" :class="{ 'row-expanded': expandedCategoryIds.has(cat.id) }">
                <div class="row-main clickable-area" @click="goToCategory(cat)">
                  <span class="item-text">{{ cat.name }}</span>
                </div>
                <div class="row-meta">
                  <span class="meta-badge" title="Количество подкатегорий">{{ cat.subCategories.length }}</span>
                  <button class="expand-arrow-btn" :class="{ 'is-expanded': expandedCategoryIds.has(cat.id) }" @click.stop="toggleExpand(cat.id)">
                    <img src="../../assets/arrow-down.svg" />
                  </button>
                </div>
              </div>
              <div v-if="expandedCategoryIds.has(cat.id)" class="inline-subcategories">
                <div v-for="(sub, idx) in cat.subCategories" :key="idx" class="inline-sub-row" @click="goToSubcategoryFromInline(cat, sub)">{{ sub }}<span class="inline-sub-count meta-badge">{{ getProductCount(sub) }}</span></div>
                <div v-if="cat.subCategories.length === 0" class="inline-empty">Нет подкатегорий</div>
              </div>
            </div>
            <div v-if="sortedCategories.length === 0" class="empty-text">Категории не найдены</div>
          </div>
        </div>

        <!-- ================================================== -->
        <!-- VIEW 2: ПОДКАТЕГОРИИ -->
        <!-- ================================================== -->
        <div v-else-if="currentView === 'subcategories' && selectedCategory">
          
          <div class="sub-view-card header-card">
            <div class="card-left">
              <img src="../../assets/folder-open.svg" alt="Logo" class="icon-orange" />
              <h1 class="card-title">{{ selectedCategory.name }}</h1>
            </div>
            <div class="header-card-actions">
              <button class="icon-btn" title="Редактировать" @click="openRenameDialog('category', selectedCategory)">
                <img src="../../assets/edit.svg" alt="edit" />
              </button>
              <button class="icon-btn danger" title="Удалить категорию" @click="promptDelete('category', selectedCategory)">
                <img src="../../assets/trash.svg" />
              </button>
            </div>
          </div>

          <div class="sub-view-card list-card">
            <div class="card-header-row">
              <div class="card-left">
                <img src="../../assets/folder-open.svg" alt="Logo" class="icon-orange" />
                <h2 class="card-subtitle">Список подкатегорий</h2>
              </div>
              <div class="card-actions">
                <div class="card-search">
                  <img src="../../assets/search-normal.svg" alt="search" class="search-icon" />
                  <input type="text" placeholder="Поиск..." v-model="searchQuery">
                </div>
                <button class="card-add-btn" @click="openAddSubDialog">
                  <img src="../../assets/add-circle.svg" />
                </button>
              </div>
            </div>

            <div class="card-list-body">
              <div v-for="(sub, index) in sortedSubcategories" :key="index" class="card-list-item" @click="goToSubcategory(sub)">
                
                <!-- Название слева -->
                <span class="sub-text">{{ sub }}</span>
                
                <!-- Мета данные (счетчик + удаление) справа -->
                <div class="row-meta">
                  <span class="meta-badge" title="Количество товаров">{{ getProductCount(sub) }}</span>
                  <button class="icon-btn danger" @click.stop="promptDelete('subcategory-item', sub)">
                    <img src="../../assets/trash.svg" />
                  </button>
                </div>
                
              </div>
              <div v-if="sortedSubcategories.length === 0" class="card-empty">Подкатегории не найдены</div>
            </div>

            <div class="card-footer" v-if="sortedSubcategories.length > 0">
              <span class="show-more-text">Посмотреть ещё</span>
            </div>
          </div>
        </div>

        <!-- ================================================== -->
        <!-- VIEW 3: ТОВАРЫ -->
        <!-- ================================================== -->
        <div v-else-if="currentView === 'products' && selectedSubcategory">
          
          <div class="sub-view-card header-card">
            <div class="card-left">
              <img src="../../assets/folder-open.svg" alt="Logo" class="icon-orange" />
              <h1 class="card-title">{{ selectedSubcategory }}</h1>
            </div>
            <div class="header-card-actions">
               <button class="icon-btn" title="Переименовать подкатегорию" @click="openRenameDialog('subcategory', selectedSubcategory)">
                <img src="../../assets/edit.svg" alt="edit" />
              </button>
              <button class="icon-btn danger" title="Удалить подкатегорию" @click="promptDelete('subcategory-page', selectedSubcategory)">
                <img src="../../assets/trash.svg" />
              </button>
            </div>
          </div>

          <div class="sub-view-card list-card">
            <div class="card-header-row">
              <div class="card-left">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" class="icon-orange">
                  <path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"></path><polyline points="3.27 6.96 12 12.01 20.73 6.96"></polyline><line x1="12" y1="22.08" x2="12" y2="12"></line>
                </svg>
                <h2 class="card-subtitle">Список товаров</h2>
              </div>
              <div class="card-actions">
                <div class="card-search">
                  <img src="../../assets/search-normal.svg" alt="search" class="search-icon" />
                  <input type="text" placeholder="Поиск товара..." v-model="searchQuery">
                </div>
                
                <div class="control-box dropdown-wrapper">
                  <button class="card-sort-btn" :class="{ 'is-active': showSortDropdown || sortOption }" @click.stop="showSortDropdown = !showSortDropdown">
                    <img v-if="!(showSortDropdown || sortOption)" src="../../assets/drop down button.svg" />
                    <img v-else src="../../assets/drop down button(1).svg" />
                    <span class="sort-btn-text">{{ sortOption ? sortLabel : 'Сортировка' }}</span>
                  </button>
                  <div v-if="showSortDropdown" class="custom-dropdown-menu sort-menu-design right-align">
                    <div class="sort-group-label">Цена</div>
                    <div class="sort-item" @click="setSortOption('price-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'price-asc' }"></div><span class="sort-text">Сначала дешевле</span></div>
                    <div class="sort-item" @click="setSortOption('price-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'price-desc' }"></div><span class="sort-text">Сначала дороже</span></div>
                    <div class="dd-divider"></div>
                    <div class="sort-group-label">Рейтинг</div>
                    <div class="sort-item" @click="setSortOption('rating-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'rating-desc' }"></div><span class="sort-text">Высокий рейтинг</span></div>
                     <div class="sort-item" @click="setSortOption('rating-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'rating-asc' }"></div><span class="sort-text">Низкий рейтинг</span></div>
                    <div class="dd-divider"></div>
                    <div class="sort-group-label">По алфавиту</div>
                    <div class="sort-item" @click="setSortOption('name-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'name-asc' }"></div><span class="sort-text">От А до Я</span></div>
                    <div class="sort-item" @click="setSortOption('name-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'name-desc' }"></div><span class="sort-text">От Я до А</span></div>
                  </div>
                </div>

                <button class="card-add-btn" @click="showAddProductDialog = true">
                  <img src="../../assets/add-circle.svg" />
                </button>
              </div>
            </div>
            
            <div v-if="sortOption" class="card-filters-row">
               <span class="filter-tag">{{ sortLabel }} <button class="tag-remove" @click="sortOption = ''">×</button></span>
            </div>

            <div class="card-list-body">
              <div v-for="prod in sortedProducts" :key="prod.id" class="card-list-item product-item">
                <div class="prod-col-name"><span class="sub-text item-main-text">{{ prod.name }}</span></div>
                <div class="prod-col-rating">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="#FFC107" stroke="none"><polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"></polygon></svg>
                  <span class="rating-val">{{ prod.rating }}</span>
                </div>
                <div class="prod-col-price"><span class="price-val">{{ formatPrice(prod.price) }} ₽</span></div>
                <div class="prod-col-action">
                  <button class="icon-btn danger" @click.stop="promptDelete('product', prod.id)">
                    <img src="../../assets/trash.svg" />
                  </button>
                </div>
              </div>
              <div v-if="sortedProducts.length === 0" class="card-empty">Товары не найдены</div>
            </div>
            
            <div class="card-footer" v-if="sortedProducts.length > 0">
              <span class="show-more-text">Показать все товары</span>
            </div>
          </div>
        </div>

        <!-- ================================================== -->
        <!-- МОДАЛКИ -->
        <!-- ================================================== -->

        <!-- 1. Добавить категорию (Global) -->
        <div v-if="showAddCategoryDialog" class="modal-overlay blur-overlay" @click.self="showAddCategoryDialog = false">
          <div class="modal-content centered-modal">
            <h3 class="modal-title-center">Новая категория</h3>
            <input v-model="newCategoryName" placeholder="Название категории" class="gray-input" @keyup.enter="addCategory"/>
            <div class="modal-actions-center">
              <button class="btn-gray" @click="showAddCategoryDialog = false">Отмена</button>
              <button class="btn-orange" @click="addCategory">Добавить</button>
            </div>
          </div>
        </div>

        <!-- 2. Добавить подкатегорию (ВНУТРИ категории, модальное окно) -->
        <div v-if="showAddSubDialog" class="modal-overlay blur-overlay" @click.self="showAddSubDialog = false">
          <div class="modal-content centered-modal">
             <h3 class="modal-title-center">Новая подкатегория</h3>
             <p class="modal-hint-center">В категорию: <b>{{ selectedCategory?.name }}</b></p>
             <input v-model="newSubName" placeholder="Название подкатегории" class="gray-input" @keyup.enter="addSubcategoryFromModal"/>
             <div class="modal-actions-center">
               <button class="btn-gray" @click="showAddSubDialog = false">Отмена</button>
               <button class="btn-orange" @click="addSubcategoryFromModal">Добавить</button>
             </div>
          </div>
        </div>

        <!-- 3. Переименование (Категория или Подкатегория) -->
        <div v-if="showRenameDialog" class="modal-overlay blur-overlay" @click.self="showRenameDialog = false">
          <div class="modal-content centered-modal">
            <h3 class="modal-title-center">Переименовать</h3>
            <input v-model="renameValue" class="gray-input" @keyup.enter="confirmRename"/>
            <div class="modal-actions-center">
              <button class="btn-gray" @click="showRenameDialog = false">Отмена</button>
              <button class="btn-orange" @click="confirmRename">Сохранить</button>
            </div>
          </div>
        </div>

        <!-- 4. Подтверждение удаления -->
        <div v-if="showDeleteConfirmDialog" class="modal-overlay blur-overlay" @click.self="showDeleteConfirmDialog = false">
          <div class="modal-content centered-modal">
            <div class="delete-icon-wrapper">
              <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="#EF4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path><line x1="12" y1="9" x2="12" y2="13"></line><line x1="12" y1="17" x2="12.01" y2="17"></line></svg>
            </div>
            <h3 class="modal-title-center">Удалить элемент?</h3>
            <p class="modal-text">Это действие необратимо. Содержимое (товары или подкатегории) также будет удалено.</p>
            <div class="modal-actions-center">
              <button class="btn-gray" @click="showDeleteConfirmDialog = false">Отмена</button>
              <button class="btn-red" @click="confirmDelete">Удалить</button>
            </div>
          </div>
        </div>

        <!-- 5. Сайдбар: Добавить подкатегорию (Global Add Menu) -->
        <div class="sidebar-overlay" :class="{ open: showAddSubSidebar }" @click.self="showAddSubSidebar = false">
          <div class="sidebar-panel">
            <h2 class="sidebar-title">Добавить подкатегорию</h2>
            <div class="sidebar-form">
              <input v-model="newSubName" placeholder="Название подкатегории" class="gray-input mb-4"/>
              <div class="custom-select-container">
                <div class="select-header" :class="{ 'is-open': isCategorySelectorOpen }" @click="isCategorySelectorOpen = !isCategorySelectorOpen">
                  <span>{{ selectedParentName }}</span>
                  <img src="../../assets/arrow-down.svg" />
                </div>
                <div v-if="isCategorySelectorOpen" class="select-body">
                  <div class="select-search-wrapper">
                    <img src="../../assets/search-normal.svg" alt="search" class="search-icon-sm" />
                    <input type="text" v-model="categorySearchQuery" placeholder="Введите название категории..." class="select-search-input"/>
                  </div>
                  <div class="select-list">
                    <label v-for="cat in filteredParentCategories" :key="cat.id" class="select-option">
                      <input type="radio" name="parentCategory" :value="cat.id" v-model="selectedParentId"/>
                      <span class="radio-custom"></span><span class="option-text">{{ cat.name }}</span>
                    </label>
                    <div v-if="filteredParentCategories.length === 0" class="empty-select">Ничего не найдено</div>
                  </div>
                </div>
              </div>
            </div>
            <div class="sidebar-actions">
              <button class="btn-gray" @click="showAddSubSidebar = false">Отмена</button>
              <button class="btn-orange" @click="addSubcategory">Добавить</button>
            </div>
          </div>
        </div>

        <!-- 6. Добавить товар -->
        <div v-if="showAddProductDialog" class="modal-overlay" @click.self="showAddProductDialog = false">
          <div class="modal-content">
            <h3>Добавить товар</h3>
            <p class="modal-hint">Выберите товар из каталога для добавления</p>
            <select v-model="selectedProductToAdd" class="full-input">
              <option value="">Выберите товар...</option>
              <option v-for="prod in availableProducts" :key="prod.id" :value="prod.id">
                {{ prod.name }} — {{ formatPrice(prod.price) }} ₽
              </option>
            </select>
            <div class="modal-actions">
              <button class="btn-secondary" @click="showAddProductDialog = false">Отмена</button>
              <button class="btn-primary" @click="addProductToSubcategory">Добавить</button>
            </div>
          </div>
        </div>

      </div>
    </template>
  </AdminLayout>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue';
import AdminLayout from './AdminLayout.vue';

// === DATA ===
const parentCategories = ref([
  { id: 1, name: 'Электроника', subCategories: ['Телефоны', 'Ноутбуки', 'Планшеты'] },
  { id: 2, name: 'Одежда', subCategories: ['Футболки', 'Штаны', 'Куртки'] },
  { id: 3, name: 'Книги', subCategories: ['Фантастика', 'Наука'] },
  { id: 4, name: 'Дом и сад', subCategories: ['Мебель', 'Декор', 'Инструменты', 'Освещение', 'Текстиль'] },
]);

const dummyProducts = ref([
  { id: 101, sub: 'Телефоны', name: 'iPhone 14 Pro', price: 99990, rating: 4.9 },
  { id: 102, sub: 'Телефоны', name: 'Samsung Galaxy S23', price: 79990, rating: 4.7 },
  { id: 103, sub: 'Ноутбуки', name: 'MacBook Air M2', price: 120000, rating: 4.8 },
  { id: 104, sub: 'Футболки', name: 'Белая футболка Basic', price: 1500, rating: 4.2 },
  { id: 105, sub: 'Телефоны', name: 'Xiaomi 13', price: 45990, rating: 4.5 },
]);

const availableProducts = ref([
  { id: 201, name: 'AirPods Pro', price: 24990, rating: 4.8 },
  { id: 202, name: 'Xiaomi Mi Band 8', price: 3990, rating: 4.6 },
]);

// === STATE ===
const currentView = ref('categories'); 
const selectedCategory = ref(null);
const selectedSubcategory = ref(null);
const searchQuery = ref('');
const expandedCategoryIds = ref(new Set());

// Modals State
const showAddMenu = ref(false);
const showAddCategoryDialog = ref(false);
const showAddSubDialog = ref(false); 
const showAddSubSidebar = ref(false); 
const showAddProductDialog = ref(false);
const showRenameDialog = ref(false);
const showDeleteConfirmDialog = ref(false);

// Operation State
const newCategoryName = ref('');
const newSubName = ref('');
const selectedProductToAdd = ref('');
const renameValue = ref('');
const renameTarget = ref(null); 
const deleteTarget = ref(null); 

// Sidebar Selector
const isCategorySelectorOpen = ref(false);
const categorySearchQuery = ref('');
const selectedParentId = ref(null);

// Sorting
const sortOption = ref('');
const showSortDropdown = ref(false);

// === COMPUTED ===
const filteredParentCategories = computed(() => {
  if (!categorySearchQuery.value) return parentCategories.value;
  return parentCategories.value.filter(c => c.name.toLowerCase().includes(categorySearchQuery.value.toLowerCase()));
});

const selectedParentName = computed(() => {
  const parent = parentCategories.value.find(c => c.id === selectedParentId.value);
  return parent ? parent.name : 'Выбрать категорию из списка';
});

const filteredCategories = computed(() => {
  if (!searchQuery.value) return parentCategories.value;
  const q = searchQuery.value.toLowerCase();
  return parentCategories.value.filter(c => c.name.toLowerCase().includes(q));
});

const sortedCategories = computed(() => {
  let items = [...filteredCategories.value];
  switch (sortOption.value) {
    case 'name-asc': items.sort((a, b) => a.name.localeCompare(b.name, 'ru')); break;
    case 'name-desc': items.sort((a, b) => b.name.localeCompare(a.name, 'ru')); break;
    case 'count-desc': items.sort((a, b) => b.subCategories.length - a.subCategories.length); break;
    case 'count-asc': items.sort((a, b) => a.subCategories.length - b.subCategories.length); break;
  }
  return items;
});

const filteredSubcategories = computed(() => {
  if (!selectedCategory.value) return [];
  const subs = selectedCategory.value.subCategories;
  if (!searchQuery.value) return subs;
  const q = searchQuery.value.toLowerCase();
  return subs.filter(s => s.toLowerCase().includes(q));
});

const sortedSubcategories = computed(() => {
  let items = [...filteredSubcategories.value];
  if (sortOption.value === 'name-asc') items.sort((a, b) => a.localeCompare(b, 'ru'));
  if (sortOption.value === 'name-desc') items.sort((a, b) => b.localeCompare(a, 'ru'));
  return items;
});

const filteredProducts = computed(() => {
  if (!selectedSubcategory.value) return [];
  let prods = dummyProducts.value.filter(p => p.sub === selectedSubcategory.value);
  if (searchQuery.value) prods = prods.filter(p => p.name.toLowerCase().includes(searchQuery.value.toLowerCase()));
  return prods;
});

const sortedProducts = computed(() => {
  let items = [...filteredProducts.value];
  if (sortOption.value === 'name-asc') items.sort((a, b) => a.name.localeCompare(b.name, 'ru'));
  if (sortOption.value === 'name-desc') items.sort((a, b) => b.name.localeCompare(a.name, 'ru'));
  if (sortOption.value === 'price-asc') items.sort((a, b) => a.price - b.price);
  if (sortOption.value === 'price-desc') items.sort((a, b) => b.price - a.price);
  if (sortOption.value === 'rating-desc') items.sort((a, b) => b.rating - a.rating);
  if (sortOption.value === 'rating-asc') items.sort((a, b) => a.rating - b.rating);
  return items;
});

const sortLabel = computed(() => {
  const labels = {
    'name-asc': 'От А до Я', 'name-desc': 'От Я до А',
    'count-desc': 'Сначала больше', 'count-asc': 'Сначала меньше',
    'price-asc': 'Сначала дешевле', 'price-desc': 'Сначала дороже',
    'rating-desc': 'Высокий рейтинг', 'rating-asc': 'Низкий рейтинг'
  };
  return labels[sortOption.value] || 'Сортировка';
});

// === ACTIONS ===

// --- Nav ---
function goHome() { currentView.value = 'categories'; selectedCategory.value = null; selectedSubcategory.value = null; searchQuery.value = ''; sortOption.value = ''; }
function goToCategory(cat) { selectedCategory.value = cat; selectedSubcategory.value = null; currentView.value = 'subcategories'; searchQuery.value = ''; sortOption.value = ''; }
function goToSubcategoryFromInline(cat, subName) { selectedCategory.value = cat; selectedSubcategory.value = subName; currentView.value = 'products'; searchQuery.value = ''; sortOption.value = ''; }
function goToSubcategory(subName) { selectedSubcategory.value = subName; currentView.value = 'products'; searchQuery.value = ''; sortOption.value = ''; }
function toggleExpand(id) { if (expandedCategoryIds.value.has(id)) expandedCategoryIds.value.delete(id); else expandedCategoryIds.value.add(id); }

// --- Add Category ---
function toggleAddMenu() { showAddMenu.value = !showAddMenu.value; }
function openCategoryModal() { showAddMenu.value = false; showAddCategoryDialog.value = true; newCategoryName.value = ''; }
function addCategory() {
  if (!newCategoryName.value.trim()) return;
  parentCategories.value.push({ id: Date.now(), name: newCategoryName.value.trim(), subCategories: [] });
  newCategoryName.value = ''; showAddCategoryDialog.value = false;
}

// --- Add Subcategory (Global Sidebar) ---
function openSubSidebar() { showAddMenu.value = false; showAddSubSidebar.value = true; newSubName.value = ''; selectedParentId.value = null; isCategorySelectorOpen.value = false; categorySearchQuery.value = ''; }
function addSubcategory() {
  if (!newSubName.value.trim() || !selectedParentId.value) return;
  const parent = parentCategories.value.find(c => c.id === selectedParentId.value);
  if (parent) parent.subCategories.push(newSubName.value.trim());
  newSubName.value = ''; selectedParentId.value = null; showAddSubSidebar.value = false;
}

// --- Add Subcategory (Context Modal) ---
function openAddSubDialog() {
  newSubName.value = '';
  showAddSubDialog.value = true;
}
function addSubcategoryFromModal() {
  if (!newSubName.value.trim() || !selectedCategory.value) return;
  selectedCategory.value.subCategories.push(newSubName.value.trim());
  newSubName.value = '';
  showAddSubDialog.value = false;
}

// --- Add Product ---
function addProductToSubcategory() {
  if (!selectedProductToAdd.value) return;
  const prod = availableProducts.value.find(p => p.id === Number(selectedProductToAdd.value));
  if (prod) {
    dummyProducts.value.push({ id: prod.id, sub: selectedSubcategory.value, name: prod.name, price: prod.price, rating: prod.rating });
    availableProducts.value = availableProducts.value.filter(p => p.id !== prod.id);
  }
  selectedProductToAdd.value = ''; showAddProductDialog.value = false;
}

// --- Rename Operations ---
function openRenameDialog(type, data) {
  renameTarget.value = { type, data };
  renameValue.value = type === 'category' ? data.name : data;
  showRenameDialog.value = true;
}

function confirmRename() {
  const newVal = renameValue.value.trim();
  if (!newVal || !renameTarget.value) return;
  
  const { type, data } = renameTarget.value;
  
  if (type === 'category') {
    data.name = newVal;
  } else if (type === 'subcategory') {
    const subs = selectedCategory.value.subCategories;
    const index = subs.indexOf(data);
    if (index !== -1) {
      subs[index] = newVal;
      // Update linked products
      dummyProducts.value.forEach(p => {
        if (p.sub === data) p.sub = newVal;
      });
      // Update view if currently selected
      if (selectedSubcategory.value === data) {
        selectedSubcategory.value = newVal;
      }
    }
  }
  showRenameDialog.value = false;
}

// --- Delete Operations ---
function promptDelete(type, data) {
  deleteTarget.value = { type, data };
  showDeleteConfirmDialog.value = true;
}

function confirmDelete() {
  if (!deleteTarget.value) return;
  const { type, data } = deleteTarget.value;

  if (type === 'category') {
    parentCategories.value = parentCategories.value.filter(c => c.id !== data.id);
    if (selectedCategory.value && selectedCategory.value.id === data.id) {
      goHome();
    }
  } else if (type === 'subcategory-page') {
    const subs = selectedCategory.value.subCategories;
    const idx = subs.indexOf(data);
    if (idx !== -1) subs.splice(idx, 1);
    dummyProducts.value = dummyProducts.value.filter(p => p.sub !== data);
    goToCategory(selectedCategory.value);
  } else if (type === 'subcategory-item') {
    const subs = selectedCategory.value.subCategories;
    const idx = subs.indexOf(data);
    if (idx !== -1) subs.splice(idx, 1);
    dummyProducts.value = dummyProducts.value.filter(p => p.sub !== data);
  } else if (type === 'product') {
    dummyProducts.value = dummyProducts.value.filter(p => p.id !== data);
  }
  showDeleteConfirmDialog.value = false;
}

// --- Utils ---
function getProductCount(subName) { return dummyProducts.value.filter(p => p.sub === subName).length; }
function setSortOption(option) { sortOption.value = option; showSortDropdown.value = false; }
function formatPrice(p) { return p.toLocaleString('ru-RU'); }
function handleGlobalClick(event) { if (!event.target.closest('.add-menu-wrapper')) showAddMenu.value = false; if (!event.target.closest('.dropdown-wrapper')) showSortDropdown.value = false; }

onMounted(() => document.addEventListener('click', handleGlobalClick));
onUnmounted(() => document.removeEventListener('click', handleGlobalClick));
</script>

<style scoped>
.admin-wrapper { padding: 20px 40px; min-height: 80vh; font-family: 'Segoe UI', sans-serif; }
.breadcrumbs { font-size: 14px; color: #9E9E9E; margin-bottom: 25px; }
.crumb-link { cursor: pointer; transition: color 0.2s; }
.crumb-link:hover { color: #FF7A00; }
.crumb-link.active { color: #333; font-weight: 500; cursor: default; }
.separator { margin: 0 8px; color: #CCC; }

/* === Headers === */
.catalog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
.header-left-group { display: flex; align-items: center; gap: 12px; }
.catalog-title { font-size: 28px; font-weight: 700; color: #111827; margin: 0; line-height: 1.2; }
.header-right-group { display: flex; align-items: center; gap: 12px; }

/* === Controls === */
.control-box { position: relative; }
.search-box-styled { display: flex; align-items: center; background-color: #F3F4F6; border-radius: 8px; padding: 0 12px; height: 40px; min-width: 200px; }
.search-box-styled input { border: none; background: transparent; outline: none; font-size: 14px; color: #374151; margin-left: 8px; width: 100%; }

.sort-btn-styled { display: flex; align-items: center; gap: 8px; background-color: #F3F4F6; border: none; border-radius: 8px; padding: 0 16px; height: 40px; font-size: 14px; color: #4B5563; cursor: pointer; white-space: nowrap; transition: all 0.2s; }
.sort-btn-styled.is-active { background-color: #FF7A00; color: white; }
.sort-btn-styled.is-active svg { stroke: white; }

.add-circle-btn { display: flex; align-items: center; justify-content: center; width: 40px; height: 40px; border-radius: 50%; background-color: #F3F4F6; border: none; cursor: pointer; color: #6B7280; transition: background 0.2s; }
.add-circle-btn:hover { background-color: #E5E7EB; }

/* === Dropdown Menu === */
.custom-dropdown-menu { position: absolute; top: calc(100% + 8px); right: 0; background: white; border: 1px solid #E5E7EB; border-radius: 12px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); min-width: 200px; padding: 6px 0; z-index: 9999; }
.custom-dropdown-menu.right-align { right: 0; left: auto; }
.custom-dropdown-menu.sort-menu-design { width: 240px; padding: 16px 0; background: #F9FAFB; top: calc(100% + 10px); }

.dd-item { padding: 10px 16px; font-size: 14px; color: #374151; cursor: pointer; }
.dd-item:hover { background-color: #FFF7ED; color: #FF7A00; }
.sort-item { display: flex; align-items: center; gap: 12px; padding: 8px 20px; cursor: pointer; }
.sort-item:hover { background-color: #F3F4F6; }
.sort-text { font-size: 15px; color: #374151; }
.sort-group-label { padding: 0 20px; margin-bottom: 12px; font-size: 14px; color: #6B7280; line-height: 1.4; }
.sort-group-label:not(:first-child) { margin-top: 12px; }
.dd-divider { height: 1px; background-color: #E5E7EB; margin: 12px 20px; }
.radio-indicator { width: 20px; height: 20px; border-radius: 50%; background-color: #E5E7EB; position: relative; flex-shrink: 0; }
.radio-indicator.selected { background-color: #FF7A00; }
.radio-indicator.selected::after { content: ''; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); width: 8px; height: 8px; background-color: white; border-radius: 50%; }

/* === List Styles (View 1) === */
.list-container { background: #fff; border-radius: 12px; border: 1px solid #E5E7EB; overflow: hidden; }
.list-header { display: flex; justify-content: space-between; padding: 12px 20px; background: #F9FAFB; color: #6B7280; font-size: 12px; text-transform: uppercase; font-weight: 600; border-bottom: 1px solid #E5E7EB; }
.list-row { display: flex; justify-content: space-between; align-items: center; padding: 16px 20px; border-bottom: 1px solid #F3F4F6; background: #dddddd; transition: background-color 0.2s ease; }
.list-row:hover, .list-row.row-expanded { background-color: #f9f9f9; }
.list-row.clickable:hover .item-text, .clickable-area:hover .item-text { color: #FF7A00; }
.row-main { display: flex; align-items: center; gap: 12px; flex-grow: 1; cursor: pointer; }
.item-text { font-size: 15px; color: #333; font-weight: 500; }
.row-meta { display: flex; align-items: center; gap: 12px; }
.meta-badge { background: #fff; color: #6B7280; padding: 4px 10px; border-radius: 12px; font-size: 13px; font-weight: 500; }
.expand-arrow-btn { background: transparent; border: none; cursor: pointer; padding: 8px; border-radius: 50%; color: #9CA3AF; display: flex; align-items: center; justify-content: center; transition: all 0.2s; margin-left: 8px; }
.expand-arrow-btn:hover { background-color: rgba(255,255,255,0.5); color: #333; }
.expand-arrow-btn.is-expanded { transform: rotate(180deg); color: #FF7A00; }
.inline-subcategories { background-color: #dddddd; box-shadow: inset 0 2px 4px rgba(0,0,0,0.02); }
.inline-sub-row { padding: 12px 20px 12px 54px; font-size: 14px; color: #555; cursor: pointer; border-bottom: 1px solid #e0e0e0; display: flex; align-items: center; justify-content:space-between; transition: background 0.2s; background-color: #dddddd; }
.inline-sub-row:hover { background-color: #f9f9f9; color: #FF7A00; }
.inline-empty { padding: 12px 20px 12px 54px; font-size: 13px; color: #888; background: #dddddd; }

/* === Sub View Cards (View 2 & 3) === */
.sub-view-card { background-color: #F9FAFB; border-radius: 16px; padding: 20px 24px; margin-bottom: 20px; }
.header-card { display: flex; justify-content: space-between; align-items: center; }
.header-card-actions { display: flex; gap: 8px; }
.list-card { padding-bottom: 12px; }
.card-left { display: flex; align-items: center; gap: 12px; }
.icon-orange { color: #FF7A00; }
.card-title { font-size: 20px; font-weight: 600; color: #374151; margin: 0; }
.card-subtitle { font-size: 16px; font-weight: 600; color: #4B5563; margin: 0; }
.icon-btn { background: none; border: none; cursor: pointer; padding: 6px; color: #9CA3AF; transition: color 0.2s; border-radius: 6px; }
.icon-btn:hover { color: #374151; background: rgba(0,0,0,0.05); }
.icon-btn.danger:hover { color: #EF4444; background: rgba(239, 68, 68, 0.1); }

.card-header-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; flex-wrap: wrap; gap: 16px; }
.card-actions { display: flex; align-items: center; gap: 12px; }
.card-search { display: flex; align-items: center; background-color: #FFFFFF; border-radius: 8px; padding: 0 12px; height: 40px; min-width: 240px; }
.card-search input { border: none; background: transparent; outline: none; font-size: 14px; color: #374151; margin-left: 8px; width: 100%; }
.card-add-btn { display: flex; align-items: center; justify-content: center; width: 40px; height: 40px; border-radius: 50%; background-color: #FFFFFF; border: none; cursor: pointer; color: #6B7280; transition: background 0.2s; }
.card-add-btn:hover { background-color: #F3F4F6; }
.card-sort-btn { display: flex; align-items: center; gap: 8px; background-color: #FFFFFF; border: none; border-radius: 8px; padding: 0 12px; height: 40px; font-size: 14px; color: #6B7280; cursor: pointer; white-space: nowrap; transition: all 0.2s; }
.card-sort-btn:hover { background-color: #F3F4F6; }
.card-sort-btn.is-active { background-color: #FF7A00; color: white; }
.card-sort-btn.is-active svg { stroke: white; }

.card-list-body { border-top: 1px solid #E5E7EB; }
.card-list-item { display: flex; justify-content: space-between; align-items: center; padding: 16px 0; border-bottom: 1px solid #E5E7EB; cursor: pointer; transition: padding-left 0.2s; }
.card-list-item:hover { padding-left: 8px; }
.card-list-item:hover .sub-text { color: #111827; }
.sub-text { font-size: 14px; color: #4B5563; }
.delete-icon-btn { background: none; border: none; cursor: pointer; color: #9CA3AF; padding: 4px; transition: color 0.2s; }
.delete-icon-btn:hover { color: #EF4444; }
.card-empty { padding: 30px; text-align: center; color: #9CA3AF; font-size: 14px; }
.card-footer { padding-top: 20px; text-align: center; }
.show-more-text { color: #FF7A00; font-size: 14px; cursor: pointer; font-weight: 500; }
.show-more-text:hover { text-decoration: underline; }

/* Product Item Grid */
.product-item { display: grid; grid-template-columns: 1fr 80px 100px 40px; gap: 16px; align-items: center; }
.item-main-text { font-weight: 500; color: #1F2937; }
.prod-col-rating { display: flex; align-items: center; gap: 4px; font-size: 14px; color: #4B5563; font-weight: 500; }
.prod-col-price { text-align: right; }
.price-val { font-weight: 600; color: #111827; font-size: 14px; }
.prod-col-action { display: flex; justify-content: flex-end; }
.active-filters { display: flex; gap: 8px; margin-bottom: 15px; flex-wrap: wrap; }
.card-filters-row { margin-bottom: 12px; display: flex; flex-wrap: wrap; gap: 8px; }
.filter-tag { display: inline-flex; align-items: center; gap: 6px; background: #FFF7ED; color: #FF7A00; padding: 6px 12px; border-radius: 20px; font-size: 13px; font-weight: 500; }
.tag-remove { background: none; border: none; color: #FF7A00; font-size: 16px; cursor: pointer; }

/* === Common & Modal === */
.gray-input { width: 100%; background-color: #F3F4F6; border: none; border-radius: 12px; padding: 14px 16px; font-size: 14px; color: #333; outline: none; }
.btn-gray { background: #F9FAFB; color: #374151; border: none; padding: 12px 24px; border-radius: 12px; font-weight: 500; cursor: pointer; font-size: 14px; }
.btn-gray:hover { background: #F3F4F6; }
.btn-orange { background: #FF7A00; color: white; border: none; padding: 12px 24px; border-radius: 12px; font-weight: 600; cursor: pointer; font-size: 14px; }
.btn-orange:hover { background: #E56D00; }
.btn-red { background: #EF4444; color: white; border: none; padding: 12px 24px; border-radius: 12px; font-weight: 600; cursor: pointer; font-size: 14px; }
.btn-red:hover { background: #DC2626; }
.mb-4 { margin-bottom: 16px; }

.blur-overlay { background: rgba(0, 0, 0, 0.2); backdrop-filter: blur(2px); z-index: 1000; position: fixed; top: 0; left: 0; width: 100%; height: 100%; display: flex; justify-content: center; align-items: center;}
.centered-modal { width: 440px; padding: 40px; text-align: center; border-radius: 20px; box-shadow: 0 20px 40px rgba(0,0,0,0.08); background: white;}
.modal-title-center { font-size: 20px; margin-bottom: 10px; font-weight: 600; }
.modal-hint-center { font-size: 14px; color: #6B7280; margin-bottom: 24px; }
.modal-text { font-size: 14px; color: #6B7280; margin-bottom: 20px; line-height: 1.5; }
.modal-actions-center { margin-top: 30px; display: flex; justify-content: center; gap: 12px; }
.delete-icon-wrapper { width: 60px; height: 60px; border-radius: 50%; background: #FEE2E2; display: flex; justify-content: center; align-items: center; margin: 0 auto 20px auto; }
.modal-overlay { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0, 0, 0, 0.4); display: flex; justify-content: center; align-items: center; z-index: 999; }
.modal-content:not(.centered-modal) { background: white; padding: 30px; border-radius: 16px; width: 400px; max-width: 90%; }
.full-input { width: 100%; padding: 12px 14px; border: 1px solid #E0E0E0; border-radius: 8px; font-size: 14px; }
.btn-primary { background: #FF7A00; color: white; border: none; padding: 10px 20px; border-radius: 8px; cursor: pointer; }
.btn-secondary { background: #F3F4F6; color: #333; border: none; padding: 10px 20px; border-radius: 8px; cursor: pointer; }
.modal-actions { margin-top: 25px; display: flex; gap: 10px; justify-content: flex-end; }

.sidebar-overlay { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0, 0, 0, 0.3); z-index: 1000; opacity: 0; pointer-events: none; transition: opacity 0.3s; }
.sidebar-overlay.open { opacity: 1; pointer-events: auto; }
.sidebar-panel { position: absolute; top: 0; right: 0; width: 480px; height: 100%; background: white; padding: 40px; box-shadow: -10px 0 30px rgba(0,0,0,0.05); transform: translateX(100%); transition: transform 0.3s; display: flex; flex-direction: column; box-sizing: border-box; }
.sidebar-overlay.open .sidebar-panel { transform: translateX(0); }
.sidebar-title { font-size: 24px; margin: 0 0 40px 0; font-weight: 600; }
.sidebar-form { flex-grow: 1; }
.sidebar-actions { display: flex; justify-content: flex-end; gap: 12px; }

/* Custom Select */
.custom-select-container { border: 1px solid #E5E7EB; border-radius: 12px; overflow: hidden; }
.select-header { background-color: #F9FAFB; padding: 14px 16px; cursor: pointer; display: flex; justify-content: space-between; align-items: center; font-size: 14px; color: #6B7280; }
.select-header.is-open { background-color: #FF7A00; color: white; border-radius: 12px 12px 0 0; }
.select-header.is-open .select-arrow { transform: rotate(180deg); color: white; }
.select-body { border: 1px solid #E5E7EB; border-top: none; border-radius: 0 0 12px 12px; padding: 16px; background: white; }
.select-search-wrapper { position: relative; margin-bottom: 12px; }
.search-icon-sm { position: absolute; left: 0; top: 2px; }
.select-search-input { width: 100%; border: none; border-bottom: 1px solid #E5E7EB; padding: 4px 0 8px 24px; font-size: 14px; outline: none; }
.select-list { max-height: 200px; overflow-y: auto; display: flex; flex-direction: column; gap: 12px; margin-bottom: 12px; }
.select-option { display: flex; align-items: center; gap: 10px; cursor: pointer; font-size: 14px; color: #4B5563; }
.select-option input { display: none; }
.radio-custom { width: 18px; height: 18px; border-radius: 50%; background: #E5E7EB; position: relative; transition: all 0.2s; }
.select-option input:checked + .radio-custom { background: #FF7A00; border: 4px solid #FFD8B3; }
.show-more-link { color: #FF7A00; font-size: 13px; cursor: pointer; text-align: center; margin-top: 10px; }

@media (max-width: 768px) {
  .admin-wrapper { padding: 15px 20px; }
  .header-right-group, .card-header-row { flex-wrap: wrap; }
  .sidebar-panel { width: 100%; }
  .product-item { grid-template-columns: 1fr auto; grid-template-rows: auto auto; gap: 8px; }
  .prod-col-rating, .prod-col-price, .prod-col-action { grid-row: 2; }
  .prod-col-name { grid-column: 1 / -1; }
  .sort-btn-text { display: none; }
}
</style>