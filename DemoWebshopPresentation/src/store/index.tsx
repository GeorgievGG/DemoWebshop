import { configureStore } from "@reduxjs/toolkit";
import { sessionSlice } from "./sessionSlice";
import { RootState } from "./types";

const store = configureStore({ 
    reducer: {
       userSession: sessionSlice.reducer
    }
})

export const selectSessionState = (state: RootState) => state.userSession.userSession

export default store