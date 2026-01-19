import { defineStore } from 'pinia'
import { login as loginApi, refreshToken, logout } from '@/services/api'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    accessToken: sessionStorage.getItem('accessToken') || null,
    refreshTimer: null,
    remember: !!localStorage.getItem('refreshToken'),
    userRole: sessionStorage.getItem('userRole') || null,
    isRefreshing: false,
    refreshSubscribers: [],
  }),

  getters: {
    isAuthenticated: (state) => !!state.accessToken,
    storedRefreshToken: (state) => {
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
        console.error('Refresh token not found');
        await this.logoutRe();
        return Promise.reject(new Error('No refresh token'));
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
        
        this.setToken(newAccessToken);
        
        if (newRefreshToken) {
          this.setRefreshToken(newRefreshToken);
        }
        
        console.log('Token refreshed successfully');
        
        this.onRefreshSuccess(newAccessToken);
        
        return newAccessToken;
      } catch (error) {
        console.error('Token refresh failed:', error);
        
        this.onRefreshFailure(error);
        
        await this.logoutRe();
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

    async logoutRe() {
      this.stopRefreshTimer();
      try {
        await logout();
      } catch (error) {
        console.error('Logout API error:', error);
      } finally {
        this.clearAuth();
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
      
      const refreshInterval = 40 * 60 * 1000; // 40 минут
      
      this.refreshTimer = setInterval(() => {
        this.refreshTokenRe().catch(err => {
          console.error('Scheduled token refresh failed:', err);
        });
      }, refreshInterval);
    },

    stopRefreshTimer() {
      if (this.refreshTimer) {
        clearInterval(this.refreshTimer);
        this.refreshTimer = null;
      }
    },

    initAuth() {
      this.remember = !!localStorage.getItem('refreshToken');
      
      if (!this.userRole) {
        this.userRole = sessionStorage.getItem('userRole') || null;
      }
      
      if (this.accessToken && this.storedRefreshToken) {
        this.startRefreshTimer();
      }
    }
  }
});