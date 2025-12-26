<template>
  <div class="category-item">
    <div class="category-header" :class="{ 'isactive': isOpen }" @click="toggle">
      <span class="category-name">{{ item.name }}</span>
      <span v-if="item.children && item.children.length" class="arrow" :class="{ rotated: isOpen }">▼</span>
    </div>
    <Transition name="slide">
      <div v-if="isOpen && item.children" class="subcategories">
        <Category v-for="(sub, index) in item.children" :key="index" :item="sub" />
      </div>
    </Transition>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const props = defineProps({
  item: {
    type: Object,
    required: true
  }
});

const isOpen = ref(false);

const toggle = () => {
  if (props.item.children && props.item.children.length) {
    isOpen.value = !isOpen.value;
  }
};
</script>

<style scoped>
.category-item {
  width: 100%;
}

.category-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 16px;
  background: #fff;
  border-bottom: 1px solid #eee;
  cursor: pointer;
  transition: background 0.3s;
  font-size: 14px;
  color: #333;
}

.category-header.isactive {
  background: #f3f3f3;
  font-weight: 500;
}

.category-header:hover {
  background: #f9f9f9;
}

.arrow {
  transition: transform 0.3s ease;
  opacity: 0.5;
}

.arrow.rotated {
  transform: rotate(180deg);
}

.subcategories {
  background-color: #fff;
  padding-left: 20px;
  border-left: 1px solid #eee;
}

.slide-enter-active,
.slide-leave-active {
  transition: all 0.3s ease-out;
  max-height: 500px;
  opacity: 1;
  overflow: hidden;
}

.slide-enter-from,
.slide-leave-to {
  max-height: 0;
  opacity: 0;
}
</style>
