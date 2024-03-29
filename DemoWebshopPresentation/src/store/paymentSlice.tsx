import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { IPaymentState, PaymentSliceState } from "./types"

const initialState: PaymentSliceState = {
    payment: { 
        hostedCheckoutId: '', 
        directPaymentId: '', 
        batchPaymentAdded: false, 
        scheduledPaymentAdded: false, 
        subscriptionAdded: false, 
        subscriptionId: '', 
        orderAmount: 0, 
        currency: 'EUR' }
}

export const paymentSlice = createSlice({
    name: 'payment',
    initialState,
    reducers: {
        setDirectPaymentId: (state, action: PayloadAction<string>) => {
            state.payment.directPaymentId = action.payload
        },
        setBatchPaymentAddedState: (state, action: PayloadAction<boolean>) => {
            state.payment.batchPaymentAdded = action.payload
        },
        setScheduledPaymentAddedState: (state, action: PayloadAction<boolean>) => {
            state.payment.scheduledPaymentAdded = action.payload
        },
        setSubscriptionAddedState: (state, action: PayloadAction<boolean>) => {
            state.payment.subscriptionAdded = action.payload
        },
        setSubscriptionIdState: (state, action: PayloadAction<string>) => {
            state.payment.subscriptionId = action.payload
        },
        setPaymentState: (state, action: PayloadAction<IPaymentState>) => {
            state.payment = action.payload
        },
        flushPaymentState: (state) => {
            state.payment = initialState.payment
        }
    }
})

export const { setDirectPaymentId, setBatchPaymentAddedState, setScheduledPaymentAddedState, setSubscriptionAddedState, setSubscriptionIdState, setPaymentState, flushPaymentState } = paymentSlice.actions