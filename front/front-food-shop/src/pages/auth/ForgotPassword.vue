<template>
  <div class="page-bg">
    <!-- Карточка -->
    <div class="login-card" style="height: 564px;">
      <h1 class="login-title">Восстановление пароля</h1>

      <p class="helper-text">
        Введите вашу электронную почту, чтобы <br>
        получить код подтверждения и восстановить <br>
        доступ к аккаунту
      </p>

      <!-- Форма -->
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

        <!-- Submit button -->
        <button
          type="submit"
          class="submit-btn"
          :class="{ 'inactive-btn': !isFormValid }"
          :disabled="!isFormValid"
        >
          Отправить код
        </button>
      </form>

      <button class="create-btn" @click="handleDecline">
        Отмена
      </button>

      <p class="contact-text">
        По всем вопросам можете обращаться:<br>
        adminexample@gmail.com
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const email = ref("");

const errors = ref({ email: null });

// Валидность формы: есть @ и не пустая строка
const isFormValid = computed(() => {
  return email.value.length > 0 && email.value.includes("@");
});

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