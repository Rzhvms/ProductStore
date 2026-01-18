import api from '@/api/instance';

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

export const refreshToken = async (accessToken) => {
    const refreshToken = localStorage.getItem("refreshToken") || sessionStorage.getItem("refreshToken") || null;
    return api.post('auth/refresh-token', { accessToken, refreshToken });
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
export const createCategory = async (name, parentId = null) => {
    try {
        const response = await api.post('categories/create', { name, parentId });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось создать категорию");
    }
};

export const getCategories = async () => {
    try {
        const response = await api.get('categories/list');
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список категорий");
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

export const updateCategory = async (id, name, parentId = null) => {
    try {
        const response = await api.put(`categories/${id}`, { name, parentId });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось обновить категорию");
    }
};

export const deleteCategory = async (id) => {
    try {
        const response = await api.delete(`categories/${id}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось удалить категорию");
    }
};

export const categoryApi = {
    create: createCategory,
    get: getCategories,
    getById: getCategory,
    update: updateCategory,
    delete: deleteCategory
};

export const createProduct = async (name, providerId, description, price, quantity, categoryId, characteristics) => {
    try {
        const response = await api.post('admin-product/create', { name, providerId, description, price, quantity, categoryId, characteristics });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось создать товар");
    }
};

export const getProducts = async (pageNumber = 1, pageSize = 10) => {
    try {
        const response = await api.get(`admin-product/list`, {
            params: {
                pageNumber,
                pageSize,
            },
        });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список товаров");
    }
};

export const getProduct = async (id) => {
    try {
        const response = await api.get(`admin-product/${id}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить товар");
    }
};

export const updateProduct = async (id, name, providerId, description, price, quantity, categoryId, characteristics) => {
    try {
        const response = await api.put(`admin-product/${id}`, { name, providerId, description, price, quantity, categoryId, characteristics });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось обновить товар");
    }
};

export const deleteProduct = async (id) => {
    try {
        const response = await api.delete(`admin-product/${id}`);
        return response.data;
    } catch (error) {
        throw new Error("Не удалось удалить товар");
    }
};

export const getCategoryProducts = async (categoryId, pageNumber = 1, pageSize = 10) => {
    try {
        const response = await api.get(`admin-category/${categoryId}/list`, {
            params: {
                pageNumber,
                pageSize,
            },
        });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить список товаров");
    }
}

export const adminProductApi = {
    create: createProduct,
    get: getProducts,
    getById: getProduct,
    update: updateProduct,
    delete: deleteProduct,
    getCategoryProducts: getCategoryProducts
};

export const getUserProfile = async (userId) => {
    try {
        const response = await api.get(`admin/user-info/profile`, { params: { userId } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить профиль пользователя");
    }
};

export const updateUserProfile = async (userId, name, lastName, phone, gender, birthDate, about, email) => {
    try {
        const response = await api.patch(`admin/user-info/profile`, { name, lastName, phone, gender, birthDate, about, email }, { params: { userId } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось обновить профиль пользователя");
    }
};

export const getUserAddress = async (userId) => {
    try {
        const response = await api.get(`admin/user-info/address`, { params: { userId } });
        return response.data;
    } catch (error) {
        throw new Error("Не удалось получить адрес пользователя");
    }
};

export const getUserOrders = async (userId, pageNumber = 1, pageSize = 10) => {
    try {
        const response = await api.get(`orders/list`, { params: { pageNumber, pageSize } });
        const data = response.data;
        const ordersList = data.orderList;
        const userOrders = ordersList.filter(order => order.customerId === userId);
        return userOrders;
    } catch (error) {
        throw new Error("Не удалось получить список заказов");
    }
};

export const getUserProfiles = async (pageNumber = 1, pageSize = 10) => {
    try {
        const response = await api.get(`orders/list`, { params: { pageNumber, pageSize } });
        const data = response.data;
        const ordersList = data.orderList;
        const userProfiles = ordersList.map(order => ({
            id: order.customerId
        }));
        return userProfiles;
    } catch (error) {
        throw new Error("Не удалось получить список пользователей");
    }
};

export const adminUserApi = {
    getProfile: getUserProfile,
    updateProfile: updateUserProfile,
    getAddress: getUserAddress,
    getOrders: getUserOrders,
    getProfiles: getUserProfiles
};