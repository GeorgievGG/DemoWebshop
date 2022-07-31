import React from 'react'
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import Login from '../../components/Login'
import ClaimTypes from '../../enums/ClaimTypes';
import { setState } from '../../store';
import { Buffer } from 'buffer';

const LoginPage = () => {
  const navigate = useNavigate()
  const dispatch = useDispatch()
  const login = async (userCredentials: UserCreds) => {
    const response = await fetch('https://localhost:7000/api/Authentication/Login', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(userCredentials)
    })

    if (response.ok) {
      const data = await response.json()
      const tokenData = JSON.parse(Buffer.from(data.access_token.split('.')[1], 'base64').toString())
      dispatch(setState({Token: data.access_token, UserLogged: true, LoggedUserId: tokenData[ClaimTypes.UserId], LoggedUserRole: tokenData[ClaimTypes.UserRole]}))
      navigate(-1)
    }
    else {
      alert(`Login for user ${userCredentials.username} failed.`)
    }
  }

  return (
    <Login onLogin={login} />
  )
}

export default LoginPage