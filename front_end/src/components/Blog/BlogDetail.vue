<template>
  <div v-if="blog.account" class="container mx-auto p-4">
    <header class="bg-gray-100 p-6 mb-6 shadow-md rounded-lg">
      <h1 class="text-4xl font-bold text-gray-800 mb-2">{{ blog.title }}</h1>
      <div class="flex justify-between text-gray-600">
        <p><strong>Người đăng:</strong> {{ blog.account.username }}</p>
        <p><strong>Ngày giờ đăng:</strong> {{ formatDate(blog.createdAt) }}</p>
      </div>
    </header>
    <main class="bg-white p-6 shadow-lg rounded-lg">
      <div v-html="blog.content" class="prose max-w-none justify-center"></div>
    </main>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  data() {
    return {
      blog: {}
    }
  },
  created() {
    this.fetchBlog()
  },
  methods: {
    async fetchBlog() {
      const id = this.$route.params.id
      try {
        const response = await axios.get(`/api/Blog/get_blog_by_id?id=${id}`)
        this.blog = response.data
      } catch (error) {
        console.error('Error fetching blog:', error)
      }
    },
    formatDate(dateString) {
      const options = {
        year: 'numeric',
        month: 'numeric',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric'
      }
      const formattedDate = new Date(dateString).toLocaleDateString('vi-VN', options)
      return formattedDate
    }
  }
}
</script>

<style scoped>
/* Add custom loader styles or other necessary styles */
</style>
