import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import UserManageView from '@/views/authorization/UserManageView.vue'
import RoleManageView from '@/views/authorization/RoleManageView.vue'
import AuthorizationManageView from '@/views/authorization/AuthorizationManageView.vue'
import DictionarySettingView from '@/views/settings/DictionarySettingView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomeView,
      redirect:'/authorization/user-manage',
      children:[
        {path:'/settings/dictionary-setting',component:DictionarySettingView},
        {path:'/authorization/user-manage',component:UserManageView},
        {path:'/authorization/role-manage',component:RoleManageView},
        {path:'/authorization/authorization-manage',component:AuthorizationManageView},
      ]
    },
    {
      path:'/login',
      component:LoginView
    }
  ]
})

export default router
