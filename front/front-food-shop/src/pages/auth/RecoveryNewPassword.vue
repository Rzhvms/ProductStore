<template>
  <div class="page-bg">
    <div class="login-card" :style="{ height: cardHeight + 'px' }">
      <h1 class="login-title" v-html="$t('auth.recoveryPassword.title')"></h1>

      <form @submit.prevent="handleSubmit" class="form">

        <!-- Новый пароль -->
        <div class="field-wrap">
          <div class="field password-field">
            <input
              v-model="password"
              :type="showPassword.password ? 'text' : 'password'"
              :placeholder="$t('auth.recoveryPassword.passwordPlaceholder')"
              class="input"
              maxlength="30"
              @input="onPasswordInput"
            />
            <button type="button" class="show-btn" @click="showPassword.password = !showPassword.password">
              {{ showPassword.password ? '🙈' : '👁️' }}
            </button>
          </div>

          <!-- Сложность пароля -->
          <transition name="fade-slide">
            <div v-if="showPasswordStrength.password" class="password-strength-wrapper">
              <p class="contact-text" style="font-size: 16px; text-align: left; margin-bottom: 4px;">
                {{ $t('auth.recoveryPassword.passwordStrengthText') }}
              </p>
              <div class="password-strength-bg">
                <div
                  class="strength-bar"
                  :style="{ width: strengthWidth.password, background: strengthColor.password }"
                ></div>
              </div>
              <span class="strength-label">{{ strengthLabel }}</span>
            </div>
          </transition>
        </div>

        <!-- Подтверждение пароля -->
        <div class="field-wrap">
          <div class="field password-field">
            <input
              v-model="confirmPassword"
              :type="showPassword.confirm ? 'text' : 'password'"
              :placeholder="$t('auth.recoveryPassword.confirmPasswordPlaceholder')"
              class="input"
              maxlength="30"
              @input="confirmPassword = sanitizeInput(confirmPassword)"
            />
            <button type="button" class="show-btn" @click="showPassword.confirm = !showPassword.confirm">
              {{ showPassword.confirm ? '🙈' : '👁️' }}
            </button>
          </div>

          <!-- Ошибка "Пароли не совпадают" -->
          <transition name="fade-slide">
            <p v-if="passwordsMismatch" class="error-text" style="margin-bottom: 0;">
              {{ $t('auth.recoveryPassword.errors.passwordMismatch') }}
            </p>
          </transition>
        </div>

        <p class="password-rules" v-html="$t('auth.recoveryPassword.rules')"></p>

        <!-- Общая ошибка -->
        <transition name="fade-slide">
          <p v-if="errorMessage" class="error-text">{{ errorMessage }}</p>
        </transition>

        <!-- Сохранить -->
        <button
          type="submit"
          class="submit-btn"
          :class="{ 'inactive-btn': !isFormValid || isSaved }"
          :disabled="!isFormValid || isSaved"
          style="margin-top: 12px;"
        >
          {{ isSaved ? $t('auth.recoveryPassword.saved') : $t('auth.recoveryPassword.save') }}
        </button>

      </form>

      <!-- Контактная информация -->
      <p class="contact-text">
        {{ $t('auth.recoveryPassword.contact.line1') }}<br>
        adminexample@gmail.com
      </p>

      <!-- Переключение языка -->
      <div class="lang-switch">
        <span :class="{ active: locale === 'ru' }" @click="changeLang('ru')">RU</span> |
        <span :class="{ active: locale === 'en' }" @click="changeLang('en')">EN</span>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, computed, reactive, watch } from "vue";
import { useI18n } from "vue-i18n";
import router from "@/router";
import { changePassword } from "@/services/api";

const { locale, t } = useI18n();

const password = ref("");
const confirmPassword = ref("");

const showPassword = reactive({ password: false, confirm: false });
const showPasswordStrength = reactive({ password: false });

const errorMessage = ref("");
const isSaved = ref(false);

const passwordStrength = ref(0);

const baseCardHeight = 524;
const cardHeight = ref(baseCardHeight);

const sanitizeInput = (value) => value.replace(/\s/g, "");

const isFormValid = computed(() => {
  return (
    password.value.length >= 8 &&
    password.value === confirmPassword.value &&
    password.value.trim().length > 0 &&
    !password.value.includes(" ")
  );
});

const updatePasswordStrength = (pass) => {
  let score = 1;
  if (pass.length >= 8) score++;
  if (pass.length >= 12) score++;
  if (pass.length >= 15) score++;

  const hasLower = /[a-z]/.test(pass);
  const hasUpper = /[A-Z]/.test(pass);
  const hasDigits = /\d/.test(pass);
  const hasSymbols = /[^A-Za-z0-9]/.test(pass);

  if (hasLower && hasUpper) score++;
  if (hasDigits) score++;
  if (hasSymbols) score++;

  const lowerPass = pass.toLowerCase();
  const isSequence =
    "abcdefghijklmnopqrstuvwxyz".includes(lowerPass) ||
    "qwertyuiopasdfghjklzxcvbnm".includes(lowerPass) ||
    "0123456789".includes(pass);

  if (isSequence) score = Math.max(1, score - 2);
  return Math.min(score, 6);
};

const onPasswordInput = () => {
  password.value = sanitizeInput(password.value);
  const s = updatePasswordStrength(password.value);
  passwordStrength.value = s;
  showPasswordStrength.password = password.value.length > 0;
};

const strengthWidth = computed(() => ({
  password: password.value ? `${passwordStrength.value * 15 + 10}%` : "10%",
}));

const strengthColor = computed(() => ({
  password: (() => {
    const s = passwordStrength.value;
    if (s <= 2) return "#E63946";
    if (s <= 4) return "#FFA84C";
    return "#8ED76A";
  })(),
}));

const strengthLabel = computed(() => {
  const s = passwordStrength.value;
  if (s <= 2) return t("auth.recoveryPassword.strength.weak");
  if (s <= 4) return t("auth.recoveryPassword.strength.medium");
  return t("auth.recoveryPassword.strength.strong");
});

const passwordsMismatch = computed(() => confirmPassword.value && password.value !== confirmPassword.value);

const updateCardHeight = () => {
  let extraHeight = 0;
  if (showPasswordStrength.password) extraHeight += 72;
  if (passwordsMismatch.value) extraHeight += 24;
  cardHeight.value = baseCardHeight + extraHeight;
};

watch([password, confirmPassword, () => showPasswordStrength.password], updateCardHeight, { immediate: true });

const handleSubmit = async () => {
  if (!isFormValid.value) {
    if (password.value !== confirmPassword.value) {
      errorMessage.value = t("auth.recoveryPassword.errors.passwordMismatch");
    } else {
      errorMessage.value = t("auth.recoveryPassword.errors.common");
    }
    return;
  }

  const token = sessionStorage.getItem("token");
  try {
    await changePassword(password.value, token);
    sessionStorage.removeItem("token");
    isSaved.value = true;
    router.push("/login");
  } catch (error) {
    errorMessage.value = error.message;
  }
};

const changeLang = (lang) => {
  locale.value = lang;
  localStorage.setItem("lang", lang);
};
</script>

<style scoped>
@import './auth.css';

.password-field {
  position: relative;
}

.show-btn {
  position: absolute;
  right: 16px;
  top: 50%;
  transform: translateY(-50%);
  border: none;
  background: transparent;
  cursor: pointer;
}

.password-strength-wrapper {
  display: flex;
  flex-direction: column;
  color: #7A7A7A;
}

.password-strength-bg {
  position: relative;
  width: 100%;
  height: 8px;
  border-radius: 4px;
  background-color: #F4F4F4;
  margin-bottom: 4px;
}

.strength-bar {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  border-radius: 4px;
  transition: width 0.3s ease, background 0.3s ease;
}

.strength-label {
  text-align: right;
  font-size: 16px;
  font-weight: 500;
  color: #7A7A7A;
}

.submit-btn.inactive-btn {
  background-color: #FFA84C;
  color: white;
  cursor: not-allowed;
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
</style>