import { Link } from "react-router-dom";

const Footer = ({ onAboutClick, showAboutLink }) => {
  return (
    <footer>
        <p>Copyright &copy; 2022</p>
        {
          showAboutLink && <Link onClick={onAboutClick} to="/about">About</Link>
        }
    </footer>
  )
}

export default Footer