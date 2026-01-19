import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import './index.css'
import './pages/auth/auth.css'
import { i18n } from './i18n'


const app = createApp(App)
app.use(createPinia())
app.use(router)
app.use(i18n)
import { useAuthStore } from './stores/auth'
const authStore = useAuthStore()
authStore.initAuth()
app.mount('#app')