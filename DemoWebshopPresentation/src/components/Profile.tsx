import React from 'react'
import { NavigateFunction } from 'react-router-dom'
import Button from './Button'
import UpdatePassword from './UpdatePassword';
import UpdatePersonData from './UpdatePersonData';

type Props = {
    navigate: NavigateFunction,
    token: string
}

const Profile = ({ navigate, token }: Props) => {
  return (
    <div>
        <UpdatePersonData token={token} />
        <UpdatePassword token={token} />
        <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
    </div>
  )
}

export default Profile

