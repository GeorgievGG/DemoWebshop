import React, { useState } from 'react'
import Button from './Button'
import DatePicker from "react-datepicker";

type Props = {
    order: OrderInfo
    onConfirmOrder: (orderId: string) => void
    onSetDeliveryDate: (orderId: string, deliveryDate: Date) => void
}

const OrderRow = ({ order, onConfirmOrder, onSetDeliveryDate }: Props) => {
    const [selectedDeliveryDate, setSelectedDeliveryDate] = useState(new Date())
    const [datePickerOpen, setDatePickerOpen] = useState(Boolean)

    const changeDatePickerState = () => {
        setDatePickerOpen(!datePickerOpen)
    }

    const setDeliveryDateState = (orderId: string, deliveryDate: Date) => {
        setSelectedDeliveryDate(deliveryDate)
        onSetDeliveryDate(orderId, deliveryDate)
    }

    return (
        <tr>
            <td>{new Date(order.orderDate).toLocaleDateString('en-US')}</td>
            <td>{order.client?.username}</td>
            <td>{order.confirmed === true ? 'Yes' : 'No'}</td>
            <td>{order.deliveryDate ? new Date(order.deliveryDate).toLocaleDateString('en-US') : null}</td>
            <td>
                {order.orderLines?.map((order) => (
                    <div>{order.productId} x {order.quantity}</div>
                ))}
            </td>
            <td>
                {order.orderLines?.reduce((partialSum, x) => partialSum + (x.quantity * x.price), 0)
                                    .toLocaleString('en-US', { style: 'currency', currency: 'USD'})}
            </td>
            <td>
                <Button className="btn btn-dark" text="Confirm" onClick={() => onConfirmOrder(order.id)} />
                <Button className="btn btn-dark" text="Set Delivery Date" onClick={() => changeDatePickerState()} />
                <DatePicker
                    selected={selectedDeliveryDate}
                    onChange={(date) => setDeliveryDateState(order.id, date!)}
                    onClickOutside={changeDatePickerState}
                    open={datePickerOpen} />
            </td>
        </tr>
    )
}

export default OrderRow