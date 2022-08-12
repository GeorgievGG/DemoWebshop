import React, { FormEventHandler } from 'react'
import { useState } from "react"
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import Button from '../common/Button'
import TokenizationCheckbox from './TokenizationCheckbox'
import { ICardDetailsFormProps } from '../../utility/types'

const CardDetailsForm = ({ tokenizable, onCheckout, tokenCardData, shouldTokenizeCardData, onTokenizationChecked}: ICardDetailsFormProps ) => {
  const navigate = useNavigate()

  const [cardholderName, setCardholderName] = useState(tokenCardData?.cardholderName ?? '')
  const [cardNumber, setCardNumber] = useState(tokenCardData?.cardNumber ?? '')
  const [cardCvv, setCardCvv] = useState('')
  const [cardExpiryDate, setCardExpiryDate] = useState(tokenCardData?.expiryDate ?? '')

  const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
    e.preventDefault()

    if (!cardNumber) {
        toast.error('Please type in card number!')
        return
    }

    if (!cardholderName) {
        toast.error('Please type in cardholder name!')
        return
    }
    
    if (!cardExpiryDate) {
        toast.error('Please type in expiry date!')
        return
    }
    
    if (!cardCvv) {
        toast.error('Please type in CVV!')
        return
    }

    onCheckout({ cardNumber, cardholderName, expiryDate: cardExpiryDate, cardCvv})
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
            { tokenizable && <TokenizationCheckbox shouldTokenizeCardData={shouldTokenizeCardData} onChecked={onTokenizationChecked} /> }
            <div className='float-end'>
              <input className="btn btn-dark" type='submit' value='Pay' />
              <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
            </div>
        </form>
    </div>
  )
}

export default CardDetailsForm

