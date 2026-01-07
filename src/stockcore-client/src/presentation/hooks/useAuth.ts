// presentation/hooks/useAuth.ts
import { useState } from 'react';
import { ApiAuthRepository } from '../../data/repositories/ApiAuthRepository';
import { LoginUseCase } from '../../application/use-cases/LoginUseCase';

export const useAuth = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Inicjalizacja (w prawdziwym prod. użyłbyś Dependency Injection / Context)
  const repository = new ApiAuthRepository();
  const loginUseCase = new LoginUseCase(repository);

  const handleLogin = async (email: string, password: string) => {
    setLoading(true);
    setError(null);
    try {
      const user = await loginUseCase.execute(email, password);
      console.log("Sukces!", user);
      // Tutaj np. zapisz token w localStorage lub stanie globalnym (Zustand/Redux)
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return { handleLogin, loading, error };
};