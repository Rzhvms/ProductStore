<template>
  <div class="page-bg">
    <div
      class="login-card"
      :class="{ 'login-card-error': codeError }"
    >
      <h1 class="login-title">Подтвердите<br>вашу почту</h1>
      <p class="contact-text" style="margin-bottom: 16px; font-size: 16px;">
        Мы отправили код подтверждения<br>
        на вашу почту
      </p>

      <form @submit.prevent="handleSubmit" class="form">
        <div class="numbers-group">
          <input
            v-for="(digit, index) in code"
            :key="index"
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
            @keydown.backspace.prevent="onBackspace(index, $event)"
            ref="inputs"
          />
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
          <span v-if="!resendActive">Запросить код повторно</span>
          <span v-else>Повторно можно через {{ timer }} сек.</span>
        </button>

        <button
          type="submit"
          class="submit-btn"
          :class="{ 'inactive-btn': !isCodeComplete }"
          :disabled="!isCodeComplete"
          style="margin-top: 16px;"
        >
          Далее
        </button>
      </form>

      <p class="contact-text" style="margin-top: 16px;">
        По всем вопросам можете обращаться:<br>
        adminexample@gmail.com
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, nextTick, onMounted } from "vue";
import router from "@/router";

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

const handleSubmit = () => {
  const enteredCode = code.value.join("");
  if (enteredCode === "123456") {
    codeError.value = false;
    codeSuccess.value = true;
    shakeActive.value = false;
    router.push("/finish-registration");
  } else {
    codeError.value = true;
    codeErrorText.value = "Неверный код подтверждения\nПопробуйте еще раз или запросите код\nповторно";
    codeSuccess.value = false;

    shakeActive.value = false;
    setTimeout(() => (shakeActive.value = true), 50);
  }
};

// Функция для запуска таймера
const startTimer = (seconds) => {
  timer.value = seconds;
  resendActive.value = true;

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

  // Сохраняем время последнего запроса
  const now = Date.now();
  localStorage.setItem("lastResendTime", now.toString());

  // Заглушка для API
  // const email = localStorage.getItem("email");
  // sendVerificationEmail(email);

  startTimer(60);
};

onMounted(() => {
  // Проверяем lastResendTime при загрузке
  const last = localStorage.getItem("lastResendTime");
  if (last) {
    const elapsed = Math.floor((Date.now() - parseInt(last)) / 1000);
    if (elapsed < 60) {
      startTimer(60 - elapsed);
    }
  } else {
    // Если пользователь только зашел и ещё ничего не нажимал
    startTimer(60);
  }
});

const onInput = (index, event) => {
  const val = event.target.value;
  code.value[index] = val.replace(/\D/g, "");
  if (val && index < code.value.length - 1) {
    nextTick(() => inputs.value[index + 1].focus());
  }

  // Сбрасываем красный цвет при стирании
  if (codeError.value) {
    codeError.value = false;
    shakeActive.value = false;
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

.numbers-group {
  display: flex;
  justify-content: space-between;
  margin: 16px 0;
  width: 362px;
}

.code-input {
  width: 48px;
  height: 48px;
  text-align: center;
  font-size: 24px;
  border-radius: 8px;
  border: 2px solid #FFA84C;
  background-color: #F4F4F4;
  outline: none;
  transition: border-color 0.2s ease, color 0.2s ease;
}

.code-input:focus {
  border-color: #FF7A00;
}

.code-error {
  border-color: #E63946 !important;
  color: #E63946;
}

.code-success {
  border-color: #8ED76A !important;
  color: #000;
}

.shake {
  animation: shake 0.3s ease;
}

@keyframes shake {
  0% { transform: translateX(0); }
  20% { transform: translateX(-5px); }
  40% { transform: translateX(5px); }
  60% { transform: translateX(-5px); }
  80% { transform: translateX(5px); }
  100% { transform: translateX(0); }
}

.resend-btn {
  width: 362px;
  height: 48px;
  margin-top: 12px;
  background: #f4f4f4;
  color: #555555;
  border-radius: 18px;
  border: none;
  font-size: 18px;
  font-weight: 600;
  cursor: pointer;
  transition: color 0.2s ease;
}

.resend-btn:disabled {
  cursor: not-allowed;
  color: #999999;
}

.resend-btn:hover:enabled,
.resend-btn:active:enabled {
  color: #FF7A00;
}

.submit-btn.inactive-btn {
  background-color: #FFA84C;
  color: white;
  cursor: not-allowed;
}
</style>