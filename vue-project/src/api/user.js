import { request } from "@/utils/request"

export const userListService=async ()=>{
    return request.get('/api/user')
}

export const userCreateService=(user)=>{
    return request.post('/api/user',user)
}

export const userDeleteService=(id)=>{
    return request.delete(`/api/user/${id}`)
}
