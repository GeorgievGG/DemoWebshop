/* eslint-disable @typescript-eslint/no-unused-vars */
type BasicUserInfo = {
    username: string
}

type ListUserInfo = BasicUserInfo & {
    id: string
}

type RegistrationInput = {
  username: string
  email: string
  firstName: string
  lastName: string
  password: string
  confirmPassword: string
}

type UserInfo = BasicUserInfo & {
    id: string
    email: string
    firstName: string
    lastName: string
    isAdmin: boolean
}

type UpdateProfileInput = BasicUserInfo & {
    email: string
    firstName: string
    lastName: string
}

type UpdatePasswordInput = {
    currentPassword: string
    newPassword: string
    repeatNewPassword: string
}
