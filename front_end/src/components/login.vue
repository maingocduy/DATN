// src/components/Login.vue
<template>
  <div
    class="bg-gradient-to-r min-h-screen from-blue-500 to-green-500 flex flex-col justify-center py-12 px-6"
  >
    <!-- Success Notification -->
    <div
      v-if="showSuccessNotification"
      class="fixed top-0 inset-x-0 p-4 bg-green-500 text-white text-center z-50"
    >
      Đặt lại mật khẩu thành công!
    </div>

    <div class="mt-8 sm:mx-auto">
      <div class="bg-white py-8 px-6 shadow sm:rounded-lg sm:px-10">
        <h2 class="text-center text-3xl font-extrabold text-gray-900 mb-6">Đăng nhập</h2>
        <form @submit.prevent="loginAsync" class="max-w-sm mx-auto">
          <div class="mb-5">
            <label for="Username" class="block mb-2 text-sm font-medium text-gray-900"
              >Tên người dùng</label
            >
            <input
              type="text"
              v-model="username"
              id="Username"
              class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              placeholder="name@flowbite.com"
              required
            />
          </div>
          <div class="mb-5">
            <label for="password" class="block mb-2 text-sm font-medium text-gray-900"
              >Mật khẩu của bạn</label
            >
            <input
              type="password"
              v-model="password"
              id="password"
              class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
              required
            />
          </div>
          <button type="submit" class="btn-submit">Đăng nhập</button>
          <button
            type="button"
            @click="openForgotPasswordPopup"
            class="text-blue-500 hover:underline mt-2 block"
          >
            Quên mật khẩu?
          </button>
        </form>
      </div>
    </div>

    <!-- Popup nhập OTP -->
    <div
      v-if="showForgotPasswordPopup"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
    >
      <div class="bg-white p-8 rounded-lg shadow-lg">
        <h2 class="text-xl font-bold mb-4">Nhập Email</h2>
        <input
          v-if="!otpSent"
          type="email"
          v-model="forgotPasswordEmail"
          placeholder="Nhập email của bạn"
          class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
        />
        <button v-if="!otpSent" @click="sendForgotPasswordAsync" class="btn-submit">Gửi OTP</button>
        <input
          v-if="otpSent && !otpVerified"
          type="text"
          v-model="otp"
          placeholder="Nhập mã OTP gồm 6 số"
          class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
        />
        <button v-if="otpSent && !otpVerified" @click="verifyOTP" class="btn-submit">
          Xác nhận
        </button>
        <div v-if="otpVerified">
          <input
            type="password"
            v-model="newPassword"
            placeholder="Nhập mật khẩu mới"
            class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
          />
          <input
            type="password"
            v-model="confirmNewPassword"
            placeholder="Xác nhận mật khẩu mới"
            class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
          />
          <button @click="resetPassword" class="btn-submit">Đặt lại mật khẩu</button>
        </div>
        <button @click="closeForgotPasswordPopup" class="btn-cancel">Hủy</button>
        <p v-if="forgotPasswordMessage" class="mt-4 text-center text-red-600">
          {{ forgotPasswordMessage }}
        </p>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from 'vuex'

export default {
  data() {
    return {
      username: '',
      password: '',
      forgotPasswordEmail: '',
      otp: ''
    }
  },
  computed: {
    ...mapState({
      loading: (state) => state.auth.loading,
      showForgotPasswordPopup: (state) => state.auth.showForgotPasswordPopup,
      forgotPasswordEmail: (state) => state.auth.forgotPasswordEmail,
      forgotPasswordMessage: (state) => state.auth.forgotPasswordMessage,
      otp: (state) => state.auth.otp,
      otpSent: (state) => state.auth.otpSent,
      otpVerified: (state) => state.auth.otpVerified,
      newPassword: (state) => state.auth.newPassword,
      confirmNewPassword: (state) => state.auth.confirmNewPassword,
      showSuccessNotification: (state) => state.auth.showSuccessNotification
    })
  },
  methods: {
    ...mapActions([
      'login',
      'sendForgotPassword',
      'verifyOTP',
      'resetPassword',
      'displaySuccessNotification',
      'openForgotPasswordPopup',
      'closeForgotPasswordPopup'
    ]),
    async loginAsync() {
      await this.login({
        username: this.username,
        password: this.password
      })
      if (this.$store.getters['isAuthenticated']) {
        this.$router.push('/blog')
      }
    },
    openForgotPassword() {
      this.openForgotPasswordPopup()
    },
    async sendForgotPasswordAsync() {
      const response = await this.sendForgotPassword({
        email: this.forgotPasswordEmail
      })
    }
  }
}
</script>

<style scoped>
.input-field {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 0.375rem;
  transition:
    border-color 0.15s ease-in-out,
    box-shadow 0.15s ease-in-out;
  outline: none;
}

.input-field:focus {
  border-color: #4f46e5;
  box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.2);
}

.btn-submit {
  background-color: #4f46e5;
  color: #fff;
  font-weight: bold;
  padding: 0.75rem 1.5rem;
  border-radius: 0.375rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.btn-submit:hover {
  background-color: #4338ca;
}

.btn-cancel {
  background-color: #d1d5db;
  color: #1f2937;
  font-weight: bold;
  padding: 0.75rem 1.5rem;
  border-radius: 0.375rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}
.btn-cancel:hover {
  background-color: #9ca3af;
}
</style>
