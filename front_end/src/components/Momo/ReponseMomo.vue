<template>
  <div class="container mx-auto px-4 min-h-screen flex items-center justify-center">
    <div class="w-full md:w-3/5 lg:w-2/5 bg-white p-8 border border-gray-300">
      <h1 class="text-4xl font-bold mb-6 text-center text-gray-800">{{ Alert }}</h1>
      <div v-if="loading" class="text-center">
        <p class="text-lg font-medium text-gray-600">Loading...</p>
      </div>
      <div v-else>
        <div v-if="error" class="text-red-500 mb-6 text-center">
          <p class="text-lg font-medium">{{ error }}</p>
        </div>
        <div v-else>
          <div v-if="responseData" class="space-y-6">
            <h2 class="font-semibold text-4xl mb-16 text-gray-800 text-center">
              🎉 Cảm ơn bạn đã đóng góp cho dự án!
            </h2>
            <p class="font-semibold text-xl text-gray-700">
              Tên người thanh toán: <span class="font-normal">{{ responseData.extraData }}</span>
            </p>
            <p class="font-semibold text-xl text-gray-700">
              Nội dung thanh toán: <span class="font-normal">{{ responseData.orderInfo }}</span>
            </p>
            <p class="font-semibold text-xl text-gray-700">
              Số tiền thanh toán: <span class="font-normal">{{ responseData.amount }}</span>
            </p>
            <p class="font-semibold text-xl text-gray-700">
              Mã đơn hàng: <span class="font-normal">{{ responseData.orderId }}</span>
            </p>
          </div>
          <button
            @click="navigateToBlog"
            class="bg-blue-500 text-white py-3 px-6 mt-8 w-full hover:bg-blue-600 transition duration-200"
          >
            Trở về trang Dự án
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

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
      this.loading = true
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
            message: 'Đã hủy thanh toán'
          })
          setTimeout(() => {
            this.$router.push({ path: `/project/${localStorage.getItem('nameProject')}` })
          }, 2000)
        } else {
          await axios.post(`api/Sponsor/add_sponsor`, {
            nameProject: localStorage.getItem('nameProject'),
            name: this.responseData.extraData,
            contact: localStorage.getItem('email'),
            address: localStorage.getItem('address'),
            contributionAmount: this.responseData.amount
          })
          localStorage.removeItem('name')
          localStorage.removeItem('email')
          localStorage.removeItem('address')
          this.$notify({
            type: 'success',
            title: 'Thông báo',
            message: this.responseData.localMessage
          })
          console.log(localStorage.getItem('nameProject'))
          console.log(decodeURIComponent(localStorage.getItem('nameProject')))
        }
      } catch (error) {
        this.error = 'Có lỗi xảy ra khi lấy dữ liệu từ URL.'
        console.error('Error fetching data from URL:', error)
      } finally {
        this.loading = false
      }
    },
    navigateToBlog() {
      this.$router.push('/project')
    }
  }
}
</script>

<style scoped>
body {
  background-color: #f3f4f6;
}

.container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
}

button {
  font-size: 16px;
  font-weight: 600;
}
</style>
