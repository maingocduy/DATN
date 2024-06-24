<template>
  <div class="container mx-auto mt-8 max-w-2xl p-6">
    <!-- Action Buttons (Moved to top right) -->
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-4xl font-bold text-indigo-600">Thông tin cá nhân</h1>
      <div class="flex space-x-4">
        <el-button type="primary" @click="openEditDialog" :icon="Edit"
          >Thay đổi thông tin</el-button
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
      <!-- Password Section -->
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
import { ElMessage, ElMessageBox, ElNotification } from 'element-plus'
import { View, Edit, Hide } from '@element-plus/icons-vue'

export default {
  data() {
    return {
      user: {},
      groups: [],
      showEditDialog: false,
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
      View: View
    }
  },
  created() {
    this.fetchUserData()
    this.fetchGroups()
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
    openEditDialog() {
      this.showEditDialog = true
    },
    closeEditDialog() {
      this.showEditDialog = false
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
    savePassword() {
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
                message: response.data.messenger,
                type: 'success'
              })
              this.passwordForm.currentPassword = ''
              this.passwordForm.password = ''
              this.passwordForm.confirmPassword = ''
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
