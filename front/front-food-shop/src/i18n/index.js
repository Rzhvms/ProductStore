import { createI18n } from 'vue-i18n'

// auth (login)
import authRu from './auth/ru'
import authEn from './auth/en'

// register
import registerRu from './auth/register/ru'
import registerEn from './auth/register/en'

// confirm email
import confirmEmailRu from './auth/confirm-email/ru'
import confirmEmailEn from './auth/confirm-email/en'

// finish registration
import finishRegistrationRu from './auth/finish-registration/ru'
import finishRegistrationEn from './auth/finish-registration/en'

// forgot password
import forgotPasswordRu from './auth/forgot-password/ru'
import forgotPasswordEn from './auth/forgot-password/en'

// recovery code
import recoveryCodeRu from './auth/recovery-code/ru'
import recoveryCodeEn from './auth/recovery-code/en'

// recovery password
import recoveryPasswordRu from './auth/recovery-password/ru'
import recoveryPasswordEn from './auth/recovery-password/en'

export const i18n = createI18n({
  legacy: false,
  locale: localStorage.getItem('lang') || 'ru',
  fallbackLocale: 'ru',
  messages: {
    ru: {
      auth: {
        ...authRu,
        register: registerRu,
        confirmEmail: confirmEmailRu,
        finishRegistration: finishRegistrationRu,
        forgotPassword: forgotPasswordRu,
        recoveryCode: recoveryCodeRu,
        recoveryPassword: recoveryPasswordRu
      }
    },
    en: {
      auth: {
        ...authEn,
        register: registerEn,
        confirmEmail: confirmEmailEn,
        finishRegistration: finishRegistrationEn,
        forgotPassword: forgotPasswordEn,
        recoveryCode: recoveryCodeEn,
        recoveryPassword: recoveryPasswordEn
      }
    }
  }
})