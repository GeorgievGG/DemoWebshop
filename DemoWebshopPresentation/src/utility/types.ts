import { IPaymentCardData, ITokenCardData } from "../pages/ShoppingBasketPage/types"

export interface ICardDetailsFormProps {
    tokenizable: boolean
    onCheckout: (input: IPaymentCardData) => void
    shouldTokenizeCardData?: boolean
    onTokenizationChecked?: () => void
    tokenCardData?: ITokenCardData
}