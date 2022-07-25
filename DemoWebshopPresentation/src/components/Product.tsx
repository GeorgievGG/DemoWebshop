import React from 'react'
import { useNavigate } from 'react-router-dom'
import Button from './Button'

type Props = {
  product: CatalogProductInfo
  userRole: string
  onDeleteClick: (productId: string) => void
}

const Product = ({ product, userRole, onDeleteClick }: Props) => {
  const navigate = useNavigate();

  const addToCart = () => {
    if (userRole !== "User") {
      navigate("/login")
    }
  
  }

  return (
    <div className='col-sm-3'>
      <div className='card'>
        <img className='pic' src={product.pictureUrl} alt={`${product.name} (${product.model})`}/>
        <h1>{product.name}</h1>
        <p>{product.model}</p>
        <p className="price">${ product.price.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}</p>
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
            <Button className="btn btn-dark single-button" text={"Add to cart"} onClick={addToCart} />
          </p>
        }
      </div>
    </div>
  )
}

export default Product

