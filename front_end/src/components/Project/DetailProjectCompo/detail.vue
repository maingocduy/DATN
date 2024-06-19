<template>
  <div v-if="loading" class="text-center">
    <p>Loading...</p>
  </div>
  <div v-else>
    <div v-if="error" class="text-red-500 mb-4">
      <p>{{ error }}</p>
    </div>
    <div v-else>
      <div class="mb-4 mt-3">
        <p class="text-lg font-semibold">Tên: {{ project.name }}</p>
        <p class="text-lg font-semibold">Quỹ: {{ project.budget }}</p>
        <p class="text-lg font-semibold">Mô tả:</p>
        <p class="text-gray-500 dark:text-gray-400 font-normal" v-html="formattedDescription"></p>
        <!-- Thêm lớp font-normal để sử dụng phông chữ dạng như yêu cầu -->
        <p class="text-lg font-semibold">Ngày bắt đầu: {{ project.startDate }}</p>
        <p class="text-lg font-semibold">Ngày kết thúc: {{ project.endDate }}</p>
      </div>
      <div class="flex items-center">
        <router-link
          :to="{ name: 'project' }"
          class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline mr-2"
        >
          Quay lại
        </router-link>
        <button
          @click="deleteProject"
          class="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline mr-2"
        >
          Xóa dự án
        </button>
        <!-- Thêm nút đóng góp -->
        <button
          @click="contribute"
          class="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          Đóng góp
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { fetchProjectData } from './fetchProjectDetail.js'
import axios from 'axios'
export default {
  data() {
    return {
      project: {},
      error: null,
      loading: false
    }
  },
  computed: {
    formattedDescription() {
      return this.project.description ? this.project.description.replace(/\n/g, '<br>') : ''
    }
  },
  mounted() {
    this.fetchProject()
  },
  methods: {
    async fetchProject() {
      const projectName = this.$route.params.name
      this.project = await fetchProjectData(projectName)
      console.log(this.project)
    },
    async deleteProject() {
      const projectName = this.$route.params.name
      const confirmation = confirm('Bạn có chắc chắn muốn xóa dự án này?')
      if (confirmation) {
        try {
          await axios.delete(`api/Project/${projectName}`)
          // Chuyển hướng người dùng sau khi xóa dự án thành công
          this.$router.push({ name: 'project' })
        } catch (error) {
          this.error = 'Không thể xóa dự án.'
          console.error('Lỗi khi xóa dự án:', error)
        }
      }
    },
    contribute() {
      // Thực hiện các thao tác để đóng góp vào dự án
      // Ví dụ: chuyển hướng đến trang đóng góp hoặc hiển thị biểu mẫu đóng góp
      this.$router.push({ name: 'Momo', params: { name: this.project.name } })
    }
  }
}
</script>

<style>
/* Không cần thêm CSS vì chúng ta đã sử dụng các lớp từ Tailwind CSS */
</style>
