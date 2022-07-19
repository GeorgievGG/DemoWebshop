import React, { MouseEventHandler } from 'react'
import { Link } from "react-router-dom";

type Props = {
  onGoBackClick: MouseEventHandler
}

const About = ({ onGoBackClick }: Props) => {
  return (
    <div>
        <h4>Version 1.0.0</h4>
        <Link onClick={onGoBackClick} to='/'>Go Back</Link>
    </div>
  )
}

export default About