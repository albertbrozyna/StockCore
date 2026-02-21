import type { AuthRepository, RegisterRequest } from "../../domain/repositories/AuthRepository";

export class RegisterUseCase{
    private authRepostory : AuthRepository;
    constructor(authRepostory : AuthRepository){
        this.authRepostory = authRepostory;
    }

    async execute(data : RegisterRequest){
        return await this.authRepostory.register(data);
    }
}