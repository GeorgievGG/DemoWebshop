import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { IUserSessionData, UserSessionSliceState } from "./types"

const initialState: UserSessionSliceState = {
    userSession: { Token: '', UserLogged: false, LoggedUserId: '', LoggedUserRole: '', PaymentCardToken: '' }
}

export const sessionSlice = createSlice({
     name: 'session',
     initialState,
     reducers: {
         setSessionData: (state, action: PayloadAction<IUserSessionData>) => {
             state.userSession = action.payload
         },
         setPaymentCardToken: (state, action: PayloadAction<string>) => {
            state.userSession.PaymentCardToken = action.payload
        },
         flushSessionData: (state) => {
             state.userSession = initialState.userSession
         }
     }
})

export const { setSessionData, setPaymentCardToken, flushSessionData } = sessionSlice.actions