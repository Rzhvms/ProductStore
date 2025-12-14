<template> 
  <div class="page-bg">
    <div class="login-card" :style="{ height: cardHeight + 'px' }">
      <h1 class="login-title">Регистрация</h1>

      <form @submit.prevent="handleSubmit" class="form">

        <!-- Email -->
        <div class="field-wrap">
          <div class="field">
            <input
              v-model="email"
              type="email"
              placeholder="Введите почту"
              class="input"
              @input="email = sanitizeInput(email)"
            />
          </div>
          <transition name="fade-slide">
            <p v-if="errors.email" class="error-text">{{ errors.email }}</p>
          </transition>
        </div>

        <!-- Новый пароль -->
        <div class="field-wrap">
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
              <p class="contact-text" style="font-size: 16px; text-align: left; margin-bottom: 4px;">
                Сложность пароля:
              </p>
              <div class="password-strength-bg">
                <div class="strength-bar" :style="{ width: strengthWidth.password, background: strengthColor.password }"></div>
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
              placeholder="Повторите пароль"
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
              Пароли не совпадают
            </p>
          </transition>
        </div>

        <p class="password-rules">
          Пароль должен содержать не менее 8 символов, включая<br>
          латинские буквы (a-z, A-Z), как минимум одну заглавную<br>
          букву и одну цифру
        </p>

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
          {{ isSaved ? 'Аккаунт создан!' : 'Далее' }}
        </button>

      </form>

      <!-- Кнопка "У меня уже есть аккаунт" -->
      <button class="create-btn" @click="handleLogin" style="margin-top: 12px;">
        У меня уже есть аккаунт
      </button>

      <!-- Контактная информация -->
      <p class="contact-text">
        По всем вопросам можете обращаться:<br>
        adminexample@gmail.com
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, reactive, watch } from "vue";
import { useRouter } from "vue-router";
import { createUser } from "@/services/api";

const router = useRouter();
const email = ref("");
const password = ref("");
const confirmPassword = ref("");
const showPassword = reactive({ password: false, confirm: false });
const showPasswordStrength = reactive({ password: false });
const errorMessage = ref("");
const isSaved = ref(false);
const passwordStrength = ref(0);
const errors = ref({ email: null, password: null });

// Базовая высота карты
const baseCardHeight = 570;
const cardHeight = ref(baseCardHeight);

// --- САНИТАЙЗИНГ ---
const sanitizeInput = (value) => value.replace(/\s/g, '');

// --- ВАЛИДАЦИЯ ---
const isFormValid = computed(() => {
  return (
    email.value.includes("@") &&
    password.value.length >= 8 &&
    password.value === confirmPassword.value &&
    password.value.trim().length > 0
  );
});

// --- PASSWORD STRENGTH ---
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

// --- COMPUTED BARS ---
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

const strengthLabel = computed(() => {
  const s = passwordStrength.value;
  if (s <= 2) return "Слабый";
  if (s <= 4) return "Средний";
  return "Сильный";
});

// --- PASSWORDS MISMATCH ---
const passwordsMismatch = computed(() => confirmPassword.value && password.value !== confirmPassword.value);

// --- Обновление высоты карты ---
const updateCardHeight = () => {
  let extraHeight = 0;
  if (showPasswordStrength.password) extraHeight += 72; // высота блока сложности пароля
  if (passwordsMismatch.value) extraHeight += 24; // высота ошибки "Пароли не совпадают"
  cardHeight.value = baseCardHeight + extraHeight;
};

// Следим за изменениями пароля, подтверждения и блока сложности пароля
watch(
  [password, confirmPassword, () => showPasswordStrength.password],
  updateCardHeight,
  { immediate: true }
);

// --- SUBMIT ---
const handleSubmit = async () => {
  if (!isFormValid.value) {
    errorMessage.value = 'Пароли не совпадают, меньше 8 символов или содержат пробелы';
    return;
  }

  errors.value = { email: null, password: null };
  isSaved.value = true;
  errorMessage.value = '';

  try {
    const claims = [{ type: "role", value: "user" }];
    const response = await createUser(email.value, password.value, claims);
    if (response.userId) {
      localStorage.setItem("email", email.value);
      localStorage.setItem("userId", response.userId);
      setTimeout(() => router.push("/confirm-email"), 500);
    }
  } catch (err) {
    console.log(err);
  }
};

const handleLogin = () => router.push("/login");
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