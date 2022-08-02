import React, { MouseEventHandler, FormEventHandler, useEffect } from 'react'
import { useState } from "react"
import { useLocation } from 'react-router-dom'
import Button from '../common/Button'

type Props = {
    token: string
    onProductUpdate: (product: CatalogProductInfo) => void
    onGoBackClick: MouseEventHandler
}

const UpdateProduct = ({token, onProductUpdate, onGoBackClick}: Props) => {
    const [name, setName] = useState('')
    const [pictureUrl, setPictureUrl] = useState('')
    const [model, setModel] = useState('')
    const [availableQuantity, setAvailableQuantity] = useState(0)
    const [price, setPrice] = useState(0.00)

    const location = useLocation()
    const state = location.state as { product: CatalogProductInfo };
    useEffect(() => {
            setName(state.product.name)
            setPictureUrl(state.product.pictureUrl)
            setModel(state.product.model)
            setAvailableQuantity(state.product.availableQuantity)
            setPrice(state.product.price)
        }, [state]
    )


    const updateProduct = async (userInput: FormProductInfo) => {
        const response = await fetch(`https://localhost:7000/api/Product/${state.product.id}`, {
        method: 'PUT',
        headers: {
            'Content-type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(userInput)
        })
        
        const body = await response.text()
        if (response.ok) {
            var updatedProduct = { ...userInput, id: state.product.id } as CatalogProductInfo
            onProductUpdate(updatedProduct)
            alert(`Product ${updatedProduct.name} updated!`)
        }
        else {
            let errorMessage = 'Unknown error'
            if (body && body !== '') {
                const data = JSON.parse(body)
                errorMessage = data.message
            }
            alert(`Update failed for product ${userInput.name}: ${errorMessage}`)
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
            case !availableQuantity && availableQuantity !== 0:
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

        updateProduct({ name, pictureUrl, model, availableQuantity, price })
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
                <input className="btn btn-dark" type='submit' value='Update' />
            </form>
            <Button className="btn btn-dark" text="Go Back" onClick={onGoBackClick} />
        </div>
    )
}

export default UpdateProduct

