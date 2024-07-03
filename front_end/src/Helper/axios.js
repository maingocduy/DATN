import axios from 'axios'
import Cookies from 'js-cookie'
import store from '../store/index'
import router from '../router/index'
import { ElNotification, ElLoading } from 'element-plus'

// Cấu hình base URL mặc định cho axios
axios.defaults.baseURL = 'https://localhost:7188/'

// Tạo instance ElLoading
let loadingService = null // Khai báo biến loadingService ở mức global
let requestCount = 0 // Biến đếm số lượng yêu cầu đang chờ phản hồi

const showLoading = () => {
  if (requestCount === 0 && !loadingService) {
    loadingService = ElLoading.service({
      fullscreen: true,
      text: 'Loading...',
      background: 'rgba(0, 0, 0, 0.7)'
    })
  }
  requestCount++
}

const hideLoading = () => {
  requestCount--
  if (requestCount === 0 && loadingService) {
    loadingService.close()
    loadingService = null
  }
}

axios.interceptors.request.use(
  (config) => {
    showLoading() // Hiển thị loading trước khi gửi yêu cầu
    config.headers.Authorization = `Bearer ` + Cookies.get('token') // Cập nhật Authorization header
    return config
  },
  (error) => {
    hideLoading() // Ẩn loading nếu gặp lỗi request
    return Promise.reject(error)
  }
)

axios.interceptors.response.use(
  (response) => {
    hideLoading() // Ẩn loading sau khi nhận phản hồi
    return response
  },
  (error) => {
    hideLoading() // Ẩn loading nếu gặp lỗi phản hồi

    if (error.response) {
      if (error.response.status === 401) {
        const token = Cookies.get('token')
        if (!token) {
          ElNotification({
            type: 'error',
            title: 'Thông báo',
            message: 'Bạn cần đăng nhập để sử dụng tính năng này.'
          })
          router.push('/login')
        } else {
          ElNotification({
            type: 'warning',
            title: 'Thông báo',
            message: 'Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.'
          })
          store.dispatch('logout') // Đăng xuất người dùng
          router.push('/login') // Chuyển hướng về trang đăng nhập
        }
      } else if (error.response.status === 403) {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Bạn không có quyền truy cập chức năng này.'
        })
        router.push('/')
      } else if (error.response.status === 500) {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Lỗi hệ thống'
        })
      }
    } else {
      ElNotification({
        type: 'error',
        title: 'Thông báo',
        message: 'Đã xảy ra lỗi mạng hoặc lỗi không xác định'
      })
    }
    return Promise.reject(error)
  }
)

export default axios
