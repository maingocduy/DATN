<template>
  <div class="bg-gray-100 flex flex-col justify-center py-12 px-6">
    <div class="sm:max-w-xl sm:mx-auto">
      <h2 class="text-center text-3xl font-extrabold text-gray-900">Sign in to your account</h2>
    </div>
    <div class="mt-8 sm:mx-auto">
      <div class="bg-white py-8 px-6 shadow sm:rounded-lg sm:px-10">
        <form @submit.prevent="login" class="max-w-sm mx-auto">
          <div class="mb-5">
            <label
              for="Username"
              class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
              >Username</label
            >
            <input
              type="Username"
              v-model="username"
              id="Username"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder="name@flowbite.com"
              required
            />
          </div>
          <div class="mb-5">
            <label
              for="password"
              class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
              >Your password</label
            >
            <input
              type="password"
              v-model="password"
              id="password"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              required
            />
          </div>

          <button
            type="submit"
            class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
          >
            Submit
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import Cookies from 'js-cookie'

export default {
  data() {
    return {
      username: '',
      password: '',
      loading: false
    }
  },
  methods: {
    async login() {
      this.loading = true
      try {
        const response = await axios.post('https://localhost:7188/api/Auth/login', {
          username: this.username,
          password: this.password
        })
        const token = response.data.token
        Cookies.set('token', token)
        this.$router.push('/blog')
      } catch (error) {
        console.error('Login failed:', error)
      } finally {
        this.loading = false
      }
    }
  }
}
</script>

<style scoped>
/* Thêm CSS tùy chỉnh nếu cần */
</style>
