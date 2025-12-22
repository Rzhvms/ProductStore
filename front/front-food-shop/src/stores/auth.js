import { defineStore } from 'pinia'
import { login as loginApi, refreshToken } from '@/services/api'
import router from '@/router';

export const useAuthStore = defineStore('auth', {
    state: () => ({
        accessToken: sessionStorage.getItem('accessToken') || null,
        refreshTimer: null,
    }),
    getters: {
        isAuthenticated: (state) => !!state.accessToken,
    },
    actions: {
        async login(email, password) {
            try {
                const data = await loginApi(email, password);
                this.setToken(data.accessToken);
                this.startRefreshTimer();
                return true;
            } catch (error) {
                console.error(error);
                throw error;
            }
        },
        async refreshTokenRe() {
            try {
                const response = await refreshToken();
                const newToken = response.data.accessToken;
                this.setToken(newToken);
            } catch (error) {
                console.error(error);
                this.logoutRe();
                router.push({ name: 'Login' });
            }
        },
        setToken(accessToken) {
            this.accessToken = accessToken;
            sessionStorage.setItem('accessToken', accessToken);
            axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
        },
        async logoutRe() {
            try {
                await logout();
            } catch (error) {
                console.error(error);
            } finally {
                this.accessToken = null;
                sessionStorage.removeItem('accessToken');
                this.stopRefreshTimer();
                router.push({ name: 'Login' });
            }
        },
        startRefreshTimer() {
            this.stopRefreshTimer();
            const time = 40 * 60 * 1000;
            this.refreshTimer = setInterval(() => {
                this.refreshToken();
            }, time);
        },
        stopRefreshTimer() {
            if (this.refreshTimer) {
                clearInterval(this.refreshTimer);
                this.refreshTimer = null;
            }
        }
    }
});