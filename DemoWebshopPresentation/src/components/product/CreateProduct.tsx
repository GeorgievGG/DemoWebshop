import React, { MouseEventHandler, FormEventHandler } from 'react'
import { useState } from "react"
import Button from '../common/Button'

type Props = {
    token: string
    onProductCreate: (productJson: any) => void
    onGoBackClick: MouseEventHandler
}

const CreateProduct = ({token, onProductCreate, onGoBackClick}: Props) => {
  const [name, setName] = useState('')
  const [pictureUrl, setPictureUrl] = useState('')
  const [model, setModel] = useState('')
  const [availableQuantity, setAvailableQuantity] = useState(0)
  const [price, setPrice] = useState(0.00)

  const createProduct = async (userInput: FormProductInfo) => {
    const response = await fetch('https://localhost:7000/api/Product', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(userInput)
    })
    
    const body = await response.text()
    if (response.ok) {
      const data = JSON.parse(body)
      onProductCreate(data)
      alert(`Product ${data.name} created!`)
    }
    else {
      let errorMessage = 'Unknown error'
      if (body && body !== '') {
        const data = JSON.parse(body)
        errorMessage = data.message
      }
      alert(`Creation failed for product ${userInput.name}: ${errorMessage}`)
    }
  }

  const onSubmit: FormEventHandler<HTMLFormElement> = (e) => {
    e.preventDefault()

    switch(true) {
        case !name:
            alert('Please type in product name!')
            return
        case !pictureUrl:
            alert('Please type in picture url!')
            return
        case !model:
            alert('Please type in product model!')
            return
        case !availableQuantity:
            alert('Please type in available quantity!')
            return
        case availableQuantity < 0:
            alert('Quantity must be 0 or more!')
            return
        case !price:
            alert('Please type in product price!')
            return
        case price <= 0:
            alert('Price must be a positive number!')
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
        <Button className="btn btn-dark" text="Go Back" onClick={onGoBackClick} />
    </div>
  )
}

export default CreateProduct

