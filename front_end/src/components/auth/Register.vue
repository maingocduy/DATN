<template>
  <div
    class="bg-gradient-to-r from-blue-500 to-green-500 min-h-screen flex items-center justify-center py-12 px-6"
  >
    <!-- Success Notification -->
    <transition name="fade">
      <div
        v-if="showSuccessNotification"
        class="fixed top-0 inset-x-0 p-4 bg-green-500 text-white text-center z-50"
      >
        Đăng ký tài khoản thành công! Vui lòng vào Email của bạn để xác thực Email.
      </div>
    </transition>

    <!-- Error Modal -->
    <transition name="fade">
      <div
        v-if="showErrorModal"
        class="fixed inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50"
      >
        <div class="bg-white p-6 rounded-lg shadow-lg w-96 relative">
          <h3 class="text-lg font-bold mb-4 text-red-500">Lỗi</h3>
          <p class="text-gray-700 mb-4">{{ registrationMessage }}</p>
          <button
            @click="closeErrorModal"
            class="absolute top-2 right-2 bg-transparent text-gray-500 hover:text-gray-700 text-xl"
          >
            &times;
          </button>
          <button
            @click="closeErrorModal"
            class="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 w-full"
          >
            Đóng
          </button>
        </div>
      </div>
    </transition>

    <div class="max-w-lg w-full bg-white rounded-lg shadow-lg p-8">
      <h2 class="text-center text-3xl font-extrabold text-gray-900 mb-6">Tạo Tài Khoản Mới</h2>
      <form @submit.prevent="handleRegistration">
        <div class="mb-5">
          <label for="username" class="block mb-2 text-sm font-medium text-gray-900"
            >Tên đăng nhập *</label
          >
          <input
            type="text"
            v-model="username"
            id="username"
            class="input-field"
            placeholder="Nhập tên người dùng"
          />
        </div>
        <div class="mb-5">
          <label for="password" class="block mb-2 text-sm font-medium text-gray-900"
            >Mật khẩu *</label
          >
          <input
            type="password"
            v-model="password"
            id="password"
            class="input-field"
            placeholder="Nhập mật khẩu"
          />
        </div>
        <div class="mb-5">
          <label for="confirmPassword" class="block mb-2 text-sm font-medium text-gray-900"
            >Xác nhận mật khẩu *</label
          >
          <input
            type="password"
            v-model="confirmPassword"
            id="confirmPassword"
            class="input-field"
            placeholder="Xác nhận mật khẩu"
          />
        </div>
        <div class="mb-5">
          <label for="name" class="block mb-2 text-sm font-medium text-gray-900">Họ và tên *</label>
          <input
            type="text"
            v-model="name"
            id="name"
            class="input-field"
            placeholder="Nhập họ và tên"
          />
        </div>
        <div class="mb-5">
          <label for="email" class="block mb-2 text-sm font-medium text-gray-900">Email *</label>
          <input
            type="text"
            v-model="email"
            id="email"
            class="input-field"
            placeholder="Nhập email"
          />
        </div>
        <div class="mb-5">
          <label for="phone" class="block mb-2 text-sm font-medium text-gray-900"
            >Số điện thoại *</label
          >
          <input
            type="tel"
            v-model="phone"
            id="phone"
            class="input-field"
            placeholder="Nhập số điện thoại"
          />
        </div>
        <div class="mb-5">
          <label for="groupName" class="block mb-2 text-sm font-medium text-gray-900"
            >Nghề nghiệp *</label
          >
          <select
            v-model="groupName"
            id="groupName"
            class="custom-select bg-gray-50 border text-sm font-medium border-gray-300 text-gray-900 rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
          >
            <option class="text-lg font-medium text-gray-900" disabled value="">
              Vui lòng chọn nghề nghiệp của bạn
            </option>
            <option
              class="text-lg font-medium text-gray-900"
              v-for="group in groups"
              :key="group.id"
              :value="group.group_name"
            >
              {{ group.group_name }}
            </option>
          </select>
        </div>
        <button type="submit" class="btn-submit">Đăng ký</button>
      </form>
      <el-loading :fullscreen="true" v-if="loading"></el-loading>
    </div>
  </div>
</template>
<script>
import { mapState, mapActions, mapMutations } from 'vuex'
import { ElNotification, ElLoading } from 'element-plus'
export default {
  data() {
    return {
      username: '',
      password: '',
      confirmPassword: '',
      name: '',
      email: '',
      phone: '',
      groupName: '',
      loading: false
    }
  },
  computed: {
    ...mapState({
      groups: (state) => state.register.groups,
      registrationMessage: (state) => state.register.registrationMessage,
      showSuccessNotification: (state) => state.register.showSuccessNotification,
      showErrorModal: (state) => state.register.showErrorModal,
      response: (state) => state.register.response
    })
  },
  methods: {
    ...mapActions(['fetchGroups', 'register']),
    ...mapMutations(['SET_SHOW_ERROR_MODAL']),
    closeErrorModal() {
      this.SET_SHOW_ERROR_MODAL(false)
    },
    async handleRegistration() {
      try {
        this.loading = true
        await this.register({
          username: this.username,
          password: this.password,
          confirmPassword: this.confirmPassword,
          email: this.email,
          phone: this.phone,
          group_name: this.groupName,
          name: this.name
        })
        if (this.response.flag) {
          ElNotification({
            type: 'success',
            title: 'Thông báo',
            message: this.response.message
          })
          this.$router.push('/login')
        }
      } catch (error) {
        console.error('Error registering:', error)
        
        // Handle error display here if needed
      }
    }
  },
  created() {
    this.fetchGroups()
  }
}
</script>

<style scoped>
.input-field {
  border: 1px solid #d1d5db;
  padding: 10px;
  width: 100%;
  box-sizing: border-box;
  border-radius: 4px;
  transition:
    border-color 0.3s,
    box-shadow 0.3s;
}

.input-field:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.3);
}

.custom-select {
  border: 1px solid #d1d5db;
  padding: 10px;
  width: 100%;
  box-sizing: border-box;
  border-radius: 4px;
  transition:
    border-color 0.3s,
    box-shadow 0.3s;
  font-size: 1rem; /* Increase font size */
}

.custom-select:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.3);
}

.custom-select option {
  padding: 10px;
  font-size: 1rem; /* Increase font size */
}

.btn-submit {
  background-color: #3b82f6;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition:
    background-color 0.3s,
    transform 0.3s;
  display: inline-block;
  width: 100%;
  text-align: center;
}

.btn-submit:hover {
  background-color: #2563eb;
  transform: translateY(-2px);
}
</style>
