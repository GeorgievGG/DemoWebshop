import { toast } from "react-toastify"
import store from "../store"
import { flushPaymentState } from "../store/paymentSlice"
import { flushSessionData } from "../store/sessionSlice"

export const handleNegativeResponse = async (response: Response, errorMessage: string, useBackEndError: boolean = false) => {
    const body = await response.text()
    let backEndError = 'Unknown error'
    if (body && body !== '') {
        const data = JSON.parse(body)
        backEndError = data.message
    }

    if (response.status === 401) {
        logout()
    }

    useBackEndError ? toast.error(errorMessage + `: ${backEndError}`) : toast.error(errorMessage)
}

export const logout = () => {
    store.dispatch(flushSessionData())
    store.dispatch(flushPaymentState())
}