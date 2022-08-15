import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { IPaymentState, PaymentSliceState } from "./types"

const initialState: PaymentSliceState = {
    payment: { hostedCheckoutId: '', directPaymentId: '', batchPaymentAdded: false, orderAmount: 0, currency: 'EUR' }
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
        setPaymentState: (state, action: PayloadAction<IPaymentState>) => {
            state.payment = action.payload
        },
        flushPaymentState: (state) => {
            state.payment = initialState.payment
        }
    }
})

export const { setDirectPaymentId, setBatchPaymentAddedState, setPaymentState, flushPaymentState } = paymentSlice.actions