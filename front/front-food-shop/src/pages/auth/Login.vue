<template>
  <div class="page-bg">
    <div class="login-card" :style="{ height: cardHeight + 'px' }">
      <h1 class="login-title">{{ $t('auth.login.title') }}</h1>

      <!-- Глобальная ошибка -->
      <transition name="fade-slide">
        <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>
      </transition>

      <form @submit.prevent="handleSubmit" class="form">

        <!-- Email -->
        <div class="field-wrap">
          <div :class="['field', errors.email ? 'field-error' : '']">
            <input
              v-model="email"
              type="email"
              :placeholder="$t('auth.login.email')"
              class="input"
            />
          </div>
        </div>

        <!-- Password -->
        <div class="field-wrap">
          <div :class="['field', errors.password ? 'field-error' : '']">
            <input
              v-model="password"
              :type="showPassword ? 'text' : 'password'"
              :placeholder="$t('auth.login.password')"
              class="input"
            />
            <button
              type="button"
              class="show-btn"
              @click="showPassword = !showPassword"
            >
              {{ showPassword ? '🙈' : '👁️' }}
            </button>
          </div>
        </div>

        <!-- Remember + Forgot -->
        <div class="remember-forgot">
          <label class="remember">
            <input
              type="checkbox"
              v-model="remember"
              class="custom-checkbox"
            />
            <span>{{ $t('auth.login.remember') }}</span>
          </label>
          <button
            type="button"
            class="forgot-btn"
            @click="handleForgot"
          >
            {{ $t('auth.login.forgot') }}
          </button>
        </div>

        <!-- Submit -->
        <button 
          type="submit" 
          class="submit-btn"
          :class="{ 'inactive-btn': !isFormValid }"
          :disabled="!isFormValid || isLoading"
        >
          {{ isLoading ? $t('auth.login.loading') : $t('auth.login.submit') }}
        </button>
      </form>

      <!-- Create account -->
      <button class="create-btn" @click="handleRegister">
        {{ $t('auth.login.create') }}
      </button>

      <p class="contact-text">
        {{ $t('auth.login.contact') }}<br>
        adminexample@gmail.com
      </p>

      <!-- Language switch -->
      <div class="lang-switch">
        <span
          :class="{ active: currentLocale === 'ru' }"
          @click="setLanguage('ru')"
        >
          RU
        </span>
        |
        <span
          :class="{ active: currentLocale === 'en' }"
          @click="setLanguage('en')"
        >
          EN
        </span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from "vue";
import { useI18n } from "vue-i18n";
import router from "@/router";
import { useAuthStore } from "@/stores/auth";

const { locale } = useI18n(); // <-- важно

const authStore = useAuthStore();

const email = ref("");
const password = ref("");
const remember = ref(false);
const showPassword = ref(false);
const isLoading = ref(false);
const regexEmail = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

const errors = ref({ email: false, password: false });

const errorMessage = ref("");

// --- Динамическая высота ---
const baseCardHeight = 508;
const cardHeight = ref(baseCardHeight);

watch(errorMessage, () => {
  cardHeight.value = baseCardHeight + (errorMessage.value ? 60 : 0);
}, { immediate: true });

// --- Валидация формы ---
const isFormValid = computed(() => 
  regexEmail.test(email.value) && password.value.length > 0
);

// --- Текущий язык (для класса active) ---
const currentLocale = computed(() => locale.value);

// --- Language switch ---
const setLanguage = (lang) => {
  try {
    if (typeof window !== "undefined" && window.localStorage) {
      localStorage.setItem("lang", lang);
    }
  } catch (e) {}

  locale.value = lang;
};

// --- SUBMIT ---
const handleSubmit = async () => {
  errors.value = { email: false, password: false };
  errorMessage.value = "";  
  if (!email.value) {
    errors.value.email = true;
    errorMessage.value = "Почта не указана";
  } else if (!regexEmail.test(email.value)) {
    errors.value.email = true;
    errorMessage.value = "Неверный формат почты";
  }

  if (!password.value) {
    errors.value.password = true;
    errorMessage.value = "Пароль не указан";
  }

  if (errorMessage.value) return;

  try {
    isLoading.value = true;
    const claims = await authStore.login(email.value, password.value, remember.value);
    if (claims.value === 'user') {
      router.push("/");
    } else {
      router.push("/admin");
    }
  } catch (error) {
    const status = error.response?.status;
    const message =
      error.response?.data?.message || error.message;

    if (
      status === 404 ||
      message === "User not found" ||
      message === "Аккаунт не найден"
    ) {
      router.push("/account-not-found");
      return;
    }

    errorMessage.value = message || "Ошибка входа";
  } finally {
    isLoading.value = false;
  }
};

// --- Навигация ---
const handleRegister = () => router.push("/register");
const handleForgot = () => router.push("/forgot-password");
</script>

<style scoped>
@import './auth.css';

.submit-btn.inactive-btn {
  background-color: #FFA84C;
  color: white;
  cursor: not-allowed;
}

.error-text {
  color: #ff3333;
  font-size: 16px;
  margin-top: 6px;
  margin-bottom: 12px;
}

.submit-btn:enabled:hover {
  background-color: #ff7a00;
  color: white;
}

.fade-slide-enter-active, .fade-slide-leave-active {
  transition: all 0.3s ease;
}
.fade-slide-enter-from, .fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
.fade-slide-enter-to, .fade-slide-leave-from {
  opacity: 1;
  transform: translateY(0);
}

/* Перекрываем фиксированную высоту для динамики */
.login-card {
  min-height: 508px;
}
</style>