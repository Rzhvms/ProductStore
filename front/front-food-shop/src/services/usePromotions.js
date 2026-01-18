import { ref, onMounted, onUnmounted, computed } from 'vue';
import * as promotionsService from '@/services/promotionsService';

export function usePromotions() {
  const promotions = ref([]);
  const isLoading = ref(true);
  let unsubscribe = null;

  function loadPromotions() {
    isLoading.value = true;
    promotions.value = promotionsService.getAllPromotions();
    isLoading.value = false;
  }

  onMounted(() => {
    loadPromotions();
    
    unsubscribe = promotionsService.subscribeToChanges((newPromotions) => {
      promotions.value = newPromotions;
    });
  });

  onUnmounted(() => {
    if (unsubscribe) unsubscribe();
  });

  const activePromotions = computed(() => 
    promotions.value.filter(p => p.status === 'active')
  );

  const archivedPromotions = computed(() => 
    promotions.value.filter(p => p.status === 'archived')
  );

  function createPromotion(data) {
    const newPromo = promotionsService.createPromotion(data);
    promotions.value = promotionsService.getAllPromotions();
    return newPromo;
  }

  function updatePromotion(id, data) {
    const updated = promotionsService.updatePromotion(id, data);
    promotions.value = promotionsService.getAllPromotions();
    return updated;
  }

  function deletePromotion(id) {
    const result = promotionsService.deletePromotion(id);
    promotions.value = promotionsService.getAllPromotions();
    return result;
  }

  function changeStatus(id, status) {
    return updatePromotion(id, { status });
  }

  return {
    promotions,
    activePromotions,
    archivedPromotions,
    isLoading,
    loadPromotions,
    createPromotion,
    updatePromotion,
    deletePromotion,
    changeStatus
  };
}

export function useProductDiscount(productId, categoryId = null, subcategoryId = null) {
  const originalPrice = ref(0);
  const discountInfo = ref({ finalPrice: 0, discount: 0, appliedPromo: null });

  function calculate(price) {
    originalPrice.value = price;
    discountInfo.value = promotionsService.calculateDiscountedPrice(
      price,
      productId,
      categoryId,
      subcategoryId
    );
  }

  const hasDiscount = computed(() => discountInfo.value.discount > 0);
  const discountPercent = computed(() => {
    if (!hasDiscount.value || !originalPrice.value) return 0;
    return Math.round((discountInfo.value.discount / originalPrice.value) * 100);
  });

  return {
    originalPrice,
    discountInfo,
    hasDiscount,
    discountPercent,
    calculate
  };
}