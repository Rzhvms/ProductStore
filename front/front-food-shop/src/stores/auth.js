import { defineStore } from 'pinia'
import { login as loginApi, refreshToken, logout } from '@/services/api'
import router from '@/router'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    accessToken: sessionStorage.getItem('accessToken') || null,
    refreshTimer: null,
    remember: !!localStorage.getItem('refreshToken'),
    userRole: sessionStorage.getItem('userRole') || null,
    isRefreshing: false,
    isLoggingOut: false,
    refreshSubscribers: [],
  }),

  getters: {
    isAuthenticated: (state) => !!state.accessToken,
    storedRefreshToken: () => {
      return localStorage.getItem('refreshToken') 
          || sessionStorage.getItem('refreshToken') 
          || null;
    }
  },

  actions: {
    async login(email, password, remember) {
      try {
        const data = await loginApi(email, password);
        this.remember = remember;
        this.setToken(data.accessToken);
        if (data.refreshToken) {
          this.setRefreshToken(data.refreshToken);
        }
        const roleClaim = data.claims?.find(c => c.type === 'role');
        this.userRole = roleClaim?.value || null;
        sessionStorage.setItem('userRole', this.userRole);
        this.startRefreshTimer();
        return this.userRole;
      } catch (error) {
        console.error('Login failed:', error);
        throw error;
      }
    },

    async refreshTokenRe() {
      const currentRefreshToken = this.storedRefreshToken;
      
      if (!currentRefreshToken) {
        this.clearAuth();
        router.push('/login');
        return Promise.reject(new Error('No refresh token'));
      }

      if (this.isLoggingOut) {
        return Promise.reject(new Error('Logout in progress'));
      }

      if (this.isRefreshing) {
        return new Promise((resolve, reject) => {
          this.refreshSubscribers.push({ resolve, reject });
        });
      }

      this.isRefreshing = true;

      try {
        const response = await refreshToken(this.accessToken, currentRefreshToken);
        
        const newAccessToken = response.data?.accessToken || response.accessToken;
        const newRefreshToken = response.data?.refreshToken || response.refreshToken;
        
        if (!newAccessToken) {
          throw new Error('No access token in response');
        }
        
        this.setToken(newAccessToken);
        
        if (newRefreshToken) {
          this.setRefreshToken(newRefreshToken);
        }
        
        this.onRefreshSuccess(newAccessToken);
        
        return newAccessToken;
      } catch (error) {
        this.onRefreshFailure(error);
        this.silentLogout();
        throw error;
      } finally {
        this.isRefreshing = false;
      }
    },

    onRefreshSuccess(token) {
      this.refreshSubscribers.forEach(({ resolve }) => resolve(token));
      this.refreshSubscribers = [];
    },

    onRefreshFailure(error) {
      this.refreshSubscribers.forEach(({ reject }) => reject(error));
      this.refreshSubscribers = [];
    },

    setToken(accessToken) {
      this.accessToken = accessToken;
      if (accessToken) {
        sessionStorage.setItem('accessToken', accessToken);
      } else {
        sessionStorage.removeItem('accessToken');
      }
    },

    setRefreshToken(refreshToken) {
      if (this.remember) {
        localStorage.setItem('refreshToken', refreshToken);
        sessionStorage.removeItem('refreshToken');
      } else {
        sessionStorage.setItem('refreshToken', refreshToken);
        localStorage.removeItem('refreshToken');
      }
    },

    silentLogout() {
      if (this.isLoggingOut) return;
      this.clearAuth();
      router.push('/login');
    },

    async logoutRe() {
      if (this.isLoggingOut) {
        return;
      }
      
      this.isLoggingOut = true;
      this.stopRefreshTimer();
      
      try {
        if (this.accessToken) {
          await logout();
        }
      } catch (error) {
      } finally {
        this.clearAuth();
        this.isLoggingOut = false;
        router.push('/login');
      }
    },

    clearAuth() {
      this.accessToken = null;
      this.userRole = null;
      this.remember = false;
      this.isRefreshing = false;
      this.refreshSubscribers = [];
      
      sessionStorage.removeItem('accessToken');
      sessionStorage.removeItem('userRole');
      sessionStorage.removeItem('refreshToken');
      localStorage.removeItem('refreshToken');
      
      this.stopRefreshTimer();
    },

    startRefreshTimer() {
      this.stopRefreshTimer();
      
      if (!this.storedRefreshToken) {
        return;
      }
      
      const refreshInterval = 40 * 60 * 1000;
      
      this.refreshTimer = setInterval(() => {
        if (!this.isLoggingOut && this.storedRefreshToken) {
          this.refreshTokenRe().catch(() => {});
        }
      }, refreshInterval);
    },

    stopRefreshTimer() {
      if (this.refreshTimer) {
        clearInterval(this.refreshTimer);
        this.refreshTimer = null;
      }
    },

    async initAuth() {
      this.remember = !!localStorage.getItem('refreshToken');
      
      if (!this.userRole) {
        this.userRole = sessionStorage.getItem('userRole') || null;
      }
      
      const hasRefreshToken = !!this.storedRefreshToken;
      
      if (this.accessToken && hasRefreshToken) {
        this.startRefreshTimer();
      } else if (!this.accessToken && hasRefreshToken) {
        try {
          await this.refreshTokenRe();
          this.startRefreshTimer();
        } catch (error) {
        }
      }
    }
  }
});