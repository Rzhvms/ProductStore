<template>
  <div class="page-bg">
    <div class="login-card" style="height: 920px;">
      <h1 class="login-title">Завершение регистрации</h1>

      <form class="form" @submit.prevent="handleSubmit">

        <!-- Имя -->
        <div class="field-wrap">
          <p class="field-label">
            Введите ваше имя: <span class="required">*</span>
          </p>
          <div class="field">
            <input v-model="name" type="text" placeholder="Олег" class="input" maxlength="40" />
          </div>
        </div>

        <!-- Фамилия -->
        <div class="field-wrap">
          <p class="field-label">
            Введите вашу фамилию: <span class="required">*</span>
          </p>
          <div class="field">
            <input v-model="lastName" type="text" placeholder="Волков" class="input" maxlength="40" />
          </div>
        </div>

        <!-- Пол -->
        <div class="field-wrap">
          <p class="field-label">
            Ваш пол: <span class="required">*</span>
          </p>

          <div class="gender-switch">
            <div v-if="gender" class="gender-slider" :class="gender"></div>

            <div class="gender-option"
              :class="{ active: gender === 'female' }"
              @click="gender = 'female'">Женский</div>

            <div class="gender-option"
              :class="{ active: gender === 'male' }"
              @click="gender = 'male'">Мужской</div>
          </div>
        </div>

        <!-- Дата рождения -->
        <div class="field-wrap">
          <p class="field-label">
            Введите вашу дату рождения: <span class="required">*</span>
          </p>

          <div class="field">
            <input
              class="input"
              type="text"
              placeholder="ДД.ММ.ГГГГ"
              :value="birthDate"
              @keydown="onBirthKeyDown"
              maxlength="10"
            />
          </div>

          <!-- Плавная анимация ошибок -->
          <transition name="fade" mode="out-in">
            <p v-if="birthError" key="birthError" class="error-text">{{ birthError }}</p>
          </transition>
        </div>

        <!-- Телефон -->
        <div class="field-wrap">
          <p class="field-label">
            Введите ваш номер телефона: <span class="required">*</span>
          </p>
          <div class="field">
            <input
              type="tel"
              :value="formattedPhone"
              @keydown="onPhoneKeyDown"
              placeholder="+7 (999) 999-99-99"
              class="input"
              maxlength="18"
            />
          </div>
        </div>

        <!-- Чекбоксы -->
        <div class="field-wrap" style="margin-bottom: 0px;">
          <label class="remember" style="font-size: 12px;">
            <input type="checkbox" v-model="agreeTerms" class="custom-checkbox" />
            Даю согласие на обработку персональных данных
            <span class="required">*</span>
          </label>

          <label class="remember" style="margin-top: 12px; font-size: 12px;">
            <input type="checkbox" v-model="agreeNews" class="custom-checkbox" />
            Я хочу получать уведомления о скидках и акциях
          </label>
        </div>

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
import { finishRegister } from "@/services/api";

const name = ref("");
const lastName = ref("");
const gender = ref("");

const birthDate = ref("");
const birthDigits = ref("");
const birthError = ref("");

const phoneDigits = ref("");

const agreeTerms = ref(false);
const agreeNews = ref(false);

// Форматирование даты
const formatBirth = () => {
  const v = birthDigits.value;
  if (v.length >= 5)
    birthDate.value = `${v.slice(0,2)}.${v.slice(2,4)}.${v.slice(4)}`;
  else if (v.length >= 3)
    birthDate.value = `${v.slice(0,2)}.${v.slice(2)}`;
  else
    birthDate.value = v;
};

// Проверка возраста и будущей даты
const validateBirthDate = () => {
  birthError.value = "";

  if (birthDigits.value.length !== 8) {
    birthError.value = "Введите дату полностью";
    return;
  }

  const d = birthDigits.value;
  const day = Number(d.slice(0, 2));
  const month = Number(d.slice(2, 4)) - 1;
  const year = Number(d.slice(4));

  const userDate = new Date(year, month, day);

  // Проверка корректности даты
  if (
    userDate.getFullYear() !== year ||
    userDate.getMonth() !== month ||
    userDate.getDate() !== day
  ) {
    birthError.value = "Некорректная дата";
    return;
  }

  const today = new Date();
  const minAgeDate = new Date(today.getFullYear() - 12, today.getMonth(), today.getDate());

  if (userDate > today) {
    birthError.value = "Дата рождения не может быть больше сегодняшней";
    return;
  }

  if (userDate > minAgeDate) {
    birthError.value = "Регистрация доступна только с 12 лет";
  }
};

// Ввод даты рождения
const onBirthKeyDown = (e) => {
  if (!/^\d$/.test(e.key) && e.key !== "Backspace") {
    e.preventDefault();
    return;
  }

  if (e.key === "Backspace") {
    birthDigits.value = birthDigits.value.slice(0, -1);
  } else if (birthDigits.value.length < 8) {
    birthDigits.value += e.key;
  }

  formatBirth();

  if (birthDigits.value.length === 8) {
    validateBirthDate();
  } else {
    birthError.value = "Введите дату полностью";
  }

  e.preventDefault();
};

// Ввод телефона
const onPhoneKeyDown = (e) => {
  if (!/^\d$/.test(e.key) && e.key !== "Backspace") {
    e.preventDefault();
    return;
  }

  if (e.key === "Backspace") {
    phoneDigits.value = phoneDigits.value.slice(0, -1);
  } else if (phoneDigits.value.length < 11) {
    phoneDigits.value += e.key;
  }

  e.preventDefault();
};

const formattedPhone = computed(() => {
  if (!phoneDigits.value) return "";

  const d = phoneDigits.value;
  let str = "+7";
  if (d.length > 1) str += ` (${d.slice(1, 4)}`;
  if (d.length >= 4) str += ")";
  if (d.length >= 4) str += ` ${d.slice(4, 7)}`;
  if (d.length >= 7) str += `-${d.slice(7, 9)}`;
  if (d.length >= 9) str += `-${d.slice(9, 11)}`;
  return str;
});

// Валидация формы
const isFormValid = computed(() => {
  return (
    name.value.trim() &&
    lastName.value.trim() &&
    gender.value &&
    birthDigits.value.length === 8 &&
    !birthError.value &&
    phoneDigits.value.length === 11 &&
    agreeTerms.value
  );
});

// Отправка формы
const handleSubmit = async () => {
  if (!isFormValid.value) return;
  const d = birthDigits.value;
  const day = Number(d.slice(0, 2));
  const month = Number(d.slice(2, 4)) - 1;
  const year = Number(d.slice(4));
  const userDate = new Date(year, month, day);
  try {
    const userId = localStorage.getItem("userId");
    const response = await finishRegister(userId, name.value, lastName.value, phoneDigits.value, gender.value, userDate);
    router.push("/admin");
  } catch (error) {
    console.log(error);
  }
};
</script>

<style scoped>
@import "./auth.css";

.gender-switch {
  width: 362px;
  height: 48px;
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
  height: 40px;
  border-radius: 16px;
  background: #ff7a00;
  z-index: 1;
  animation: fadeIn 0.35s forwards;
}

.gender-slider.female { left: 4px; }
.gender-slider.male { right: 4px; }

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
}

.gender-option.active { color: white; font-weight: 600; }

.submit-btn.inactive {
  background: #FFA84C;
  cursor: not-allowed;
}

.input {
  width: 200px;
  padding: 8px;
  font-size: 16px;
}

.error-text {
  color: #ff4d4f;
  font-size: 12px;
  margin-top: 4px;
}

/* Плавная анимация ошибок */
.fade-enter-active, .fade-leave-active {
  transition: all 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
  transform: translateY(-4px);
}
.fade-enter-to, .fade-leave-from {
  opacity: 1;
  transform: translateY(0);
}
</style>