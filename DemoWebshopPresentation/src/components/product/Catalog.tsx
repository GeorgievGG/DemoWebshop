import React, { useState, useEffect } from 'react'
import { Confirm } from 'react-admin'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { selectPaymentState, selectProductsState, selectSessionState } from '../../store'
import { flushPaymentState, setPaymentState } from '../../store/paymentSlice'
import { deleteProduct, setProducts } from '../../store/productsSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import { createOrderCall, handleNegativeResponse } from '../../utility'
import CatalogLine from './CatalogLine'
import RiseLoader from "react-spinners/RiseLoader";

function Catalog() {
    const dispatch = useDispatch()
    const navigate = useNavigate()
    const [hasLoaded, setHasLoaded] = useState(false)

    const [open, setOpen] = useState(false)
    const [deletedProductId, setDeletedProductId] = useState('')
    const sessionState = useSelector<RootState, IUserSessionData>(selectSessionState)
    const products = useSelector<RootState, CatalogProductInfo[]>(selectProductsState)
    const paymentState = useSelector<RootState, IPaymentState>(selectPaymentState)

    useEffect(() => {
        fetch('https://localhost:7000/api/Product', {
                method: 'GET'
            })
            .then(response => handleGetProductsResponse(response))
        }, []
    )

    useEffect(() => {
        if (hasLoaded && paymentState && paymentState.subscriptionAdded && paymentState.subscriptionId) {
            toast.success('Subscription added')
            var subscription = [{quantity: 1, price: paymentState.orderAmount, productId: paymentState.subscriptionId}] as OrderLine[]
            createOrderCall(subscription, sessionState.Token)
            .then(response => handleCreateOrderResponse(response))

        }
        }, [hasLoaded]
    )

    const handleGetProductsResponse = async (response: Response) => {
        if (response.ok) {
            const productsJson = await response.json()
            if (sessionState.LoggedUserRole === 'Admin') {
                dispatch(setProducts(productsJson))
            }
            else {
                dispatch(setProducts(productsJson.filter((product: CatalogProductInfo) => product.availableQuantity !== 0)))
            }

            setHasLoaded(true)
        }
        else {
            handleNegativeResponse(response, "Couldn't retrieve products!", null, false)
        }
    }
    
    const handleCreateOrderResponse = async (response: Response) => {
        if (response.ok) {
            toast.success(`Order created successfully!`)
            dispatch(flushPaymentState())
        }
        else {
            handleNegativeResponse(response, "Creating order failed", null, true)
        }
    }
    
    const chunk = <T,>(arr: T[], size: number) =>
        Array.from({ length: Math.ceil(arr.length / size) }, (_v, i) =>
            arr.slice(i * size, i * size + size)
    )

    const addToCart = async (productId: string) => {
        const response = await fetch(`https://localhost:7000/api/ShoppingBasket/IncreaseShoppingQuantity/${productId}`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            }
        })
        
        if (response.ok) {
            toast.success('Added to basket!')
        }
        else {
            handleNegativeResponse(response, "Adding to basket failed", null, true)
        }
    }

    const navigateToSubscriptionPage = (subscriptionId: string, subscriptionFee: number, pageAddress: string) => {
        dispatch(setPaymentState({ orderAmount: subscriptionFee, currency: 'EUR', subscriptionId: subscriptionId }))
        navigate(pageAddress)
    }
    
    const handleConfirm = async () => {
        const response = await fetch(`https://localhost:7000/api/Product/${deletedProductId}`, {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${sessionState.Token}`
            }
        })

        if (response.ok) {
            deleteProduct(deletedProductId)
        }
        else {
            handleNegativeResponse(response, `Couldn't delete product ${deletedProductId}`, null, true)
        }

        setOpen(false)
        setDeletedProductId('')
    }

    const openConfirmDialog = (productId: string) => { 
        setOpen(true)
        setDeletedProductId(productId)
    }

    const handleDialogClose = () => { 
        setOpen(false)
        setDeletedProductId('')
    }
  
    return (
        <>
            <Confirm
                isOpen={open}
                title="Delete product"
                content="Are you sure you want to delete this item?"
                onConfirm={handleConfirm}
                onClose={handleDialogClose}
                confirm="Yes"
                cancel="No"
            />
            {
                hasLoaded ?
                    products.length > 0 ?
                    chunk(products, 4).map((productsChunk, index) => {
                        return (
                            <CatalogLine key={index} products={productsChunk} userRole={sessionState.LoggedUserRole} onAddToCart={addToCart} onSubscribe={navigateToSubscriptionPage} onDeleteClick={openConfirmDialog} />
                        )
                    }) :
                    (
                      'No products to Show'
                    ) :
                <RiseLoader className='loader' color={'black'} size={15} />
            }
        </>
    )
}

export default Catalog