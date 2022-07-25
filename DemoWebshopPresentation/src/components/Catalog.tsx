import React, { useState, useEffect } from 'react'
import { Confirm } from 'react-admin';
import CatalogLine from './CatalogLine'

type Props = {
    token: string
    userRole: string
    products: CatalogProductInfo[]
    onProductsLoaded: (productsJson: any) => void
    onProductDelete: (productId: string) => void
}

function Catalog({ token, userRole, products, onProductsLoaded, onProductDelete }: Props) {
    const [open, setOpen] = useState(false);
    const [deletedProductId, setDeletedProductId] = useState('');

    useEffect(() => {
        fetch('https://localhost:7000/api/Product', {
                method: 'GET'
            })
            .then(response => handleGetProductsResponse(response))
        }, []
    )

    const handleGetProductsResponse = async (response: Response) => {
        if (response.ok) {
            const data = await response.json()
            onProductsLoaded(data)
        }
        else {
          alert(`Couldn't retrieve products!`)
        }
    }
    
    const chunk = <T,>(arr: T[], size: number) =>
        Array.from({ length: Math.ceil(arr.length / size) }, (_v, i) =>
            arr.slice(i * size, i * size + size)
    )
    
    const handleConfirm = async () => {
        const response = await fetch(`https://localhost:7000/api/Product/${deletedProductId}`, {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })

        if (response.ok) {
            onProductDelete(deletedProductId)
        }
        else {
          alert(`Couldn't delete product ${deletedProductId}!`)
        }

        setOpen(false);
        setDeletedProductId('');
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
                        <CatalogLine key={index} products={productsChunk} userRole={userRole} onDeleteClick={openConfirmDialog} />
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