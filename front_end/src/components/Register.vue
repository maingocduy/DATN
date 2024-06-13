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
            >Tên người dùng *</label
          >
          <input
            type="text"
            v-model="username"
            id="username"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
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
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
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
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            placeholder="Xác nhận mật khẩu"
          />
        </div>
        <div class="mb-5">
          <label for="name" class="block mb-2 text-sm font-medium text-gray-900">Họ và tên</label>
          <input
            type="text"
            v-model="name"
            id="name"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            placeholder="Nhập họ và tên"
          />
        </div>
        <div class="mb-5">
          <label for="email" class="block mb-2 text-sm font-medium text-gray-900">Email *</label>
          <input
            type="text"
            v-model="email"
            id="email"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
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
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            placeholder="Nhập
          số điện thoại"
          />
        </div>
        <div class="mb-5">
          <label for="groupName" class="block mb-2 text-sm font-medium text-gray-900"
            >Nghề nghiệp *</label
          >
          <select
            v-model="groupName"
            id="groupName"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
          >
            <option disabled value="">Vui lòng chọn nghề nghiệp của bạn</option>
            <option v-for="group in groups" :key="group.id" :value="group.group_name">
              {{ group.group_name }}
            </option>
          </select>
        </div>
        <button
          type="submit"
          class="text-white bg-gradient-to-r from-blue-600 to-blue-800 hover:from-blue-700 hover:to-blue-900 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center"
        >
          Đăng ký
        </button>
      </form>
    </div>
  </div>
</template>
<script>
import { mapState, mapMutations, mapActions } from 'vuex'

export default {
  data() {
    return {
      username: '',
      password: '',
      confirmPassword: '',
      name: '',
      email: '',
      phone: '',
      groupName: ''
    }
  },
  computed: {
    ...mapState({
      username: (state) => state.register.username,
      password: (state) => state.register.password,
      confirmPassword: (state) => state.register.confirmPassword,
      name: (state) => state.register.name,
      email: (state) => state.register.email,
      phone: (state) => state.register.phone,
      group_name: (state) => state.register.groupName,
      groups: (state) => state.register.groups,
      registrationMessage: (state) => state.register.registrationMessage,
      showSuccessNotification: (state) => state.register.showSuccessNotification,
      showErrorModal: (state) => state.register.showErrorModal,
      response: (state) => state.register.response
    })
  },
  methods: {
    ...mapMutations([
      'SET_REGISTRATION_MESSAGE',
      'SET_SHOW_SUCCESS_NOTIFICATION',
      'SET_SHOW_ERROR_MODAL'
    ]),
    ...mapActions(['fetchGroups', 'register']),
    closeErrorModal() {
      this.SET_SHOW_ERROR_MODAL(false)
    },
    async handleRegistration() {
      try {
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
          this.$notify({
            type: 'success',
            title: 'Thông báo',
            text: this.response.message
          })
          this.$router.push('/blog')
        } else {
          this.$notify({
            type: 'error',
            title: 'Thông báo',
            text: this.response.message
          })
        }
      } catch (error) {
        console.error('Error registering:', error)
        // Handle error display here if needed
      }
    }
  },
  created() {
    this.fetchGroups()
    console.log(this.groups)
  }
}
</script>
<style scoped>
/* CSS tùy chỉnh cho các thành phần */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter, .fade-leave-to /* .fade-leave-active in <2.1.8 */ {
  opacity: 0;
}

.fixed.inset-0.flex.items-center.justify-center.z-50.bg-black.bg-opacity-50 {
  backdrop-filter: blur(5px);
}

.bg-white.p-6.rounded-lg.shadow-lg.w-96.relative {
  animation: scaleUp 0.3s ease-in-out;
}

@keyframes scaleUp {
  0% {
    transform: scale(0.8);
  }
  100% {
    transform: scale(1);
  }
}

.absolute.top-2.right-2.bg-transparent.text-gray-500.hover\:text-gray-700.text-xl {
  cursor: pointer;
  font-weight: bold;
}

.bg-red-500.text-white.px-4.py-2.rounded.hover\:bg-red-600.w-full {
  transition: background-color 0.3s ease;
}
</style>
