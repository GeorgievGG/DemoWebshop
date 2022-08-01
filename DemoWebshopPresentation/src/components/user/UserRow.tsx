import React from 'react'
import Button from '../common/Button'

type Props = {
    user: UserInfo
    loggedUserId: string
    onSetAdmin: (userId: string) => void
    onDeleteUser: (userId: string) => void
}

const UserRow = ({ user, loggedUserId, onSetAdmin, onDeleteUser }: Props) => {
    return (
        <tr>
            <td>{user.username}</td>
            <td>{user.email}</td>
            <td>{user.firstName}</td>
            <td>{user.lastName}</td>
            <td>{user.isAdmin ? 'Yes' : 'No' }</td>
            <td>
                {
                    !user.isAdmin && <Button className="btn btn-dark" text="Make Admin" onClick={() => onSetAdmin(user.id)} />
                }
            </td>
            <td>
                {
                    user.id !== loggedUserId && <Button className="btn btn-dark" text="Delete" onClick={() => onDeleteUser(user.id)} />
                }
            </td>
        </tr>
    )
}

export default UserRow