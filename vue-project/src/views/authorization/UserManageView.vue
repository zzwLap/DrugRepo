<template>
    <el-card>
        <el-button type="primary" @click="createFormVisible = true">新增用户</el-button>
        <el-table :data="userList" stripe border style="width: 100%">
            <el-table-column prop="id" label="Id" />
            <el-table-column prop="userName" label="姓名" />
            <el-table-column prop="userId" label="账号" />
            <el-table-column prop="password" label="密码" />
            <el-table-column prop="sex" label="性别" />
            <el-table-column prop="age" label="年龄" />
            <el-table-column label="操作">
                <template #default="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">修改</el-button>
                    <el-button size="small" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
    </el-card>

    <el-dialog v-model="createFormVisible" title="新增用户" width="500">
        <el-form ref="createFormRef" :model="userModel" :rules="rules" label-width="auto">
            <el-form-item label="姓名" prop="userName">
                <el-input v-model="userModel.userName" autocomplete="off" />
            </el-form-item>
            <el-form-item label="登录账号" prop="userId">
                <el-input v-model="userModel.userId" autocomplete="off" />
            </el-form-item>
            <el-form-item label="密码" prop="password">
                <el-input v-model="userModel.password" type="password" autocomplete="off" />
            </el-form-item>
            <el-form-item label="性别" prop="sex">
                <el-select v-model="userModel.sex" placeholder="请选择性别">
                    <el-option label="男" value="男" />
                    <el-option label="女" value="女" />
                </el-select>
            </el-form-item>
            <el-form-item label="年龄" prop="age">
                <el-input v-model="userModel.age" type="number" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <div class="dialog-footer">
                <el-button @click="createFormVisible = false; createFormRef.resetFields()">取消</el-button>
                <el-button type="primary" @click="handleUserCreate">确认</el-button>
            </div>
        </template>
    </el-dialog>

</template>

<script lang="ts" setup>
import { userCreateService, userDeleteService, userListService } from '@/api/user';
import { ElMessage, ElMessageBox } from 'element-plus';
import { ref } from 'vue';

const createFormRef = ref()
const createFormVisible = ref(false)
const userModel = ref({
    userId: '',
    userName: '',
    password: '',
    sex: '',
    age: null
})
const rules = {
    userId: [{ required: true, min: 3, message: '账号最少3个字符', trigger: 'blur' }],
    userName: [{ required: true, message: '姓名必填', trigger: 'blur' }],
    password: [{ required: true, min: 6, message: '密码最少是6个字符', trigger: 'blur' }],
}

const userList = ref()
const getUserList = async () => {
    const list = await userListService()
    userList.value = list
}
getUserList()

const handleUserCreate = async () => {
    await createFormRef.value.validate()

    await userCreateService(userModel.value)
    ElMessage.success('用户添加成功')

    createFormVisible.value = false
    createFormRef.value.resetFields()

    await getUserList()
}

const handleEdit = (index, row) => {
    console.log(index, row)
}
const handleDelete = async (index, row) => {
    await ElMessageBox.confirm(
        '确认删除数据',
        '提示',
        {
            confirmButtonText: '确认',
            cancelButtonText: '取消',
            type: 'warning'
        }
    )

    await userDeleteService(row.id)
    await getUserList()
}
</script>