<template>
  <aside class="sidebar-container">
    <!-- КНОПКА КАТАЛОГА -->
    <div class="catalog-btn" @click="isCatalogOpen = !isCatalogOpen">
      <div class="burger-icon">
        <span />
        <span />
        <span />
      </div>
      <span>Каталог</span>
      <span class="main-arrow" :class="{ rotated: isCatalogOpen }">▼</span>
    </div>

    <!-- КАТЕГОРИИ -->
    <Transition name="fade">
      <div v-if="isCatalogOpen" class="catalog-list">
        <div v-if="loading" class="loading-row">Загрузка...</div>
        <div v-else-if="error" class="error-row">Ошибка загрузки категорий</div>
        <div v-else>
          <div
              v-for="cat in categories"
              :key="cat.id"
              class="category-item"
          >
            <!-- КАТЕГОРИЯ (показываем name) -->
            <div
                class="category-title"
                :class="{ active: String(activeCategoryId) === String(cat.id) }"
                @click="selectCategory(cat)"
            >
              {{ cat.name }}
            </div>

            <!-- ПОДКАТЕГОРИИ (показываем name) -->
            <div
                v-if="cat.sub && String(activeCategoryId) === String(cat.id)"
                class="subcategory-list"
            >
              <div
                  v-for="sub in cat.sub"
                  :key="sub.id"
                  class="subcategory-item"
                  :class="{ active: String(activeSubcategoryId) === String(sub.id) }"
                  @click.stop="selectSubcategory(sub, cat)"
              >
                {{ sub.name }}
              </div>
            </div>
          </div>

          <div v-if="categories.length === 0" class="empty-row">
            Категории не найдены
          </div>
        </div>
      </div>
    </Transition>
  </aside>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue'
import api from '@/api/instance'

const emit = defineEmits(['update:filters'])

const props = defineProps({
  filters: {
    type: Object,
    required: true
  }
})

/* state */
const isCatalogOpen = ref(true)
const loading = ref(false)
const error = ref(false)

const categories = ref([]) // normalized: [{ id, name, sub: [{id,name}, ...] }]

const activeCategoryId = ref(null)
const activeCategoryName = ref(null)
const activeSubcategoryId = ref(null)
const activeSubcategoryName = ref(null)

/* normalize server list to hierarchy
   server items: { categoryId, categoryName, parentCategoryId }
*/
const buildHierarchy = (raw) => {
  if (!Array.isArray(raw)) return []

  // map by id
  const map = new Map()
  raw.forEach(item => {
    const id = item.categoryId
    const name = item.categoryName ?? ''
    map.set(id, { id, name, parentId: item.parentCategoryId ?? null, sub: [] })
  })

  // determine roots and attach children
  const roots = []
  for (const [id, node] of map) {
    const parentId = node.parentId
    // treat parentId === id or null or undefined as root
    if (!parentId || String(parentId) === String(id)) {
      roots.push(node)
    } else {
      const parent = map.get(parentId)
      if (parent) {
        parent.sub.push({ id: node.id, name: node.name })
      } else {
        // parent not found -> treat as root
        roots.push(node)
      }
    }
  }

  // sort for nicer UI
  roots.sort((a,b) => String(a.name).localeCompare(String(b.name)))
  roots.forEach(r => r.sub.sort((a,b) => String(a.name).localeCompare(String(b.name))))

  return roots
}

/* load categories from API */
const loadCategories = async () => {
  loading.value = true
  error.value = false
  try {
    const resp = await api.get('categories/list')
    // response expected: array of { categoryId, categoryName, parentCategoryId }
    const raw = resp.data ?? []
    categories.value = buildHierarchy(raw)
  } catch (e) {
    console.error('Ошибка получения категорий', e)
    error.value = true
    categories.value = []
  } finally {
    loading.value = false
    // sync props after categories loaded
    syncFromProps(props.filters)
  }
}

/* selection handlers */
const selectCategory = (cat) => {
  if (!cat) return
  // если кликаем по уже выбранной категории — сбрасываем подкатегорию
  if (String(activeCategoryId.value) === String(cat.id)) {
    activeCategoryId.value = cat.id
    activeCategoryName.value = cat.name
    activeSubcategoryId.value = null
    activeSubcategoryName.value = null
  } else {
    activeCategoryId.value = cat.id
    activeCategoryName.value = cat.name
    activeSubcategoryId.value = null
    activeSubcategoryName.value = null
  }
  emitFilters()
}

const selectSubcategory = (sub, parentCat) => {
  if (!sub) return
  // выставляем подкатегорию и гарантируем, что активная категория — её родитель
  activeSubcategoryId.value = sub.id
  activeSubcategoryName.value = sub.name

  if (parentCat) {
    activeCategoryId.value = parentCat.id
    activeCategoryName.value = parentCat.name
  }

  // При выборе подкатегории эмитим categoryId = sub.id (т.к. продукты хранят categoryId)
  emitFilters()
}

/* emit filters in backward-compatible format:
   { category: <name>, categoryId: <id>, subcategory: <name>, subcategoryId: <id> }
   IMPORTANT: categoryId должен содержать subcategoryId, если выбрана подкатегория
*/
const emitFilters = () => {
  // если выбрана подкатегория — используем её id в поле categoryId
  const effectiveCategoryId = activeSubcategoryId.value ?? activeCategoryId.value ?? null
  const effectiveCategoryName = activeSubcategoryId.value ? activeSubcategoryName.value : (activeCategoryName.value ?? null)

  emit('update:filters', {
    // legacy visible fields
    category: activeCategoryName.value ?? null,
    subcategory: activeSubcategoryName.value ?? null,
    // ids — categoryId для backend фильтрации всегда содержит id выбранного уровня (subcategory or category)
    categoryId: effectiveCategoryId,
    subcategoryId: activeSubcategoryId.value ?? null
  })
}

/* sync from parent props.filters */
const syncFromProps = (newFilters) => {
  if (!newFilters) {
    activeCategoryId.value = null
    activeCategoryName.value = null
    activeSubcategoryId.value = null
    activeSubcategoryName.value = null
    return
  }

  // If parent sent subcategoryId, find it in our tree and set parent accordingly
  if (newFilters.subcategoryId) {
    const subId = String(newFilters.subcategoryId)
    let foundParent = null
    let foundSub = null

    for (const cat of categories.value) {
      const s = cat.sub.find(x => String(x.id) === subId)
      if (s) {
        foundParent = cat
        foundSub = s
        break
      }
    }

    if (foundSub && foundParent) {
      activeCategoryId.value = foundParent.id
      activeCategoryName.value = foundParent.name
      activeSubcategoryId.value = foundSub.id
      activeSubcategoryName.value = foundSub.name
      return
    } else {
      // если не нашли — просто применим из incoming filters (fallback)
      activeSubcategoryId.value = newFilters.subcategoryId
      activeSubcategoryName.value = newFilters.subcategory ?? null
      // если пришёл categoryId, установим его как активную категорию
      if (newFilters.categoryId) {
        activeCategoryId.value = newFilters.categoryId
        activeCategoryName.value = newFilters.category ?? null
      }
      return
    }
  }

  // Если пришёл только categoryId — ищем категорию по id
  if (newFilters.categoryId) {
    const cat = categories.value.find(c => String(c.id) === String(newFilters.categoryId))
    if (cat) {
      activeCategoryId.value = cat.id
      activeCategoryName.value = cat.name
      activeSubcategoryId.value = null
      activeSubcategoryName.value = null
      return
    } else {
      // возможно пришло id подкатегории в поле categoryId (старый бек)
      const subId = String(newFilters.categoryId)
      let foundParent = null
      let foundSub = null
      for (const c of categories.value) {
        const s = c.sub.find(x => String(x.id) === subId)
        if (s) {
          foundParent = c
          foundSub = s
          break
        }
      }
      if (foundSub && foundParent) {
        activeCategoryId.value = foundParent.id
        activeCategoryName.value = foundParent.name
        activeSubcategoryId.value = foundSub.id
        activeSubcategoryName.value = foundSub.name
        return
      }
      // fallback: set category name/id from incoming
      activeCategoryId.value = newFilters.categoryId
      activeCategoryName.value = newFilters.category ?? null
      activeSubcategoryId.value = null
      activeSubcategoryName.value = null
      return
    }
  }

  // If only names provided, try to match by name
  if (newFilters.category) {
    const found = categories.value.find(c => String(c.name) === String(newFilters.category))
    if (found) {
      activeCategoryId.value = found.id
      activeCategoryName.value = found.name
    } else {
      activeCategoryId.value = null
      activeCategoryName.value = newFilters.category
    }
  } else {
    activeCategoryId.value = null
    activeCategoryName.value = null
  }

  if (newFilters.subcategory) {
    const cat = categories.value.find(c => String(c.id) === String(activeCategoryId.value))
    const foundSub = cat?.sub?.find(s => String(s.name) === String(newFilters.subcategory))
    if (foundSub) {
      activeSubcategoryId.value = foundSub.id
      activeSubcategoryName.value = foundSub.name
    } else {
      activeSubcategoryId.value = null
      activeSubcategoryName.value = newFilters.subcategory
    }
  } else {
    activeSubcategoryId.value = null
    activeSubcategoryName.value = null
  }
}

/* watch prop changes */
watch(() => props.filters, (nf) => {
  // when parent changes filters (e.g., reset) — sync local state
  syncFromProps(nf)
}, { immediate: true, deep: true })

/* load on mount */
onMounted(() => {
  loadCategories()
})
</script>

<style scoped>
.sidebar-container {
  width: 280px;
  background: #fff;
  border-right: 1px solid #eee;
  display: flex;
  flex-direction: column;
}

.catalog-btn {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 18px 20px;
  font-weight: 800;
  cursor: pointer;
}

.catalog-btn:hover {
  background: #f7f7f7;
}

.burger-icon span {
  display: block;
  width: 18px;
  height: 2px;
  background: #333;
  margin-bottom: 3px;
}

.main-arrow {
  margin-left: auto;
  transition: 0.3s;
  font-size: 12px;
}

.main-arrow.rotated {
  transform: rotate(180deg);
}

.catalog-list {
  padding: 10px 0;
}

.loading-row,
.error-row,
.empty-row {
  padding: 12px 20px;
  color: #666;
}

.category-item {
  padding: 0 20px;
}

.category-title {
  padding: 10px 0;
  font-weight: 700;
  cursor: pointer;
}

.category-title.active {
  color: #f57c00;
}

.subcategory-list {
  padding-left: 15px;
  margin-bottom: 10px;
}

.subcategory-item {
  padding: 6px 0;
  font-size: 14px;
  cursor: pointer;
  color: #555;
}

.subcategory-item.active {
  color: #f57c00;
  font-weight: 700;
}
</style>
