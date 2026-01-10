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

export const getUser = async () => {
    try {
        const response = await api.get('user/profile');
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить данные пользователя");
    }
};

export const updateUser = async (json) => {
    try {
        const response = await api.patch('user/profile', json);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось обновить данные пользователя");
    }
};

export const updatePassword = async (oldPassword, newPassword) => {
    try {
        const response = await api.patch('user/me/password', { oldPassword, newPassword });
        const data = response.data;
        if (!data.isSuccess) {
            throw new Error(data.message);
        }
        return data;
    } catch (error) {
        throw new Error("Не удалось изменить пароль: " + error.message);
    }
};

export const getProducts = async (pageNumber, pageSize) => {
    try {
        const response = await api.get('product/list', { params: { pageNumber, pageSize } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список товаров");
    }
};

export const getProduct = async (id) => {
    try {
        const response = await api.get(`product/${id}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить товар");
    }
};

export const getCategoryProducts = async (categoryId, pageNumber, pageSize) => {
    try {
        const response = await api.get(`product/${categoryId}/list`, { params: { pageNumber, pageSize } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список товаров по категории");
    }
};

export const productApi = {
    get: getProduct,
    getList: getProducts,
    getCategory: getCategoryProducts,
}

export const getCart = async () => {
    try {
        const response = await api.get('cart');
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список товаров");
    }
};

export const clearCart = async () => {
    try {
        const response = await api.delete('cart');
        return response.data;
    } catch (error) {
        throw new Error("Не удалось очистить корзину");
    }
};

export const addToCart = async (productId, quantity) => {
    try {
        const response = await api.post('cart/items', { productId, quantity });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось добавить товар в корзину");
    }
};

export const removeFromCart = async (productId) => {
    try {
        const response = await api.delete(`cart/items/${productId}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось удалить товар из корзины");
    }
};

export const cartApi = {
    get: getCart,
    clear: clearCart,
    add: addToCart,
    remove: removeFromCart,
}

export const getAddressSuggestions = async (query) => {
    try {
        const response = await api.get('address/suggest', { params: { q: query } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить адреса");
    }
};