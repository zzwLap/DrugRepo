import { request } from "@/utils/request"

export const getAllCategory=()=>{
    return request.post('/api/datadictionary/getallhierachydictionary')
}
export const getAllDict=()=>{
    return request.get('/api/datadictionary/getalldict')
}

