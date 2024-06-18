import './input.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './Helper/axios'
import store from './store'
import PrimeVue from 'primevue/config'
import ElementPlus from 'element-plus'
import 'primeicons/primeicons.css'
import InputOtp from 'primevue/inputotp'
import Carousel from 'primevue/carousel'
import 'element-plus/dist/index.css'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
const app = createApp(App)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(key, component)
}
app.use(store)
app.use(router)
app.use(ElementPlus)
app.use(PrimeVue, {
  unstyled: true
})
app.component('InputOtp', InputOtp)
app.component('Carousel', Carousel)
app.mount('#app')
