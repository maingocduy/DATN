import './input.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import CKEditor from '@ckeditor/ckeditor5-vue'
import './Helper/axios'
import store from './store'
import PrimeVue from 'primevue/config'
import ElementPlus from 'element-plus'
import 'primeicons/primeicons.css'
import InputOtp from 'primevue/inputotp'
import Carousel from 'primevue/carousel'
import 'element-plus/dist/index.css'
const app = createApp(App)

app.use(store)
app.use(router)
app.use(ElementPlus)
app.use(PrimeVue, {
  unstyled: true
})
app.component('InputOtp', InputOtp)
app.component('Carousel', Carousel)
app.use(CKEditor).config.compilerOptions.isCustomElement = (tag) => tag.startsWith('ck')
app.mount('#app')
