// src/store/index.js
import { createStore } from 'vuex'
import authModule from './modules/auth'
import header from './modules/header'
const store = createStore({
  modules: {
    auth: authModule,
    header: header
  }
})

export default store
