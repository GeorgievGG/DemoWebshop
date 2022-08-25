import { toast } from "react-toastify"

export const handleNegativeResponse = async (response: Response, errorMessage: string, useBackEndError: boolean = false) => {
    const body = await response.text()
    let backEndError = 'Unknown error'
    if (body && body !== '') {
        const data = JSON.parse(body)
        backEndError = data.message
    }

    useBackEndError ? toast.error(errorMessage + `: ${backEndError}`) : toast.error(errorMessage)
}