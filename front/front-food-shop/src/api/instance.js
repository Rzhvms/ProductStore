import axios from 'axios'

const api = axios.create({
    baseURL: 'http://localhost:5282/api/',
    headers: {
        'Content-Type': 'application/json',
    },
});

api.interceptors.request.use(config => {
    const accessToken = sessionStorage.getItem('accessToken');
    if (accessToken) {
        config.headers.Authorization = `Bearer ${accessToken}`;
    }
    return config;
});

api.interceptors.response.use(response => {
    (response) => response,
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
                router.push({ name: 'Login' });
                return Promise.reject(error);
            }
        }
        return Promise.reject(error);
    }
});

export default api;