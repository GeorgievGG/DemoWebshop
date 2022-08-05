import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { PaymentSliceState } from "./types"

const initialState: PaymentSliceState = {
    hostedCheckoutId: ''
}

export const paymentSlice = createSlice({
    name: 'payment',
    initialState,
    reducers: {
        setHostedCheckoutId: (state, action: PayloadAction<string>) => {
            state.hostedCheckoutId = action.payload
        },
        flushHostedCheckoutId: (state) => {
            state.hostedCheckoutId = initialState.hostedCheckoutId
        }
    }
})

export const { setHostedCheckoutId, flushHostedCheckoutId } = paymentSlice.actions