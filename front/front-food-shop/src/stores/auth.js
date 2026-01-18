import { defineStore } from 'pinia'
import { login as loginApi, refreshToken, logout } from '@/services/api'
import router from '@/router';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    accessToken: sessionStorage.getItem('accessToken') || null,
    refreshTimer: null,
    remember: false,
    userRole: sessionStorage.getItem('userRole') || null, // роль подтягиваем сразу
  }),
  getters: {
    isAuthenticated: (state) => !!state.accessToken,
  },
  actions: {
    async login(email, password, remember) {
      try {
        const data = await loginApi(email, password);

        // Устанавливаем accessToken
        this.setToken(data.accessToken);

        // Сохраняем refreshToken
        const refToken = data.refreshToken;
        if (remember) {
          localStorage.setItem('refreshToken', refToken);
          sessionStorage.removeItem('refreshToken');
        } else {
          sessionStorage.setItem('refreshToken', refToken);
          localStorage.removeItem('refreshToken');
        }
        this.remember = remember;

        // Получаем роль пользователя из claims и сохраняем в sessionStorage
        const roleClaim = data.claims?.find(c => c.type === 'role');
        this.userRole = roleClaim?.value || null;
        sessionStorage.setItem('userRole', this.userRole);

        // Запускаем таймер обновления токена
        this.startRefreshTimer();

        return data.claims;
      } catch (error) {
        console.error(error);
        throw error;
      }
    },

    async refreshTokenRe() {
      try {
        const response = await refreshToken(this.accessToken);
        const newToken = response.data.accessToken;
        const refToken = response.data.refreshToken;

        if (this.remember) {
          localStorage.setItem('refreshToken', refToken);
          sessionStorage.removeItem('refreshToken');
        } else {
          sessionStorage.setItem('refreshToken', refToken);
          localStorage.removeItem('refreshToken');
        }

        this.setToken(newToken);
      } catch (error) {
        console.error(error);
        this.logoutRe();
        router.push('/login');
      }
    },

    setToken(accessToken) {
      this.accessToken = accessToken;
      sessionStorage.setItem('accessToken', accessToken);
    },

    logoutRe() {
      try {
        logout(); // вызываем API logout
      } catch (error) {
        console.error(error);
      } finally {
        this.accessToken = null;
        this.userRole = null;
        sessionStorage.removeItem('accessToken');
        sessionStorage.removeItem('userRole');
        this.stopRefreshTimer();
        router.push('/login');
      }
    },

    startRefreshTimer() {
      this.stopRefreshTimer();
      const time = 40 * 60 * 1000; // 40 минут
      this.refreshTimer = setInterval(() => {
        this.refreshTokenRe();
      }, time);
    },

    stopRefreshTimer() {
      if (this.refreshTimer) {
        clearInterval(this.refreshTimer);
        this.refreshTimer = null;
      }
    },

    // Инициализация роли при загрузке хедера
    initUserRole() {
      if (!this.userRole) {
        this.userRole = sessionStorage.getItem('userRole') || null;
      }
    }
  }
});
