<template>
  <div class="min-h-screen flex flex-col items-center justify-center bg-gray-100">
    <!-- Nội dung chính -->
    <div class="text-center px-4 sm:px-6 lg:px-8">
      <h1 class="text-3xl font-bold mb-6 text-indigo-600">{{ Alert }}</h1>
      <div v-if="loading" class="text-center">
        <p>Loading...</p>
      </div>
      <div v-else>
        <div v-if="error" class="text-red-500 mb-4 text-center">
          <p>{{ error }}</p>
        </div>
        <div v-else>
          <div v-if="responseData" class="mb-4">
            <p class="font-bold text-2xl mb-4 text-green-600">
              🎉 Chúc mừng bạn đã xác thực Email thành công!
            </p>
            <p class="font-bold text-xl mb-2">
              Tên đăng nhập: <span class="text-gray-700">{{ responseData.user }}</span>
            </p>
            <p class="font-medium text-lg mb-4 text-gray-700">
              Giờ bạn đã có thể đăng nhập vào tài khoản của bạn. Hãy ấn nút dưới để trở về trang
              đăng nhập.
            </p>
            <a
              href="/login"
              class="inline-block text-center bg-blue-500 text-white py-3 px-6 rounded-lg hover:bg-blue-600 shadow-lg transition-transform transform hover:scale-105"
            >
              Trở về Trang đăng nhập
            </a>
          </div>
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
      loading: true, // Đặt loading thành true khi bắt đầu
      error: null,
      responseData: null,
      Alert: ''
    }
  },
  created() {
    this.getData()
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
      } finally {
        this.loading = false // Đặt loading thành false khi hoàn tất
      }
    }
  }
}
</script>

<style scoped>
/* Đảm bảo footer luôn nằm ở cuối trang hoặc khung hình */
html,
body {
  height: 100%;
  margin: 0;
}

body {
  display: flex;
  flex-direction: column;
  background-color: #f3f4f6;
}

#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.min-h-screen {
  min-height: 100vh;
}

.flex {
  display: flex;
}

.flex-col {
  flex-direction: column;
}

.items-center {
  align-items: center;
}

.justify-center {
  justify-content: center;
}

.bg-gray-100 {
  background-color: #f7fafc;
}
</style>
