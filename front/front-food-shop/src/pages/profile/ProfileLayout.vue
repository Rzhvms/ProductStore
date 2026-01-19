<!-- ProfileLayout.vue -->
<template>
  <div class="page">

    <!-- HEADER -->
    <header class="header">
      <!-- Лого -->
      <router-link to="/" class="logo">LUKOSCHKO</router-link>

      <!-- PROFILE ICON -->
      <div class="profile-wrapper" @click.stop="toggleMenu">
        <button
          class="icon-btn icon-profile"
          :class="{ active: menuOpen }"
          aria-label="Профиль"
        >
          <svg width="36" height="36" viewBox="0 0 36 36">
            <rect class="icon-bg" width="36" height="36" rx="18" />
            <path
              d="M18 18.75C14.83 18.75 12.25 16.17 12.25 13C12.25 9.83 14.83 7.25 18 7.25C21.17 7.25 23.75 9.83 23.75 13C23.75 16.17 21.17 18.75 18 18.75Z"
              fill="currentColor"
            />
            <path
              d="M26.59 28.75C26.18 28.75 25.84 28.41 25.84 28C25.84 24.55 22.32 21.75 18 21.75C13.68 21.75 10.16 24.55 10.16 28C10.16 28.41 9.82 28.75 9.41 28.75C9 28.75 8.66 28.41 8.66 28C8.66 23.73 12.85 20.25 18 20.25C23.15 20.25 27.34 23.73 27.34 28C27.34 28.41 27 28.75 26.59 28.75Z"
              fill="currentColor"
            />
          </svg>
        </button>

        <div v-if="menuOpen" class="profile-menu">
          <button @click="goToProfile">Вернуться в магазин</button>

          <button
            v-if="authStore.userRole === 'admin'"
            @click="goToAdmin"
          >
            Админ-панель
          </button>

          <div class="divider"></div>
          <button class="logout" @click="logout">Выйти</button>
        </div>
      </div>
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
  },
  data() {
    return {
      menuOpen: false,
    };
  },
  mounted() {
    document.addEventListener("click", this.closeMenu);
  },
  beforeUnmount() {
    document.removeEventListener("click", this.closeMenu);
  },
  methods: {
    toggleMenu() {
      this.menuOpen = !this.menuOpen;
    },
    closeMenu() {
      this.menuOpen = false;
    },
    goToProfile() {
      this.$router.push("/");
      this.menuOpen = false;
    },
    goToAdmin() {
      this.$router.push("/admin");
      this.menuOpen = false;
    },
    logout() {
      this.authStore.logoutRe();
      this.menuOpen = false;
    },
  },
};
</script>
<style scoped>

/* BODY CONTAINER */
.container {
  display: flex;
  gap: 20px; /* расстояние между sidebar и контентом */
  align-items: flex-start;
}

/* SIDEBAR */
.sidebar {
  width: 300px;          /* фиксированная ширина */
  flex-shrink: 0;        /* не уменьшается при сжатии контейнера */
  padding: 20px;
  box-sizing: border-box;
  border-radius: 10px;
}

/* MAIN CONTENT */
.content {
  flex: 1;               /* занимает всё оставшееся место */
  min-width: 0;          /* важно для корректного сжатия контента в flex */
  padding: 20px;
  box-sizing: border-box;
  overflow-wrap: break-word; /* чтобы текст не вылазил */
}

/* OPTIONAL: при очень маленьких экранах можно переносить sidebar */
@media (max-width: 768px) {
  .container {
    flex-direction: column;
  }
  .sidebar {
    width: 100%;
  }
}

/* HEADER */
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 30px;
  border-bottom: 1px solid #eee;
}

.header .logo {
  font-size: 22px;
  font-weight: bold;
  color: #FF7A00;
  text-decoration: none;
}

/* PROFILE ICON */
.profile-wrapper {
  position: relative;
}

.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
  color: #ddd;
  transition: color 0.2s;
}

.icon-bg {
  fill: #f9f9f9;
  transition: fill 0.2s;
}

.icon-btn.icon-profile {
  transform: translateY(3px);
}

.icon-btn:hover {
  color: #ff7a00;
}

.icon-btn:hover .icon-bg {
  fill: #fff1e6;
}

.icon-btn.active {
  color: #fff;
}

.icon-btn.active .icon-bg {
  fill: #ff7a00;
}

/* PROFILE MENU */
.profile-menu {
  position: absolute;
  top: 44px;
  right: 0;
  width: 190px;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.12);
  padding: 8px;
  z-index: 1000000;
}

.profile-menu button {
  width: 100%;
  padding: 9px 12px;
  background: none;
  border: none;
  text-align: left;
  font-size: 14px;
  cursor: pointer;
  border-radius: 6px;
}

.profile-menu button:hover {
  background: #f5f5f5;
}

.profile-menu .logout {
  color: #e53935;
}

.divider {
  height: 1px;
  background: #eee;
  margin: 6px 0;
}
</style>
<style src="./profile.css"></style>
