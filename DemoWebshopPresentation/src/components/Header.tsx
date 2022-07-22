import React from 'react'
import { MouseEventHandler } from 'react';
import { NavigateFunction } from 'react-router-dom';
import Button from "./Button";

type Props = {
    title: string
    userLogged: boolean
    userRole: string
    navigate: NavigateFunction
    onLogoutClick: MouseEventHandler
}

const Header = ({ title, userLogged, userRole, navigate, onLogoutClick }: Props) => {
    return (
        <header>
            <ul className='horizontal'>
                <li>
                    <h1>{title}</h1>
                </li>
                <li>
                    { !userLogged && <Button className="btn btn-dark" text={"Login"} onClick={() => navigate("/login")} /> }
                </li>
                <li>
                    { !userLogged && <Button className="btn btn-dark" text={"Register"} onClick={() => navigate("/register")} /> }
                </li>
                <li>
                    { userRole === "Admin" && <Button className="btn btn-dark" text={"Create Product"} onClick={() => navigate("/createProduct")} /> }
                </li>
                <li>
                    { userLogged && <Button className="btn btn-dark" text={"Profile"} onClick={() => navigate("/profile")} /> }
                </li>
                <li>
                    { userLogged && <Button className="btn btn-dark" text={"Logout"} onClick={onLogoutClick} /> }
                </li>
            </ul>
        </header>
    )
}

export default Header