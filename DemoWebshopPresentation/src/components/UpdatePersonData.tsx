import React, { FormEventHandler, useEffect } from 'react'
import { useState } from "react"
import { Buffer } from 'buffer';
import ClaimTypes from '../enums/ClaimTypes'

type Props = {
    token: string
}

const UpdatePersonData = ({ token }: Props) => {
    const [username, setUsername] = useState('')
    const [email, setEmail] = useState('')
    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')

    useEffect(() => {
        const tokenData = JSON.parse(Buffer.from(token.split('.')[1], 'base64').toString())
        const userId = tokenData[ClaimTypes.UserId]
        fetch(
            `https://localhost:7000/api/User/${userId}`, {
                method: 'GET',
                headers: {
                  'Authorization': `Bearer ${token}`
                }
            })
            .then(response => handleGetUserResponse(response))
        }, []
    )

    const handleGetUserResponse = async (response: Response) => {
        if (response.ok) {
            const data = await response.json()
            setUsername(data.username)
            setEmail(data.email)
            setFirstName(data.firstName)
            setLastName(data.lastName)
        }
        else {
          alert(`Couldn't load your user data!`)
        }
    }
    
    const onSubmitPersonalInfo: FormEventHandler<HTMLFormElement> = (e) => {
        e.preventDefault()
    
        if (!username) {
            alert('Please type in username!')
            return
        }
    
        if (!email) {
            alert('Please type in email!')
            return
        }
    
        updateProfile({ username, email, firstName, lastName })
    }

    const updateProfile = async (userInput: UpdateProfileInput) => {
        const res = await fetch('https://localhost:7000/api/User', {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(userInput)
        })
        
        if (res.ok) {
            alert(`User updated!`)
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await res.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Updating profile failed for user ${userInput.username}: ${errorMessage}`)
        }
    }

    return (
        <div>
            <form className="add-form"
                onSubmit={onSubmitPersonalInfo}>
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
                <input className="btn btn-dark" type='submit' value='Update' />
            </form>
        </div>
      )
}

export default UpdatePersonData