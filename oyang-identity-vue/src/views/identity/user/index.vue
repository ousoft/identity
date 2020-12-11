<template>
  <div class="app-container">
    <el-form :inline="true" :model="paginationModel" class="demo-form-inline">
      <el-form-item>
        <el-input
          placeholder="请输入关键字"
          v-model="paginationModel.loginName"
        >
          <template slot="prepend">登录名</template>
        </el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">查询</el-button>
      </el-form-item>
    </el-form>
    <el-button-group>
      <el-button size="medium" @click="onAdd">新增</el-button>
      <el-button size="medium" @click="onUpdate">修改</el-button>
      <el-button size="medium" @click="onDelete">删除</el-button>
    </el-button-group>
    <el-table
      :data="tableData"
      sort-change="handleSortChange"
      stripe
      style="width: 100%"
      :default-sort="{ prop: 'id', order: 'ascending' }"
    >
      <el-table-column type="index" >序号</el-table-column>
      <el-table-column prop="id" label="Id"></el-table-column>
      <el-table-column
        prop="loginName"
        label="LoginName"
        sortable="custom"
      ></el-table-column>
    </el-table>
    <el-pagination
      @size-change="handlePageSizeChange"
      @current-change="handlePageIndexChange"
      :current-page="paginationModel.pageIndex"
      :page-sizes="[10, 20, 50, 100]"
      :page-size="paginationModel.pageSize"
      layout="total, sizes, prev, pager, next, jumper"
      :total="totalCount"
      style="margin-top:10px;"
    >
    </el-pagination>
  </div>
</template>

<script>
import request from '@/utils/request'

export default {
  name: 'User',
  data() {
    return {
      loading: false,
      paginationModel: {
        pageIndex: 1,
        pageSize: 10,
        sortField: 'id',
        isAscending: true,
        loginName: ''
      },
      totalCount: 0,
      tableData: []
    }
  },
  created() {
    this.onSubmit()
  },
  methods: {
    handleSortChange(column, prop, order) {
      this.paginationModel.sortField = prop
      this.paginationModel.isAscending = order === 'descending' ? false : true
      this.getList()
    },
    handlePageIndexChange(pageIndex) {
      this.paginationModel.pageIndex = pageIndex
      this.getList()
    },
    handlePageSizeChange(pageSize) {
      this.paginationModel.pageSize = pageSize
      this.getList()
    },
    onSubmit() {
      const self = this
      request({
        url: '/api/User/GetList',
        method: 'get',
        params: self.paginationModel
      })
        .then(response => {
          self.tableData = response.items
          self.totalCount = response.totalCount
        })
        .catch(error => {
          console.debug(error)
        })
    },
    onAdd() {},
    onUpdate() {},
    onDelete() {}
  }
}
</script>
