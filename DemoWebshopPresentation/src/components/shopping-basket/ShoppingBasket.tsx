import React, { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { IBasket } from '../../pages/ShoppingBasketPage/types'
import { selectPaymentState, selectSessionState } from '../../store'
import { flushPaymentState, setPaymentState } from '../../store/paymentSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import { createOrderCall, handleNegativeResponse } from '../../utility'
import Button from '../common/Button'
import ShoppingBasketRow from './ShoppingBasketRow'
import RiseLoader from "react-spinners/RiseLoader";

const ShoppingBasket = () => {
    const [shoppingBasket, setShoppingBasket] = useState<IBasket>({id: '', basketLines: []})
    const [hasLoaded, setHasLoaded] = useState(false)
    
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    const paymentState = useSelector<RootState, IPaymentState>(selectPaymentState)
    const navigate = useNavigate()
    const dispatch = useDispatch()

    useEffect(() => {
        fetch(`${process.env.REACT_APP_SERVER_URL}/api/ShoppingBasket`, {
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
            handleNegativeResponse(response, "Couldn't retrieve shopping basket data!", null, false)
        }
    }

    useEffect(() => {
        if (paymentState && paymentState.hostedCheckoutId && hasLoaded) {
            fetch(`${process.env.REACT_APP_SERVER_URL}/api/Payment/${paymentState.hostedCheckoutId}/CheckHostedCheckoutPagePaymentResult`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${sessionState.Token}`
                }
            })
            .then(response => handleHostedCheckoutResultCheck(response))
        }
        }, [hasLoaded]
    )

    useEffect(() => {
        if (paymentState && paymentState.directPaymentId && hasLoaded) {
            fetch(`${process.env.REACT_APP_SERVER_URL}/api/Payment/${paymentState.directPaymentId}/CheckDirectPaymentResult`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${sessionState.Token}`
                }
            })
            .then(response => handleDirectPaymentResultCheck(response))
        }
        }, [hasLoaded]
    )

    useEffect(() => {
        if (paymentState && 
            hasLoaded &&
            (
                paymentState.batchPaymentAdded || 
                paymentState.scheduledPaymentAdded
            )) {
            createOrder()
            dispatch(flushPaymentState())
        }
        }, [hasLoaded]
    )

    const handleHostedCheckoutResultCheck = async (response: Response) => {
        if (response.ok) {
            const data = await response.json()
            const paymentStatus = data?.createdPaymentOutput?.payment?.status ?? ''
            const isPaymentSuccessful = paymentStatus === 'CAPTURED' || paymentStatus === 'PENDING_CAPTURE'
            if (isPaymentSuccessful){
                createOrder()
            }
            else {
                handleUnsuccessfulPayment(paymentStatus)
            }
            // INFO: I know that Captured status might be delayed. 
            // For the sake of simplicity, I won't implement more detailed logic for that case
            dispatch(flushPaymentState())
        }
        else {
            handleUnsuccessfulPayment(response)
        }
    }

    const handleDirectPaymentResultCheck = async (response: Response) => {
        if (response.ok) {
            const data = await response.json()
            const paymentStatus = data?.status ?? ''
            let isPaymentSuccessful = false
            if (paymentStatus === 'PENDING_CAPTURE') {
                const captureResponse = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/Payment/${paymentState.directPaymentId}/CapturePayment`, {
                    method: 'POST',
                    headers: {
                        'Content-type': 'application/json',
                        'Authorization': `Bearer ${sessionState.Token}`
                    }
                })
                if (response.ok) {
                    const data = await captureResponse.json()
                    isPaymentSuccessful = data?.status === 'CAPTURE_REQUESTED'
                }
            }

            if (isPaymentSuccessful) {
                createOrder()
            }
            else {
                handleUnsuccessfulPayment(response)
            }
            // INFO: I know that Captured status might be delayed. 
            // For the sake of simplicity, I won't implement more detailed logic for that case
            dispatch(flushPaymentState())
        }
        else {
            handleUnsuccessfulPayment(response)
        }
    }

    const handleUnsuccessfulPayment = async (response: Response, paymentStatus?: string) => {
        const message = `Payment unsuccessful${paymentStatus ? `, status: ${paymentStatus}` : ''}!`
        handleNegativeResponse(response, message, null, false)
    }

    const increaseShoppingQuantity = async (productId: string) => {
        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/ShoppingBasket/IncreaseShoppingQuantity/${productId}`, {
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
            handleNegativeResponse(response, "Changing cart data failed", null, true)
        }
    }
    
    const decreaseShoppingQuantity = async (productId: string) => {
        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/ShoppingBasket/DecreaseShoppingQuantity/${productId}`, {
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
            handleNegativeResponse(response, "Changing cart data failed", null, true)
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

        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/ShoppingBasket/SetShoppingQuantity`, {
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
            handleNegativeResponse(response, "Changing cart data failed", null, true)
        }
    }

    const navigateToHostedCheckoutPage = async () => {
        const orderAmount = shoppingBasket.basketLines
        .reduce((partialSum, x) => partialSum + (x.quantity * x.product.price), 0)

        if (orderAmount === 0) {
            toast.error("Can't checkout an empty order!")
            return
        }

        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/Payment/GetHostedCheckoutPage`, {
            method: 'POST',           
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            },
            body: JSON.stringify({ orderAmount, redirectUrl: window.location.href, currency: 'EUR' })
        })
        
        if (response.ok) {
            const data = await response.json()
            dispatch(setPaymentState({ hostedCheckoutId: data.hostedCheckoutId, orderAmount: orderAmount, currency: 'EUR' }))
            window.location.href = data.redirectUrl
        }
        else {
            handleNegativeResponse(response, "Retrieving hosted checkout page failed", null, true)
        }
    }

    const createOrder = async () => {
        const orderLines = shoppingBasket.basketLines.map(function (basketLine) {
            return { quantity: basketLine.quantity, price: basketLine.product.price, productId: basketLine.product.id } as OrderLine
        })

        const response = await createOrderCall(orderLines, sessionState.Token)
        if (response.ok) {
            toast.success(`Order created successfully!`)
            await clearShoppingBasket()
            setShoppingBasket({id: '', basketLines: []})
            navigate('/')
        }
        else {
            handleNegativeResponse(response, "Creating order failed", null, true)
        }
    }

    const navigateToPaymentForm = async (pageAddress: string) => {
        const orderAmount = shoppingBasket.basketLines
            .reduce((partialSum, x) => partialSum + (x.quantity * x.product.price), 0)
            
        if (orderAmount === 0) {
            toast.error("Can't checkout an empty order!")
            return
        }

        dispatch(setPaymentState({ orderAmount: orderAmount, currency: 'EUR' }))
        navigate(pageAddress)
    }

    const clearShoppingBasket = async () => {
        await fetch(`${process.env.REACT_APP_SERVER_URL}/api/ShoppingBasket`, {
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
                        <>
                            {
                                shoppingBasket.basketLines.map((basketLine) => (
                                    <ShoppingBasketRow key={basketLine.product.id} basketLine={basketLine} onMinusClicked={decreaseShoppingQuantity} onQuantityChanged={setShoppingQuantity} onPlusClicked={increaseShoppingQuantity}   />
                                ))
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
                        </> :
                        <tr>
                            <td colSpan={4} className="text-center">
                                <RiseLoader className='loader' color={'black'} size={15} />
                            </td>
                        </tr>
                    }
                   
                </tbody>
            </table>
            <div className='float-end'>
                <Button className="btn btn-dark" text="Checkout by HCP" onClick={navigateToHostedCheckoutPage} />
                <Button className="btn btn-dark" text="Checkout by S-S" onClick={() => navigateToPaymentForm('/directPayment')} />
                <Button className="btn btn-dark" text="Add to Bulk file" onClick={() => navigateToPaymentForm('/bulkPayment')} />
                <Button className="btn btn-dark" text="Pay in 3-month installments" onClick={() => navigateToPaymentForm('/scheduledPayment')} />
                <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
            </div>
        </div>
      )
}

export default ShoppingBasket