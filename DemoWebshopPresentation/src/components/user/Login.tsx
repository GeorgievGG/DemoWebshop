import React, { FormEventHandler, useState } from 'react'
import { useDispatch } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import ClaimTypes from '../../enums/ClaimTypes'
import { IUserLoginInput } from '../../pages/LoginPage/types'
import { setSessionData } from '../../store/sessionSlice'
import Button from '../common/Button'
import { Buffer } from 'buffer'
import { toast } from 'react-toastify'
import { handleNegativeResponse } from '../../utility'

const Login = () => {
  const navigate = useNavigate()
  const dispatch = useDispatch()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')

  const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
    e.preventDefault()

    if (!username) {
        toast.error('Please type in username!')
        return
    }
    
    if (!password) {
        toast.error('Please type in password!')
        return
    }

    login({ username, password })
  }

  const login = async (userCredentials: IUserLoginInput) => {
    const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/Authentication/Login`, {
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
      handleNegativeResponse(response, `Login for user ${userCredentials.username} failed.`, null, false)
    }
  }

  // TODO: use refresh token
  // const refreshToken = async (refreshToken: string) => {
  //   const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/Authentication/RefreshToken`, {
  //     method: 'POST',
  //     headers: {
  //       'Content-type': 'application/json'
  //     },
  //     body: JSON.stringify(refreshToken)
  //   })

  //   if (response.ok) {
  //     const data = await response.json()
  //   }
  //   else {
  //     toast.error(`Refreshing token failed. You're being logged out!`)
  //     logout()
  //   }
  // }

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