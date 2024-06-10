import Cookies from 'js-cookie'

const header = {
  state: () => ({
    token: null,
    username: null,
    role: null
  }),

  mutations: {
    SET_TOKEN(state, token) {
      state.token = token
    },
    SET_USERNAME(state, username) {
      state.username = username
    },
    SET_ROLE(state, role) {
      state.role = role
    }
  },

  actions: {
    setAuthData({ commit }, { token, username, role }) {
      commit('SET_TOKEN', token)
      commit('SET_USERNAME', username)
      commit('SET_ROLE', role)
    },
    initializeAuth({ commit }) {
      const token = Cookies.get('token')
      const username = Cookies.get('username')
      const role = Cookies.get('role')
      if (token) {
        commit('SET_TOKEN', token)
        commit('SET_USERNAME', username)
        commit('SET_ROLE', role)
      }
    },
    logout({ commit }) {
      commit('SET_TOKEN', null)
      commit('SET_USERNAME', null)
      commit('SET_ROLE', null)
      Cookies.remove('token')
      Cookies.remove('username')
      Cookies.remove('role')
    }
  },

  getters: {
    isAuthenticated: (state) => !!state.token,
    username: (state) => state.username,
    role: (state) => state.role
  }
}

export default header
