import { request } from "@/utils/request"

export const getAllDrugsService=()=>{
    return request.get('/api/drugs/getdrugs')
}
export const getDrugService=(id)=>{
    return request.get(`/api/drugs/getdrug?drugId=${id}`)
}
export const addDrugService=(drug)=>{
    return request.post('/api/drugs/adddrugs',drug)
}

export const updateDrugService=(id,drug)=>{
    return request.put(`/api/drugs/modifydruginfo/${id}`,drug)
}

export const deleteDrugService=(id)=>{
    return request.delete(`/api/drugs/deletedrug?drugId=${id}`)
}

export const batchAddDrugsService=(drugs)=>{
    return request.post('/api/drugs/batchadddrugs',drugs)
}

export const uploadExcelFileService=(file)=>{
    return request.postForm('/api/drugs/uploaddrugfilebatchadddrugs',{file:file})
}