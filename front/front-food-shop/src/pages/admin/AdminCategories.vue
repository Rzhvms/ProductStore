<template>
  <div v-if="isLoading" class="loading-overlay">
    <span class="loader"></span>
  </div>
  <div v-if="error" class="error-toast" @click="clearError">
    {{ error }}
    <button>Закрыть</button>
  </div>
  <AdminLayout>
    <template #default>
      <div class="admin-wrapper">
        <div class="breadcrumbs">
          <span 
            class="crumb-link" 
            :class="{ active: currentView === 'categories' }"
            @click="goHome"
          >
            Каталог
          </span>
          <span v-if="selectedCategory" class="separator">›</span>
          <span 
            v-if="selectedCategory" 
            class="crumb-link"
            :class="{ active: currentView === 'subcategories' }"
            @click="goToCategory(selectedCategory)"
          >
            {{ selectedCategory.categoryName }}
          </span>
          <span v-if="selectedSubcategory" class="separator">›</span>
          <span 
            v-if="selectedSubcategory" 
            class="crumb-link active"
          >
            {{ selectedSubcategory.categoryName }}
          </span>
        </div>
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
                <img src="../../assets/close-circle.svg" class="clear-circle" @click="searchQuery = ''" />
              </div>
              <div class="control-box dropdown-wrapper">
                <button 
                  class="sort-btn-styled" 
                  :class="{ 'is-active': showSortDropdown || sortOption }"
                  @click.stop="showSortDropdown = !showSortDropdown"
                >
                  <img v-if="!(showSortDropdown || sortOption)" src="../../assets/drop down button.svg" />
                  <img v-else src="../../assets/drop down button(1).svg" />
                  <span>{{ buttonSortLabel }}</span>
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
                  <div class="dd-divider"></div>
                  <div class="dd-item" @click="openSubSidebar">Добавить подкатегорию</div>
                </div>
              </div>
            </div>
          </div>
          <div v-if="sortOption" class="active-filters">
            <span class="filter-tag">{{ sortLabel }} <button class="tag-remove" @click="sortOption = ''">×</button></span>
          </div>
          <div class="list-container">
            <div v-for="cat in visibleCategories" :key="cat.categoryId" class="category-item-wrapper">
              <div class="list-row" :class="{ 'row-expanded': expandedCategoryIds.has(cat.categoryId) }">
                <div class="row-main clickable-area" @click="goToCategory(cat)">
                  <span class="item-text">{{ cat.categoryName }}</span>
                </div>
                <div class="row-meta">
                  <span class="meta-badge" title="Количество подкатегорий">{{ cat.subCategories.length }}</span>
                  <button class="expand-arrow-btn" :class="{ 'is-expanded': expandedCategoryIds.has(cat.categoryId) }" @click.stop="toggleExpand(cat.categoryId)">
                    <img src="../../assets/arrow-down.svg" />
                  </button>
                </div>
              </div>
              <div v-if="expandedCategoryIds.has(cat.categoryId)" class="inline-subcategories">
                <div v-for="(sub, idx) in cat.subCategories" :key="idx" class="inline-sub-row" @click="goToSubcategoryFromInline(cat, sub)">{{ sub.categoryName }}<span class="inline-sub-count meta-badge">{{ getProductCount(sub) }}</span></div>
                <div v-if="cat.subCategories.length === 0" class="inline-empty">Нет подкатегорий</div>
              </div>
            </div>
            <div v-if="sortedCategories.length === 0" class="empty-text">Категории не найдены</div>
          </div>
          <div class="card-footer" v-if="visibleCategories.length < sortedCategories.length">
            <span class="show-more-text" @click="showMoreCategories">Показать ещё</span>
          </div>
        </div>
        <div v-else-if="currentView === 'subcategories' && selectedCategory" style="display: flex; flex-direction: column;">
          <button class="icon-btn danger" title="Удалить категорию" @click="promptDelete('category', selectedCategory)">
            <span>Удалить категорию</span>
            <img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/>
          </button>
          <div class="sub-view-card header-card">
            <div class="card-left">
              <img src="../../assets/folder.svg" alt="Logo" class="icon-orange" />
              <h1 class="card-title">{{ selectedCategory.categoryName }}</h1>
            </div>
            <div class="header-card-actions">
              <button class="icon-btn" title="Редактировать" @click="openRenameDialog('category', selectedCategory)">
                <img src="../../assets/edit.svg" alt="edit" />
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
                <div class="search-box-styled card">
                  <img src="../../assets/search-normal.svg" alt="search" class="search-icon" />
                  <input type="text" placeholder="Поиск..." v-model="searchQuery">
                  <img src="../../assets/close-circle.svg" class="clear-circle" @click="searchQuery = ''" />
                </div>
                <div class="control-box dropdown-wrapper card">
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
                    <div class="sort-group-label">По количеству<br>товаров</div>
                    <div class="sort-item" @click="setSortOption('count-desc')"><div class="radio-indicator" :class="{ selected: sortOption === 'count-desc' }"></div><span class="sort-text">Сначала больше</span></div>
                    <div class="sort-item" @click="setSortOption('count-asc')"><div class="radio-indicator" :class="{ selected: sortOption === 'count-asc' }"></div><span class="sort-text">Сначала меньше</span></div>
                  </div>
                </div>
                <button class="add-circle-btn card" @click="openAddSubDialog">
                  <img src="../../assets/add-circle.svg" />
                </button>
              </div>
            </div>
            <div v-if="sortOption" class="card-filters-row">
               <span class="filter-tag">{{ sortLabel }} <button class="tag-remove" @click="sortOption = ''">×</button></span>
            </div>
            <div class="card-list-body">
              <div v-for="(sub, index) in visibleSubcategories" :key="index" class="card-list-item" @click="goToSubcategory(sub)">
                <span class="sub-text">{{ sub.categoryName }}</span>
                <div class="row-meta">
                  <span class="meta-badge" title="Количество товаров">{{ getProductCount(sub) }}</span>
                  <button class="expand-arrow-btn card" @click="goToSubcategory(sub)">
                    <img src="../../assets/arrow-down.svg" />
                  </button>
                </div>
              </div>
              <div v-if="sortedSubcategories.length === 0" class="card-empty">Подкатегории не найдены</div>
            </div>
            <div class="card-footer" v-if="visibleSubCount < sortedSubcategories.length">
              <span class="show-more-text" @click="showMoreSubcategories">Показать ещё</span>
            </div>
          </div>
        </div>
        <div v-else-if="currentView === 'products' && selectedSubcategory" style="display: flex; flex-direction: column;">
          <button class="icon-btn danger" title="Удалить подкатегорию" @click="promptDelete('subcategory-page', selectedSubcategory)">
            <span>Удалить подкатегорию</span>
            <img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/>
          </button>
          <div class="sub-view-card header-card">
            <div class="card-left">
              <img src="../../assets/folder-open.svg" alt="Logo" class="icon-orange" />
              <h1 class="card-title">{{ selectedSubcategory.categoryName }}</h1>
            </div>
            <div class="header-card-actions">
               <button class="icon-btn" title="Переименовать подкатегорию" @click="openRenameDialog('subcategory', selectedSubcategory)">
                <img src="../../assets/edit.svg" alt="edit" />
              </button>
            </div>
          </div>
          <div class="sub-view-card list-card">
            <div class="card-header-row">
              <div class="card-left">
                <img src="../../assets/box.svg" alt="Logo" class="icon-orange" />
                <h2 class="card-subtitle">Список товаров</h2>
              </div>
              <div class="card-actions">
                <div class="search-box-styled card">
                  <img src="../../assets/search-normal.svg" alt="search" class="search-icon" />
                  <input type="text" placeholder="Поиск товара..." v-model="searchQuery">
                  <img src="../../assets/close-circle.svg" class="clear-circle" @click="searchQuery = ''" />
                </div>
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
                <button class="add-circle-btn card" @click="showAddProductDialog = true">
                  <img src="../../assets/add-circle.svg" />
                </button>
              </div>
            </div>
            <div v-if="sortOption" class="card-filters-row">
               <span class="filter-tag">{{ sortLabel }} <button class="tag-remove" @click="sortOption = ''">×</button></span>
            </div>
            <div class="card-list-body">
              <div v-for="prod in visibleProducts" :key="prod.id" class="card-list-item product-item">
                <div class="prod-col-name">
                  <span class="sub-text item-main-text">{{ prod.name }}</span>
                </div>
                <div class="rat-pri-del">
                  <div class="prod-col-rating">
                    <div class="stars-wrapper">
                      <svg v-for="i in 5" :key="i" class="star-icon" width="32" height="32" viewBox="0 0 24 24">
                        <defs>
                          <linearGradient :id="'grad-' + prod.id + '-' + i">
                            <stop offset="0%" stop-color="#FF7A00" />
                            <stop 
                              :offset="calculateOffset(prod.rating, i)" 
                              stop-color="#FF7A00" 
                            />
                            <stop 
                              :offset="calculateOffset(prod.rating, i)" 
                              stop-color="#E5E7EB" 
                            />
                            <stop offset="100%" stop-color="#E5E7EB" />
                          </linearGradient>
                        </defs>
                        <path 
                          :fill="'url(#grad-' + prod.id + '-' + i + ')'"
                          d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" 
                        />
                      </svg>
                    </div>
                    <span class="rating-val-orange">{{ prod.rating }}</span>
                  </div>
                  <div class="prod-col-price">
                    <span class="price-val">{{ formatPrice(prod.price) }} ₽</span>
                  </div>
                  <div class="prod-col-action">
                    <button class="icon-btn danger card" @click.stop="promptDelete('product', prod.id)">
                      <img src="../../assets/trash.svg" style="filter: invert(48%) sepia(0%) saturate(3407%) hue-rotate(30deg) brightness(100%) contrast(88%);" />
                    </button>
                  </div>
                </div>  
              </div>
              <div v-if="visibleProducts.length === 0" class="card-empty">Товары не найдены</div>
            </div>
            <div class="card-footer" v-if="visibleCount < sortedProducts.length">
              <span class="show-more-text" @click="showMoreProducts">Показать ещё</span>
            </div>
          </div>
        </div>
        <div v-if="showAddCategoryDialog" class="modal-overlay blur-overlay" @click.self="showAddCategoryDialog = false">
          <div class="modal-content centered-modal">
            <h3 class="modal-title-center">Название категории</h3>
            <input v-model="newCategoryName" placeholder="Введите новое название" class="gray-input" @keyup.enter="addCategory"/>
            <div class="modal-actions-center">
              <button class="btn-gray" @click="showAddCategoryDialog = false">Отмена</button>
              <button class="btn-orange" @click="addCategory">Сохранить</button>
            </div>
          </div>
        </div>
        <div v-if="showAddSubDialog" class="modal-overlay blur-overlay" @click.self="showAddSubDialog = false">
          <div class="modal-content centered-modal">
             <h3 class="modal-title-center">Новая подкатегория</h3>
             <p class="modal-hint-center">В категорию: <b>{{ selectedCategory?.categoryName }}</b></p>
             <input v-model="newSubName" placeholder="Название подкатегории" class="gray-input" @keyup.enter="addSubcategoryFromModal"/>
             <div class="modal-actions-center">
               <button class="btn-gray" @click="showAddSubDialog = false">Отмена</button>
               <button class="btn-orange" @click="addSubcategoryFromModal">Добавить</button>
             </div>
          </div>
        </div>
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
        <div v-if="showDeleteConfirmDialog" class="modal-overlay blur-overlay" @click.self="showDeleteConfirmDialog = false">
          <div class="modal-content centered-modal">
            <h3 class="modal-title-center" v-if="currentView==='subcategories'">Удалить категорию</h3>
            <h3 class="modal-title-center" v-else-if="currentView==='products'">Удалить подкатегорию</h3>
            <p class="modal-text" v-if="currentView==='subcategories'">Вы дейсвительно хотите удалить категорию вместе со всеми подкатегориями и товарами без возможности восстановления?</p>
            <p class="modal-text" v-else-if="currentView==='products'">Вы дейсвительно хотите удалить подкатегорию вместе со всеми товарами без возможности восстановления?</p>
            <div class="modal-actions-center">
              <button class="btn-gray" @click="showDeleteConfirmDialog = false">Отмена</button>
              <button class="btn-red" @click="confirmDelete">Удалить</button>
            </div>
          </div>
        </div>
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
                    <div v-for="cat in filteredParentCategories" :key="cat.categoryId" class="select-option" :class="{ active: selectedParentId === cat.categoryId }" @click="selectedParentId = cat.categoryId">
                      <div class="radio-indicator" :class="{ selected: selectedParentId === cat.categoryId }"></div> 
                      <span class="option-text">{{ cat.categoryName }}</span>
                    </div>
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
        <div v-if="showAddProductDialog" class="modal-overlay" @click.self="showAddProductDialog = false">
          <div class="modal-content">
            <h3>Добавить товар</h3>
            <p class="modal-hint">Выберите товар из каталога для добавления</p>
            <select v-model="selectedProductToAdd" class="full-input">
              <option value="">Выберите товар...</option>
              <option v-for="prod in products" :key="prod.id" :value="prod.id">
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
import { categoryApi, adminProductApi } from '@/services/api';

const isLoading = ref(false);
const isSaving = ref(false);
const error = ref(null);

const categories = ref([]);
const products = ref([]);

const currentView = ref('categories');
const selectedCategory = ref(null);
const selectedSubcategory = ref(null);
const searchQuery = ref('');
const expandedCategoryIds = ref(new Set());
const itemsPerPage = 10;
const visibleCount = ref(itemsPerPage);
const visibleSubCount = ref(itemsPerPage);
const visibleCatCount = ref(itemsPerPage);

const showAddMenu = ref(false);
const showAddCategoryDialog = ref(false);
const showAddSubDialog = ref(false); 
const showAddSubSidebar = ref(false); 
const showAddProductDialog = ref(false);
const showRenameDialog = ref(false);
const showDeleteConfirmDialog = ref(false);

const newCategoryName = ref('');
const newSubName = ref('');
const selectedProductToAdd = ref('');
const renameValue = ref('');
const renameTarget = ref(null);
const deleteTarget = ref(null);

const isCategorySelectorOpen = ref(false);
const categorySearchQuery = ref('');
const selectedParentId = ref(null);

const sortOption = ref('');
const showSortDropdown = ref(false);

async function loadCategories() {
  isLoading.value = true;
  error.value = null;
  try {
    categories.value = await categoryApi.get();
    const prodRes = await adminProductApi.get(1, itemsPerPage);
    products.value = prodRes.productList;
  } catch (error) {
    error.value = error.message;
    console.error(error);
  } finally {
    isLoading.value = false;
  }
}

async function addCategory() {
  if (!newCategoryName.value.trim()) return;
  isSaving.value = true;
  try {
    const category = await categoryApi.create(newCategoryName.value.trim());
    newCategoryName.value = '';
    showAddCategoryDialog.value = false;
    await loadCategories();
  } catch (error) {
    error.value = error.message;
  } finally {
    isSaving.value = false;
  }
}

async function addSubcategory() {
  if (!newSubName.value.trim() || !selectedParentId.value) return;
  isSaving.value = true;
  try {
    const category = await categoryApi.create(newSubName.value.trim(), selectedParentId.value);
    newSubName.value = '';
    selectedParentId.value = null;
    showAddSubSidebar.value = false;
    await loadCategories();
  } catch (error) {
    error.value = error.message;
  } finally {
    isSaving.value = false;
  }
}

async function addSubcategoryFromModal() {
  if (!newSubName.value.trim() || !selectedCategory.value) return;
  isSaving.value = true;
  try {
    const category = await categoryApi.create(newSubName.value.trim(), selectedCategory.value.categoryId);
    newSubName.value = '';
    showAddSubDialog.value = false;
    await loadCategories();
    selectedCategory.value = parentCategories.value.find(c => c.categoryId === selectedCategory.value.categoryId);
  } catch (err) {
    error.value = err.message;
  } finally {
    isSaving.value = false;
  }
}

async function confirmRename() {
  const newVal = renameValue.value.trim();
  if (!newVal || !renameTarget.value) return;
  const { type, data } = renameTarget.value;
  isSaving.value = true;
  try {
    if (type === 'category') {
      await categoryApi.update(data.categoryId, newVal);
    } else if (type === 'subcategory') {
      await categoryApi.update(data.categoryId, newVal, data.parentId);
    }
    await loadCategories();
    if (selectedCategory.value) {
      selectedCategory.value = parentCategories.value.find(c => c.categoryId === selectedCategory.value.categoryId);
    }
    if (selectedSubcategory.value && type === 'subcategory') {
      selectedSubcategory.value = categories.value.find(c => c.categoryId === data.categoryId);
    }
    showRenameDialog.value = false;
  } catch (err) {
    error.value = err.message;
  } finally {
    isSaving.value = false;
  }
}

async function confirmDelete() {
  if (!deleteTarget.value) return;
  const { type, data } = deleteTarget.value;
  isSaving.value = true;
  try {
    if (type === 'category') {
      await categoryApi.delete(data.categoryId);
      await loadCategories();
      if (selectedCategory.value?.categoryId === data.categoryId) {
        goHome();
      }
    } else if (type === 'subcategory-page' || type === 'subcategory-item') {
      await categoryApi.delete(data.categoryId);
      await loadCategories();
      if (selectedCategory.value) {
        selectedCategory.value = parentCategories.value.find(c => c.categoryId === selectedCategory.value.categoryId);
      }
      if (type === 'subcategory-page') {
        currentView.value = 'subcategories';
        selectedSubcategory.value = null;
      }
    } else if (type === 'product') {
      adminProductApi.delete(data.id);
      await loadCategories();
    }
    showDeleteConfirmDialog.value = false;
  } catch (err) {
    error.value = err.message;
  } finally {
    isSaving.value = false;
  }
}

const parentCategories = computed(() => {
  return categories.value
    .filter(c => c.parentCategoryId === null)
    .map(parent => ({
      ...parent,
      subCategories: categories.value.filter(sub => sub.parentCategoryId === parent.categoryId)
    }));
});

const filteredParentCategories = computed(() => {
  if (!categorySearchQuery.value) return parentCategories.value;
  return parentCategories.value.filter(c => 
    c.categoryName.toLowerCase().includes(categorySearchQuery.value.toLowerCase())
  );
});

const selectedParentName = computed(() => {
  const parent = parentCategories.value.find(c => c.categoryId === selectedParentId.value);
  return parent ? parent.categoryName : 'Выбрать категорию из списка';
});

const filteredCategories = computed(() => {
  if (!searchQuery.value) return parentCategories.value;
  const q = searchQuery.value.toLowerCase();
  return parentCategories.value.filter(c => c.categoryName.toLowerCase().includes(q));
});

const sortedCategories = computed(() => {
  let items = [...filteredCategories.value];
  switch (sortOption.value) {
    case 'name-asc': items.sort((a, b) => a.categoryName.localeCompare(b.categoryName, 'ru')); break;
    case 'name-desc': items.sort((a, b) => b.categoryName.localeCompare(a.categoryName, 'ru')); break;
    case 'count-desc': items.sort((a, b) => b.subCategories.length - a.subCategories.length); break;
    case 'count-asc': items.sort((a, b) => a.subCategories.length - b.subCategories.length); break;
  }
  return items;
});

const visibleCategories = computed(() => {
  return sortedCategories.value.slice(0, visibleCatCount.value);
});

const filteredSubcategories = computed(() => {
  if (!selectedCategory.value) return [];
  const subs = selectedCategory.value.subCategories || [];
  if (!searchQuery.value) return subs;
  const q = searchQuery.value.toLowerCase();
  return subs.filter(s => s.categoryName.toLowerCase().includes(q));
});

const sortedSubcategories = computed(() => {
  let items = [...filteredSubcategories.value];
  if (sortOption.value === 'name-asc') items.sort((a, b) => a.categoryName.localeCompare(b.categoryName, 'ru'));
  if (sortOption.value === 'name-desc') items.sort((a, b) => b.categoryName.localeCompare(a.categoryName, 'ru'));
  return items;
});

const visibleSubcategories = computed(() => {
  return sortedSubcategories.value.slice(0, visibleSubCount.value);
});

const filteredProducts = computed(() => {
  if (!selectedSubcategory.value) return [];
  let prods = products.value.filter(p => p.categoryId === selectedSubcategory.value.categoryId);
  if (searchQuery.value) {
    prods = prods.filter(p => p.name.toLowerCase().includes(searchQuery.value.toLowerCase()));
  }
  return prods;
});

const sortedProducts = computed(() => {
  let items = [...filteredProducts.value];
  switch (sortOption.value) {
    case 'name-asc': items.sort((a, b) => a.name.localeCompare(b.name, 'ru')); break;
    case 'name-desc': items.sort((a, b) => b.name.localeCompare(a.name, 'ru')); break;
    case 'price-asc': items.sort((a, b) => a.price - b.price); break;
    case 'price-desc': items.sort((a, b) => b.price - a.price); break;
    case 'rating-desc': items.sort((a, b) => b.rating - a.rating); break;
    case 'rating-asc': items.sort((a, b) => a.rating - b.rating); break;
  }
  return items;
});

const visibleProducts = computed(() => {
  return sortedProducts.value.slice(0, visibleCount.value);
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

const buttonSortLabel = computed(() => {
  if (!sortOption.value) return 'Сортировка';
  if (sortOption.value.startsWith('name-')) return 'По алфавиту';
  if (sortOption.value.startsWith('count-')) return 'По количеству';
  if (sortOption.value.startsWith('price-')) return 'По цене';
  if (sortOption.value.startsWith('rating-')) return 'По рейтингу';
  return 'Сортировка';
});

function goHome() {
  currentView.value = 'categories';
  selectedCategory.value = null;
  selectedSubcategory.value = null;
  searchQuery.value = '';
  sortOption.value = '';
}

function goToCategory(cat) {
  selectedCategory.value = cat;
  selectedSubcategory.value = null;
  currentView.value = 'subcategories';
  searchQuery.value = '';
  sortOption.value = '';
  visibleSubCount.value = itemsPerPage;
}

function goToSubcategoryFromInline(cat, sub) {
  selectedCategory.value = cat;
  selectedSubcategory.value = sub; // теперь это объект!
  currentView.value = 'products';
  searchQuery.value = '';
  sortOption.value = '';
}

function goToSubcategory(sub) {
  selectedSubcategory.value = sub; // теперь это объект!
  currentView.value = 'products';
  searchQuery.value = '';
  sortOption.value = '';
  visibleCount.value = itemsPerPage;
}

function toggleExpand(id) {
  if (expandedCategoryIds.value.has(id)) {
    expandedCategoryIds.value.delete(id);
  } else {
    expandedCategoryIds.value.add(id);
  }
}

function showMoreProducts() { visibleCount.value += itemsPerPage; }
function showMoreSubcategories() { visibleSubCount.value += itemsPerPage; }
function showMoreCategories() { visibleCatCount.value += itemsPerPage; }

function toggleAddMenu() { showAddMenu.value = !showAddMenu.value; }

function openCategoryModal() {
  showAddMenu.value = false;
  showAddCategoryDialog.value = true;
  newCategoryName.value = '';
}

function openSubSidebar() {
  showAddMenu.value = false;
  showAddSubSidebar.value = true;
  newSubName.value = '';
  selectedParentId.value = null;
  isCategorySelectorOpen.value = false;
  categorySearchQuery.value = '';
}

function openAddSubDialog() {
  newSubName.value = '';
  showAddSubDialog.value = true;
}

function openRenameDialog(type, data) {
  renameTarget.value = { type, data };
  renameValue.value = data.categoryName;
  showRenameDialog.value = true;
}

function promptDelete(type, data) {
  deleteTarget.value = { type, data };
  showDeleteConfirmDialog.value = true;
}

async function addProductToSubcategory() {
  if (!selectedProductToAdd.value) return;
  const prod = products.value.find(p => p.id === Number(selectedProductToAdd.value));
  if (prod && selectedSubcategory.value) {
    await adminProductApi.update(id, prod.name, prod.providerId, prod.description, prod.price, prod.quantity, selectedSubcategory.value.categoryId, prod.characteristics);
    await loadCategories();
  }
  selectedProductToAdd.value = '';
  showAddProductDialog.value = false;
}

function getProductCount(sub) {
  return products.value ? products.value.filter(p => p.categoryId === sub.categoryId).length : 0;
}

function setSortOption(option) {
  sortOption.value = option;
  showSortDropdown.value = false;
}

function formatPrice(p) {
  return p.toLocaleString('ru-RU');
}

function handleGlobalClick(event) {
  if (!event.target.closest('.add-menu-wrapper')) showAddMenu.value = false;
  if (!event.target.closest('.dropdown-wrapper')) showSortDropdown.value = false;
}

const calculateOffset = (rating, index) => {
  if (rating >= index) return '100%';
  if (rating <= index - 1) return '0%';
  return ((rating % 1) * 100) + '%';
};

function clearError() {
  error.value = null;
}

onMounted(async () => {
  document.addEventListener('click', handleGlobalClick);
  await loadCategories();
});

onUnmounted(() => {
  document.removeEventListener('click', handleGlobalClick);
});
</script>

<style scoped>
.admin-wrapper { padding: 20px 40px; min-height: 80vh; font-family: 'Segoe UI', sans-serif; }
.breadcrumbs { font-size: 14px; color: #9E9E9E; margin-bottom: 25px; }
.crumb-link { cursor: pointer; transition: color 0.2s; }
.crumb-link:hover { color: #FFA84C; }
.crumb-link.active { color: #FF7A00; font-weight: 500; cursor: default; }
.separator { margin: 0 8px; color: #CCC; }

.catalog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
.header-left-group { display: flex; align-items: center; gap: 12px; }
.catalog-title { font-size: 28px; font-weight: 700; color: #111827; margin: 0; line-height: 1.2; }
.header-right-group { display: flex; align-items: center; gap: 12px; }

.control-box { position: relative; }
.search-box-styled { display: flex; align-items: center; background-color: #f9f9f9; border-radius: 16px; padding: 12px 24px; min-width: 200px; }
.search-box-styled input { border: none; background: transparent; outline: none; font-size: 14px; color: #374151; margin-left: 8px; width: 100%; }
.search-box-styled.card { background-color: #ffffff; }
.search-box-styled.card:hover { background-color: #F3F4F6; }

.sort-btn-styled { position: relative; z-index: 10000; justify-content: center; min-width: 210px; display: flex; align-items: center; gap: 8px; background-color: #f9f9f9; border: none; border-radius: 16px; padding: 12px 18px; font-size: 14px; color: #4B5563; cursor: pointer; white-space: nowrap; transition: all 0.2s; }
.sort-btn-styled.is-active { background-color: #FF7A00; color: white; }
.sort-btn-styled.is-active svg { stroke: white; }
.sort-btn-styled.card { background-color: #ffffff; }
.sort-btn-styled.card:hover { background-color: #F3F4F6; }
.sort-btn-styled.card.is-active { background-color: #FF7A00; color: white; }


.add-circle-btn { display: flex; align-items: center; justify-content: center; border-radius: 16px; background-color: #f9f9f9; border: none; cursor: pointer; color: #6B7280; transition: background 0.2s; padding: 12px 18px; }
.add-circle-btn:hover { background-color: #E5E7EB; }
.add-circle-btn.card { background-color: #ffffff; }
.add-circle-btn.card:hover { background-color: #F3F4F6; }
.custom-dropdown-menu { min-width: 210px; position: absolute; top: calc(100% + 8px); border: 1px solid #E5E7EB; border-radius: 16px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); padding: 6px 0; z-index: 9999; background-color: #f6f6f6; }
.custom-dropdown-menu.right-align { right: 0; left: auto; }
.custom-dropdown-menu.sort-menu-design { padding: 16px 0; background: #FFF; top: calc(100% - 10px); }

.dd-item { padding: 10px 16px; font-size: 16px; color: #374151; cursor: pointer; }
.dd-item:hover { background-color: #FFF7ED; color: #FF7A00; }
.sort-item { display: flex; align-items: center; gap: 12px; padding: 8px 20px; cursor: pointer; }
.sort-item:hover { background-color: #F3F4F6; }
.sort-text { color: #555555; }
.sort-group-label { padding: 0 20px; margin-bottom: 12px; color: #555; line-height: 24px; }
.sort-group-label:not(:first-child) { margin-top: 12px; }
.dd-divider { height: 1px; background-color: #dddddd; margin: 0 auto; width: 83%;}
.radio-indicator { width: 20px; height: 20px; border-radius: 50%; background-color: #E5E7EB; position: relative; flex-shrink: 0; }
.radio-indicator.selected { background-color: #FF7A00; }
.radio-indicator.selected::after { content: ''; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); width: 8px; height: 8px; background-color: white; border-radius: 50%; }

.list-container { background: #fff; overflow: hidden; }
.list-header { display: flex; justify-content: space-between; padding: 12px 20px; background: #F9FAFB; color: #6B7280; font-size: 12px; text-transform: uppercase; font-weight: 600; border-bottom: 1px solid #E5E7EB; }
.list-row { display: flex; justify-content: space-between; align-items: center; padding: 16px 8px; border-bottom: 1px solid #dddddd; background: #ffffff; transition: background-color 0.2s ease; }
.list-row:hover { background-color: #f9f9f9; }
.list-row.clickable:hover .item-text, .clickable-area:hover .item-text { color: #FF7A00; }
.row-main { display: flex; align-items: center; gap: 12px; flex-grow: 1; cursor: pointer; }
.item-text { color: #7a7a7a; font-weight: 500; }
.row-meta { display: flex; align-items: center; gap: 12px; }
.meta-badge { background: #f9f9f9; color: #7a7a7a; padding: 4px 10px; border-radius: 12px; font-size: 13px; font-weight: 500; }
.expand-arrow-btn { background: transparent; border: none; cursor: pointer; padding: 8px; border-radius: 50%; color: #9CA3AF; display: flex; align-items: center; justify-content: center; transition: all 0.2s; margin-left: 8px; }
.expand-arrow-btn:hover { background-color: rgba(255,255,255,0.5); color: #333; }
.expand-arrow-btn.is-expanded { transform: rotate(180deg); color: #FF7A00; }
.expand-arrow-btn.card { transform: rotate(270deg); }
.inline-subcategories { box-shadow: inset 0 2px 4px rgba(0,0,0,0.02); }
.inline-sub-row { padding: 16px 8px; padding-right: 66px; padding-left: 48px; color: #7a7a7a; cursor: pointer; border-bottom: 1px solid #e0e0e0; display: flex; align-items: center; justify-content:space-between; transition: background 0.2s; }
.inline-sub-row:hover { background-color: #f9f9f9; color: #FF7A00; }
.inline-sub-row:hover span { background-color: #FFffff; }
.inline-empty { padding: 12px 20px 12px 54px; font-size: 13px; color: #888; background: #dddddd; }

.sub-view-card { background-color: #F9F9F9; border-radius: 16px; padding: 20px 24px; margin-bottom: 20px; }
.header-card { display: flex; justify-content: space-between; align-items: center; }
.header-card-actions { display: flex; gap: 8px; }
.list-card { padding-bottom: 12px; }
.card-left { display: flex; align-items: center; gap: 12px; }
.icon-orange { color: #FF7A00; }
.card-title { font-size: 20px; font-weight: 600; color: #374151; margin: 0; }
.card-subtitle { font-size: 16px; font-weight: 600; color: #4B5563; margin: 0; }
.icon-btn { background: none; border: none; cursor: pointer; padding: 6px; color: #9CA3AF; transition: color 0.2s; border-radius: 6px; }
.icon-btn:hover { color: #374151; background: rgba(0,0,0,0.05); }
.icon-btn.danger { align-self: flex-end; display: flex; align-items: center; gap: 8px; background-color: #f9f9f9; border: none; border-radius: 16px; color: #E63946; transition: color 0.2s; margin-bottom: 20px; font-size: 16px; font-weight: 400; padding: 12px 18px; }
.icon-btn.danger:hover { color: #EF4444; background: rgba(239, 68, 68, 0.1); }
.icon-btn.danger.card { margin: 0;}

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

.card-list-item { display: flex; justify-content: space-between; align-items: center; padding: 16px 0; border-bottom: 1px solid #E5E7EB; cursor: pointer; transition: padding-left 0.2s; }
.card-list-item:last-child { border-bottom: none; }
.card-list-item:hover { padding-left: 8px; }
.card-list-item:hover .sub-text { color: #111827; }
.card-list-item.product-item { display: flex; justify-content: space-between; align-items: center; padding: 16px 0; border-bottom: 1px solid #E5E7EB; cursor: pointer; transition: padding-left 0.2s; }
.card-list-item.product-item:last-child { border-bottom: none; }
.rat-pri-del { display: flex; justify-content: space-between; align-items: center; gap: 12px; width: 50%; }
.sub-text { font-size: 14px; color: #4B5563; }
.delete-icon-btn { background: none; border: none; cursor: pointer; color: #9CA3AF; padding: 4px; transition: color 0.2s; }
.delete-icon-btn:hover { color: #EF4444; }
.card-empty { padding: 30px; text-align: center; color: #9CA3AF; font-size: 14px; }
.card-footer { padding-top: 20px; text-align: center; }
.show-more-text { color: #FF7A00; font-size: 14px; cursor: pointer; font-weight: 500; }
.show-more-text:hover { text-decoration: underline; }

.product-item { display: grid; grid-template-columns: 1fr 80px 100px 40px; gap: 16px; align-items: center; }
.item-main-text { font-weight: 500; color: #1F2937; }
.prod-col-rating { display: flex; align-items: center; gap: 4px; font-size: 14px; color: #4B5563; font-weight: 500; }
.prod-col-price { text-align: right; }
.price-val { font-weight: 600; color: #7a7a7a; font-size: 18px; }
.prod-col-action { display: flex; justify-content: flex-end; }
.active-filters { display: flex; gap: 8px; margin-bottom: 15px; flex-wrap: wrap; }
.card-filters-row { margin-bottom: 12px; display: flex; flex-wrap: wrap; gap: 8px; }
.filter-tag { display: inline-flex; align-items: center; gap: 6px; background: #FFF7ED; color: #FF7A00; padding: 6px 12px; border-radius: 20px; font-size: 13px; font-weight: 500; }
.tag-remove { background: none; border: none; color: #FF7A00; font-size: 16px; cursor: pointer; }

.gray-input { background-color: #F9F9F9; border: none; border-radius: 16px; padding: 12px 24px; font-size: 14px; color: #333; outline: none; }
.btn-gray { background: #F9FAFB; color: #374151; border: none; padding: 12px 24px; border-radius: 12px; font-weight: 500; cursor: pointer; font-size: 14px; }
.btn-gray:hover { background: #F3F4F6; }
.btn-orange { background: #FF7A00; color: white; border: none; padding: 12px 24px; border-radius: 12px; font-weight: 600; cursor: pointer; font-size: 14px; }
.btn-orange:hover { background: #E56D00; }
.btn-red { background: #EF4444; color: white; border: none; padding: 12px 24px; border-radius: 12px; font-weight: 600; cursor: pointer; font-size: 14px; }
.btn-red:hover { background: #DC2626; }
.mb-4 { margin-bottom: 16px; width: -webkit-fill-available; }

.blur-overlay { background: rgba(0, 0, 0, 0.2); backdrop-filter: blur(2px); z-index: 1000; position: fixed; top: 0; left: 0; width: 100%; height: 100%; display: flex; justify-content: center; align-items: center;}
.centered-modal { display: flex; flex-direction: column; gap: 32px; width: 400px; padding: 40px; text-align: center; border-radius: 40px; box-shadow: 0 20px 40px rgba(0,0,0,0.08); background: white;}
.modal-title-center { font-size: 24px; margin-bottom: 10px; font-weight: 600; }
.modal-hint-center { font-size: 14px; color: #6B7280; margin-bottom: 24px; }
.modal-text { font-size: 16px; color: #6B7280; margin-bottom: 20px; line-height: 1.5; }
.modal-actions-center { display: flex; justify-content: center; gap: 12px; width: 100%; }
.modal-actions-center button { width: 50%; }
.delete-icon-wrapper { width: 60px; height: 60px; border-radius: 50%; background: #FEE2E2; display: flex; justify-content: center; align-items: center; margin: 0 auto 20px auto; }
.modal-overlay { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0, 0, 0, 0.4); display: flex; justify-content: center; align-items: center; z-index: 10001; }
.modal-content:not(.centered-modal) { background: white; padding: 30px; border-radius: 16px; width: 400px; max-width: 90%; }
.full-input { width: 100%; padding: 12px 14px; border: 1px solid #E0E0E0; border-radius: 8px; font-size: 14px; }
.btn-primary { background: #FF7A00; color: white; border: none; padding: 10px 20px; border-radius: 8px; cursor: pointer; }
.btn-secondary { background: #F3F4F6; color: #333; border: none; padding: 10px 20px; border-radius: 8px; cursor: pointer; }
.modal-actions { margin-top: 25px; display: flex; gap: 10px; justify-content: flex-end; }

.sidebar-overlay { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0, 0, 0, 0.3); z-index: 10000; opacity: 0; pointer-events: none; transition: opacity 0.3s; }
.sidebar-overlay.open { opacity: 1; pointer-events: auto; }
.sidebar-panel { position: absolute; top: 0; right: 0; width: 480px; height: 100%; background: white; padding: 40px; box-shadow: -10px 0 30px rgba(0,0,0,0.05); transform: translateX(100%); transition: transform 0.3s; display: flex; flex-direction: column; box-sizing: border-box; gap: 32px; }
.sidebar-overlay.open .sidebar-panel { transform: translateX(0); }
.sidebar-title { font-size: 24px; margin: 0; font-weight: 600; text-align: center;}
.sidebar-actions { display: flex; gap: 12px; width: 100%; }
.sidebar-actions button { width: 50%; }

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

@media (max-width: 768px) {
  .admin-wrapper { padding: 15px 20px; }
  .header-right-group, .card-header-row { flex-wrap: wrap; }
  .sidebar-panel { width: 100%; }
  .product-item { grid-template-columns: 1fr auto; grid-template-rows: auto auto; gap: 8px; }
  .prod-col-rating, .prod-col-price, .prod-col-action { grid-row: 2; }
  .prod-col-name { grid-column: 1 / -1; }
  .sort-btn-text { display: none; }
}

.stars-wrapper {
  display: flex;
  gap: 2px;
  align-items: center;
}

.star-icon {
  filter: drop-shadow(0px 1px 1px rgba(0,0,0,0.05));
}

.rating-val-orange {
  margin-left: 8px;
  font-weight: 700;
  color: #FF7A00;
  font-size: 14px;
}

.show-more-text {
  cursor: pointer;
  color: #FF7A00;
  font-weight: 500;
  transition: opacity 0.2s;
}

.show-more-text:hover {
  opacity: 0.8;
  text-decoration: underline;
}

.show-more-text {
  display: inline-block;
  padding: 8px 16px;
  user-select: none;
}

.clear-circle {
  cursor: pointer;
}
</style>