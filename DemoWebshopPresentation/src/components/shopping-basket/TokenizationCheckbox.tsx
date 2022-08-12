import React from 'react'
import { ITokenizationCheckboxProps } from '../../pages/DirectPaymentPage/types'

const TokenizationCheckbox = ({ shouldTokenizeCardData, onChecked }: ITokenizationCheckboxProps ) => {
  return (
    <div className="form-control border-0 checkboxContainer">
        <label>
            <input id='tokenizeCheckbox' 
                type='checkbox' 
                checked={shouldTokenizeCardData}
                onChange={(e) => onChecked && onChecked(e.currentTarget.checked)} />
                Remember my payment details for future purchases
        </label>
    </div>
  )
}

export default TokenizationCheckbox