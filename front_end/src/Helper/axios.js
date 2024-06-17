import axios from 'axios'
import Cookies from 'js-cookie'
import store from '../store/index'
import router from '../router/index'
import { ElNotification } from 'element-plus'

// Cấu hình base URL mặc định cho axios
axios.defaults.baseURL = 'https://localhost:7188/'

// Hàm để cập nhật Authorization header với token mới từ Cookies
const updateAuthorizationHeader = () => {
  const token = Cookies.get('token')
  if (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
  } else {
    delete axios.defaults.headers.common['Authorization']
  }
}

// Intercept request để cập nhật header Authorization trước khi gửi request
axios.interceptors.request.use(
  (config) => {
    updateAuthorizationHeader()
    return config
  },
  (error) => Promise.reject(error)
)

// Intercept response errors để xử lý lỗi 401 và 403
axios.interceptors.response.use(
  (response) => response,
  (error) => {
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
