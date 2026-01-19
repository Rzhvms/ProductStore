import axios from 'axios'
import router from '@/router'
import { useAuthStore } from '@/stores/auth'

const api = axios.create({
  baseURL: 'http://localhost:8080/api/',
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  (config) => {
    const store = useAuthStore(); 
    if (store.accessToken) {
      config.headers.Authorization = `Bearer ${store.accessToken}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    const store = useAuthStore();

    if (!error.response) {
      return Promise.reject(error);
    }

    if (originalRequest.url?.includes('/auth/refresh-token')) {
      await store.logoutRe();
      navigateToLogin();
      return Promise.reject(error);
    }

    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const newToken = await store.refreshTokenRe();
        
        originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return api(originalRequest);
      } catch (refreshError) {
        await store.clearAuth();
        navigateToLogin();
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

function navigateToLogin() {
  if (router.currentRoute.value.name !== 'Login') {
    router.push({ name: 'Login' });
  }
}

export default api;