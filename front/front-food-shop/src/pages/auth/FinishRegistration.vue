<template>
  <div class="page-bg">
    <div class="login-card" style="height: 832px;">
      <h1 class="login-title">Завершение регистрации</h1>

      <form class="form" @submit.prevent="handleSubmit">

        <!-- Имя -->
        <div class="field-wrap">
          <p style="margin-top: 0px; font-weight: 600;">
            Введите ваше имя: <span style="color:red">*</span>
          </p>
          <div class="field">
            <input v-model="name" type="text" placeholder="Олег" class="input" />
          </div>
        </div>

        <!-- Фамилия -->
        <div class="field-wrap">
          <p style="margin-top: 4px; font-weight: 600;">
            Введите вашу фамилию: <span style="color:red">*</span>
          </p>
          <div class="field">
            <input v-model="lastName" type="text" placeholder="Волков" class="input" />
          </div>
        </div>

        <!-- Пол -->
        <div class="field-wrap">
          <p style="margin-top: 4px; font-weight: 600;">
            Ваш пол: <span style="color:red">*</span>
          </p>

          <div class="gender-switch">
            <div v-if="gender" class="gender-slider" :class="gender"></div>

            <div
              class="gender-option"
              :class="{ active: gender === 'female' }"
              @click="gender = 'female'"
            >
              Женский
            </div>

            <div
              class="gender-option"
              :class="{ active: gender === 'male' }"
              @click="gender = 'male'"
            >
              Мужской
            </div>
          </div>
        </div>

        <!-- Телефон -->
        <div class="field-wrap">
          <p style="margin-top: 4px; font-weight: 600;">
            Введите ваш номер телефона: <span style="color:red">*</span>
          </p>
          <div class="field">
            <input
              type="tel"
              :value="formattedPhone"
              @focus="onPhoneFocus"
              @input="onPhoneInput($event)"
              @keydown="onPhoneKeyDown($event)"
              placeholder="+7 (999) 999-99-99"
              class="input"
            />
          </div>
        </div>

        <!-- Чекбоксы -->
        <div class="field-wrap" style="margin-bottom: 0px;">
          <label class="remember" style="font-size: 12px;">
            <input type="checkbox" v-model="agreeTerms" class="custom-checkbox" />
            Даю согласие на обработку персональных данных <span style="color:red">*</span>
          </label>

          <label class="remember" style="margin-top: 12px; font-size: 12px;">
            <input type="checkbox" v-model="agreeNews" class="custom-checkbox" />
            Я хочу получать уведомления о скидках и акциях
          </label>
        </div>

        <!-- Кнопка -->
        <button
          type="submit"
          class="submit-btn"
          :disabled="!isFormValid"
          :class="{ inactive: !isFormValid }"
        >
          Далее
        </button>

      </form>

      <p class="contact-text">
        По всем вопросам можете обращаться:<br />
        adminexample@gmail.com
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import router from "@/router";

const name = ref("");
const lastName = ref("");
const gender = ref("");

const phoneDigits = ref(""); // пустая строка
const agreeTerms = ref(false);
const agreeNews = ref(false);

// Валидность формы
const isFormValid = computed(() => {
  return (
    name.value.trim() &&
    lastName.value.trim() &&
    gender.value &&
    phoneDigits.value.length === 11 &&
    agreeTerms.value
  );
});

// Форматированный телефон для отображения
const formattedPhone = computed(() => {
  if (!phoneDigits.value) return ""; // пока нет цифр показываем пустое поле с плейсхолдером

  const d = phoneDigits.value;
  let str = "+7";
  if (d.length > 1) str += ` (${d.slice(1, 4)}`;
  if (d.length >= 4) str += ")";
  if (d.length >= 4) str += ` ${d.slice(4, 7)}`;
  if (d.length >= 7) str += `-${d.slice(7, 9)}`;
  if (d.length >= 9) str += `-${d.slice(9, 11)}`;
  return str;
});

// При фокусе вставляем +7, если поле пустое
const onPhoneFocus = () => {
  if (!phoneDigits.value) phoneDigits.value = "7";
};

// Обработка ввода
const onPhoneInput = (e) => {
  let val = e.target.value.replace(/\D/g, "");
  if (!val.startsWith("7")) val = "7" + val;
  if (val.length > 11) val = val.slice(0, 11);
  phoneDigits.value = val;
};

// Защита +7 от удаления
const onPhoneKeyDown = (e) => {
  if (
    (e.key === "Backspace" || e.key === "Delete") &&
    phoneDigits.value.length <= 1
  ) {
    e.preventDefault();
  }
};

// Отправка формы
const handleSubmit = async () => {
  if (!isFormValid.value) return;
  try {
    router.push("/admin");
  } catch (err) {
    console.error(err);
  }
};
</script>

<style scoped>
@import "./auth.css";

.gender-switch {
  width: 362px;
  height: 56px;
  background: #f4f4f4;
  border-radius: 18px;
  display: flex;
  position: relative;
  overflow: hidden;
}

.gender-slider {
  position: absolute;
  top: 4px;
  width: 50%;
  height: 48px;
  border-radius: 18px;
  background: #ff7a00;
  z-index: 1;
  transition: opacity 0.35s ease;
  opacity: 0;
  animation: fadeIn 0.35s forwards;
}

.gender-slider.female { left: 4px; opacity: 1; }
.gender-slider.male { right: 4px; opacity: 1; }

@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }

.gender-option {
  flex: 1;
  z-index: 2;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 18px;
  font-weight: 500;
  color: #7a7a7a;
  cursor: pointer;
  transition: color 0.25s ease;
}

.gender-option.active { color: white; font-weight: 600; }

.submit-btn.inactive {
  background: #FFA84C;
  cursor: not-allowed;
}
</style>