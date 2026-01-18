<!-- components/Footer.vue -->
<template>
  <footer class="lk-footer">
    <div class="footer-col">
      <h4>Наши контакты</h4>
      <p>+7 999 999 99-99</p>
      <p>горячая линия, ежедневно с 9:00 до 21:00</p>
      <p>example@gmail.com</p>
    </div>

    <div class="footer-col">
      <h4>Покупателям</h4>
      <a href="#">Справочная информация</a>
      <a href="#">Обратная связь</a>
      <a href="#">Оценка товаров</a>
      <a href="#">О нас</a>
    </div>

    <div class="footer-col">
      <h4>Партнерам и сотрудникам</h4>
      <a href="#">Вакансии</a>
      <a href="#">Поставщикам</a>
      <a href="#">Отдел маркетинга и рекламы</a>
      <a href="#">Предложения по ассортименту</a>
    </div>

    <div class="footer-col">
      <h4>Правовая информация</h4>
      <a href="#">Пользовательское соглашение</a>
      <a href="#">Оферта о продаже товаров</a>
      <a href="#">Политика обработки персональных данных</a>
    </div>

    <div class="footer-col">
      <h4>Наше приложение</h4>
      <p>Android и iOS</p>
      <div class="footer-app-box"></div>
    </div>
  </footer>
</template>

<script setup>
</script>

<style scoped>
  
/* === FOOTER === */
.lk-footer {
  width: 100%;
  background: #f2f2f2;
  padding: 16px 48px 24px 48px;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  box-sizing: border-box;
  gap: 30px;
  flex-shrink: 0;
}

.lk-footer .footer-col {
  display: flex;
  flex-direction: column;
}

.lk-footer h4 {
  color: #f57c00;
  font-size: 18px;
  margin-bottom: 12px;
  font-weight: 600;
}

.lk-footer p,
.lk-footer a {
  margin: 6px 0;
  opacity: 0.8;
  font-size: 14px;
  color: #333;
  text-decoration: none;
}

.lk-footer a:hover {
  opacity: 1;
}

.footer-app-box {
  width: 171px;
  height: 171px;
  background: #fff;
  border-radius: 14px;
  border: 1px solid #ddd;
}
</style>
