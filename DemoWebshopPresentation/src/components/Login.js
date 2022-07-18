import { useState } from "react"

const Login = ({onLogin}) => {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')

  const onSubmit = (e) => {
    e.preventDefault()

    if (!username) {
        alert('Please type in username!')
        return
    }
    
    if (!password) {
        alert('Please type in password!')
        return
    }

    onLogin({ username, password })
    setUsername('')
    setPassword('')
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
            <label>Password</label>
            <input type='password' 
                   value={password}
                   onChange={(e) => setPassword(e.target.value)} />
        </div>
        <input className="btn btn-block" type='submit' value='Login' />
    </form>
  )
}

export default Login