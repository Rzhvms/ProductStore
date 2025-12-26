<template>
  <AdminLayout>
    <template #default>
      <div class="admin-content gray-panel">

        <!-- Верхняя панель: поиск + кнопка добавления -->
        <div class="top-panel">
          <input 
            type="text" 
            placeholder="Поиск категории или подкатегории..." 
            v-model="searchTerm"
          />
          <button class="add-category-btn" @click="showAddCategoryDialog = true">Добавить категорию</button>
        </div>

        <!-- Таблица категорий -->
        <div class="users-grid">
          <div class="grid-header-row">
            <div class="header-cell">#</div>
            <div class="header-cell">Категория</div>
            <div class="header-cell">Подкатегории</div>
            <div class="header-cell">Действия</div>
          </div>

          <div 
            class="grid-data-row" 
            v-for="(cat, index) in filteredCategories" 
            :key="cat.id"
          >
            <div class="data-cell">{{ index + 1 }}</div>

            <!-- Категория -->
            <div class="data-cell col-name">
              <div v-if="editingCategory === cat.id">
                <input v-model="newCategoryName" @keyup.enter="confirmEditCategory(cat)" />
                <button class="small-btn" @click="confirmEditCategory(cat)">Сохранить</button>
                <button class="small-btn" @click="cancelEditCategory">Отмена</button>
              </div>
              <div v-else class="category-row">
                <span>{{ cat.name }}</span>
                <div class="category-actions">
                  <button class="small-btn" @click="startEditCategory(cat)">Редактировать</button>
                  <button class="small-btn delete-btn" @click="confirmDeleteCategory(cat)">✕</button>
                </div>
              </div>
            </div>

            <!-- Подкатегории -->
            <div class="data-cell col-email">
              <ul class="sub-list">
                <li v-for="(sub, subIndex) in cat.subCategories" :key="sub">
                  <span class="sub-name">{{ sub }}</span>
                  <button class="small-btn delete-btn" @click="confirmDeleteSub(cat.id, subIndex, sub)">✕</button>
                </li>
                <li v-if="addingSubCategory === cat.id" class="sub-input-row">
                  <input v-model="newSubName" placeholder="Название подкатегории" @keyup.enter="confirmAddSub(cat)" />
                  <button class="small-btn" @click="confirmAddSub(cat)">Добавить</button>
                  <button class="small-btn" @click="cancelAddSub">Отмена</button>
                </li>
              </ul>
            </div>

            <div class="data-cell">
              <button class="order-btn" v-if="addingSubCategory !== cat.id" @click="startAddSub(cat)">
                Добавить подкатегорию
              </button>
            </div>
          </div>
        </div>

        <!-- Модалка удаления -->
        <div v-if="showDeleteDialog" class="modal-overlay">
          <div class="modal-content">
            <p>Удалить "{{ deleteTarget.name }}"?</p>
            <div class="modal-actions">
              <button class="small-btn" @click="performDelete">Да</button>
              <button class="small-btn" @click="cancelDelete">Отмена</button>
            </div>
          </div>
        </div>

        <!-- Модалка добавления категории -->
        <div v-if="showAddCategoryDialog" class="modal-overlay">
          <div class="modal-content">
            <h3>Добавить категорию</h3>
            <input v-model="newCategoryName" placeholder="Название категории" />

            <h4>Подкатегории</h4>
            <ul class="sub-list">
              <li v-for="(sub, index) in newCategorySub" :key="index">
                <span class="sub-name">{{ sub }}</span>
                <button class="small-btn delete-btn" @click="newCategorySub.splice(index, 1)">✕</button>
              </li>
              <li class="sub-input-row">
                <input v-model="newSubName" placeholder="Название подкатегории" @keyup.enter="addNewCategorySub" />
                <button class="small-btn" @click="addNewCategorySub">Добавить</button>
              </li>
            </ul>

            <div class="modal-actions">
              <button class="small-btn" @click="addCategory">Сохранить</button>
              <button class="small-btn" @click="cancelAddCategory">Отмена</button>
            </div>
          </div>
        </div>

      </div>
    </template>
  </AdminLayout>
</template>

<script setup>
import { ref, computed } from 'vue';
import AdminLayout from './AdminLayout.vue';
import './admin.css'; 

const searchTerm = ref('');
const editingCategory = ref(null);
const newCategoryName = ref('');
const addingSubCategory = ref(null);
const newSubName = ref('');

const parentCategories = ref([
  { id: 1, name: 'Электроника', subCategories: ['Телефоны', 'Ноутбуки'] },
  { id: 2, name: 'Одежда', subCategories: ['Футболки', 'Штаны'] },
  { id: 3, name: 'Книги', subCategories: ['Фантастика', 'Наука'] },
]);

const showDeleteDialog = ref(false);
const deleteTarget = ref({ type: '', catId: null, subIndex: null, name: '' });

const showAddCategoryDialog = ref(false);
const newCategorySub = ref([]);

const filteredCategories = computed(() => {
  if (!searchTerm.value) return parentCategories.value;
  return parentCategories.value.filter(cat =>
    cat.name.toLowerCase().includes(searchTerm.value.toLowerCase()) ||
    cat.subCategories.some(sub => sub.toLowerCase().includes(searchTerm.value.toLowerCase()))
  );
});

// Категории
function startEditCategory(cat) {
  editingCategory.value = cat.id;
  newCategoryName.value = cat.name;
}
function confirmEditCategory(cat) {
  if (newCategoryName.value.trim()) cat.name = newCategoryName.value.trim();
  editingCategory.value = null;
}
function cancelEditCategory() {
  editingCategory.value = null;
}

// Удаление
function confirmDeleteCategory(cat) {
  deleteTarget.value = { type: 'category', catId: cat.id, name: cat.name };
  showDeleteDialog.value = true;
}
function confirmDeleteSub(catId, subIndex, subName) {
  deleteTarget.value = { type: 'sub', catId, subIndex, name: subName };
  showDeleteDialog.value = true;
}
function performDelete() {
  const target = deleteTarget.value;
  if (target.type === 'category') {
    const index = parentCategories.value.findIndex(c => c.id === target.catId);
    if (index > -1) parentCategories.value.splice(index, 1);
  } else if (target.type === 'sub') {
    const cat = parentCategories.value.find(c => c.id === target.catId);
    if (cat) cat.subCategories.splice(target.subIndex, 1);
  }
  showDeleteDialog.value = false;
}
function cancelDelete() {
  showDeleteDialog.value = false;
}

// Подкатегории
function startAddSub(cat) {
  addingSubCategory.value = cat.id;
  newSubName.value = '';
}
function confirmAddSub(cat) {
  if (!newSubName.value.trim()) return;
  const category = parentCategories.value.find(c => c.id === cat.id);
  if (category) category.subCategories.push(newSubName.value.trim());
  addingSubCategory.value = null;
}
function cancelAddSub() {
  addingSubCategory.value = null;
}

// Подкатегории в модалке добавления категории
function addNewCategorySub() {
  if (!newSubName.value.trim()) return;
  newCategorySub.value.push(newSubName.value.trim());
  newSubName.value = '';
}

// Добавление новой категории
function addCategory() {
  if (!newCategoryName.value.trim()) return;
  const newId = Math.max(...parentCategories.value.map(c => c.id), 0) + 1;
  parentCategories.value.push({
    id: newId,
    name: newCategoryName.value.trim(),
    subCategories: [...newCategorySub.value],
  });
  newCategoryName.value = '';
  newCategorySub.value = [];
  showAddCategoryDialog.value = false;
}

function cancelAddCategory() {
  newCategoryName.value = '';
  newCategorySub.value = [];
  newSubName.value = '';
  showAddCategoryDialog.value = false;
}
</script>

<style scoped>
/* === Поверх глобальных стилей для admincategories === */

/* Верхняя панель */
.top-panel {
  display: flex !important;
  justify-content: space-between !important;
  align-items: center !important;
  margin-bottom: 25px !important;
}
.top-panel input {
  flex: 1 !important;
  padding: 10px !important;
  border-radius: 6px !important;
  border: 1px solid #ddd !important;
  font-size: 14px !important;
  margin-right: 10px !important;
}
.add-category-btn {
  padding: 10px 20px !important;
  background: #FF7A00 !important;
  color: #fff !important;
  border: none !important;
  border-radius: 6px !important;
  cursor: pointer !important;
}
.add-category-btn:hover {
  background: #e06600 !important;
}

/* Таблица: исправленная сетка */
.users-grid .grid-header-row,
.users-grid .grid-data-row {
  display: grid !important;
  grid-template-columns: 60px 2fr 3fr 1fr !important; /* действия шире */
  gap: 15px !important;
  align-items: center !important;
}
.grid-data-row {
  padding: 12px 10px !important;
  border-bottom: 1px solid #eee !important;
  background: rgba(255,255,255,0.7) !important;
  border-radius: 6px !important;
}
.header-cell {
  background: #fff !important;
  padding: 12px 10px !important;
  border-radius: 6px !important;
  font-weight: 700 !important;
  font-size: 14px !important;
  text-align: center !important;
  box-shadow: 0 2px 4px rgba(0,0,0,0.02) !important;
  text-transform: uppercase !important;
}
.data-cell {
  font-size: 14px !important;
  text-align: center !important;
}

/* Категории и подкатегории */
.category-row {
  display: flex !important;
  justify-content: space-between !important;
  align-items: center !important;
}
.category-actions {
  display: flex !important;
  gap: 8px !important;
}
.sub-list li {
  display: flex !important;
  justify-content: space-between !important;
  align-items: center !important;
  gap: 8px !important;
  margin-bottom: 5px !important;
  font-size: 14px !important;
}
.sub-name {
  flex-grow: 1 !important;
}
.sub-input-row {
  display: flex !important;
  gap: 8px !important;
  margin-top: 5px !important;
}
.sub-input-row input {
  flex: 1 !important;
  padding: 6px 10px !important;
  border-radius: 4px !important;
  border: 1px solid #ddd !important;
}
.sub-input-row .small-btn {
  padding: 4px 8px !important;
}
.small-btn {
  background: #fff !important;
  border: 1px solid #ddd !important;
  border-radius: 4px !important;
  padding: 2px 6px !important;
  font-size: 12px !important;
  cursor: pointer !important;
}
.small-btn:hover {
  background: #ff7a00 !important;
  color: #fff !important;
}
.order-btn {
  background: #fff !important;
  border: none !important;
  padding: 8px 15px !important;
  border-radius: 20px !important;
  font-weight: 700 !important;
  font-size: 13px !important;
  cursor: pointer !important;
  box-shadow: 0 2px 5px rgba(0,0,0,0.05) !important;
  width: 100% !important;
}
.order-btn:hover {
  background: #FF7A00 !important;
  color: #fff !important;
}

/* Модалки */
.modal-overlay {
  position: fixed !important;
  top: 0 !important;
  left: 0 !important;
  width: 100% !important;
  height: 100% !important;
  background: rgba(0,0,0,0.3) !important;
  display: flex !important;
  justify-content: center !important;
  align-items: center !important;
  z-index: 1000 !important;
}
.modal-content {
  background: #fff !important;
  padding: 25px 30px !important;
  border-radius: 12px !important;
  text-align: center !important;
  max-width: 500px !important;
  width: 100% !important;
}
.modal-content h3 {
  margin-bottom: 15px !important;
}
.modal-content h4 {
  margin: 10px 0 !important;
}
.modal-actions {
  margin-top: 20px !important;
}
.modal-actions .small-btn {
  margin: 0 8px !important;
}

/* Адаптив */
@media (max-width: 1100px) {
  .top-panel {
    flex-direction: column !important;
    align-items: stretch !important;
  }
  .top-panel input {
    margin-bottom: 10px !important;
    margin-right: 0 !important;
  }
  .users-grid .grid-header-row,
  .users-grid .grid-data-row {
    grid-template-columns: 50px 1fr 1fr 1fr !important;
    font-size: 12px !important;
  }
}
</style>
