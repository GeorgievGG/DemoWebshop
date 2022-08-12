import React from 'react'
import CardDetailsForm from '../../components/shopping-basket/CardDetailsForm'

const InsertBulkPaymentPage = () => {
    const tmp = () => {
        
    }

    return (
        <CardDetailsForm tokenizable={false} onCheckout={() => tmp()} />
    )
}

export default InsertBulkPaymentPage