import React, { MouseEventHandler, useEffect, useState } from 'react'
import Button from './Button'
import ShoppingBasketRow from './ShoppingBasketRow'


type Props = {
    token: string
    onGoBackClick: MouseEventHandler
}

type Basket = {
    id: string
    basketLines: ShoppingBasketLine[]
}

type ShoppingBasketLine = {
    quantity: number
    product: BasketProductInfo
}

const ShoppingBasket = ({ token, onGoBackClick }: Props) => {
    const [shoppingBasket, setShoppingBasket] = useState<Basket>({id: '', basketLines: []})
    const [hasLoaded, setHasLoaded] = useState(false)

    useEffect(() => {
        fetch('https://localhost:7000/api/ShoppingBasket', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
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
        const res = await fetch(`https://localhost:7000/api/ShoppingBasket/IncreaseShoppingQuantity/${productId}`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
        
        if (res.ok) {
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
            const body = await res.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Adding to cart failed: ${errorMessage}`)
        }
    }
    
    const decreaseShoppingQuantity = async (productId: string) => {
        const res = await fetch(`https://localhost:7000/api/ShoppingBasket/DecreaseShoppingQuantity/${productId}`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
        
        if (res.ok) {
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
            const body = await res.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Adding to cart failed: ${errorMessage}`)
        }
    }
    
    const setShoppingQuantity = async (productId: string, purchaseQty: number) => {
        const res = await fetch(`https://localhost:7000/api/ShoppingBasket/SetShoppingQuantity`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ quantity: purchaseQty, productId})
        })
        
        if (res.ok) {
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
            const body = await res.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Adding to cart failed: ${errorMessage}`)
        }
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
                                    .toLocaleString('en-US', { style: 'currency', currency: 'USD'})
                            }
                        </td>
                    </tr>
                </tbody>
            </table>
            <div className='float-end'>
                <Button className="btn btn-dark" text="Checkout" onClick={onGoBackClick} />
                <Button className="btn btn-dark" text="Go Back" onClick={onGoBackClick} />
            </div>
        </div>
      )
}

export default ShoppingBasket