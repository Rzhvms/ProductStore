<template>
  <div class="page-bg">
    <div class="login-card" style="height: 920px;">
      <h1 class="login-title" v-html="$t('auth.finishRegistration.title')"></h1>

      <form class="form" @submit.prevent="handleSubmit">

        <!-- Имя -->
        <div class="field-wrap">
          <p class="field-label" v-html="$t('auth.finishRegistration.nameLabel')"></p>
          <div class="field">
            <input v-model="name" type="text" :placeholder="$t('auth.finishRegistration.namePlaceholder')" class="input" maxlength="40" />
          </div>
        </div>

        <!-- Фамилия -->
        <div class="field-wrap">
          <p class="field-label" v-html="$t('auth.finishRegistration.lastNameLabel')"></p>
          <div class="field">
            <input v-model="lastName" type="text" :placeholder="$t('auth.finishRegistration.lastNamePlaceholder')" class="input" maxlength="40" />
          </div>
        </div>

        <!-- Пол -->
        <div class="field-wrap">
          <p class="field-label" v-html="$t('auth.finishRegistration.genderLabel')"></p>

          <div class="gender-switch">
            <div v-if="gender" class="gender-slider" :class="gender"></div>

            <div class="gender-option"
              :class="{ active: gender === 'female' }"
              @click="gender = 'female'">
              {{ $t('auth.finishRegistration.genderFemale') }}
            </div>

            <div class="gender-option"
              :class="{ active: gender === 'male' }"
              @click="gender = 'male'">
              {{ $t('auth.finishRegistration.genderMale') }}
            </div>
          </div>
        </div>

        <!-- Дата рождения -->
        <div class="field-wrap">
          <p class="field-label" v-html="$t('auth.finishRegistration.birthLabel')"></p>

          <div class="field">
            <input
              class="input"
              type="text"
              :placeholder="$t('auth.finishRegistration.birthPlaceholder')"
              :value="birthDate"
              @keydown="onBirthKeyDown"
              maxlength="10"
            />
          </div>

          <transition name="fade" mode="out-in">
            <p v-if="birthError" key="birthError" class="error-text">{{ birthError }}</p>
          </transition>
        </div>

        <!-- Телефон -->
        <div class="field-wrap">
          <p class="field-label" v-html="$t('auth.finishRegistration.phoneLabel')"></p>
          <div class="field">
            <input
              type="tel"
              :value="formattedPhone"
              @keydown="onPhoneKeyDown"
              :placeholder="$t('auth.finishRegistration.phonePlaceholder')"
              class="input"
              maxlength="18"
            />
          </div>
        </div>

        <!-- Чекбоксы -->
        <div class="field-wrap" style="margin-bottom: 0px;">
          <label class="remember" style="font-size: 12px;">
            <input type="checkbox" v-model="agreeTerms" class="custom-checkbox" />
            <span v-html="$t('auth.finishRegistration.termsLabel')"></span>
          </label>

          <label class="remember" style="margin-top: 12px; font-size: 12px;">
            <input type="checkbox" v-model="agreeNews" class="custom-checkbox" />
            {{ $t('auth.finishRegistration.newsLabel') }}
          </label>
        </div>

        <button
          type="submit"
          class="submit-btn"
          :disabled="!isFormValid"
          :class="{ inactive: !isFormValid }"
        >
          {{ $t('auth.finishRegistration.submit') }}
        </button>

      </form>

      <p class="contact-text" style="margin-top: 16px;">
        {{ $t('auth.finishRegistration.contact.line1') }}<br>
        adminexample@gmail.com
      </p>

      <!-- Language switch -->
      <div class="lang-switch">
        <span
          :class="{ active: currentLocale === 'ru' }"
          @click="setLanguage('ru')"
        >
          RU
        </span>
        |
        <span
          :class="{ active: currentLocale === 'en' }"
          @click="setLanguage('en')"
        >
          EN
        </span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import { useI18n } from "vue-i18n";
import router from "@/router";
import { finishRegister } from "@/services/api";

const { locale } = useI18n();

const currentLocale = computed(() => locale.value);

const setLanguage = (lang) => {
  locale.value = lang;
  if (typeof localStorage !== "undefined") {
    localStorage.setItem("lang", lang);
  }
};

const name = ref("");
const lastName = ref("");
const gender = ref("");

const birthDate = ref("");
const birthDigits = ref("");
const birthError = ref("");

const phoneDigits = ref("");

const agreeTerms = ref(false);
const agreeNews = ref(false);

const formatBirth = () => {
  const v = birthDigits.value;
  if (v.length >= 5)
    birthDate.value = `${v.slice(0,2)}.${v.slice(2,4)}.${v.slice(4)}`;
  else if (v.length >= 3)
    birthDate.value = `${v.slice(0,2)}.${v.slice(2)}`;
  else
    birthDate.value = v;
};

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

const handleSubmit = async () => {
  if (!isFormValid.value) return;
  const d = birthDigits.value;
  const day = Number(d.slice(0, 2));
  const month = Number(d.slice(2, 4)) - 1;
  const year = Number(d.slice(4));
  const userDate = new Date(year, month, day);
  let notif = {
    email: agreeNews.value,
    sms: agreeNews.value,
    push: agreeNews.value
  };
  try {
    const userId = localStorage.getItem("userId");
    await finishRegister(userId, name.value, lastName.value, phoneDigits.value, gender.value, userDate, JSON.stringify(notif));
    router.push("/admin");
  } catch (error) {
    console.log(error);
  }
};
</script>

<style scoped>
@import "./auth.css";

.contact-text {
    margin-top: 12px;
    margin-bottom: 5px;
    font-size: 12px;
    color: #7a7a7a;
    text-align: center;
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

/* Язык */
.lang-switch span {
  cursor: pointer;
  opacity: 0.6;
}

.lang-switch span.active {
  font-weight: 600;
  opacity: 1;
  color: #ff7a00;
}
</style>