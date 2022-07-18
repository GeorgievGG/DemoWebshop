import { useState } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import Footer from "./components/Footer";
import About from "./components/About";

function App() {
  const [showAboutLink, setShowAboutLink] = useState(true)

  const toggleAboutLinkStatus = () => {
    setShowAboutLink(!showAboutLink)
  }

  return (
    <Router>
        <div className="container">
          <Header title='Hello from Demo Webshop!' />
          
          <Routes>
            <Route path='/' element={
              <>
                <div>Body</div>
              </>
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
