import { configureStore, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { IUserSessionData, RootState, UserSessionSliceState } from "./types";

const initialState: UserSessionSliceState = {
   userSession: { Token: '', UserLogged: false, LoggedUserId: '', LoggedUserRole: ''}
}

export const sessionSlice = createSlice({
    name: 'session',
    initialState,
    reducers: {
        setState: (state, action: PayloadAction<IUserSessionData>) => {
            state.userSession = action.payload
        },
        flushState: (state) => {
            state.userSession = initialState.userSession
        }
    }
})

export const { setState, flushState } = sessionSlice.actions

const store = configureStore({ 
    reducer: {
       userSession: sessionSlice.reducer
    } 
})

export const getSessionState = (state: RootState) => state.userSession.userSession

export default store