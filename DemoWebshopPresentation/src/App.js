import { useState } from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Header from "./components/Header";
import Footer from "./components/Footer";
import About from "./components/About";
import Login from "./components/Login";
import Register from "./components/Register";

function App() {
  const [showAboutLink, setShowAboutLink] = useState(true)

  const toggleAboutLinkStatus = () => {
    setShowAboutLink(!showAboutLink)
  }

  const login = async (userCredentials) => {
    const res = await fetch('https://localhost:7000/api/Authentication/Login', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(userCredentials)
    })
    const data = await res.json()
  }

  const register = async (userInput) => {
    const res = await fetch('https://localhost:7000/api/User', {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(userInput)
    })
    const data = await res.json()
  }

  return (
    <Router>
        <div className="container">
          <Header title='Hello from Demo Webshop!' />
          
          <Routes>
            <Route path='/' element={
              <>
                <div>Body</div>
                <Link to="/login">Login</Link>
                <Link to="/register">Register</Link>
              </>
              } />
            <Route path='/login' element=
              {
                <Login onLogin={login} />
              } />
            <Route path='/register' element=
              {
                <Register onRegister={register} />
              } />
            <Route path='/about' element=
              {
                <About onGoBackClick={toggleAboutLinkStatus}/>
              } />
          </Routes>
          <Footer onAboutClick={toggleAboutLinkStatus} showAboutLink={ showAboutLink } />
        </div>
    </Router>
  );
}

export default App;
