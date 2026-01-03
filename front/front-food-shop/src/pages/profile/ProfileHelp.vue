<template>
  <ProfileLayout>
    <input type="text" placeholder="Введите поисковый запрос..." class="faq-search" v-model="searchQuery" />

    <TransitionGroup name="list" tag="div" class="faq-list">
      <div v-if="filteredItems.length === 0" key="not-found" style="padding: 20px; color: #7a7a7a;">Ничего не найдено по запросу "{{ searchQuery }}"</div>
      <!-- 1. FAQ -->
      <div v-for="item in filteredItems" :key="item.id" class="faq-item" :class="{ open: openId === item.id }">
        <div class="faq-header" @click="toggle(item.id)">
          <span>{{ item.title }}</span>
          <div class="arrow"></div>
        </div>
        <div class="faq-content">
          <div v-html="item.body"></div>
        </div>
      </div>
    </TransitionGroup>

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
      openId: 1,
      searchQuery: '',
      faqItems: [
        {
          id: 1,
          title: "FAQ (часто задаваемые вопросы)",
          body: `
            <div class="faq-subitem">
              <h4>Как изменить имя пользователя?</h4>
              <p>Вы можете изменить имя в разделе «Личные данные». Некоторые данные, такие как дата рождения и номер телефона, могут изменяться только через службу поддержки.</p>
            </div>
            <div class="faq-subitem">
              <h4>Как сменить пароль?</h4>
              <p>В разделе «Управление аккаунтом» нажмите «Сменить пароль» и следуйте инструкции.</p>
            </div>
          `
        },
        {
          id: 2,
          title: "Информация об оплате и доставке",
          body: `<p>Доставка осуществляется по всей России, почти в каждом городе курьерскими службами. Оплата возможна картой, СБП или наличными при получении.</p>`
        },
        {
          id: 3,
          title: "Условия программы лояльности",
          body: `<p>За каждую покупку вы получаете бонусы. Бонусы можно использовать для оплаты до 30% стоимости заказа.</p>`
        },
        {
          id: 4,
          title: "Правила возврата",
          body: `<p>Вы можете вернуть товар в течение 14 дней при наличии чека и товарного вида.</p>`
        },
        {
          id: 5,
          title: "Инструкция по использованию сайта и аккаунта",
          body: `<p>В личном кабинете вы можете управлять заказами, личными данными, бонусами и уведомлениями.</p>`
        },
        {
          id: 6,
          title: "Контакты техподдержки",
          body: `<p>Если вы не нашли ответа на свой вопрос, пишите нам на example@gmail.com или звоните +7 999 999 99-99.</p>`
        }
      ]
    };
  },

  computed: {
    filteredItems() {
      if (!this.searchQuery) return this.faqItems;
      const query = this.searchQuery.toLowerCase();
      return this.faqItems.filter(item => {
        const inTitle = item.title.toLowerCase().includes(query);
        const cleanBody = item.body.replace(/<[^>]*>/gm, '').toLowerCase();
        const inBody = cleanBody.includes(query);
        return inTitle || inBody;
      });
    }
  },
    
  methods: {
    toggle(id) {
      this.openId = this.openId === id ? null : id;
    }
  },

  watch: {
    searchQuery(query) {
      if (query && this.filteredItems.length > 0) this.openId = 1;
    }
  }
};
</script>

<style src="./profile.css"></style>
