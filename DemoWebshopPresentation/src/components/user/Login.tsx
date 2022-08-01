import React, { FormEventHandler, useState } from 'react'
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom'
import ClaimTypes from '../../enums/ClaimTypes';
import { IUserLoginInput } from '../../pages/LoginPage/types';
import { setSessionData } from '../../store/sessionSlice';
import Button from '../common/Button'

const Login = () => {
  const navigate = useNavigate()
  const dispatch = useDispatch()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')

  const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
    e.preventDefault()

    if (!username) {
        alert('Please type in username!')
        return
    }
    
    if (!password) {
        alert('Please type in password!')
        return
    }

    login({ username, password })
  }

  const login = async (userCredentials: IUserLoginInput) => {
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
      dispatch(setSessionData({Token: data.access_token, UserLogged: true, LoggedUserId: tokenData[ClaimTypes.UserId], LoggedUserRole: tokenData[ClaimTypes.UserRole]}))
      navigate(-1)
    }
    else {
      alert(`Login for user ${userCredentials.username} failed.`)
    }
  }

  return (
    <div>
      <form className="add-form"
            onSubmit={onSubmit}>
        <div className="form-control border-0">
            <label>Username</label>
            <input type='text' 
                  placeholder='Type username'
                  value={username}
                  onChange={(e) => setUsername(e.target.value)} />
        </div>
        <div className="form-control border-0">
            <label>Password</label>
            <input type='password' 
                  value={password}
                  onChange={(e) => setPassword(e.target.value)} />
        </div>
        <input className="btn btn-dark" type='submit' value='Login' />
        <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
      </form>
    </div>
  )
}

export default Login