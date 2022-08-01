import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { IUserSessionData, UserSessionSliceState } from "./types"

const initialState: UserSessionSliceState = {
    userSession: { Token: '', UserLogged: false, LoggedUserId: '', LoggedUserRole: ''}
}

export const sessionSlice = createSlice({
     name: 'session',
     initialState,
     reducers: {
         setSessionData: (state, action: PayloadAction<IUserSessionData>) => {
             state.userSession = action.payload
         },
         flushSessionData: (state) => {
             state.userSession = initialState.userSession
         }
     }
})

export const { setSessionData, flushSessionData } = sessionSlice.actions