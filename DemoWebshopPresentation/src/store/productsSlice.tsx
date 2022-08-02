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
        addProduct: (state, action: PayloadAction<CatalogProductInfo>) => {
            state.products = [...state.products, action.payload]
        },
        updateProduct: (state, action: PayloadAction<CatalogProductInfo>) => {
            state.products = state.products.map((product) =>
                product.id === action.payload.id ? action.payload : product
            )
        },
        deleteProduct: (state, action: PayloadAction<string>) => {
            state.products = state.products.filter((product) => product.id !== action.payload)
        }
    }
})

export const { setProducts, addProduct, updateProduct, deleteProduct } = productsSlice.actions