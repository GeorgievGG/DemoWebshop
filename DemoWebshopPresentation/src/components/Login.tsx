import React, { MouseEventHandler, FormEventHandler } from 'react'
import { useState } from "react"
import Button from './Button'

type Props = {
  onLogin: Function,
  onGoBackClick: MouseEventHandler
}

const Login = ({onLogin, onGoBackClick}: Props) => {
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

    onLogin({ username, password })
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
      </form>
      <Button text="Go Back" onClick={onGoBackClick} />
    </div>
  )
}

export default Login