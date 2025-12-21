export const API_URL = "http://localhost:5282/api/";

export const sendVerificationEmail = async (email) => {
    const response = await fetch(`${API_URL}send-verification-email`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            Email: email,
        })
    });

    if (!response.ok) {
        throw new Error("Не удалось получить данные пользователя");
    }

    const data = await response.json();
    return data;
};

export const createUser = async (email, password, claims) => {
    const response = await fetch(`${API_URL}auth/register`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            "email": email,
            "password": password,
            "claims": claims
        })
    });

    if (!response.ok) {
        throw new Error("Не удалось создать пользователя");
    }

    const data = await response.json();
    return data;
};

export const finishRegister = async (userId, name, lastName, phone, gender) => {
    const response = await fetch(`${API_URL}auth/register`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            "userId": userId,
            "name": name,
            "lastName": lastName,
            "phone": phone,
            "gender": gender
        })
    });

    if (!response.ok) {
        throw new Error("Не удалось завершить регистрацию");
    }

    const data = await response.json();
    return data;
};

export const login = async (email, password) => {
    const response = await fetch(`${API_URL}auth/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            "email": email,
            "password": password
        })
    });

    if (!response.ok) {
        throw new Error("Не удалось войти в систему");
    }

    const data = await response.json();
    return data;
};

export const getUser = async (idToken) => {
    const response = await fetch(`${API_URL}user/profile`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${idToken}`
        },
    });

    if (!response.ok) {
        throw new Error("Не удалось получить данные пользователя");
    }

    const data = await response.json();
    return data;
};

export const updateUser = async (idToken, json) => {
    const response = await fetch(`${API_URL}user/profile`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${idToken}`
        },
        body: JSON.stringify(json)
    });

    if (!response.ok) {
        throw new Error("Не удалось обновить данные пользователя");
    }

    const data = await response.json();
    return data;
};