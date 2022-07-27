type OrderInfo = {
    id: string
    orderDate: Date
    confirmed: boolean
    deliveryDate: Date
    client: UserInfo
    orderLines: OrderLine[]
}
  
type OrderLine = {
    quantity: number
    price: number
    product: BasketProductInfo
}