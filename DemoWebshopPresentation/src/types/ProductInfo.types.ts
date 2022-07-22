/* eslint-disable @typescript-eslint/no-unused-vars */
type BasicProductInfo = {
    name: string
    model: string
    pictureUrl: string
    availableQuantity: number
    price: number
}

type CatalogProductInfo = BasicProductInfo & {
    id: string
}