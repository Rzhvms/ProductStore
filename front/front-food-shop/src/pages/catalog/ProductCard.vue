<template>
  <div class="product-page">
    <!-- Header -->
    <header class="header container">
      <div class="logo">LUKOSCHKO</div>
      <div class="header-icons">
        <button class="icon-btn">🛒</button>
        <button class="icon-btn">❤️</button>
        <button class="icon-btn">👤</button>
      </div>
    </header>

    <!-- Breadcrumbs -->
    <nav class="breadcrumbs container">
      <ul>
        <li><a href="#">Каталог</a></li>
        <li><span class="separator">›</span></li>
        <li><a href="#">Категория</a></li>
        <li><span class="separator">›</span></li>
        <li><a href="#">Подкатегория</a></li>
        <li><span class="separator">›</span></li>
        <li class="active">Какое-то название...</li>
      </ul>
    </nav>

    <!-- Main Content -->
    <!-- Здесь явно используем display: block в CSS -->
    <main class="main-content container">
      
      <!-- Секция 1: Товар (Grid внутри) -->
      <section class="product-hero">
        <!-- Галерея -->
        <div class="gallery-wrapper">
          <div class="thumbnails">
            <div 
              v-for="(img, index) in 4" 
              :key="index" 
              class="thumb"
              :class="{ active: index === 1 }"
            ></div>
          </div>
          <div class="main-image"></div>
        </div>

        <!-- Информация -->
        <div class="product-info">
          <h1 class="product-title">
            Какое-то название очень крутого вкусного и замечательного товара, <span class="weight">1000г</span>
          </h1>

          <div class="rating-row">
            <div class="stars">
              <span v-for="n in 5" :key="n" class="star" :class="{ filled: n <= 4 }">★</span>
            </div>
            <span class="rating-val">3,6</span>
            <a href="#reviews" class="reviews-link">1082 отзыва</a>
            <span class="article">Артикул: 111111</span>
          </div>

          <div class="tags-row">
            <div class="tag-group">
              <span class="label">Категория:</span>
              <span class="tag">название категории</span>
            </div>
            <div class="tag-group">
              <span class="label">Бренд:</span>
              <span class="tag">название бренда</span>
            </div>
            <div class="stock-status">В наличии 200шт.</div>
          </div>

          <div class="price-action-row">
            <div class="price">149,99 ₽ / шт</div>
            <button class="btn-primary">В корзину</button>
            <button class="btn-favorite">♡</button>
          </div>
        </div>
      </section>

      <!-- Секция 2: О товаре -->
      <section class="about-section">
        <h2>О товаре</h2>
        <div class="about-card">
          <div class="nutrition-block">
            <h3>Пищевая ценность <span class="sub">на 100г продукта</span></h3>
            <div class="nutrition-grid">
              <div class="nutri-item">Белки: 4г</div>
              <div class="nutri-item">Жиры: 17г</div>
              <div class="nutri-item">Углеводы: 65г</div>
              <div class="nutri-item">Энергетическая ценность: 130кКал/544кДж</div>
            </div>
          </div>

          <div class="composition-block">
            <h3>Состав</h3>
            <p>
              Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века.
            </p>
          </div>

          <div class="specs-block">
            <h3>Характеристики</h3>
            <ul class="specs-list">
              <li v-for="(spec, index) in specs" :key="index" class="spec-item">
                <span class="spec-name">{{ spec.name }}</span>
                <span class="spec-dots"></span>
                <span class="spec-value">{{ spec.value }}</span>
              </li>
            </ul>
          </div>
        </div>
      </section>

      <!-- Секция 3: Отзывы -->
      <section class="reviews-section" id="reviews">
        <div class="reviews-header">
          <h2>Отзывы <span class="count">11 111</span></h2>
          <button class="btn-primary">Написать отзыв</button>
        </div>

        <div class="sort-row">
          <span>Сортировка по:</span>
          <button class="sort-btn active">Дате ↑</button>
          <button class="sort-btn">Рейтингу</button>
          <button class="sort-btn">Полезности</button>
        </div>

        <div class="reviews-list">
          <div v-for="i in 4" :key="i" class="review-card">
            <div class="review-top">
              <div class="user">
                <div class="avatar-placeholder">👤</div>
                <span class="username">Аноним</span>
              </div>
              <div class="review-rating">
                <span class="stars">★★★★☆</span>
                <span class="score">3,6</span>
              </div>
            </div>
            <div class="review-body">
              <p>Lorem Ipsum - это текст-"рыба", часто используемый в печати и вэб-дизайне.</p>
            </div>
            <div class="review-footer">
              <span class="date">Дата публикации: 13.05.25</span>
              <div class="likes">
                <span>Был ли отзыв полезен?</span>
                <button class="like-btn">👍</button>
                <button class="like-btn">👎</button>
              </div>
            </div>
          </div>
        </div>
        
        <div class="load-more-container">
           <button class="btn-primary">Связаться с нами</button>
        </div>
      </section>
    </main>

    <!-- Footer -->
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
const specs = [
  { name: 'Полное название', value: 'Печенье LOTTE Choco Pie бисквитное...' },
  { name: 'Тип продукта', value: 'Пирожное' },
  { name: 'Вид продукта', value: 'Бисквитный' },
  { name: 'Вкус', value: 'Шоколад' },
  { name: 'Тип покрытия/заливки', value: 'В глазури' },
  { name: 'Количество штук в упаковке', value: '12 шт.' },
  { name: 'Энергетическая ценность', value: '438 кКал/1839 кДж' },
  { name: 'Содержание глютена', value: '+' },
  { name: 'Страна', value: 'Россия' },
];
</script>

<style lang="scss" scoped>
/* Variables */
$bg-color: #ffffff;
$bg-secondary: #f6f6f6;
$primary-orange: #ff8800;
$text-dark: #222;
$text-grey: #888;
$border-radius: 12px;
$container-max-width: 1920px; /* Ширина контента как на макете */

/* Base Layout */
.container {
  max-width: $container-max-width;
  margin: 0 auto;
  padding: 0 20px;
  width: 100%;
  box-sizing: border-box;
}

/* Исправление, о котором вы просили: */
.main-content {
  display: block; /* Секции идут строго друг под другом */
  width: 100%;
}

a { text-decoration: none; color: inherit; }
ul { list-style: none; padding: 0; margin: 0; }
button { cursor: pointer; border: none; background: none; font-family: inherit;}

/* UI Elements */
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

/* Breadcrumbs */
.breadcrumbs {
  margin-bottom: 30px;
  ul {
    display: flex;
    font-size: 12px;
    color: $text-grey;
    li {
      display: flex;
      align-items: center;
      .separator { margin: 0 8px; }
      &.active { color: $text-dark; }
    }
  }
}

/* --- SECTIONS --- */

/* 1. Hero Section (Grid for Image + Info) */
.product-hero {
  display: grid;
  grid-template-columns: 1fr 1fr; /* Две колонки */
  gap: 40px;
  margin-bottom: 60px;
  
  .gallery-wrapper {
    display: flex;
    gap: 20px;
    
    .thumbnails {
      display: flex;
      flex-direction: column;
      gap: 10px;
      .thumb {
        width: 80px;
        height: 80px;
        background-color: $bg-secondary;
        border-radius: $border-radius;
        cursor: pointer;
        &.active { border: 2px solid $primary-orange; }
      }
    }
    
    .main-image {
      flex-grow: 1;
      background-color: $bg-secondary;
      border-radius: $border-radius;
      min-height: 400px;
    }
  }
  
  .product-info {
    padding: 20px;
    background-color: $bg-secondary;
    border-radius: $border-radius;
    
    .product-title {
      font-size: 28px;
      font-weight: bold;
      margin-bottom: 20px;
      line-height: 1.2;
      .weight { color: $text-grey; }
    }
    
    .rating-row {
      display: flex;
      align-items: center;
      gap: 15px;
      margin-bottom: 25px;
      font-size: 14px;
      .stars { color: $primary-orange; }
      .rating-val { color: $primary-orange; font-weight: bold; }
      .reviews-link { color: $primary-orange; border-bottom: 1px dashed; }
      .article { margin-left: auto; color: $text-dark; }
    }
    
    .tags-row {
      display: flex;
      align-items: center;
      gap: 30px;
      margin-bottom: 40px;
      font-size: 14px;
      
      .tag-group {
        .label { color: $text-dark; margin-right: 8px; }
        .tag { background: #666; color: white; padding: 4px 12px; border-radius: 15px; font-size: 12px;}
      }
      .stock-status { margin-left: auto; }
    }
    
    .price-action-row {
      display: flex;
      align-items: center;
      gap: 20px;
      .price { font-size: 24px; font-weight: bold; }
      .btn-favorite { width: 45px; height: 45px; border-radius: 50%; border: 1px solid #ddd; background: white; font-size: 20px;}
    }
  }
}

/* 2. About Section */
.about-section {
  margin-bottom: 60px;
  h2 { margin-bottom: 20px; font-size: 24px; }
  
  .about-card {
    background-color: $bg-secondary;
    padding: 30px;
    border-radius: $border-radius;
    
    h3 { font-size: 18px; margin-bottom: 15px; margin-top: 25px; 
        &:first-child { margin-top: 0; }
        .sub { font-size: 14px; color: $text-grey; font-weight: normal; margin-left: 10px; }
    }
    
    .nutrition-grid {
      display: flex;
      gap: 30px;
      font-size: 14px;
      color: $text-dark;
      margin-bottom: 25px;
    }
    
    .composition-block p {
      font-size: 14px;
      line-height: 1.5;
      color: $text-dark;
      margin-bottom: 25px;
      max-width: 800px;
    }
    
    .specs-list {
      max-width: 900px;
      .spec-item {
        display: flex;
        align-items: baseline;
        margin-bottom: 10px;
        font-size: 14px;
        .spec-name { color: $text-dark; }
        .spec-dots { flex: 1; border-bottom: 1px dotted #ccc; margin: 0 10px; }
        .spec-value { color: $text-dark; font-weight: 500; width: 40%; }
      }
    }
  }
}

/* 3. Reviews Section */
.reviews-section {
  margin-bottom: 80px;
  
  .reviews-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
    h2 { font-size: 24px; .count { color: $text-grey; font-weight: normal; font-size: 18px; margin-left: 10px;} }
  }
  
  .sort-row {
    display: flex;
    gap: 20px;
    align-items: center;
    font-size: 14px;
    margin-bottom: 30px;
    color: $primary-orange;
    
    span { color: $text-dark; font-weight: bold; }
    .sort-btn { color: $primary-orange; &.active { font-weight: bold;} }
  }
  
  .review-card {
    background-color: $bg-secondary;
    border-radius: $border-radius;
    padding: 20px;
    margin-bottom: 15px;
    
    .review-top {
      display: flex;
      justify-content: space-between;
      margin-bottom: 15px;
      .user {
        display: flex;
        align-items: center;
        gap: 10px;
        .avatar-placeholder { width: 32px; height: 32px; background: #ddd; border-radius: 50%; display: flex; align-items: center; justify-content: center;}
        .username { font-weight: bold; }
      }
      .review-rating {
        display: flex;
        align-items: center;
        gap: 10px;
        .stars { color: $primary-orange; }
        .score { font-weight: bold; color: $primary-orange; }
      }
    }
    
    .review-body {
      font-size: 14px;
      line-height: 1.5;
      margin-bottom: 15px;
    }
    
    .review-footer {
      display: flex;
      justify-content: space-between;
      font-size: 12px;
      color: $text-grey;
      
      .likes {
        display: flex;
        align-items: center;
        gap: 10px;
        .like-btn { font-size: 14px; color: #aaa; &:hover { color: $text-dark; }}
      }
    }
  }
  .load-more-container {
      display: flex;
      justify-content: flex-end;
      margin-top: 20px;
  }
}

/* Footer */
.footer {
  background-color: $bg-secondary;
  padding: 60px 0;
  font-size: 14px;
  color: $text-dark;
  
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
</style>