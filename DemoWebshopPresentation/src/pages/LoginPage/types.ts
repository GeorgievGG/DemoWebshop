export interface ILoginProps {
  onLogin: (input: IUserLoginInput) => void
}

export interface IUserLoginInput {
  username: string,
  password: string
}