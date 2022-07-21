import React, { useState, useEffect } from 'react'

type Props = {
    token: string
}

function Catalog({ token }: Props) {
    const [products, setProducts] = useState([])

    useEffect(() => {
        fetch('https://localhost:7000/api/Product', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
            .then(response => handleGetProductsResponse(response))
        }, []
    )

    const handleGetProductsResponse = async (response: Response) => {
        if (response.ok) {
            const data = await response.json()
        }
        else {
          alert(`Couldn't retrieve products!`)
        }
    }

    return (
        <>
            {
                products.length > 0 ? (
                    <div>Products</div>
                ) :
                (
                    <div className='row'>
                        <div className='col-sm-3'>'No products to show'</div>
                        <div className='col-sm-3'>'No products to show'</div>
                        <div className='col-sm-3'>'No products to show'</div>
                        <div className='col-sm-3'>'No products to show'</div>
                    </div>
                )
            }
        </>
    )
}

export default Catalog