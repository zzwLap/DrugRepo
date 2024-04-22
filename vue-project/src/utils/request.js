import router from '@/router'
import axios from 'axios'
import { ElMessage } from 'element-plus'

export const tokenStore = {
    get token() {
        return localStorage.getItem('token')
    },
    set token(val) {
        localStorage.setItem('token', val)
    }
}

export const request = axios.create({
    baseURL: '/proxy',
    timeout: 10000
})

request.interceptors.request.use(request => {
    if (tokenStore.token) {
        request.headers.Authorization = `Bearer ${tokenStore.token}`
    }
    return request
})

request.interceptors.response.use(response => {
    return response.data
}, error => {
    const response = error.response
    if (response) {
        if (response.status === 401) {
            ElMessage.error('请先登录')
            router.push('/login')
        } else {
            ElMessage.error(response.data)
        }
    } else {
        ElMessage.error('网络连接失败')
    }
    return Promise.reject(error)
})