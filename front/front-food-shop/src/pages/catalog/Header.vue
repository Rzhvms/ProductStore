<template>
  <header class="header">
    <div class="header-left">
      <div class="logo" @click="$router.push('/')">LUKOSCHKO</div>
      <button class="catalog-btn" @click="$router.push('/catalog')">Каталог</button>
    </div>

    <div class="header-right">
      <!-- Корзина -->
      <button class="icon-btn" :class="{ active: route.path === '/cart' }" @click="goCart" aria-label="Корзина">
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
          <path d="M5.19001 6.37945C5.00001 6.37945 4.8 6.29945 4.66 6.15945C4.37 5.86945 4.37 5.38945 4.66 5.09945L8.29 1.46945C8.58001 1.17945 9.06001 1.17945 9.35001 1.46945C9.64001 1.75945 9.64001 2.23945 9.35001 2.52945L5.72 6.15945C5.57 6.29945 5.38001 6.37945 5.19001 6.37945Z" fill="currentColor"/>
          <path d="M18.81 6.37945C18.62 6.37945 18.43 6.30945 18.28 6.15945L14.65 2.52945C14.36 2.23945 14.36 1.75945 14.65 1.46945C14.94 1.17945 15.42 1.17945 15.71 1.46945L19.34 5.09945C19.63 5.38945 19.63 5.86945 19.34 6.15945C19.2 6.29945 19 6.37945 18.81 6.37945Z" fill="currentColor"/>
          <path d="M20.21 10.5996H4C3.3 10.6096 2.5 10.6096 1.92 10.0296C1.46 9.57961 1.25 8.87961 1.25 7.84961C1.25 5.09961 3.26 5.09961 4.22 5.09961H19.78C20.74 5.09961 22.75 5.09961 22.75 7.84961C22.75 8.88961 22.54 9.57961 22.08 10.0296C21.56 10.5496 20.86 10.5996 20.21 10.5996Z" fill="currentColor"/>
          <path d="M9.76001 18.3C9.35001 18.3 9.01001 17.96 9.01001 17.55V14C9.01001 13.59 9.35001 13.25 9.76001 13.25C10.17 13.25 10.51 13.59 10.51 14V17.55C10.51 17.97 10.17 18.3 9.76001 18.3Z" fill="currentColor"/>
          <path d="M14.36 18.3C13.95 18.3 13.61 17.96 13.61 17.55V14C13.61 13.59 13.95 13.25 14.36 13.25C14.77 13.25 15.11 13.59 15.11 14V17.55C15.11 17.97 14.77 18.3 14.36 18.3Z" fill="currentColor"/>
          <path d="M14.89 22.7507H8.86C5.28 22.7507 4.48 20.6207 4.17 18.7707L2.76 10.1207C2.69 9.71073 2.97 9.33073 3.38 9.26073C3.79 9.19073 4.17 9.47073 4.24 9.88073L5.65 18.5207C5.94 20.2907 6.54 21.2507 8.86 21.2507H14.89C17.46 21.2507 17.75 20.3507 18.08 18.6107L19.76 9.86073C19.84 9.45073 20.23 9.18073 20.64 9.27073C21.05 9.35073 21.31 9.74073 21.23 10.1507L19.55 18.9007C19.16 20.9307 18.51 22.7507 14.89 22.7507Z" fill="currentColor"/>
        </svg>
      </button>

      <!-- Избранное -->
      <button class="icon-btn" aria-label="Избранное">
        <svg width="24" height="24" viewBox="0 0 24 24">
          <path d="M12 21.65C11.69 21.65 11.39 21.61 11.14 21.52C7.32 20.21 1.25 15.56 1.25 8.69C1.25 5.19 4.08 2.35 7.56 2.35C9.25 2.35 10.83 3.01 12 4.19C13.17 3.01 14.75 2.35 16.44 2.35C19.92 2.35 22.75 5.2 22.75 8.69C22.75 15.57 16.68 20.21 12.86 21.52C12.61 21.61 12.31 21.65 12 21.65Z" fill="currentColor"/>
        </svg>
      </button>

      <!-- Профиль -->
      <div class="profile-wrapper" ref="profileRef">
        <button class="icon-btn icon-profile" :class="{ active: isMenuOpen || route.path === '/profile' }" @click.stop="toggleMenu" aria-label="Профиль">
          <svg width="36" height="36" viewBox="0 0 36 36">
            <rect class="icon-bg" width="36" height="36" rx="18"/>
            <path d="M18 18.75C14.83 18.75 12.25 16.17 12.25 13C12.25 9.83 14.83 7.25 18 7.25C21.17 7.25 23.75 9.83 23.75 13C23.75 16.17 21.17 18.75 18 18.75Z" fill="currentColor"/>
            <path d="M26.59 28.75C26.18 28.75 25.84 28.41 25.84 28C25.84 24.55 22.32 21.75 18 21.75C13.68 21.75 10.16 24.55 10.16 28C10.16 28.41 9.82 28.75 9.41 28.75C9 28.75 8.66 28.41 8.66 28C8.66 23.73 12.85 20.25 18 20.25C23.15 20.25 27.34 23.73 27.34 28C27.34 28.41 27 28.75 26.59 28.75Z" fill="currentColor"/>
          </svg>
        </button>

        <div v-if="isMenuOpen" class="profile-menu">
          <!-- Не авторизован -->
          <button v-if="!authStore.isAuthenticated" class="menu-item" @click="goLogin">Войти</button>

          <!-- Авторизован -->
          <template v-else>
            <button class="menu-item" @click="goProfile">Личный кабинет</button>

            <!-- Админ -->
            <button v-if="authStore.userRole === 'admin'" class="menu-item" @click="goAdmin">Админ-панель</button>

            <button class="menu-item danger" @click="logout">Выйти</button>
          </template>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const isMenuOpen = ref(false)
const profileRef = ref(null)

const toggleMenu = () => isMenuOpen.value = !isMenuOpen.value

const handleClickOutside = (e) => {
  if (isMenuOpen.value && profileRef.value && !profileRef.value.contains(e.target)) {
    isMenuOpen.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)

  // Инициализируем роль пользователя из sessionStorage
  authStore.initUserRole()
})

onBeforeUnmount(() => document.removeEventListener('click', handleClickOutside))

const goLogin = () => { isMenuOpen.value = false; router.push('/login') }
const goProfile = () => { isMenuOpen.value = false; router.push('/profile') }
const goAdmin = () => { isMenuOpen.value = false; router.push('/admin') }
const logout = () => { authStore.logoutRe(); isMenuOpen.value = false }
const goCart = () => router.push('/cart')
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
  gap: 24px;
}

.logo {
  display: flex;
  align-items: center;
  font-size: 24px;
  font-weight: bold;
  color: #ff8800;
  cursor: pointer;
  line-height: 1;
  transform: translateY(1px);
}

.catalog-btn {
  padding: 10px 18px;
  border-radius: 20px;
  background: #ff7a00;
  color: #fff;
  border: none;
  font-weight: 600;
  cursor: pointer;
}
.catalog-btn:hover {
  opacity: 0.9;
}

.header-right {
  display: flex;
  gap: 16px;
}

/* ICONS */
.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  color: #ddd;
}

.icon-btn:hover {
  color: #ff7a00;
}

/* Активные кнопки кроме профиля */
.icon-btn:not(.icon-profile).active {
  color: #ff7a00;
}

/* Активная кнопка профиля */
.icon-btn.icon-profile.active {
  color: #fff;
}

/* Фон иконок */
.icon-bg {
  fill: #f9f9f9;
}

.icon-btn:hover .icon-bg {
  fill: #fff1e6;
}

.icon-btn.icon-profile.active .icon-bg {
  fill: #ff7a00;
}

/* MENU */
.profile-wrapper {
  position: relative;
}

.profile-menu {
  position: absolute;
  right: 0;
  top: 44px;
  min-width: 180px;
  background: #fff;
  border-radius: 10px;
  border: 1px solid #eee;
  box-shadow: 0 10px 30px rgba(0,0,0,.08);
}

.menu-item {
  display: block;
  width: 100%;
  padding: 12px 16px;
  border: none;
  background: none;
  text-align: left;
  cursor: pointer;
}

.menu-item:hover {
  background: #f5f5f5;
}

.menu-item.danger {
  color: #d33;
}

.header svg {
  display: inline !important;
  vertical-align: middle;
}

.profile-wrapper {
  position: relative;
  z-index: 50;
}

.profile-menu {
  position: absolute;
  right: 0;
  top: 44px;
  z-index: 100;
  min-width: 180px;
  background: #fff;
  border-radius: 10px;
  border: 1px solid #eee;
  box-shadow: 0 10px 30px rgba(0,0,0,.08);
}
</style>
