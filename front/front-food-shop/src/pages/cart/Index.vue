<template>
  <Header />
  <div class="min-h-screen bg-gray-50 p-8 font-sans text-gray-900">
    <div class="mx-auto">
      <div class="flex justify-between items-center mb-8">
        <h1 class="text-3xl font-bold">Корзина</h1>
        <div class="relative">
          <button
            class="
              relative z-[10000]
              flex items-center justify-center gap-2
              min-w-[210px]
              px-[18px] py-3
              rounded-2xl
              text-sm
              whitespace-nowrap
              transition-all duration-200
              cursor-pointer
              bg-[#f9f9f9] text-[#4B5563]
            "
            :class="{
              'bg-[#FF7A00] text-white': showSortDropdown || sortOption
            }"
            @click.stop="showSortDropdown = !showSortDropdown"
          >
            <!-- <img
              v-if="!(showSortDropdown || sortOption)"
              src='../../assets/drop down button.svg'
            />
            <img
              v-else
              src='../../assets/drop down button(1).svg'
            /> -->
            <span>{{ buttonSortLabel }}</span>
          </button>

          <div
            v-if="showSortDropdown"
            class="
              absolute mt-2
              w-full
              rounded-2xl
              bg-white
              shadow-lg
              p-4
              z-[9999]
            "
          >
            <div class="text-xs font-semibold text-gray-500 mb-2">
              По алфавиту
            </div>

            <div
              class="flex items-center gap-3 py-2 cursor-pointer"
              @click="setSortOption('name-asc')"
            >
              <div
                class="w-4 h-4 rounded-full border border-gray-300 flex items-center justify-center"
                :class="{ 'border-[#FF7A00]': sortOption === 'name-asc' }"
              >
                <div
                  v-if="sortOption === 'name-asc'"
                  class="w-2 h-2 rounded-full bg-[#FF7A00]"
                ></div>
              </div>
              <span class="text-sm">От А до Я</span>
            </div>

            <div
              class="flex items-center gap-3 py-2 cursor-pointer"
              @click="setSortOption('name-desc')"
            >
              <div
                class="w-4 h-4 rounded-full border border-gray-300 flex items-center justify-center"
                :class="{ 'border-[#FF7A00]': sortOption === 'name-desc' }"
              >
                <div
                  v-if="sortOption === 'name-desc'"
                  class="w-2 h-2 rounded-full bg-[#FF7A00]"
                ></div>
              </div>
              <span class="text-sm">От Я до А</span>
            </div>

            <div class="my-3 h-px bg-gray-200"></div>

            <div class="text-xs font-semibold text-gray-500 mb-2">
              По количеству<br />подкатегорий
            </div>

            <div
              class="flex items-center gap-3 py-2 cursor-pointer"
              @click="setSortOption('count-desc')"
            >
              <div
                class="w-4 h-4 rounded-full border border-gray-300 flex items-center justify-center"
                :class="{ 'border-[#FF7A00]': sortOption === 'count-desc' }"
              >
                <div
                  v-if="sortOption === 'count-desc'"
                  class="w-2 h-2 rounded-full bg-[#FF7A00]"
                ></div>
              </div>
              <span class="text-sm">Сначала больше</span>
            </div>

            <div
              class="flex items-center gap-3 py-2 cursor-pointer"
              @click="setSortOption('count-asc')"
            >
              <div
                class="w-4 h-4 rounded-full border border-gray-300 flex items-center justify-center"
                :class="{ 'border-[#FF7A00]': sortOption === 'count-asc' }"
              >
                <div
                  v-if="sortOption === 'count-asc'"
                  class="w-2 h-2 rounded-full bg-[#FF7A00]"
                ></div>
              </div>
              <span class="text-sm">Сначала меньше</span>
            </div>
          </div>
        </div>
      </div>

      <div v-if="cartItems.length === 0" class="text-center py-20 bg-white rounded-3xl shadow-sm">
        <h2 class="text-xl text-gray-400">В корзине пока пусто</h2>
      </div>

      <div v-else class="flex gap-8">
        <div class="flex-1 space-y-4 flex flex-col">
          <button v-if="cartItems.length > 0" @click="handleClearCart" class="text-gray-400 hover:text-red-500 transition text-sm self-end">
            Очистить корзину
          </button>
          <div v-for="item in cartItems" :key="item.id" class="bg-white p-4 rounded-2xl flex items-center shadow-sm relative">
            <div v-if="item.discount" class="absolute top-4 left-4 bg-orange-500 text-white text-xs px-2 py-1 rounded-lg z-10">
              {{ item.discount }}%
            </div>
            
            <div 
              @click="goToProduct(item.productId)"
              class="w-24 h-24 bg-gray-100 rounded-xl mr-4 cursor-pointer overflow-hidden flex items-center justify-center"
            >
              <img v-if="item.image" :src="item.image" :alt="item.name" class="w-full h-full object-cover">
              <span v-else class="text-2xl">📦</span>
            </div>

            <div class="flex-1">
              <h3 @click="goToProduct(item.productId)" class="font-semibold text-lg cursor-pointer hover:text-orange-500 transition">
                {{ item.name }}
              </h3>
              <p class="text-gray-400 text-sm">{{ item.price }} ₽</p>
              <button @click="removeItem(item.productId)" class="mt-2 p-2 bg-red-50 rounded-lg text-red-500 hover:bg-red-100 transition">
                🗑️
              </button>
            </div>

            <div class="flex flex-col items-end gap-4">
              <div class="flex items-center gap-4">
                <button class="text-gray-400 hover:text-orange-500 text-xl">♡</button>
                <div class="flex items-center bg-gray-50 rounded-xl px-2 py-1 border border-gray-100">
                  <button @click="updateQuantity(item.productId, item.quantity + 1)" class="px-2 text-orange-500 font-bold text-xl">+</button>
                  <span class="px-4 font-medium">{{ item.quantity }}</span>
                  <button 
                    @click="item.quantity > 1 ? updateQuantity(item.productId, item.quantity - 1) : removeItem(item.productId)" 
                    class="px-2 text-orange-500 font-bold text-xl"
                  >-</button>
                </div>
              </div>
              <div class="text-right">
                <span v-if="item.oldPrice" class="text-gray-400 line-through text-sm mr-2">{{ item.oldPrice * item.quantity }} ₽</span>
                <span :class="{'text-red-500': item.discount, 'font-bold text-xl': true}">
                  {{ item.price * item.quantity }} ₽
                </span>
              </div>
            </div>
          </div>
        </div>

        <div class="w-[400px] space-y-4">
          <div class="bg-white p-6 rounded-3xl shadow-sm space-y-6">
            <section class="relative">
  <h4 class="text-gray-500 text-sm mb-2">Адрес</h4>

  <input
    v-model="addressQuery"
    type="text"
    placeholder="Введите адрес"
    class="
      w-full p-3
      bg-gray-50
      rounded-xl
      border-none
      focus:ring-1 focus:ring-orange-200
      outline-none
    "
  />

  <!-- Dropdown подсказок -->
  <ul
    v-if="addressSuggestions.length"
    class="
      absolute z-50 mt-2
      w-full
      bg-white
      rounded-xl
      shadow-lg
      overflow-hidden
    "
  >
    <li
      v-for="item in addressSuggestions"
      :key="item.id"
      class="
        px-4 py-3
        text-sm
        cursor-pointer
        hover:bg-gray-100
        transition
      "
      @click="selectAddress(item)"
    >
      <div class="font-medium">
        {{ item.title.text || item.title }}
      </div>
      <div class="text-xs text-gray-400">
        {{ item.subtitle?.text || item.subtitle }}
      </div>
    </li>
  </ul>
</section>


            <div class="space-y-2 border-t pt-4">
              <div class="flex justify-between text-sm">
                <span class="text-gray-500">Сумма заказа</span>
                <span>{{ totalOrderSum }} ₽</span>
              </div>
              <div class="flex justify-between text-sm">
                <span class="text-gray-500">Доставка</span>
                <span>{{ deliveryFee === 0 ? 'Бесплатно' : deliveryFee + ' ₽' }}</span>
              </div>
              <div v-if="totalDiscount > 0" class="flex justify-between text-sm text-green-500">
                <span>Ваша экономия</span>
                <span>-{{ totalDiscount }} ₽</span>
              </div>
              <div class="flex justify-between items-end pt-4">
                <span class="text-3xl font-bold">Итог</span>
                <span class="text-3xl font-bold text-orange-500">{{ totalOrderSum + deliveryFee }} ₽</span>
              </div>
            </div>

            <button @click="checkout" class="w-full bg-orange-500 text-white py-4 rounded-2xl font-bold hover:bg-orange-600 transition shadow-lg shadow-orange-200">
              Оформить заказ
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <Footer />
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import Header from '../catalog/Header.vue'
import Footer from '../catalog/Footer.vue'
import { cartApi, getAddressSuggestions } from '@/services/api'

const router = useRouter()
const cartItems = ref([])
const deliveryFee = ref(0) // Можно сделать зависимым от суммы заказа

const address = ref('')
const addressQuery = ref(address.value)
const addressSuggestions = ref([])
let abortController = null

const showSortDropdown = ref(false)
const sortOption = ref(null)

const buttonSortLabel = computed(() => {
  if (!sortOption.value) return 'Сортировка';
  if (sortOption.value.startsWith('name-')) return 'По алфавиту';
  if (sortOption.value.startsWith('count-')) return 'По количеству';
  if (sortOption.value.startsWith('price-')) return 'По цене';
  if (sortOption.value.startsWith('rating-')) return 'По рейтингу';
  return 'Сортировка';
});

function setSortOption(option) {
  sortOption.value = option;
  showSortDropdown.value = false;
}
// --- COMPUTED PROPERTIES (РАСЧЕТЫ) ---
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

// --- METHODS (МЕТОДЫ API) ---
const loadData = async () => {
  try {
    const data = await cartApi.get()
    // Важно: если бэк присылает просто массив, то cartItems.value = data
    cartItems.value = data.items || data 
  } catch (error) {
    console.error("Ошибка при загрузке корзины:", error)
  }
}

const updateQuantity = async (productId, newQuantity) => {
  try {
    // Обычно API добавления (post) при повторном вызове обновляет количество
    await cartApi.add(productId, newQuantity)
    await loadData() // Перезагружаем данные, чтобы увидеть актуальные цены
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

const checkout = () => {
  // Логику обсудим позже, пока просто редирект на оплату (заглушка)
  router.push('/payment')
}

const closeSuggestions = () => {
  addressSuggestions.value = []
}

onMounted(() => {
  loadData()
  document.addEventListener('click', closeSuggestions)
})

watch(addressQuery, async (value) => {
  if (!value) return
  
  if (value.length < 3) {
    addressSuggestions.value = []
    return
  }

  if (abortController) abortController.abort()
  abortController = new AbortController()
  try {
    const response = await getAddressSuggestions(value)
    addressSuggestions.value = response || []
  } catch (error) {
    console.error('Ошибка автодополнения адреса', error)
  }
})

const selectAddress = (item) => {
  addressQuery.value = item.title.text || item.title
  address.value = item.title.text || item.title
  addressSuggestions.value = []
}
</script>