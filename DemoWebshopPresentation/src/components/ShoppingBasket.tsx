import React, { MouseEventHandler, useState } from 'react'
import Button from './Button'


type Props = {
    token: string
    loggedUserId: string
    onGoBackClick: MouseEventHandler
}

type ShoppingBasket = {
    id: string
    basketLines: ShoppingBasketLine[]
}

type ShoppingBasketLine = {
    quantity: number
    productId: string
}

const ShoppingBasket = ({ token, loggedUserId, onGoBackClick }: Props) => {
    const [shoppingBasket, setShoppingBasket] = useState<ShoppingBasket>()


    return (
        <div>
            <table className="table">
            <thead>
                <tr>
                    <th scope="col">Product</th>
                    <th scope="col">Purchase quantity</th>
                    <th scope="col">Price per unit</th>
                    <th scope="col">Price for all</th>
                </tr>
            </thead>
            <tbody>
                {
                    // shoppingBasket.basketLines.map((user) => (
                    //     <ShoppingBasketLine key={user.id} loggedloggedUserId={loggedloggedUserId} user={user} onSetAdmin={onSetAdmin} onDeleteUser={openConfirmDialog} />
                    // ))
                }
            </tbody>
            </table>
    
            <Button className="btn btn-dark" text="Go Back" onClick={onGoBackClick} />
        </div>
      )
}

export default ShoppingBasket