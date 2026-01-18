<template>
  <div class="page-bg">
    <div
      class="login-card"
      :class="{ 'login-card-error': codeError }"
    >
      <h1 class="login-title" v-html="$t('auth.recoveryCode.title')"></h1>

      <p class="contact-text" style="margin-bottom: 16px; font-size: 16px;" v-html="$t('auth.recoveryCode.subtitle')"></p>

      <form @submit.prevent="handleSubmit" class="form">
        <div class="numbers-group">
          <div
            v-for="(digit, index) in code"
            :key="index"
            class="code-input-wrapper"
            :style="{
              marginRight: (index === 2 || index === 5) ? '0px' : '12px',
              marginLeft: (index === 3 ? '38px' : '0px')
            }"
          >
            <input
              type="text"
              maxlength="1"
              class="code-input"
              :class="[
                { 'code-error': codeError },
                { 'code-success': codeSuccess },
                { 'shake': shakeActive }
              ]"
              v-model="code[index]"
              @input="onInput(index, $event)"
              @keydown="onKeyDown(index, $event)"
              @keydown.backspace.prevent="onBackspace(index, $event)"
              ref="inputs"
            />
          </div>
        </div>

        <p v-if="codeError" class="error-text" style="text-align: center; font-size: 16px; white-space: pre-line; margin-top: 16px; margin-bottom: 28px;">
          {{ codeErrorText }}
        </p>

        <button
          type="button"
          class="create-btn resend-btn"
          :disabled="resendActive"
          @click="handleResend"
        >
          <span v-if="!resendActive">{{ $t('auth.recoveryCode.resend') }}</span>
          <span v-else>{{ $t('auth.recoveryCode.resendTimer', { timer }) }}</span>
        </button>

        <button
          type="submit"
          class="submit-btn"
          :class="{ 'inactive-btn': !isCodeComplete }"
          style="margin-top: 16px;"
        >
          {{ $t('auth.recoveryCode.submit') }}
        </button>
      </form>

      <p class="contact-text" style="margin-top: 16px;">
        {{ $t('auth.recoveryCode.contact.line1') }}<br>
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
import { ref, computed, nextTick, onMounted } from "vue";
import { useI18n } from "vue-i18n";
import router from "@/router";
import { verifyEmail } from "@/services/api";

const { locale, t } = useI18n();

const currentLocale = computed(() => locale.value);

const setLanguage = (lang) => {
  locale.value = lang;
  if (typeof localStorage !== "undefined") {
    localStorage.setItem("lang", lang);
  }
};

const code = ref(["", "", "", "", "", ""]);
const codeError = ref(false);
const codeErrorText = ref("");
const codeSuccess = ref(false);
const shakeActive = ref(false);

const inputs = ref([]);

const resendActive = ref(false);
const timer = ref(60);
let interval = null;

const isCodeComplete = computed(() => code.value.every(d => d !== ""));

const handleSubmit = async () => {
  const enteredCode = code.value.join("");
  const email = localStorage.getItem("email");
  try {
    const response = await verifyEmail(email, enteredCode);
    const token = response.accessToken;
    sessionStorage.setItem("token", token);
    codeError.value = false;
    codeSuccess.value = true;
    shakeActive.value = false;
    router.push("/new-password");
  } catch (error) {
    codeError.value = true;
    codeErrorText.value = t('auth.recoveryCode.errors.invalidCode');
    codeSuccess.value = false;
    shakeActive.value = false;
    setTimeout(() => (shakeActive.value = true), 50);
  }
};

const startTimer = (seconds) => {
  timer.value = seconds;
  resendActive.value = true;

  clearInterval(interval);
  interval = setInterval(() => {
    if (timer.value > 0) {
      timer.value -= 1;
    } else {
      resendActive.value = false;
      clearInterval(interval);
    }
  }, 1000);
};

const handleResend = () => {
  if (resendActive.value) return;
  localStorage.setItem("lastResendTime", Date.now().toString());
  startTimer(60);
};

onMounted(() => {
  const last = localStorage.getItem("lastResendTime");
  if (last) {
    const elapsed = Math.floor((Date.now() - parseInt(last)) / 1000);
    if (elapsed < 60) {
      startTimer(60 - elapsed);
    } else {
      startTimer(60);
      localStorage.removeItem("lastResendTime");
    }
  } else {
    startTimer(60);
  }
});

const onInput = (index, event) => {
  const val = event.target.value.replace(/\D/g, "");
  code.value[index] = val;

  if (val && index < code.value.length - 1) {
    nextTick(() => inputs.value[index + 1].focus());
  }

  if (codeError.value) {
    codeError.value = false;
    shakeActive.value = false;
  }
};

const onKeyDown = (index, event) => {
  if (event.key === "Enter") {
    event.preventDefault();
    if (isCodeComplete.value) handleSubmit();
    return;
  }

  if (!/[0-9]/.test(event.key) && !["Backspace","ArrowLeft","ArrowRight","Tab"].includes(event.key)) {
    event.preventDefault();
  }
};

const onBackspace = (index, event) => {
  if (!code.value[index] && index > 0) {
    nextTick(() => {
      inputs.value[index - 1].focus();
      code.value[index - 1] = "";
    });
  } else {
    code.value[index] = "";
  }
  if (codeError.value) {
    codeError.value = false;
    shakeActive.value = false;
  }
};
</script>

<style scoped>
@import './auth.css';

.login-card {
  width: 410px;
  height: 580px;
  transition: height 0.3s ease;
}

.login-card-error {
  height: 652px;
}

/* остальные стили как у confirm-email */
.numbers-group { display: flex; justify-content: flex-start; margin: 16px 0; }
.code-input-wrapper { display: inline-block; }
.code-input { width: 38px; height: 50px; text-align: center; font-size: 22px; border-radius: 18px; border: 1px solid #FFA84C; background-color: #F4F4F4; outline: none; transition: border-color 0.2s ease, color 0.2s ease; }
.code-input:focus { border-color: #FF7A00; }
.code-error { border-color: #E63946 !important; color: #E63946; }
.code-success { border-color: #8ED76A !important; color: #000; }
.shake { animation: shake 0.3s ease; }
@keyframes shake { 0% { transform: translateX(0); } 20% { transform: translateX(-5px); } 40% { transform: translateX(5px); } 60% { transform: translateX(-5px); } 80% { transform: translateX(5px); } 100% { transform: translateX(0); } }

.resend-btn {
  width: 362px;
  height: 48px;
  margin-top: 12px;
  background: #f4f4f4;
  color: #999999;
  border-radius: 18px;
  border: none;
  font-size: 18px;
  font-weight: 600;
  cursor: not-allowed;
  transition: color 0.2s ease;
}
.resend-btn:enabled { cursor: pointer; color: #555555; }
.resend-btn:hover:enabled,
.resend-btn:active:enabled { color: #FF7A00; }

.submit-btn.inactive-btn {
  background-color: #FFA84C;
  color: white;
  cursor: not-allowed;
}

/* Language switch */
.lang-switch span {
  cursor: pointer;
  opacity: 0.6;
}
.lang-switch span.active {
  font-weight: 600;
  opacity: 1;
  color: #ff7a00;
}
</style>