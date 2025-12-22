<template>
  <div class="catalog-page">
    <!-- Header (Повторяем шапку) -->
    <header class="header container">
      <div class="logo">LUKOSCHKO</div>
      <div class="header-icons">
        <button class="icon-btn">🛒</button>
        <button class="icon-btn">❤️</button>
        <button class="icon-btn">👤</button>
      </div>
    </header>

    <main class="main-content container" style="display: block;">
      <!-- Заголовок и Поиск -->
      <div class="page-top-row">
        <h1>Каталог</h1>
        <div class="search-bar">
          <span class="search-icon">🔍</span>
          <input type="text" placeholder="Поиск..." />
        </div>
      </div>

      <!-- Список категорий (Аккордеон) -->
      <div class="categories-list">
        <div 
          v-for="(category, index) in categories" 
          :key="index" 
          class="category-item"
        >
          <!-- Заголовок категории (кликабельный) -->
          <div class="category-header" @click="toggleCategory(index)">
            <span class="cat-name">{{ category.name }}</span>
            <!-- Стрелочка, которая вращается -->
            <span class="chevron" :class="{ open: category.isOpen }">⌵</span>
          </div>

          <!-- Подкатегории (появляются если isOpen = true) -->
          <transition name="slide-fade">
            <div v-if="category.isOpen" class="subcategories-list">
              <div 
                v-for="(sub, subIndex) in category.items" 
                :key="subIndex" 
                class="subcategory-item"
                :class="{ active: subIndex === 1 && index === 2 }" 
              >
                <!-- :class active - просто для примера, чтобы показать серый фон как на макете -->
                {{ sub }}
              </div>
            </div>
          </transition>
        </div>
      </div>

      <!-- Кнопка внизу справа -->
      <div class="bottom-action">
        <button class="btn-primary">Связаться с нами</button>
      </div>
    </main>

    <!-- Footer (Тот же, что и на странице товара) -->
    <footer class="footer">
      <div class="container footer-grid">
        <div class="footer-col">
          <h4>Наши контакты</h4>
          <p>+7 999 999 99-99</p>
          <p>example@gmail.com</p>
          <div class="socials">
             <span v-for="i in 5" :key="i" class="social-circle"></span>
          </div>
          <p class="copyright">Copyright</p>
        </div>
        <div class="footer-col">
          <h4>Покупателям</h4>
          <ul>
            <li><a href="#">Справочная информация</a></li>
            <li><a href="#">Обратная связь</a></li>
          </ul>
        </div>
        <div class="footer-col">
           <h4>Партнерам</h4>
            <ul>
            <li><a href="#">Вакансии</a></li>
            <li><a href="#">Поставщикам</a></li>
          </ul>
        </div>
        <div class="footer-col">
           <h4>Инфо</h4>
            <ul>
            <li><a href="#">Правовая информация</a></li>
          </ul>
        </div>
         <div class="footer-col">
           <h4>Приложение</h4>
           <p>Android и iOS</p>
           <div class="app-qr-placeholder"></div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import { ref } from 'vue';

// Моковые данные для каталога
const categories = ref([
  { name: 'Готовая', items: ['Салаты', 'Супы'], isOpen: false },
  { name: 'Название категории', items: ['Подкатегория 1', 'Подкатегория 2'], isOpen: false },
  { 
    name: 'Название категории', 
    items: [
      'Название подкатегории',
      'Название подкатегории', // Этот будет подсвечен серым через CSS класс active
      'Название подкатегории',
      'Название подкатегории',
      'Название подкатегории',
      'Название подкатегории',
    ], 
    isOpen: true // Эта категория открыта по умолчанию, как на макете
  },
  { name: 'Название категории', items: ['Тест'], isOpen: false },
  { name: 'Название категории', items: [], isOpen: false },
  { name: 'Название категории', items: [], isOpen: false },
  { name: 'Название категории', items: [], isOpen: false },
  { name: 'Название категории', items: [], isOpen: false },
  { name: 'Название категории', items: [], isOpen: false },
  { name: 'Название категории', items: [], isOpen: false },
]);

// Логика переключения
const toggleCategory = (index) => {
  categories.value[index].isOpen = !categories.value[index].isOpen;
};
</script>

<style lang="scss" scoped>
/* Переменные */
$primary-orange: #ff8800;
$text-dark: #222;
$text-grey: #888;
$bg-secondary: #f6f6f6;
$border-color: #e0e0e0;
$container-max-width: 1920px;

/* Reset & Base */
.container {
  max-width: $container-max-width;
  margin: 0 auto;
  padding: 0 20px;
  box-sizing: border-box;
}

button { cursor: pointer; border: none; background: none; font-family: inherit;}
a { text-decoration: none; color: inherit; }
ul { list-style: none; padding: 0; margin: 0; }

.btn-primary {
  background-color: $primary-orange;
  color: white;
  padding: 12px 24px;
  border-radius: 30px;
  font-weight: 600;
  font-size: 16px;
  transition: opacity 0.2s;
  &:hover { opacity: 0.9; }
}

/* Header */
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  .logo { font-weight: bold; font-size: 24px; color: $primary-orange; }
  .icon-btn { font-size: 20px; margin-left: 15px; color: $text-grey; }
}

/* --- Catalog Specific Styles --- */

.main-content {
  min-height: 600px;
  padding-bottom: 60px;
}

/* Верхняя строка с заголовком и поиском */
.page-top-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 20px;
  margin-bottom: 30px;

  h1 {
    font-size: 32px;
    font-weight: bold;
    color: $text-dark;
  }

  .search-bar {
    display: flex;
    align-items: center;
    background-color: $bg-secondary;
    padding: 12px 20px;
    border-radius: 8px;
    width: 60%; /* Широкий поиск как на макете */
    
    .search-icon {
      color: $text-grey;
      margin-right: 10px;
      font-size: 18px;
    }
    
    input {
      border: none;
      background: transparent;
      outline: none;
      width: 100%;
      font-size: 16px;
      color: $text-dark;
      &::placeholder {
        color: $text-grey;
      }
    }
  }
}

/* Список категорий */
.categories-list {
  border-top: 1px solid $border-color;
  
  .category-item {
    border-bottom: 1px solid $border-color;
    
    .category-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 20px 0;
      cursor: pointer;
      user-select: none;
      transition: color 0.2s;

      .cat-name {
        font-size: 16px;
        font-weight: 500;
        color: $text-dark;
      }

      .chevron {
        color: $text-grey;
        font-size: 14px;
        transition: transform 0.3s ease;
        &.open {
          transform: rotate(180deg); /* Поворот стрелки */
        }
      }
      
      &:hover .cat-name {
        color: $primary-orange;
      }
    }

    /* Список подкатегорий */
    .subcategories-list {
      padding-bottom: 20px;
      
      .subcategory-item {
        padding: 12px 20px; /* Отступы внутри */
        margin-bottom: 4px;
        color: $text-dark;
        font-size: 16px;
        cursor: pointer;
        /* На макете подкатегории не имеют отступа слева от края, 
           но имеют фон на всю ширину при наведении.
           Мы делаем паддинг слева, чтобы текст был сдвинут. */
           
        &.active, &:hover {
          background-color: $bg-secondary;
          border-radius: 4px; /* Небольшое скругление если нужно, на макете вроде прямые */
        }
      }
    }
  }
}

/* Кнопка внизу */
.bottom-action {
  display: flex;
  justify-content: flex-end;
  margin-top: 40px;
}

/* Footer (Same as Product page) */
.footer {
  background-color: $bg-secondary;
  padding: 60px 0;
  font-size: 14px;
  color: $text-dark;
  margin-top: auto;
  
  .footer-grid {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 40px;
    
    h4 { color: $primary-orange; margin-bottom: 20px; }
    ul li { margin-bottom: 10px; }
    p { margin-bottom: 10px; color: #555; }
    
    .socials {
        display: flex;
        gap: 10px;
        margin: 20px 0;
        .social-circle { width: 30px; height: 30px; background: #999; border-radius: 50%; display: inline-block; }
    }
    .copyright { color: $primary-orange; font-weight: bold; margin-top: 20px;}
    .app-qr-placeholder { width: 100px; height: 100px; background: #ddd; }
  }
}

/* Анимация раскрытия (опционально) */
.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: all 0.3s ease-out;
  max-height: 500px; /* Достаточно большое число */
  overflow: hidden;
  opacity: 1;
}

.slide-fade-enter-from,
.slide-fade-leave-to {
  max-height: 0;
  opacity: 0;
  padding-bottom: 0;
}
</style>