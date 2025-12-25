<template>
  <ProfileLayout>
    <!-- ЗАГОЛОВОК -->
    <div class="pd-header" style="margin-bottom: 20px;">
      <h2>Личные заказы</h2>
    </div>

    <!-- БЛОК ФИЛЬТРОВ (Стилизован под personal-data) -->
    <div class="personal-data filter-section">
      
      <!-- Поиск и Даты -->
      <div class="form-grid-3x2 filters-grid">
        
        <!-- Поиск (на всю ширину в мобилке, или широко в десктопе) -->
        <div class="form-item search-item">
          <label>Поиск заказа</label>
          <div class="input-with-icon">
            <input 
              type="text" 
              placeholder="Номер заказа или товар" 
              v-model="filters.search" 
            />
            <span class="icon">🔍</span>
          </div>
        </div>

        <!-- Даты (Используем стиль form-item) -->
        <div class="form-item">
          <label>От</label>
          <input type="text" placeholder="дд.мм.гггг" class="date-input" />
        </div>

        <div class="form-item">
          <label>До</label>
          <input type="text" placeholder="дд.мм.гггг" class="date-input" />
        </div>
      </div>

      <!-- Кнопка поиска по чеку (Стиль как pd-edit-btn или status-btn) -->
      <div class="actions-row">
        <button class="pd-edit-btn" style="width: auto; padding: 0 20px;">
          + Поиск по чеку
        </button>
      </div>

      <!-- ТАБЫ (Стилизованы как gender-switch из твоего кода) -->
      <div class="form-item" style="margin-top: 20px;">
        <label>Статус заказа</label>
        <div class="gender-switch tabs-switch">
          <!-- Скользящий фон (упрощенная логика для примера) -->
          <div 
            v-for="tab in tabs" 
            :key="tab"
            class="gender-option"
            :class="{ active: currentTab === tab }"
            @click="currentTab = tab"
          >
            {{ tab }}
          </div>
        </div>
      </div>
    </div>

    <!-- СПИСОК ЗАКАЗОВ -->
    <div class="orders-container">
      
      <div 
        v-for="order in orders" 
        :key="order.id" 
        class="loyalty-card order-card" 
        :class="{ 'is-open': order.isOpen }"
      >
        <!-- Шапка заказа -->
        <div class="order-header" @click="toggleOrder(order.id)">
          <div class="card-left">
            <div class="card-number">Заказ {{ order.id }}</div>
            <div class="card-name" style="font-size: 14px; margin-top: 4px;">от {{ order.date }}</div>
          </div>
          
          <div class="card-right order-status-block">
            <button class="status-btn" :class="getStatusClass(order.status)">
              {{ order.status }}
            </button>
            <span class="chevron" :class="{ rotated: order.isOpen }">▼</span>
          </div>
        </div>

        <!-- СВЕРНУТЫЙ ВИД (Превью) -->
        <div v-if="!order.isOpen" class="order-preview">
          <div class="thumbnails-row">
            <div v-for="n in 3" :key="n" class="thumb-square"></div>
            <span v-if="order.products.length > 3" class="more-count">Ещё +{{ order.products.length - 3 }}</span>
          </div>
          <div class="preview-total">
            <span class="price-label">Итого:</span>
            <span class="price-value">{{ order.totalPrice }} ₽</span>
          </div>
        </div>

        <!-- РАЗВЕРНУТЫЙ ВИД (Детали) -->
        <div v-else class="order-details">
          <hr class="divider" />
          
          <!-- Инфо о доставке (Сетка как в профиле) -->
          <div class="form-grid-3x2" style="margin-bottom: 20px;">
            <div class="form-item">
              <label>Телефон</label>
              <input type="text" :value="order.phone" disabled />
            </div>
            <div class="form-item search-item">
              <label>Способ получения</label>
              <input type="text" :value="order.delivery" disabled />
            </div>
          </div>

          <!-- Список товаров -->
          <div class="products-list">
            <div v-for="(product, idx) in order.products" :key="idx" class="product-row">
              <div class="product-img"></div>
              <div class="product-info">
                <div class="p-name">{{ product.name }}</div>
                <div class="p-code">Код: {{ product.code }}</div>
              </div>
              <div class="product-price">
                <div class="p-total">{{ product.price }} ₽</div>
                <div class="p-calc">{{ product.qty }} шт. х {{ product.price }}</div>
              </div>
            </div>
          </div>

          <div class="order-footer-actions">
            <div class="total-big">
              Итого: <span>{{ order.totalPrice }} ₽</span>
            </div>
            <button class="pd-save">Повторить заказ</button>
          </div>
        </div>

      </div>
    </div>

  </ProfileLayout>
</template>

<script>
import ProfileLayout from "./ProfileLayout.vue";

export default {
  name: "OrdersPage",
  components: { ProfileLayout },

  data() {
    return {
      currentTab: 'Все',
      tabs: ['Все', 'Открытые', 'Выкупленные', 'Отменённые'],
      filters: {
        search: '',
        dateFrom: '',
        dateTo: ''
      },
      orders: [
        {
          id: '4B-124892',
          date: '14.10.2023',
          status: 'В доставке',
          totalPrice: '179 990',
          isOpen: false,
          phone: '+7 999 000-00-00',
          delivery: 'СДЭК, ул. Пушкина',
          products: [
            { name: 'Товар 1', code: '111', price: '100 000', qty: 1 },
            { name: 'Товар 2', code: '222', price: '20 000', qty: 2 },
            { name: 'Товар 3', code: '333', price: '10 000', qty: 1 },
            { name: 'Товар 4', code: '444', price: '5 000', qty: 1 },
          ]
        },
        {
          id: '4B-888555',
          date: '10.09.2023',
          status: 'Завершен',
          totalPrice: '108 996',
          isOpen: true,
          phone: '+7 999 999-99-99',
          delivery: 'СДЭК по адресу ул. Пушкина д. Колотушкина',
          products: [
            { name: 'Блок питания Be Quiet Dark Power 12 Pro черный', code: '5437234', price: '25 999', qty: 1 },
            { name: 'Процессор AMD Ryzen 9 9950x3d OEM', code: '4387483', price: '69 999', qty: 1 },
            { name: 'Оперативная память Acer', code: '832473', price: '12 998', qty: 2 },
          ]
        }
      ]
    };
  },

  methods: {
    toggleOrder(id) {
      const order = this.orders.find(o => o.id === id);
      if (order) order.isOpen = !order.isOpen;
    },
    getStatusClass(status) {
      if (status === 'Завершен') return 'status-done';
      if (status === 'В доставке') return 'status-delivery';
      return '';
    }
  }
};
</script>

<style scoped>
/* 
  Я предполагаю, что основные стили (form-item, pd-header и т.д.) 
  тянутся из твоего profile.css.
  Здесь я добавляю стили только для специфичных элементов Заказов,
  но использую переменную цветов и подходы из твоего макета.
*/

/* Сетка фильтров */
.filters-grid {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr; /* Поиск шире, даты уже */
  gap: 20px;
  align-items: end;
}

.input-with-icon {
  position: relative;
}
.input-with-icon input {
  width: 100%;
  padding-right: 35px; /* Место под иконку */
}
.input-with-icon .icon {
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  opacity: 0.5;
}

/* Переопределение стилей карты лояльности под карточку заказа */
.order-card {
  background: #fff; /* Белый фон, как у блоков в профиле */
  border: 1px solid #e0e0e0;
  border-radius: 12px;
  padding: 0; /* Убираем паддинг контейнера, будем задавать внутри */
  margin-bottom: 20px;
  box-shadow: none; /* Убираем сильные тени, делаем чище */
  display: block; /* Сбрасываем флекс лояльности */
  color: #333;
}

.order-header {
  padding: 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  cursor: pointer;
}

.order-status-block {
  display: flex;
  align-items: center;
  gap: 15px;
}

/* Стили статусов */
.status-btn {
  /* Базовый стиль из твоего кода */
  cursor: default;
  background: #eee;
  color: #333;
  border: none;
}
.status-done {
  background: #e6fffa;
  color: #00a080;
}
.status-delivery {
  background: #ebf8ff;
  color: #3182ce;
}

.chevron {
  transition: transform 0.3s;
  font-size: 12px;
  opacity: 0.6;
}
.chevron.rotated {
  transform: rotate(180deg);
}

/* Превью (свернутый вид) */
.order-preview {
  padding: 0 20px 20px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.thumbnails-row {
  display: flex;
  gap: 10px;
}
.thumb-square {
  width: 50px;
  height: 50px;
  background-color: #f0f0f0;
  border-radius: 8px;
}
.more-count {
  align-self: center;
  font-size: 13px;
  color: #888;
}
.price-value {
  font-weight: bold;
  font-size: 18px;
}

/* Детали (развернутый вид) */
.order-details {
  padding: 20px;
  background-color: #fff; /* Фон контента */
}
.divider {
  border: 0;
  border-top: 1px solid #eee;
  margin: 0 0 20px 0;
}

.products-list {
  display: flex;
  flex-direction: column;
  gap: 15px;
  margin-bottom: 20px;
}
.product-row {
  display: flex;
  gap: 15px;
  align-items: flex-start;
  padding-bottom: 15px;
  border-bottom: 1px solid #f9f9f9;
}
.product-img {
  width: 60px;
  height: 60px;
  background: #eee;
  border-radius: 8px;
  flex-shrink: 0;
}
.product-info {
  flex-grow: 1;
}
.p-name {
  font-weight: 600;
  font-size: 14px;
  margin-bottom: 5px;
}
.p-code {
  font-size: 12px;
  color: #999;
}
.product-price {
  text-align: right;
  min-width: 100px;
}
.p-total {
  font-weight: 700;
  font-size: 14px;
}
.p-calc {
  font-size: 12px;
  color: #999;
}

/* Футер заказа */
.order-footer-actions {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  gap: 20px;
  margin-top: 20px;
}
.total-big {
  font-size: 16px;
  color: #555;
}
.total-big span {
  font-weight: bold;
  color: #000;
  font-size: 18px;
  margin-left: 5px;
}

/* Адаптация табов под стиль gender-switch */
.tabs-switch {
  display: flex;
  background: #f0f0f0;
  padding: 4px;
  border-radius: 8px; /* или как в твоем CSS */
  overflow: x-auto; /* Скролл на мобилке если много табов */
}
.tabs-switch .gender-option {
  flex: 1;
  text-align: center;
  padding: 8px 15px;
  white-space: nowrap;
}
.tabs-switch .gender-option.active {
  background: #fff;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  color: #ff7f00; /* Твой оранжевый */
}

/* Responsive */
@media (max-width: 768px) {
  .filters-grid {
    grid-template-columns: 1fr;
  }
  .order-preview {
    flex-direction: column;
    align-items: flex-start;
    gap: 15px;
  }
}
</style>