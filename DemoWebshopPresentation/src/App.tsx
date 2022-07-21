import React from 'react'
import { useState } from "react";
import { Routes, Route, useNavigate } from "react-router-dom";
import Header from "./components/Header";
import Footer from "./components/Footer";
import About from "./components/About";
import Login from "./components/Login";
import Register from "./components/Register";
import Profile from "./components/Profile";
import Catalog from "./components/Catalog";
import useScript from './hooks/UseScript';
import "bootstrap/dist/css/bootstrap.css";

type UserCreds = {
  username: string,
  password: string
}

type RegistrationInput = {
  username: string
  email: string
  firstName: string
  lastName: string
  password: string
  confirmPassword: string
}

function App() {
  useScript('https://unpkg.com/react-dom/umd/react-dom.production.min.js');
  useScript('https://unpkg.com/react/umd/react.production.min.js');
  useScript('https://unpkg.com/react-bootstrap@next/dist/react-bootstrap.min.js');

  const navigate = useNavigate();
  const [showAboutLink, setShowAboutLink] = useState(true)
  const [userLogged, setUserLogged] = useState(false)
  const [token, setToken] = useState('')

  const navigateBack = () => {
    navigate(-1)
  }

  const login = async (userCredentials: UserCreds) => {
    const res = await fetch('https://localhost:7000/api/Authentication/Login', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(userCredentials)
    })

    if (res.ok) {
      const data = await res.json()
      setUserLogged(true)
      setToken(data.access_token)
      navigateBack()
    }
    else {
      alert(`Login for user ${userCredentials.username} failed.`)
    }
  }

  const refreshToken = async (refreshToken: string) => {
    const res = await fetch('https://localhost:7000/api/Authentication/RefreshToken', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(refreshToken)
    })

    if (res.ok) {
      const data = await res.json()
    }
    else {
      alert(`Refreshing token failed. You're being logged out!`)
      logout()
    }
  }

  const register = async (userInput: RegistrationInput) => {
    const res = await fetch('https://localhost:7000/api/User', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(userInput)
    })
    
    const body = await res.text()
    if (res.ok) {
      const data = JSON.parse(body)
      alert(`User ${data.username} registered!`)
      navigateBack()
    }
    else {
      let errorMessage = 'Unknown error'
      if (body && body !== '') {
        const data = JSON.parse(body)
        errorMessage = data.message
      }
      alert(`Registration failed for user ${userInput.username}: ${errorMessage}`)
    }
  }

  const logout = async () => {
    setToken('')
    setUserLogged(false)
    navigate("/")
  }

  const toggleAboutLinkStatus = () => {
    setShowAboutLink(!showAboutLink)
  }

  return (
      <div className="container">
        <Header title='Hello from Demo Webshop!' 
                userLogged={userLogged} 
                navigate={navigate}
                onLogoutClick={logout} />
        
        <Routes>
          <Route path='/' element={
              <Catalog token={token} />
            } />
          <Route path='/login' element=
            {
              <Login onLogin={login} onGoBackClick={navigateBack} />
            } />
          <Route path='/register' element=
            {
              <Register onRegister={register} onGoBackClick={navigateBack} />
            } />
          <Route path='/profile' element=
            {
              <Profile navigate={navigate} token={token} />
            } />
          <Route path='/about' element=
            {
              <About onGoBackClick={toggleAboutLinkStatus}/>
            } />
        </Routes>
        <Footer onAboutClick={toggleAboutLinkStatus} showAboutLink={ showAboutLink } />
      </div>
  );
}

export default App;
