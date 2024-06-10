import './input.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import CKEditor from '@ckeditor/ckeditor5-vue'
import './Helper/axios'
import store from './store'
const app = createApp(App)
app.use(store)
app.use(router)
app.use(CKEditor).config.compilerOptions.isCustomElement = (tag) => tag.startsWith('ck')
app.mount('#app')
