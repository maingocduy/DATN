<template>
  <div
    class="bg-gradient-to-r min-h-screen from-blue-500 to-green-500 flex flex-col justify-center py-12 px-6"
  >
    <div class="mt-8 sm:mx-auto">
      <div class="bg-white py-8 px-6 shadow sm:rounded-lg sm:px-10">
        <h2 class="text-center text-3xl font-extrabold text-gray-900 mb-6">Đăng nhập</h2>
        <form @submit.prevent="loginAsync" class="max-w-sm mx-auto">
          <div class="mb-5">
            <label for="Username" class="block mb-2 text-sm font-medium text-gray-900"
              >Tên đăng nhập</label
            >
            <input
              type="text"
              v-model="username"
              id="Username"
              class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
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
          v-model="emailInput"
          placeholder="Nhập email của bạn"
          class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
        />
        <button v-if="!otpSent" @click="sendForgotPasswordAsync" class="btn-submit">Gửi OTP</button>
        <input
          v-if="otpSent && !otpVerified"
          type="text"
          v-model="otps"
          placeholder="Nhập mã OTP gồm 6 số"
          class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
        />
        <button v-if="otpSent && !otpVerified" @click="verifyOTPAsync" class="btn-submit">
          Xác thực OTP
        </button>
        <div v-if="otpVerified">
          <h2 class="text-xl font-bold mb-4">Đặt lại mật khẩu</h2>
          <input
            type="password"
            v-model="newPass"
            placeholder="Nhập mật khẩu mới"
            class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
          />
          <input
            type="password"
            v-model="confirmPass"
            placeholder="Xác nhận mật khẩu mới"
            class="input-field bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 mb-4"
          />
          <button @click="resetPasswordAsync" class="btn-submit">Đặt lại mật khẩu</button>
        </div>
        <button @click="closeForgotPassword" class="mt-4 text-red-500 hover:underline">Đóng</button>
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
      emailInput: '',
      otps: '',
      newPass: '',
      confirmPass: ''
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
      showSuccessNotification: (state) => state.auth.showSuccessNotification,
      response: (state) => state.auth.response // Lấy response từ Vuex state
    })
  },
  watch: {
    emailInput(newEmail) {
      this.updateForgotPasswordEmail(newEmail)
    }
  },
  methods: {
    ...mapActions([
      'login',
      'sendForgotPassword',
      'verifyOTP',
      'resetPassword',
      'displaySuccessNotification',
      'openForgotPasswordPopup',
      'closeForgotPasswordPopup',
      'updateForgotPasswordEmail'
    ]),
    async loginAsync() {
      try {
        await this.login({
          username: this.username,
          password: this.password
        })
        console.log(this.response.flag)
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
            text: this.response.message || 'Lỗi hệ thống'
          })
        }
      } catch (error) {
        this.$notify({
          type: 'error',
          title: 'Thông báo',
          text: 'Lỗi hệ thống '
        })
      }
    },
    openForgotPassword() {
      this.openForgotPasswordPopup()
    },
    closeForgotPassword() {
      this.closeForgotPasswordPopup()
    },
    async sendForgotPasswordAsync() {
      try {
        console.log(this.forgotPasswordEmail)
        await this.sendForgotPassword({
          email: this.forgotPasswordEmail
        })
        this.$notify({
          type: 'success',
          title: 'Thông báo',
          text: 'Đã gửi mã OTP!'
        })
      } catch (error) {
        const message = error.response?.data.message || 'Đã xảy ra lỗi khi gửi email!'
        this.$notify({
          type: 'error',
          title: 'Thông báo',
          text: message
        })
      }
    },
    async verifyOTPAsync() {
      try {
        console.log(this.otps)
        await this.verifyOTP({
          otp: this.otps
        })
        this.$notify({
          type: 'success',
          title: 'Thông báo',
          text: 'Xác thực thành công!'
        })
      } catch (error) {
        const message = error.response?.message
        this.$notify({
          type: 'error',
          title: 'Thông báo',
          text: message
        })
      }
    },
    async resetPasswordAsync() {
      try {
        console.log(this.otps)
        await this.resetPassword({
          email: this.emailInput,
          otp: this.otps,
          newPass: this.newPass,
          confirmPass: this.confirmPass
        })
        if (this.forgotPasswordMessage != 'Đặt lại mật khẩu thành công') {
          this.$notify({
            type: 'error',
            title: 'Thông báo',
            text: this.forgotPasswordMessage
          })
        } else {
          this.$notify({
            type: 'success',
            title: 'Thông báo',
            text: this.forgotPasswordMessage
          })
        }
      } catch (error) {
        const message = error.response?.message
        this.$notify({
          type: 'error',
          title: 'Thông báo',
          text: message
        })
      }
    }
  }
}
</script>

<style scoped>
.input-field {
  border: 1px solid #d1d5db;
  padding: 8px;
  width: 100%;
  box-sizing: border-box;
  border-radius: 4px;
  transition: border-color 0.3s;
}

.input-field:focus {
  outline: none;
  border-color: #3b82f6;
}

.btn-submit {
  background-color: #3b82f6;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-submit:hover {
  background-color: #2563eb;
}
</style>
