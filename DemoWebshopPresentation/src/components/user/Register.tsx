import React, { FormEventHandler } from 'react'
import { useState } from "react"
import { useNavigate } from 'react-router-dom'
import { IRegistrationInput } from '../../pages/RegisterPage/types'
import Button from '../common/Button'

const Register = () => {
  const navigate = useNavigate();
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

    register({ username, email, firstName, lastName, password, confirmPassword })
  }

  const register = async (userInput: IRegistrationInput) => {
    const response = await fetch('https://localhost:7000/api/User', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(userInput)
    })
    
    const body = await response.text()
    if (response.ok) {
      const data = JSON.parse(body)
      alert(`User ${data.username} registered!`)
      navigate(-1)
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
            <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
        </form>
    </div>
  )
}

export default Register

