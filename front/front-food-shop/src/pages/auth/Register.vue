<template>
  <div class="page-bg">
    <div class="login-card">
      <h1 class="login-title">Регистрация</h1>

      <form @submit.prevent="handleSubmit" class="form">

        <!-- Email -->
        <div class="field-wrap">
          <div :class="['field', errors.email ? 'field-error' : '']">
            <input
              v-model="email"
              type="email"
              placeholder="Почта"
              class="input"
            />
          </div>
          <p v-if="errors.email" class="error-text">{{ errors.email }}</p>
        </div>

        <!-- Password -->
        <div class="field-wrap">
          <div :class="['field', errors.password ? 'field-error' : '']">
            <input
              v-model="password"
              :type="showPassword ? 'text' : 'password'"
              placeholder="Пароль"
              class="input"
              maxlength="30"
              @input="onPasswordInput"
            />
            <button type="button" class="show-btn" @click="showPassword = !showPassword">
              {{ showPassword ? '🙈' : '👁️' }}
            </button>
          </div>
          <p v-if="errors.password" class="error-text">{{ errors.password }}</p>

          <transition name="fade-slide">
            <div v-if="showPasswordStrength" class="password-strength-wrapper">
              <p class="contact-text" style="font-size: 16px; text-align: left; margin-bottom: 4px;">Сложность пароля:</p>
              <div class="password-strength">
                <div class="strength-bar" :style="{ width: strengthWidth, background: strengthColor }"></div>
              </div>
            </div>
          </transition>
        </div>

        <p class="contact-text" style="margin-top: 8px; font-size: 12px;">
          Пароль должен содержать не менее 8 символов, включая <br>
          латинские буквы (a-z, A-Z), как минимум одну заглавную<br>
          букву и одну цифру
        </p>

        <button
          type="submit"
          class="submit-btn"
          :class="{ 'inactive-btn': !isFormValid }"
          :disabled="!isFormValid"
        >
          Далее
        </button>
      </form>

      <transition name="fade-slide-btn">
        <button v-if="showAltButton" class="create-btn" @click="handleLogin">
          У меня уже есть аккаунт
        </button>
      </transition>

      <p class="contact-text" style="margin-top: 16px;">
        По всем вопросам можете обращаться:<br>
        adminexample@gmail.com
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { createUser } from "@/services/api";

const router = useRouter();
const email = ref("");
const password = ref("");
const showPassword = ref(false);

const errors = ref({ email: null, password: null });
const passwordStrength = ref(0);
const showPasswordStrength = ref(false);

/* --- VALIDATION --- */

const isFormValid = computed(() => {
  return email.value.includes("@") && password.value.length >= 8;
});

const showAltButton = computed(() => !email.value && !password.value);

/* --- SUBMIT --- */

const handleSubmit = async () => {
  errors.value = { email: null, password: null };

  if (!email.value) errors.value.email = "Почта не указана";
  else if (!email.value.includes("@")) errors.value.email = "Неверный формат почты";

  if (!password.value) errors.value.password = "Пароль не указан";
  else if (password.value.length < 8) errors.value.password = "Пароль слишком короткий";

  if (errors.value.email || errors.value.password) return;
  let claims = [];
  let claim = { type: "role", value: "user" };
  claims.push(claim);
  try {
    const response = await createUser(email.value, password.value, claims);
    if (response.userId) {
      localStorage.setItem("UserId", response.userId);
      router.push("/confirm-email");
    }
  } catch (error) {
    console.log(error);
  }
};

/* --- PASSWORD STRENGTH --- */

const onPasswordInput = () => {
  updatePasswordStrength();

  if (password.value.length > 0) {
    setTimeout(() => showPasswordStrength.value = true, 50);
  } else {
    showPasswordStrength.value = false;
  }
};

const updatePasswordStrength = () => {
  const pass = password.value;

  let score = 1;

  // --- длина ---
  if (pass.length >= 8) score++;
  if (pass.length >= 12) score++;
  if (pass.length >= 15) score++;

  // --- композиция ---
  const hasLower = /[a-z]/.test(pass);
  const hasUpper = /[A-Z]/.test(pass);
  const hasDigits = /\d/.test(pass);
  const hasSymbols = /[^A-Za-z0-9]/.test(pass);

  if (hasLower && hasUpper) score++;
  if (hasDigits) score++;
  if (hasSymbols) score++;

  // --- оценка для простых паттернов ---
  const lowerPass = pass.toLowerCase();
  const isSequence =
    "abcdefghijklmnopqrstuvwxyz".includes(lowerPass) ||
    "qwertyuiopasdfghjklzxcvbnm".includes(lowerPass) ||
    "0123456789".includes(pass);

  if (isSequence) score = Math.max(1, score - 2);

  passwordStrength.value = Math.min(score, 6);
};

const strengthWidth = computed(() => {
  if (!password.value) return "10%";
  return `${passwordStrength.value * 15 + 10}%`; // до 100%
});

const strengthColor = computed(() => {
  const s = passwordStrength.value;
  if (s <= 2) return "#E63946";  // красный
  if (s <= 4) return "#FFA84C";  // желтый
  return "#8ED76A";              // зеленый
});

const handleLogin = () => router.push("/login");
</script>

<style scoped>
@import './auth.css';

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

.fade-slide-btn-enter-active, .fade-slide-btn-leave-active {
  transition: all 0.2s ease;
}
.fade-slide-btn-enter-from, .fade-slide-btn-leave-to {
  opacity: 0;
  transform: translateY(0);
}
.fade-slide-btn-enter-to, .fade-slide-btn-leave-from {
  opacity: 1;
  transform: translateY(0);
}

.password-strength {
  margin-top: 8px;
  margin-bottom: 8px;
  display: flex;
  flex-direction: column;
}

.strength-bar {
  height: 8px;
  border-radius: 4px;
  transition: width 0.3s ease, background 0.3s ease;
}

.submit-btn.inactive-btn {
  background-color: #FFA84C;
  color: white;
  cursor: not-allowed;
}

.submit-btn:enabled:hover {
  background-color: #f4f4f4;
  color: #ff7a00;
}
</style>