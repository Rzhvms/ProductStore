<template>
  <div class="page-wrapper">
    <div class="payment-card shadow-2xl" ref="cardRef">
      <div class="header">
        <div class="brand-logo">
          <span class="text-white font-bold text-xl">Оплата заказа</span>
        </div>
        <h1 class="title">Заказ №{{ orderId }}</h1>
        <div class="amount-badge">{{ amount }} ₽</div>
      </div>

      <div class="content">
        <div class="input-group">
          <label>Номер карты</label>
          <input 
            v-model="cardNumber" 
            v-maska="'#### #### #### ####'"
            placeholder="0000 0000 0000 0000"
            class="payment-input"
          />
        </div>
        
        <div class="row">
          <div class="input-group">
            <label>Срок</label>
            <input v-model="expiry" v-maska="'##/##'" placeholder="MM/YY" class="payment-input" />
          </div>
          <div class="input-group">
            <label>CVC</label>
            <input v-model="cvc" v-maska="'###'" type="password" placeholder="***" class="payment-input" />
          </div>
        </div>

        <div class="actions">
          <button @click="handlePay" :disabled="isLoading" class="button pay-button">
            <span v-if="!isLoading">Оплатить {{ amount }} ₽</span>
            <span v-else class="loader"></span>
          </button>
          <button @click="router.back()" class="button cancel-button">Отмена</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
// Импортируем методы из вашего api.js
// Предположим, там есть payOrder
// import { payOrder } from '@/services/api'; 

const route = useRoute();
const router = useRouter();

const orderId = ref(route.query.orderId || '—');
const amount = ref(route.query.amount || '0');
const isLoading = ref(false);

const cardNumber = ref('');
const expiry = ref('');
const cvc = ref('');

const handlePay = async () => {
  if (cardNumber.value.length < 16) return alert('Проверьте номер карты');
  
  isLoading.value = true;
  try {
    // Здесь вызываем ваш рест из swagger
    // const res = await payOrder(orderId.value, { ...данные карты... });
    
    // Имитация задержки сети
    await new Promise(r => setTimeout(r, 1500));
    
    router.push({ name: 'PaymentSuccess', query: { orderId: orderId.value, amount: amount.value } });
  } catch (e) {
    router.push({ name: 'PaymentFailed', query: { reason: 'Ошибка авторизации банка' } });
  } finally {
    isLoading.value = false;
  }
};
</script>

<style scoped>
/* Исправленные стили */
.page-wrapper {
  background: #f4f4f7;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  padding: 20px;
}

.payment-card {
  background: white;
  width: 100%;
  max-width: 440px;
  border-radius: 28px;
  overflow: hidden;
  border: 1px solid rgba(0,0,0,0.05);
}

.header {
  background: #1d1d1f;
  padding: 40px 32px;
  text-align: center;
  color: white;
}

.amount-badge {
  display: inline-block;
  background: #FF7A00;
  padding: 8px 20px;
  border-radius: 100px;
  font-weight: 700;
  font-size: 20px;
  margin-top: 15px;
}

.payment-input {
  width: 100%;
  background: #f5f5f7;
  border: 1px solid #d2d2d7;
  padding: 14px;
  border-radius: 12px;
  font-size: 16px;
  transition: all 0.2s;
}

.payment-input:focus {
  border-color: #FF7A00;
  background: white;
  outline: none;
  box-shadow: 0 0 0 4px rgba(255,122,0,0.1);
}

.loader {
  width: 20px;
  height: 20px;
  border: 3px solid rgba(255,255,255,0.3);
  border-top-color: white;
  border-radius: 50%;
  display: inline-block;
  animation: spin 0.8s linear infinite;
}

@keyframes spin { to { transform: rotate(360deg); } }
</style>