import { useState } from "react"

const Register = ({onRegister}) => {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')

  const onSubmit = (e) => {
    e.preventDefault()

    if (!username) {
        alert('Please type in username!')
        return
    }

    if (!email) {
        alert('Please type in username!')
        return
    }
    
    if (!password) {
        alert('Please type in password!')
        return
    }
    
    if (!confirmPassword) {
        alert('Please type in password!')
        return
    }

    onRegister({ username, email, firstName, lastName, password, confirmPassword })
    flushForm()
  }
  
  const flushForm = () => {
      setUsername('')
      setEmail('')
      setFirstName('')
      setLastName('')
      setPassword('')
      setConfirmPassword('')
  }

  return (
    <form className="add-form"
          onSubmit={onSubmit}>
        <div className="form-control">
            <label>Username</label>
            <input type='text' 
                   placeholder='Type username'
                   value={username}
                   onChange={(e) => setUsername(e.target.value)} />
        </div>
        <div className="form-control">
            <label>Email</label>
            <input type='text' 
                   placeholder='Type email'
                   value={email}
                   onChange={(e) => setEmail(e.target.value)} />
        </div>
        <div className="form-control">
            <label>First Name</label>
            <input type='text' 
                   placeholder='Type first name'
                   value={firstName}
                   onChange={(e) => setFirstName(e.target.value)} />
        </div>
        <div className="form-control">
            <label>Last Name</label>
            <input type='text' 
                   placeholder='Type last name'
                   value={lastName}
                   onChange={(e) => setLastName(e.target.value)} />
        </div>
        <div className="form-control">
            <label>Password</label>
            <input type='password' 
                   value={password}
                   onChange={(e) => setPassword(e.target.value)} />
        </div>
        <div className="form-control">
            <label>Confirm Password</label>
            <input type='password' 
                   value={confirmPassword}
                   onChange={(e) => setConfirmPassword(e.target.value)} />
        </div>
        <input className="btn btn-block" type='submit' value='Register' />
    </form>
  )
}

export default Register

