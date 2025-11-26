<template>
  <div class="page-bg">
    <div class="login-card" :style="{ height: cardHeight + 'px' }">
      <h1 class="login-title">Создайте новый пароль</h1>

      <form @submit.prevent="handleSubmit" class="form">

        <!-- Новый пароль -->
        <div class="field-wrap">
          <p>Новый пароль: <span style="color:red">*</span></p>
          <div class="field password-field">
            <input
              v-model="password"
              :type="showPassword.password ? 'text' : 'password'"
              placeholder="Введите пароль"
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
              <p class="contact-text" style="font-size: 16px; text-align: left; margin-bottom: 4px;">Сложность пароля:</p>
              <div class="password-strength">
                <div class="strength-bar" :style="{ width: strengthWidth.password, background: strengthColor.password }"></div>
              </div>
            </div>
          </transition>
        </div>

        <!-- Подтверждение пароля -->
        <div class="field-wrap">
          <p>Подтверждение пароля: <span style="color:red">*</span></p>
          <div class="field password-field">
            <input
              v-model="confirmPassword"
              :type="showPassword.confirm ? 'text' : 'password'"
              placeholder="Повторите пароль"
              class="input"
              maxlength="30"
            />
            <button type="button" class="show-btn" @click="showPassword.confirm = !showPassword.confirm">
              {{ showPassword.confirm ? '🙈' : '👁️' }}
            </button>
          </div>

          <!-- Ошибка совпадения -->
          <p v-if="confirmPassword && password !== confirmPassword" class="error-text" style="margin-bottom: 0px;">
            Пароли не совпадают
          </p>
        </div>

        <!-- Общая ошибка -->
        <p v-if="errorMessage" class="error-text" >{{ errorMessage }}</p>

        <!-- Сохранить -->
        <button
          type="submit"
          class="submit-btn"
          :class="{ 'inactive-btn': !isFormValid || isSaved }"
          :disabled="!isFormValid || isSaved"
          style="margin-top: 12px;"
        >
          {{ isSaved ? 'Пароль изменён!' : 'Сохранить' }}
        </button>

        <!-- Контактная информация -->
        <p class="contact-text" style="margin-top: 12px; font-size: 14px;">
          По всем вопросам можете обращаться:<br>
          adminexample@gmail.com
        </p>

      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import router from '@/router';

const password = ref('');
const confirmPassword = ref('');
const showPassword = ref({ password: false, confirm: false });
const errorMessage = ref('');
const isSaved = ref(false);
const showPasswordStrength = ref({ password: false });
const passwordStrength = ref(0);

// Начальная фиксированная высота карточки
const baseCardHeight = 560;
const cardHeight = ref(baseCardHeight);

const isFormValid = computed(() => {
  return (
    password.value.length >= 8 &&
    password.value === confirmPassword.value
  );
});

/* --- Password Strength --- */
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
  const s = updatePasswordStrength(password.value);
  passwordStrength.value = s;
  showPasswordStrength.value.password = password.value.length > 0;

  // Расчет дополнительной высоты для слайдера и ошибки
  let extraHeight = 0;
  if (showPasswordStrength.value.password) extraHeight += 40;
  if (confirmPassword.value && password !== confirmPassword) extraHeight += 24;

  cardHeight.value = baseCardHeight + extraHeight;
};

/* --- Computed bars --- */
const strengthWidth = computed(() => ({
  password: password.value ? `${passwordStrength.value * 15 + 10}%` : '10%'
}));

const strengthColor = computed(() => ({
  password: (() => {
    const s = passwordStrength.value;
    if (s <= 2) return "#E63946";
    if (s <= 4) return "#FFA84C";
    return "#8ED76A";
  })()
}));

/* --- Submit --- */
const handleSubmit = async () => {
  if (!isFormValid.value) {
    errorMessage.value = 'Пароли не совпадают или меньше 8 символов';
    return;
  }

  isSaved.value = true;
  errorMessage.value = '';

  setTimeout(() => router.push('/login'), 5000);
};
</script>

<style scoped>
@import './auth.css';

.field-wrap p { font-weight: 600; }

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
  margin-top: 8px;
}

.password-strength {
  height: 8px;
  border-radius: 4px;
  background: #f4f4f4;
}

.strength-bar {
  height: 100%;
  border-radius: 4px;
  transition: width 0.3s ease, background 0.3s ease;
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

.submit-btn.inactive-btn {
  background-color: #FFA84C;
  color: white;
  cursor: not-allowed;
}

.saved-message {
  font-size: 16px;
  font-weight: 600;
  margin-top: 16px;
}
</style>