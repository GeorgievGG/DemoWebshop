import React, { useState, useEffect } from 'react'
import { Confirm } from 'react-admin'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { selectPaymentState, selectProductsState, selectSessionState } from '../../store'
import { flushPaymentState, setPaymentState } from '../../store/paymentSlice'
import { deleteProduct, setProducts } from '../../store/productsSlice'
import { IPaymentState, IUserSessionData, RootState } from '../../store/types'
import { handleNegativeResponse } from '../../utility'
import CatalogLine from './CatalogLine'

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
        if (hasLoaded && paymentState && paymentState.subscriptionAdded) {
            toast.success('Subscription added')
            dispatch(flushPaymentState())
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
          toast.error("Couldn't retrieve products!")
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
            handleNegativeResponse(response, "Adding to basket failed", true)
        }
    }

    const navigateToSubscriptionPage = async (subscriptionFee: number, pageAddress: string) => {
        dispatch(setPaymentState({ orderAmount: subscriptionFee, currency: 'EUR' }))
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
          toast.error(`Couldn't delete product ${deletedProductId}!`)
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
                products.length > 0 ?
                chunk(products, 4).map((productsChunk, index) => {
                    return (
                        <CatalogLine key={index} products={productsChunk} userRole={sessionState.LoggedUserRole} onAddToCart={addToCart} onSubscribe={navigateToSubscriptionPage} onDeleteClick={openConfirmDialog} />
                    )
                }) :
                (
                  'No products to Show'
                )
            }
        </>
    )
}

export default Catalog