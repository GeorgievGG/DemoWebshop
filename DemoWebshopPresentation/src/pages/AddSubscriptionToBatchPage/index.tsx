import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import CardDetailsForm from '../../components/shopping-basket/CardDetailsForm'
import { selectPaymentState, selectSessionState } from '../../store'
import { setSubscriptionAddedState } from '../../store/paymentSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import { IPaymentCardData } from '../ShoppingBasketPage/types'

const AddSubscriptionToBatchPage = () => {
    const navigate = useNavigate()
    const dispatch = useDispatch()
    
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    const paymentState = useSelector<RootState, IPaymentState>(selectPaymentState)

    const addSubcriptionToBatch = async (userInput: IPaymentCardData) => {
        const response = await fetch('https://localhost:7000/api/Payment/AddSubscription', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            },
            body: JSON.stringify({ cardData: userInput, paymentData: paymentState })
        })
        
        const body = await response.text()
        if (response.ok) {
            dispatch(setSubscriptionAddedState(true))
            navigate(-1)
        }
        else {
            let errorMessage = 'Unknown error'
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            toast.error(`Couldn't add subscription!`)
        }
    }

    return (
        <CardDetailsForm tokenizable={false} onCheckout={addSubcriptionToBatch} />
    )
}

export default AddSubscriptionToBatchPage