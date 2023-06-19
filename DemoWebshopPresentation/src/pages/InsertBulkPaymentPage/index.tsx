import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import CardDetailsForm from '../../components/shopping-basket/CardDetailsForm'
import { selectPaymentState, selectSessionState } from '../../store'
import { setBatchPaymentAddedState } from '../../store/paymentSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import { handleNegativeResponse } from '../../utility'
import { IPaymentCardData } from '../ShoppingBasketPage/types'

const InsertBulkPaymentPage = () => {
    const navigate = useNavigate()
    const dispatch = useDispatch()
    
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    const paymentState = useSelector<RootState, IPaymentState>(selectPaymentState)

    const addBulkPaymentRecord = async (userInput: IPaymentCardData) => {
        const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/api/Payment/AddBatchPayment`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            },
            body: JSON.stringify({ cardData: userInput, paymentData: paymentState })
        })
        
        if (response.ok) {
            dispatch(setBatchPaymentAddedState(true))
            navigate(-1)
        }
        else {
            handleNegativeResponse(response, "Couldn't send payment", null, true)
        }
    }

    return (
        <CardDetailsForm tokenizable={false} onCheckout={addBulkPaymentRecord} />
    )
}

export default InsertBulkPaymentPage