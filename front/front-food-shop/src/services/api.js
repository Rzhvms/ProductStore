import api from '../api/instance';

export const resendPinCode = async (email) => {
    try {
        await api.post('auth/resend-code', { email });
    } catch (error) {
        const data = error.response?.data;
        if (data?.error === "User not found") {
            throw new Error("Пользователь не найден");
        }
        throw new Error("Не удалось отправить код подтверждения");
    }
};

export const sendVerificationEmail = async (email, pin) => {
    try {
        const response = await api.post('auth/verify-email', { email, pin });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось отправить пин-код");
    }
};

export const verifyEmail = async (email, pin) => {
    try {
        const response = await api.post('auth/confirm-operation', { email, pin });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось подтвердить операцию");
    }
};

export const changePassword = async (password, token) => {
    try {
        const response = await api.post('auth/change-password', { password }, { headers: { Authorization: `Bearer ${token}` } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось изменить пароль");
    }
};

export const createUser = async (email, password, claims) => {
    try {
        const response = await api.post('auth/register', { email, password, claims });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось создать аккаунт");
    }
};

export const finishRegister = async (userId, name, lastName, phone, gender, birthDate, about) => {
    try {
        const response = await api.patch(`auth/register`, { 
            userId,
            name,
            lastName,
            phone,
            gender,
            birthDate,
            about
        });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось завершить регистрацию");
    }
};

export const login = async (email, password) => {
    try {
        const response = await api.post('auth/login', { email, password });
        return response.data;
    } catch (error) {
        const data = error.response?.data;
        const message = data?.message;
        if (message === "UserDoesNotExist") {
            throw new Error("Аккаунт не найден");
        } else if (message === "CredentialsAreNotValid") {
            throw new Error("Неверный логин или пароль");
        }
        throw new Error("Не удалось войти");
    }
};

export const refreshToken = async () => {
    const refreshToken = localStorage.getItem("refreshToken") || sessionStorage.getItem("refreshToken") || null;
    return api.post('auth/refresh-token', { refreshToken });
};

export const logout = async () => {
    return api.post('auth/sign-out');
};