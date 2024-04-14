<template>
    <el-card>
        <template #header>
            <el-button type="primary" @click="mode = 1; formVisible = true">添加字典</el-button>
            <el-button type="primary" @click="refreshData">刷新</el-button>
        </template>

        <el-row>
            <el-col :span="4">
                <el-tree :data="treeData" :props="defaultProps" @node-click="handleNodeClick" />
            </el-col>
            <el-col :span="20">
                <el-table :data="dataList" stripe border>
                    <el-table-column prop="id" label="Id" />
                    <el-table-column prop="categoryName" label="类别" />
                    <el-table-column prop="displayName" label="名称" />
                    <el-table-column prop="value" label="值" />
                    <el-table-column label="操作">
                        <template #default="scope">
                            <el-button size="small" @click="handleEdit(scope.$index, scope.row)">修改</el-button>
                            <el-button size="small" type="danger"
                                @click="handleDelete(scope.$index, scope.row)">删除</el-button>
                        </template>
                    </el-table-column>
                </el-table>

            </el-col>
        </el-row>

    </el-card>

    <el-dialog v-model="formVisible" title="添加字典" width="500">
        <el-form ref="formRef" :model="dataModel" :rules="rules" label-width="auto">
            <el-form-item label="名称" prop="displayName">
                <el-input v-model="dataModel.displayName" autocomplete="off" />
            </el-form-item>
            <el-form-item label="值" prop="value">
                <el-input v-model="dataModel.value" autocomplete="off" />
            </el-form-item>
            <el-form-item label="启用">
                <el-switch v-model="dataModel.enable" />
            </el-form-item>
        </el-form>
        <template #footer>
            <div class="dialog-footer">
                <el-button @click="formVisible = false; formRef.resetFields()">取消</el-button>
                <el-button type="primary" @click="handleCreate">确认</el-button>
            </div>
        </template>
    </el-dialog>
</template>
<style scoped>
.el-card {
    height: 100%;
    box-sizing: border-box;
}
</style>

<script setup>
import { getAllCategory, getAllDict } from '@/api/dict';
import { userCreateService, userDeleteService, userGetService, userListService, userUpdateService } from '@/api/user';
import { ElMessage, ElMessageBox } from 'element-plus';
import { ref } from 'vue';

const treeData = ref()
const defaultProps = {
    children: 'children',
    label: 'displayName',
}

let selectedNode
const handleNodeClick = async (data) => {
    selectedNode = data.displayName
    await getList()
}

const getTreeList = async () => {
    /** @type {[]} */
    const list = await getAllCategory()
    for (const item of list) {
        item.children = list.filter(x => x.parentId === item.id)
    }
    const ps = list.filter(x => x.parentId === 0)
    treeData.value = ps
}
getTreeList()

const formRef = ref()
const formVisible = ref(false)
const dataModel = ref({
    displayName: '',
    value: '',
    enable: true
})
const rules = {
    displayName: [{ required: true, message: '名称不能为空', trigger: 'blur' }],
    value: [{ required: true, message: '值不能为空', trigger: 'blur' }],
}

const dataList = ref()
const getList = async () => {
    dataList.value = await getAllDict(selectedNode)
}
getList()

const refreshData = async () => {
    await getList()
    ElMessage.success('刷新成功')
}

let mode // 0 查看; 1 创建; 2 修改
const handleCreate = async () => {
    await formRef.value.validate()

    if (mode === 1) {
        await userCreateService(dataModel.value)
        ElMessage.success('用户添加成功')
    } else if (mode === 2) {
        await userUpdateService(dataModel.value.id, dataModel.value)
        ElMessage.success('用户修改成功')
    }

    formVisible.value = false
    formRef.value.resetFields()

    await getList()
}

const handleEdit = async (index, row) => {
    dataModel.value = await userGetService(row.id)
    mode = 2
    formVisible.value = true
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
    await getList()
}
</script>