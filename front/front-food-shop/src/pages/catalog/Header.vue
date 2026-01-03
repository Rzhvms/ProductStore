<template>
  <header class="header">
    <div class="header-left">
      <div class="logo" @click="$router.push('/')">LUKOSCHKO</div>

      <button class="catalog-btn" @click="$router.push('/catalog')">
        Каталог
      </button>
    </div>

    <div class="header-right">
      <button class="icon-btn" @click="goCart">🛒</button>
      <button class="icon-btn">❤️</button>

      <div class="profile-wrapper">
        <button class="icon-btn" @click="toggleMenu">👤</button>

        <div v-if="isMenuOpen" class="profile-menu">
          <!-- Не авторизован -->
          <template v-if="!authStore?.isAuth">
            <button class="menu-item" @click="goLogin">Войти</button>
          </template>

          <!-- Авторизован -->
          <template v-else>
            <button class="menu-item" @click="goProfile">Личный кабинет</button>
            <button class="menu-item danger" @click="logout">Выйти</button>
          </template>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

// Router
const router = useRouter()

// Pinia store (если ещё не инициализирован, безопасно)
let authStore
try { authStore = useAuthStore() } catch(e) { authStore = { isAuth: false, logoutRe: () => {} } }

// Меню профиля
const isMenuOpen = ref(false)
const toggleMenu = () => (isMenuOpen.value = !isMenuOpen.value)

// Навигация
const goLogin = () => { isMenuOpen.value = false; router.push('/login') }
const goProfile = () => { isMenuOpen.value = false; router.push('/profile') }
const logout = () => { authStore.logoutRe(); isMenuOpen.value = false; router.push('/') }
const goCart = () => { router.push('/cart') }
</script>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 48px;
  border-bottom: 1px solid #eee;
  background: #fff;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 24px;
}

.logo {
  font-size: 24px;
  font-weight: bold;
  color: #ff8800;
  cursor: pointer;
}

.catalog-btn {
  padding: 10px 18px;
  border-radius: 20px;
  background: #ff8800;
  color: #fff;
  font-weight: 600;
  border: none;
  cursor: pointer;
}

.catalog-btn:hover {
  opacity: 0.9;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.icon-btn {
  font-size: 20px;
  background: none;
  border: none;
  cursor: pointer;
  color: #888;
}

/* PROFILE MENU */
.profile-wrapper {
  position: relative;
}

.profile-menu {
  position: absolute;
  right: 0;
  top: 40px;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 10px;
  min-width: 180px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.08);
  display: flex;
  flex-direction: column;
  z-index: 100;
}

.menu-item {
  padding: 12px 16px;
  background: none;
  border: none;
  text-align: left;
  cursor: pointer;
}

.menu-item:hover {
  background: #f5f5f5;
}

.menu-item.danger {
  color: #d33;
}
</style>
