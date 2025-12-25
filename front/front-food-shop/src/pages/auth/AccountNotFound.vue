<template>
  <div class="page-bg">
    <div class="login-card not-found-card">
      <h1 class="login-title">Аккаунт не найден</h1>

      <p class="not-found-text">
        Похоже, такого аккаунта не существует
      </p>

      <button class="submit-btn" @click="handleRetry">
        Повторить попытку
      </button>

      <p class="contact-text">
        По всем вопросам можете обращаться:<br>
        adminexample@gmail.com
      </p>
    </div>
  </div>
</template>

<script setup>
import router from "@/router";

const handleRetry = () => {
  router.push("/login");
};
</script>

<style scoped>
@import "./auth.css";

/* Фиксируем размер карточки 410x332 */
.not-found-card {
  width: 410px;
  height: 332px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  text-align: center;
}

/* Текст под заголовком */
.not-found-text {
  margin: 20px 0 30px;
  font-size: 15px;
  color: #8a8a8a;
  line-height: 1.4;
}
</style>