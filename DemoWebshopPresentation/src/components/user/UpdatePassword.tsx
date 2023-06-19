import React, { FormEventHandler } from 'react'
import { useState } from "react"
import { toast } from 'react-toastify'
import { handleNegativeResponse } from '../../utility'

type Props = {
    token: string
}

const Profile = ({ token }: Props) => {
    const [currentPassword, setCurrentPassword] = useState('')
    const [newPassword, setNewPassword] = useState('')
    const [repeatNewPassword, setRepeatNewPassword] = useState('')

    const onSubmitPasswords: FormEventHandler<HTMLFormElement> = (e) => {
        e.preventDefault()
    
        if (!currentPassword) {
            toast.error('Current password missing!')
            return
        }
    
        if (!newPassword) {
            toast.error('New password missing!')
            return
        }
    
        if (!repeatNewPassword) {
            toast.error('Confirm password missing!')
            return
        }
    
        updatePasswords({ currentPassword, newPassword, repeatNewPassword })
    }

    const updatePasswords = async (userInput: UpdatePasswordInput) => {
        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/User/UpdatePassword`, {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(userInput)
        })
        
        if (response.ok) {
            toast.success(`Password updated!`)
        }
        else {
            handleNegativeResponse(response, "Updating password failed", null, true)
        }
    }

    return (
        <div>
            <form className="add-form"
                onSubmit={onSubmitPasswords}>
                <div className="form-control border-0">
                    <label>Current Password</label>
                    <input type='password' 
                        value={currentPassword}
                        onChange={(e) => setCurrentPassword(e.target.value)} />
                </div>
                <div className="form-control border-0">
                    <label>New Password</label>
                    <input type='password' 
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)} />
                </div>
                <div className="form-control border-0">
                    <label>Confirm Password</label>
                    <input type='password' 
                        value={repeatNewPassword}
                        onChange={(e) => setRepeatNewPassword(e.target.value)} />
                </div>
                <input className="btn btn-dark" type='submit' value='Update Password' />
            </form>
        </div>
      )
    }
    
    export default Profile