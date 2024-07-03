<template>
  <div class="container mx-auto mt-8 max-w-2xl p-6">
    <!-- Action Buttons (Moved to top right) -->
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-4xl font-bold text-indigo-600">Thông tin cá nhân</h1>
      <div class="flex space-x-4">
        <el-button type="primary" @click="openEditDialog" :icon="Edit"
          >Thay đổi thông tin</el-button
        >
        <el-button type="primary" @click="openBlogListDialog" :icon="Edit"
          >Danh sách Blog đã tạo</el-button
        >
      </div>
    </div>

    <!-- User Information Card -->
    <div class="grid grid-cols-2 gap-6 p-6">
      <div class="flex items-center">
        <strong class="mr-2 text-lg">Tên đăng nhập:</strong>
        <span>{{ user.username }}</span>
      </div>
      <div class="flex items-center">
        <strong class="mr-2 text-lg">Họ và tên:</strong>
        <span>{{ user.member && user.member.name }}</span>
      </div>
      <div class="flex items-center">
        <strong class="mr-2 text-lg">Email:</strong>
        <span>{{ user.member && user.member.email }}</span>
      </div>
      <div class="flex items-center">
        <strong class="mr-2 text-lg">Số điện thoại:</strong>
        <span>{{ user.member && user.member.phone }}</span>
      </div>
      <div v-if="user.member && user.member.groups" class="flex items-center">
        <strong class="mr-2 text-lg">Nhóm:</strong>
        <span>{{ user.member.groups.group_name }}</span>
      </div>
    </div>

    <!-- Edit User Info Popup -->
    <el-dialog
      title="Sửa thông tin cá nhân"
      v-model="showEditDialog"
      width="40%"
      :before-close="handleEditDialogClose"
    >
      <el-form :model="editForm" ref="editForm" :rules="editFormRules" label-width="130px">
        <el-form-item label="Họ và tên:" prop="name">
          <el-input
            size="large"
            style="width: 90%"
            v-model="editForm.name"
            placeholder="Nhập họ và tên"
          ></el-input>
        </el-form-item>
        <el-form-item label="Email:" prop="email">
          <el-input
            disabled
            size="large"
            style="width: 90%"
            v-model="editForm.email"
            placeholder="Nhập email"
          ></el-input>
        </el-form-item>
        <el-form-item label="Số điện thoại:" prop="phone">
          <el-input
            size="large"
            style="width: 90%"
            v-model="editForm.phone"
            placeholder="Nhập số điện thoại"
          ></el-input>
        </el-form-item>
        <el-form-item label="Nhóm:" prop="group_name">
          <el-select
            size="large"
            style="width: 90%"
            v-model="editForm.group_name"
            placeholder="Chọn nhóm"
          >
            <el-option
              v-for="group in groups"
              :key="group.group_id"
              :label="group.group_name"
              :value="group.group_name"
            ></el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <div class="flex justify-end mt-6">
        <el-button @click="closeEditDialog">Hủy</el-button>
        <el-button type="primary" @click="saveUserInfo">Lưu</el-button>
      </div>
    </el-dialog>

    <!-- Blog List Popup -->
    <el-dialog title="Danh sách Blog đã tạo" v-model="showBlogListDialog" width="80%">
      <el-table :data="blogs" style="width: 100%">
        <el-table-column prop="index" label="STT" width="80">
          <template #default="{ $index }">
            {{ $index + 1 }}
          </template>
        </el-table-column>

        <el-table-column prop="title" label="Title"></el-table-column>
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
          <template #default="scope">
            <el-tooltip effect="light" content="Xem chi tiết" placement="top-start">
              <el-button
                @click="handleDetail(scope.row)"
                :icon="Document"
                type="primary"
                size="small"
                circle
              ></el-button
            ></el-tooltip>
            <el-tooltip effect="light" content="Sửa Blog" placement="top-start">
              <el-button
                @click="editBlog(scope.row)"
                type="warning"
                :icon="Edit"
                size="small"
                circle
              ></el-button>
            </el-tooltip>
            <el-tooltip effect="light" content="Xóa Blog" placement="top-start">
              <el-button
                @click="deleteBlog(scope.row)"
                type="danger"
                :icon="Delete"
                size="small"
                circle
              ></el-button>
            </el-tooltip>
          </template>
        </el-table-column>
      </el-table>
      <div class="flex justify-center mt-4">
        <el-pagination
          @current-change="handlePageChange"
          :current-page="currentPage"
          :page-size="6"
          layout="prev, pager, next"
          :total="totalPages * 6"
        >
        </el-pagination>
      </div>
    </el-dialog>

    <!-- Change Password Section -->
    <div class="mt-8 p-6 border-t border-gray-200">
      <h2 class="text-2xl font-bold text-indigo-600 mb-4">Đổi mật khẩu</h2>
      <el-form
        :model="passwordForm"
        ref="passwordForm"
        :rules="passwordFormRules"
        label-width="130px"
      >
        <el-form-item label="Mật khẩu hiện tại:" prop="currentPassword">
          <el-input
            size="large"
            style="width: 90%"
            v-model="passwordForm.currentPassword"
            type="password"
            autocomplete="current-password"
            :show-password="showCurrentPassword"
          ></el-input>
          <el-button type="text" icon="el-icon-view" @click="toggleShowCurrentPassword">
            <i :class="showNewPassword ? 'View' : 'Hide'"></i>
          </el-button>
        </el-form-item>
        <el-form-item label="Mật khẩu mới:" prop="password">
          <el-input
            size="large"
            style="width: 90%"
            v-model="passwordForm.password"
            type="password"
            autocomplete="new-password"
            :show-password="showNewPassword"
          ></el-input>
          <el-button type="text" icon="el-icon-view" @click="toggleShowNewPassword">
            <i :class="showNewPassword ? 'View' : 'Hide'"></i>
          </el-button>
        </el-form-item>
        <el-form-item label="Xác nhận mật khẩu mới:" prop="confirmPassword">
          <el-input
            size="large"
            style="width: 90%"
            v-model="passwordForm.confirmPassword"
            type="password"
            autocomplete="new-password"
            :show-password="showConfirmPassword"
          ></el-input>
          <el-button type="text" icon="el-icon-view" @click="toggleShowConfirmPassword">
            <i :class="showConfirmPassword ? 'View' : 'Hide'"></i>
          </el-button>
        </el-form-item>
      </el-form>
      <div class="flex justify-end mt-6">
        <el-button type="primary" @click="savePassword">Đổi mật khẩu</el-button>
      </div>
    </div>
  </div>
</template>
<script>
import axios from 'axios'
import { ElMessageBox, ElNotification } from 'element-plus'
import { View, Edit, Hide, Delete, Document } from '@element-plus/icons-vue'

export default {
  data() {
    return {
      user: {},
      groups: [],
      showEditDialog: false,
      showBlogListDialog: false,
      showCurrentPassword: true,
      showNewPassword: true,
      showConfirmPassword: true,
      editForm: {
        name: '',
        email: '',
        phone: '',
        group_name: ''
      },
      passwordForm: {
        currentPassword: '',
        password: '',
        confirmPassword: ''
      },
      blogs: [],
      currentPage: 1,
      totalPages: 0,
      editFormRules: {
        name: [{ required: true, message: 'Họ và tên không được để trống', trigger: 'blur' }],
        email: [{ required: true, message: 'Email không được để trống', trigger: 'blur' }],
        phone: [{ required: true, message: 'Số điện thoại không được để trống', trigger: 'blur' }],
        group_name: [{ required: true, message: 'Nhóm không được để trống', trigger: 'blur' }]
      },
      passwordFormRules: {
        currentPassword: [
          { required: true, message: 'Mật khẩu hiện tại không được để trống', trigger: 'blur' }
        ],
        password: [
          { required: true, message: 'Mật khẩu mới không được để trống', trigger: 'blur' }
        ],
        confirmPassword: [
          { required: true, message: 'Xác nhận mật khẩu không được để trống', trigger: 'blur' },
          { validator: this.validatePasswordConfirm, trigger: 'blur' }
        ]
      },
      Edit: Edit,
      Hide: Hide,
      View: View,
      Delete: Delete,
      Document: Document
    }
  },
  async created() {
    await this.fetchUserData()
    this.fetchGroups()
    this.fetchBlogs()
  },
  methods: {
    async fetchUserData() {
      try {
        const response = await axios.get(`/api/account/Username`, {
          params: {
            username: this.$route.params.username
          }
        })
        this.user = response.data
        this.editForm = {
          name: this.user.member ? this.user.member.name : '',
          email: this.user.member ? this.user.member.email : '',
          phone: this.user.member ? this.user.member.phone : '',
          group_name:
            this.user.member && this.user.member.groups ? this.user.member.groups.group_name : ''
        }
      } catch (error) {
        console.error('Error fetching user data:', error)
      }
    },
    fetchGroups() {
      axios
        .get('api/group')
        .then((response) => {
          this.groups = response.data
        })
        .catch((error) => {
          console.error('Error fetching groups:', error)
        })
    },
    fetchBlogs() {
      axios
        .post('api/Blog/all_blog_by_id', {
          acc_id: this.user.account_id,
          pageNumber: this.currentPage
        })
        .then((response) => {
          this.blogs = response.data.blogs
          this.totalPages = response.data.totalPages
          console.log(this.blogs)
        })
        .catch((error) => {
          console.error('Error fetching blogs:', error)
        })
    },
    handlePageChange(page) {
      this.currentPage = page
      this.fetchBlogs()
    },
    openEditDialog() {
      this.showEditDialog = true
    },
    closeEditDialog() {
      this.showEditDialog = false
    },
    openBlogListDialog() {
      this.showBlogListDialog = true
    },
    saveUserInfo() {
      this.$refs.editForm.validate((valid) => {
        if (valid) {
          axios
            .post('api/Member/update_member', {
              name: this.editForm.name,
              email: this.editForm.email,
              phone: this.editForm.phone,
              group_name: this.editForm.group_name
            })
            .then((response) => {
              ElNotification({
                title: 'Thành công',
                message: response.data.messenger,
                type: 'success'
              })
              this.closeEditDialog()
              this.fetchUserData()
            })
            .catch((error) => {
              ElNotification({
                title: 'Lỗi',
                message: error.response.data.messenger,
                type: 'error'
              })
            })
        }
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
    savePassword() {
      if (this.passwordForm.password.length < 6) {
        ElNotification({
          title: 'Lỗi',
          message: 'Mật khẩu chứa ít nhất 6 ký tự',
          type: 'error'
        })
        return
      }
      this.$refs.passwordForm.validate((valid) => {
        if (valid) {
          axios
            .post('api/account/change_pass', {
              username: this.user.username,
              oldPassword: this.passwordForm.currentPassword,
              password: this.passwordForm.password
            })
            .then((response) => {
              ElNotification({
                title: 'Thành công',
                message: response.data.message,
                type: 'success'
              })
              this.passwordForm.currentPassword = ''
              this.passwordForm.password = ''
              this.passwordForm.confirmPassword = ''
            })
            .catch((error) => {
              ElNotification({
                title: 'Lỗi',
                message: error.response.data.message,
                type: 'error'
              })
            })
        }
      })
    },
    handleDetail(row) {
      // Implement your detail view logic here, for example, navigate to a detailed blog page
      this.$router.push('/BlogDetail/' + row.blog_id)
    },
    validatePasswordConfirm(rule, value, callback) {
      if (value !== this.passwordForm.password) {
        callback(new Error('Xác nhận mật khẩu không khớp'))
      } else {
        callback()
      }
    },
    handleEditDialogClose(done) {
      this.$confirm('Bạn có muốn huỷ bỏ sửa thông tin?', 'Thông báo', {
        confirmButtonText: 'OK',
        cancelButtonText: 'Hủy',
        type: 'warning'
      })
        .then(() => {
          done()
        })
        .catch(() => {})
    },
    toggleShowCurrentPassword() {
      this.showCurrentPassword = !this.showCurrentPassword
    },
    toggleShowNewPassword() {
      this.showNewPassword = !this.showNewPassword
    },
    toggleShowConfirmPassword() {
      this.showConfirmPassword = !this.showConfirmPassword
    },
    showErrorNotification(messenger) {
      ElNotification({
        title: 'Lỗi',
        message: messenger,
        type: 'error'
      })
    },
    showSuccessNotification(messenger) {
      ElNotification({
        title: 'Thành công',
        message: messenger,
        type: 'success'
      })
    },
    editBlog(blog) {
      this.$router.push('/Blog/Update/' + blog.blog_id)
    },
    deleteBlog(row) {
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
            } else {
              this.showErrorNotification('Xóa bài viết thất bại.')
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
          this.showErrorNotification('Xóa bài viết thất bại!')
        })
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 1.5rem;
}

.mb-4 {
  margin-bottom: 1rem;
}

.text-lg {
  font-size: 1.125rem; /* Equivalent to 18px */
}

.el-button {
  margin: 0 10px;
}
</style>
