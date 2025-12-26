<template>
    <aside class="sidebar-container">
        <div class="sidebar-header">
            <h2 class="logo">LUKOSCHKO</h2>
        </div>
        <div class="catalog-wrapper">
            <div class="catalog-btn" @click="isCatalogOpen = !isCatalogOpen">
                <div class="burger-icon">
                    <span></span>
                    <span></span>
                    <span></span>
                </div>
                <span>Каталог</span>
                <span class="main-arrow" :class="{ rotated: isCatalogOpen }">▼</span>
            </div>
            <Transition name="fade">
                <div v-if="isCatalogOpen" class="catalog-list">
                    <Category v-for="(cat, index) in categories" :key="index" :item="cat" />
                </div>
            </Transition>
        </div>
        <div class="other-links">
            <p>Другие пункты меню</p>
        </div>
    </aside>
</template>

<script setup>
import Category from './Category.vue';
import { ref } from 'vue';

const isCatalogOpen = ref(true);

const categories = ref([
    {
        name: 'Выпечка',
        children: [
            {
                name: 'Слойки'
            },
            {
                name: 'Пирожки'
            }
        ]
    },
    {
        name: 'Овощи'
    },
    {
        name: 'Фрукты'
    },
    {
        name: 'Крупы',
        children: [
            {
                name: 'Рис'
            },
            {
                name: 'Гречка'
            }
        ]
    }
]);
</script>

<style scoped>
.sidebar-container {
    width: 300px;
    height: 100vh;
    background-color: #fff;
    border-right: 1px solid #e5e5e5;
    display: flex;
    flex-direction: column;
    flex-shrink: 0;
}

.sidebar-header {
    padding: 20px;
}

.logo {
    color: #ff6600;
    font-weight: bold;
    font-size: 24px;
    margin: 0;
    text-transform: uppercase;
}

.catalog-wrapper {
    border-top: 1px solid #eee;
}

.catalog-btn {
    display: flex;
    align-items: center;
    gap: 10px;
    padding: 15px 20px;
    cursor: pointer;
    font-weight: bold;
    font-size: 18px;
    color: #333;
    transition: 0.3s;
}

.catalog-btn:hover {
    background-color: #ff6600;
}

.burger-icon span {
    display: block;
    width: 18px;
    height: 2px;
    background-color: currentColor;
    margin-bottom: 3px;
}

.main-arrow {
    transition: transform 0.3s;
    margin-left: auto;
    font-size: 12px;
}

.main-arrow.rotated {
    transform: rotate(180deg);
}

.catalog-list {
    border-top: 1px solid #eee;
    border-bottom: 1px solid #eee;
    background: #fff;
}

.other-links {
    padding: 20px;
    color: #999;
}
</style>