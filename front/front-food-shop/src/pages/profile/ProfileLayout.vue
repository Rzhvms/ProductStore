<!-- ProfileLayout.vue -->
<template>
  <div class="page">

    <!-- HEADER -->
    <header class="header">
      <div class="logo">LUKOSCHKO</div>
      <div class="profile-icon">👤</div>
    </header>

    <!-- TITLE -->
    <h1 class="title">Личный кабинет</h1>

    <!-- BODY -->
    <div class="container">

      <!-- LEFT SIDEBAR -->
      <div class="sidebar">
        <nav class="menu">
          <router-link
            to="/profile"
            class="menu-item"
            active-class="active"
          >
            Профиль
          </router-link>

          <router-link
            to="/profile/orders"
            class="menu-item"
            active-class="active"
          >
            Заказы
          </router-link>

          <router-link
            to="/profile/help"
            class="menu-item"
            active-class="active"
          >
            Справочная информация
          </router-link>
        </nav>
        <button class="logout-btn" @click="authStore.logoutRe">Выйти из аккаунта</button>
      </div>

      <!-- MAIN CONTENT -->
      <div class="content">
        <!-- Здесь будет вставляться уникальный контент страницы -->
        <slot></slot>
      </div>
    </div>

    <!-- FOOTER -->
    <footer class="lk-footer">
      <div class="footer-col">
        <h4>Наши контакты</h4>
        <p>+7 999 999 99-99</p>
        <p>горячая линия, ежедневно с 9:00 до 21:00</p>
        <p>example@gmail.com</p>
        <div class="f-socials">
           <a href="#" class="soc-circle"><img src='../../assets/telegram.svg' alt="Telegram"></a>
           <a href="#" class="soc-circle"><img src='../../assets/whatsapp.svg' alt="WhatsApp"></a>
           <a href="#" class="soc-circle vk"><img src='../../assets/vk.svg' alt="VK"></a>
           <a href="#" class="soc-circle ok"><img src='../../assets/ok.svg' alt="OK"></a>
           <a href="#" class="soc-circle inst"><img src='../../assets/instagram.svg' alt="Instagram"></a>
        </div>
        <h4>Copyright</h4>
      </div>
      <div class="footer-col">
        <h4>Покупателям</h4>
        <a href="#">Справочная информация</a>
        <a href="#">Обратная связь</a>
        <a href="#">Оценка товаров</a>
        <a href="#">О нас</a>
      </div>
      <div class="footer-col">
        <h4>Партнерам и сотрудникам</h4>
        <a href="#">Вакансии</a>
        <a href="#">Поставщикам</a>
        <a href="#">Отдел маркетинга и рекламы</a>
        <a href="#">Предложения по ассортименту</a>
      </div>
      <div class="footer-col">
        <h4>Правовая информация</h4>
        <a href="#">Правовая информация</a>
        <a href="#">Пользовательское соглашение</a>
        <a href="#">Оферта о продаже товаров дистанционным способом</a>
        <a href="#">Политика обработки персональных данных</a>
        <a href="#">Горячая линия по этике</a>
      </div>
      <div class="footer-col">
        <h4>Наше приложение</h4>
        <p>Android и iOS</p>
        <div class="footer-app-box"></div>
      </div>
    </footer>

  </div>
</template>

<script>
import { useAuthStore } from '@/stores/auth';

export default {
  name: "ProfileLayout",
  setup() {
    const authStore = useAuthStore();
    return { 
      authStore 
    };
  }
};
</script>

<style src="./profile.css"></style>