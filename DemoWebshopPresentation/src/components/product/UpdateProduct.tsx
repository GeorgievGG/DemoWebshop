import React, { FormEventHandler, useEffect } from 'react'
import { useState } from "react"
import { useDispatch, useSelector } from 'react-redux'
import { useLocation, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { selectSessionState } from '../../store'
import { updateProduct } from '../../store/productsSlice'
import { IUserSessionData, RootState } from '../../store/types'
import { handleNegativeResponse } from '../../utility'
import Button from '../common/Button'

const UpdateProduct = () => {
    const [name, setName] = useState('')
    const [pictureUrl, setPictureUrl] = useState('')
    const [model, setModel] = useState('')
    const [availableQuantity, setAvailableQuantity] = useState(0)
    const [price, setPrice] = useState(0.00)
    const [isSubscription, setIsSubscription] = useState(false)

    const location = useLocation()
    const dispatch = useDispatch()
    const navigate = useNavigate()
    const state = location.state as { product: CatalogProductInfo }
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    
    useEffect(() => {
            setName(state.product.name)
            setPictureUrl(state.product.pictureUrl)
            setModel(state.product.model)
            setAvailableQuantity(state.product.availableQuantity)
            setPrice(state.product.price)
            setIsSubscription(state.product.isSubscription)
        }, [state]
    )


    const update = async (userInput: FormProductInfo) => {
        const response = await fetch(`${process.env.API_URL}/api/Product/${state.product.id}`, {
        method: 'PUT',
        headers: {
            'Content-type': 'application/json',
            'Authorization': `Bearer ${sessionState.Token}`
        },
        body: JSON.stringify(userInput)
        })
        
        if (response.ok) {
            const updatedProduct = { ...userInput, id: state.product.id } as CatalogProductInfo
            dispatch(updateProduct(updatedProduct))
            toast.success(`Product ${updatedProduct.name} updated!`)
        }
        else {
            handleNegativeResponse(response, `Update failed for product ${userInput.name}`, null, true)
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
            case !availableQuantity && availableQuantity !== 0:
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

        update({ name, pictureUrl, model, availableQuantity, price, isSubscription })
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
                <div className="form-control border-0 checkboxContainer">
                    <label>
                        <input id='tokenizeCheckbox' 
                            type='checkbox' 
                            checked={isSubscription}
                            onChange={(e) => setIsSubscription(e.currentTarget.checked)} />
                            Is the product subscription-based?
                    </label>
                </div>
                <input className="btn btn-dark" type='submit' value='Update' />
            </form>
            <Button className="btn btn-dark" text="Go Back" onClick={() => navigate(-1)} />
        </div>
    )
}

export default UpdateProduct

