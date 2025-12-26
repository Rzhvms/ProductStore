<template>
  <AdminLayout>
    <template #default>

      <div class="modal-overlay" v-if="showAddModal || showEditModal">
        <div class="modal-content">
          <h2>{{ showAddModal ? 'Добавить товар' : 'Редактировать товар' }}</h2>

          <div class="modal-body">
            <label>Ключевое имя</label>
            <input type="text" v-model="modalProduct.name" placeholder="Введите название" />

            <label>Категория</label>
            <select v-model="modalProduct.category">
              <option disabled value="">Выберите категорию</option>
              <option v-for="cat in categories" :key="cat.name" :value="cat.name">{{ cat.name }}</option>
            </select>

            <label>Подкатегория</label>
            <select v-model="modalProduct.subcategory" :disabled="!modalProduct.category">
              <option disabled value="">Выберите подкатегорию</option>
              <option v-for="sub in getSubcategories(modalProduct.category)" :key="sub">{{ sub }}</option>
            </select>

            <label>Цена, ₽</label>
            <input type="text" v-model="modalProduct.price" @input="formatPrice" placeholder="499" />

            <label>Ссылка на картинку</label>
            <input type="text" v-model="modalProduct.image" placeholder="https://example.com/image.png" />

            <label>Состав</label>
            <textarea v-model="modalProduct.ingredients" placeholder="Состав через запятую"></textarea>

            <label>Пищевая ценность (на 100 г)</label>
            <div class="nutrition-grid">
              <div class="nutrient">
                <label>Калории</label>
                <input type="number" v-model="modalProduct.nutrition.calories" placeholder="330" />
              </div>
              <div class="nutrient">
                <label>Белки</label>
                <input type="number" v-model="modalProduct.nutrition.proteins" placeholder="2.5" />
              </div>
              <div class="nutrient">
                <label>Жиры</label>
                <input type="number" v-model="modalProduct.nutrition.fats" placeholder="0.6" />
              </div>
              <div class="nutrient">
                <label>Углеводы</label>
                <input type="number" v-model="modalProduct.nutrition.carbs" placeholder="83" />
              </div>
            </div>

            <label>Характеристики</label>
            <div class="tags-box">
              <div class="tag-item" v-for="(tag, index) in modalProduct.features" :key="index">
                <input type="text" v-model="modalProduct.features[index]" />
                <button class="remove-tag" @click="modalProduct.features.splice(index,1)">×</button>
              </div>
            </div>
            <button class="add-feature-btn" @click="modalProduct.features.push('')">+ Добавить характеристику</button>
          </div>

          <div class="modal-footer">
            <button class="cancel-btn" @click="closeModals">Отмена</button>
            <button v-if="showAddModal" class="save-btn" @click="confirmAdd">Сохранить</button>
            <button v-if="showEditModal" class="save-btn" @click="confirmEdit">Обновить</button>
          </div>
        </div>
      </div>

      <div class="main-content">
        <div class="controls-area">
          <div class="filters-box">
            <div class="filter-group search-group">
              <label>Поиск</label>
              <input type="text" placeholder="По ключевому имени" v-model="filters.search" />
            </div>
            <div class="filter-group price-group">
              <label>Стоимость, ₽</label>
              <div class="price-inputs">
                <input type="number" placeholder="От" v-model="filters.priceFrom" />
                <span class="dash">—</span>
                <input type="number" placeholder="До" v-model="filters.priceTo" />
              </div>
            </div>
          </div>
          <button class="add-product-btn" @click="openAddModal">Добавить товар</button>
        </div>

        <div class="products-grid">
          <div class="product-card" v-for="(p,i) in products" :key="i">
            <div class="image-placeholder" :style="{ backgroundImage: p.image ? 'url('+p.image+')' : '' }">
              <button class="edit-btn" @click="openEditModal(i)">✎</button>
            </div>
            <div class="card-info">
              <p class="product-name">{{ p.name }}</p>
              <p class="product-category">{{ p.category }} <span v-if="p.subcategory">/ {{ p.subcategory }}</span></p>
              <p class="product-price">{{ p.price }} ₽</p>
            </div>
          </div>
        </div>
      </div>

    </template>
  </AdminLayout>
</template>

<script setup>
import { reactive, ref } from 'vue'
import AdminLayout from './AdminLayout.vue'
import './admin.css'

const filters = reactive({ search:'', priceFrom:null, priceTo:null })

const categories = reactive([
  { name:'Фрукты', sub:['Цитрусовые','Ягоды','Тропические'] },
  { name:'Овощи', sub:['Корнеплоды','Листовые','Бобовые'] },
  { name:'Напитки', sub:['Соки','Чай','Кофе'] }
])

const getSubcategories = (catName) => {
  const cat = categories.find(c=>c.name===catName)
  return cat ? cat.sub : []
}

const products = ref([
  {
    name:'Манго сушёное',
    price:'450',
    image:'',
    category:'Фрукты',
    subcategory:'Тропические',
    ingredients:'Манго сушёное',
    nutrition:{ calories:330, proteins:2.5, fats:0.6, carbs:83 },
    features:['Натуральное','Без сахара']
  },
  {
    name:'Морковь свежая',
    price:'120',
    image:'',
    category:'Овощи',
    subcategory:'Корнеплоды',
    ingredients:'Морковь',
    nutrition:{ calories:41, proteins:0.9, fats:0.2, carbs:10 },
    features:['Свежая','Без химии']
  },
  {
    name:'Апельсиновый сок',
    price:'250',
    image:'',
    category:'Напитки',
    subcategory:'Соки',
    ingredients:'Апельсин',
    nutrition:{ calories:45, proteins:0.7, fats:0.1, carbs:10 },
    features:['100% сок','Без сахара']
  }
])

const showAddModal = ref(false)
const showEditModal = ref(false)
const editingIndex = ref(null)

const modalProduct = reactive({
  name:'', price:'', image:'', category:'', subcategory:'', ingredients:'',
  nutrition:{ calories:null, proteins:null, fats:null, carbs:null },
  features:[]
})

const openAddModal = () => {
  Object.assign(modalProduct,{ name:'', price:'', image:'', category:'', subcategory:'', ingredients:'', nutrition:{calories:null,proteins:null,fats:null,carbs:null}, features:[] })
  showAddModal.value = true
}

const openEditModal = (i) => {
  editingIndex.value = i
  Object.assign(modalProduct, JSON.parse(JSON.stringify(products.value[i])))
  showEditModal.value = true
}

const closeModals = () => { showAddModal.value=false; showEditModal.value=false }

const formatPrice = () => {
  modalProduct.price = modalProduct.price.replace(/\D/g,'').replace(/\B(?=(\d{3})+(?!\d))/g,' ')
}

const confirmAdd = () => {
  if(!modalProduct.name.trim()) return alert('Название обязательно')
  products.value.push(JSON.parse(JSON.stringify(modalProduct)))
  closeModals()
}

const confirmEdit = () => {
  products.value[editingIndex.value] = JSON.parse(JSON.stringify(modalProduct))
  closeModals()
}
</script>
