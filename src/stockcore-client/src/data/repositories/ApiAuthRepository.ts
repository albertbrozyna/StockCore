// data/repositories/ApiAuthRepository.ts
import { AuthRepository, LoginRequest, RegisterRequest } from "../../domain/repositories/AuthRepository";
import { User } from "../../domain/entities/User";

export class ApiAuthRepository implements AuthRepository {
  async login(data : LoginRequest): Promise<User> {
    const response = await fetch('http://localhost:5109', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    if (!response.ok) throw new Error('Błędne dane logowania');
    return response.json();
  }

    async register(data : RegisterRequest) : Promise<string>{
      const response = await fetch('http://localhost:5109/api/v1/register',{
        method: 'POST',
        headers : { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      })

      return 'tekst';
    }

}