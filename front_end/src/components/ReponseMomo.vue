<template>
  <div class="container mx-auto mt-8">
    <div class="max-w-md mx-auto">
      <h1 class="text-3xl font-bold mb-4">{{ Alert }}</h1>
      <div v-if="loading" class="text-center">
        <p>Loading...</p>
      </div>
      <div v-else>
        <div v-if="error" class="text-red-500 mb-4">
          <p>{{ error }}</p>
        </div>
        <div v-else>
          <div v-if="responseData" class="mb-4">
            <p class="font-bold">Tên người thanh toán: {{ responseData.extraData }}</p>
            <p class="font-bold">Nội dung thanh toán: {{ responseData.orderInfo }}</p>
            <p class="font-bold">Số tiền thanh toán: {{ responseData.amount }}</p>
            <p class="font-bold">Mã đơn hàng: {{ responseData.orderId }}</p>
            <!-- Thêm các thông tin khác cần hiển thị từ responseData -->
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import Toasted from 'vue-toasted' // Import Toasted

export default {
  data() {
    return {
      loading: false,
      error: null,
      responseData: null,
      Alert: ''
    }
  },
  mounted() {
    this.getData() // Khởi tạo Toasted
  },
  methods: {
    async getData() {
      try {
        // Lấy thông tin từ query params
        const {
          partnerCode,
          accessKey,
          requestId,
          amount,
          orderId,
          orderInfo,
          orderType,
          transId,
          message,
          localMessage,
          responseTime,
          errorCode,
          payType,
          extraData,
          signature,
          storeId
        } = this.$route.query

        // Lưu thông tin vào biến responseData
        this.responseData = {
          partnerCode,
          accessKey,
          requestId,
          amount,
          orderId,
          orderInfo,
          orderType,
          transId,
          message,
          localMessage,
          responseTime,
          errorCode,
          payType,
          extraData,
          signature,
          storeId
        }

        console.log(this.responseData)

        if (this.responseData.localMessage === 'Dữ liệu sai định dạng') {
          this.Alert = 'Đã hủy thanh toán'
        } else {
          this.Alert = 'Đóng góp thành công'
          await axios.post(`api/Sponsor/${this.responseData.storeId}`, {
            name: this.responseData.extraData,
            contact: localStorage.getItem('email'),
            address: localStorage.getItem('address'),
            contributionAmount: this.responseData.amount
          })
          localStorage.removeItem('email')
          localStorage.removeItem('address')
        }
      } catch (error) {
        // Xử lý lỗi nếu có
        this.error = 'Có lỗi xảy ra khi lấy dữ liệu từ URL.'
        console.error('Error fetching data from URL:', error)
      }
    }
  }
}
</script>

<style>
/* Thêm bất kỳ luật CSS tùy chỉnh nào bạn muốn ở đây */
</style>
