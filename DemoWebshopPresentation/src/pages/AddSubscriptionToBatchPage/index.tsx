import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import CardDetailsForm from '../../components/shopping-basket/CardDetailsForm'
import { selectPaymentState, selectSessionState } from '../../store'
import { setSubscriptionAddedState } from '../../store/paymentSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import { handleNegativeResponse } from '../../utility'
import { IPaymentCardData } from '../ShoppingBasketPage/types'

const AddSubscriptionToBatchPage = () => {
    const navigate = useNavigate()
    const dispatch = useDispatch()
    
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    const paymentState = useSelector<RootState, IPaymentState>(selectPaymentState)

    const addSubcriptionToBatch = async (userInput: IPaymentCardData) => {
        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/Payment/AddSubscription`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            },
            body: JSON.stringify({ cardData: userInput, paymentData: paymentState })
        })
        
        if (response.ok) {
            dispatch(setSubscriptionAddedState(true))
            navigate(-1)
        }
        else {
            handleNegativeResponse(response, "Couldn't add subscription!", null, false)
        }
    }

    return (
        <CardDetailsForm tokenizable={false} onCheckout={addSubcriptionToBatch} />
    )
}

export default AddSubscriptionToBatchPage