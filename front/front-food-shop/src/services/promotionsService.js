const STORAGE_KEY = 'app_promotions';

const SEED_DATA = [
  {
    id: 1,
    title: 'Бонус новым покупателям',
    description: 'Дарим 500 рублей на первый заказ',
    benefitType: 'bonus',
    value: 500,
    valueType: 'fixed',
    dateStart: '2025-01-01',
    dateEnd: '2025-06-30',
    status: 'active',
    color: '#C8E6C9',
    targetType: 'all',
    targetIds: [],
    selectedItems: [],
    createdAt: Date.now(),
    updatedAt: Date.now()
  }
];

function initStorage() {
  const existing = localStorage.getItem(STORAGE_KEY);
  if (!existing) {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(SEED_DATA));
  }
}

export function getAllPromotions() {
  initStorage();
  try {
    return JSON.parse(localStorage.getItem(STORAGE_KEY)) || [];
  } catch (e) {
    console.error('Ошибка чтения акций:', e);
    return [];
  }
}

export function getPromotionById(id) {
  const promotions = getAllPromotions();
  return promotions.find(p => p.id === id) || null;
}

export function getActivePromotions() {
  const promotions = getAllPromotions();
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  return promotions.filter(promo => {
    if (promo.status !== 'active') return false;

    const startDate = new Date(promo.dateStart);
    const endDate = promo.dateEnd ? new Date(promo.dateEnd) : null;

    const started = startDate <= today;
    const notEnded = !endDate || endDate >= today;

    return started && notEnded;
  });
}

export function getPromotionsForProduct(productId, categoryId = null, subcategoryId = null) {
  const activePromos = getActivePromotions();

  return activePromos.filter(promo => {
    if (promo.targetType === 'all') return true;

    if (promo.targetType === 'product') {
      return promo.targetIds?.includes(productId);
    }

    if (promo.targetType === 'category' && categoryId) {
      return promo.targetIds?.includes(categoryId);
    }

    if (promo.targetType === 'subcategory' && subcategoryId) {
      return promo.targetIds?.includes(subcategoryId);
    }

    return false;
  });
}

export function calculateDiscountedPrice(originalPrice, productId, categoryId = null, subcategoryId = null) {
  const promos = getPromotionsForProduct(productId, categoryId, subcategoryId);
  
  if (promos.length === 0) {
    return { 
      finalPrice: originalPrice, 
      discount: 0, 
      appliedPromo: null 
    };
  }

  let bestDiscount = 0;
  let bestPromo = null;

  for (const promo of promos) {
    if (promo.benefitType !== 'discount') continue;

    let discount = 0;
    if (promo.valueType === 'percent') {
      discount = originalPrice * (promo.value / 100);
    } else {
      discount = promo.value;
    }

    if (discount > bestDiscount) {
      bestDiscount = discount;
      bestPromo = promo;
    }
  }

  const finalPrice = Math.max(0, originalPrice - bestDiscount);

  return {
    finalPrice: Math.round(finalPrice * 100) / 100,
    discount: Math.round(bestDiscount * 100) / 100,
    appliedPromo: bestPromo
  };
}

export function getBonusesForOrder(cartItems) {
  const activePromos = getActivePromotions();
  const bonusPromos = activePromos.filter(p => p.benefitType === 'bonus');
  
  let totalBonus = 0;
  const appliedBonuses = [];

  for (const promo of bonusPromos) {
    let applies = false;

    if (promo.targetType === 'all') {
      applies = true;
    } else {
      for (const item of cartItems) {
        if (promo.targetType === 'product' && promo.targetIds?.includes(item.productId)) {
          applies = true;
          break;
        }
        if (promo.targetType === 'category' && promo.targetIds?.includes(item.categoryId)) {
          applies = true;
          break;
        }
      }
    }

    if (applies) {
      const bonusAmount = promo.valueType === 'percent' 
        ? 0 // Бонусы в процентах нужно считать от суммы заказа
        : promo.value;
      
      totalBonus += bonusAmount;
      appliedBonuses.push({ promo, amount: bonusAmount });
    }
  }

  return { totalBonus, appliedBonuses };
}

export function createPromotion(data) {
  const promotions = getAllPromotions();
  
  const newPromo = {
    ...data,
    id: Date.now(),
    targetIds: data.selectedItems?.map(item => item.id) || [],
    createdAt: Date.now(),
    updatedAt: Date.now()
  };

  promotions.push(newPromo);
  savePromotions(promotions);
  
  notifyChange('created', newPromo);
  
  return newPromo;
}

export function updatePromotion(id, data) {
  const promotions = getAllPromotions();
  const index = promotions.findIndex(p => p.id === id);
  
  if (index === -1) return null;

  promotions[index] = {
    ...promotions[index],
    ...data,
    targetIds: data.selectedItems?.map(item => item.id) || promotions[index].targetIds,
    updatedAt: Date.now()
  };

  savePromotions(promotions);
  notifyChange('updated', promotions[index]);
  
  return promotions[index];
}

export function updatePromotionStatus(id, status) {
  return updatePromotion(id, { status });
}

export function deletePromotion(id) {
  const promotions = getAllPromotions();
  const filtered = promotions.filter(p => p.id !== id);
  
  if (filtered.length === promotions.length) return false;
  
  savePromotions(filtered);
  notifyChange('deleted', { id });
  
  return true;
}

function savePromotions(promotions) {
  localStorage.setItem(STORAGE_KEY, JSON.stringify(promotions));
}

function notifyChange(action, data) {
  window.dispatchEvent(new CustomEvent('promotions-updated', {
    detail: { action, data, timestamp: Date.now() }
  }));
  
}

export function subscribeToChanges(callback) {
  const storageHandler = (e) => {
    if (e.key === STORAGE_KEY) {
      callback(getAllPromotions());
    }
  };
  
  const customHandler = (e) => {
    callback(getAllPromotions(), e.detail);
  };

  window.addEventListener('storage', storageHandler);
  window.addEventListener('promotions-updated', customHandler);

  return () => {
    window.removeEventListener('storage', storageHandler);
    window.removeEventListener('promotions-updated', customHandler);
  };
}

export function resetToDefaults() {
  localStorage.setItem(STORAGE_KEY, JSON.stringify(SEED_DATA));
  notifyChange('reset', null);
}

export function exportPromotions() {
  return JSON.stringify(getAllPromotions(), null, 2);
}

export function importPromotions(jsonString) {
  try {
    const data = JSON.parse(jsonString);
    if (!Array.isArray(data)) throw new Error('Неверный формат');
    savePromotions(data);
    notifyChange('imported', null);
    return true;
  } catch (e) {
    console.error('Ошибка импорта:', e);
    return false;
  }
}