import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { IPaymentState, PaymentSliceState } from "./types"

const initialState: PaymentSliceState = {
    payment: { hostedCheckoutId: '', paymentAmount: 0, currency: 'EUR' }
}

export const paymentSlice = createSlice({
    name: 'payment',
    initialState,
    reducers: {
        setPaymentState: (state, action: PayloadAction<IPaymentState>) => {
            state.payment = action.payload
        },
        flushHostedCheckoutId: (state) => {
            state.payment = initialState.payment
        }
    }
})

export const { setPaymentState, flushHostedCheckoutId } = paymentSlice.actions