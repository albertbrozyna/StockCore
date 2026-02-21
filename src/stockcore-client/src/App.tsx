import { useState } from 'react'
import { Routes,Route } from 'react-router-dom';
import './App.css'
import LoginPage from './presentation/pages/LoginPage'
import RegisterPage from './presentation/pages/RegisterPage';
function App() {

  return (
    <div className="App">
      <Routes> 
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/" element={<H}


      </Routes>
    </div>
  )
}

export default App
