import React, { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { selectSessionState } from '../../store'
import { IUserSessionData, RootState } from '../../store/types'
import { handleNegativeResponse } from '../../utility'
import Button from '../common/Button'
import OrderRow from './OrderRow'
import RiseLoader from "react-spinners/RiseLoader";

export const OrderList = () => {
  const [orders, setOrders] = useState<OrderInfo[]>([])
  const [hasLoaded, setHasLoaded] = useState(false)
  
  const navigate = useNavigate()
  const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)

  useEffect(() => {
    fetch(`${process.env.API_URL}/api/Order`, {
            method: 'GET',
            headers: {
              'Authorization': `Bearer ${sessionState.Token}`
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
      handleNegativeResponse(response, "Couldn't retrieve orders!", null, false)
    }
  }

  const confirmOrder = async (orderId: string) => {
    const response = await fetch(`${process.env.API_URL}/api/Order/${orderId}/ConfirmOrder`, {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            }
        })
        
        if (response.ok) {
            toast.success(`Order updated!`)
            setOrders(orders.map((order) =>
              order.id === orderId ? { ...order, confirmed: true } : order
            ))
        }
        else {
            handleNegativeResponse(response, 'Updating order failed', null, true)
        }
  }

  const setOrderDeliveryDate = async (orderId: string, deliveryDate: Date) => {
    const response = await fetch(`${process.env.API_URL}/api/Order/${orderId}/SetDeliveryDate`, {
            method: 'PUT',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            },
            body: JSON.stringify({ deliveryDate })
        })
        
        if (response.ok) {
            toast.success(`Order updated!`)
            setOrders(orders.map((order) =>
              order.id === orderId ? { ...order, deliveryDate: deliveryDate } : order
            ))
        }
        else {
          handleNegativeResponse(response, 'Updating order failed', null, true)
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
                  <td colSpan={6} className="text-center">
                    <RiseLoader className='loader' color={'black'} size={15} />
                  </td>
              </tr>
            }
          </tbody>
        </table>

        <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
      </div>
    </>
  )
}

export default OrderList
