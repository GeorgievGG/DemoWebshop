import React, { useEffect, useState } from 'react'
import { Confirm } from 'react-admin'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { selectSessionState } from '../../store'
import { IUserSessionData, RootState } from '../../store/types'
import Button from '../common/Button'
import UserRow from './UserRow'

export const UserList = () => {
  const [users, setUsers] = useState<UserInfo[]>([])
  const [open, setOpen] = useState(false);
  const [deletedUserId, setDeletedUserId] = useState('');
  const navigate = useNavigate()
  const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)

  useEffect(() => {
    fetch('https://localhost:7000/api/User', {
            method: 'GET',
            headers: {
              'Authorization': `Bearer ${sessionState.Token}`
            }
        })
        .then(response => handleGetUsersResponse(response))
    }, []
  )
  
  const handleGetUsersResponse = async (response: Response) => {
    if (response.ok) {
        const data = await response.json()
        setUsers(data)
    }
    else {
      alert(`Couldn't retrieve users!`)
    }
  }

  const onSetAdmin = async (userId: string) => {
    const response = await fetch(`https://localhost:7000/api/User/${userId}/SetUserAdmin`, {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            }
        })
        
        if (response.ok) {
            alert(`User updated!`)
            setUsers(users.map((user) =>
              user.id === userId ? { ...user, isAdmin: true } : user
            ))
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await response.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Updating user failed: ${errorMessage}`)
        }
  }

  const openConfirmDialog = (userId: string) => { 
      setOpen(true)
      setDeletedUserId(userId);
  }

  const handleConfirm = async () => {
    const response = await fetch(`https://localhost:7000/api/User/${deletedUserId}`, {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json',
            'Authorization': `Bearer ${sessionState.Token}`
        }
    })

    if (response.ok) {
      deleteUserById(deletedUserId)
    }
    else {
      const body = await response.text()
      const bodyJson = JSON.parse(body)
      alert(`Couldn't delete user: ${bodyJson.message}!`)
    }

    setOpen(false);
    setDeletedUserId('');
  }
  
  const deleteUserById = (userId: string) => {
    setUsers(users.filter((user) => user.id !== userId))
  }

  const handleDialogClose = () => { 
    setOpen(false)
    setDeletedUserId('')
  }

  return (
    <>
      <Confirm
          isOpen={open}
          title="Delete product"
          content="Are you sure you want to delete this item?"
          onConfirm={handleConfirm}
          onClose={handleDialogClose}
          confirm="Yes"
          cancel="No"
      />
      <div>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Username</th>
              <th scope="col">Email</th>
              <th scope="col">First Name</th>
              <th scope="col">Last Name</th>
              <th scope="col">IsAdmin</th>
              <th scope="col"></th>
              <th scope="col"></th>
            </tr>
          </thead>
          <tbody>
            {
              users.map((user) => (
                  <UserRow key={user.id} loggedUserId={sessionState.LoggedUserId} user={user} onSetAdmin={onSetAdmin} onDeleteUser={openConfirmDialog} />
              ))
            }
          </tbody>
        </table>

        <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
      </div>
    </>
  )
}

export default UserList