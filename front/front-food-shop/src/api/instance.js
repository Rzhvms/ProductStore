import axios from 'axios'
import { useAuthStore } from '@/stores/auth'

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
        const origRequest = error.config;
        if (error.response.status === 401 && !origRequest._retry) {
            origRequest._retry = true;
            const authStore = useAuthStore();
            try {
                await authStore.refreshTokenRe();
                origRequest.headers.Authorization = `Bearer ${authStore.accessToken}`;
                return api(origRequest);
            } catch (error) {
                authStore.logoutRe();
                router.push('/login');
                return Promise.reject(error);
            }
        }
        return Promise.reject(error);
    }
);

export default api;