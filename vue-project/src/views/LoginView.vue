<script lang="ts" setup>
import { reactive, ref } from 'vue'
import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
import { useRouter } from 'vue-router';

const router = useRouter()

const ruleFormRef = ref<FormInstance>()
const loginData = reactive({
  username: 'admin',
  password: '123456',
})

const rules = reactive<FormRules<typeof loginData>>({
  username: [
    { required: true, message: '用户名必填', trigger: 'blur' }
  ],
  password: [
    { required: true, message: '密码必填', trigger: 'blur' },
    { min: 6, max: 10, message: '密码必须是6-10个字符', trigger: 'blur' }
  ]
})

const submitForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  formEl.validate((valid) => {
    if (valid) {
      if (loginData.username === 'admin' && loginData.password === '123456') {
        router.push('/')
      } else {
        ElMessage.error('账号或密码错误')
      }
    } else {
      return false
    }
  })
}

</script>

<template>
  <div class="container">
    <el-form ref="ruleFormRef" class="form" :model="loginData" status-icon :rules="rules" label-width="auto">
      <el-form-item label="账号" prop="username">
        <el-input v-model="loginData.username" type="text" autocomplete="off" />
      </el-form-item>
      <el-form-item label="密码" prop="password">
        <el-input v-model="loginData.password" type="password" autocomplete="off" show-password="true" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm(ruleFormRef)">登录</el-button>
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