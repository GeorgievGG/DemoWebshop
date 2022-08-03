import React from 'react'
import Button from '../common/Button'

type Props = {
    basketLine: ShoppingBasketLine
    onMinusClicked: (productId: string) => void
    onQuantityChanged: (productId: string, purchaseQty: number) => void
    onPlusClicked: (productId: string) => void
}

type ShoppingBasketLine = {
    quantity: number
    product: BasketProductInfo
}

const ShoppingBasketRow = ({ basketLine, onMinusClicked, onQuantityChanged, onPlusClicked }: Props) => {
    return (
        <tr>
            <td>{basketLine.product.name}</td>
            <td>
                {
                    <div>
                        <Button className="btn btn-dark" text="-" onClick={() => onMinusClicked(basketLine.product.id)} />
                        <input type='number' 
                            value={basketLine.quantity}
                            onChange={(e) => onQuantityChanged(basketLine.product.id, e.target.valueAsNumber)} />
                        <Button className="btn btn-dark" text="+" onClick={() => onPlusClicked(basketLine.product.id)} />
                    </div>
                }
            </td>
            <td>{basketLine.product.price.toLocaleString('de-DE', { style: 'currency', currency: 'EUR'})}</td>
            <td>{(basketLine.quantity * basketLine.product.price).toLocaleString('de-DE', { style: 'currency', currency: 'EUR'})}</td>
        </tr>
    )
}

export default ShoppingBasketRow