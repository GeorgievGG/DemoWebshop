import React, { useState } from 'react'
import "bootstrap/dist/css/bootstrap.css";
import "react-datepicker/dist/react-datepicker.css";
import { Routes, Route, useNavigate } from "react-router-dom";
import Footer from "./components/common/Footer";
import useScript from './hooks/UseScript';
import UpdateProduct from './components/product/UpdateProduct';
import ShoppingBasket from './components/shopping-basket/ShoppingBasket';
import Profile from './components/user/Profile';
import UserList from './components/user/UserList';
import CreateProduct from './components/product/CreateProduct';
import OrderList from './components/order/OrderList';
import About from './components/common/About';
import Header from './components/common/Header';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import CatalogPage from './pages/CatalogPage';

function App() {
  useScript('https://unpkg.com/react/umd/react.production.min.js');
  useScript('https://unpkg.com/react-bootstrap@next/dist/react-bootstrap.min.js');

  const navigate = useNavigate();
  const [showAboutLink, setShowAboutLink] = useState(true)
  const [userLogged, setUserLogged] = useState(false)
  const [token, setToken] = useState('')
  const [loggedUserId, setloggedUserId] = useState('')
  const [userRole, setUserRole] = useState('')
  const [products, setProducts] = useState<CatalogProductInfo[]>([])
  const navigateBack = () => navigate(-1)
  

  const refreshToken = async (refreshToken: string) => {
    const response = await fetch('https://localhost:7000/api/Authentication/RefreshToken', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(refreshToken)
    })

    if (response.ok) {
      const data = await response.json()
    }
    else {
      alert(`Refreshing token failed. You're being logged out!`)
      logout()
    }
  }
  
  const addProduct = (productJson: any) => {
    setProducts([...products, productJson])
  }
  
  const updateProduct = (updatedProduct: CatalogProductInfo) => {
    products.map((product) =>
        product.id === updatedProduct.id ? updatedProduct : product
      )
  }

  const logout = async () => {
    setToken('')
    setloggedUserId('')
    setUserRole('')
    setUserLogged(false)
    navigate("/")
    setProducts(products.filter((product) => product.availableQuantity !== 0))
  }

  const toggleAboutLinkStatus = () => {
    setShowAboutLink(!showAboutLink)
  }

  return (
      <div className="container">
        <Header userLogged={userLogged} 
                userRole={userRole}
                navigate={navigate}
                onLogoutClick={logout} />
        
        <div className='body-wrapper'>
          <Routes>
            <Route path='/' element={
                <CatalogPage />
              } />
            <Route path='/login' element=
              {
                <LoginPage />
              } />
            <Route path='/register' element=
              {
                <RegisterPage />
              } />
            <Route path='/profile' element=
              {
                <Profile navigate={navigate} token={token} />
              } />
            <Route path='/userList' element=
              {
                <UserList token={token} loggedUserId={loggedUserId} onGoBackClick={navigateBack} />
              } />
            <Route path='/createProduct' element=
              {
                <CreateProduct token={token} onProductCreate={addProduct} onGoBackClick={navigateBack} />
              } />
            <Route path='/updateProduct' element=
              {
                <UpdateProduct token={token} onProductUpdate={updateProduct} onGoBackClick={navigateBack} />
              } />
            <Route path='/shoppingBasket' element=
              {
                <ShoppingBasket token={token} navigateBack={navigateBack} />
              } />
            <Route path='/orderList' element=
              {
                <OrderList token={token} onGoBackClick={navigateBack} />
              } />
            <Route path='/about' element=
              {
                <About onGoBackClick={toggleAboutLinkStatus}/>
              } />
          </Routes>
        </div>
        <Footer onAboutClick={toggleAboutLinkStatus} showAboutLink={ showAboutLink } />
      </div>
  );
}

export default App;
