<script lang="ts" setup>
import { RouterView } from 'vue-router'
import {
  Document,
  Menu as IconMenu,
  Setting,
  ArrowDown
} from '@element-plus/icons-vue'
import { tokenStore } from '@/utils/request';
import router from '@/router';

const logout = () => {
  tokenStore.token = ''
  router.push('/login')
}
</script>


<template>
  <el-container style="height: 100%;">
    <el-aside class="nav">
      <h1>药品管理系统</h1>
      <el-menu active-text-color="#ffd04b" background-color="#545c64" default-active="/authorization/user-manage"
        router="true" text-color="#fff">
        <el-sub-menu index="/settings">
          <template #title>
            <el-icon>
              <setting />
            </el-icon>
            <span>系统设置</span>
          </template>
          <el-menu-item index="/settings/dictionary-setting">字典设置</el-menu-item>
        </el-sub-menu>
        <el-sub-menu index="/authorization">
          <template #title>
            <el-icon>
              <document />
            </el-icon>
            <span>权限管理</span>
          </template>
          <el-menu-item index="/authorization/user-manage">用户管理</el-menu-item>
          <el-menu-item index="/authorization/role-manage">角色管理</el-menu-item>
          <el-menu-item index="/authorization/authorization-manage">权限管理</el-menu-item>
        </el-sub-menu>
        <el-sub-menu index="3">
          <template #title>
            <el-icon>
              <icon-menu />
            </el-icon>
            <span>药品管理</span>
          </template>
          <el-menu-item index="/drug/drug-manage">药品信息</el-menu-item>
          <el-menu-item index="3-2">库存管理</el-menu-item>
          <el-menu-item index="3-3">调拨管理</el-menu-item>
          <el-menu-item index="3-4">损益管理</el-menu-item>
          <el-menu-item index="3-5">盘点管理</el-menu-item>
          <el-menu-item index="3-6">入库管理</el-menu-item>
          <el-menu-item index="3-7">出库管理</el-menu-item>
          <el-menu-item index="3-8">药品变更记录</el-menu-item>
        </el-sub-menu>
      </el-menu>
    </el-aside>
    <el-container>
      <el-header>
        <el-dropdown>
          <span>
            <el-avatar src="https://cube.elemecdn.com/0/88/03b0d39583f48206768a7534e55bcpng.png" />
            <el-icon>
              <arrow-down />
            </el-icon>
          </span>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item>个人信息</el-dropdown-item>
              <el-dropdown-item @click="logout">退出登录</el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </el-header>
      <el-main>
        <RouterView class="router"></RouterView>
      </el-main>
    </el-container>
  </el-container>
</template>

<style scoped>
.nav {
  width: 250px;
  background-color: #545c64;

  h1 {
    height: 45px;
    color: white;
    font-size: 25px;
    margin: 10px 15px;
  }

  .el-menu {
    border: unset;
  }
}
</style>
