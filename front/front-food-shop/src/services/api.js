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

export const getAddressSuggestions = async (queryParams) => {
    try {
        const response = await api.get('suggest-address', { params: { queryParams } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить адреса");
    }
};

export const getCategory = async (id) => {
    try {
        const response = await api.get(`categories/${id}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить категорию");
    }
};

export const postReview = async (productId, rating, message) => {
    try {
        const response = await api.post('review', { productId, rating, message });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось оставить отзыв");
    }
};

export const getReview = async (id) => {
    try {
        const response = await api.get(`review/${id}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить отзыв");
    }
};

export const deleteReview = async (id) => {
    try {
        const response = await api.delete(`review/${id}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось удалить отзыв");
    }
};

export const getReviews = async (productId) => {
    try {
        const response = await api.get(`review/${productId}/list`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список отзывов");
    }
};

export const updateReview = async (id, rating, message) => {
    try {
        const response = await api.patch(`review/patch-review/${id}`, { rating, message });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось обновить отзыв");
    }
};

export const reviewApi = {
    get: getReview,
    delete: deleteReview,
    update: updateReview,
    getList: getReviews,
    post: postReview,
}

export const getFavoriteProducts = async (pageNumber = 1, pageSize = 10) => {
    try {
        const response = await api.get('favorite-products/list', { params: { pageNumber, pageSize } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список избранных товаров");
    }
};

export const addToFavorites = async (productId) => {
    try {
        const response = await api.post(`favorite-products/${productId}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось добавить товар в избранное");
    }
};

export const removeFromFavorites = async (productId) => {
    try {
        const response = await api.delete(`favorite-products/${productId}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось удалить товар из избранного");
    }
};

export const clearFavorites = async (productIdList) => {
    try {
        const response = await api.delete('favorite-products/delete-list', { productIdList });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось очистить избранное");
    }
};

export const favoriteApi = {
    get: getFavoriteProducts,
    add: addToFavorites,
    remove: removeFromFavorites,
    clear: clearFavorites,
}

export const createOrder = async (orderDetails) => {
    try {
        const response = await api.post('orders', orderDetails);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось создать заказ");
    }
};