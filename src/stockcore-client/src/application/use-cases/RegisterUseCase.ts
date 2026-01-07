import { AuthRepository, RegisterRequest } from "../../domain/repositories/AuthRepository";

export class RegisterUseCase{
    constructor(private authRepostory : AuthRepository){}

    async excectue(data : RegisterRequest){
        return await this.authRepostory.register(data);
    }
}