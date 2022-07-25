import React, { MouseEventHandler, useEffect, useState } from 'react'
import { Confirm } from 'react-admin'
import Button from './Button'
import UserRow from './UserRow'

type Props = {
  token: string
  loggedUserId: string
  onGoBackClick: MouseEventHandler
}

type UserInfo = {
  id: string
  username: string
  email: string
  firstName: string
  lastName: string
  isAdmin: boolean
}

export const UserList = ({ token, loggedUserId, onGoBackClick }: Props) => {
  const [users, setUsers] = useState<UserInfo[]>([])
  const [open, setOpen] = useState(false);
  const [deletedUserId, setDeletedUserId] = useState('');

  useEffect(() => {
    fetch('https://localhost:7000/api/User', {
            method: 'GET',
            headers: {
              'Authorization': `Bearer ${token}`
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
    const res = await fetch(`https://localhost:7000/api/User/${userId}/SetUserAdmin`, {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
        
        if (res.ok) {
            alert(`User updated!`)
            setUsers(users.map((user) =>
              user.id === userId ? { ...user, isAdmin: true } : user
            ))
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await res.text()
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
            'Authorization': `Bearer ${token}`
        }
    })

    if (response.ok) {
      deleteUserById(deletedUserId)
    }
    else {
      alert(`Couldn't delete product!`)
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
                  <UserRow key={user.id} loggedUserId={loggedUserId} user={user} onSetAdmin={onSetAdmin} onDeleteUser={openConfirmDialog} />
              ))
            }
          </tbody>
        </table>

        <Button className="btn btn-dark" text="Go Back" onClick={onGoBackClick} />
      </div>
    </>
  )
}
