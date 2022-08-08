import { configureStore, getDefaultMiddleware } from "@reduxjs/toolkit";
import { persistStore, persistCombineReducers, FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER } from 'reduxjs-toolkit-persist';
import storage from 'reduxjs-toolkit-persist/lib/storage'
import autoMergeLevel1 from 'reduxjs-toolkit-persist/lib/stateReconciler/autoMergeLevel1';
import { productsSlice } from "./productsSlice";
import { sessionSlice } from "./sessionSlice";
import { RootState } from "./types";
import { paymentSlice } from "./paymentSlice";

const persistConfig = {
    key: 'root',
    storage: storage,
    stateReconciler: autoMergeLevel1,
};

const persistedReducer = persistCombineReducers(
    persistConfig,
    {
      userSession: sessionSlice.reducer,
      products: productsSlice.reducer,
      payment: paymentSlice.reducer
    }
);

const store = configureStore({ 
    reducer: persistedReducer,
    middleware: getDefaultMiddleware({
        serializableCheck: {
          /* ignore persistance actions */
          ignoredActions: [
            FLUSH,
            REHYDRATE,
            PAUSE,
            PERSIST,
            PURGE,
            REGISTER
          ],
        },
    })
})

export const selectSessionState = (state: RootState) => state.userSession.userSession
export const selectProductsState = (state: RootState) => state.products.products
export const selectPaymentState = (state: RootState) => state.payment.payment
export const persistor = persistStore(store)

export default store