<template>
    <el-card style="height: 100%;box-sizing:border-box;">
        <template #header>
            <el-button type="primary" @click="mode = 1; formVisible = true">添加药品</el-button>
            <el-button type="primary" @click="refreshData">刷新</el-button>
        </template>
        <el-table :data="dataList" stripe border style="width: 100%">
            <el-table-column prop="drugId" label="Id" />
            <el-table-column prop="drugName" label="药品名称" />
            <el-table-column prop="spec" label="药品规格" />
            <el-table-column prop="pinYin" label="拼音码" />
            <el-table-column prop="sort" label="排序编号" />
            <el-table-column prop="clinicalUnit" label="临床单位" />
            <el-table-column prop="packageUnit" label="包装单位" />
            <el-table-column prop="c2PQuantity" label="临床到包装单位的转换" />
            <el-table-column prop="approvalNumber" label="批准文号" />
            <el-table-column prop="drugCode" label="本地药品编码" />
            <el-table-column prop="nationDrugCode" label="国家药品编码" />
            <el-table-column prop="radManufacturer" label="药品研发厂家" />
            <el-table-column prop="dosageForm" label="药品剂型" />
            <el-table-column prop="defaultSaleUnit" label="销售单位" />
            <el-table-column prop="clinicalSaleUnit" label="临床销售单位" />
            <el-table-column prop="packagePrice" label="包装单位销售单价" />
            <el-table-column prop="clinicalPrice" label="临床销售单价" />
            <el-table-column label="操作">
                <template #default="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">修改</el-button>
                    <el-button size="small" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
    </el-card>

    <el-dialog v-model="formVisible" title="添加药品" width="850">
        <el-form :inline="true" ref="formRef" :model="dataModel" :rules="rules" label-width="auto">
            <el-form-item label="药品名称" prop="drugName">
                <el-input v-model="dataModel.drugName" autocomplete="off" />
            </el-form-item>
            <el-form-item label="药品规格" prop="spec">
                <el-input v-model="dataModel.spec" autocomplete="off" />
            </el-form-item>
            <el-form-item label="拼音码" prop="pinYin">
                <el-input v-model="dataModel.pinYin" autocomplete="off" />
            </el-form-item>
            <el-form-item label="排序编号" prop="sort">
                <el-input v-model="dataModel.sort" type="number" autocomplete="off" />
            </el-form-item>
            <el-form-item label="临床单位" prop="clinicalUnit">
                <el-input v-model="dataModel.clinicalUnit" autocomplete="off" />
            </el-form-item>
            <el-form-item label="包装单位" prop="packageUnit">
                <el-input v-model="dataModel.packageUnit" autocomplete="off" />
            </el-form-item>
            <el-form-item label="临床到包装单位的转换" prop="c2PQuantity">
                <el-input v-model="dataModel.c2PQuantity" type="number" autocomplete="off" />
            </el-form-item>
            <el-form-item label="批准文号" prop="approvalNumber">
                <el-input v-model="dataModel.approvalNumber" autocomplete="off" />
            </el-form-item>
            <el-form-item label="本地药品编码" prop="drugCode">
                <el-input v-model="dataModel.drugCode" autocomplete="off" />
            </el-form-item>
            <el-form-item label="国家药品编码" prop="nationDrugCode">
                <el-input v-model="dataModel.nationDrugCode" autocomplete="off" />
            </el-form-item>
            <el-form-item label="药品研发厂家" prop="radManufacturer">
                <el-input v-model="dataModel.radManufacturer" autocomplete="off" />
            </el-form-item>
            <el-form-item label="药品剂型" prop="dosageForm">
                <el-input v-model="dataModel.dosageForm" autocomplete="off" />
            </el-form-item>
            <el-form-item label="销售单位" prop="defaultSaleUnit">
                <el-input v-model="dataModel.defaultSaleUnit" type="number" autocomplete="off" />
            </el-form-item>
            <el-form-item label="临床销售单位" prop="clinicalSaleUnit">
                <el-input v-model="dataModel.clinicalSaleUnit" type="number" autocomplete="off" />
            </el-form-item>
            <el-form-item label="包装单位销售单价" prop="packagePrice">
                <el-input v-model="dataModel.packagePrice" type="number" autocomplete="off" />
            </el-form-item>
            <el-form-item label="临床销售单价" prop="clinicalPrice">
                <el-input v-model="dataModel.clinicalPrice" type="number" autocomplete="off" />
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

<script setup>
import { addDrugService, deleteDrugService, getAllDrugsService, getDrugService, updateDrugService } from '@/api/drug';
import { ElMessage, ElMessageBox } from 'element-plus';
import { ref } from 'vue';

const formRef = ref()
const formVisible = ref(false)
const dataModel = ref({
    drugName: '',
    spec: '',
    pinYin: '',
    sort: 0,
    clinicalUnit: '',
    packageUnit: '',
    c2PQuantity: 1,
    approvalNumber: '',
    drugCode: '',
    nationDrugCode: '',
    radManufacturer: '',
    dosageForm: '',
    defaultSaleUnit: 0,
    clinicalSaleUnit: 0,
    packagePrice: 0,
    clinicalPrice: 0
})
const rules = {
    drugName: [{ required: true, message: '必填', trigger: 'blur' }],
    spec: [{ required: true, message: '必填', trigger: 'blur' }],
    pinYin: [{ required: true, message: '必填', trigger: 'blur' }],
    sort: [{ required: true, message: '必填', trigger: 'blur' }],
    clinicalUnit: [{ required: true, message: '必填', trigger: 'blur' }],
    packageUnit: [{ required: true, message: '必填', trigger: 'blur' }],
    c2PQuantity: [{ required: true, message: '必填', trigger: 'blur' }],
    approvalNumber: [{ required: true, message: '必填', trigger: 'blur' }],
    drugCode: [{ required: true, message: '必填', trigger: 'blur' }],
    nationDrugCode: [{ required: true, message: '必填', trigger: 'blur' }],
    radManufacturer: [{ required: true, message: '必填', trigger: 'blur' }],
    dosageForm: [{ required: true, message: '必填', trigger: 'blur' }],
    defaultSaleUnit: [{ required: true, message: '必填', trigger: 'blur' }],
    clinicalSaleUnit: [{ required: true, message: '必填', trigger: 'blur' }],
    packagePrice: [{ required: true, message: '必填', trigger: 'blur' }],
    clinicalPrice: [{ required: true, message: '必填', trigger: 'blur' }],
}

const dataList = ref()
const getList = async () => {
    dataList.value = await getAllDrugsService()
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
        await addDrugService(dataModel.value)
        ElMessage.success('药品添加成功')
    } else if (mode === 2) {
        await updateDrugService(dataModel.value.id, dataModel.value)
        ElMessage.success('药品修改成功')
    }

    formVisible.value = false
    formRef.value.resetFields()

    await getList()
}

const handleEdit = async (index, row) => {
    dataModel.value = await getDrugService(row.id)
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

    await deleteDrugService(row.id)
    await getList()
}
</script>