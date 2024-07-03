import axios from 'axios'
import Cookies from 'js-cookie'
import { Exception } from 'sass'

const authModule = {
  state: () => ({
    response: [],
    notify: null,
    token: Cookies.get('token') || '',
    username: Cookies.get('username') || '',
    role: Cookies.get('role') || '',
    loading: false,
    forgotPasswordEmail: '',
    forgotPasswordMessage: '',
    otp: '',
    otpSent: false,
    otpVerified: false,
    newPassword: '',
    confirmNewPassword: '',
    showSuccessNotification: false,
    showForgotPasswordPopup: false
  }),
  mutations: {
    SET_RESPONSE(state, payload) {
      state.response = payload
    },
    SET_LOADING(state, payload) {
      state.loading = payload
    },
    SET_TOKEN(state, token) {
      state.token = token
      Cookies.set('token', token)
    },
    SET_REFRESH_TOKEN(state, rtoken) {
      state.rtoken = rtoken
      Cookies.set('rtoken', rtoken)
    },
    SET_FORGOT_PASSWORD_EMAIL(state, email) {
      state.forgotPasswordEmail = email
    },
    SET_FORGOT_PASSWORD_MESSAGE(state, message) {
      state.forgotPasswordMessage = message
    },
    SET_OTP(state, otp) {
      state.otp = otp
    },
    SET_OTP_SENT(state, status) {
      state.otpSent = status
    },
    SET_OTP_VERIFIED(state, status) {
      state.otpVerified = status
    },
    SET_NEW_PASSWORD(state, password) {
      state.newPassword = password
    },
    SET_CONFIRM_NEW_PASSWORD(state, password) {
      state.confirmNewPassword = password
    },
    SET_ROLE(state, role) {
      state.role = role
      Cookies.set('role', role)
    },
    SET_USERNAME(state, username) {
      state.username = username
      Cookies.set('username', username)
    },
    SET_SHOW_SUCCESS_NOTIFICATION(state, status) {
      state.showSuccessNotification = status
    },
    SET_SHOW_FORGOT_PASSWORD_POPUP(state, status) {
      state.showForgotPasswordPopup = status
    },
    RESET_STATE(state) {
      state.forgotPasswordEmail = ''
      state.otp = ''
      state.otpSent = false
      state.otpVerified = false
      state.newPassword = ''
      state.confirmNewPassword = ''
      state.username = ''
      state.role = ''
      Cookies.remove('username')
      Cookies.remove('role')
    }
  },
  actions: {
    async login({ commit }, { username, password }) {
      commit('SET_LOADING', true)
      try {
        const response = await axios.post('https://localhost:7188/api/Auth/login', {
          username,
          password
        })
        commit('SET_RESPONSE', response.data)
        if (response.data.flag) {
          const token = response.data.token
          const rtoken = response.data.rToken
          commit('SET_REFRESH_TOKEN', rtoken)
          commit('SET_TOKEN', token)
          commit('SET_USERNAME', response.data.name)
          commit('SET_ROLE', response.data.role)
        }
      } catch (error) {
        console.error('Login failed:', error)
      } finally {
        commit('SET_LOADING', false)
      }
    },
    async reSendOtp({ commit, state }, { email }) {
      try {
        commit('SET_FORGOT_PASSWORD_EMAIL', email)
        const response = await axios.post(
          'https://localhost:7188/api/account/re_send_otp',
          JSON.stringify({ email: state.forgotPasswordEmail }),
          {
            headers: {
              'Content-Type': 'application/json'
            }
          }
        )
        console.log(response.data.message)
        commit('SET_FORGOT_PASSWORD_MESSAGE', response.data.message)
        commit('SET_OTP_SENT', true)
      } catch (error) {
        const message = error.response?.data?.message || 'Đã xảy ra lỗi'
        commit('SET_FORGOT_PASSWORD_MESSAGE', message)
        throw error
      }
    },
    async sendForgotPassword({ commit, state }, { email }) {
      try {
        commit('SET_FORGOT_PASSWORD_EMAIL', email)
        const response = await axios.post(
          'https://localhost:7188/api/account/forgot',
          JSON.stringify({ email: state.forgotPasswordEmail }),
          {
            headers: {
              'Content-Type': 'application/json'
            }
          }
        )
        console.log(response.data.message)
        commit('SET_FORGOT_PASSWORD_MESSAGE', response.data.message)
        commit('SET_OTP_SENT', true)
      } catch (error) {
        const message = error.response?.data?.message || 'Đã xảy ra lỗi'
        commit('SET_FORGOT_PASSWORD_MESSAGE', message)
        throw error
      }
    },
    async verifyOTP({ commit, state }, { otp }) {
      try {
        commit('SET_OTP', otp)
        const response = await axios.post('https://localhost:7188/api/account/enter_otp', {
          otp: state.otp
        })
        console.log(response.data.message)
        commit('SET_FORGOT_PASSWORD_MESSAGE', response.data.message)
        commit('SET_OTP_VERIFIED', true)
      } catch (error) {
        const message = error.response?.data?.message || 'Đã xảy ra lỗi'
        commit('SET_FORGOT_PASSWORD_MESSAGE', message)
        throw error
      }
    },
    async resetPassword({ commit, state }, { newPass, confirmPass }) {
      if (newPass.length < 6) {
        commit('SET_FORGOT_PASSWORD_MESSAGE', 'Mật khẩu phải nhiều hơn 6 ký tự')
        return
      }
      commit('SET_NEW_PASSWORD', newPass)
      commit('SET_CONFIRM_NEW_PASSWORD', confirmPass)
      if (state.newPassword !== state.confirmNewPassword) {
        commit('SET_FORGOT_PASSWORD_MESSAGE', 'Mật khẩu không khớp')
        return
      }
      try {
        const response = await axios.post('https://localhost:7188/api/account/changeForgetPass', {
          email: state.forgotPasswordEmail,
          otp: state.otp,
          Password: state.newPassword
        })
        console.log(response.data.message)
        commit('SET_FORGOT_PASSWORD_MESSAGE', response.data.message)
        commit('SET_SHOW_SUCCESS_NOTIFICATION', true)
        commit('RESET_STATE')
        commit('SET_SHOW_FORGOT_PASSWORD_POPUP', false)
      } catch (error) {
        const message = error.response?.data?.message || 'Đã xảy ra lỗi'
        commit('SET_FORGOT_PASSWORD_MESSAGE', message)
        throw error
      }
    },
    displaySuccessNotification({ commit }) {
      commit('SET_SHOW_SUCCESS_NOTIFICATION', true)
      setTimeout(() => {
        commit('SET_SHOW_SUCCESS_NOTIFICATION', false)
      }, 3000)
    },
    openForgotPasswordPopup({ commit }) {
      commit('SET_SHOW_FORGOT_PASSWORD_POPUP', true)
    },
    closeForgotPasswordPopup({ commit }) {
      commit('RESET_STATE')
      commit('SET_SHOW_FORGOT_PASSWORD_POPUP', false)
    },
    updateForgotPasswordEmail({ commit }, email) {
      commit('SET_FORGOT_PASSWORD_EMAIL', email)
    }
  },
  getters: {
    response: (state) => state.response
  }
}

export default authModule
