import { request } from "@/utils/request"

export const getAllCategory=()=>{
    return request.get('/api/datadictionary/gethierachydictionary?categoryName=普通字典')
}
export const getAllDict=(name)=>{
    return request.get(`/api/datadictionary/getdictionaries?categoryName=${name}`)
}

