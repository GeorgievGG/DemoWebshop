import { configureStore } from "@reduxjs/toolkit";
import { productsSlice } from "./productsSlice";
import { sessionSlice } from "./sessionSlice";
import { RootState } from "./types";

const store = configureStore({ 
    reducer: {
       userSession: sessionSlice.reducer,
       products: productsSlice.reducer
    }
})

export const selectSessionState = (state: RootState) => state.userSession.userSession
export const selectProductsState = (state: RootState) => state.products.products

export default store