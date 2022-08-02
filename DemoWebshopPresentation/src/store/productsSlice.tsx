import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { ProductsSliceState } from "./types"

const initialState: ProductsSliceState = {
    products: []
}

export const productsSlice = createSlice({
    name: 'products',
    initialState,
    reducers: {
        setProducts: (state, action: PayloadAction<CatalogProductInfo[]>) => {
            state.products = action.payload
        },
        deleteProduct: (state, action: PayloadAction<string>) => {
            state.products = state.products.filter((product) => product.id !== action.payload)
        }
    }
})

export const { setProducts, deleteProduct } = productsSlice.actions