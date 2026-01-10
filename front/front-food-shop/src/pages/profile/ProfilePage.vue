<template>
  <ProfileLayout>
    <div class="loyalty-card">
      <div class="loyalty-left">
        <h3 class="loyalty-title">Карта лояльности</h3>
        <button class="status-btn">статус карты</button>
        <div class="loyalty-bonuses">
          Ваши бонусы: <span>100</span>
        </div>
      </div>
      
      <div class="loyalty-right">
        <div class="card-visual-placeholder"></div>
        <div class="card-number-text">000 000 000 000</div>
      </div>
    </div>

    <!-- ЛИЧНЫЕ ДАННЫЕ -->
    <div class="personal-data">
      <div class="pd-header">
        <h2>Личные данные</h2>

        <div v-if="!isEditing" class="pd-edit-btn" @click="enableEdit">
          <img src="../../assets/edit.svg" alt="edit" style="filter: invert(53%) sepia(15%) saturate(7005%) hue-rotate(358deg) brightness(101%) contrast(108%)"/>
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
            <div class="gender-option" :class="{ active: form.gender === 'female' }" @click="setGender('female')">
              Женский
            </div>
            <div class="gender-option" :class="{ active: form.gender === 'male' }" @click="setGender('male')">
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
          <button class="acc-btn change-pass" @click="openPasswordModal">Сменить пароль</button>
          <button class="acc-btn delete-acc">Удалить аккаунт</button>
        </div>
      </div>
    </div>

    <button class="contact-us-btn">Связаться с нами</button>
  </ProfileLayout>

  <!-- МОДАЛЬНОЕ ОКНО СМЕНЫ ПАРОЛЯ -->
  <div v-if="isPasswordModalOpen" class="modal-overlay" @click.self="closePasswordModal">
    <div class="modal-window">
      
      <div class="modal-header">
        <h2>Смена пароля</h2>
        <button class="close-btn" @click="closePasswordModal">&times;</button>
      </div>

      <div class="modal-body">
        <p class="description">
          Введите текущий пароль, чтобы подтвердить личность, а затем придумайте новый.
        </p>

        <!-- 1. Текущий пароль -->
        <div class="input-group">
          <label>Текущий пароль</label>
          <div class="input-wrapper">
            <input 
              :type="passVisibility.current ? 'text' : 'password'" 
              v-model="passForm.current"
              placeholder="••••••••"
              @input="passwordError = ''" 
            >
            <!-- При вводе сбрасываем ошибку (@input) -->
            <button class="toggle-visibility" @click="toggleVis('current')">
               {{ passVisibility.current ? '🙈' : '👁️' }}
            </button>
          </div>
        </div>

        <!-- 2. Новый пароль -->
        <div class="input-group">
          <label>Новый пароль</label>
          <div class="input-wrapper">
            <input 
              :type="passVisibility.new ? 'text' : 'password'" 
              v-model="passForm.new"
              placeholder="Придумайте сложный пароль"
              @input="passwordError = ''"
            >
            <button class="toggle-visibility" @click="toggleVis('new')">
              {{ passVisibility.new ? '🙈' : '👁️' }}
            </button>
          </div>
        </div>

        <!-- 3. Подтверждение -->
        <div class="input-group">
          <label>Повторите новый пароль</label>
          <div class="input-wrapper">
            <input 
              :type="passVisibility.confirm ? 'text' : 'password'" 
              v-model="passForm.confirm"
              placeholder="Повторите пароль"
              :style="passwordsMismatch ? 'border-color: #E53935;' : ''"
              @input="passwordError = ''"
            >
             <button class="toggle-visibility" @click="toggleVis('confirm')">
              {{ passVisibility.confirm ? '🙈' : '👁️' }}
            </button>
          </div>
          <span class="error-text" v-if="passwordsMismatch">Пароли не совпадают</span>
        </div>

        <!-- ГЛОБАЛЬНАЯ ОШИБКА -->
        <div v-if="passwordError" class="global-error-message">
          ⚠️ {{ passwordError }}
        </div>

      </div>

      <div class="modal-footer">
        <button class="btn-cancel" @click="closePasswordModal">Отмена</button>
        <button 
          class="btn-save" 
          @click="submitPasswordChange"
          :disabled="!isFormValid"
          :style="!isFormValid ? 'opacity: 0.6; cursor: not-allowed;' : ''"
        >
          Сохранить
        </button>
      </div>

    </div>
  </div>
</template>

<script>
import ProfileLayout from "./ProfileLayout.vue";
import { getUser, updateUser, updatePassword } from "@/services/api";

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
      },
      
      isPasswordModalOpen: false,
      passwordError: "",
      passForm: {
        current: "",
        new: "",
        confirm: ""
      },
      passVisibility: {
        current: false,
        new: false,
        confirm: false
      }
    };
  },

  computed: {
    passwordsMismatch() {
      return this.passForm.new && this.passForm.confirm && (this.passForm.new !== this.passForm.confirm);
    },
    isFormValid() {
      return (
        this.passForm.current.length > 0 &&
        this.passForm.new.length >= 6 &&
        this.passForm.new === this.passForm.confirm
      );
    }
  },

  async created() {
    try {
      const user = await getUser();
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
      this.form.phone = '+' + user.phone;
      this.form.gender = user.gender;
      this.form.notifEmail = (notifJson && notifJson.email) ? notifJson.email : false;
      this.form.notifSMS = (notifJson && notifJson.sms) ? notifJson.sms : false;
      this.form.notifPush = (notifJson && notifJson.push) ? notifJson.push : false;
    } catch (e) {
      console.error("Ошибка загрузки профиля", e);
    }
  },

  methods: {
    enableEdit() {
      this.original = { ...this.form };
      this.isEditing = true;
    },
    async saveChanges() {
      const notifJson = {
        email: this.form.notifEmail,
        sms: this.form.notifSMS,
        push: this.form.notifPush
      };
      
      let birthDate = this.form.birth.split(".");
      let formattedDate = birthDate[2] + "-" + birthDate[1] + "-" + birthDate[0];
      const birthDateObj = new Date(formattedDate).toISOString();

      const json = { 
        name: this.form.name,
        lastName: this.form.surname,
        email: this.form.email,
        phone: this.form.phone.replace('+', ''),
        gender: this.form.gender,
        birthDate: birthDateObj,
        about: JSON.stringify(notifJson)
       };
      await updateUser(json);
      this.isEditing = false;
    },
    cancelEdit() {
      this.form = { ...this.original };
      this.isEditing = false;
    },
    setGender(gender) {
      if (this.isEditing) {
        this.form.gender = gender;
      }
    },

    openPasswordModal() {
      this.passForm = { current: "", new: "", confirm: "" };
      this.passVisibility = { current: false, new: false, confirm: false };
      this.passwordError = "";
      this.isPasswordModalOpen = true;
    },
    closePasswordModal() {
      this.isPasswordModalOpen = false;
    },
    toggleVis(field) {
      this.passVisibility[field] = !this.passVisibility[field];
    },
    async submitPasswordChange() {
      this.passwordError = "";
      if (!this.isFormValid) return;

      try {
        await updatePassword(this.passForm.current, this.passForm.new);
        this.closePasswordModal();
      } catch (error) {
        console.error("Ошибка смены пароля", error);
        if (error) {
          this.passwordError = error.message;
        } else {
          this.passwordError = "Неверный текущий пароль или ошибка сервера.";
        }
      }
    }
  }
};
</script>

<style scoped>
@import './profile.css';

.modal-overlay {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0, 0, 0, 0.4);
  backdrop-filter: blur(3px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-window {
  background: #fff;
  width: 420px;
  padding: 30px;
  border-radius: 20px;
  box-shadow: 0 10px 25px rgba(0,0,0,0.1);
  font-family: 'Inter', sans-serif;
  animation: fadeIn 0.2s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: scale(0.95); }
  to { opacity: 1; transform: scale(1); }
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}
.modal-header h2 {
  margin: 0;
  font-size: 20px;
  color: #111;
}
.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #999;
}

.description {
  font-size: 14px;
  color: #666;
  margin-bottom: 20px;
  line-height: 1.4;
}

.input-group {
  margin-bottom: 16px;
}
.input-group label {
  display: block;
  font-size: 12px;
  color: #888;
  margin-bottom: 6px;
  font-weight: 500;
}
.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
}
.input-wrapper input {
  width: 100%;
  padding: 12px 16px;
  padding-right: 40px;
  background: #F3F4F6;
  border: 1px solid transparent;
  border-radius: 12px;
  font-size: 14px;
  outline: none;
  transition: 0.2s;
}
.input-wrapper input:focus {
  background: #fff;
  border-color: #FFAB40;
}

.toggle-visibility {
  position: absolute;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
  opacity: 0.5;
}
.toggle-visibility:hover {
  opacity: 1;
}

.error-text {
  color: #E53935;
  font-size: 11px;
  margin-top: 4px;
  display: block;
}

.global-error-message {
  margin-top: 15px;
  padding: 10px 14px;
  background-color: #FEE2E2;
  color: #B91C1C;
  border-radius: 8px;
  font-size: 13px;
  line-height: 1.4;
  text-align: center;
  border: 1px solid #FECACA;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  margin-top: 30px;
}
.btn-cancel {
  padding: 10px 20px;
  background: transparent;
  border: none;
  color: #666;
  cursor: pointer;
  font-weight: 500;
}
.btn-cancel:hover {
  color: #333;
}
.btn-save {
  padding: 10px 24px;
  background: #FFAB40;
  color: white;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 600;
  transition: 0.2s;
}
.btn-save:hover {
  background: #FF9100;
}

.loyalty-card {
  display: flex;
  justify-content: space-between;
  align-items: stretch; 
  background-color: #F9F9F9; 
  padding: 24px 30px;
  border-radius: 20px;
  margin-bottom: 40px;
}

.loyalty-left {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 15px;
  width: 20%;
}

.loyalty-title {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
  color: #333;
}

.status-btn {
  background-color: #FF7A00;
  color: #fff;
  border: none;
  border-radius: 50px;
  padding: 10px 24px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  width: fit-content;
  transition: background 0.2s;
}

.status-btn:hover {
  background-color: #E06900;
}

.loyalty-bonuses {
  font-size: 16px;
  font-weight: 600;
  color: #444;
  margin-top: auto;
}

.loyalty-bonuses span {
  color: #FF7A00;
  font-weight: 700;
  margin-left: 5px;
}

.loyalty-right {
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  width: 100%;
}

.card-visual-placeholder {
  background-color: #FFFFFF;
  border-radius: 12px;
  height: 60px;
  width: 100%;
  margin-bottom: 12px;
  box-shadow: 0 2px 5px rgba(0,0,0,0.03);
}

.card-number-text {
  font-family: monospace;
  color: #888;
  font-size: 14px;
  text-align: center;
  letter-spacing: 1px;
}

</style>