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
              placeholder="25.10.2000"
              :value="birthDate"
              @keydown="onBirthKeyDown"
              maxlength="10"
            />
          </div>

          <p v-if="birthError" class="field-error">{{ birthError }}</p>
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
import { ref, computed, watch } from "vue";
import router from "@/router";

const name = ref("");
const lastName = ref("");
const gender = ref("");

const birthDate = ref("");
const birthDigits = ref("");
const birthError = ref("");

const phoneDigits = ref("");

const agreeTerms = ref(false);
const agreeNews = ref(false);

// Только цифры в дате рождения
const onBirthKeyDown = async (e) => {
  if (!/^\d$/.test(e.key) && e.key !== "Backspace") return e.preventDefault();

  if (e.key === "Backspace") {
    birthDigits.value = birthDigits.value.slice(0, -1);
  } else {
    if (birthDigits.value.length >= 8) return;
    birthDigits.value += e.key;
  }

  formatBirth();
  await validateBirthDate();
  e.preventDefault();
};

const formatBirth = () => {
  const v = birthDigits.value;
  if (v.length >= 5)
    birthDate.value = `${v.slice(0,2)}.${v.slice(2,4)}.${v.slice(4)}`;
  else if (v.length >= 3)
    birthDate.value = `${v.slice(0,2)}.${v.slice(2)}`;
  else
    birthDate.value = v;
};

// Только цифры в номере телефона
const onPhoneKeyDown = (e) => {
  if (!/^\d$/.test(e.key) && e.key !== "Backspace") return e.preventDefault();

  if (e.key === "Backspace") {
    phoneDigits.value = phoneDigits.value.slice(0, -1);
  } else {
    if (phoneDigits.value.length >= 11) return;
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

// Проверка даты по UTC
const validateBirthDate = async () => {
  birthError.value = "";

  if (birthDigits.value.length !== 8) {
    birthError.value = "Введите дату полностью";
    return;
  }

  const d = birthDigits.value;
  const userDate = new Date(`${d.slice(4)}-${d.slice(2,4)}-${d.slice(0,2)}T00:00:00Z`);

  if (isNaN(userDate.getTime())) {
    birthError.value = "Некорректная дата";
    return;
  }

  try {
    const res = await fetch("https://worldtimeapi.org/api/timezone/Etc/UTC");
    const data = await res.json();
    const todayUTC = new Date(data.datetime);

    if (userDate >= todayUTC) {
      birthError.value = "Дата рождения не может быть больше сегодняшней";
      return;
    }

    const minAgeDate = new Date(todayUTC);
    minAgeDate.setFullYear(minAgeDate.getFullYear() - 12);

    if (userDate > minAgeDate) {
      birthError.value = "Регистрация доступна только с 12 лет";
      return;
    }

  } catch {
    birthError.value = "Ошибка проверки даты";
  }
};

// Проверка валидности формы
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

// Отправка
const handleSubmit = () => {
  if (!isFormValid.value) return;
  router.push("/admin");
};
</script>

<style scoped>
@import "./auth.css";

.field-error {
  color: red;
  font-size: 12px;
  margin-top: 4px;
}

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
</style>