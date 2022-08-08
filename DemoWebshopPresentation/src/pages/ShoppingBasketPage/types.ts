export interface ServerToServerPaymentInput
{
    CardData: ICardData
    PaymentData: IPaymentData
    BrowserData: IBrowserData
    RedirectUrl: string
}

export interface ICardData
{
    CardholderName: string
    CardNumber: string
    CardCVV: string
    CardExpiryDate: string
}

export interface IPaymentData
{
    orderAmount: number
    currency: string
}

export interface IBrowserData
{
    Locale: string
    TimezoneOffsetUtcMinutes: number
    UserAgent: string
    ColorDepth: number
    ScreenHeight: number
    ScreenWidth: number
}

export interface IBasket {
    id: string
    basketLines: IShoppingBasketLine[]
}

export interface IShoppingBasketLine {
    quantity: number
    product: BasketProductInfo
}

export interface IDirectPaymentData extends IPaymentData {
    cardNumber: string
    cardholderName: string
    cardExpiryDate: string
    cardCvv: string
}