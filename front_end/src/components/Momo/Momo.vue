<template>
  <div
    class="bg-gradient-to-r from-blue-500 to-green-500 min-h-screen flex items-center justify-center py-12 px-6"
  >
    <div class="max-w-lg w-full bg-white rounded-lg shadow-lg p-8">
      <h2 class="text-center text-3xl font-extrabold text-gray-900 mb-6">Quyên góp</h2>
      <form @submit.prevent="submitForm">
        <div class="mb-5">
          <label for="fullName" class="block mb-2 text-sm font-medium text-gray-900"
            >Tên khách hàng *</label
          >
          <input
            type="text"
            v-model="fullName"
            id="fullName"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            placeholder="Nhập tên khách hàng"
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
          <label for="address" class="block mb-2 text-sm font-medium text-gray-900">Địa chỉ</label>
          <input
            type="text"
            v-model="address"
            id="address"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            placeholder="Nhập địa chỉ"
          />
        </div>
        <div class="mb-5">
          <label for="amount" class="block mb-2 text-sm font-medium text-gray-900">Số tiền *</label>
          <input
            type="text"
            v-model="amount"
            id="amount"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            placeholder="Nhập số tiền"
          />
        </div>
        <div class="mb-5">
          <label for="orderInfo" class="block mb-2 text-sm font-medium text-gray-900"
            >Nội dung thanh toán</label
          >
          <textarea
            v-model="orderInfo"
            id="orderInfo"
            rows="3"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5"
            placeholder="Nhập nội dung thanh toán"
          ></textarea>
        </div>
        <button
          type="submit"
          class="text-white bg-gradient-to-r from-blue-600 to-blue-800 hover:from-blue-700 hover:to-blue-900 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center"
        >
          Thanh toán
        </button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import Cookies from 'js-cookie'
import { ElNotification } from 'element-plus'
export default {
  data() {
    return {
      fullName: '',
      email: '',
      address: '',
      amount: '',
      orderInfo: ''
    }
  },
  mounted() {
    this.GetAccInfor()
  },
  methods: {
    isValidEmail(email) {
      const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/
      return emailPattern.test(email)
    },
    async submitForm() {
      console.log('Tên khách hàng:', this.fullName)
      console.log('Email:', this.email)
      console.log('Địa chỉ:', this.address)
      console.log('Số tiền:', this.amount)
      console.log('Nội dung thanh toán:', this.orderInfo)

      try {
        if (!this.fullName) {
          ElNotification({
            title: 'Lỗi',
            message: 'Tên người dùng là bắt buộc không để trống',
            type: 'error'
          })
          return
        } else if (!this.email) {
          ElNotification({
            title: 'Lỗi',
            message: 'Email không được để trống !',
            type: 'error'
          })
          return
        } else if (!this.isValidEmail(this.email)) {
          ElNotification({
            title: 'Lỗi',
            message: 'Email sai định dạng !',
            type: 'error'
          })
          return
        } else if (!this.amount) {
          ElNotification({
            title: 'Lỗi',
            message: 'Số tiền quyên góp không được để trống !',
            type: 'error'
          })
          return
        }
        localStorage.setItem('email', this.email)
        localStorage.setItem('address', this.address)
        const response = await axios.post(`https://localhost:7188/api/Momo/momo`, {
          fullName: this.fullName,
          orderInfo: this.orderInfo,
          amount: this.amount
        })
        localStorage.setItem('nameProject', this.$route.params.name)
        window.location.href = response.data.payUrl
      } catch (error) {
        console.error('Error submitting payment:', error)
      }
    },
    async GetAccInfor() {
      if (Cookies.get('username') && Cookies.get('token') && Cookies.get('role')) {
        try {
          const response = await axios.get('https://localhost:7188/api/account/Username', {
            params: {
              username: Cookies.get('username')
            }
          })
          this.fullName = response.data.member.name
          this.email = response.data.member.email
        } catch (error) {
          console.error('Error fetching account information:', error)
        }
      }
    }
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
