<template>
  <div class="container mx-auto mt-8">
    <form @submit.prevent="submitForm" class="max-w-md mx-auto">
      <!-- Các trường nhập dữ liệu -->
      <div class="mb-4">
        <label for="fullName" class="block text-gray-700 text-sm font-bold mb-2"
          >Tên khách hàng</label
        >
        <input
          v-model="fullName"
          type="text"
          id="fullName"
          name="fullName"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>
      <div class="mb-4">
        <label for="email" class="block text-gray-700 text-sm font-bold mb-2">Email</label>
        <input
          v-model="email"
          type="email"
          id="email"
          name="email"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>
      <div class="mb-4">
        <label for="address" class="block text-gray-700 text-sm font-bold mb-2">Địa chỉ</label>
        <input
          v-model="address"
          type="text"
          id="address"
          name="address"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>
      <div class="mb-4">
        <label for="amount" class="block text-gray-700 text-sm font-bold mb-2">Số tiền</label>
        <input
          v-model="amount"
          type="text"
          id="amount"
          name="amount"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>
      <div class="mb-4">
        <label for="orderInfo" class="block text-gray-700 text-sm font-bold mb-2"
          >Nội dung thanh toán</label
        >
        <textarea
          v-model="orderInfo"
          id="orderInfo"
          name="orderInfo"
          rows="3"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        ></textarea>
      </div>
      <!-- Nút submit -->
      <div class="flex items-center justify-between">
        <button
          type="submit"
          class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          Thanh toán (Checkout)
        </button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  data() {
    return {
      fullName: '',
      email: '', // Thêm trường email vào data
      address: '', // Thêm trường address vào data
      amount: '',
      orderInfo: ''
    }
  },
  methods: {
    async submitForm() {
      // Log dữ liệu đã nhập vào các trường
      console.log('Tên khách hàng:', this.fullName)
      console.log('Email:', this.email) // Log email
      console.log('Địa chỉ:', this.address) // Log địa chỉ
      console.log('Số tiền:', this.amount)
      console.log('Nội dung thanh toán:', this.orderInfo)
      // Tiếp tục gửi dữ liệu form đến server
      try {
        localStorage.setItem('email', this.email)
        localStorage.setItem('address', this.address)
        const response = await axios.post(
          `https://localhost:7188/api/Momo/${this.$route.params.name}`,
          {
            // Thêm địa chỉ vào dữ liệu gửi đi
            fullName: this.fullName,
            orderInfo: this.orderInfo,
            amount: this.amount
          }
        )
        // Xử lý phản hồi từ server
        window.location.href = response.data.payUrl
      } catch (error) {
        console.error('Error submitting payment:', error)
        // Xử lý lỗi khi gửi dữ liệu đến server
      }
    }
  }
}
</script>
