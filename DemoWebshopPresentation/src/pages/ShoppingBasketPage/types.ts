export interface IBasket {
    id: string
    basketLines: IShoppingBasketLine[]
}

export interface IShoppingBasketLine {
    quantity: number
    product: BasketProductInfo
}

export interface IPaymentCardData {
    cardNumber: string
    cardholderName: string
    cardExpiryDate: string
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
    cardData: IPaymentCardData
    paymentData: IPaymentData
    browserData: IBrowserData
    redirectUrl: string
}