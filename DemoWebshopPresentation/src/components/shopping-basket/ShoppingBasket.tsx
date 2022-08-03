import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { selectSessionState } from '../../store'
import { IUserSessionData, RootState } from '../../store/types'
import Button from '../common/Button'
import ShoppingBasketRow from './ShoppingBasketRow'

type Basket = {
    id: string
    basketLines: ShoppingBasketLine[]
}

type ShoppingBasketLine = {
    quantity: number
    product: BasketProductInfo
}

const ShoppingBasket = () => {
    const [shoppingBasket, setShoppingBasket] = useState<Basket>({id: '', basketLines: []})
    const [hasLoaded, setHasLoaded] = useState(false)
    
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    const navigate = useNavigate()

    useEffect(() => {
        fetch('https://localhost:7000/api/ShoppingBasket', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${sessionState.Token}`
                }
            })
            .then(response => handleGetShoppingBasketResponse(response))
        }, []
    )

    const handleGetShoppingBasketResponse = async (response: Response) => {
        if (response.ok) {
            const data = await response.json()
            setShoppingBasket(data)
            setHasLoaded(true)
        }
        else {
          alert(`Couldn't retrieve products!`)
        }
    }

    const increaseShoppingQuantity = async (productId: string) => {
        const response = await fetch(`https://localhost:7000/api/ShoppingBasket/IncreaseShoppingQuantity/${productId}`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            }
        })
        
        if (response.ok) {
            setShoppingBasket({
                ...shoppingBasket, 
                basketLines: shoppingBasket.basketLines.map(
                    (line) => line.product.id === productId ? 
                    { ...line, quantity: ++line.quantity} : 
                    line
                )
            })
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await response.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Adding to cart failed: ${errorMessage}`)
        }
    }
    
    const decreaseShoppingQuantity = async (productId: string) => {
        const response = await fetch(`https://localhost:7000/api/ShoppingBasket/DecreaseShoppingQuantity/${productId}`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            }
        })
        
        if (response.ok) {
            setShoppingBasket({
                ...shoppingBasket, 
                basketLines: shoppingBasket.basketLines.map(
                    (line) => line.product.id === productId ? 
                    { ...line, quantity: --line.quantity} : 
                    line
                ).filter((line) => line.quantity > 0)
            })
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await response.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Adding to cart failed: ${errorMessage}`)
        }
    }
    
    const setShoppingQuantity = async (productId: string, purchaseQty: number) => {
        if (!purchaseQty && purchaseQty !== 0) {
            setShoppingBasket({
                ...shoppingBasket, 
                basketLines: shoppingBasket.basketLines.map(
                    (line) => line.product.id === productId ? 
                    { ...line, quantity: purchaseQty} : 
                    line
                ).filter((line) => line.quantity > 0)
            })
            return
        }

        const response = await fetch(`https://localhost:7000/api/ShoppingBasket/SetShoppingQuantity`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            },
            body: JSON.stringify({ quantity: purchaseQty, productId})
        })
        
        if (response.ok) {
            setShoppingBasket({
                ...shoppingBasket, 
                basketLines: shoppingBasket.basketLines.map(
                    (line) => line.product.id === productId ? 
                    { ...line, quantity: purchaseQty} : 
                    line
                ).filter((line) => line.quantity > 0)
            })
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await response.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Adding to cart failed: ${errorMessage}`)
        }
    }

    const createOrder = async () => {
        const orderLines = shoppingBasket.basketLines.map(function (basketLine) {
            return { quantity: basketLine.quantity, price: basketLine.product.price, productId: basketLine.product.id }
        })
        const response = await fetch(`https://localhost:7000/api/Order`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            },
            body: JSON.stringify({ orderLines: orderLines })
        })
        
        if (response.ok) {
            alert(`Order created successfully!`)
            await clearShoppingBasket()
            setShoppingBasket({id: '', basketLines: []})
            navigate(-1)
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await response.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Adding to cart failed: ${errorMessage}`)
        }
    }

    const clearShoppingBasket = async () => {
        await fetch(`https://localhost:7000/api/ShoppingBasket`, {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            }
        })
    }

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
                        hasLoaded ?
                        shoppingBasket.basketLines.map((basketLine) => (
                            <ShoppingBasketRow key={basketLine.product.id} basketLine={basketLine} onMinusClicked={decreaseShoppingQuantity} onQuantityChanged={setShoppingQuantity} onPlusClicked={increaseShoppingQuantity}   />
                        )) :
                        <tr>
                            <td colSpan={4} className="text-center">
                                Loading....
                            </td>
                        </tr>
                    }
                    <tr>
                        <td colSpan={2} />
                        <td className='fw-bold'>TOTAL:</td>
                        <td className='fw-bold'>
                            {
                                shoppingBasket.basketLines
                                    .reduce((partialSum, x) => partialSum + (x.quantity * x.product.price), 0)
                                    .toLocaleString('de-DE', { style: 'currency', currency: 'EUR'})
                            }
                        </td>
                    </tr>
                </tbody>
            </table>
            <div className='float-end'>
                <Button className="btn btn-dark" text="Checkout" onClick={createOrder} />
                <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
            </div>
        </div>
      )
}

export default ShoppingBasket