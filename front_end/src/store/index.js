// src/store/index.js
import { createStore } from 'vuex'
import authModule from './modules/auth'
import header from './modules/header'
import registerModule from './modules/register'
const store = createStore({
  modules: {
    auth: authModule,
    header: header,
    register: registerModule
  }
})

export default store
