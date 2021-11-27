export interface Entity {
  id: string;
  createdAt: string;
  updatedAt: string;
}

export interface User extends Entity {
  name: string;
  username: string;
  email: string;
}

export interface Product extends Entity {
  name: string;
  description: string;
  imageUrl?: string;
  price: number;
  quantity: number;
  ownerId: string; 
}

export interface Order extends Entity {
  price: number;
  quantity: number;
  product: Product;
  productId: string;
  ownerId: string;
}

export interface SignUp {
  name: string;
  username: string;
  email: string;
  password: string;
}

export interface CreateProductDTO {
  name: string;
  description: string;
  imageUrl?: string;
  price: number;
  quantity: number;
}

export interface PatchProductDTO {
  name: string;
  description: string;
  imageUrl?: string;
  price: number;
}

export interface CreateOrderDTO {
  price: number;
  quantity: number;
  productId: string;
  ownerId: string;
}

export async function signUp(data: SignUp, abort?: AbortSignal) {
  const res = await fetch(`/api/user/create`, {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json"
    },
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as User;
}

export async function logIn(email: string, password: string, abort?: AbortSignal) {
  const res = await fetch(`/api/user/login`, {
    method: "POST",
    body: JSON.stringify({
      email,
      password,
    }),
    headers: {
      "Content-Type": "application/json"
    },
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as User;
}

export async function logOut(abort?: AbortSignal) {
  const res = await fetch(`/api/user/logout`, {
    method: "POST",
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }
}

export async function getCurrentUser(abort?: AbortSignal) {
  const res = await fetch(`/api/user`, {
    method: "GET",
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as User;
}

export async function getProduct(id: string, abort?: AbortSignal) {
  const res = await fetch(`/api/products/${id}`, {
    method: "GET",
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as Product;
}

export async function getProducts(abort?: AbortSignal) {
  const res = await fetch(`/api/products`, {
    method: "GET",
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as Product[];
}

export async function createProduct(dto: CreateProductDTO, abort?: AbortSignal) {
  const res = await fetch(`/api/products`, {
    method: "POST",
    body: JSON.stringify(dto),
    headers: {
      "Content-Type": "application/json"
    },
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as Product;
}

export async function patchProduct(id: string, dto: PatchProductDTO, abort?: AbortSignal) {
  const res = await fetch(`/api/products/${id}`, {
    method: "PATCH",
    body: JSON.stringify(dto),
    headers: {
      "Content-Type": "application/json"
    },
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as Product;
}

export async function getOrder(id: string, abort?: AbortSignal) {
  const res = await fetch(`/api/orders/${id}`, {
    method: "GET",
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as Order;
}

export async function getOrders(abort?: AbortSignal) {
  const res = await fetch(`/api/orders`, {
    method: "GET",
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as Order[];
}

export async function createOrder(dto: CreateOrderDTO, abort?: AbortSignal) {
  const res = await fetch(`/api/orders`, {
    method: "POST",
    body: JSON.stringify(dto),
    headers: {
      "Content-Type": "application/json"
    },
    signal: abort,
  });

  if (!res.ok) {
    throw new Error(res.statusText);
  }

  return await res.json() as Order;
}