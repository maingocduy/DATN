<template>
  <div class="container mx-auto mt-8">
    <div class="max-w-7xl mx-auto">
      <h1 class="text-3xl font-bold mb-4 text-center">Danh sách dự án</h1>
      <!-- Nút Thêm dự án -->
      <div class="text-center mb-4">
        <a
          href="newProject"
          class="inline-block px-4 py-2 text-sm font-medium text-white bg-green-500 rounded-lg hover:bg-green-600 focus:ring-4 focus:outline-none focus:ring-green-300"
        >
          Thêm dự án mới
        </a>
      </div>
      <!-- Kết thúc Nút Thêm dự án -->
      <div v-if="loading" class="text-center">
        <p>Loading...</p>
      </div>
      <div v-else>
        <div v-if="error" class="text-red-500 mb-4">
          <p>{{ error }}</p>
        </div>
        <div v-else>
          <div class="grid gap-6 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3">
            <div
              v-for="project in projects"
              :key="project.id"
              class="bg-white border border-gray-200 rounded-lg shadow-lg hover:shadow-xl transform hover:-translate-y-1 transition-all duration-300"
            >
              <a :href="'/project/' + project.name">
                <img
                  v-if="project.image"
                  class="rounded-t-lg w-full h-48 object-cover"
                  :src="project.image"
                  alt=""
                />
                <img
                  v-else
                  class="rounded-t-lg w-full h-48 object-cover"
                  src="../../public/Images/doctor.jpg"
                  alt="Placeholder"
                />
              </a>
              <div class="p-6">
                <a :href="'/project/' + project.name">
                  <h5 class="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
                    {{ project.name }}
                  </h5>
                </a>
                <p class="mb-4 text-gray-700 dark:text-gray-400">{{ project.description }}</p>
                <a
                  :href="'/project/' + project.name"
                  class="inline-flex items-center px-3 py-2 text-sm font-medium text-center text-white bg-blue-600 rounded-lg hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:bg-blue-500 dark:hover:bg-blue-600 dark:focus:ring-blue-700"
                >
                  Chi tiết
                  <svg
                    class="rtl:rotate-180 w-3.5 h-3.5 ml-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 14 10"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M1 5h12m0 0L9 1m4 4L9 9"
                    />
                  </svg>
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="flex justify-center mt-8">
      <button
        v-if="pageNumber > 1"
        @click="previousPage"
        class="bg-gray-200 hover:bg-gray-300 text-gray-600 font-bold py-2 px-4 rounded-l focus:outline-none focus:shadow-outline"
      >
        Trang trước
      </button>
      <template v-if="totalPages > 1">
        <template v-for="n in totalPages" :key="n">
          <button
            @click="goToPage(n)"
            :class="[
              'mx-1',
              pageNumber === n
                ? 'bg-blue-500 text-white'
                : 'bg-gray-200 hover:bg-gray-300 text-gray-600'
            ]"
            class="py-2 px-4 rounded focus:outline-none focus:shadow-outline"
          >
            {{ n }}
          </button>
        </template>
      </template>
      <button
        v-if="pageNumber < totalPages"
        @click="nextPage"
        class="bg-gray-200 hover:bg-gray-300 text-gray-600 font-bold py-2 px-4 rounded-r focus:outline-none focus:shadow-outline"
      >
        Trang sau
      </button>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  data() {
    return {
      projects: [],
      error: null,
      loading: false,
      pageNumber: 1,
      pageSize: 6,
      totalProjects: 0
    }
  },
  computed: {
    totalPages() {
      return Math.ceil(this.totalProjects / this.pageSize)
    }
  },
  mounted() {
    this.fetchProjects()
  },
  methods: {
    async fetchProjects() {
      this.loading = true
      try {
        const response = await axios.get(
          `https://localhost:7188/api/Project?pageNumber=${this.pageNumber}`
        )
        this.projects = response.data.projects.map((project) => {
          return {
            ...project,
            image: project.images.length > 0 ? project.images[0] : null // Gán ảnh đầu tiên hoặc null nếu không có ảnh
          }
        })
        this.totalProjects = response.headers['x-total-count']
        console.log(this.projects)
      } catch (error) {
        this.error = 'Không thể tải dữ liệu dự án.'
        console.error('Lỗi khi tải dữ liệu dự án:', error)
      } finally {
        this.loading = false
      }
    },
    async previousPage() {
      if (this.pageNumber > 1) {
        this.pageNumber--
        await this.fetchProjects()
      }
    },
    async nextPage() {
      if (this.pageNumber < this.totalPages) {
        this.pageNumber++
        await this.fetchProjects()
      }
    },
    async goToPage(page) {
      if (page !== this.pageNumber) {
        this.pageNumber = page
        await this.fetchProjects()
      }
    }
  }
}
</script>

<style>
/* Không cần thêm CSS vì chúng ta đã sử dụng các lớp từ Tailwind CSS */
</style>
