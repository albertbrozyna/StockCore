import { AuthRepository } from "../../domain/repositories/AuthRepository";

export class LoginUseCase {
  constructor(private authRepository: AuthRepository) {}

  async execute(email: string, password: string) {
    return await this.authRepository.login({email, password});
  }
}