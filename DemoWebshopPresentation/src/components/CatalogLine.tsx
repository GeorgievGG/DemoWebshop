import React from 'react'
import Product from './Product'

type Props = {
  products: ProductInfo[]
  userRole: string
  onDeleteClick: (productId: string) => void
}

const CatalogLine = ({ products, userRole, onDeleteClick }: Props) => {
  return (
    <div className='row'>
        {
            products.map((product) => (
                <Product key={product.id} product={product} userRole={userRole} onDeleteClick={onDeleteClick} />
            ))}
    </div>
  )
}

export default CatalogLine