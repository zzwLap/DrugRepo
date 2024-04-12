import { request } from "@/utils/request"

export const userListService= ()=>{
    return request.get('/api/user')
}

export const userGetService=(id)=>{
    return request.get(`/api/user/${id}`)
}
export const userCreateService=(user)=>{
    return request.post('/api/user',user)
}

export const userUpdateService=(id,user)=>{
    return request.put(`/api/user/${id}`,user)
}

export const userDeleteService=(id)=>{
    return request.delete(`/api/user/${id}`)
}
