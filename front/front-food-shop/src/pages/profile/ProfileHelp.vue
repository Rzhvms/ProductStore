<template>
  <ProfileLayout>
    <input type="text" placeholder="Введите поисковый запрос..." class="faq-search" />

    <div class="faq-list">
      <!-- 1. FAQ -->
      <div class="faq-item" :class="{ open: openId === 1 }">
        <div class="faq-header" @click="toggle(1)">
          <span>FAQ (часто задаваемые вопросы)</span>
          <div class="arrow"></div>
        </div>

        <div class="faq-content">
          <div class="faq-subitem">
            <h4>Как изменить имя пользователя?</h4>
            <p>
              Вы можете изменить имя в разделе «Личные данные».  
              Некоторые данные, такие как дата рождения и номер телефона, могут изменяться только через службу поддержки.
            </p>
          </div>

          <div class="faq-subitem">
            <h4>Как сменить пароль?</h4>
            <p>
              В разделе «Управление аккаунтом» нажмите «Сменить пароль» и следуйте инструкции.
            </p>
          </div>
        </div>
      </div>

      <!-- 2 -->
      <div class="faq-item" :class="{ open: openId === 2 }">
        <div class="faq-header" @click="toggle(2)">
          <span>Информация об оплате и доставке</span>
          <div class="arrow"></div>
        </div>
        <div class="faq-content">
          <p>
            Доставка осуществляется по всей России, почти в каждом городе курьерскими службами.
            Оплата возможна картой, СБП или наличными при получении.
          </p>
        </div>
      </div>

      <!-- 3 -->
      <div class="faq-item" :class="{ open: openId === 3 }">
        <div class="faq-header" @click="toggle(3)">
          <span>Условия программы лояльности</span>
          <div class="arrow"></div>
        </div>
        <div class="faq-content">
          <p>
            За каждую покупку вы получаете бонусы. Бонусы можно использовать для оплаты до 30% стоимости заказа.
          </p>
        </div>
      </div>

      <!-- 4 -->
      <div class="faq-item" :class="{ open: openId === 4 }">
        <div class="faq-header" @click="toggle(4)">
          <span>Правила возврата</span>
          <div class="arrow"></div>
        </div>
        <div class="faq-content">
          <p>
            Вы можете вернуть товар в течение 14 дней при наличии чека и товарного вида.
          </p>
        </div>
      </div>

      <!-- 5 -->
      <div class="faq-item" :class="{ open: openId === 5 }">
        <div class="faq-header" @click="toggle(5)">
          <span>Инструкция по использованию сайта и аккаунта</span>
          <div class="arrow"></div>
        </div>
        <div class="faq-content">
          <p>
            В личном кабинете вы можете управлять заказами, личными данными, бонусами и уведомлениями.
          </p>
        </div>
      </div>

      <!-- 6 -->
      <div class="faq-item" :class="{ open: openId === 6 }">
        <div class="faq-header" @click="toggle(6)">
          <span>Контакты техподдержки</span>
          <div class="arrow"></div>
        </div>
        <div class="faq-content">
          <p>
            Если вы не нашли ответа на свой вопрос, пишите нам на example@gmail.com или звоните +7 999 999 99-99.
          </p>
        </div>
      </div>

    </div>

    <button class="contact-us-btn">Связаться с нами</button>

  </ProfileLayout>
</template>

<script>
import ProfileLayout from "./ProfileLayout.vue";

export default {
  name: "ProfileInfo",
  components: { ProfileLayout },

  data() {
    return {
      openId: 1, // только первый открыт
    };
  },

  methods: {
    toggle(id) {
      this.openId = this.openId === id ? null : id;
    }
  }
};
</script>

<style src="./profile.css"></style>
