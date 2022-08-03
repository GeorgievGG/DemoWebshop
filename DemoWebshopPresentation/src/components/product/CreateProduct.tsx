import React, { FormEventHandler } from 'react'
import { useState } from "react"
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { selectSessionState } from '../../store'
import { IUserSessionData, RootState } from '../../store/types'
import { addProduct } from '../../store/productsSlice'
import Button from '../common/Button'
import { toast } from 'react-toastify'

const CreateProduct = () => {
  const [name, setName] = useState('')
  const [pictureUrl, setPictureUrl] = useState('')
  const [model, setModel] = useState('')
  const [availableQuantity, setAvailableQuantity] = useState(0)
  const [price, setPrice] = useState(0.00)

  const dispatch = useDispatch()
  const navigate = useNavigate()
  const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)

  const createProduct = async (userInput: FormProductInfo) => {
    const response = await fetch('https://localhost:7000/api/Product', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${sessionState.Token}`
      },
      body: JSON.stringify(userInput)
    })
    
    const body = await response.text()
    if (response.ok) {
      const data = JSON.parse(body)
      dispatch(addProduct(data))
      toast.success(`Product ${data.name} created!`)
    }
    else {
      let errorMessage = 'Unknown error'
      if (body && body !== '') {
        const data = JSON.parse(body)
        errorMessage = data.message
      }
      toast.error(`Creation failed for product ${userInput.name}: ${errorMessage}`)
    }
  }

  const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
    e.preventDefault()

    switch(true) {
        case !name:
            toast.error('Please type in product name!')
            return
        case !pictureUrl:
            toast.error('Please type in picture url!')
            return
        case !model:
            toast.error('Please type in product model!')
            return
        case !availableQuantity:
            toast.error('Please type in available quantity!')
            return
        case availableQuantity < 0:
            toast.error('Quantity must be 0 or more!')
            return
        case !price:
            toast.error('Please type in product price!')
            return
        case price <= 0:
            toast.error('Price must be a positive number!')
            return
    }

    createProduct({ name, pictureUrl, model, availableQuantity, price })
  }

  return (
    <div>
        <form className="add-form"
            onSubmit={onSubmit}>
            <div className="form-control border-0">
                <label>Product name</label>
                <input type='text' 
                    placeholder='Type name'
                    value={name}
                    onChange={(e) => setName(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Product image URL</label>
                <input type='text' 
                    placeholder='Type image URL'
                    value={pictureUrl}
                    onChange={(e) => setPictureUrl(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Product model/description</label>
                <input type='text' 
                    placeholder='Type model/description'
                    value={model}
                    onChange={(e) => setModel(e.target.value)} />
            </div>
            <div className="form-control border-0">
                <label>Available quantity</label>
                <input type='number' 
                    value={availableQuantity}
                    onChange={(e) => setAvailableQuantity(e.target.valueAsNumber)} />
            </div>
            <div className="form-control border-0">
                <label>Price</label>
                <input type='number' 
                    step=".01"
                    value={price}
                    onChange={(e) => setPrice(e.target.valueAsNumber)} />
            </div>
            <input className="btn btn-dark" type='submit' value='Create' />
        </form>
        <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
    </div>
  )
}

export default CreateProduct

