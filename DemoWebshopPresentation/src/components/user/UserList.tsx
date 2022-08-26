import React, { useEffect, useState } from 'react'
import { Confirm } from 'react-admin'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { selectSessionState } from '../../store'
import { IUserSessionData, RootState } from '../../store/types'
import { handleNegativeResponse } from '../../utility'
import Button from '../common/Button'
import UserRow from './UserRow'
import RiseLoader from "react-spinners/RiseLoader";

export const UserList = () => {
  const [users, setUsers] = useState<UserInfo[]>([])
  const [hasLoaded, setHasLoaded] = useState(false)
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
        setHasLoaded(true)
    }
    else {
      handleNegativeResponse(response, "Couldn't retrieve users!", null, false)
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
            toast.success(`User updated!`)
            setUsers(users.map((user) =>
              user.id === userId ? { ...user, isAdmin: true } : user
            ))
        }
        else {
          handleNegativeResponse(response, "Updating user failed", null, true)
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
      handleNegativeResponse(response, "Couldn't delete user", null, true)
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
      { 
        hasLoaded ?
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
        </div> :
        <RiseLoader className='loader' color={'black'} size={15} /> 
      }
    </>
  )
}

export default UserList