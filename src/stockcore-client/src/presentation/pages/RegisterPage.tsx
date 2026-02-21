// presentation/pages/RegisterPage.tsx
import React, { useState } from 'react';
import { Link } from 'react-router-dom'; // Załóżmy, że masz react-router-dom
import { useAuth } from '../hooks/useAuth';

const RegisterPage = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [agreeToTerms, setAgreeToTerms] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { handleRegister } = useAuth();

  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    if (password !== confirmPassword) {
      setError("Hasła nie pasują do siebie.");
      setLoading(false);
      return;
    }
    if (!agreeToTerms) {
      setError("Musisz zaakceptować regulamin.");
      setLoading(false);
      return;
    }
    
    // Tutaj normalnie wywołałbyś useRegister z warstwy hooks/useAuth
   

    await handleRegister({
      email,
      firstName,
      lastName,
      userRole: 1, // Przykładowa rola użytkownika
      password,
      phoneNumber: "",
      companyJoinCode: ""
    });

    
    // Symulacja API
    setTimeout(() => {
      setLoading(false);
      //setError("Wystąpił błąd podczas rejestracji.");
      alert("Rejestracja zakończona sukcesem! Sprawdź email.");
      // Możesz przekierować użytkownika na stronę logowania
      // navigate('/login');
    }, 2000);
  };

  return (
    <div className="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8 relative z-0">
      {/* Warstwa dla efektu cząsteczek, jeśli używasz */}
      <div className="background-particles"></div> 

      <div className="max-w-xl w-full space-y-8 bg-gray-800 bg-opacity-80 backdrop-filter backdrop-blur-md p-10 rounded-2xl shadow-2xl border border-gray-700 relative z-10">
        <div>
          <h2 className="mt-6 text-center text-4xl font-extrabold text-white">
            Dołącz do <span className="text-blue-400">STOCKCORE</span> 📈
          </h2>
          <p className="mt-2 text-center text-lg text-gray-400">
            Stwórz swoje konto inwestora
          </p>
        </div>
        
        <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
          {error && (
            <div className="bg-red-900 bg-opacity-50 border border-red-700 text-red-300 px-4 py-3 rounded-lg text-sm" role="alert">
              {error}
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label htmlFor="first-name" className="sr-only">Imię</label>
              <input
                id="first-name"
                name="firstName"
                type="text"
                required
                className="appearance-none relative block w-full px-4 py-3 border border-gray-700 placeholder-gray-500 text-white bg-gray-900 rounded-lg focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm transition-colors"
                placeholder="Imię"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
              />
            </div>
            <div>
              <label htmlFor="last-name" className="sr-only">Nazwisko</label>
              <input
                id="last-name"
                name="lastName"
                type="text"
                required
                className="appearance-none relative block w-full px-4 py-3 border border-gray-700 placeholder-gray-500 text-white bg-gray-900 rounded-lg focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm transition-colors"
                placeholder="Nazwisko"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
              />
            </div>
          </div>

          <div>
            <label htmlFor="email-address" className="sr-only">Adres email</label>
            <input
              id="email-address"
              name="email"
              type="email"
              autoComplete="email"
              required
              className="appearance-none relative block w-full px-4 py-3 border border-gray-700 placeholder-gray-500 text-white bg-gray-900 rounded-lg focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm transition-colors"
              placeholder="Adres email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>

          <div>
            <label htmlFor="password" className="sr-only">Hasło</label>
            <input
              id="password"
              name="password"
              type="password"
              autoComplete="new-password"
              required
              className="appearance-none relative block w-full px-4 py-3 border border-gray-700 placeholder-gray-500 text-white bg-gray-900 rounded-lg focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm transition-colors"
              placeholder="Hasło"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          
          <div>
            <label htmlFor="confirm-password" className="sr-only">Potwierdź hasło</label>
            <input
              id="confirm-password"
              name="confirmPassword"
              type="password"
              autoComplete="new-password"
              required
              className="appearance-none relative block w-full px-4 py-3 border border-gray-700 placeholder-gray-500 text-white bg-gray-900 rounded-lg focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm transition-colors"
              placeholder="Potwierdź hasło"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />
          </div>

          <div className="flex items-center">
            <input
              id="agree-to-terms"
              name="agree-to-terms"
              type="checkbox"
              className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-600 rounded bg-gray-700"
              checked={agreeToTerms}
              onChange={(e) => setAgreeToTerms(e.target.checked)}
            />
            <label htmlFor="agree-to-terms" className="ml-2 block text-sm text-gray-300">
              Akceptuję <a href="#" className="font-medium text-blue-400 hover:text-blue-300">regulamin</a>
            </label>
          </div>

          <button
            type="submit"
            disabled={loading}
            className="group relative w-full flex justify-center py-3 px-4 border border-transparent text-lg font-semibold rounded-xl text-white bg-gradient-to-r from-blue-600 to-indigo-700 hover:from-blue-700 hover:to-indigo-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-all shadow-lg hover:shadow-xl disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {loading ? 'Rejestruję...' : 'Zarejestruj się'}
          </button>
        </form>

        <div className="mt-6 text-center text-sm text-gray-400">
          Masz już konto? {' '}
          <Link to="/login" className="font-medium text-blue-400 hover:text-blue-300">
            Zaloguj się
          </Link>
        </div>
      </div>
    </div>
  );
};

export default RegisterPage;