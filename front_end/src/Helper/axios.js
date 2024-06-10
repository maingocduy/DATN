import axios from 'axios'
import Cookies from 'js-cookie'

// Lấy JWT từ cookie có tên là 'token'

axios.defaults.baseURL = 'https://localhost:7188/'
axios.defaults.headers.common['Authorization'] = 'Bearer ' + Cookies.get('token')
