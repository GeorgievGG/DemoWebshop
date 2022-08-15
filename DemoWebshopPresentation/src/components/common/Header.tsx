import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { selectProductsState, selectSessionState } from '../../store'
import { flushPaymentState } from '../../store/paymentSlice'
import { setProducts } from '../../store/productsSlice'
import { flushSessionData } from '../../store/sessionSlice'
import { IUserSessionData, RootState } from '../../store/types'
import Button from './Button'

const Header = () => {
    const navigate = useNavigate()
    const dispatch = useDispatch()
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    const products = useSelector<RootState, CatalogProductInfo[]>(selectProductsState)

    const logout = async () => {
        dispatch(flushSessionData())
        dispatch(flushPaymentState())
        dispatch(setProducts(products.filter((product) => product.availableQuantity !== 0)))
        navigate("/")
    }

    return (
        <header>
            <ul className='horizontal'>
                <li>
                    <img className='logo' src="../images/logo.png" alt="Failed to load logo" onClick={() => navigate("/")} />
                </li>
                <li>
                    { !sessionState.UserLogged && <Button className="btn btn-dark" text={"Login"} onClick={() => navigate("/login")} /> }
                </li>
                <li>
                    { !sessionState.UserLogged && <Button className="btn btn-dark" text={"Register"} onClick={() => navigate("/register")} /> }
                </li>
                <li>
                    { sessionState.LoggedUserRole === "Admin" && <Button className="btn btn-dark" text={"Create Product"} onClick={() => navigate("/createProduct")} /> }
                </li>
                <li>
                    { sessionState.LoggedUserRole === "Admin" && <Button className="btn btn-dark" text={"Users"} onClick={() => navigate("/userList")} /> }
                </li>
                <li>
                    { sessionState.LoggedUserRole === "Admin" && <Button className="btn btn-dark" text={"Orders"} onClick={() => navigate("/orderList")} /> }
                </li>
                <li>
                    { sessionState.UserLogged && <Button className="btn btn-dark" text={"Profile"} onClick={() => navigate("/profile")} /> }
                </li>
                <li>
                    { sessionState.UserLogged && sessionState.LoggedUserRole === "User" && <Button className="btn btn-dark" text={"Basket"} onClick={() => navigate("/shoppingBasket")} /> }
                </li>
                <li>
                    { sessionState.UserLogged && <Button className="btn btn-dark" text={"Logout"} onClick={logout} /> }
                </li>
            </ul>
        </header>
    )
}

export default Header