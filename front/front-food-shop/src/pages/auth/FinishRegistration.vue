<template>
  <div class="auth-container">
    <!-- Карточка -->
    <div class="auth-card" style="height: 832px;">
      <!-- Заголовок -->
      <h1 class="auth-title">Завершение регистрации</h1>

      <!-- Форма -->
      <form @submit.prevent="handleSubmit" class="auth-form">
        <div class="field-wrap">
          <label class="field-label">Введите ваше имя:</label>
          <input
            v-model="name"
            type="text"
            placeholder="Олег"
            class="input"
          />
        </div>

        <div class="field-wrap">
          <label class="field-label">Введите вашу фамилию:</label>
          <input
            v-model="lastName"
            type="text"
            placeholder="Волков"
            class="input"
          />
        </div>

        <div class="field-wrap">
          <label class="field-label">Ваш пол:</label>
          <select v-model="gender" class="input">
            <option value="" disabled>Выберите пол</option>
            <option value="male">Мужской</option>
            <option value="female">Женский</option>
          </select>
        </div>

        <div class="field-wrap">
          <label class="field-label">Введите ваш номер телефона:</label>
          <input
            v-model="phone"
            type="tel"
            placeholder="+7 (...) ...-..-.."
            class="input"
          />
        </div>

        <!-- Submit -->
        <button type="submit" class="submit-btn" :disabled="!isFormValid">
          Завершить
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
import { ref, computed } from "vue";
import router from "@/router";
// import { createUser } from "@/services/api";

const name = ref("");
const lastName = ref("");
const gender = ref("");
const phone = ref("");

const isFormValid = computed(() => {
  return name.value && lastName.value && gender.value && phone.value;
});

const handleSubmit = () => {
  const email = localStorage.getItem("email");
  // Заглушка под API
  // createUser(email, name.value, lastName.value, gender.value, phone.value);
  router.push("/admin");
};
</script>

<style scoped>
@import './auth.css';

.field-label {
  display: block;
  font-size: 16px;
  margin-bottom: 6px;
  color: #3c3c3c;
}

.field-wrap {
  width: 362px;
  margin: 12px auto;
}

.input, select {
  width: 100%;
  height: 48px;
  padding: 0 16px;
  border-radius: 18px;
  border: none;
  background-color: #f4f4f4;
  font-size: 16px;
  outline: none;
}

.submit-btn {
  width: 362px;
  height: 48px;
  background: #ff7a00;
  color: white;
  border-radius: 18px;
  border: none;
  font-size: 18px;
  font-weight: 600;
  margin-top: 32px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.submit-btn:hover {
  background: #ffa84c;
}

.submit-btn:disabled {
  background: #FFA84C;
  cursor: not-allowed;
}
</style>