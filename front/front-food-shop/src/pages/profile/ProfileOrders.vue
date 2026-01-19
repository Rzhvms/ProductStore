<script>
import ProfileLayout from "./ProfileLayout.vue";
// Импортируем методы API
import { getUserOrders, getUser } from "@/services/api.js"; 

export default {
  name: "OrdersPage",
  components: { ProfileLayout },

  data() {
    return {
      loading: false,
      currentTab: 'Все',
      tabs: ['Все', 'Открытые', 'Выкупленные', 'Отменённые'],
      filters: {
        search: '',
        dateFrom: '',
        dateTo: ''
      },
      orders: [] // Изначально пустой массив
    };
  },

  async created() {
    await this.fetchOrders();
  },

  methods: {
    async fetchOrders() {
      this.loading = true;
      try {
        // Вызываем без аргументов, так как фильтрация на бэкенде
        const rawOrders = await getUserOrders(); 
        
        this.orders = rawOrders.map(order => ({
          id: order.id,
          date: new Date(order.orderDate).toLocaleDateString('ru-RU'),
          status: this.mapStatus(order.state),
          totalPrice: order.totalSum,
          isOpen: false,
          // В ответе нет товаров и телефона, ставим заглушки
          products: [], 
          phone: '-'
        }));
      } catch (error) {
        console.error(error.message);
      } finally {
        this.loading = false;
      }
    },

    mapStatus(state) {
      // Маппинг числового state в текстовый статус
      const states = {
        0: 'В обработке',
        1: 'В доставке',
        2: 'Завершен',
        3: 'Отменен'
      };
      return states[state] || 'Новый';
    },

    getStatusClass(status) {
      if (status === 'Завершен') return 'status-done';
      if (status === 'В доставке') return 'status-delivery';
      if (status === 'Отменен') return 'status-cancelled';
      return '';
    },

    toggleOrder(id) {
      const order = this.orders.find(o => o.id === id);
      if (order) order.isOpen = !order.isOpen;
    }
  }
};
</script>

<template>
  <ProfileLayout>
    <div class="pd-header" style="margin-bottom: 20px;">
      <h2>Личные заказы</h2>
    </div>

    <div v-if="loading" class="loading-state">
      Загрузка заказов...
    </div>

    <div v-else>
      <div class="personal-data filter-section">
        </div>

      <div class="orders-container">
        <div v-if="orders.length === 0" class="no-orders">
          У вас пока нет заказов
        </div>
        
        <div 
          v-for="order in orders" 
          :key="order.fullId" 
          class="loyalty-card order-card" 
          :class="{ 'is-open': order.isOpen }"
        >
          <div class="order-header" @click="toggleOrder(order.id)">
            <div class="card-left">
              <div class="card-number">Заказ №{{ order.id }}</div>
              <div class="card-name" style="font-size: 14px; margin-top: 4px;">от {{ order.date }}</div>
            </div>
            
            <div class="card-right order-status-block">
              <button class="status-btn" :class="getStatusClass(order.status)">
                {{ order.status }}
              </button>
              <span class="chevron" :class="{ rotated: order.isOpen }">▼</span>
            </div>
          </div>

          <div v-if="!order.isOpen" class="order-preview">
            <div class="preview-total">
              <span class="price-label">Итого:</span>
              <span class="price-value">{{ order.totalPrice }} ₽</span>
            </div>
          </div>

          <div v-else class="order-details">
            <hr class="divider" />
            <div class="form-grid-3x2" style="margin-bottom: 20px;">
              <div class="form-item">
                <label>Телефон</label>
                <input type="text" :value="order.phone" disabled />
              </div>
              <div class="form-item">
                <label>ID Доставки</label>
                <input type="text" :value="order.delivery" disabled />
              </div>
            </div>

            <div v-if="order.products.length" class="products-list">
               </div>
            <div v-else class="no-products-info">
              Детализация товаров временно недоступна
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
    </div>
  </ProfileLayout>
</template>

<style scoped>
/* Добавьте к вашим стилям */
.loading-state, .no-orders {
  text-align: center;
  padding: 40px;
  color: #888;
}
.status-cancelled {
  background: #fff5f5;
  color: #e53e3e;
}
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
  padding: 4px 12px;
  border-radius: 16px;
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
