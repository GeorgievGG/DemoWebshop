import React from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { selectSessionState } from '../../store'
import { IUserSessionData, RootState } from '../../store/types'
import Button from '../common/Button'
import UpdatePassword from './UpdatePassword'
import UpdatePersonData from './UpdatePersonData'

const Profile = () => {
  const navigate = useNavigate()
  const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
  
  return (
    <div>
        <UpdatePersonData token={sessionState.Token} />
        <UpdatePassword token={sessionState.Token} />
        <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
    </div>
  )
}

export default Profile

