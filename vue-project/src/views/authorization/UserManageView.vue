<template>
    <el-card>
        <el-button type="primary">新增用户</el-button>
        <el-table :data="tableData" stripe border style="width: 100%">
            <el-table-column prop="id" label="Id" />
            <el-table-column prop="name" label="姓名" />
            <el-table-column prop="sex" label="性别" />
            <el-table-column prop="birthday" label="生日" />
            <el-table-column prop="address" label="地址" />
            <el-table-column label="操作">
                <template #default="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">Edit</el-button>
                    <el-button size="small" type="danger"
                        @click="handleDelete(scope.$index, scope.row)">Delete</el-button>
                </template>
            </el-table-column>
        </el-table>
    </el-card>

</template>

<script lang="ts" setup>
const tableData = [
    {
        id: 1,
        name: '小明',
        sex: '男',
        birthday: '2016-05-03',
        address: '江苏省南京市',
    },
    {
        id: 1,
        name: '小明',
        sex: '男',
        birthday: '2016-05-03',
        address: '江苏省南京市',
    },
    {
        id: 1,
        name: '小明',
        sex: '男',
        birthday: '2016-05-03',
        address: '江苏省南京市',
    },
    {
        id: 1,
        name: '小明',
        sex: '男',
        birthday: '2016-05-03',
        address: '江苏省南京市',
    },
]

const handleEdit = (index: number, row: User) => {
    console.log(index, row)
}
const handleDelete = (index: number, row: User) => {
    console.log(index, row)
}
</script>