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
            <p class="font-bold">Tên người thanh toán: {{ responseData.user }}</p>
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
  created() {
    this.getData() // Khởi tạo Toasted
  },
  methods: {
    async getData() {
      try {
        // Lấy thông tin từ query params
        const { user, code, userId } = this.$route.query

        // Gửi yêu cầu API ConfirmEmail
        const response = await axios.post('https://localhost:7188/api/auth/ConfirmEmail', {
          userID: userId,
          code: code
        })

        // Kiểm tra kết quả và xử lý tùy thuộc vào trạng thái của response
        if (response.status === 200) {
          // Nếu kết quả trả về thành công, cập nhật dữ liệu và hiển thị thông báo
          this.responseData = {
            user,
            code,
            userId
          }
          this.Alert = 'Xác thực Email thành công'
        } else {
          // Nếu có lỗi, xử lý lỗi
          this.error = 'Có lỗi xảy ra khi xác thực Email.'
          console.error('Error confirming email:', response.data)
        }
      } catch (error) {
        // Xử lý lỗi nếu có
        this.error = 'Có lỗi xảy ra khi gửi yêu cầu xác thực Email.'
        console.error('Error confirming email:', error)
      }
    }
  }
}
</script>

<style>
/* Thêm bất kỳ luật CSS tùy chỉnh nào bạn muốn ở đây */
</style>
