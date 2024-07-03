<template>
  <div class="admin-page">
    <el-header class="header">
      <img src="../../public/Images/logo.jpg" alt="Logo" class="logo" />
      <span class="title">Quản lý Blog</span>
    </el-header>
    <el-main class="content">
      <el-input
        v-model="keyword"
        placeholder="Tìm kiếm theo tiêu đề"
        clearable
        class="search-input"
        @input="handleSearch"
        style="width: 300px; margin-bottom: 10px"
      >
        <template #prefix>
          <el-icon><Search /></el-icon>
        </template>
      </el-input>
      <el-select
        v-model="status"
        placeholder="Lọc theo trạng thái"
        clearable
        class="status-select"
        @change="handleSearch"
        style="width: 200px; margin-bottom: 10px; margin-left: 10px"
      >
        <el-option label="Tất cả" :value="null"></el-option>
        <el-option label="Đã duyệt" :value="true"></el-option>
        <el-option label="Chưa duyệt" :value="false"></el-option>
      </el-select>
      <el-table :data="blogs" :table-layout="auto" stripe>
        <el-table-column label="STT" width="60">
          <template #default="{ $index }">
            {{ $index + 1 }}
          </template>
        </el-table-column>
        <el-table-column prop="title" label="Tiêu đề"></el-table-column>

        <el-table-column prop="account.username" label="Người tạo"></el-table-column>
        <el-table-column prop="created_at" label="Ngày tạo">
          <template #default="{ row }">
            <span>{{ formatDate(row.createdAt) }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="status" label="Trạng thái">
          <template #default="{ row }">
            <span>{{ row.approved === true ? 'Đã duyệt' : 'Chưa duyệt' }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Hành động">
          <template #default="{ row }">
            <el-tooltip
              effect="light"
              :content="row.approved ? 'Ẩn blog' : 'Duyệt blog'"
              placement="top-start"
            >
              <el-button
                @click="handleApprove(row)"
                :type="row.approved ? 'danger' : 'success'"
                :icon="row.approved ? CircleCloseFilled : SuccessFilled"
                circle
                size="mini"
                class="tooltip-approve"
              ></el-button>
            </el-tooltip>
            <el-tooltip effect="light" content="Xem chi tiết" placement="top-start">
              <el-button
                @click="handleDetail(row)"
                type="primary"
                :icon="Document"
                circle
                size="mini"
                class="tooltip-detail"
              ></el-button>
            </el-tooltip>
            <el-tooltip effect="light" content="Xóa Blog" placement="top-start">
              <el-button
                @click="handleDelete(row)"
                type="danger"
                :icon="Delete"
                circle
                size="mini"
                class="tooltip-delete"
              ></el-button>
            </el-tooltip>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        @current-change="handleCurrentChange"
        :current-page="pageNumber"
        :page-size="pageSize"
        :page-count="totalPages"
        layout="prev, pager, next"
        style="margin-top: 10px"
        :disabled="false"
      ></el-pagination>
    </el-main>
  </div>
</template>

<script>
import axios from 'axios'
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
  ElIcon,
  ElSelect,
  ElOption
} from 'element-plus'
import { Delete, Search, Document, SuccessFilled, CircleCloseFilled } from '@element-plus/icons-vue'

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
    ElSelect,
    ElOption,
    Search,
    Delete,
    Document,
    SuccessFilled,
    CircleCloseFilled
  },
  data() {
    return {
      blogs: [],
      keyword: null,
      status: null,
      pageNumber: 1,
      pageSize: 6,
      totalPages: 0,
      Delete: Delete,
      Document: Document,
      SuccessFilled: SuccessFilled,
      CircleCloseFilled: CircleCloseFilled
    }
  },
  methods: {
    async fetchBlogs() {
      try {
        const response = await axios.post('api/blog/all_blog', {
          pageNumber: this.pageNumber,
          keyword: this.keyword,
          approved: this.status
        })
        this.totalPages = response.data.totalPages
        this.blogs = response.data.blogs
        console.log(response)
      } catch (error) {
        console.error('Error fetching blogs:', error)
        this.showErrorNotification('Failed to fetch blogs.')
      }
    },
    async handleDelete(row) {
      ElMessageBox.confirm('Bạn có chắc muốn xóa bài viết này không ?', 'Xác nhận', {
        confirmButtonText: 'Chấp nhận',
        cancelButtonText: 'Hủy',
        type: 'warning'
      })
        .then(async () => {
          try {
            const response = await axios.delete('api/blog/delete_blog', {
              params: {
                title: row.title
              }
            })
            if (response.status === 200) {
              this.showSuccessNotification(response.data.message)
              this.fetchBlogs() // Refresh blogs list after deletion
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
          console.log('Hủy xóa bài viết!')
        })
    },
    async handleApprove(row) {
      ElMessageBox.confirm('Bạn có chắc muốn đổi trạng thái của bài viết này không ?', 'Xác nhận', {
        confirmButtonText: 'Chấp nhận',
        cancelButtonText: 'Hủy',
        type: 'warning'
      })
        .then(async () => {
          try {
            const response = await axios.post('api/blog/update_status', {
              id: row.blog_id
            })
            if (response.status === 200) {
              this.showSuccessNotification(response.data.message)
              this.fetchBlogs() // Refresh blogs list after approval
            } else {
              this.showErrorNotification('Đổi trạng thái bài viết thất bại.')
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
          this.showInfoNotification('Đã hủy duyệt bài viết.')
        })
    },
    handleDetail(row) {
      // Implement your detail view logic here, for example, navigate to a detailed blog page
      this.$router.push('/BlogDetail/' + row.blog_id)
    },
    handleCurrentChange(val) {
      this.pageNumber = val
      this.fetchBlogs()
    },
    handleSearch() {
      this.pageNumber = 1 // Reset to first page when performing search
      this.fetchBlogs()
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
    formatDate(dateString) {
      const options = {
        year: 'numeric',
        month: 'numeric',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric'
      }
      const formattedDate = new Date(dateString).toLocaleDateString('vi-VN', options)
      return formattedDate
    },

    indexMethod(index) {
      return (this.pageNumber - 1) * 10 + index + 1
    }
  },
  created() {
    this.fetchBlogs()
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

.status-select {
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

.tooltip-approve :deep(.el-tooltip__popper) {
  background-color: green;
  color: white;
}

.tooltip-detail :deep(.el-tooltip__popper) {
  background-color: #409eff;
  color: white;
}
</style>
