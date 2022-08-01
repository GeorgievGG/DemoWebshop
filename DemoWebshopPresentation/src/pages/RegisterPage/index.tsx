import React from "react"
import { useNavigate } from "react-router-dom";
import Register from "../../components/user/Register"
import { IRegistrationInput } from "./types";

const LoginPage = () => {
    const navigate = useNavigate();

    const register = async (userInput: IRegistrationInput) => {
        const response = await fetch('https://localhost:7000/api/User', {
          method: 'POST',
          headers: {
            'Content-type': 'application/json'
          },
          body: JSON.stringify(userInput)
        })
        
        const body = await response.text()
        if (response.ok) {
          const data = JSON.parse(body)
          alert(`User ${data.username} registered!`)
          navigate(-1)
        }
        else {
          let errorMessage = 'Unknown error'
          if (body && body !== '') {
            const data = JSON.parse(body)
            errorMessage = data.message
          }
          alert(`Registration failed for user ${userInput.username}: ${errorMessage}`)
        }
    }
  
    return (
      <Register onRegister={register} />
    )
  }
  
  export default LoginPage