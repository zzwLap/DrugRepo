import {request} from '@/utils/request'

export const loginService=(user)=>{
    return request.get(`/api/login?username=${user.username}&password=${user.password}`)
}