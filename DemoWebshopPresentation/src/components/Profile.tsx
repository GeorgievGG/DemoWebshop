import React, { FormEventHandler, useEffect } from 'react'
import { useState } from "react"
import { NavigateFunction } from 'react-router-dom'
import { Buffer } from 'buffer';
import ClaimTypes from '../enums/ClaimTypes'
import Button from './Button'
import UpdatePassword from './UpdatePassword';
import UpdatePersonData from './UpdatePersonData';

type Props = {
    navigate: NavigateFunction
}

const Profile = ({ navigate }: Props) => {
  const token = localStorage.getItem("access_token")!

  return (
    <div>
        <UpdatePersonData token={token} />
        <UpdatePassword token={token} />
        <Button text="Go Back" onClick={() => navigate(-1)} />
    </div>
  )
}

export default Profile

