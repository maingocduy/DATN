<template>
  <div
    class="bg-gradient-to-r from-blue-500 to-green-500 min-h-screen flex items-center justify-center"
  >
    <div
      class="flex flex-row bg-white rounded-lg shadow-lg overflow-hidden transform transition-transform duration-300"
    >
      <div class="flex-1 p-8 md:p-16">
        <h2 class="text-3xl font-extrabold text-gray-900 mb-6 text-center">Đăng nhập</h2>
        <form @submit.prevent="loginAsync" class="space-y-6">
          <div>
            <label for="Username" class="block text-sm font-medium text-gray-700"
              >Tên đăng nhập</label
            >
            <input type="text" v-model="username" id="Username" class="input-field mt-1" />
          </div>
          <div>
            <label for="password" class="block text-sm font-medium text-gray-700"
              >Mật khẩu của bạn</label
            >
            <input type="password" v-model="password" id="password" class="input-field mt-1" />
          </div>
          <div class="items-center justify-between">
            <button type="submit" class="btn-submit">Đăng nhập</button>
            <button
              type="button"
              @click="openForgotPasswordPopup"
              class="text-sm w-full mt-3 text-blue-600 hover:underline"
            >
              Quên mật khẩu?
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Popup nhập OTP -->
    <div
      v-if="showForgotPasswordPopup"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
    >
      <div class="bg-white w-full max-w-md p-8 rounded-lg shadow-lg">
        <h2 class="text-xl font-bold mb-4 text-center" v-if="!otpSent">Nhập Email</h2>
        <h2 class="text-xl font-bold mb-4 text-center" v-else-if="!otpVerified">Nhập OTP</h2>
        <h2 class="text-xl font-bold mb-4 text-center" v-else>Đặt lại mật khẩu</h2>

        <div v-if="!otpSent">
          <input
            type="email"
            v-model="emailInput"
            placeholder="Nhập email của bạn"
            class="input-field mb-4"
          />
          <button @click="sendForgotPasswordAsync" class="btn-submit w-full">Gửi OTP</button>
        </div>

        <div v-else-if="otpSent && !otpVerified">
          <div class="card flex justify-center mb-2">
            <InputOtp v-model="otps" :length="6">
              <template #default="{ attrs, events }">
                <input type="text" v-bind="attrs" v-on="events" class="custom-otp-input rounded" />
              </template>
            </InputOtp>
          </div>
          <button @click="verifyOTPAsync" class="btn-submit w-full">Xác thực OTP</button>
        </div>

        <div v-else>
          <input
            type="password"
            v-model="newPass"
            placeholder="Nhập mật khẩu mới"
            class="input-field mb-4"
          />
          <input
            type="password"
            v-model="confirmPass"
            placeholder="Xác nhận mật khẩu mới"
            class="input-field mb-4"
          />
          <button @click="resetPasswordAsync" class="btn-submit w-full">Đặt lại mật khẩu</button>
        </div>

        <button @click="closeForgotPassword" class="btn-close mt-4 w-full">Đóng</button>
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
        if (this.response.flag) {
          this.$notify({
            type: 'success',
            title: 'Thông báo',
            text: this.response.message
          })
          this.$router.push('/')
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
      this.emailInput = ''
      this.otps = ''
      this.newPass = ''
      this.confirmPass = ''
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
          text: this.forgotPasswordMessage
        })
      } catch (error) {
        const message = error.response?.data.message || 'Đã xảy ra lỗi khi gửi email!'
        console.log(message)
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
        const message = error.response?.data.message
        console.log(message)
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
        console.log(this.forgotPasswordMessage)
        if (this.forgotPasswordMessage == 'Đặt lại mật khẩu thành công') {
          this.$notify({
            type: 'success',
            title: 'Thông báo',
            text: this.forgotPasswordMessage
          })
        } else {
          this.$notify({
            type: 'error',
            title: 'Thông báo',
            text: this.forgotPasswordMessage
          })
        }
        this.forgotPasswordMessage = ''
      } catch (error) {
        const message = error.response?.data.message
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

.btn-close {
  background-color: #ec2121;
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

.btn-close:hover {
  background-color: rgb(240 82 82);
  transform: translateY(-2px);
}
.custom-otp-input {
  width: 53px;
  font-size: 36px;
  text-align: center;
  transition: all 0.2s;
  background: transparent;
  padding: 10px;
  margin-right: 8px;
}
.custom-otp-input:last-child {
  margin-right: 0; /* Remove margin from the last input to avoid extra space */
}
.custom-otp-input:focus {
  outline: 0 none;
  border-bottom-color: var(--primary);
}
</style>
