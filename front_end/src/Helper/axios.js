import axios from 'axios'
import Cookies from 'js-cookie'
import store from '../store/index'
import router from '../router/index'
import { ElNotification, ElLoading } from 'element-plus'

// Cấu hình base URL mặc định cho axios
axios.defaults.baseURL = 'https://localhost:7188/'

// Hàm để cập nhật Authorization header với token mới từ Cookies

// Intercept request để cập nhật header Authorization trước khi gửi request
// Intercept response errors để xử lý lỗi 401 và 403

// Tạo instance ElLoading
let loadingService // Khai báo biến loadingService ở mức global
const updateAuthorizationHeader = () => {
  axios.defaults.headers.common['Authorization'] = `Bearer ${Cookies.get('token')}`
}
// Hàm để khởi tạo loadingService
const initLoadingService = () => {
  loadingService = ElLoading.service({
    fullscreen: true,
    text: 'Loading...',
    background: 'rgba(0, 0, 0, 0.7)'
  })
}
axios.interceptors.request.use(
  (config) => {
    if (!loadingService) {
      // Kiểm tra nếu loadingService chưa được khởi tạo
      initLoadingService() // Khởi tạo loadingService nếu chưa tồn tại
    } // Hiển thị loading trước khi gửi yêu cầu
    updateAuthorizationHeader() // Cập nhật Authorization header
    return config
  },
  (error) => {
    if (loadingService) {
      loadingService.close() // Ẩn loading nếu gặp lỗi request
    }
    return Promise.reject(error)
  }
)
axios.interceptors.response.use(
  (response) => {
    if (loadingService) {
      loadingService.close() // Ẩn loading sau khi nhận phản hồi
    }
    return response
  },
  (error) => {
    if (error.response) {
      loadingService.close()
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
