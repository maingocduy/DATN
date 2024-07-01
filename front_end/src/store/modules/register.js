import axios from 'axios'

const registerModule = {
  state: () => ({
    response: [],
    username: '',
    password: '',
    confirmPassword: '',
    name: '',
    email: '',
    phone: '',
    group_name: '',
    groups: [],
    registrationMessage: '',
    showSuccessNotification: false,
    showErrorModal: false
  }),
  mutations: {
    SET_RESPONSE(state, payload) {
      state.response = payload
    },
    SET_USERNAME(state, username) {
      state.username = username
    },
    SET_PASSWORD(state, password) {
      state.password = password
    },
    SET_CONFIRM_PASSWORD(state, confirmPassword) {
      state.confirmPassword = confirmPassword
    },
    SET_NAME(state, name) {
      state.name = name
    },
    SET_EMAIL(state, email) {
      state.email = email
    },
    SET_PHONE(state, phone) {
      state.phone = phone
    },
    SET_GROUP_NAME(state, group_name) {
      state.group_name = group_name
    },
    SET_GROUPS(state, payload) {
      state.groups = payload
    },
    SET_REGISTRATION_MESSAGE(state, message) {
      state.registrationMessage = message
    },
    SET_SHOW_SUCCESS_NOTIFICATION(state, status) {
      state.showSuccessNotification = status
    },
    SET_SHOW_ERROR_MODAL(state, status) {
      state.showErrorModal = status
    }
  },
  actions: {
    async fetchGroups({ commit }) {
      try {
        const response = await axios.get('https://localhost:7188/api/Group')
        commit('SET_GROUPS', response.data)
      } catch (error) {
        console.error('Không thể lấy danh sách nhóm:', error)
      }
    },
    async register(
      { commit },
      { username, password, confirmPassword, name, email, phone, group_name }
    ) {
      if (!username || !password || !confirmPassword || !email || !phone || !group_name) {
        commit('SET_REGISTRATION_MESSAGE', 'Vui lòng điền đầy đủ thông tin các trường bắt buộc!')
        commit('SET_SHOW_ERROR_MODAL', true)
        return
      } else if (!isValidEmail(email)) {
        commit('SET_REGISTRATION_MESSAGE', 'Email không đúng định dạng. Vui lòng kiểm tra lại!')
        commit('SET_SHOW_ERROR_MODAL', true)
        return
      } else if (!isValidPhone(phone)) {
        commit('SET_REGISTRATION_MESSAGE', 'Số điện thoại không hợp lệ!')
        commit('SET_SHOW_ERROR_MODAL', true)
        return
      } else if (password.length < 6) {
        commit('SET_REGISTRATION_MESSAGE', 'Mật khẩu chứa ít nhất 6 ký tự !')
        commit('SET_PASSWORD', '')
        commit('SET_CONFIRM_PASSWORD', '')
        commit('SET_SHOW_ERROR_MODAL', true)
        return
      } else if (username.includes(' ')) {
        commit('SET_REGISTRATION_MESSAGE', 'Tên đăng nhập không được có khoảng trắng!')
        commit('SET_USERNAME', '')
        commit('SET_SHOW_ERROR_MODAL', true)

        return
      } else if (username.length < 6) {
        commit('SET_REGISTRATION_MESSAGE', 'Tên đăng nhập chứa ít nhất 6 ký tự!')
        commit('SET_SHOW_ERROR_MODAL', true)
        commit('SET_USERNAME', '')
        return
      } else if (password !== confirmPassword) {
        commit('SET_REGISTRATION_MESSAGE', 'Mật khẩu không trùng khớp!')
        commit('SET_PASSWORD', '')
        commit('SET_CONFIRM_PASSWORD', '')
        commit('SET_SHOW_ERROR_MODAL', true)
        return
      }

      try {
        const response = await axios.post('https://localhost:7188/api/Auth/Register', {
          username,
          password,
          confirmPassword,
          name,
          email,
          phone,
          group_name
        })
        commit('SET_RESPONSE', response.data)
        commit('SET_REGISTRATION_MESSAGE', response.data.message)
        commit('SET_SHOW_SUCCESS_NOTIFICATION', response.data.message)
        setTimeout(() => {
          commit('SET_SHOW_SUCCESS_NOTIFICATION', false)
        }, 3000)
      } catch (error) {
        console.error('Đăng ký thất bại:', error)
        commit('SET_REGISTRATION_MESSAGE', error.response.data.message)
        commit('SET_SHOW_ERROR_MODAL', true)
      }
    }
  },
  getters: {
    getUsername: (state) => state.username,
    getPassword: (state) => state.password,
    getConfirmPassword: (state) => state.confirmPassword,
    getName: (state) => state.name,
    getEmail: (state) => state.email,
    getPhone: (state) => state.phone,
    getGroupName: (state) => state.groupName,
    getGroups: (state) => state.groups,
    getRegistrationMessage: (state) => state.registrationMessage,
    getShowSuccessNotification: (state) => state.showSuccessNotification,
    getShowErrorModal: (state) => state.showErrorModal
  }
}

function isValidEmail(email) {
  const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/
  return emailPattern.test(email)
}

function isValidPhone(phone) {
  const phonePattern = /^[0-9]{10,11}$/
  return phonePattern.test(phone)
}

export default registerModule
