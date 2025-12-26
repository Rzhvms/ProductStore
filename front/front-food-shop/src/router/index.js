import { createRouter, createWebHistory } from 'vue-router'

// Auth pages
import Login from '../pages/auth/Login.vue'
import Register from '../pages/auth/Register.vue'
import ConfirmEmail from '../pages/auth/ConfirmEmail.vue'
import AccountNotFound from '../pages/auth/AccountNotFound.vue'
import FinishRegistration from '../pages/auth/FinishRegistration.vue'
import RecoveryCode from '@/pages/auth/RecoveryCode.vue'
import RecoveryNewPassword from '@/pages/auth/RecoveryNewPassword.vue'
import ForgotPassword from '../pages/auth/ForgotPassword.vue'
import ForgotPasswordOptions from '../pages/auth/ForgotPasswordOptions.vue'

// Home
import Home from '../pages/home/Index.vue'

// Catalog
import CatalogIndex from '../pages/catalog/Index.vue'
import Category from '../pages/catalog/Category.vue'
import Subcategory from '../pages/catalog/Subcategory.vue'
import ProductCard from '../pages/catalog/ProductCard.vue'

// Cart
import Cart from '../pages/cart/Index.vue'

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

import App from '../App.vue'

const routes = [
  { path: '/', component: App },

  // Auth
  { path: '/login', component: Login },
  { path: '/register', component: Register },
  { path: '/confirm-email', component: ConfirmEmail },
  { path: '/recovery-code', component: RecoveryCode },
  { path: '/account-not-found', component: AccountNotFound },
  { path: '/new-password', component: RecoveryNewPassword },
  { path: '/finish-registration', component: FinishRegistration },
  { path: '/forgot-password', component: ForgotPassword },
  { path: '/forgot-password-options', component: ForgotPasswordOptions },

  // Catalog
  { path: '/catalog', component: CatalogIndex },
  { path: '/catalog/category', component: Category },
  { path: '/catalog/subcategory', component: Subcategory },
  { path: '/catalog/product', component: ProductCard },

  // Cart + Checkout
  { path: '/cart', component: Cart },
  { path: '/checkout/address', component: CheckoutAddress },
  { path: '/checkout/confirm', component: CheckoutConfirm },
  { path: '/checkout/payment', component: CheckoutPayment },

  // Profile
  { path: '/profile', component: ProfileMain },
  { path: '/orders', component: ProfileOrders },
  { path: '/help', component: ProfileHelp },

  // Admin
  { path: '/admin', component: AdminIndex },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router