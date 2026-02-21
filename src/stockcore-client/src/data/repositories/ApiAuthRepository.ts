// data/repositories/ApiAuthRepository.ts
import type { AuthRepository, LoginRequest, RegisterRequest } from "../../domain/repositories/AuthRepository";
import type { User } from "../../domain/entities/User";
import axiosClient from "../api/axiosClient";

export class ApiAuthRepository implements AuthRepository {
  async login(data : LoginRequest): Promise<User> {
    const response = await axiosClient.post('/v1/login',data);

    return response.data();
  }

  async register(data : RegisterRequest) : Promise<string>{
    const response = await axiosClient.post('/api/v1/register',data);
    return response.data();
  }

}