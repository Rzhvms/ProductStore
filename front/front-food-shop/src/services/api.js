const API_URL = "http://localhost:5282/api/";

export const resendPinCode = async (email) => {
    const response = await fetch(`${API_URL}auth/resend-code`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            "email": email
        })
    });

    if (!response.ok) {
        const data = await response.json();
        if (data?.error === "User not found") {
            throw new Error("Пользователь не найден");
        }
        throw new Error("Не удалось получить код подтверждения");
    }
};

export const sendVerificationEmail = async (email, pin) => {
    const response = await fetch(`${API_URL}auth/verify-email`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            "email": email,
            "pin": pin
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

export const finishRegister = async (userId, name, lastName, phone, gender, birthdate) => {
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
            "gender": gender,
            "birthDate": birthdate
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

    const data = await response.json();
    if (!response.ok) {
        const message = data?.message;
        if (message === "UserDoesNotExist") {
            throw new Error("Аккаунт не найден");
        } else if (message === "CredentialsAreNotValid") {
            throw new Error("Неверный пароль");
        }
        throw new Error("Не удалось войти в систему");
    }

    return data;
};