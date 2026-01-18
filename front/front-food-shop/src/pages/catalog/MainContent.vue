<template>
    <main class="main-content">
        <div class="search-section">
            <div class="search-bar">
                <span class="search-icon">🔍</span>
                <input type="text" placeholder="Поиск..." />
                <button class="search-btn">Искать</button>
            </div>
        </div>
        <div class="section-block promo">
            <h3>Акции</h3>
            <div class="promo-content">

            </div>
        </div>
        <div class="section-block popular">
            <h3>Популярные товары</h3>
            <div class="products-grid">
                <div class="product-card" v-for="n in 4" :key="n"></div>
            </div>
        </div>
    </main>
</template>

<script setup>
</script>

<style scoped>
.main-content {
    flex-grow: 1;
    padding: 30px;
    background-color: #f8f8f8;
    overflow-y: auto;
}

.search-section {
    margin-bottom: 30px;
}

.search-bar {
    display: flex;
    align-items: center;
    background-color: #fff;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    overflow: hidden;
    padding: 5px 15px;
}

.search-icon {
    color: #999;
    margin-right: 10px;
}

input {
    flex-grow: 1;
    border: none;
    padding: 10px;
    outline: none;
    font-size: 16px;
}

.search-btn {
    background-color: #ff6600;
    color: #fff;
    border: none;
    padding: 8px 20px;
    border-radius: 4px;
    cursor: pointer;
}

.section-block {
    margin-bottom: 30px;
}

.section-block h3 {
    margin-bottom: 15px;
    color: #333;
}

.promo-content {
    height: 200px;
    background-color: #e5e5e5;
    border: 2px solid #333;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #555;
}

.products-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
    gap: 20px;
    min-height: 200px;
    border: 2px solid #333;
    padding: 20px;
    background-color: #fff;
}

.product-card {
    height: 100px;
    background-color: #eee;
}
</style>