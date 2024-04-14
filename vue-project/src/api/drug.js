import { request } from "@/utils/request"

export const getAllDrugsService=()=>{
    return request.get('/api/drugs/getdrugs')
}
export const getDrugService=(id)=>{
    
}
export const addDrugService=(drug)=>{
    return request.post('/api/drugs/adddrugs',drug)
}

export const updateDrugService=(id,drug)=>{
    return request.post('/api/drugs/modifydruginfo',drug)
}

export const deleteDrugService=(id)=>{

}