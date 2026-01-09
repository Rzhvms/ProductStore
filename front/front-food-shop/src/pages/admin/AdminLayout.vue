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
        <div class="profile-wrapper" @click.stop="toggleMenu">
          <div class="profile-icon">
            <img src="../../assets/User.svg" alt="User">
          </div>
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
  padding-bottom: 2px;
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

.profile-icon {
  width: 24px;
  height: 24px;
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