/* eslint-disable @typescript-eslint/no-unused-vars */
type BasicProductInfo = {
    name: string
    availableQuantity: number
    price: number
    isSubscription: boolean
}

type BasketProductInfo = BasicProductInfo & {
    id: string
}

type CatalogProductInfo = FormProductInfo & {
    id: string
}

type FormProductInfo = BasicProductInfo & {
    model: string
    pictureUrl: string
}