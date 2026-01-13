<template>
  <AdminLayout>
    <div v-if="currentView === 'user'" class="breadcrumbs">
      <span class="crumb-link" @click="goHome">Пользователи</span>
      <span class="crumb-arrow"> > </span>
      <span class="crumb-link active">{{ selectedUser.id }}</span>
    </div> 
    <div v-if="currentView === 'users'"> 
      <div class="view-header catalog-header">
            <div class="header-left-group">
              <img src="../../assets/user-orange.svg" class="folder-icon" />
              <h1 class="catalog-title">Пользователи</h1>
            </div>
            <div class="header-right-group">
              <div class="control-box search-box-styled">
                <img src="../../assets/search-normal.svg" />
                <input type="text" placeholder="Поиск..." v-model="searchQuery">
                <img src="../../assets/close-circle.svg" class="clear-circle" @click="searchQuery = ''" />
              </div>
              <button class="tool-btn" @click="toggleFilters" :class="{ 'active': isFiltered }">
                <img src="../../assets/filter.svg" alt="filter" />
                <span>Фильтры</span>
              </button>
              <div class="control-box dropdown-wrapper">
                <button class="sort-btn-styled" :class="{ 'is-active': showSortDropdown }" @click.stop="showSortDropdown = !showSortDropdown">
                  <img src="../../assets/drop down button.svg" />
                  <span>{{ buttonSortLabel }}</span>
                </button>
                
                <div v-if="showSortDropdown" class="custom-dropdown-menu sort-menu-design">
                  <div class="sort-group-label">По алфавиту</div>
                  <div class="sort-item" @click="setSortOption('name-asc')">
                      <div class="radio-indicator" :class="{ selected: sortOption === 'name-asc' }"></div>
                      <span class="sort-text">От А до Я</span>
                  </div>
                  <div class="sort-item" @click="setSortOption('name-desc')">
                      <div class="radio-indicator" :class="{ selected: sortOption === 'name-desc' }"></div>
                      <span class="sort-text">От Я до А</span>
                  </div>
                  
                  <div class="dd-divider"></div>
                  <div class="sort-group-label">По заказам</div>
                  <div class="sort-item" @click="setSortOption('orders-desc')">
                      <div class="radio-indicator" :class="{ selected: sortOption === 'orders-desc' }"></div>
                      <span class="sort-text">Сначала много</span>
                  </div>
                  <div class="sort-item" @click="setSortOption('orders-asc')">
                      <div class="radio-indicator" :class="{ selected: sortOption === 'orders-asc' }"></div>
                      <span class="sort-text">Сначала мало</span>
                  </div>
                  
                  <div class="dd-divider"></div>
                  <div class="sort-group-label">По возрасту</div>
                  <div class="sort-item" @click="setSortOption('date-desc')">
                      <div class="radio-indicator" :class="{ selected: sortOption === 'date-desc' }"></div>
                      <span class="sort-text">Сначала младшие</span>
                  </div>
                  <div class="sort-item" @click="setSortOption('date-asc')">
                      <div class="radio-indicator" :class="{ selected: sortOption === 'date-asc' }"></div>
                      <span class="sort-text">Сначала старшие</span>
                  </div>
                </div>
              </div>
            </div>
          </div>

      <div class="table-container">
        <table class="users-table">
          <thead>
            <tr>
              <th>Фамилия Имя Отчество</th>
              <th>День рождения</th>
              <th>Почта</th>
              <th>Номер телефона</th>
              <th>Город</th>
              <th>Количество заказов</th>
            </tr>
          </thead>
          <tbody v-if="filteredUsers.length > 0">
            <tr v-for="(user, index) in filteredUsers" :key="index" @click="openUser(user)">
              <td>{{ user.lastName }} {{ user.name }}</td>
              <td>{{ user.birthDate }}</td>
              <td class="text-muted">{{ user.email }}</td>
              <td>{{ user.phone }}</td>
              <td>{{ user.city }}</td>
              <td>{{ user.orders }}</td>
            </tr>
          </tbody>
          <tr v-else>
            <td colspan="6" class="no-data">Пользователи не найдены</td>
          </tr>
        </table>
      </div>
    </div>
    <div v-else>
  <div class="delete-account-wrapper">
    <button class="icon-btn danger" @click="promptDelete">
      <img src="../../assets/trash.svg" class="trash-icon" />
      <span>Удалить аккаунт</span>
    </button>
  </div>

  <div class="user-info-card">
    <div class="card-header-main">
      <div class="card-title-group">
        <img src="../../assets/user-orange.svg" class="title-icon" />
        <h2 class="card-main-title">Информация о пользователе</h2>
      </div>
      <button v-if="!isEditing" class="edit-btn" @click="startEdit">
        <img src="../../assets/edit.svg" alt="edit" />
      </button>
      <div v-else class="edit-controls">
        <button class="save-btn" @click="saveUser">Сохранить</button>
        <button class="cancel-btn" @click="cancelEdit">Отмена</button>
      </div>
    </div>

    <div class="user-data-grid">
      <div class="data-field">
        <label>Фамилия Имя Отчество</label>
        <input v-if="isEditing" v-model="editForm.fullname" class="field-value editing"/>
        <div v-else class="field-value">{{ selectedUser.fullname }}</div>
      </div>
      <div class="data-field">
        <label>День рождения</label>
        <input v-if="isEditing" v-model="editForm.birthday" class="field-value editing"/>
        <div v-else class="field-value">{{ selectedUser.birthday }}</div>
      </div>
      <div class="data-field">
        <label>Почта</label>
        <input v-if="isEditing" v-model="editForm.email" class="field-value editing"/>
        <div v-else class="field-value">{{ selectedUser.email }}</div>
      </div>
      <div class="data-field">
        <label>Номер</label>
        <input v-if="isEditing" v-model="editForm.phone" class="field-value editing"/>
        <div v-else class="field-value">{{ selectedUser.phone }}</div>
      </div>
      <div class="data-field">
        <label>Город</label>
        <div class="field-simple">{{ selectedUser.city }}</div>
      </div>
      <div class="data-field">
        <label>Количество заказов</label>
        <div class="field-simple">{{ selectedUser.orders }}</div>
      </div>
    </div>
  </div>

  <div class="user-info-card orders-card">
    <div class="card-header-row">
      <div class="card-title-group">
        <img src="../../assets/folder-open.svg" class="title-icon" />
        <h2 class="card-main-title">Список заказов</h2>
      </div>
      <div class="card-actions">
        <div class="search-box-styled card">
          <img src="../../assets/search-normal.svg" />
          <input type="text" placeholder="Поиск..." v-model="searchOrderQuery">
        </div>
        <div class="control-box dropdown-wrapper">
          <button class="sort-btn-styled card" :class="{ 'is-active': showSortDropdown }" @click.stop="showSortDropdown = !showSortDropdown">
            <img src="../../assets/drop down button.svg" />
            <span>По алфавиту</span>
          </button>
          <div v-if="showSortDropdown" class="custom-dropdown-menu sort-menu-design">
            <div class="sort-group-label">По дате создания</div>
            <div class="sort-item" @click="setSortOption('date-asc')">
              <div class="radio-indicator" :class="{ selected: sortOption === 'date-asc' }"></div>
              <span class="sort-text">Сначала старшие</span>
            </div>
            <div class="sort-item" @click="setSortOption('date-desc')">
              <div class="radio-indicator" :class="{ selected: sortOption === 'date-desc' }"></div>
              <span class="sort-text">Сначала новее</span>
            </div>
            <div class="dd-divider"></div>
            <div class="sort-group-label">По цене</div>
            <div class="sort-item" @click="setSortOption('price-asc')">
              <div class="radio-indicator" :class="{ selected: sortOption === 'price-asc' }"></div>
              <span class="sort-text">Сначала дешевле</span>
            </div>
            <div class="sort-item" @click="setSortOption('price-desc')">
              <div class="radio-indicator" :class="{ selected: sortOption === 'price-desc' }"></div>
              <span class="sort-text">Сначала дороже</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <table class="orders-detail-table">
      <thead>
        <tr>
          <th>Id заказа</th>
          <th>Дата создания</th>
          <th>Дата доставки</th>
          <th>Стоимость</th>
          <th>Адрес</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="n in 5" :key="n">
          <td class="id-cell">d60e22d8-ab9e-40a7-bf54-be12feb7fc5a</td>
          <td>19.09.2003</td>
          <td>19.09.2003</td>
          <td class="price-cell">4000₽</td>
          <td class="address-cell">ул. Пушкина, д. Колотушкина</td>
          <td class="arrow-cell">
            <img src="../../assets/arrow-down.svg" alt=">" style="transform: rotate(270deg);" />
          </td>
        </tr>
      </tbody>
    </table>
    
    <div class="show-more-footer" v-if="visibleOrders.length < filteredUserOrders.length">
      <span class="show-more-link" @click="showMoreOrders">Показать ещё</span>
    </div>
  </div>
</div>
    <div class="filters-overlay" v-if="showFilters" @click.self="closeFilters">
      <div class="filters-sidebar">
        <div class="sidebar-header">
          <h2>Фильтры</h2>
          <button class="close-btn" @click="closeFilters">×</button>
        </div>

        <div class="sidebar-content">
          <div class="filter-block">
            <label class="f-label">Город</label>
            <div class="cat-tree">
              <div v-for="city in cities" :key="city" class="cat-child">
                <label class="checkbox-row">
                  <input type="checkbox" :value="city" v-model="selectedCities">
                  <span class="check-box"></span>
                  <span>{{ city }}</span>
                </label>
              </div>
            </div>
          </div>

          <div class="filter-block">
            <label class="f-label">Количество заказов</label>
            <div class="range-inputs">
              <div class="input-wrap">
                <span>От:</span>
                <input type="number" v-model.number="orderRange.from" placeholder="0">
              </div>
              <div class="input-wrap">
                <span>До:</span>
                <input type="number" v-model.number="orderRange.to" placeholder="999">
              </div>
            </div>
          </div>
        </div>

        <div class="sidebar-footer">
          <button class="reset-btn" @click="resetFilters">Сбросить</button>
          <button class="apply-btn" @click="closeFilters">Применить</button>
        </div>
      </div>
    </div>
  </AdminLayout>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import AdminLayout from './AdminLayout.vue';
import { adminUserApi } from '@/services/api'; 

// --- СОСТОЯНИЕ ---
const searchQuery = ref('');
const searchOrderQuery = ref('');
const showSortDropdown = ref(false);
const sortOption = ref('name-asc');
const showFilters = ref(false);
const currentView = ref('users');
const selectedUser = ref(null);
const itemsPerPage = 5;
const visibleCount = ref(itemsPerPage);
const isLoading = ref(false);

const users = ref([]); // Основной список пользователей с бэкенда
const userOrders = ref([]); // Список заказов выбранного пользователя

// Хелпер для форматирования даты из ISO в DD.MM.YYYY
const formatDate = (isoDate) => {
  if (!isoDate) return 'Не указано';
  const date = new Date(isoDate);
  return date.toLocaleDateString('ru-RU');
};

// --- ЗАГРУЗКА ДАННЫХ ---
const loadData = async () => {
  isLoading.value = true;
  try {
    const profilesRaw = await adminUserApi.getProfiles(1, 100); 
    const uniqueIds = [...new Set(profilesRaw.map(u => u.id))];

    const usersData = await Promise.all(uniqueIds.map(async (userId) => {
      try {
        const [profile, address, orders] = await Promise.allSettled([
          adminUserApi.getProfile(userId),
          adminUserApi.getAddress(userId),
          adminUserApi.getOrders(userId)
        ]);

        if (profile.status === 'rejected') return null;

        const profileData = profile.value;
        const city = address.status === 'fulfilled' && address.value?.city ? address.value.city : 'Не указан';
        const ordersCount = orders.status === 'fulfilled' ? orders.value.length : 0;

        return {
          id: userId,
          name: profileData.name,
          lastName: profileData.lastName,
          fullname: `${profileData.lastName} ${profileData.name}`,
          birthDate: formatDate(profileData.birthDate),
          rawBirthDate: profileData.birthDate, // Для сортировки и API
          email: profileData.email,
          phone: profileData.phone,
          city: city,
          orders: ordersCount,
          about: profileData.about,
          gender: profileData.gender
        };
      } catch (err) {
        console.error(`Ошибка загрузки данных для ${userId}`, err);
        return null;
      }
    }));

    users.value = usersData.filter(u => u !== null);
  } catch (error) {
    console.error("Не удалось загрузить пользователей", error);
    alert("Ошибка загрузки списка пользователей");
  } finally {
    isLoading.value = false;
  }
};

onMounted(async () => {
  await loadData();
});

// --- ПРОСМОТР ПОЛЬЗОВАТЕЛЯ ---
const openUser = async (user) => {
  selectedUser.value = { ...user }; // Копия для отображения
  currentView.value = 'user';
  userOrders.value = []; // Очистка старых заказов
  visibleCount.value = itemsPerPage;

  try {
    const orders = await adminUserApi.getOrders(user.id);
    // Маппинг заказов в формат таблицы
    userOrders.value = orders.map(order => ({
      id: order.id,
      created_at: formatDate(order.orderDate),
      raw_created_at: order.orderDate,
      delivery_at: formatDate(order.orderDate), // В API пока нет даты доставки, берем дату заказа
      price: order.totalSum,
      address: 'Загружается...' // Адрес заказа требует отдельного запроса или расширения API
    }));
  } catch (error) {
    console.error("Не удалось загрузить заказы пользователя", error);
  }
};

const goHome = () => {
  currentView.value = 'users';
  selectedUser.value = null;
};

// --- РЕДАКТИРОВАНИЕ ПОЛЬЗОВАТЕЛЯ ---
const isEditing = ref(false);
const editForm = ref({});

const startEdit = () => {
  // Подготовка данных для формы. fullname разбиваем обратно, дату преобразуем если нужно
  editForm.value = { 
    ...selectedUser.value,
    // Если в инпуте нужна дата в формате YYYY-MM-DD для type="date"
    birthday: selectedUser.value.rawBirthDate ? selectedUser.value.rawBirthDate.split('T')[0] : ''
  }; 
  isEditing.value = true;
};

const cancelEdit = () => {
  isEditing.value = false;
};

const saveUser = async () => {
  try {
    // Разбиваем ФИО, если оно было изменено одной строкой (простая логика)
    const nameParts = editForm.value.fullname.trim().split(' ');
    const lastName = nameParts[0] || editForm.value.lastName;
    const name = nameParts.slice(1).join(' ') || editForm.value.name;

    await adminUserApi.updateProfile(
      selectedUser.value.id,
      name,
      lastName,
      editForm.value.phone,
      editForm.value.gender,
      new Date(editForm.value.birthday).toISOString(), // Отправляем ISO дату
      editForm.value.about,
      editForm.value.email
    );

    // Обновляем локальные данные
    selectedUser.value = {
      ...selectedUser.value,
      name,
      lastName,
      fullname: `${lastName} ${name}`,
      email: editForm.value.email,
      phone: editForm.value.phone,
      birthDate: formatDate(editForm.value.birthday),
      rawBirthDate: editForm.value.birthday
    };

    // Обновляем список пользователей без перезагрузки API
    const index = users.value.findIndex(u => u.id === selectedUser.value.id);
    if (index !== -1) {
      users.value[index] = { ...selectedUser.value };
    }

    isEditing.value = false;
    alert("Профиль обновлен");
  } catch (error) {
    console.error(error);
    alert("Не удалось сохранить изменения");
  }
};

const promptDelete = () => {
  // В Swagger нет эндпоинта удаления пользователя админом (только deleteProduct или deleteCategory).
  // Поэтому пока оставляем заглушку или удаляем только локально.
  if (confirm(`Вы уверены, что хотите удалить аккаунт ${selectedUser.value.fullname}? (Функция в разработке)`)) {
    // Логика удаления через API (если появится эндпоинт)
    // await adminUserApi.deleteUser(selectedUser.value.id); 
    alert("API для удаления пользователя отсутствует");
  }
};

// --- ФИЛЬТРАЦИЯ И СОРТИРОВКА (Client Side) ---

const buttonSortLabel = computed(() => {
  const labels = {
    'name-asc': 'ФИО: А-Я',
    'name-desc': 'ФИО: Я-А',
    'orders-desc': 'Заказы: много',
    'orders-asc': 'Заказы: мало',
    'date-desc': 'Сначала младшие',
    'date-asc': 'Сначала старшие'
  };
  return labels[sortOption.value] || 'По алфавиту';
});

const setSortOption = (option) => {
  sortOption.value = option;
  showSortDropdown.value = false;
};

const toggleFilters = () => { showFilters.value = !showFilters.value; };
const closeFilters = () => { showFilters.value = false; };

const resetFilters = () => {
  selectedCities.value = [];
  orderRange.value = { from: null, to: null };
};

// Фильтры
const cities = computed(() => [...new Set(users.value.map(u => u.city))].filter(c => c !== 'Не указан'));
const selectedCities = ref([]);
const orderRange = ref({ from: null, to: null });

// Хелпер для парсинга даты из строки "DD.MM.YYYY" (для сортировки списка)
function parseDate(dateStr) {
  if (!dateStr || dateStr === 'Не указано') return 0;
  const [d, m, y] = dateStr.split('.').map(Number);
  return new Date(y, m - 1, d).getTime();
}

const filteredUsers = computed(() => {
  let result = [...users.value];

  // 1. Поиск
  if (searchQuery.value) {
    const q = searchQuery.value.toLowerCase();
    result = result.filter(u => u.fullname.toLowerCase().includes(q) || u.email.toLowerCase().includes(q));
  }

  // 2. Фильтр по городам
  if (selectedCities.value.length > 0) {
    result = result.filter(u => selectedCities.value.includes(u.city));
  }

  // 3. Фильтр по количеству заказов
  if (orderRange.value.from !== null && orderRange.value.from !== "") {
    result = result.filter(u => u.orders >= orderRange.value.from);
  }
  if (orderRange.value.to !== null && orderRange.value.to !== "") {
    result = result.filter(u => u.orders <= orderRange.value.to);
  }

  // 4. Сортировка
  result.sort((a, b) => {
    switch (sortOption.value) {
      case 'name-asc': return a.fullname.localeCompare(b.fullname);
      case 'name-desc': return b.fullname.localeCompare(a.fullname);
      case 'orders-desc': return b.orders - a.orders;
      case 'orders-asc': return a.orders - b.orders;
      // Для дат используем rawBirthDate (ISO string), так точнее
      case 'date-desc': return new Date(b.rawBirthDate || 0) - new Date(a.rawBirthDate || 0); 
      case 'date-asc': return new Date(a.rawBirthDate || 0) - new Date(b.rawBirthDate || 0);
      default: return 0;
    }
  });

  return result;
});

// Логика списка заказов пользователя
const filteredUserOrders = computed(() => {
  let result = [...userOrders.value];
  if (searchOrderQuery.value) {
    const q = searchOrderQuery.value.toLowerCase();
    result = result.filter(o => 
      o.id.toLowerCase().includes(q) || 
      (o.address && o.address.toLowerCase().includes(q))
    );
  }
  
  // Сортировка заказов
  result.sort((a, b) => {
    switch (sortOption.value) { // Используем ту же переменную сортировки, или создать отдельную
      case 'price-asc': return a.price - b.price;
      case 'price-desc': return b.price - a.price;
      case 'date-asc': return new Date(a.raw_created_at) - new Date(b.raw_created_at);
      case 'date-desc': return new Date(b.raw_created_at) - new Date(a.raw_created_at);
      default: return 0;
    }
  });
  return result;
});

const visibleOrders = computed(() => {
  return filteredUserOrders.value.slice(0, visibleCount.value);
});

const showMoreOrders = () => {
  visibleCount.value += itemsPerPage;
};

</script>

<style scoped>
.container {
  font-family: 'Inter', sans-serif; /* Похожий на макет шрифт */
  padding: 40px;
  background-color: #fff;
  color: #333;
}

/* Header Styles */
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

.title {
  font-size: 28px;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 12px;
}

.controls {
  display: flex;
  gap: 12px;
}

.search-input {
  background: #f5f5f5;
  border: none;
  padding: 10px 20px 10px 40px;
  border-radius: 8px;
  width: 300px;
  outline: none;
}

.btn {
  background: #f5f5f5;
  border: none;
  padding: 10px 16px;
  border-radius: 8px;
  cursor: pointer;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: background 0.2s;
}

.btn:hover {
  background: #eeeeee;
}

/* Table Styles */
.table-container {
  overflow-x: auto;
}

.users-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
}

.users-table th {
  padding: 16px 8px;
  color: #888;
  font-weight: 500;
  font-size: 14px;
  border-bottom: 1px solid #f0f0f0;
}

.users-table td {
  padding: 16px 8px;
  font-size: 14px;
  border-bottom: 1px solid #f0f0f0;
}

.text-muted {
  color: #888;
}

/* Удаляем границу у последней строки */
.users-table tr:last-child td {
  border-bottom: none;
}

.tool-btn {
  padding: 12px 24px;
  border-radius: 8px;
  border: none;
  background: #F9F9F9;
  color: #555;
  display: flex; align-items: center; gap: 8px;
  cursor: pointer;
  font-size: 14px;
  transition: 0.2s;
}
.tool-btn:hover { background: #eee; }
.tool-btn.active {
  background: #FF8C00;
  color: #fff;
  img {
    filter: invert(100%) sepia(0%) saturate(8%) hue-rotate(199deg) brightness(1003%) contrast(104%);
  }
}

.catalog-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
.header-left-group { display: flex; align-items: center; gap: 12px; }
.catalog-title { font-size: 28px; font-weight: 700; color: #111827; margin: 0; line-height: 1.2; }
.header-right-group { display: flex; align-items: center; gap: 12px; }

.control-box { position: relative; }
.search-box-styled { display: flex; align-items: center; background-color: #f9f9f9; border-radius: 16px; padding: 12px 24px; min-width: 200px; }
.search-box-styled input { border: none; background: transparent; outline: none; font-size: 14px; color: #374151; margin-left: 8px; width: 100%; }
.search-box-styled.card { background-color: #ffffff; }
.search-box-styled.card:hover { background-color: #F3F4F6; }

.sort-btn-styled { position: relative; z-index: 10000; justify-content: center; min-width: 210px; display: flex; align-items: center; gap: 8px; background-color: #f9f9f9; border: none; border-radius: 16px; padding: 12px 18px; font-size: 14px; color: #4B5563; cursor: pointer; white-space: nowrap; transition: all 0.2s; }
.sort-btn-styled.is-active { background-color: #FF7A00; color: white; }
.sort-btn-styled.is-active svg { stroke: white; }
.sort-btn-styled.card { background-color: #ffffff; }
.sort-btn-styled.card:hover { background-color: #F3F4F6; }
.sort-btn-styled.card.is-active { background-color: #FF7A00; color: white; }

.custom-dropdown-menu { min-width: 210px; position: absolute; top: calc(100% + 8px); border: 1px solid #E5E7EB; border-radius: 16px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); padding: 6px 0; z-index: 9999; background-color: #f6f6f6; }
.custom-dropdown-menu.right-align { right: 0; left: auto; }
.custom-dropdown-menu.sort-menu-design { padding: 16px 0; background: #FFF; top: calc(100% - 10px); }

.dd-item { padding: 10px 16px; font-size: 16px; color: #374151; cursor: pointer; }
.dd-item:hover { background-color: #FFF7ED; color: #FF7A00; }
.sort-item { display: flex; align-items: center; gap: 12px; padding: 8px 20px; cursor: pointer; }
.sort-item:hover { background-color: #F3F4F6; }
.sort-text { color: #555555; }
.sort-group-label { padding: 0 20px; margin-bottom: 12px; color: #555; line-height: 24px; }
.sort-group-label:not(:first-child) { margin-top: 12px; }
.dd-divider { height: 1px; background-color: #dddddd; margin: 0 auto; width: 83%;}
.radio-indicator { width: 20px; height: 20px; border-radius: 50%; background-color: #E5E7EB; position: relative; flex-shrink: 0; }
.radio-indicator.selected { background-color: #FF7A00; }
.radio-indicator.selected::after { content: ''; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); width: 8px; height: 8px; background-color: white; border-radius: 50%; }

.filters-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.4);
  z-index: 90000;
  display: flex;
  justify-content: flex-end;
}

.filters-sidebar {
  width: 400px;
  max-width: 100%;
  background: #fff;
  height: 100%;
  display: flex;
  flex-direction: column;
  animation: slideIn 0.3s ease;
}

@keyframes slideIn {
  from { transform: translateX(100%); }
  to { transform: translateX(0); }
}

.sidebar-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 30px;
  border-bottom: 1px solid #eee;
}

.sidebar-header h2 {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
}

.close-btn {
  background: none;
  border: none;
  font-size: 28px;
  cursor: pointer;
  color: #999;
  line-height: 1;
  padding: 0;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 6px;
  transition: all 0.2s;
}

.close-btn:hover {
  background: #f5f5f5;
  color: #333;
}

.sidebar-content {
  flex: 1;
  overflow-y: auto;
  padding: 20px 30px;
}

.filter-block {
  margin-bottom: 28px;
}

.f-label {
  display: block;
  font-weight: 600;
  margin-bottom: 14px;
  font-size: 15px;
  color: #333;
}

.range-inputs {
  display: flex;
  gap: 15px;
}

.input-wrap {
  flex: 1;
  display: flex;
  align-items: center;
  background: #F5F5F5;
  border-radius: 10px;
  padding: 10px 14px;
  transition: all 0.2s;
}

.input-wrap:focus-within {
  background: #fff;
  box-shadow: 0 0 0 2px #FF7A00;
}

.input-wrap span {
  color: #999;
  font-size: 14px;
  margin-right: 8px;
}

.input-wrap input {
  border: none;
  background: transparent;
  width: 100%;
  outline: none;
  font-weight: 500;
  font-size: 14px;
}

.cat-search {
  width: 100%;
  padding: 12px 16px;
  border: none;
  background: #F5F5F5;
  border-radius: 10px;
  font-size: 14px;
  margin-bottom: 16px;
  outline: none;
  transition: all 0.2s;
}

.cat-search:focus {
  background: #fff;
  box-shadow: 0 0 0 2px #FF7A00;
}

.cat-search::placeholder {
  color: #aaa;
}

.cat-tree {
  max-height: 320px;
  overflow-y: auto;
}

.cat-tree::-webkit-scrollbar {
  width: 6px;
}

.cat-tree::-webkit-scrollbar-track {
  background: #f5f5f5;
  border-radius: 3px;
}

.cat-tree::-webkit-scrollbar-thumb {
  background: #ddd;
  border-radius: 3px;
}

.cat-tree::-webkit-scrollbar-thumb:hover {
  background: #ccc;
}

.cat-group {
  margin-bottom: 6px;
}

.cat-parent {
  display: flex;
  align-items: center;
  padding: 12px 14px;
  background: #F8F8F8;
  border-radius: 10px;
  cursor: pointer;
  gap: 10px;
  transition: background 0.2s;
}

.cat-parent:hover {
  background: #f0f0f0;
}

.cat-name {
  flex: 1;
  font-weight: 500;
  font-size: 14px;
  color: #333;
}

.chevron {
  font-size: 18px;
  color: #999;
  transition: transform 0.25s ease;
  font-weight: bold;
}

.chevron.expanded {
  transform: rotate(90deg);
}

.cat-children {
  padding-left: 44px;
  padding-top: 8px;
  overflow: hidden;
}

.cat-child {
  padding: 6px 0;
}

.cat-child .checkbox-row span:last-child {
  font-size: 14px;
  color: #555;
}

/* Анимация раскрытия категорий */
.slide-enter-active,
.slide-leave-active {
  transition: all 0.25s ease;
}

.slide-enter-from,
.slide-leave-to {
  opacity: 0;
  max-height: 0;
  padding-top: 0;
}

.slide-enter-to,
.slide-leave-from {
  opacity: 1;
  max-height: 500px;
}

/* Пустой результат поиска */
.no-results {
  padding: 24px;
  text-align: center;
  color: #999;
  font-size: 14px;
}

/* Кнопка "Посмотреть все" */
.show-all-link {
  margin-top: 14px;
  color: #FF7A00;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
  display: inline-block;
}

.show-all-link:hover {
  text-decoration: underline;
}

/* Футер сайдбара */
.sidebar-footer {
  display: flex;
  gap: 14px;
  padding: 20px 30px;
  border-top: 1px solid #eee;
  background: #fff;
}

.reset-btn {
  flex: 1;
  height: 48px;
  border-radius: 10px;
  border: none;
  background: #F5F5F5;
  font-size: 15px;
  font-weight: 600;
  color: #555;
  cursor: pointer;
  transition: all 0.2s;
}

.reset-btn:hover {
  background: #eee;
}

.apply-btn {
  flex: 1;
  height: 48px;
  border-radius: 10px;
  border: none;
  background: #FF7A00;
  color: white;
  font-size: 15px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.apply-btn:hover {
  background: #e66e00;
}

.apply-btn:active {
  transform: scale(0.98);
}

.breadcrumbs { font-size: 14px; color: #9E9E9E; margin-bottom: 25px; }
.crumb-link { cursor: pointer; transition: color 0.2s; }
.crumb-link:hover { color: #FFA84C; }
.crumb-link.active { color: #FF7A00; font-weight: 500; cursor: default; }
.separator { margin: 0 8px; color: #CCC; }
.sub-view-card { background-color: #F9F9F9; border-radius: 16px; padding: 20px 24px; margin-bottom: 20px; }
.header-card { display: flex; justify-content: space-between; align-items: center; }
.header-card-actions { display: flex; gap: 8px; }
.list-card { padding-bottom: 12px; }
.card-left { display: flex; align-items: center; gap: 12px; }
.icon-orange { color: #FF7A00; }
.card-title { font-size: 20px; font-weight: 600; color: #374151; margin: 0; }
.card-subtitle { font-size: 16px; font-weight: 600; color: #4B5563; margin: 0; }
.icon-btn { background: none; border: none; cursor: pointer; padding: 6px; color: #9CA3AF; transition: color 0.2s; border-radius: 6px; }
.icon-btn:hover { color: #374151; background: rgba(0,0,0,0.05); }
.icon-btn.danger { align-self: flex-end; display: flex; align-items: center; gap: 8px; background-color: #f9f9f9; border: none; border-radius: 16px; color: #E63946; transition: color 0.2s; margin-bottom: 20px; font-size: 16px; font-weight: 400; padding: 12px 18px; }
.icon-btn.danger:hover { color: #EF4444; background: rgba(239, 68, 68, 0.1); }
.icon-btn.danger.card { margin: 0;}

.card-header-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; flex-wrap: wrap; gap: 16px; }
.card-actions { display: flex; align-items: center; gap: 12px; }
.card-search { display: flex; align-items: center; background-color: #FFFFFF; border-radius: 8px; padding: 0 12px; height: 40px; min-width: 240px; }
.card-search input { border: none; background: transparent; outline: none; font-size: 14px; color: #374151; margin-left: 8px; width: 100%; }
.card-add-btn { display: flex; align-items: center; justify-content: center; width: 40px; height: 40px; border-radius: 50%; background-color: #FFFFFF; border: none; cursor: pointer; color: #6B7280; transition: background 0.2s; }
.card-add-btn:hover { background-color: #F3F4F6; }
.card-sort-btn { display: flex; align-items: center; gap: 8px; background-color: #FFFFFF; border: none; border-radius: 8px; padding: 0 12px; height: 40px; font-size: 14px; color: #6B7280; cursor: pointer; white-space: nowrap; transition: all 0.2s; }
.card-sort-btn:hover { background-color: #F3F4F6; }
.card-sort-btn.is-active { background-color: #FF7A00; color: white; }
.card-sort-btn.is-active svg { stroke: white; }

.card-list-item { display: flex; justify-content: space-between; align-items: center; padding: 16px 0; border-bottom: 1px solid #E5E7EB; cursor: pointer; transition: padding-left 0.2s; }
.card-list-item:last-child { border-bottom: none; }
.card-list-item:hover { padding-left: 8px; }
.card-list-item:hover .sub-text { color: #111827; }
.card-list-item.product-item { display: flex; justify-content: space-between; align-items: center; padding: 16px 0; border-bottom: 1px solid #E5E7EB; cursor: pointer; transition: padding-left 0.2s; }
.card-list-item.product-item:last-child { border-bottom: none; }
.rat-pri-del { display: flex; justify-content: space-between; align-items: center; gap: 12px; width: 50%; }
.sub-text { font-size: 14px; color: #4B5563; }
.delete-icon-btn { background: none; border: none; cursor: pointer; color: #9CA3AF; padding: 4px; transition: color 0.2s; }
.delete-icon-btn:hover { color: #EF4444; }
.card-empty { padding: 30px; text-align: center; color: #9CA3AF; font-size: 14px; }
.card-footer { padding-top: 20px; text-align: center; }
.show-more-text { color: #FF7A00; font-size: 14px; cursor: pointer; font-weight: 500; }
.show-more-text:hover { text-decoration: underline; }
/* Общие контейнеры карточек */
.user-info-card {
  background: #F9F9F9;
  border-radius: 20px;
  padding: 32px;
  margin-bottom: 24px;
}

.delete-account-wrapper {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 16px;
}

.card-header-main {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.card-title-group {
  display: flex;
  align-items: center;
  gap: 12px;
}

.card-main-title {
  font-size: 20px;
  font-weight: 600;
  color: #333;
  margin: 0;
}

/* Сетка полей ввода (визуальные блоки) */
.user-data-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}

.data-field label {
  display: block;
  font-size: 14px;
  color: #333;
  font-weight: 600;
  margin-bottom: 8px;
}

.field-value {
  background: #FFFFFF;
  border-radius: 12px;
  padding: 14px 20px;
  font-size: 14px;
  color: #666;
  min-height: 20px;
  width: -moz-available;
  outline: none;
  border: none;
}

.edit-controls {
  display: flex;
  gap: 20px;
}

.field-simple {
  font-size: 14px;
  color: #666;
  padding-top: 4px;
}

/* Таблица заказов */
.orders-detail-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.orders-detail-table th {
  text-align: left;
  font-size: 13px;
  color: #999;
  font-weight: 400;
  padding: 12px 8px;
}

.orders-detail-table td {
  padding: 18px 8px;
  font-size: 14px;
  color: #666;
  border-top: 1px solid #EEEEEE;
}

.id-cell { color: #999; font-size: 12px; }
.price-cell { font-weight: 500; color: #333; }
.address-cell { color: #888; }

.arrow-cell {
  text-align: right;
  width: 40px;
}

.show-more-footer {
  text-align: center;
  margin-top: 24px;
}

.show-more-link {
  color: #FF7A00;
  cursor: pointer;
  font-weight: 500;
  font-size: 14px;
}

/* Иконки */
.trash-icon {
  width: 20px;
  filter: invert(28%) sepia(94%) saturate(1553%) hue-rotate(338deg) brightness(91%) contrast(97%);
}

.edit-btn {
  background: none;
  border: none;
  cursor: pointer;
  position: relative;
  opacity: 1;

}

tr {
  transition: background-color 0.2s ease;
}

tr:hover {
  background-color: #F9f9f9;
}
</style>