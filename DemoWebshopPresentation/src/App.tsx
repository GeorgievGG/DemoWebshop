import React, { useState } from 'react'
import "bootstrap/dist/css/bootstrap.css";
import "react-datepicker/dist/react-datepicker.css";
import { Routes, Route } from "react-router-dom";
import Footer from "./components/common/Footer";
import useScript from './hooks/UseScript';
import About from './components/common/About';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import CatalogPage from './pages/CatalogPage';
import HeaderPage from './pages/HeaderPage';
import ProfilePage from './pages/ProfilePage';
import UserListPage from './pages/UserListPage';
import CreateProductPage from './pages/CreateProductPage';
import UpdateProductPage from './pages/UpdateProductPage';
import ShoppingBasketPage from './pages/ShoppingBasketPage';
import OrderListPage from './pages/OrderListPage';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function App() {
  useScript('https://unpkg.com/react/umd/react.production.min.js');
  useScript('https://unpkg.com/react-bootstrap@next/dist/react-bootstrap.min.js');

  const [showAboutLink, setShowAboutLink] = useState(true)

  const toggleAboutLinkStatus = () => {
    setShowAboutLink(!showAboutLink)
    toast.success("Success!")
    toast.error("Error!")
  }

  return (
      <div className="container">
        <HeaderPage />
        <ToastContainer theme='dark' />
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
                <ProfilePage />
              } />
            <Route path='/userList' element=
              {
                <UserListPage />
              } />
            <Route path='/createProduct' element=
              {
                <CreateProductPage />
              } />
            <Route path='/updateProduct' element=
              {
                <UpdateProductPage />
              } />
            <Route path='/shoppingBasket' element=
              {
                <ShoppingBasketPage />
              } />
            <Route path='/orderList' element=
              {
                <OrderListPage />
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
