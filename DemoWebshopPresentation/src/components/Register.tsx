import React, { MouseEventHandler, FormEventHandler } from 'react'
import { useState } from "react"
import Button from './Button'

type Props = {
  onRegister: Function,
  onGoBackClick: MouseEventHandler
}

const Register = ({onRegister, onGoBackClick}: Props) => {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')

  const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
    e.preventDefault()

    if (!username) {
        alert('Please type in username!')
        return
    }

    if (!email) {
        alert('Please type in username!')
        return
    }
    
    if (!password) {
        alert('Please type in password!')
        return
    }
    
    if (!confirmPassword) {
        alert('Please type in password!')
        return
    }

    onRegister({ username, email, firstName, lastName, password, confirmPassword })
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
                <label>Email</label>
                <input type='text' 
                    placeholder='Type email'
                    value={email}
                    onChange={(e) => setEmail(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>First Name</label>
                <input type='text' 
                    placeholder='Type first name'
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Last Name</label>
                <input type='text' 
                    placeholder='Type last name'
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Password</label>
                <input type='password' 
                    value={password}
                    onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Confirm Password</label>
                <input type='password' 
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)} />
            </div>
            <input className="btn btn-dark" type='submit' value='Register' />
        </form>
        <Button className="btn btn-dark" text="Go Back" onClick={onGoBackClick} />
    </div>
  )
}

export default Register

