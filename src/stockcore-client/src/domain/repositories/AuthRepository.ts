import type { User } from "../entities/User";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  firstName: string;
  lastName: string;
  userRole: number;
  password: string;
  phoneNumber: string;
  companyJoinCode: string;
}

export interface AuthRepository {
  login(data: LoginRequest): Promise<User>;
  register(data: RegisterRequest): Promise<string>;
}
