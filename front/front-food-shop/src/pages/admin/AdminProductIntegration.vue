<template>
  <AdminLayout>
    <div class="content-wrapper">
      <div class="page-header">
        <div class="title-section">
          <div class="icon-box orange-box">
            <img src="../../assets/box.svg" alt="Logo" class="icon-orange" />
          </div>
          <h1>Импорт и экспорт товаров</h1>
        </div>
      </div>

      <div class="integration-grid">

        <!-- ══════════ ЭКСПОРТ ══════════ -->
        <div class="integration-card">
          <div class="card-icon-header">
            <div class="card-icon export-icon">↓</div>
            <div>
              <h2 class="card-title">Экспорт</h2>
              <p class="card-subtitle">Скачать товары в формате JSON</p>
            </div>
          </div>

          <div class="export-options">

            <!-- Экспорт всех товаров -->
            <div class="option-block">
              <div class="option-header">
                <span class="option-title">Все товары</span>
                <span class="option-hint">Экспортировать весь каталог одним файлом</span>
              </div>
              <button
                class="btn-action export-btn"
                :disabled="exportListLoading"
                @click="handleExportList"
              >
                <span v-if="exportListLoading" class="btn-spinner"></span>
                <span v-else>Скачать все (products.json)</span>
              </button>
            </div>

            <div class="divider"></div>

            <!-- Экспорт одного товара по ID -->
            <div class="option-block">
              <div class="option-header">
                <span class="option-title">Один товар по ID</span>
                <span class="option-hint">Введите идентификатор товара или выберите из списка</span>
              </div>

              <div class="select-or-input">
                <div class="custom-select-container" v-click-outside="() => isProductSelectorOpen = false">
                  <div
                    class="select-header"
                    :class="{ 'is-open': isProductSelectorOpen }"
                    @click="isProductSelectorOpen = !isProductSelectorOpen"
                  >
                    <span>{{ selectedProductLabel }}</span>
                    <img src="../../assets/arrow-down.svg" />
                  </div>
                  <div v-if="isProductSelectorOpen" class="select-body">
                    <div class="select-search-wrapper">
                      <img src="../../assets/search-normal.svg" alt="search" class="search-icon-sm" />
                      <input
                        type="text"
                        v-model="productSearchQuery"
                        placeholder="Поиск по названию..."
                        class="select-search-input"
                      />
                    </div>
                    <div class="select-list">
                      <div
                        v-for="p in filteredProducts"
                        :key="p.id"
                        class="select-option"
                        :class="{ active: exportProductId === p.id }"
                        @click="selectExportProduct(p)"
                      >
                        <div class="radio-indicator" :class="{ selected: exportProductId === p.id }"></div>
                        <span class="option-text">{{ p.name }}</span>
                        <span v-if="!p.isVisible" class="inline-hidden-badge">Скрыт</span>
                      </div>
                      <div v-if="filteredProducts.length === 0" class="empty-select">Ничего не найдено</div>
                    </div>
                  </div>
                </div>
              </div>

              <button
                class="btn-action export-btn"
                :disabled="!exportProductId || exportOneLoading"
                @click="handleExportOne"
              >
                <span v-if="exportOneLoading" class="btn-spinner"></span>
                <span v-else>Скачать товар (product_id.json)</span>
              </button>
            </div>
          </div>
        </div>

        <!-- ══════════ ИМПОРТ ══════════ -->
        <div class="integration-card">
          <div class="card-icon-header">
            <div class="card-icon import-icon">↑</div>
            <div>
              <h2 class="card-title">Импорт</h2>
              <p class="card-subtitle">Загрузить товары из JSON-файла</p>
            </div>
          </div>

          <!-- Таб-переключатель: один / список -->
          <div class="import-tabs">
            <button
              class="import-tab"
              :class="{ active: importMode === 'one' }"
              @click="importMode = 'one'; resetImport()"
            >
              Один товар
            </button>
            <button
              class="import-tab"
              :class="{ active: importMode === 'list' }"
              @click="importMode = 'list'; resetImport()"
            >
              Список товаров
            </button>
          </div>

          <p class="import-hint">
            <template v-if="importMode === 'one'">
              Загрузите JSON-файл с одним товаром (полученный через экспорт "Один товар").
              Если товар с таким ID уже существует — он будет обновлён.
            </template>
            <template v-else>
              Загрузите JSON-файл со списком товаров (полученный через экспорт "Все товары").
              Каждый товар будет создан или обновлён по ID.
            </template>
          </p>

          <!-- Зона дропа файла -->
          <div
            class="drop-zone"
            :class="{
              'drop-zone-active': isDragOver,
              'drop-zone-has-file': importFile,
              'drop-zone-error': importError
            }"
            @dragover.prevent="isDragOver = true"
            @dragleave.prevent="isDragOver = false"
            @drop.prevent="handleDrop"
            @click="openImportFileDialog"
          >
            <template v-if="!importFile">
              <div class="drop-icon">📂</div>
              <p class="drop-text">Перетащите JSON-файл сюда или нажмите для выбора</p>
              <span class="drop-hint">Только .json файлы</span>
            </template>
            <template v-else>
              <div class="drop-icon">📄</div>
              <p class="drop-text drop-filename">{{ importFile.name }}</p>
              <span class="drop-hint">{{ formatFileSize(importFile.size) }}</span>
              <button class="remove-file-btn" @click.stop="resetImport">×</button>
            </template>
          </div>

          <div v-if="importError" class="import-error-msg">{{ importError }}</div>

          <div v-if="importSuccess" class="import-success-msg">
            ✅ {{ importSuccess }}
          </div>

          <button
            class="btn-action import-btn"
            :disabled="!importFile || importLoading"
            @click="handleImport"
          >
            <span v-if="importLoading" class="btn-spinner"></span>
            <span v-else>Загрузить файл</span>
          </button>

          <input
            ref="importFileInputRef"
            type="file"
            accept=".json,application/json"
            style="display: none;"
            @change="handleImportFileChange"
          />
        </div>

      </div>
    </div>
  </AdminLayout>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import AdminLayout from './AdminLayout.vue';
import { adminProductApi } from '@/services/api.js';

// ── Состояние ───────────────────────────────────────────────
const products = ref([]);
const isLoadingProducts = ref(false);

// Экспорт
const exportProductId = ref(null);
const exportProductName = ref('');
const exportListLoading = ref(false);
const exportOneLoading = ref(false);
const isProductSelectorOpen = ref(false);
const productSearchQuery = ref('');

// Импорт
const importMode = ref('one');
const importFile = ref(null);
const importLoading = ref(false);
const importError = ref('');
const importSuccess = ref('');
const isDragOver = ref(false);
const importFileInputRef = ref(null);

// ── Загрузка списка продуктов для селектора ─────────────────
const loadProducts = async () => {
  isLoadingProducts.value = true;
  try {
    const data = await adminProductApi.get(1, 200);
    products.value = data.productList || [];
  } catch (e) {
    console.error('Ошибка загрузки товаров:', e.message);
  } finally {
    isLoadingProducts.value = false;
  }
};

onMounted(() => {
  loadProducts();
});

// ── Computed ─────────────────────────────────────────────────
const filteredProducts = computed(() => {
  const q = productSearchQuery.value.toLowerCase().trim();
  if (!q) return products.value;
  return products.value.filter(p => p.name.toLowerCase().includes(q));
});

const selectedProductLabel = computed(() => {
  if (!exportProductId.value) return 'Выберите товар из списка';
  return exportProductName.value || exportProductId.value;
});

// ── Экспорт ──────────────────────────────────────────────────
const selectExportProduct = (product) => {
  exportProductId.value = product.id;
  exportProductName.value = product.name;
  isProductSelectorOpen.value = false;
  productSearchQuery.value = '';
};

const downloadBlob = (response, fallbackName) => {
  const contentDisposition = response.headers?.['content-disposition'] || '';
  const filenameMatch = contentDisposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/);
  const filename = filenameMatch ? filenameMatch[1].replace(/['"]/g, '') : fallbackName;

  const url = URL.createObjectURL(response.data);
  const link = document.createElement('a');
  link.href = url;
  link.download = filename;
  link.click();
  URL.revokeObjectURL(url);
};

const handleExportList = async () => {
  exportListLoading.value = true;
  try {
    const response = await adminProductApi.exportList();
    downloadBlob(response, 'products.json');
  } catch (e) {
    alert(e.message);
  } finally {
    exportListLoading.value = false;
  }
};

const handleExportOne = async () => {
  if (!exportProductId.value) return;
  exportOneLoading.value = true;
  try {
    const response = await adminProductApi.exportById(exportProductId.value);
    downloadBlob(response, `product_${exportProductId.value}.json`);
  } catch (e) {
    alert(e.message);
  } finally {
    exportOneLoading.value = false;
  }
};

// ── Импорт ───────────────────────────────────────────────────
const resetImport = () => {
  importFile.value = null;
  importError.value = '';
  importSuccess.value = '';
  isDragOver.value = false;
};

const openImportFileDialog = () => {
  importFileInputRef.value?.click();
};

const handleImportFileChange = (e) => {
  const file = e.target.files[0];
  if (file) setImportFile(file);
  e.target.value = '';
};

const handleDrop = (e) => {
  isDragOver.value = false;
  const file = e.dataTransfer.files[0];
  if (file && (file.type === 'application/json' || file.name.endsWith('.json'))) {
    setImportFile(file);
  } else {
    importError.value = 'Пожалуйста, загрузите файл формата .json';
  }
};

const setImportFile = (file) => {
  importFile.value = file;
  importError.value = '';
  importSuccess.value = '';
};

const handleImport = async () => {
  if (!importFile.value) return;
  importLoading.value = true;
  importError.value = '';
  importSuccess.value = '';
  try {
    if (importMode.value === 'one') {
      await adminProductApi.importOne(importFile.value);
      importSuccess.value = 'Товар успешно импортирован (создан или обновлён)';
    } else {
      await adminProductApi.importList(importFile.value);
      importSuccess.value = 'Список товаров успешно импортирован';
    }
    importFile.value = null;
  } catch (e) {
    importError.value = e.message || 'Произошла ошибка при импорте. Проверьте формат файла.';
  } finally {
    importLoading.value = false;
  }
};

const formatFileSize = (bytes) => {
  if (bytes < 1024) return `${bytes} Б`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} КБ`;
  return `${(bytes / 1024 / 1024).toFixed(1)} МБ`;
};

// ── Директива click-outside ──────────────────────────────────
const vClickOutside = {
  mounted(el, binding) {
    el.clickOutsideEvent = (event) => {
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
</script>

<style scoped>
@import './admin.css';

.content-wrapper {
  padding: 20px 40px;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 32px;
}

.title-section {
  display: flex;
  align-items: center;
  gap: 14px;
}

.icon-box {
  width: 40px;
  height: 40px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.orange-box {
  background: #fff3e6;
}

.icon-orange {
  width: 22px;
  height: 22px;
  filter: invert(47%) sepia(97%) saturate(500%) hue-rotate(354deg) brightness(103%) contrast(101%);
}

h1 {
  font-size: 24px;
  font-weight: 700;
  color: #222;
  margin: 0;
}

/* ── Grid ─────────────────────────────────────────────── */
.integration-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 28px;
}

@media (max-width: 900px) {
  .integration-grid {
    grid-template-columns: 1fr;
  }
}

/* ── Card ─────────────────────────────────────────────── */
.integration-card {
  background: #fff;
  border-radius: 16px;
  padding: 28px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.04);
  border: 1px solid #eee;
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.card-icon-header {
  display: flex;
  align-items: center;
  gap: 16px;
}

.card-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  font-weight: 800;
  flex-shrink: 0;
}

.export-icon {
  background: #fff3e6;
  color: #FF7A00;
}

.import-icon {
  background: #e8f5e9;
  color: #4caf50;
}

.card-title {
  font-size: 20px;
  font-weight: 700;
  margin: 0;
  color: #222;
}

.card-subtitle {
  font-size: 13px;
  color: #888;
  margin: 4px 0 0;
}

/* ── Export options ───────────────────────────────────── */
.export-options {
  display: flex;
  flex-direction: column;
  gap: 0;
}

.option-block {
  display: flex;
  flex-direction: column;
  gap: 12px;
  padding: 16px 0;
}

.option-header {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.option-title {
  font-size: 15px;
  font-weight: 600;
  color: #333;
}

.option-hint {
  font-size: 12px;
  color: #999;
}

.divider {
  height: 1px;
  background: #f0f0f0;
}

/* ── Buttons ──────────────────────────────────────────── */
.btn-action {
  padding: 11px 20px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
  border: none;
  cursor: pointer;
  transition: background 0.2s, opacity 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

.btn-action:disabled {
  opacity: 0.45;
  cursor: not-allowed;
}

.export-btn {
  background: #FF7A00;
  color: #fff;
}

.export-btn:not(:disabled):hover {
  background: #e06900;
}

.import-btn {
  background: #4caf50;
  color: #fff;
}

.import-btn:not(:disabled):hover {
  background: #43a047;
}

/* ── Spinner ──────────────────────────────────────────── */
.btn-spinner {
  width: 16px;
  height: 16px;
  border: 2px solid rgba(255,255,255,0.4);
  border-top-color: #fff;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
  display: inline-block;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* ── Custom select (стиль из AdminProductAdd) ─────────── */
.custom-select-container {
  position: relative;
}

.select-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 10px 14px;
  border: 1px solid #ddd;
  border-radius: 8px;
  cursor: pointer;
  font-size: 14px;
  background: #fafafa;
  user-select: none;
  transition: border-color 0.2s;
}

.select-header.is-open {
  border-color: #FF7A00;
  background: #fff;
}

.select-header img {
  width: 16px;
  opacity: 0.5;
  transition: transform 0.2s;
}

.select-header.is-open img {
  transform: rotate(180deg);
}

.select-body {
  position: absolute;
  top: calc(100% + 4px);
  left: 0;
  width: 100%;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  box-shadow: 0 8px 24px rgba(0,0,0,0.10);
  z-index: 100;
  overflow: hidden;
}

.select-search-wrapper {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  border-bottom: 1px solid #f0f0f0;
}

.search-icon-sm {
  width: 16px;
  opacity: 0.4;
  flex-shrink: 0;
}

.select-search-input {
  flex: 1;
  border: none;
  outline: none;
  font-size: 13px;
  background: transparent;
}

.select-list {
  max-height: 220px;
  overflow-y: auto;
}

.select-option {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 10px 14px;
  cursor: pointer;
  font-size: 14px;
  transition: background 0.15s;
}

.select-option:hover,
.select-option.active {
  background: #fff8f2;
}

.radio-indicator {
  width: 16px;
  height: 16px;
  border-radius: 50%;
  border: 2px solid #ddd;
  flex-shrink: 0;
  transition: border-color 0.15s, background 0.15s;
}

.radio-indicator.selected {
  border-color: #FF7A00;
  background: #FF7A00;
}

.option-text {
  flex: 1;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.empty-select {
  padding: 12px 16px;
  font-size: 13px;
  color: #bbb;
  text-align: center;
}

.inline-hidden-badge {
  font-size: 10px;
  color: #aaa;
  background: #f5f5f5;
  border: 1px solid #e0e0e0;
  border-radius: 3px;
  padding: 1px 5px;
  flex-shrink: 0;
}

/* ── Import tabs ──────────────────────────────────────── */
.import-tabs {
  display: flex;
  gap: 0;
  border: 1px solid #eee;
  border-radius: 8px;
  overflow: hidden;
}

.import-tab {
  flex: 1;
  padding: 10px;
  font-size: 14px;
  font-weight: 500;
  border: none;
  background: #f9f9f9;
  color: #777;
  cursor: pointer;
  transition: background 0.2s, color 0.2s;
}

.import-tab.active {
  background: #FF7A00;
  color: #fff;
  font-weight: 600;
}

.import-hint {
  font-size: 13px;
  color: #888;
  line-height: 1.55;
  margin: 0;
}

/* ── Drop zone ────────────────────────────────────────── */
.drop-zone {
  border: 2px dashed #ddd;
  border-radius: 12px;
  padding: 32px 20px;
  text-align: center;
  cursor: pointer;
  transition: border-color 0.2s, background 0.2s;
  position: relative;
  background: #fafafa;
}

.drop-zone:hover,
.drop-zone-active {
  border-color: #FF7A00;
  background: #fff8f2;
}

.drop-zone-has-file {
  border-color: #4caf50;
  background: #f1faf1;
}

.drop-zone-error {
  border-color: #ff4d4f;
  background: #fff1f0;
}

.drop-icon {
  font-size: 36px;
  margin-bottom: 10px;
}

.drop-text {
  font-size: 14px;
  color: #555;
  margin: 0 0 4px;
}

.drop-filename {
  font-weight: 600;
  color: #333;
}

.drop-hint {
  font-size: 12px;
  color: #aaa;
}

.remove-file-btn {
  position: absolute;
  top: 10px;
  right: 12px;
  background: none;
  border: none;
  font-size: 22px;
  color: #bbb;
  cursor: pointer;
  line-height: 1;
  padding: 0;
}

.remove-file-btn:hover {
  color: #ff4d4f;
}

/* ── Messages ─────────────────────────────────────────── */
.import-error-msg {
  font-size: 13px;
  color: #ff4d4f;
  background: #fff1f0;
  border: 1px solid #ffccc7;
  border-radius: 8px;
  padding: 10px 14px;
}

.import-success-msg {
  font-size: 13px;
  color: #2e7d32;
  background: #f1faf1;
  border: 1px solid #c8e6c9;
  border-radius: 8px;
  padding: 10px 14px;
}
</style>