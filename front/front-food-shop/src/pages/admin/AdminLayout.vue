<template>
  <div class="page">
    <header class="header">
      <router-link to="/admin" class="logo">LUKOSCHKO</router-link>

      <nav class="admin-menu">
        <router-link to="/admin/categories" class="menu-item" active-class="active">
          Каталог
        </router-link>
        <router-link to="/admin/products" class="menu-item" active-class="active">
          Товары
        </router-link>
        <router-link to="/admin/statistics" class="menu-item" active-class="active">
          Статистика
        </router-link>
        <router-link to="/admin/users" class="menu-item" active-class="active">
          Пользователи
        </router-link>

        <!-- PROFILE -->
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
            <button @click="goToCatalog">Назад к магазину</button>
            <button @click="goToProfile">Профиль</button>
            <div class="divider"></div>
            <button class="logout" @click="logout">Выйти</button>
          </div>
        </div>
      </nav>
    </header>

    <main class="content">
      <slot></slot>
    </main>
  </div>
</template>

<script>

import { useAuthStore } from '@/stores/auth'
export default {
  name: "AdminLayout",
  setup() {
    const authStore = useAuthStore()
    return { authStore }
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
    goToCatalog() {
      this.$router.push("/");
      this.menuOpen = false;
    },
    goToProfile() {
      this.$router.push("/profile");
      this.menuOpen = false;
    },
    logout() {
      this.authStore.logoutRe();
      this.menuOpen = false;
    },
    goToHome() {
      this.$router.push("/admin");
    },
  },
};
</script>

<style scoped>
html, body {
  margin: 0;
  padding: 0;
  min-height: 100%;
  font-family: Libre Franklin, sans-serif;
}

.menu-item.active {
  background: none !important;
  padding: 0 !important;
  border-radius: 0 !important;
  color: #ff7a00 !important;
  font-weight: 400;
}

.admin-menu .menu-item {
  padding: 0 !important;
  margin: 0;
  line-height: 1;
  height: 100%;
  display: flex;
  align-items: center;
  background: none;
  border-radius: 0;
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
  text-decoration: none;
}

.admin-menu {
  display: flex;
  gap: 48px;
  align-items: center;
}

.menu-item {
  color: #7a7a7a;
  text-decoration: none;
  font-weight: 400;
  font-size: 14px;
  transition: color 0.2s;
}

.menu-item:hover {
  color: #ffa84c;
  text-decoration: underline;
}

.menu-item.active {
  color: #ff7a00;
}

.profile-wrapper {
  position: relative;
}

/* PROFILE ICON STATES */
.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
  color: #ddd;
}

.icon-bg {
  fill: #f9f9f9;
  transition: fill 0.2s;
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

.icon-btn.icon-profile {
  transform: translateY(2px);
}


/* MENU */
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

.content {
  flex: 1;
  padding: 20px 30px;
}
</style>
