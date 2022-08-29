import React from 'react'
import Product from './Product'

type Props = {
  products: CatalogProductInfo[]
  userRole: string
  onAddToCart: (productId: string) => void
  onSubscribe: (subsctiptionId: string, subscriptionFee: number, pageAddress: string) => void
  onDeleteClick: (productId: string) => void
}

const CatalogLine = ({ products, userRole, onAddToCart, onSubscribe, onDeleteClick }: Props) => {
  return (
    <div className='row'>
        {
            products.map((product) => (
                <Product key={product.id} product={product} userRole={userRole} onAddToCart={onAddToCart} onSubscribe={onSubscribe} onDeleteClick={onDeleteClick} />
            ))}
    </div>
  )
}

export default CatalogLine