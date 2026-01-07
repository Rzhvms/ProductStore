<template>
  <AdminLayout>
    <div class="product-page">
        <!-- Верхняя панель: Переключатель режимов -->
        <div class="top-bar">
        <div class="mode-switcher">
            <button 
            :class="{ active: !isEditMode }" 
            @click="isEditMode = false"
            >
            Режим просмотра
            </button>
            <button 
            :class="{ active: isEditMode }" 
            @click="isEditMode = true"
            >
            Режим редактирования
            </button>
        </div>
        
        <div class="top-actions" v-if="!isEditMode">
            <button class="btn-outline"><img src="../../assets/percentage-circle.svg"> Добавить товар в акцию</button>
            <button class="btn-outline"><img src="../../assets/chart.svg"> Статистика</button>
            <button class="btn-text text-red"><img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/> Удалить товар</button>
        </div>
        <div class="top-actions" v-else>
            <button class="btn-text text-red"><img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/> Удалить товар</button>
        </div>
        </div>

        <div class="main-content">
        
        <!-- ЛЕВАЯ КОЛОНКА: Изображения -->
        <div class="left-col">
    
          <!-- === РЕЖИМ ПРОСМОТРА (Галерея) === -->
          <div v-if="!isEditMode" class="view-gallery">
              <!-- Колонка миниатюр -->
              <div class="thumbnails-col">
                  <div 
                      v-for="(img, index) in form.images" 
                      :key="index"
                      class="thumb-item"
                      :class="{ active: activeImageIndex === index }"
                      @click="activeImageIndex = index"
                  >
                      <!-- Здесь может быть <img>, пока ставим заглушку -->
                  </div>
              </div>

              <!-- Большое изображение -->
              <div class="main-image-display">
                  <!-- Здесь может быть <img>, пока ставим заглушку -->
                  <div class="main-placeholder"></div>
              </div>
          </div>


          <!-- === РЕЖИМ РЕДАКТИРОВАНИЯ (Сетка карточек) === -->
          <div v-else class="edit-gallery-grid">
              <div 
                  v-for="(img, index) in form.images" 
                  :key="index" 
                  class="edit-card"
              >
                  <!-- Бейджи -->
                  <span class="badge orange" v-if="index === 0">обложка</span>
                  <span class="badge grey" v-else>карусель</span>
                  
                  <!-- Удаление -->
                  <button class="btn-delete-card" @click="removeImage(index)">
                      <img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/>
                  </button>

                  <!-- Плейсхолдер картинки -->
                  <div class="card-placeholder"></div>
              </div>

              <!-- Кнопка добавления -->
              <div class="edit-card add-new" @click="addImage">
                  <span class="plus-icon">+</span>
              </div>
          </div>

      </div>

        <div class="right-col">
          
          <!-- Блок 1: Название и категория -->
          <div class="card info-card">
          
              <!-- Заголовок / Инпут названия -->
              <div v-if="isEditMode" class="form-group">
                  <label>Название и граммовка:</label>
                  <div class="row-inputs">
                      <input v-model="form.title" type="text" placeholder="Название товара" class="input-main">
                      
                      <input 
                          :value="form.weight" 
                          @input="validateInput(form, 'weight', $event)" 
                          placeholder="1000" 
                          class="input-short"
                      >
                  <div class="control-box dropdown-wrapper">
                    <button 
                      class="sort-btn-styled" 
                      :class="{ 'is-active': showUnitMenu }"
                      @click.stop="showUnitMenu = !showUnitMenu"
                    >
                      <span>{{ selectedUnitLabel }}</span>
                      <img v-if="!showUnitMenu" src="../../assets/arrow-down.svg" />
                      <img v-else src="../../assets/arrow-down.svg" style="transform: rotate(180deg); filter: invert(100%) sepia(0%) saturate(7500%) hue-rotate(74deg) brightness(1210%) contrast(113%);" />
                    </button>
                    
                    <div v-if="showUnitMenu" class="custom-dropdown-menu sort-menu-design">
                      <template v-for="(val, key, index) in unitDict" :key="key">
                        <div 
                          class="sort-item" 
                          @click="selectUnit(key)"
                        >
                          <div 
                            class="radio-indicator" 
                            :class="{ selected: form.unit === key }"
                          ></div>
                          <span>{{ val }}</span>
                        </div>
                        <!-- Разделитель между элементами, кроме последнего -->
                        <div v-if="index < Object.keys(unitDict).length - 1" class="dd-divider"></div>
                      </template>
                    </div>
                  </div>
                      
                  </div>
              </div>
              
              <!-- Режим просмотра заголовка -->
              <div v-else>
                  <h1>{{ form.title }}, {{ form.weight }}{{ form.unit }}</h1>
                  <div class="row-rating">
                      <div class="stars-wrapper">
                          <svg v-for="star in 5" :key="star" class="star-icon" width="32" height="32" viewBox="0 0 24 24">
                              <defs>
                                  <linearGradient :id="'grad-' + (form.id || 'new') + '-' + star">
                                      <stop offset="0%" stop-color="#FF7A00" />
                                      <stop :offset="calculateOffset(form.rating, star)" stop-color="#FF7A00" />
                                      <stop :offset="calculateOffset(form.rating, star)" stop-color="#E5E7EB" />
                                      <stop offset="100%" stop-color="#E5E7EB" />
                                  </linearGradient>
                              </defs>
                              <path :fill="'url(#grad-' + (form.id || 'new') + '-' + star + ')'" d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z" />
                          </svg>  
                          <span class="rating-value" style="margin-left: 20px;">{{ form.rating || 0 }}</span>
                      </div>
                      <span class="rating-value">({{ form.rating || 0 }} отзывов)</span>
                  </div>
              </div>

              <!-- Категория (Сложный дропдаун в новом стиле) -->
<div v-if="isEditMode" class="form-group">
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
      
      <template v-for="cat in filteredCategories" :key="cat.name">
        
        <!-- Родительская категория (папка) -->
        <div 
          class="select-option parent-option" 
          @click.stop="toggleCategory(cat.name)"
        >
          <span class="option-text font-bold">{{ cat.name }}</span>
          <img 
            src="../../assets/arrow-down.svg" 
            class="cat-arrow" 
            :class="{ 'is-expanded': expandedCategory === cat.name }"
          />
        </div>
        
        <!-- Подкатегории -->
        <template v-if="expandedCategory === cat.name">
          <label 
            v-for="sub in cat.subs" 
            :key="sub" 
            class="select-option sub-option"
            @click.stop="selectSubcategory(cat.name, sub)"
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
            <span class="option-text">{{ sub }}</span>
          </label>
        </template>
        
      </template>
      
      <!-- Пусто -->
      <div v-if="filteredCategories.length === 0" class="empty-select">
        Ничего не найдено
      </div>
      
    </div>
  </div>
</div>
</div>
              <div v-else class="view-row">
                  <span style="font-size: 18px; font-weight: 600;">Категория: <span class="tag">{{  form.category }} - {{ form.subcategory }}</span></span>
              </div>

              <!-- Цена -->
              <div v-if="isEditMode" class="form-group price-group">
                  <div class="row-inputs">
                      <span class="label-in">Цена:</span>
                      <input :value="form.price" @input="validateInput(form, 'price', $event)" class="input-price">
                      
                      <span class="label-in">За:</span>
                      
                      <!-- [2] КАСТОМНЫЙ SELECT: Цена за (Шт) -->
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
                          <template v-for="(val, key, index) in priceUnitsDict " :key="key">
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
                      
                      <span class="label-in ml-auto">В наличии</span>
                      
                      <div class="counter">
                          <button @click="form.stock--">-</button>
                          <!-- Исправлен класс инпута внутри каунтера -->
                          <input :value="form.stock" @input="validateInput(form, 'stock', $event)" class="input-short-counter" style="padding: 0; ">
                          <button @click="form.stock++">+</button>
                      </div>
                      
                      <!-- [3] КАСТОМНЫЙ SELECT: Единица наличия (Килограмм) -->
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
              
              <div v-else class="price-view">
                  <h2>{{ form.price }} ₽ / {{ priceUnitsDict[form.priceUnit] }}</h2>
                  <span v-if="form.stock > 0">В наличии {{ form.stock }} {{ stockUnitsDict[form.stockUnit] }}</span>
              </div>

          </div> <!-- Конец info-card -->

      </div> <!-- Конец right-col -->
        </div> <!-- Конец main-content -->


        <!-- СЕКЦИЯ: О товаре (общая ширина) -->
        <div class="full-width-section">
        <h2>О товаре</h2>
        
        <div class="card content-card">
            
            <!-- Пищевая ценность -->
            <div class="form-group">
              <h3>Пищевая ценность <span>на 100г продукта</span></h3>
              <div class="nutrition-grid">
                <!-- Жиры -->
                <div class="nut-item">
                    <span class="nut-label">Жиры:</span>
                    
                    <input 
                        v-if="isEditMode" 
                        :value="form.nutrition.fats"
                        @input="validateInput(form.nutrition, 'fats', $event)"
                        class="nut-input"
                    >
                    <span v-else class="nut-value">{{ form.nutrition.fats }}<span class="nut-unit">г</span></span>
                </div>

                <!-- Углеводы -->
                <div class="nut-item">
                    <span class="nut-label">Углеводы:</span>
                    
                    <input 
                        v-if="isEditMode" 
                        :value="form.nutrition.carbs"
                        @input="validateInput(form.nutrition, 'carbs', $event)"
                        class="nut-input"
                    >
                    <span v-else class="nut-value">{{ form.nutrition.carbs }}<span class="nut-unit">г</span></span>
                </div>

                <!-- Белки -->
                <div class="nut-item">
                    <span class="nut-label">Белки:</span>
                    
                    <input 
                        v-if="isEditMode" 
                        :value="form.nutrition.protein"
                        @input="validateInput(form.nutrition, 'protein', $event)"
                        class="nut-input"
                    >
                    <span v-else class="nut-value">{{ form.nutrition.protein }}<span class="nut-unit">г</span></span>
                </div>

                <!-- Энергетическая ценность -->
                <div class="nut-item wide">
                    <span class="nut-label">Энергетическая ценность:</span>
                    
                    <input 
                        v-if="isEditMode" 
                        :value="form.nutrition.energy"
                        @input="validateInput(form.nutrition, 'energy', $event)"
                        class="nut-input"
                    >
                    <span v-else class="nut-value">{{ form.nutrition.energy }}<span class="nut-unit">кКал/</span>{{ form.nutrition.joules }}<span class="nut-unit">кДж</span></span>
                    <span v-if ="isEditMode">/</span>
                    <input v-if ="isEditMode" :value="form.nutrition.joules" @input="validateInput(form.nutrition, 'joules', $event)" class="nut-input">
                  </div>
              </div>
            </div>

            <!-- Состав -->
            <div class="form-group">
            <h3>Состав</h3>
            <textarea 
                v-if="isEditMode" 
                v-model="form.description" 
                rows="5"
            ></textarea>
            <p v-else class="text-content">{{ form.description }}</p>
            </div>

            <!-- Характеристики -->
            <div class="form-group">
            <div class="header-with-action">
                <h3>Характеристики</h3>
                <button v-if="isEditMode" class="add-circle-btn card" @click="addAttr">
                  <img src="../../assets/add-circle.svg" />
                </button>
            </div>
            
            <div class="attrs-list">
                <!-- Режим редактирования -->
                <template v-if="isEditMode">
                <div v-for="(attr, i) in form.attributes" :key="i" class="attr-row-edit">
                    <input v-model="attr.name" placeholder="Название">
                    <input v-model="attr.value" placeholder="Значение">
                    <button class="icon-btn danger" @click="removeAttr(i)">
                      <img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/>
                    </button>
                </div>
                </template>

                <!-- Режим просмотра -->
                <template v-else>
                <div v-for="(attr, i) in form.attributes" :key="i" class="attr-row-view">
                    <span class="attr-name">{{ attr.name }}</span>
                    <span class="attr-dots"></span>
                    <span class="attr-val">{{ attr.value }}</span>
                </div>
                </template>
            </div>
            </div>

        </div>
        </div>

        <div v-if="!isEditMode" class="full-width-section reviews">
          
          <div class="reviews-top-bar">
              <h3>Отзывы <span class="grey-text">{{ reviews.length }}</span></h3>
              
              <div class="sort-controls">
                  <span class="sort-label">Сортировка по:</span>
                  
                  <!-- Кнопка: Дата -->
                  <button 
                      class="btn-sort" 
                      :class="{ active: currentSort.field === 'date' }" 
                      @click="toggleSort('date')"
                  >
                      Дате 
                      <span v-if="currentSort.field === 'date'">
                          {{ currentSort.direction === 'asc' ? '↑' : '↓' }}
                      </span>
                  </button>

                  <!-- Кнопка: Рейтинг -->
                  <button 
                      class="btn-sort" 
                      :class="{ active: currentSort.field === 'rating' }" 
                      @click="toggleSort('rating')"
                  >
                      Рейтингу
                      <span v-if="currentSort.field === 'rating'">
                          {{ currentSort.direction === 'asc' ? '↑' : '↓' }}
                      </span>
                  </button>

                  <!-- Кнопка: Полезность -->
                  <button 
                      class="btn-sort" 
                      :class="{ active: currentSort.field === 'useful' }" 
                      @click="toggleSort('useful')"
                  >
                      Полезности
                      <span v-if="currentSort.field === 'useful'">
                          {{ currentSort.direction === 'asc' ? '↑' : '↓' }}
                      </span>
                  </button>
              </div>
          </div>

          <!-- Список отзывов (Заменили reviews на sortedReviews) -->
          <div class="reviews-list">
              <div v-for="review in sortedReviews" :key="review.id" class="review-card">
                        
                  <!-- === ВАРИАНТ 1: ВАШ ОТВЕТ === -->
                  <div v-if="review.type === 'admin_reply'" class="review-content">
                      <div class="review-header-title">
                          <h4>{{ review.title }}</h4>
                      </div>
                      
                      <p class="review-text">{{ review.text }}</p>
                      
                      <!-- Блок цитаты -->
                      <div class="review-quote">
                          <div class="quote-bar"></div>
                          <p>{{ review.quote }}</p>
                      </div>
                  </div>

                  <!-- === ВАРИАНТ 2: ОТЗЫВ ПОЛЬЗОВАТЕЛЯ === -->
                  <div v-else class="review-content">
                      <div class="review-user-header">
                          <div class="user-info">
                              <div class="avatar-circle">
                                  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#ccc" stroke-width="2">
                                      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                                      <circle cx="12" cy="7" r="4"></circle>
                                  </svg>
                              </div>
                              <span class="user-name">{{ review.author }}</span>
                          </div>

                          <!-- Звезды -->
                          <div class="stars-row">
                              <svg v-for="i in 5" :key="i" width="20" height="20" viewBox="0 0 24 24" class="star-mini">
                                  <defs>
                                      <linearGradient :id="'r-grad-' + review.id + '-' + i">
                                          <stop offset="0%" stop-color="#FF7A00"/>
                                          <stop :offset="calculateOffset(review.rating, i)" stop-color="#FF7A00"/>
                                          <stop :offset="calculateOffset(review.rating, i)" stop-color="#D1D5DB"/>
                                          <stop offset="100%" stop-color="#D1D5DB"/>
                                      </linearGradient>
                                  </defs>
                                  <path :fill="'url(#r-grad-' + review.id + '-' + i + ')'" d="M12 2L15.09 8.26L22 9.27L17 14.14L18.18 21.02L12 17.77L5.82 21.02L7 14.14L2 9.27L8.91 8.26L12 2Z"/>
                              </svg>
                              <span class="rating-num">{{ review.rating.toString().replace('.', ',') }}</span>
                          </div>
                      </div>

                      <p class="review-text">{{ review.text }}</p>
                  </div>

                  <!-- === ФУТЕР (ОБЩИЙ) === -->
                  <div class="review-footer">
                      <span class="pub-date">Дата публикации: {{ review.date }}</span>
                      
                      <div class="review-actions">
                          <!-- Кнопка Глаз (Показать/Скрыть) -->
                          <button class="action-btn">
                              <!-- Иконка перечеркнутого глаза (если isHidden) -->
                              <img v-if="review.isHidden" src="../../assets/eye-slash.svg">
                              <!-- Иконка обычного глаза -->
                              <img v-else src="../../assets/eye.svg">
                              {{ review.isHidden ? 'Скрыть комментарий' : 'Показать комментарий' }}
                          </button>

                          <!-- Кнопка Ответить -->
                          <button class="action-btn">
                              <img src="../../assets/undo.svg">
                              Ответить на комментарий
                          </button>

                          <!-- Кнопка Удалить -->
                          <button class="action-btn red">
                              <img src="../../assets/trash.svg" style="filter: invert(25%) sepia(82%) saturate(1764%) hue-rotate(334deg) brightness(106%) contrast(86%)"/>
                              Удалить комментарий
                          </button>
                      </div>
                  </div>

              </div>
          </div>
      </div>


        <!-- STICKY FOOTER (Только Edit Mode) -->
        <div v-if="isEditMode" class="sticky-footer">
        <div class="footer-content">
            <button class="btn-cancel" @click="cancelChanges">Отменить</button>
            <button class="btn-save" @click="saveChanges">Сохранить</button>
        </div>
        </div>

    </div>
  </AdminLayout>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue';
import AdminLayout from './AdminLayout.vue';
import Category from '../catalog/Category.vue';


// --- STATE ---
const isEditMode = ref(false); // Главный переключатель// === Логика категорий ===
const showCatDropdown = ref(false);
const isCategorySelectorOpen = ref(false);
const categorySearchQuery = ref('');
const expandedCategory = ref(null);
// Мок данных категорий
const categoriesList = ref([
  { 
    name: 'Овощи', 
    subs: ['Картофель', 'Морковь', 'Огурцы'] 
  },
  { 
    name: 'Фрукты', 
    subs: ['Цитрусовые', 'Яблоки', 'Бананы'] 
  },
  { 
    name: 'Ягоды', 
    subs: ['Клубника', 'Малина'] 
  }
]);

const filteredCategories = computed(() => {
  const query = categorySearchQuery.value.toLowerCase().trim();
  
  if (!query) return categoriesList.value;
  
  return categoriesList.value
    .map(cat => ({
      name: cat.name,
      subs: cat.subs.filter(sub => 
        sub.toLowerCase().includes(query) || 
        cat.name.toLowerCase().includes(query)
      )
    }))
    .filter(cat => 
      cat.subs.length > 0 || 
      cat.name.toLowerCase().includes(query)
    );
});

const selectedCategoryLabel = computed(() => {
  const cat = form.value.category;
  const sub = form.value.subcategory;

  // Если есть и то, и другое
  if (cat && sub) {
    return `${cat} - ${sub}`;
  }
  
  // Если по какой-то причине есть только подкатегория
  if (sub) {
    return sub;
  }

  return 'Выберите категорию';
});

// Раскрыть/закрыть папку
const toggleCategory = (catName) => {
  expandedCategory.value = expandedCategory.value === catName ? null : catName;
};

// Выбрать подкатегорию
const selectSubcategory = (catName, sub) => {
  form.value.category = catName;
  form.value.subcategory = sub;
  isCategorySelectorOpen.value = false;
  expandedCategory.value = null;
};
const unitDict = ref({ 'кг': 'Килограмм', 'г': 'Грамм', 'л': 'Литр', 'мл': 'Миллилитр' });
// Индекс активного изображения для режима просмотра
const activeImageIndex = ref(0);
// Данные формы

// === Состояние сортировки ===
// direction: 'asc' (↑), 'desc' (↓), null (выкл)
const currentSort = ref({
  field: null, 
  direction: null
});

// === Мок данных (добавил поле useful) ===
const reviews = ref([
  {
    id: 1,
    type: 'admin_reply',
    title: 'Ваш ответ',
    text: 'Lorem Ipsum - это текст-"рыба"...',
    quote: 'Цитата...',
    date: '10.05.25', // Сделал дату старше для проверки
    rating: 0,      // У админа нет рейтинга
    useful: 0,
    isHidden: true
  },
  {
    id: 2,
    type: 'user_review',
    author: 'Аноним',
    rating: 3.6,
    useful: 15,     // Полезность
    text: 'Текст отзыва...',
    date: '13.05.25',
    isHidden: false
  },
  {
    id: 3,
    type: 'user_review',
    author: 'Иван Иванов',
    rating: 5.0,
    useful: 5,
    text: 'Отличный товар!',
    date: '12.05.25',
    isHidden: false
  }
]);

// === Логика переключения (3 шага) ===
const toggleSort = (field) => {
  // Если нажали на другую кнопку - сбрасываем на "asc" для новой кнопки
  if (currentSort.value.field !== field) {
    currentSort.value = { field, direction: 'asc' };
    return;
  }

  // Если нажали на ту же кнопку - перебираем состояния
  if (currentSort.value.direction === 'asc') {
    currentSort.value.direction = 'desc'; // 2-е нажатие
  } else if (currentSort.value.direction === 'desc') {
    currentSort.value = { field: null, direction: null }; // 3-е нажатие (выкл)
  }
};

// === Вычисляемое свойство для отображения ===
const sortedReviews = computed(() => {
  // Если сортировка выключена
  if (!currentSort.value.field) {
    return reviews.value;
  }

  return [...reviews.value].sort((a, b) => {
    let modifier = currentSort.value.direction === 'asc' ? 1 : -1;
    
    // === ИСПРАВЛЕННАЯ СОРТИРОВКА ПО ДАТЕ ===
    if (currentSort.value.field === 'date') {
      // Функция для превращения '13.05.25' в Timestamp
      const getTimestamp = (dateStr) => {
        const [day, month, year] = dateStr.split('.');
        // new Date(год, месяц(0-11), день)
        // Добавляем 2000 к году (25 -> 2025)
        // Вычитаем 1 из месяца, т.к. в JS месяцы с 0
        return new Date(2000 + Number(year), Number(month) - 1, Number(day)).getTime();
      };

      return (getTimestamp(a.date) - getTimestamp(b.date)) * modifier;
    }

    // Сортировка по рейтингу
    if (currentSort.value.field === 'rating') {
      return (a.rating - b.rating) * modifier;
    }

    // Сортировка по полезности
    if (currentSort.value.field === 'useful') {
      return (a.useful - b.useful) * modifier;
    }

    return 0;
  });
});

const form = ref({
  title: 'Какое-то название очень крутого вкусного и замечательного товара',
  weight: 1000,
  unit: 'г',
  price: 30000,
  stock: 200,
  rating: 3.7,
  category: '',
  subcategory: '',
  images: [1, 2, 3], // Мок изображений
  nutrition: {
    fats: 4,
    carbs: 4,
    protein: 4,
    energy: 400,
    joules: 44
  },
  description: 'Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне...',
  attributes: [
    { name: 'Полное название', value: 'Печенье LOTTE Choco Pie...' },
    { name: 'Тип продукта', value: 'Пирожное' },
    { name: 'Вид продукта', value: 'Бисквитный' }
  ]
});

// --- LOGIC ---

// === Состояния выпадающих списков ===
const showUnitMenu = ref(false);
const showPriceUnitMenu = ref(false);
const showStockUnitMenu = ref(false);

// === Словари (Данные для списков) ===
// unitDict у тебя уже есть: { 'кг': 'Килограмм', ... }

// Опции для "Цена за..."
const priceUnitsDict = ref({ 'st': 'Шт.', 'kg': 'Кг', 'l': 'Л' });
// Опции для "В наличии..."
const stockUnitsDict = ref({ 'kg': 'Килограмм', 'st': 'Штук', 'l': 'Литров' });

// Если в form еще нет этих полей, зададим дефолтные (или добавь их в свой объект form)
if (!form.value.priceUnit) form.value.priceUnit = 'st';
if (!form.value.stockUnit) form.value.stockUnit = 'kg';


// === Директива v-click-outside (Чтобы закрывать меню при клике в пустоту) ===
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


const addImage = () => {
  form.value.images.push(Date.now());
};

const removeImage = (index) => {
  form.value.images.splice(index, 1);
};

const addAttr = () => {
  form.value.attributes.push({ name: '', value: '' });
};

const removeAttr = (index) => {
  form.value.attributes.splice(index, 1);
};

const saveChanges = () => {
  console.log('Saving...', form.value);
  isEditMode.value = false;
};

const cancelChanges = () => {
  if(confirm('Сбросить изменения?')) {
    isEditMode.value = false;
  }
};

const calculateOffset = (rating, starIndex) => {
  if (!rating) return '0%';
  
  const val = rating - (starIndex - 1);
  
  if (val >= 1) return '100%';
  if (val <= 0) return '0%';
  
  return `${val * 100}%`;
};

const validateInput = (obj, key, event) => {
  let val = event.target.value;
  val = val.replace(/[^0-9.]/g, '');
  if ((val.match(/\./g) || []).length > 1) {
     val = val.slice(0, -1);
  }
  event.target.value = val;
  obj[key] = val;
};

const selectedUnitLabel = computed(() => {
  return unitDict.value[form.value.unit] || form.value.unit;
});

const selectUnit = (unit) => {
  form.value.unit = unit;
  showUnitMenu.value = false;
};

const closeUnitMenu = (e) => {
  if (e.target.closest('.custom-dropdown-menu')) showUnitMenu.value = false;
};

onMounted(() => document.addEventListener('click', closeUnitMenu));
onUnmounted(() => document.removeEventListener('click', closeUnitMenu));

</script>

<style scoped lang="scss">
@import './admin.css';
/* --- Variables --- */
$primary-orange: #FF7F00;
$bg-grey: #F5F7FA;
$text-dark: #333;
$border-color: #E0E0E0;

span { font-weight: 600; font-size: 18px; }

/* --- Layout --- */
.product-page {
  font-family: 'Libre Franklin', sans-serif;
  margin: 0 auto;
  padding: 20px;
  color: $text-dark;
}

.breadcrumbs {
  color: $primary-orange;
  font-size: 14px;
  margin-bottom: 20px;
}

/* --- Top Bar --- */
.top-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

.mode-switcher {
  background: #F9F9F9;
  border-radius: 20px;
  padding: 4px;
  display: flex;

  button {
    border: none;
    padding: 10px 20px;
    border-radius: 16px;
    cursor: pointer;
    background: transparent;
    font-weight: 500;
    transition: 0.3s;

    &.active {
      background: $primary-orange;
      color: white;
      box-shadow: 0 2px 5px rgba(0,0,0,0.1);
    }
  }
}

.top-actions {
    display: flex;
    gap: 48px;
    align-items: center;
}

.top-actions button {
  font-size: 18px;
  font-weight: 400;
  margin-left: 15px;
  border: none;
  background: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 10px;
  transition: 0.2s;
  background-color: #f9f9f9;
  padding: 12px 18px;
  border-radius: 16px;
}
.btn-outline { color: #555; }
.text-red { color: #e74c3c; }

/* --- Main Grid --- */
.main-content {
  display: flex;
  gap: 30px;
  margin-bottom: 30px;
}

.left-col { flex: 0 0 35%; }
.right-col { flex: 1; background-color: #f9f9f9; padding: 48px; border-radius: 24px; }
.card.info-card { display: flex; flex-direction: column; justify-content: center; gap: 40px; height: 100%; }
.card.content-card { background-color: #f9f9f9; padding: 24px; border-radius: 24px; }
/* --- Images --- */
.image-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 15px;
}

.image-card {
  aspect-ratio: 1;
  background: $bg-grey;
  border-radius: 12px;
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;

  .badge {
    position: absolute;
    top: 10px;
    left: 10px;
    background: $primary-orange;
    color: white;
    padding: 4px 10px;
    border-radius: 12px;
    font-size: 12px;
    &.grey { background: #7f8c8d; }
  }

  .btn-delete {
    position: absolute;
    top: 10px;
    right: 10px;
    background: white;
    border: none;
    border-radius: 50%;
    width: 30px;
    height: 30px;
    cursor: pointer;
    color: red;
  }
}

.add-new {
  border: 2px dashed $border-color;
  cursor: pointer;
  .plus-icon { font-size: 40px; color: $primary-orange; }
}

.form-group {
  margin-bottom: 25px;
  
  label {
    display: block;
    font-weight: 600;
    margin-bottom: 10px;
  }
}

.row-inputs {
  display: flex;
  gap: 10px;
  align-items: center;
}

input, select, textarea {
  border: none;
  outline: none;
  padding: 12px 24px;
  border-radius: 16px;
  font-size: 18px;
}

textarea {
  resize: none;
  appearance: textfield;
  width: -webkit-fill-available;
}

.add-circle-btn {
  background-color: #fff;
}

.input-main { flex: 1; }
.input-short { width: 80px; }
.select-short { width: auto; background-color: #fff; }

/* --- Category Dropdown --- */
.category-dropdown-trigger {
  background: $primary-orange;
  color: white;
  padding: 15px;
  border-radius: 8px;
  cursor: pointer;
  display: flex;
  justify-content: space-between;
}

.category-dropdown-menu {
  border: 1px solid $border-color;
  margin-top: 10px;
  border-radius: 8px;
  padding: 15px;
  background: white;
  
  .search-cat { width: 100%; margin-bottom: 10px; box-sizing: border-box; }
  
  .cat-item {
    margin-bottom: 5px;
    .cat-header { padding: 8px; cursor: pointer; &:hover { background: #f0f0f0; } }
    .sub-cats { padding-left: 20px; display: flex; flex-direction: column; gap: 5px; margin-top: 5px; }
  }
}

/* --- Nutrition Grid --- */
.nutrition-grid {
  display: flex;
  gap: 15px;
  flex-wrap: wrap;
  padding: 15px;
  border-radius: 10px;
  
  .nut-item {
    display: flex;
    align-items: center;
    gap: 8px;
  }
}

/* --- Attributes --- */
.header-with-action {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.btn-circle {
  width: 30px; height: 30px;
  border-radius: 50%;
  border: 1px solid #ccc;
  background: white;
  cursor: pointer;
}

.attr-row-edit {
  display: grid;
  grid-template-columns: 1fr 2fr 40px;
  gap: 10px;
  margin-bottom: 10px;
}

.attr-row-view {
  display: flex;
  align-items: baseline;
  margin-bottom: 8px;
  
  .attr-name { font-weight: 600; }
  .attr-dots {border-bottom: 1px dotted #ccc; margin: 0 10px; }
  .attr-val { font-weight: 600; }
}

/* --- Sticky Footer --- */
.sticky-footer {
  position: fixed;
  bottom: 0;
  left: 0;
  width: 100%;
  background: white;
  border-top: 1px solid $border-color;
  padding: 15px;
  box-shadow: 0 -2px 10px rgba(0,0,0,0.05);
  display: flex;
  justify-content: flex-end;
  z-index: 100;
}

.footer-content {
  max-width: 1400px;
  width: 100%;
  margin: 0 auto;
  display: flex;
  justify-content: flex-end;
  gap: 15px;
}

.btn-save {
  background: $primary-orange;
  color: white;
  border: none;
  padding: 12px 30px;
  border-radius: 8px;
  font-weight: bold;
  cursor: pointer;
}

.btn-cancel {
  background: #f0f0f0;
  color: #333;
  border: none;
  padding: 12px 30px;
  border-radius: 8px;
  cursor: pointer;
}


.row-rating {
  display: flex;
  align-items: center;
  gap: 8px;
  justify-content: space-between;
}

.stars-wrapper {
  display: flex;
  align-items: center;
}

.rating-value {
  color: #FF7A00;
  font-weight: 600;
  font-size: 24px;
}
.tag {
	background: #555;
	color: #fff;
	padding: 4px 18px;
	border-radius: 24px;
  margin-left: 20px;
}

.price-view {
	display: flex;
	align-items: center;
	justify-content: space-between;
}

/* === ОБЩИЕ СТИЛИ ДЛЯ ЛЕВОЙ КОЛОНКИ === */
.left-col {
    width: 100%;
    /* Ограничиваем ширину, если нужно, или оставляем flex-элементом родителя */
}

/* === РЕЖИМ ПРОСМОТРА (View Mode) === */
.view-gallery {
    display: flex;
    gap: 16px;
}

.thumbnails-col {
    display: flex;
    flex-direction: column;
    gap: 12px;
    width: 140px;
}

/* Миниатюра */
.thumb-item {
    width: 100%;
    aspect-ratio: 1 / 1;
    background-color: #F9F9F9;
    border-radius: 12px;
    cursor: pointer;
    transition: all 0.2s;
    border: 2px solid transparent;
}

.thumb-item:hover {
    background-color: #E5E7EB;
}

.thumb-item.active {
    border-color: #FF7A00; /* Оранжевая рамка для активной */
}

/* Большое фото справа */
.main-image-display {
    flex: 1;
    background-color: #f9f9f9;
    border-radius: 24px;
    overflow: hidden;
}

.main-placeholder {
    width: 600px;
    height: 600px;
    background-color: #F9F9F9; /* Светло-серый фон */
}


/* === РЕЖИМ РЕДАКТИРОВАНИЯ (Edit Mode) === */
.edit-gallery-grid {
    display: grid;
    /* 3 колонки, как на макете */
    grid-template-columns: repeat(3, 1fr); 
    gap: 16px;
}

.edit-card {
    position: relative;
    aspect-ratio: 1 / 1;
    background-color: #f9f9f9; /* Очень светлый фон */
    border-radius: 16px;
    padding: 12px; /* Отступ внутри для контента, если нужно */
    display: flex;
    align-items: center;
    justify-content: center;
}

/* Плейсхолдер внутри карточки (если нет реальной картинки) */
.card-placeholder {
    width: 100%;
    height: 100%;
    /* Можно добавить иконку картинки фоном */
}

/* БЕЙДЖИ */
.badge {
    position: absolute;
    top: 12px;
    left: 12px;
    padding: 4px 12px;
    border-radius: 100px;
    font-size: 12px;
    font-weight: 600;
    color: white;
    z-index: 2;
}

.badge.orange {
    background-color: #FF7A00;
}

.badge.grey {
    background-color: #6B7280;
}

/* КНОПКА УДАЛЕНИЯ */
.btn-delete-card {
    position: absolute;
    top: 12px;
    right: 12px;
    background: transparent;
    border: none;
    cursor: pointer;
    padding: 4px;
    z-index: 2;
    opacity: 0.6;
    transition: opacity 0.2s;
}

.btn-delete-card:hover {
    opacity: 1;
}

/* КНОПКА ДОБАВЛЕНИЯ */
.edit-card.add-new {
    cursor: pointer;
    border: 2px dashed #E5E7EB;
    background: #fff;
    transition: border-color 0.2s;
}

.edit-card.add-new:hover {
    border-color: #FF7A00;
}

.plus-icon {
    font-size: 24px;
    color: #FF7A00;
    font-weight: bold;
}

.nutrition-grid {
    gap: 24px;
    margin-top: 12px;
}

.nut-item {
    display: flex;
    align-items: center;
    gap: 8px; /* Расстояние между лейблом, цифрой и единицей изм. */
    font-size: 14px;
    color: #374151; /* Темно-серый текст */
}

.nut-label {
    color: #6B7280; /* Светло-серый для названия поля */
}

.nut-value {
    font-weight: 600; /* Жирный шрифт для цифр в режиме просмотра */
    color: #111827;   /* Черный цвет */
    min-width: 20px;  /* Чтобы не прыгало, если пусто */
    text-align: center;
}

.nut-input {
    border-radius: 16px;
    font-size: 18px;
    text-align: center;
    outline: none;
    transition: border-color 0.2s;
}

.nut-input:focus {
    border-color: #FF7A00; /* Оранжевый при фокусе */
}

.nut-unit {
    color: #9CA3AF;
}

/* Секция сортировки */
.reviews-top-bar {
    display: flex;
    flex-direction: column;
    gap: 30px;
    margin-bottom: 20px;
}

.sort-controls {
    display: flex;
    align-items: center;
    gap: 15px;
    font-size: 14px;
}

.sort-label {
    font-weight: 600;
    color: #111827;
}

.btn-sort {
    background: none;
    border: none;
    cursor: pointer;
    font-size: 14px;
    color: #FF7A00; /* Цвет по умолчанию как на макете */
    padding: 0;
    font-weight: 500;
}

.btn-sort:not(.active) {
    color: #9CA3AF; /* Серый для неактивных, если нужно */
    /* На макете все оранжевые, но обычно активный выделяется. 
       Если нужно как на картинке (все оранжевые) - уберите этот блок */
}
.btn-sort.active {
    font-weight: 700;
}


/* Карточка отзыва */
.review-card {
    background-color: #F9F9F9;
    border-radius: 12px;
    padding: 24px;
    margin-bottom: 20px;
}

.review-header-title h4 {
    margin: 0 0 12px 0;
    color: #4B5563;
    font-size: 18px;
    font-weight: 600;
}

/* Стили текста */
.review-text {
    font-size: 14px;
    line-height: 1.5;
    color: #374151;
    margin-bottom: 16px;
}

/* Блок цитаты (оранжевая полоска) */
.review-quote {
    display: flex;
    gap: 16px;
    margin-bottom: 16px;
}

.quote-bar {
    width: 4px;
    background-color: #FF7A00;
    border-radius: 2px;
    flex-shrink: 0;
}

.review-quote p {
    font-size: 14px;
    color: #9CA3AF; /* Светло-серый текст цитаты */
    line-height: 1.5;
    margin: 0;
}

/* Хедер юзера */
.review-user-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 16px;
}

.user-info {
    display: flex;
    align-items: center;
    gap: 12px;
}

.avatar-circle {
    width: 32px;
    height: 32px;
    border-radius: 50%;
    background-color: #fff;
    border: 1px solid #E5E7EB;
    display: flex;
    align-items: center;
    justify-content: center;
}

.user-name {
    font-weight: 700;
    font-size: 18px;
    color: #374151;
}

.stars-row {
    display: flex;
    align-items: center;
    gap: 4px;
}

.rating-num {
    font-weight: 700;
    font-size: 18px;
    color: #FF7A00;
    margin-left: 8px;
}

/* Футер карточки */
.review-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-top: 8px;
    /* Если нужно разделить линией, можно добавить border-top */
}

.pub-date {
    font-size: 13px;
    color: #9CA3AF;
}

.review-actions {
    display: flex;
    gap: 24px;
}

.action-btn {
    display: flex;
    align-items: center;
    gap: 8px;
    background: none;
    border: none;
    cursor: pointer;
    font-size: 13px;
    color: #6B7280;
    transition: color 0.2s;
}

.action-btn:hover {
    color: #374151;
}

.action-btn.red {
    color: #EF4444;
}

.action-btn.red:hover {
    color: #DC2626;
}

input {
  border: none;
  background-color: #fff;
  padding: 12px 24px;
  border-radius: 16px;
}

input:focus {
  outline: none;
  border: none;
}

.counter {
  background-color: #fff;
  display: flex;
  padding: 12px 18px;
  border-radius: 16px;
  align-items: center;
}

.counter button {
  font-size: 24px;
  color: #FF7A00;
  background: none;
  border: none;
  outline: none;
  cursor: pointer;
}

.icon-btn { background: none; border: none; cursor: pointer; color: #9CA3AF; transition: color 0.2s; border-radius: 6px; }
.icon-btn:hover { color: #374151; background: rgba(0,0,0,0.05); }
.icon-btn.danger { background-color: #f9f9f9; border: none; color: #E63946; transition: color 0.2s; font-size: 18px; font-weight: 400; }
.icon-btn.danger:hover { color: #EF4444; background: rgba(239, 68, 68, 0.1); }
.icon-btn.danger.card { margin: 0;}


/* Обертка для селекта */
.custom-select-wrapper {
    position: relative;
    min-width: 130px; /* Ширина как у твоих input-short */
}

/* Кнопка-триггер (выглядит как инпут) */
.select-trigger {
    height: 40px; /* Высота под твои инпуты */
    background: #FFF;
    border: 1px solid #D1D5DB; /* Стандартный бордер */
    border-radius: 8px; /* Скругление */
    padding: 0 12px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    cursor: pointer;
    font-size: 14px;
    color: #111827;
    user-select: none;
    transition: border-color 0.2s;
}

.select-trigger:hover, 
.select-trigger.active {
    border-color: #9CA3AF;
}

.select-trigger .arrow {
    font-size: 10px;
    color: #6B7280;
    margin-left: 8px;
}

/* Модификация выпадающего списка для форм */
.custom-dropdown-menu.form-dd {
    width: max-content;
    min-width: 100%;
    max-width: 250px;
    top: calc(100% + 4px);
    left: 0;
    right: auto;
}

.custom-dropdown-menu.form-dd.right-align {
    left: auto;
    right: 0;
}

/* Поправка инпута внутри счетчика */
.input-short-counter {
    width: 40px;
    text-align: center;
    border: none;
    outline: none;
    font-size: 14px;
    font-weight: 600;
    background: transparent;
}

/* Растягиваем обертку на всю ширину родителя */
.custom-select-wrapper.full-width {
    width: 100%;
    min-width: unset;
}

/* Растягиваем выпадающее меню на всю ширину */
.custom-dropdown-menu.full-dd {
    width: 100%;
    max-width: none;
    padding-bottom: 8px;
}

/* Обрезаем текст в триггере, если очень длинный */
.truncate-text {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: 90%;
}

/* Поле поиска внутри меню */
.dd-search-container {
    padding: 0 16px 8px 16px;
}

.dd-search-input {
    width: 100%;
    padding: 8px 12px;
    border: 1px solid #E5E7EB;
    border-radius: 8px;
    font-size: 14px;
    outline: none;
    background-color: #f9f9f9;
}

.dd-search-input:focus {
    border-color: #FF7A00;
    background-color: #fff;
}

/* Скролл для списка, если категорий много */
.dd-scroll-area {
    max-height: 250px;
    overflow-y: auto;
}

/* Родительская категория */
.sort-item.parent-cat {
    justify-content: space-between;
    background-color: #fff;
    font-weight: 600;
}

.cat-arrow {
    font-size: 10px;
    color: #9CA3AF;
    transition: transform 0.2s;
}

.cat-arrow.open {
    transform: rotate(180deg);
}

/* Контейнер для подкатегорий */
.sub-cat-list {
    background-color: #f9f9f9; /* Чуть серее фон для вложенности */
    border-top: 1px solid #F3F4F6;
    border-bottom: 1px solid #F3F4F6;
}

/* Элемент подкатегории */
.sort-item.sub-item {
    padding-left: 32px; /* Отступ слева */
}

/* Жирный шрифт для родителей */
.font-bold {
    font-weight: 600;
    color: #111827;
}

.attrs-list {
    display: flex;
    flex-direction: column;
    gap: 12px;
}

.attr-dots {
  width: 12.5%;
}

/* === Dropdown Menu === */
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
.sort-btn-styled { background-color: #fff; }
.sort-btn-styled span { font-weight: 400; }
/* Родительская категория (папка) */
.select-option.parent-option {
  justify-content: space-between;
  background-color: #f3f4f6;
  font-weight: 600;
  color: #374151;
  border-radius: 8px;
  margin-bottom: 4px;
}

.select-option.parent-option:hover {
  background-color: #e5e7eb;
}

/* Стрелка папки */
.cat-arrow {
  width: 12px;
  height: 12px;
  transition: transform 0.2s ease;
}

.cat-arrow.is-expanded {
  transform: rotate(180deg);
}

/* Подкатегории */
.select-option.sub-option {
  display: flex;
  padding-left: 28px;
  background-color: transparent;
  border-bottom: 1px solid #eee;
}

.select-option.sub-option:hover {
  background-color: #fff7ed;
}

/* Radio индикатор */
.radio-indicator {
  width: 18px;
  height: 18px;
  min-width: 18px;
  border-radius: 50%;
  background: #E5E7EB;
  position: relative;
  transition: all 0.2s;
}

.radio-indicator.selected {
  background: #FF7A00;
  box-shadow: inset 0 0 0 3px #FFD8B3;
}

/* Текст опции */
.option-text {
  flex: 1;
}

.font-bold {
  font-weight: 600;
}

/* Пустой результат */
.empty-select {
  text-align: center;
  color: #9ca3af;
  padding: 20px;
  font-size: 14px;
}

/* Скрыть нативный radio */
.select-option input[type="radio"] {
  display: none;
}

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

</style>