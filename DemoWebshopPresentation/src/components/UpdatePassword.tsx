import React, { FormEventHandler } from 'react'
import { useState } from "react"

type Props = {
    token: string
}

type UpdatePasswordInput = {
    currentPassword: string
    newPassword: string
    repeatNewPassword: string
}

const Profile = ({ token }: Props) => {
    const [currentPassword, setCurrentPassword] = useState('')
    const [newPassword, setNewPassword] = useState('')
    const [repeatNewPassword, setRepeatNewPassword] = useState('')

    const onSubmitPasswords: FormEventHandler<HTMLFormElement> = (e) => {
        e.preventDefault()
    
        if (!currentPassword) {
            alert('Current password missing!')
            return
        }
    
        if (!newPassword) {
            alert('New password missing!')
            return
        }
    
        if (!repeatNewPassword) {
            alert('Confirm password missing!')
            return
        }
    
        updatePasswords({ currentPassword, newPassword, repeatNewPassword })
    }

    const updatePasswords = async (userInput: UpdatePasswordInput) => {
        const res = await fetch('https://localhost:7000/api/User/UpdatePassword', {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(userInput)
        })
        
        if (res.ok) {
            alert(`Password updated!`)
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await res.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Updating password failed: ${errorMessage}`)
        }
    }

    return (
        <div>
            <form className="add-form"
                onSubmit={onSubmitPasswords}>
                <div className="form-control">
                    <label>Current Password</label>
                    <input type='password' 
                        value={currentPassword}
                        onChange={(e) => setCurrentPassword(e.target.value)} />
                </div>
                <div className="form-control">
                    <label>New Password</label>
                    <input type='password' 
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)} />
                </div>
                <div className="form-control">
                    <label>Confirm Password</label>
                    <input type='password' 
                        value={repeatNewPassword}
                        onChange={(e) => setRepeatNewPassword(e.target.value)} />
                </div>
                <input className="btn btn-block" type='submit' value='Update Password' />
            </form>
        </div>
      )
    }
    
    export default Profile