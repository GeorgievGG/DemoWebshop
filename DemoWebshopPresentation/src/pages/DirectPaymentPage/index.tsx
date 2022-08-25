import React, { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import CardDetailsForm from '../../components/shopping-basket/CardDetailsForm'
import { selectPaymentState, selectSessionState } from '../../store'
import { setDirectPaymentId } from '../../store/paymentSlice'
import { setPaymentCardToken } from '../../store/sessionSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import { handleNegativeResponse } from '../../utility'
import { IDirectPaymentData, IPaymentCardData, ITokenCardData } from '../ShoppingBasketPage/types'


const DirectPaymentPage = () => {
  const dispatch = useDispatch()
  const navigate = useNavigate()
  useEffect(() => {
      if (sessionState.PaymentCardToken) {
        fetch(`https://localhost:7000/api/Payment/Token/${sessionState.PaymentCardToken}`, {
          method: 'GET',
          headers: {
            'Content-type': 'application/json',
            'Authorization': `Bearer ${sessionState.Token}`
          }
        })
        .then(response => handleGetTokenResponse(response))
      }
    }, []
  )

  const handleGetTokenResponse = async (response: Response) => {
      if (response.ok) {
          const tokenJson = await response.json()
          setTokenCardInfo(tokenJson?.card?.data?.cardWithoutCvv)
      }
      else {
        toast.error("Couldn't retrieve token!")
      }

      setPageLoaded(true)
  }

  const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
  const paymentState = useSelector<RootState, IPaymentState>(selectPaymentState)
  const [tokenCardInfo, setTokenCardInfo] = useState<ITokenCardData>()
  const [shouldTokenizeCardData, setShouldTokenizeCardData] = useState(false)
  const [pageLoaded, setPageLoaded] = useState(false)

  const generateDirectPaymentModel = (userInput: IPaymentCardData): IDirectPaymentData  => {
    const date = new Date()
    let paymentModel = {
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
    } as IDirectPaymentData

    if (sessionState.PaymentCardToken && cardInputMatchesToken(userInput)) {
      paymentModel.token = sessionState.PaymentCardToken
    }
    else {
      paymentModel.cardData = userInput
    }

    return paymentModel
  }

  const cardInputMatchesToken = (userInput: IPaymentCardData): boolean  => {
      if (userInput.cardNumber.substring(0, 6) !== tokenCardInfo?.cardNumber.substring(0, 6)) {
        return false
      }
      if (userInput.cardNumber.substring(12, 17) !== tokenCardInfo?.cardNumber.substring(12, 17)) {
        return false
      }
      if (userInput.cardholderName !== tokenCardInfo?.cardholderName) {
        return false
      }
      if (userInput.expiryDate !== tokenCardInfo?.expiryDate) {
        return false
      }

      return true
  }
  
  const sendPayment = async (userInput: IPaymentCardData) => {
    if (shouldTokenizeCardData) {
      const tokenResponse = await fetch('https://localhost:7000/api/Payment/Token', {
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
      handleNegativeResponse(response, "Couldn't send payment", true)
    }
  }

  return (
    <>
    {
      pageLoaded && <CardDetailsForm tokenizable={true} onCheckout={sendPayment} shouldTokenizeCardData={shouldTokenizeCardData} onTokenizationChecked={() => setShouldTokenizeCardData(!shouldTokenizeCardData)} tokenCardData={tokenCardInfo} />
    }
    </>
  )
}

export default DirectPaymentPage