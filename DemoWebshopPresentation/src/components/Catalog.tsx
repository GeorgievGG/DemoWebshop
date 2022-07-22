import React, { useState, useEffect } from 'react'
import { Confirm } from 'react-admin';
import CatalogLine from './CatalogLine'

type Props = {
    token: string
    userRole: string
}

function Catalog({ token, userRole }: Props) {
    const [products, setProducts] = useState<ProductInfo[]>([])
    const [open, setOpen] = useState(false);
    const [deletedProductId, setDeletedProductId] = useState('');

    useEffect(() => {
        fetch('https://localhost:7000/api/Product', {
                method: 'GET'
            })
            .then(responseponse => handleGetProductsResponse(responseponse))
        }, []
    )

    const handleGetProductsResponse = async (responseponse: Response) => {
        if (responseponse.ok) {
            const data = await responseponse.json()
            setProducts(data)
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
        const response = await fetch(`https://localhost:7000/api/User/${deletedProductId}`, {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })

        if (response.ok) {
            setProducts(products.filter((product) => product.id !== deletedProductId))
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

    const handleDialogClose = () => setOpen(false);
  
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