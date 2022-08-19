import store from "."

export interface IUserSessionData {
    Token: string
    UserLogged: boolean
    LoggedUserId: string
    LoggedUserRole: string
    PaymentCardToken?: string
}

export interface UserSessionSliceState {
    userSession: IUserSessionData
}

export interface ProductsSliceState {
    products: CatalogProductInfo[]
}

export interface IPaymentState {
    hostedCheckoutId?: string
    directPaymentId?: string
    batchPaymentAdded?: boolean
    scheduledPaymentAdded?: boolean
    orderAmount: number
    currency: string
}

export interface PaymentSliceState {
    payment: IPaymentState
}

export type RootState = ReturnType<typeof store.getState>