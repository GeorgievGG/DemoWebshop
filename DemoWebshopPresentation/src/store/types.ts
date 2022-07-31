import store from "."

export interface IUserSessionData {
    Token: string
    UserLogged: boolean
    LoggedUserId: string
    LoggedUserRole: string
}

export interface UserSessionSliceState {
    userSession: IUserSessionData
}

export type RootState = ReturnType<typeof store.getState>