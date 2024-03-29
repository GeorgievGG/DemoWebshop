import React from 'react'
import { useNavigate } from 'react-router-dom'
import Button from '../common/Button'

type Props = {
  product: CatalogProductInfo
  userRole: string
  onAddToCart: (productId: string) => void
  onSubscribe: (subscriptionId: string, subscriptionFee: number, pageAddress: string) => void
  onDeleteClick: (productId: string) => void
}

const Product = ({ product, userRole, onAddToCart, onSubscribe, onDeleteClick }: Props) => {
  const navigate = useNavigate();

  const addToCart = async () => {
    if (userRole !== "User") {
      navigate("/login")
      return
    }
    
    onAddToCart(product.id)
  }

  const subscribe = async () => {
    if (userRole !== "User") {
      navigate("/login")
      return
    }
    
    onSubscribe(product.id, product.price, '/subscription')
  }

  return (
    <div className='col-sm-3'>
      <div className='card'>
        <img className='pic' src={product.pictureUrl} alt={`${product.name} (${product.model})`}/>
        <h1>{product.name}</h1>
        <p>{product.model}</p>
        <p className="price">
          {
            !product.isSubscription ?
              product.price.toLocaleString('de-DE', { style: 'currency', currency: 'EUR'}) :
              product.price.toLocaleString('de-DE', { style: 'currency', currency: 'EUR'}) + '/Month'
          }
          </p>
        {
          userRole === "Admin" ?
          <>
            <div className='row product-card-buttons-row'>
              <div className='col-sm-6 product-card-columns'>
                <Button className="btn btn-dark" text={"Update"} onClick={() => navigate("/updateProduct", { state: { product: product } })} />
              </div>
              <div className='col-sm-6 product-card-columns'>
                <Button className="btn btn-dark" text={"Delete"} onClick={() => onDeleteClick(product.id)} />
              </div>
            </div>
          </> :
          <p className="p-button">
            {
              !product.isSubscription ?
                <Button className="btn btn-dark single-button" text={"Add to cart"} onClick={addToCart} /> :
                <Button className="btn btn-dark single-button" text={"Subscribe (Batch)"} onClick={subscribe} />
            }
          </p>
        }
      </div>
    </div>
  )
}

export default Product

