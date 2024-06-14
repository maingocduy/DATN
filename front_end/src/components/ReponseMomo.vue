<template>
  <div class="container mx-auto mt-12 px-4">
    <div class="max-w-md mx-auto bg-white p-6 rounded-lg shadow-lg">
      <h1 class="text-3xl font-bold mb-6 text-center text-gray-800">{{ Alert }}</h1>
      <div v-if="loading" class="text-center">
        <p>Loading...</p>
      </div>
      <div v-else>
        <div v-if="error" class="text-red-500 mb-6 text-center">
          <p>{{ error }}</p>
        </div>
        <div v-else>
          <div v-if="responseData" class="space-y-4">
            <p class="font-semibold text-lg text-gray-700">
              Tên người thanh toán: <span class="font-normal">{{ responseData.extraData }}</span>
            </p>
            <p class="font-semibold text-lg text-gray-700">
              Nội dung thanh toán: <span class="font-normal">{{ responseData.orderInfo }}</span>
            </p>
            <p class="font-semibold text-lg text-gray-700">
              Số tiền thanh toán: <span class="font-normal">{{ responseData.amount }}</span>
            </p>
            <p class="font-semibold text-lg text-gray-700">
              Mã đơn hàng: <span class="font-normal">{{ responseData.orderId }}</span>
            </p>
            <!-- Thêm các thông tin khác cần hiển thị từ responseData -->
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import axios from 'axios'
import Toasted from 'vue-toasted'

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
    this.getData()
  },
  methods: {
    async getData() {
      try {
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
          this.$notify({
            type: 'error',
            title: 'Thông báo',
            text: 'Đã hủy thanh toán'
          })
          setTimeout(() => {
            this.$router.push({ path: `/project/${this.responseData.storeId}` })
          }, 2000)
        } else {
          await axios.post(`api/Sponsor/${this.responseData.storeId}`, {
            name: this.responseData.extraData,
            contact: localStorage.getItem('email'),
            address: localStorage.getItem('address'),
            contributionAmount: this.responseData.amount
          })
          localStorage.removeItem('email')
          localStorage.removeItem('address')
          this.$notify({
            type: 'success',
            title: 'Thông báo',
            text: this.response.localMessage
          })
        }
      } catch (error) {
        this.error = 'Có lỗi xảy ra khi lấy dữ liệu từ URL.'
        console.error('Error fetching data from URL:', error)
      }
    }
  }
}
</script>
<style scoped>
/* Thêm bất kỳ luật CSS tùy chỉnh nào bạn muốn ở đây */
body {
  background-color: #f3f4f6;
}
</style>
