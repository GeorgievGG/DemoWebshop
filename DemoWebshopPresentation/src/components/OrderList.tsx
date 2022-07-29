import React, { MouseEventHandler, useEffect, useState } from 'react'
import Button from './Button'
import OrderRow from './OrderRow'

type Props = {
  token: string
  onGoBackClick: MouseEventHandler
}

export const OrderList = ({ token, onGoBackClick }: Props) => {
  const [orders, setOrders] = useState<OrderInfo[]>([])
  const [hasLoaded, setHasLoaded] = useState(false)

  useEffect(() => {
    fetch('https://localhost:7000/api/Order', {
            method: 'GET',
            headers: {
              'Authorization': `Bearer ${token}`
            }
        })
        .then(response => handleGetOrdersResponse(response))
    }, []
  )
  
  const handleGetOrdersResponse = async (response: Response) => {
    if (response.ok) {
        const data = await response.json()
        setOrders(data)
        setHasLoaded(true)
    }
    else {
      alert(`Couldn't retrieve orders!`)
    }
  }

  const confirmOrder = async (orderId: string) => {
    const response = await fetch(`https://localhost:7000/api/Order/${orderId}/ConfirmOrder`, {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
        
        if (response.ok) {
            alert(`Order updated!`)
            setOrders(orders.map((order) =>
              order.id === orderId ? { ...order, confirmed: true } : order
            ))
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await response.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Updating order failed: ${errorMessage}`)
        }
  }

  const setOrderDeliveryDate = async (orderId: string, deliveryDate: Date) => {
    const response = await fetch(`https://localhost:7000/api/Order/${orderId}/SetDeliveryDate`, {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ deliveryDate })
        })
        
        if (response.ok) {
            alert(`Order updated!`)
            setOrders(orders.map((order) =>
              order.id === orderId ? { ...order, deliveryDate: deliveryDate } : order
            ))
        }
        else {
            let errorMessage = 'Unknown error'
            const body = await response.text()
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Updating order failed: ${errorMessage}`)
        }
  }

  return (
    <>
      <div>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Order Date</th>
              <th scope="col">Client</th>
              <th scope="col">Confirmed</th>
              <th scope="col">Delivery Date</th>
              <th scope="col">Contents</th>
              <th scope="col">Total Amount</th>
              <th scope="col"></th>
            </tr>
          </thead>
          <tbody>
            {
              hasLoaded ?
              orders.map((order) => (
                  <OrderRow key={order.id} order={order} onConfirmOrder={confirmOrder} onSetDeliveryDate={setOrderDeliveryDate} />
              )) :
              <tr>
                  <td colSpan={4} className="text-center">
                      Loading....
                  </td>
              </tr>
            }
          </tbody>
        </table>

        <Button className="btn btn-dark" text="Go Back" onClick={onGoBackClick} />
      </div>
    </>
  )
}
