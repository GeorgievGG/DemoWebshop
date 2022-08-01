export interface IRegisterProps {
    onRegister: (input: IRegistrationInput) => void
}

export interface IRegistrationInput {
    username: string
    email: string
    firstName: string
    lastName: string
    password: string
    confirmPassword: string
}