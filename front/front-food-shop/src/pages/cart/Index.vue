<template>
  <Header />
  
  <div class="min-h-screen bg-white p-6 font-sans text-gray-900">
    <div class="mx-auto">
      
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <h1 class="text-3xl font-bold">Корзина</h1>
        
        <div class="relative w-full md:w-auto z-20">
          <button 
            class="flex items-center justify-between w-full md:w-[280px] bg-[#F9F9F9] hover:bg-gray-100 rounded-xl px-4 py-3 transition text-sm font-medium outline-none focus:ring-1 focus:ring-orange-200" 
            :class="{ 'ring-1 ring-orange-200 bg-white': showSortDropdown }"
            @click.stop="showSortDropdown = !showSortDropdown"
          >
            <div class="flex items-center gap-2 text-gray-700">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M3 6h18M6 12h12m-9 6h6"></path>
              </svg>
              <span>{{ buttonSortLabel }}</span>
            </div>
            
            <svg 
              width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
              class="transition-transform duration-200 text-gray-400"
              :class="{ 'rotate-180': showSortDropdown }"
            >
              <path d="M6 9l6 6 6-6"></path>
            </svg>
          </button>

          <div v-if="showSortDropdown" class="absolute right-0 mt-2 w-full md:w-[280px] bg-white rounded-xl shadow-xl border border-gray-100 overflow-hidden py-2 animate-fade-in z-30">
            <div class="px-4 py-2 text-xs font-bold text-gray-400 uppercase tracking-wider">По названию</div>
            <div @click="setSortOption('name-asc')" class="flex items-center px-4 py-3 hover:bg-gray-50 cursor-pointer group">
              <div class="w-5 h-5 rounded-full border border-gray-300 mr-3 flex items-center justify-center group-hover:border-orange-400 transition">
                 <div v-if="sortOption === 'name-asc'" class="w-2.5 h-2.5 bg-[#FF7A00] rounded-full"></div>
              </div>
              <span class="text-sm text-gray-700">От А до Я</span>
            </div>
            <div @click="setSortOption('name-desc')" class="flex items-center px-4 py-3 hover:bg-gray-50 cursor-pointer group">
              <div class="w-5 h-5 rounded-full border border-gray-300 mr-3 flex items-center justify-center group-hover:border-orange-400 transition">
                <div v-if="sortOption === 'name-desc'" class="w-2.5 h-2.5 bg-[#FF7A00] rounded-full"></div>
              </div>
              <span class="text-sm text-gray-700">От Я до А</span>
            </div>
            <div class="h-px bg-gray-100 my-1 mx-4"></div>
            <div class="px-4 py-2 text-xs font-bold text-gray-400 uppercase tracking-wider">По цене</div>
            <div @click="setSortOption('price-desc')" class="flex items-center px-4 py-3 hover:bg-gray-50 cursor-pointer group">
              <div class="w-5 h-5 rounded-full border border-gray-300 mr-3 flex items-center justify-center group-hover:border-orange-400 transition">
                <div v-if="sortOption === 'price-desc'" class="w-2.5 h-2.5 bg-[#FF7A00] rounded-full"></div>
              </div>
              <span class="text-sm text-gray-700">Сначала дорогие</span>
            </div>
            <div @click="setSortOption('price-asc')" class="flex items-center px-4 py-3 hover:bg-gray-50 cursor-pointer group">
              <div class="w-5 h-5 rounded-full border border-gray-300 mr-3 flex items-center justify-center group-hover:border-orange-400 transition">
                <div v-if="sortOption === 'price-asc'" class="w-2.5 h-2.5 bg-[#FF7A00] rounded-full"></div>
              </div>
              <span class="text-sm text-gray-700">Сначала дешевые</span>
            </div>
          </div>
        </div>
      </div>

      <div v-if="cartItems.length === 0" class="text-center py-20 bg-[#F9F9F9] rounded-3xl">
        <h2 class="text-xl text-gray-400">В корзине пока пусто</h2>
      </div>

      <div v-else class="grid grid-cols-1 lg:grid-cols-[1fr_400px] gap-8 items-start">
        
        <div class="flex flex-col">
          <div class="flex justify-end mb-4">
             <button @click="handleClearCart" class="text-gray-400 text-xs hover:text-red-500 transition">
              Очистить всё
            </button>
          </div>

          <div 
            v-for="item in sortedCartItems" 
            :key="item.id" 
            class="group py-6 border-b border-gray-100 first:pt-0"
          >
            <div class="flex justify-between items-center">
              <div class="flex gap-8 w-full">
                <div
                  @click="goToProduct(item.productId)"
                  class="relative w-24 h-24 min-w-[96px] bg-[#F9F9F9] rounded-2xl cursor-pointer overflow-hidden flex items-center justify-center"
                >
                  <div v-if="item.discount" class="absolute top-2 left-2 bg-[#FF7A00] text-white text-[10px] font-bold px-1.5 py-0.5 rounded-md z-10">
                    -{{ item.discount }}%
                  </div>
                  
                  <img v-if="item.image" :src="item.image" :alt="item.name" class="w-full h-full object-cover">
                  <span v-else class="text-2xl">📦</span>
                </div>
                
                <div class="flex-1 flex flex-col justify-between py-1">
                  <div class="flex justify-between items-start">
                    <div>
                      <h3 
                        @click="goToProduct(item.productId)" 
                        class="font-semibold text-lg leading-tight cursor-pointer hover:text-[#FF7A00] transition mb-1 pr-4"
                      >
                        {{ item.name }}
                      </h3>
                      <div class="flex items-center gap-2 text-sm text-gray-400">
                        <span v-if="item.oldPrice" class="line-through">{{ item.oldPrice }} ₽</span>
                        <span>{{ item.price }} ₽</span>
                      </div>
                    </div>
                    
                    <button 
                      @click="removeItem(item.productId)" 
                      class="w-8 h-8 flex items-center justify-center bg-[#FFF0F0] text-red-500 rounded-xl hover:bg-red-100 transition flex-shrink-0"
                    >
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M3 6h18M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a2 2 0 012-2h4a2 2 0 012 2v2"></path>
                      </svg>
                    </button>
                  </div>

                  <div class="flex justify-between items-end mt-2">    
                    <div class="flex items-center bg-[#F9F9F9] rounded-lg h-10 px-1">
                      <button 
                        @click="item.quantity > 1 ? updateQuantity(item.productId, item.quantity - 1) : removeItem(item.productId)" 
                        class="w-8 h-full text-[#FF7A00] text-lg font-medium hover:bg-gray-200 rounded-md transition flex items-center justify-center"
                      >
                        &minus;
                      </button>
                      <span class="w-8 text-center font-medium text-sm">{{ item.quantity }}</span>
                      <button 
                        @click="updateQuantity(item.productId, item.quantity + 1)" 
                        class="w-8 h-full text-[#FF7A00] text-lg font-medium hover:bg-gray-200 rounded-md transition flex items-center justify-center"
                      >
                        +
                      </button>
                    </div>

                    <div class="text-right">
                      <span v-if="item.oldPrice" class="block text-gray-400 line-through text-xs mb-0.5">
                        {{ item.oldPrice * item.quantity }} ₽
                      </span>
                      <span class="text-xl font-bold" :class="{'text-red-500': item.discount}">
                        {{ item.price * item.quantity }} ₽
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="bg-[#F9F9F9] p-6 rounded-[32px] sticky top-4">
          
          <div class="mb-6 relative">
            <label class="block text-sm text-gray-500 mb-2 font-medium">
              Адрес <span class="text-red-500">*</span>
            </label>
            <input
              v-model="addressQuery"
              type="text"
              placeholder="Введите адрес"
              class="w-full bg-white rounded-xl p-3 text-sm outline-none focus:ring-1 focus:ring-orange-200 shadow-sm transition-all"
              :class="{'ring-2 ring-red-300 bg-red-50': errors.address}"
              @input="errors.address = false"
            />
            <span v-if="errors.address" class="text-xs text-red-500 mt-1 pl-1">Укажите адрес доставки</span>
            
            <ul
              v-if="addressSuggestions.length"
              class="absolute z-50 mt-2 w-full bg-white rounded-xl shadow-lg overflow-hidden border border-gray-100"
            >
              <li
                v-for="suggest in addressSuggestions"
                :key="suggest.id"
                class="px-4 py-3 text-sm cursor-pointer hover:bg-gray-50 transition"
                @click="selectAddress(suggest)"
              >
                <div class="font-medium">{{ suggest.title.text || suggest.title }}</div>
                <div class="text-xs text-gray-400">{{ suggest.subtitle?.text || suggest.subtitle }}</div>
              </li>
            </ul>
          </div>

          <div class="grid grid-cols-3 gap-3 mb-4">
            <div>
              <label class="block text-xs text-gray-500 mb-1.5 ml-1">Этаж</label>
              <input v-model="details.floor" type="text" class="w-full bg-white rounded-xl p-3 text-sm outline-none shadow-sm text-center">
            </div>
            <div>
              <label class="block text-xs text-gray-500 mb-1.5 ml-1">Подъезд</label>
              <input v-model="details.entrance" type="text" class="w-full bg-white rounded-xl p-3 text-sm outline-none shadow-sm text-center">
            </div>
            <div>
              <label class="block text-xs text-gray-500 mb-1.5 ml-1">Кв./Офис</label>
              <input v-model="details.apartment" type="text" class="w-full bg-white rounded-xl p-3 text-sm outline-none shadow-sm text-center">
            </div>
          </div>

          <div class="mb-6">
            <label class="block text-sm text-gray-500 mb-2 font-medium">Комментарий курьеру</label>
            <textarea 
              v-model="details.comment" 
              placeholder="Код домофона, ориентиры..."
              class="w-full bg-white rounded-xl p-3 text-sm outline-none shadow-sm resize-none h-12 py-3"
            ></textarea>
          </div>

          <div 
            @click="showTimeModal = true"
            class="bg-white rounded-2xl p-4 flex items-center justify-between shadow-sm cursor-pointer hover:shadow-md transition mb-6"
          >
            <div class="flex items-center gap-3">
              <span class="text-[#FF7A00]">
                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <circle cx="12" cy="12" r="10"></circle>
                  <polyline points="12 6 12 12 16 14"></polyline>
                </svg>
              </span>
              <div>
                <div class="text-sm font-medium text-gray-600">Время доставки</div>
                <div class="text-xs text-gray-400 font-medium">{{ selectedTime.label }}</div>
              </div>
            </div>
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="text-gray-300">
              <path d="M9 18l6-6-6-6"></path>
            </svg>
          </div>

          <div class="space-y-3 border-b border-gray-200 pb-6 mb-6">
            <div class="flex justify-between text-sm">
              <span class="text-gray-500">Сумма заказа</span>
              <span class="font-medium">{{ totalOrderSum }} ₽</span>
            </div>
<!--            <div class="flex justify-between text-sm">-->
<!--              <span class="text-gray-500">Доставка</span>-->
<!--              <span class="font-medium">{{ deliveryFee === 0 ? '0 ₽' : deliveryFee + ' ₽' }}</span>-->
<!--            </div>-->
<!--            <div class="flex justify-between text-sm">-->
<!--              <span class="text-gray-500">Сборка и упаковка</span>-->
<!--              <span class="font-medium">{{ assemblyFee }} ₽</span>-->
<!--            </div>-->
            <div v-if="totalDiscount > 0" class="flex justify-between text-sm">
              <span class="text-green-500">Ваша экономия</span>
              <span class="text-green-500">-{{ totalDiscount }} ₽</span>
            </div>
          </div>

          <div class="flex justify-between items-center mb-6">
            <span class="text-3xl font-bold text-gray-800">Итог</span>
            <span class="text-3xl font-bold text-gray-900">{{ grandTotal }} ₽</span>
          </div>

          <div 
            @click="showPaymentModal = true"
            class="bg-white rounded-2xl p-4 flex items-center justify-between shadow-sm cursor-pointer hover:shadow-md transition mb-6"
            :class="{'ring-2 ring-red-300': errors.payment}"
          >
            <div>
              <div class="text-sm font-medium text-gray-600">Способ оплаты</div>
              <div class="text-xs text-gray-400 font-medium">{{ selectedPayment.name }}</div>
            </div>
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="text-gray-300">
              <path d="M9 18l6-6-6-6"></path>
            </svg>
          </div>

          <button 
            @click="checkout" 
            class="w-full bg-[#FF7A00] text-white py-4 rounded-xl font-bold hover:bg-orange-600 transition shadow-lg shadow-orange-100 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Оформить заказ
          </button>
        </div>
      </div>
    </div>

    <div v-if="showTimeModal" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm animate-fade-in" @click.self="showTimeModal = false">
      <div class="bg-white w-full max-w-md rounded-2xl p-6 shadow-2xl">
        <h3 class="text-xl font-bold mb-4">Когда доставить?</h3>
        <div class="space-y-2 max-h-[60vh] overflow-y-auto pr-2">
          <div 
            v-for="time in availableTimes" 
            :key="time.id"
            @click="selectTime(time)"
            class="p-4 rounded-xl border cursor-pointer flex justify-between items-center transition"
            :class="selectedTime.id === time.id ? 'border-[#FF7A00] bg-orange-50' : 'border-gray-100 hover:border-orange-200'"
          >
            <div>
              <div class="font-medium text-gray-800">{{ time.label }}</div>
              <div v-if="time.subLabel" class="text-xs text-gray-400">{{ time.subLabel }}</div>
            </div>
            <div v-if="selectedTime.id === time.id" class="w-5 h-5 bg-[#FF7A00] rounded-full flex items-center justify-center text-white">
              <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M20 6L9 17l-5-5"></path></svg>
            </div>
          </div>
        </div>
        <button @click="showTimeModal = false" class="w-full mt-6 bg-gray-100 hover:bg-gray-200 text-gray-800 font-medium py-3 rounded-xl transition">
          Закрыть
        </button>
      </div>
    </div>

    <div v-if="showPaymentModal" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm animate-fade-in" @click.self="showPaymentModal = false">
      <div class="bg-white w-full max-w-md rounded-2xl p-6 shadow-2xl">
        <h3 class="text-xl font-bold mb-4">Способ оплаты</h3>
        <div class="space-y-2">
          <div 
            v-for="method in paymentMethods" 
            :key="method.id"
            @click="selectPayment(method)"
            class="p-4 rounded-xl border cursor-pointer flex justify-between items-center transition"
            :class="selectedPayment.id === method.id ? 'border-[#FF7A00] bg-orange-50' : 'border-gray-100 hover:border-orange-200'"
          >
            <div class="flex items-center gap-3">
              <span class="text-2xl">{{ method.icon }}</span>
              <span class="font-medium text-gray-800">{{ method.name }}</span>
            </div>
             <div v-if="selectedPayment.id === method.id" class="w-5 h-5 bg-[#FF7A00] rounded-full flex items-center justify-center text-white">
              <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3"><path d="M20 6L9 17l-5-5"></path></svg>
            </div>
          </div>
        </div>
        <button @click="showPaymentModal = false" class="w-full mt-6 bg-gray-100 hover:bg-gray-200 text-gray-800 font-medium py-3 rounded-xl transition">
          Закрыть
        </button>
      </div>
    </div>

  </div>
  <Footer />
</template>

<script setup>
import { ref, computed, onMounted, watch, reactive, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import Header from '../catalog/Header.vue'
import Footer from '../catalog/Footer.vue'
import { cartApi, getAddressSuggestions, createOrder } from '@/services/api'

const router = useRouter()
const cartItems = ref([])
// const deliveryFee = ref(99)
// const assemblyFee = ref(29)

// --- Address & Details ---
const address = ref('')
const addressQuery = ref('улица Крестинского, 11')
const addressSuggestions = ref([])
const details = reactive({
  floor: '',
  entrance: '',
  apartment: '',
  comment: ''
})

// --- Validation Errors ---
const errors = reactive({
  address: false,
  payment: false
})

// --- MODALS STATE ---
const showTimeModal = ref(false)
const showPaymentModal = ref(false)

// --- Delivery Time Logic ---
const availableTimes = [
  { id: 'fast', label: 'В ближайшее время', subLabel: '15-30 минут' },
  { id: 'today_12', label: 'Сегодня, 12:00 - 14:00' },
  { id: 'today_14', label: 'Сегодня, 14:00 - 16:00' },
  { id: 'today_18', label: 'Сегодня, 18:00 - 20:00' },
  { id: 'tomorrow_10', label: 'Завтра, 10:00 - 12:00' }
]
const selectedTime = ref(availableTimes[0])

const selectTime = (time) => {
  selectedTime.value = time
  showTimeModal.value = false
}

// --- Payment Logic ---
const paymentMethods = [
  { id: 'sberpay', name: 'SberPay', icon: '🟢' },
  { id: 'card', name: 'Банковской картой', icon: '💳' },
  { id: 'cash', name: 'Наличными курьеру', icon: '💵' },
]
const selectedPayment = ref(paymentMethods[0])

const selectPayment = (method) => {
  selectedPayment.value = method
  showPaymentModal.value = false
  errors.payment = false
}

// --- SORTING LOGIC ---
const showSortDropdown = ref(false)
const sortOption = ref('') 

const buttonSortLabel = computed(() => {
  switch (sortOption.value) {
    case 'name-asc': return 'От А до Я'
    case 'name-desc': return 'От Я до А'
    case 'price-desc': return 'Сначала дорогие'
    case 'price-asc': return 'Сначала дешевые'
    default: return 'Сортировка'
  }
})

const setSortOption = (option) => {
  sortOption.value = option
  showSortDropdown.value = false
}

const sortedCartItems = computed(() => {
  const items = [...cartItems.value]
  if (!sortOption.value) return items

  return items.sort((a, b) => {
    switch (sortOption.value) {
      case 'name-asc': return a.name.localeCompare(b.name)
      case 'name-desc': return b.name.localeCompare(a.name)
      case 'price-asc': return a.price - b.price
      case 'price-desc': return b.price - a.price
      default: return 0
    }
  })
})

let abortController = null

// --- COMPUTED ---
const totalOrderSum = computed(() => {
  return cartItems.value.reduce((acc, item) => acc + (item.price * item.quantity), 0)
})

const totalDiscount = computed(() => {
  return cartItems.value.reduce((acc, item) => {
    if (item.oldPrice) {
      return acc + ((item.oldPrice - item.price) * item.quantity)
    }
    return acc
  }, 0)
})

const grandTotal = computed(() => {
  return totalOrderSum.value
})

// --- METHODS ---
const loadData = async () => {
  try {
    const data = await cartApi.get()
    cartItems.value = data.items || data 
  } catch (error) {
    console.error("Ошибка при загрузке корзины:", error)
  }
}

const updateQuantity = async (productId, newQuantity) => {
  try {
    await cartApi.add(productId, newQuantity)
    await loadData()
  } catch (error) {
    console.error("Ошибка при изменении количества:", error)
  }
}

const removeItem = async (productId) => {
  if (confirm('Удалить товар из корзины?')) {
    try {
      await cartApi.remove(productId)
      await loadData()
    } catch (error) {
      console.error("Ошибка при удалении товара:", error)
    }
  }
}

const handleClearCart = async () => {
  try {
    await cartApi.clear()
    cartItems.value = []
  } catch (error) {
    console.error("Не удалось очистить корзину")
  }
}

const goToProduct = (id) => {
  router.push(`/product/${id}`)
}

// --- CHECKOUT with VALIDATION ---
const checkout = async () => {
  // Reset errors
  errors.address = false
  errors.payment = false

  let hasError = false

  // 1. Проверка адреса (не пустой и больше 5 символов)
  if (!addressQuery.value || addressQuery.value.trim().length < 5) {
    errors.address = true
    hasError = true
  }

  // 2. Проверка товаров
  if (cartItems.value.length === 0) {
    alert('Корзина пуста')
    return
  }

  if (hasError) {
    // Скролл к ошибке (простой вариант - вверх страницы)
    window.scrollTo({ top: 0, behavior: 'smooth' })
    return
  }

  // Success
  const OrderDetails={
    items: cartItems.value,
    address: addressQuery.value,
    details: details,   
    deliveryTime: selectedTime.value,
    paymentMethod: selectedPayment.value,
    totals: {
      items: totalOrderSum.value,
      // delivery: deliveryFee.value,
      grandTotal: grandTotal.value
    }
  }
  const order = await createOrder(OrderDetails)
  const orderId = order.orderId
  const paymentUrl = order.paymentUrl
  router.push(`/payment/${orderId}`)
}

const closeSuggestions = () => {
  addressSuggestions.value = []
}

const closeSortDropdown = () => {
  showSortDropdown.value = false
}

const selectAddress = (item) => {
  addressQuery.value = item.title.text || item.title
  address.value = item.title.text || item.title
  addressSuggestions.value = []
  errors.address = false
}

// --- LIFECYCLE & WATCHERS ---
onMounted(() => {
  loadData()
  document.addEventListener('click', closeSuggestions)
  document.addEventListener('click', closeSortDropdown)
})

onUnmounted(() => {
  document.removeEventListener('click', closeSuggestions)
  document.removeEventListener('click', closeSortDropdown)
})

watch(addressQuery, async (value) => {
  if (!value || value === address.value) return 
  
  if (value.length < 3) {
    addressSuggestions.value = []
    return
  }

  if (abortController) abortController.abort()
  abortController = new AbortController()
  try {
    const response = await getAddressSuggestions(value)
    addressSuggestions.value = response.suggestedAddressList || []
  } catch (error) {
    console.error('Ошибка автодополнения адреса', error)
  }
})
</script>

<style scoped>
/* Простая анимация появления */
.animate-fade-in {
  animation: fadeIn 0.2s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(5px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>