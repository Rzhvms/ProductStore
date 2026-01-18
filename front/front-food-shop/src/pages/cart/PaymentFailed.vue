<template>
  <div class="page-wrapper">
    <div class="cancel-card" ref="cardRef">
      <div class="header">
        <div class="cancel-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"></line>
            <line x1="6" y1="6" x2="18" y2="18"></line>
          </svg>
        </div>
        <h1 class="title">Оплата отменена</h1>
        <div class="subtitle">Платеж не был завершен</div>
        <div class="reason">{{ reasonText }}</div>
      </div>

      <div class="content">
        <div class="details-card">
          <div class="detail-row">
            <div class="detail-label">Номер заказа</div>
            <div class="detail-value">{{ displayOrderId }}</div>
          </div>
          <div class="detail-row">
            <div class="detail-label">Платеж</div>
            <div class="detail-value">{{ displayPaymentId }}</div>
          </div>
          <div class="detail-row">
            <div class="detail-label">Сумма</div>
            <div class="detail-value">{{ formattedAmount }}</div>
          </div>
          <div class="detail-row">
            <div class="detail-label">Статус</div>
            <div class="detail-value" style="color:#FF453A">Отменен</div>
          </div>
        </div>

        <div class="info-box">
          <div class="info-title">Что делать дальше?</div>
          <div class="info-text">
            Вы можете повторить попытку оплаты или вернуться в корзину.
            Заказ сохранен на 24 часа.
          </div>
        </div>

        <div class="actions">
          <button class="button home-button" @click="goToCart">Вернуться в корзину</button>
        </div>
      </div>

      <div class="footer">Нужна помощь? Обратитесь в поддержку</div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const cardRef = ref(null)

const paymentData = ref(null)
const paymentId = route.query.paymentId

const reasonMap = {
  failed: 'Ошибка оплаты',
  canceled: 'Отменено пользователем',
  timeout: 'Время ожидания истекло',
  insufficient_funds: 'Недостаточно средств',
  card_declined: 'Карта отклонена'
}

const reasonText = computed(() => {
  const reason = paymentData.value?.cancelReason || 'canceled'
  return reasonMap[reason] || 'Платеж отменен'
})

const displayOrderId = computed(() => paymentData.value?.orderId ? paymentData.value.orderId.substring(0,8).toUpperCase() : '—')
const displayPaymentId = computed(() => paymentId ? paymentId.substring(0,8).toUpperCase() : '—')
const formattedAmount = computed(() => (paymentData.value?.amount || 0).toLocaleString('ru-RU',{minimumFractionDigits:2}) + ' ₽')

const loadPayment = async () => {
  if (!paymentId) return
  try {
    const r = await fetch(`/api/mock-payments/${paymentId}`)
    if (!r.ok) throw r.status
    paymentData.value = await r.json()
  } catch (e) {
    // Mock
    paymentData.value = {
      orderId: 'ORD-ERR-999',
      amount: 1499,
      cancelReason: 'canceled'
    }
  }
}

const goToCart = () => {
  if (cardRef.value) {
    cardRef.value.style.opacity = '0'
    cardRef.value.style.transform = 'translateY(20px)'
  }
  setTimeout(() => router.push('/cart'), 300) // Убедитесь, что роут корзины правильный (например, /cart или /)
}

onMounted(() => {
  setTimeout(() => {
    if (cardRef.value) {
      cardRef.value.style.opacity = '1'
      cardRef.value.style.transform = 'translateY(0)'
    }
  }, 100)
  loadPayment()
})
</script>

<style scoped>
.page-wrapper {
  font-family:-apple-system,BlinkMacSystemFont,"SF Pro Display","Helvetica Neue",Arial,sans-serif;
  background:linear-gradient(135deg,#f8f9fa 0%,#e9ecef 100%);
  display:flex; justify-content:center; align-items:center;
  min-height:100vh; padding:20px; color:#1d1d1f;
}

.cancel-card {
  width:100%; max-width:380px;
  background:rgba(255,255,255,0.95);
  backdrop-filter:blur(20px);
  border-radius:24px;
  box-shadow:0 10px 40px rgba(0,0,0,.08),0 0 0 1px rgba(255,255,255,.3) inset;
  overflow:hidden; position:relative; text-align:center;
  opacity:0; transform:translateY(20px);
  transition:.5s ease;
}

.header { padding:60px 32px 40px; }

.cancel-icon {
  width:56px;height:56px;border-radius:50%;
  background:linear-gradient(135deg,#FF453A,#FF3B30);
  display:flex;align-items:center;justify-content:center;
  margin:0 auto 20px;
  box-shadow:0 8px 24px rgba(255,69,58,.25);
  animation:pop .6s cubic-bezier(.68,-.55,.265,1.55);
}

.title {
  font-size:32px;font-weight:700;margin-bottom:12px;
  background:linear-gradient(135deg,#1d1d1f,#FF453A);
  -webkit-background-clip:text;
  -webkit-text-fill-color:transparent;
}

.subtitle { color:#86868b;font-size:17px; }

.reason {
  display:inline-block;margin-top:12px;
  background:rgba(255,69,58,.1);
  color:#FF453A;
  padding:8px 16px;border-radius:12px;
  font-weight:600;font-size:14px;
}

.content { padding:0 32px 40px; }

.details-card {
  background:#f8f9fa;border-radius:20px;
  padding:24px;margin-bottom:28px;
}

.detail-row {
  display:flex;justify-content:space-between;
  padding:14px 0;border-bottom:1px solid #eee;
}
.detail-row:last-child{border:none}

.detail-label { color:#86868b;font-size:15px; }
.detail-value { font-size:15px;font-weight:600; }

.info-box {
  background:linear-gradient(135deg,#FF453A10,#FF3B3008);
  border:1px solid rgba(255,69,58,.1);
  border-radius:18px;
  padding:18px;text-align:left;margin-bottom:24px;
}
.info-title { color:#FF453A;font-weight:600;margin-bottom:6px; }

.actions { display:flex;flex-direction:column;gap:12px; }

.button {
  padding:18px;border-radius:14px;
  font-size:17px;font-weight:600;
  cursor:pointer;border:none;
  transition:.3s ease;
}

.home-button {
  background:linear-gradient(135deg, #FF6B35 0%, #FF9F1C 100%);
  box-shadow: 0 6px 20px rgba(255, 107, 53, 0.25);
  color:#ffffff;
}
.home-button:hover { transform: translateY(-2px);
  box-shadow: 0 10px 25px rgba(255, 107, 53, 0.35); }

.footer {
  padding:20px;font-size:12px;color:#86868b;
  border-top:1px solid #eee;
  background:#f8f9fa;
}

@keyframes pop {
  0%{transform:scale(0);opacity:0}
  70%{transform:scale(1.1)}
  100%{transform:scale(1)}
}
</style>