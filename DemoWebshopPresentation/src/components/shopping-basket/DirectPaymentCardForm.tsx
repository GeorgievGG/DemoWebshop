import React, { FormEventHandler } from 'react'
import { useState } from "react"
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { IDirectPaymentData, IPaymentCardData } from '../../pages/ShoppingBasketPage/types'
import { selectPaymentState, selectSessionState } from '../../store'
import { setDirectPaymentId } from '../../store/paymentSlice'
import { setPaymentCardToken } from '../../store/sessionSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import Button from '../common/Button'

const DirectPaymentCardForm = () => {
  const navigate = useNavigate()
  const dispatch = useDispatch()

  // TODO: on load -> check the state for token, call getToken if it exists and fill in data

  const paymentState = useSelector<RootState, IPaymentState>(selectPaymentState)
  const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
  
  const [cardholderName, setCardholderName] = useState('')
  const [cardNumber, setCardNumber] = useState('')
  const [cardCvv, setCardCvv] = useState('')
  const [cardExpiryDate, setCardExpiryDate] = useState('')
  const [shouldTokenizeCardData, setShouldTokenizeCardData] = useState(false)

  const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
    e.preventDefault()

    if (!cardholderName) {
        toast.error('Please type in username!')
        return
    }

    if (!cardNumber) {
        toast.error('Please type in username!')
        return
    }
    
    if (!cardCvv) {
        toast.error('Please type in password!')
        return
    }
    
    if (!cardExpiryDate) {
        toast.error('Please type in password!')
        return
    }

    sendPayment({ cardNumber, cardholderName, cardExpiryDate, cardCvv})
  }

  const generateDirectPaymentModel = (userInput: IPaymentCardData): IDirectPaymentData  => {
    // TODO: if token exists
    // if gettoken matches the input
    // send token instead of carddata
    const date = new Date()

    return {
      cardData: userInput,
      paymentData: paymentState,
      browserData: { 
        locale: navigator.language,
        timezoneOffsetUtcMinutes: date.getTimezoneOffset(),
        userAgent: navigator.userAgent,
        colorDepth: window.screen.colorDepth,
        screenHeight: window.innerHeight,
        screenWidth: window.innerWidth
      },
      redirectUrl: window.location.href.replace('directPayment', 'shoppingBasket')
    }
  }

  const sendPayment = async (userInput: IPaymentCardData) => {
    if (shouldTokenizeCardData) {
      const tokenResponse = await fetch('https://localhost:7000/api/Payment/CreateToken', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${sessionState.Token}`
      },
      body: JSON.stringify(userInput)
      })
      const body = await tokenResponse.text()
      if (tokenResponse.ok) {
        const data = JSON.parse(body)
        // INFO: I know Redux is not a proper place to store that, 
        // I'm just making an exception in order to move quickly while exploring different request options 
        dispatch(setPaymentCardToken(data.token))
      }
      else {
        toast.error(`Couldn't tokenize card details!`)
      }
    }

    const response = await fetch('https://localhost:7000/api/Payment/ServerToServerPayment', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${sessionState.Token}`
      },
      body: JSON.stringify(generateDirectPaymentModel(userInput))
    })
    
    const body = await response.text()
    if (response.ok) {
      const data = JSON.parse(body)
      dispatch(setDirectPaymentId(data.payment.id))
      navigate(-1)
    }
    else {
      let errorMessage = 'Unknown error'
      if (body && body !== '') {
        const data = JSON.parse(body)
        errorMessage = data.message
      }
      toast.error(`Couldn't send payment: ${errorMessage}!`)
    }
}

  return (
    <div>
        <form className="add-form"
            onSubmit={onSubmit}>
            <div className="form-control border-0">
                <label>Card number</label>
                <input type='text' 
                    value={cardNumber}
                    onChange={(e) => setCardNumber(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Cardholder's name</label>
                <input type='text' 
                    value={cardholderName}
                    onChange={(e) => setCardholderName(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Expiry date</label>
                <input type='text' 
                    placeholder='MMYY'
                    value={cardExpiryDate}
                    onChange={(e) => setCardExpiryDate(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Security code</label>
                <input type='text' 
                    placeholder='123'
                    value={cardCvv}
                    onChange={(e) => setCardCvv(e.target.value)} />
            </div>
            <div className="form-control border-0 checkboxContainer">
                <label>
                  <input id='tokenizeCheckbox' 
                    type='checkbox' 
                    checked={shouldTokenizeCardData}
                    onChange={(e) => setShouldTokenizeCardData(e.currentTarget.checked)} />
                    Remember my payment details for future purchases
                </label>
            </div>
            <input className="btn btn-dark" type='submit' value='Pay' />
            <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
        </form>
    </div>
  )
}

export default DirectPaymentCardForm

