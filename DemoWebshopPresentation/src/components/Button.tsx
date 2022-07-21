import React, { MouseEventHandler } from 'react'

type Props = {
  text: string,
  onClick: MouseEventHandler
}

const Button = ({ text, onClick }: Props) => {
  return (
    <button className='btn btn-dark' onClick={onClick}>{text}</button>
  )
}

export default Button