// presentation/hooks/useAuth.ts
import { useState } from 'react';
import { ApiAuthRepository } from '../../data/repositories/ApiAuthRepository';
import type {RegisterRequest } from '../../domain/repositories/AuthRepository';
import {RegisterUseCase} from '../../application/use-cases/RegisterUseCase';
import { LoginUseCase } from '../../application/use-cases/LoginUseCase';

interface UseAuthReturn{
  handleLogin: (email: string, password: string) => Promise<void>;
  handleRegister: (data : RegisterRequest) => Promise<void>;
  loading: boolean;
  error: string | null;
}

export const useAuth = () : UseAuthReturn  => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Inicjalizacja (w prawdziwym prod. użyłbyś Dependency Injection / Context)
  const repository = new ApiAuthRepository();
  const loginUseCase = new LoginUseCase(repository);
  const registerUseCase = new RegisterUseCase(repository);

  const handleLogin = async (email: string, password: string) => {
    setLoading(true);
    setError(null);
    try {
      const user = await loginUseCase.execute(email, password);
      console.log("Sukces!", user);
      // Tutaj np. zapisz token w localStorage lub stanie globalnym (Zustand/Redux)
    } catch (err: unknown) {
      const errorMessage = err instanceof Error ? err.message : 'Unknonwn error'
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };


  const handleRegister = async (data : RegisterRequest) => {
      setLoading(true);
      setError(null);

      try{
          const response = await registerUseCase.execute(data);

          console.log("Zarejestrowano pomyślnie", response);
      }catch(err:unknown){

          const errorMessage = err instanceof Error ? err.message : 'Unknown error'
          setError(errorMessage);

          setLoading(false);
          

      }finally{
          setLoading(false);
      }
  }


  return { handleLogin, handleRegister, loading, error };
};