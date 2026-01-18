import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

// Auth pages
import Login from '../pages/auth/Login.vue'
import Register from '../pages/auth/Register.vue'
import ConfirmEmail from '../pages/auth/ConfirmEmail.vue'
import AccountNotFound from '../pages/auth/AccountNotFound.vue'
import FinishRegistration from '../pages/auth/FinishRegistration.vue'
import RecoveryCode from '@/pages/auth/RecoveryCode.vue'
import RecoveryNewPassword from '@/pages/auth/RecoveryNewPassword.vue'
import ForgotPassword from '../pages/auth/ForgotPassword.vue'

// Home
import Home from '../pages/home/Index.vue'

// Catalog
import CatalogIndex from '../pages/catalog/Index.vue'
import Catalog from '@/pages/catalog/Catalog.vue'
import Category from '../pages/catalog/Category.vue'
import Subcategory from '../pages/catalog/Subcategory.vue'
import ProductCard from '../pages/catalog/ProductCard.vue'

// Cart
import Cart from '../pages/cart/Index.vue'
import Payment from '../pages/cart/Payment.vue'
import PaymentSuccess from '../pages/cart/PaymentSuccess.vue'
import PaymentFailed from '../pages/cart/PaymentFailed.vue'

// Checkout
import CheckoutAddress from '../pages/checkout/Address.vue'
import CheckoutConfirm from '../pages/checkout/Confirm.vue'
import CheckoutPayment from '../pages/checkout/Payment.vue'

// Profile
import ProfileMain from '../pages/profile/ProfilePage.vue'
import ProfileOrders from '../pages/profile/ProfileOrders.vue'
import ProfileHelp from '../pages/profile/ProfileHelp.vue'

// Admin
import AdminIndex from '../pages/admin/Index.vue'
import ProfilePage from '../pages/profile/ProfilePage.vue'
import AdminProducts from '../pages/admin/AdminProducts.vue'
import AdminCategories from '../pages/admin/AdminCategories.vue'
import AdminUsers from '../pages/admin/AdminUsers.vue'
import AdminDashboard from '../pages/admin/AdminDashboard.vue'
import AdminStatistic from '../pages/admin/AdminStatistic.vue'
import AdminProduct from '../pages/admin/AdminProduct.vue'
import AdminProductAdd from '../pages/admin/AdminProductAdd.vue'

import App from '../App.vue'

const routes = [
  { path: '/', component: CatalogIndex },

  // Auth
  { path: '/login', component: Login, meta: { guestOnly: true } },
  { path: '/register', component: Register, meta: { guestOnly: true } },
  { path: '/confirm-email', component: ConfirmEmail, meta: { guestOnly: true } },
  { path: '/recovery-code', component: RecoveryCode, meta: { guestOnly: true } },
  { path: '/account-not-found', component: AccountNotFound, meta: { guestOnly: true } },
  { path: '/new-password', component: RecoveryNewPassword, meta: { guestOnly: true } },
  { path: '/finish-registration', component: FinishRegistration, meta: { guestOnly: true } },
  { path: '/forgot-password', component: ForgotPassword, meta: { guestOnly: true } },

  // Catalog
  { path: '/catalog', component: Catalog },
  { path: '/catalog/category', component: Category },
  { path: '/catalog/subcategory', component: Subcategory },
  { path: '/catalog/product/:id', component: ProductCard },

  // Cart
  { path: '/cart', component: Cart, meta: { requiresAuth: true } },
  { path: '/payment/:id', component: Payment, meta: { requiresAuth: true } },
  { path: '/payment/success', component: PaymentSuccess, meta: { requiresAuth: true } },
  { path: '/payment/failed', component: PaymentFailed, meta: { requiresAuth: true } },

  // Checkout
  { path: '/checkout/address', component: CheckoutAddress, meta: { requiresAuth: true } },
  { path: '/checkout/confirm', component: CheckoutConfirm, meta: { requiresAuth: true } },
  { path: '/checkout/payment', component: CheckoutPayment, meta: { requiresAuth: true } },

  // Profile
  { path: '/profile', component: ProfilePage, meta: { requiresAuth: true } },
  { path: '/profile/help', component: ProfileHelp, meta: { requiresAuth: true } },
  { path: '/profile/orders', component: ProfileOrders, meta: { requiresAuth: true } },

  // Admin
  { path: '/admin', component: AdminDashboard, meta: { requiresAuth: true } },
  { path: '/admin/products', component: AdminProducts, meta: { requiresAuth: true } },
  { path: '/admin/products/:id', component: AdminProduct, meta: { requiresAuth: true }, props: true },
  { path: '/admin/products/add', component: AdminProductAdd, meta: { requiresAuth: true } },
  { path: '/admin/categories', component: AdminCategories, meta: { requiresAuth: true } },
  { path: '/admin/statistics', component: AdminStatistic, meta: { requiresAuth: true } },
  { path: '/admin/users', component: AdminUsers, meta: { requiresAuth: true } },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const isAuthenticated = authStore.isAuthenticated;
  if (to.meta.requiresAuth && !isAuthenticated) {
    return next('/login');
  } else if (to.meta.guestOnly && isAuthenticated) {
    return next('/');
  }
  next();
});

export default router