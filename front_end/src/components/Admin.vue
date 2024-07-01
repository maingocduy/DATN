<template>
  <div class="admin-page">
    <el-header class="header">
      <img
        src="../../public/Images/309043961_434949368736287_4246824704006557099_n (1).jpg"
        alt="Logo"
        class="logo"
      />
      <span class="title">Quản lý tài khoản</span>
    </el-header>
    <el-main class="content">
      <el-input
        v-model="keyword"
        placeholder="Tìm kiếm theo tên tài khoản"
        clearable
        class="search-input"
        @input="handleSearch"
        style="width: 300px; margin-bottom: 10px"
      >
        <template #prefix>
          <el-icon><Search /></el-icon>
        </template>
      </el-input>
      <el-table :data="accounts" :table-layout="auto" stripe>
        <el-table-column label="STT" width="60" type="index" :index="indexMethod"></el-table-column>
        <el-table-column prop="username" label="Tên tài khoản"></el-table-column>
        <el-table-column prop="member.name" label="Tên thành viên"></el-table-column>
        <el-table-column prop="member.email" label="Email" width="260"></el-table-column>
        <el-table-column prop="member.groups.group_name" label="Vị trí"></el-table-column>
        <el-table-column prop="role" label="Quyền"></el-table-column>
        <el-table-column label="Trạng thái">
          <template #default="scope">
            <span>{{ scope.row.status === 1 ? 'Đã xác nhận' : 'Chưa xác nhận' }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Hành động">
          <template #default="scope">
            <template v-if="scope.row.role !== 'Admin'">
              <el-tooltip effect="light" content="Đổi vai trò" placement="top-start">
                <el-button
                  @click="handleChangeRole(scope.row)"
                  type="primary"
                  :icon="UserFilled"
                  circle
                  size="mini"
                  class="tooltip-change-role"
                ></el-button>
              </el-tooltip>
              <el-tooltip effect="light" content="Xóa tài khoản" placement="top-start">
                <el-button
                  @click="handleDelete(scope.row)"
                  type="danger"
                  :icon="Delete"
                  circle
                  size="mini"
                  class="tooltip-delete"
                ></el-button>
              </el-tooltip>
            </template>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        @current-change="handleCurrentChange"
        :current-page="pageNumber"
        layout="prev, pager, next"
        :page-count="totalPages"
        :page-size="6"
        style="margin-top: 10px"
      ></el-pagination>
    </el-main>
  </div>
</template>

<script>
import axios from 'axios'
import Cookies from 'js-cookie'
import {
  ElButton,
  ElTable,
  ElTableColumn,
  ElHeader,
  ElMain,
  ElInput,
  ElPagination,
  ElNotification,
  ElMessageBox,
  ElTooltip,
  ElIcon
} from 'element-plus'
import { Delete, Search, UserFilled } from '@element-plus/icons-vue'

export default {
  components: {
    ElButton,
    ElTable,
    ElTableColumn,
    ElHeader,
    ElMain,
    ElInput,
    ElPagination,
    ElTooltip,
    ElIcon,
    Search,
    UserFilled
  },
  data() {
    return {
      accounts: [],
      keyword: null,
      pageNumber: 1,
      totalPages: 0,
      Delete: Delete,
      Search: Search,
      UserFilled: UserFilled,
      role: Cookies.get('role')
    }
  },
  methods: {
    async fetchAccounts() {
      try {
        const response = await axios.post('api/account/get_all_accounts', {
          pageNumber: this.pageNumber,
          keyword: this.keyword
        })
        this.totalPages = response.data.totalPages
        this.accounts = response.data.accounts
        console.log(this.accounts)
      } catch (error) {
        console.error('Error fetching accounts:', error)
        this.showErrorNotification('Failed to fetch accounts.')
      }
    },
    async handleDelete(row) {
      ElMessageBox.confirm('Bạn có chắc muốn xóa tài khoản này không ?', 'Confirm', {
        confirmButtonText: 'Chấp nhận',
        cancelButtonText: 'Hủy',
        type: 'warning'
      })
        .then(async () => {
          try {
            const response = await axios.delete('api/account/delete_acc', {
              params: {
                username: row.username
              }
            })
            if (response.status === 200) {
              this.showSuccessNotification(response.data.message)
              this.fetchAccounts() // Refresh accounts list after deletion
            } else {
              this.showErrorNotification('Xóa tài khoản thất bại.')
            }
          } catch (error) {
            if (error.response && error.response.status === 404) {
              this.showErrorNotification(error.response.data)
            } else {
              this.showErrorNotification('Đã xảy ra lỗi. Vui lòng thử lại sau.')
            }
          }
        })
        .catch(() => {
          this.showErrorNotification('Xóa tài khoản thất bại!')
        })
    },
    async handleChangeRole(row) {
      ElMessageBox.confirm('Bạn có chắc muốn thay đổi quyền của người này không ?', 'Confirm', {
        confirmButtonText: 'Chấp nhận',
        cancelButtonText: 'Hủy',
        type: 'warning'
      })
        .then(async () => {
          try {
            const response = await axios.post('api/account/update_role', null, {
              params: {
                username: row.username
              }
            })
            if (response.status === 200) {
              this.showSuccessNotification(response.data.message)
              this.fetchAccounts() // Refresh accounts list after role change
            } else {
              this.showErrorNotification('Đổi quyền thất bại.')
            }
          } catch (error) {
            if (error.response && error.response.status === 404) {
              this.showErrorNotification(error.response.data.Message)
            } else {
              this.showErrorNotification('Đã xảy ra lỗi. Vui lòng thử lại sau.')
            }
          }
        })
        .catch(() => {
          this.showInfoNotification('Đã hủy đổi quyền.')
        })
    },
    handleCurrentChange(val) {
      this.pageNumber = val
      this.fetchAccounts()
    },
    handleSearch() {
      this.pageNumber = 1 // Reset to first page when performing search
      this.fetchAccounts()
    },
    showErrorNotification(message) {
      ElNotification({
        title: 'Lỗi',
        message: message,
        type: 'error'
      })
    },
    showSuccessNotification(message) {
      ElNotification({
        title: 'Thành công',
        message: message,
        type: 'success'
      })
    },
    showInfoNotification(message) {
      ElNotification({
        title: 'Thông tin',
        message: message,
        type: 'info'
      })
    },
    indexMethod(index) {
      return (this.pageNumber - 1) * 10 + index + 1
    }
  },
  created() {
    this.fetchAccounts()
  }
}
</script>

<style scoped>
.admin-page {
  width: 100%;
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.header {
  display: flex;
  align-items: center;
  background-color: #409eff;
  color: white;
  padding: 10px;
}

.logo {
  height: 40px;
  margin-right: 10px;
}

.title {
  font-size: 24px;
  font-weight: bold;
}

.content {
  flex: 1;
  padding: 20px;
  background-color: #f5f5f5;
}

.search-input {
  margin-bottom: 20px;
}

.el-table {
  background-color: white;
  margin-bottom: 20px;
}

.el-table-column {
  text-align: center;
}

.el-pagination {
  text-align: center;
}

/* Customizing tooltips */
.tooltip-delete :deep(.el-tooltip__popper) {
  background-color: red;
  color: white;
}

.tooltip-change-role :deep(.el-tooltip__popper) {
  background-color: green;
  color: white;
}
</style>
