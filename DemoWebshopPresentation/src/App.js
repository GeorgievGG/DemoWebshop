import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/Header";
import Footer from "./components/Footer";
import About from "./components/About";

function App() {
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
            <Route path='/about' element={<About />} />
          </Routes>
          <Footer />
        </div>
    </Router>
  );
}

export default App;
