export interface IBasket {
    id: string
    basketLines: IShoppingBasketLine[]
}

export interface IShoppingBasketLine {
    quantity: number
    product: BasketProductInfo
}

export interface ITokenCardData {
    cardNumber: string
    cardholderName: string
    expiryDate: string
}

export interface IPaymentCardData extends ITokenCardData {
    cardCvv: string
}

export interface IPaymentData
{
    orderAmount: number
    currency: string
}

export interface IBrowserData
{
    locale: string
    timezoneOffsetUtcMinutes: number
    userAgent: string
    colorDepth: number
    screenHeight: number
    screenWidth: number
}

export interface IDirectPaymentData {
    cardData?: IPaymentCardData
    token?: string
    paymentData: IPaymentData
    browserData: IBrowserData
    redirectUrl: string
}