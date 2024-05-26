<template>
  <div class="container mx-auto mt-8">
    <div class="max-w-3xl mx-auto">
      <h1 class="text-3xl font-bold mb-4 text-center">Tạo dự án mới</h1>
      <div class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <!-- Tên dự án -->
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="projectName">
            Tên dự án
          </label>
          <input
            v-model="projectName"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="projectName"
            type="text"
            placeholder="Tên dự án"
          />
        </div>
        <!-- Quỹ -->
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="budget"> Quỹ </label>
          <input
            v-model.number="budget"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="budget"
            type="number"
            placeholder="Quỹ"
          />
        </div>
        <!-- Ngày bắt đầu -->
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="startDate">
            Ngày bắt đầu
          </label>
          <input
            v-model="startDate"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="startDate"
            type="date"
          />
        </div>
        <!-- Ngày kết thúc -->
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="endDate">
            Ngày kết thúc
          </label>
          <input
            v-model="endDate"
            @input="checkEndDate"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            id="endDate"
            type="date"
          />
        </div>
        <!-- Mô tả -->
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="description">
            Mô tả
          </label>
          <TinyMCEEditor />
        </div>
        <!-- Nút tạo dự án -->
        <div class="flex items-center justify-between">
          <button
            @click="createProject"
            class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            type="button"
          >
            Tạo dự án
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import TinyMCEEditor from '../Helper/tinymce.vue'
export default {
  data() {
    return {
      projectName: '',
      budget: 0,
      description: '',
      startDate: '',
      endDate: ''
    }
  },
  methods: {
    createProject() {
      // Xử lý logic để tạo dự án mới, có thể gửi dữ liệu lên server tại đây
      console.log('Tên dự án:', this.projectName)
      console.log('Quỹ:', this.budget)
      console.log('Mô tả:', this.description)
      console.log('Ngày bắt đầu:', this.startDate)
      console.log('Ngày kết thúc:', this.endDate)
    },
    checkEndDate() {
      // Kiểm tra hợp lệ mỗi khi người dùng thay đổi ngày bắt đầu
      if (this.startDate && this.endDate <= this.startDate) {
        alert('Ngày kết thúc phải sau ngày bắt đầu')
        this.endDate = ''
      }
    }
  },
  components: {
    TinyMCEEditor
  }
}
</script>

<style scoped>
/* Thêm CSS tùy chỉnh nếu cần */
</style>
