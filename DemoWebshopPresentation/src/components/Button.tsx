import React, { MouseEventHandler } from 'react'

type Props = {
  className: string,
  text: string,
  onClick: MouseEventHandler
}

const Button = ({ className, text, onClick }: Props) => {
  return (
    <button className={className} onClick={onClick}>{text}</button>
  )
}

export default Button