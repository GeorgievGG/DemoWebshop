import React from 'react'
import Product from './Product'

type Props = {
  products: CatalogProductInfo[]
  userRole: string
  onAddToCart: (productId: string) => void
  onDeleteClick: (productId: string) => void
}

const CatalogLine = ({ products, userRole, onAddToCart, onDeleteClick }: Props) => {
  return (
    <div className='row'>
        {
            products.map((product) => (
                <Product key={product.id} product={product} userRole={userRole} onAddToCart={onAddToCart} onDeleteClick={onDeleteClick} />
            ))}
    </div>
  )
}

export default CatalogLine