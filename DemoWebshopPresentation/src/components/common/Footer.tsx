import React, { MouseEventHandler } from 'react'
import { Link } from "react-router-dom";


type Props = {
  onAboutClick: MouseEventHandler,
  showAboutLink: boolean
}

const Footer = ({ onAboutClick, showAboutLink } : Props) => {
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