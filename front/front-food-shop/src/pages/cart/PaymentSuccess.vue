<template>
  <div class="page-wrapper">
    <div class="success-card" ref="cardRef">
      <div class="header">
        <div class="success-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"></polyline>
          </svg>
        </div>
        <h1 class="title">Оплата прошла успешно</h1>
        <div class="subtitle">Заказ подтвержден и передан в обработку</div>
      </div>

      <div class="content">
        <div class="details-card">
          <div class="amount-display">
            <div class="amount-label">Оплаченная сумма</div>
            <div>
              <span class="amount-value">{{ formattedAmount }}</span>
              <span class="amount-currency">₽</span>
            </div>
          </div>
          <div class="detail-row">
            <div class="detail-label">Номер заказа</div>
            <div class="detail-value">{{ displayOrderId }}</div>
          </div>
          <div class="detail-row">
            <div class="detail-label">Платеж</div>
            <div class="detail-value">{{ displayPaymentId }}</div>
          </div>
          <div class="detail-row">
            <div class="detail-label">Дата оплаты</div>
            <div class="detail-value">{{ displayDate }}</div>
          </div>
          <div class="detail-row">
            <div class="detail-label">Статус</div>
            <div class="detail-value" style="color:#34C759;">Успешно</div>
          </div>
        </div>

        <div class="actions">
          <button class="button primary-button" @click="goToOrders">Перейти к заказам</button>
          <button class="button secondary-button" @click="goHome">Вернуться на главную</button>
        </div>
      </div>

      <div class="footer">
        <div class="confirmation-badge">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="mr-1"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path><polyline points="22 4 12 14.01 9 11.01"></polyline></svg>
          Чек отправлен на вашу почту
        </div>
      </div>
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

const formattedAmount = computed(() => {
  const val = paymentData.value?.amount || 0
  return val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
})

const displayOrderId = computed(() => paymentData.value?.orderId ? paymentData.value.orderId.substring(0,8).toUpperCase() : '—')
const displayPaymentId = computed(() => paymentData.value?.id ? paymentData.value.id.substring(0,8).toUpperCase() : (paymentId || '—'))

const displayDate = computed(() => {
  const dateStr = paymentData.value?.paidAt || paymentData.value?.updatedAt || new Date()
  const d = new Date(dateStr)
  const months = ['янв','фев','мар','апр','мая','июн','июл','авг','сен','окт','ноя','дек']
  return `${d.getDate()} ${months[d.getMonth()]}, ${d.getHours().toString().padStart(2,'0')}:${d.getMinutes().toString().padStart(2,'0')}`
})

const loadPaymentData = async () => {
  if (!paymentId) return
  try {
    const res = await fetch(`/api/mock-payments/${paymentId}`)
    if (!res.ok) throw new Error(res.status)
    paymentData.value = await res.json()
  } catch(e) { 
    console.error('Ошибка загрузки данных платежа:', e)
    // Mock данные
    paymentData.value = {
      amount: 1499,
      orderId: 'ORD-TEST-123',
      id: paymentId,
      paidAt: new Date()
    }
  }
}

const createConfetti = () => {
  const colors = ['#34C759','#30D158','#32D74B','#FFD60A','#FF9F0A']
  const container = cardRef.value
  if (!container) return

  for(let i=0; i<50; i++){
    setTimeout(() => {
      const confetti = document.createElement('div')
      confetti.className = 'confetti'
      confetti.style.background = colors[Math.floor(Math.random()*colors.length)]
      confetti.style.left = Math.random() * 100 + '%'
      confetti.style.top = '-20px'
      confetti.style.transform = `rotate(${Math.random()*360}deg)`
      
      // Стили для конфетти добавляем динамически или можно в CSS global
      confetti.style.position = 'absolute'
      confetti.style.width = '10px'
      confetti.style.height = '10px'
      confetti.style.borderRadius = '50%'
      confetti.style.opacity = '0'

      container.appendChild(confetti)

      setTimeout(() => {
        confetti.style.transition = 'all 1s cubic-bezier(0.4,0,0.2,1)'
        confetti.style.opacity = '0.8'
        confetti.style.top = Math.random() * 100 + '%'
        confetti.style.transform += ` translateX(${Math.random()*100-50}px)`
      }, 10)

      setTimeout(() => { 
        confetti.style.opacity = '0'
        setTimeout(() => confetti.remove(), 1000)
      }, 2000)
    }, i * 50)
  }
}

const fadeOutAndRedirect = (path) => {
  if (cardRef.value) {
    cardRef.value.style.opacity = '0'
    cardRef.value.style.transform = 'translateY(20px)'
  }
  setTimeout(() => router.push(path), 300)
}

const goToOrders = () => fadeOutAndRedirect('/profile/orders') // Укажите правильный путь к заказам
const goHome = () => fadeOutAndRedirect('/')

onMounted(() => {
  setTimeout(() => {
    if (cardRef.value) {
      cardRef.value.style.opacity = '1'
      cardRef.value.style.transform = 'translateY(0)'
    }
  }, 100)
  
  loadPaymentData()
  createConfetti()
})
</script>

<style scoped>
.page-wrapper {
  font-family: -apple-system, BlinkMacSystemFont, "SF Pro Display", "Helvetica Neue", Arial, sans-serif;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  padding: 20px;
  color: #1d1d1f;
}

.success-card {
  width: 100%; max-width: 380px; 
  background: rgba(255,255,255,0.95);
  backdrop-filter: blur(20px); 
  border-radius: 24px;
  box-shadow: 0 10px 40px rgba(0,0,0,0.08), 0 0 0 1px rgba(255,255,255,0.3) inset;
  overflow: hidden; 
  position: relative; 
  text-align: center;
  opacity: 0;
  transform: translateY(20px);
  transition: opacity 0.5s ease, transform 0.5s ease;
}

.success-card::before {
  content:''; position:absolute; top:0; left:0; right:0; height:60px;
  background:linear-gradient(to bottom, rgba(255,255,255,0.8), rgba(255,255,255,0));
  z-index:1;
}

.header { padding:60px 32px 40px; position:relative; z-index:2; }

.success-icon {
  width:56px; height:56px; background:linear-gradient(135deg, #34C759 0%, #30D158 100%);
  border-radius:50%; display:flex; align-items:center; justify-content:center;
  margin:0 auto 20px; box-shadow:0 8px 24px rgba(52,199,89,0.25);
  animation: successPop 0.6s cubic-bezier(0.68,-0.55,0.265,1.55);
}

.title {
  font-size:32px; font-weight:700; letter-spacing:-0.5px; margin-bottom:12px;
  background:linear-gradient(135deg, #1d1d1f 0%, #34C759 100%);
  -webkit-background-clip:text; -webkit-text-fill-color:transparent;
}

.subtitle { color:#86868b; font-size:17px; font-weight:500; line-height:1.4; }
.content { padding:0 32px 40px; }

.details-card {
  background:#f8f9fa; border-radius:20px; padding:24px; margin-bottom:32px;
  position:relative; overflow:hidden;
}

.details-card::before {
  content:''; position:absolute; top:0; left:0; right:0; height:2px;
  background:linear-gradient(to right, transparent, #34C759, transparent);
}

.detail-row {
  display:flex; justify-content:space-between; padding:14px 0; border-bottom:1px solid #f2f2f7;
}
.detail-row:last-child { border-bottom:none; }
.detail-label { color:#86868b; font-size:15px; font-weight:500; text-align:left; }
.detail-value { color:#1d1d1f; font-size:15px; font-weight:600; text-align:right; }

.amount-display {
  background:linear-gradient(135deg, #34C75915 0%, #30D15810 100%);
  border-radius:18px; padding:20px; margin-bottom:24px; border:1px solid rgba(52,199,89,0.1);
}
.amount-label { color:#34C759; font-size:13px; font-weight:600; text-transform:uppercase; margin-bottom:6px; }
.amount-value { font-size:36px; font-weight:800; letter-spacing:-1px; color:#1d1d1f; line-height:1; }
.amount-currency { font-size:22px; font-weight:600; color:#86868b; margin-left:4px; }

.actions { display:flex; flex-direction:column; gap:12px; }

.button {
  padding:18px; border-radius:14px; font-size:17px; font-weight:600; border:none;
  cursor:pointer; transition:all 0.3s cubic-bezier(0.4,0,0.2,1); position:relative; overflow:hidden;
}

.primary-button {
  background:linear-gradient(135deg, #FF6B35 0%, #FF9F1C 100%);
  box-shadow: 0 6px 20px rgba(255, 107, 53, 0.25);
  color:#ffffff;
}
.primary-button:hover { transform: translateY(-2px); box-shadow: 0 10px 25px rgba(255, 107, 53, 0.35); }

.secondary-button { background:transparent; color:#86868b; border:2px solid #e5e5ea; }
.secondary-button:hover { background:#f8f9fa; border-color:#d2d2d7; }

.footer { padding:24px 32px; text-align:center; color:#86868b; font-size:12px; border-top:1px solid #f2f2f7; background:#f8f9fa; }
.confirmation-badge { display:inline-flex; align-items:center; gap:6px; font-weight:600; letter-spacing:0.3px; color:#34C759; }

@keyframes successPop {0%{transform:scale(0);opacity:0;}70%{transform:scale(1.1);opacity:1;}100%{transform:scale(1);}}
</style>