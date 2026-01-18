import axios from 'axios'
import { useAuthStore } from '@/stores/auth'
import router from '@/router'

const api = axios.create({
    baseURL: 'http://localhost:8080/api/',
    headers: {
        'Content-Type': 'application/json',
    },
});

api.interceptors.request.use(config => {
    const authStore = useAuthStore();
    if (authStore.accessToken) {
        config.headers.Authorization = `Bearer ${authStore.accessToken}`;
    }
    return config;
});

api.interceptors.response.use(
    (response) => { return response; },
    async (error) => {
        const originalRequest = error.config;
        
        if (!error.response) {
            return Promise.reject(error);
        }

        if (originalRequest.url && originalRequest.url.includes('/auth/refresh-token')) {
            const authStore = useAuthStore();
            authStore.logoutRe();
            if (router.currentRoute.value.name !== 'Login') {
                router.push('/login');
            }
            return Promise.reject(error);
        }

        if (error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            const authStore = useAuthStore();
            try {
                await authStore.refreshTokenRe();
                originalRequest.headers.Authorization = `Bearer ${authStore.accessToken}`;
                return api(originalRequest);
            } catch (refreshError) {
                authStore.logoutRe();
                router.push('/login');
                return Promise.reject(refreshError);
            }
        }
        return Promise.reject(error);
    }
);

export default api;