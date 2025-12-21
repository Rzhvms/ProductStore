<template>
  <ProfileLayout>
    <!-- КАРТА ЛОЯЛЬНОСТИ -->
    <div class="loyalty-card">
      <div class="card-left">
        <div class="card-number">Карта лояльности</div>
        <div class="card-name">Имя Фамилия</div>
        <button class="status-btn">статус карты</button>
      </div>
      <div class="card-right">
        <div class="card-bonus-label">Ваши бонусы:</div>
        <div class="card-bonus-value">100</div>
        <div class="card-nums">000 000 000 000</div>
      </div>
    </div>

    <!-- ЛИЧНЫЕ ДАННЫЕ -->
    <div class="personal-data">
      <div class="pd-header">
        <h2>Личные данные</h2>

        <div v-if="!isEditing" class="pd-edit-btn" @click="enableEdit">
          Редактировать
        </div>

        <div v-else class="pd-edit-actions">
          <button class="pd-save" @click="saveChanges">Сохранить</button>
          <button class="pd-cancel" @click="cancelEdit">Отмена</button>
        </div>
      </div>

      <div class="form-grid-3x2">
        <div class="form-item">
          <label>Имя</label>
          <input type="text" v-model="form.name" :disabled="!isEditing" />
        </div>

        <div class="form-item">
          <label>Фамилия</label>
          <input type="text" v-model="form.surname" :disabled="!isEditing" />
        </div>

        <div class="form-item">
          <label>Дата рождения *</label>
          <input type="text" v-model="form.birth" disabled />
          <small>Для изменения необходимо обратиться в службу поддержки.</small>
        </div>

        <div class="form-item">
          <label>Почта</label>
          <input type="email" v-model="form.email" :disabled="!isEditing" />
        </div>

        <div class="form-item">
          <label>Номер телефона *</label>
          <input type="text" v-model="form.phone" disabled />
          <small>Для изменения необходимо обратиться в службу поддержки.</small>
        </div>

        <div class="form-item">
          <label>Пол</label>
          <div class="gender-switch" :class="{ disabled: !isEditing }">
            <div v-if="form.gender" class="gender-slider" :class="form.gender"></div>

            <div
              class="gender-option"
              :class="{ active: form.gender === 'female' }"
              @click="setGender('female')"
            >
              Женский
            </div>

            <div
              class="gender-option"
              :class="{ active: form.gender === 'male' }"
              @click="setGender('male')"
            >
              Мужской
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- ДОПОЛНИТЕЛЬНЫЕ НАСТРОЙКИ -->
    <div class="extra-settings">
      <h2>Дополнительные настройки</h2>

      <div class="notification-settings">
        <h3>Настройки уведомлений</h3>
        <p class="notif-desc">
          Выберите удобный способ получения рекламных и информационных сообщений:
        </p>

        <ul class="notif-list">
          <li>Уведомления о заказах и доставке</li>
          <li>Акции, персональные предложения и новости</li>
          <li>Рекомендации и сервисные напоминания</li>
        </ul>

        <div class="checkbox-group" :class="{ disabled: !isEditing }">
          <label class="checkbox-item">
            <input type="checkbox" class="custom-checkbox" v-model="form.notifEmail" :disabled="!isEditing" />
            Email-уведомления
          </label>

          <label class="checkbox-item">
            <input type="checkbox" class="custom-checkbox" v-model="form.notifSMS" :disabled="!isEditing" />
            SMS-уведомления
          </label>

          <label class="checkbox-item">
            <input type="checkbox" class="custom-checkbox" v-model="form.notifPush" :disabled="!isEditing" />
            Push-уведомления
          </label>
        </div>
      </div>

      <div class="account-management">
        <h3>Управление аккаунтом</h3>
        <div class="account-actions">
          <button class="acc-btn change-pass">Сменить пароль</button>
          <button class="acc-btn delete-acc">Удалить аккаунт</button>
        </div>
      </div>
    </div>

    <button class="contact-us-btn">Связаться с нами</button>
  </ProfileLayout>
</template>

<script>
import ProfileLayout from "./ProfileLayout.vue";
import { getUser, updateUser } from "@/services/api";

export default {
  name: "ProfilePage",
  components: { ProfileLayout },

  data() {
    return {
      isEditing: false,
      original: {},
      form: {
        name: "",
        surname: "",
        birth: "",
        email: "",
        phone: "",
        gender: "",
        notifEmail: true,
        notifSMS: false,
        notifPush: true
      }
    };
  },

  async created() {
    const idToken = localStorage.getItem("idToken");
    const user = await getUser(idToken);
    const notifJson = JSON.parse(user.about) || {};
    this.form.name = user.name;
    this.form.surname = user.lastName;
    const date = new Date(user.birthDate);
    const day = date.getDate();
    let month = date.getMonth() + 1;
    if (month < 10) month = `0${month}`;
    const year = date.getFullYear();
    this.form.birth = `${day}.${month}.${year}`;
    this.form.email = user.email;
    this.form.phone = user.phone;
    this.form.gender = user.gender;
    this.form.notifEmail = (notifJson && notifJson.email) ? notifJson.email : false;
    this.form.notifSMS = (notifJson && notifJson.sms) ? notifJson.sms : false;
    this.form.notifPush = (notifJson && notifJson.push) ? notifJson.push : false;
  },

  methods: {
    enableEdit() {
      this.original = { ...this.form };
      this.isEditing = true;
    },
    saveChanges() {
      const idToken = localStorage.getItem("idToken");
      const notifJson = {
        email: this.form.notifEmail,
        sms: this.form.notifSMS,
        push: this.form.notifPush
      };
      const name = this.form.name;
      const lastName = this.form.surname;
      const birth = this.form.birth;
      let birthDate = birth.split(".");
      birthDate = birthDate[2] + "-" + birthDate[1] + "-" + birthDate[0];
      const birthDateObj = new Date(birthDate).toISOString();
      const email = this.form.email;
      const phone = this.form.phone;
      const gender = this.form.gender;
      const json = { 
        name: name,
        lastName: lastName,
        email: email,
        phone: phone,
        gender: gender,
        birthDate: birthDateObj,
        about: JSON.stringify(notifJson)
       };
      updateUser(idToken, json);
      this.isEditing = false;
    },
    cancelEdit() {
      this.form = { ...this.original };
      this.isEditing = false;
    },
    setGender(gender) {
      this.form.gender = gender;
    }
  }
};
</script>

<style src="./profile.css"></style>
