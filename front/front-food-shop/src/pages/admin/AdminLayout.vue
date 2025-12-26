<template>
  <div class="page">

    <!-- HEADER -->
    <header class="header">
      <div class="logo">LUKOSCHKO – PANEL</div>

      <nav class="admin-menu">
        <router-link to="/admin" class="menu-item" active-class="active">
          Главная
        </router-link>
        <router-link to="/admin/products" class="menu-item" active-class="active">
          Товары
        </router-link>
        <router-link to="/admin/categories" class="menu-item" active-class="active">
          Категории
        </router-link>
        <router-link to="/admin/statistics" class="menu-item" active-class="active">
          Статистика
        </router-link>
        <router-link to="/admin/users" class="menu-item" active-class="active">
          Пользователи
        </router-link>
      </nav>

      <!-- PROFILE (МИНИМАЛЬНОЕ ИЗМЕНЕНИЕ) -->
      <div class="profile-wrapper" @click.stop="toggleMenu">
        <div class="profile-icon">
          <!-- SVG вместо эмодзи -->
          <svg
            width="20"
            height="20"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M12 12C14.7614 12 17 9.76142 17 7C17 4.23858 14.7614 2 12 2C9.23858 2 7 4.23858 7 7C7 9.76142 9.23858 12 12 12Z"
              fill="white"
            />
            <path
              d="M4 20C4 16.6863 7.58172 14 12 14C16.4183 14 20 16.6863 20 20"
              stroke="white"
              stroke-width="2"
              stroke-linecap="round"
            />
          </svg>
        </div>

        <div v-if="menuOpen" class="profile-menu">
          <button @click="goToCatalog">Назад к магазину</button>
          <button @click="goToProfile">Профиль</button>
          <div class="divider"></div>
          <button class="logout" @click="logout">Выйти</button>
        </div>
      </div>
    </header>

    <!-- MAIN CONTENT -->
    <main class="content">
      <slot></slot>
    </main>

    <!-- FOOTER -->
    <footer class="lk-footer">
      <div class="footer-col">
        <h4>Наши контакты</h4>
        <p>+7 999 999 99-99</p>
        <p>горячая линия, ежедневно с 9:00 до 21:00</p>
        <p>example@gmail.com</p>
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
export default {
  name: "AdminLayout",
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
    goToCatalog() {
      this.$router.push("/catalog");
      this.menuOpen = false;
    },
    goToProfile() {
      this.$router.push("/profile");
      this.menuOpen = false;
    },
    logout() {
      this.$router.push("/catalog");
      this.menuOpen = false;
    },
  },
};
</script>

<style scoped>
/* === ТВОИ СТИЛИ (НЕ ТРОГАЛ) === */
html, body {
  margin: 0;
  padding: 0;
  min-height: 100%;
  font-family: Inter, sans-serif;
}

.page {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px 30px;
  border-bottom: 1px solid #eee;
}

.logo {
  font-size: 22px;
  font-weight: bold;
  color: #FF7A00;
}

.admin-menu {
  display: flex;
  gap: 25px;
}

.menu-item {
  color: #FFB366;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.2s;
}

.menu-item:hover {
  color: #FF8C00;
}

.menu-item.active {
  color: #FF7A00;
  border-bottom: 2px solid #FF7A00;
}

/* === ПРОФИЛЬ: МИНИМАЛЬНОЕ ДОПОЛНЕНИЕ === */
.profile-wrapper {
  position: relative;
}

.profile-icon {
  width: 36px;
  height: 36px;
  background: #FF7A00;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.profile-menu {
  position: absolute;
  top: 44px;
  right: 0;
  width: 190px;
  background: #fff;
  border-radius: 10px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.12);
  padding: 8px;
  z-index: 10;
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

/* === CONTENT / FOOTER === */
.content {
  flex: 1;
  padding: 20px 30px;
}

.lk-footer {
  width: 100%;
  background: #f2f2f2;
  padding: 36px 48px;
  padding-top: 16px;
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 30px;
  box-sizing: border-box;
}

.lk-footer .footer-col {
  display: flex;
  flex-direction: column;
}

.lk-footer h4 {
  color: #f57c00;
  font-size: 18px;
  margin-bottom: 12px;
}

.lk-footer p,
.lk-footer a {
  margin: 6px 0;
  opacity: 0.8;
  font-size: 14px;
  color: #333;
  text-decoration: none;
}

.lk-footer a:hover {
  opacity: 1;
}

.footer-app-box {
  width: 171px;
  height: 171px;
  background: #fff;
  border-radius: 14px;
  border: 1px solid #ddd;
}
</style>