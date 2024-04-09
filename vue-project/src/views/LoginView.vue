<script lang="ts" setup>
import { ref } from 'vue'
import { ElMessage } from 'element-plus'
import { useRouter } from 'vue-router';
import { loginService } from '@/api/login';
import { tokenStore } from '@/utils/request';

const router = useRouter()

const loginFormRef = ref()
const loginData = ref({
  username: '',
  password: '',
})

const rules = {
  username: [
    { required: true, message: '用户名必填', trigger: 'blur' }
  ],
  password: [
    { required: true, message: '密码必填', trigger: 'blur' },
    { min: 6, max: 10, message: '密码必须是6-10个字符', trigger: 'blur' }
  ]
}

const loginSubmit = async () => {
  await loginFormRef.value.validate()
  const token = await loginService(loginData.value)
  tokenStore.token = token
  router.push('/')
}

</script>

<template>
  <div class="container">
    <el-form ref="loginFormRef" class="form" :model="loginData" status-icon :rules="rules" label-width="auto">
      <el-form-item label="账号" prop="username">
        <el-input v-model="loginData.username" type="text" autocomplete="off" />
      </el-form-item>
      <el-form-item label="密码" prop="password">
        <el-input v-model="loginData.password" type="password" autocomplete="off" show-password="true" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="loginSubmit()">登录</el-button>
      </el-form-item>
    </el-form>

  </div>
</template>

<style scoped>
.container {
  width: 100%;
  height: 100%;
  display: flex;
}

.form {
  margin: auto;
  width: 300px;
}
</style>