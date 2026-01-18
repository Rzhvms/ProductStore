<template>
  <div class="page-bg">
    <!-- Карточка -->
    <div class="login-card" style="height: 585px;">
      <h1 class="login-title" v-html="$t('auth.forgotPassword.title')"></h1>

      <p class="helper-text" v-html="$t('auth.forgotPassword.helperText')"></p>

      <!-- Форма -->
      <form @submit.prevent="handleSubmit" class="form">

        <!-- Email -->
        <div class="field-wrap">
          <div :class="['field', errors.email ? 'field-error' : '']">
            <input
              v-model="email"
              type="email"
              :placeholder="$t('auth.forgotPassword.emailPlaceholder')"
              class="input"
            />
          </div>
          <p v-if="errors.email" class="error-text">{{ errors.email }}</p>
        </div>

        <!-- Submit button -->
        <button
          type="submit"
          class="submit-btn"
          :class="{ 'inactive-btn': !isFormValid }"
          :disabled="!isFormValid"
        >
          {{ $t('auth.forgotPassword.submit') }}
        </button>
      </form>

      <button class="create-btn" @click="handleDecline">
        {{ $t('auth.forgotPassword.decline') }}
      </button>

      <p class="contact-text">
        {{ $t('auth.forgotPassword.contact') }}<br>
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
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { useI18n } from "vue-i18n";
import { resendPinCode } from "@/services/api";

const router = useRouter();
const { locale } = useI18n();

const email = ref("");
const errors = ref({ email: null });

// Валидность формы: есть @ и не пустая строка
const isFormValid = computed(() => {
  return email.value.length > 0 && email.value.includes("@");
});

// --- Language switch ---
const currentLocale = computed(() => locale.value);
const setLanguage = (lang) => {
  try {
    if (typeof window !== "undefined" && window.localStorage) {
      localStorage.setItem("lang", lang);
    }
  } catch (e) {}
  locale.value = lang;
};

const handleSubmit = async () => {
  errors.value.email = null;

  if (!email.value) {
    errors.value.email = "Почта не указана";
    return;
  } else if (!email.value.includes("@")) {
    errors.value.email = "Неверный формат почты";
    return;
  }

  try {
    await resendPinCode(email.value);
    localStorage.setItem("email", email.value);
    router.push("/recovery-code");
  } catch (error) {
    errors.value.email = error.message;
  }
};

const handleDecline = () => {
  router.push("/login");
};
</script>

<style scoped>
@import './auth.css';

/* Отключённая кнопка — как в регистрации */
.submit-btn.inactive-btn {
  background-color: #FFA84C;
  color: white;
  cursor: not-allowed;
}
</style>
