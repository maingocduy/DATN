<template>
  <div class="max-w-4xl mx-auto p-5">
    <form @submit.prevent="handleSubmit">
      <!-- Thêm phần nhập title -->
      <div class="mb-4">
        <label for="title" class="block text-gray-700 font-bold mb-2">Tiêu Đề:</label>
        <input
          type="text"
          id="title"
          v-model="title"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          placeholder="Enter title"
        />
      </div>
      <!-- Kết thúc phần nhập title -->
      <label for="title" class="block text-gray-700 font-bold mb-2">Nội dung:</label>
      <TinyMCEEditor />
      <button
        type="submit"
        class="mt-4 px-4 py-2 bg-blue-500 text-white font-bold rounded hover:bg-blue-700"
      >
        Đăng
      </button>
    </form>
  </div>
</template>

<script>
import axios from 'axios'
import TinyMCEEditor from '../../Helper/tinymce.vue'
import { ElNotification } from 'element-plus'
export default {
  data() {
    return {
      editor: null, // Lưu trữ instance của TinyMCE
      postData: '', // Lưu trữ dữ liệu từ editor
      title: '' // Lưu trữ tiêu đề nhập vào
    }
  },
  mounted() {},
  methods: {
    async handleSubmit() {
      var myContent = tinymce.get('editor').getContent()
      // console.log('Title:', this.title) // Log tiêu đề
      // console.log('Form submitted:', myContent)
      if (!this.title || !myContent) {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Bạn nhập thiếu trường bắt buộc'
        })
        return
      }
      await axios
        .post('https://localhost:7188/api/Blog', {
          title: this.title,
          content: myContent
        })
        .then(
          (response) => {
            console.log(response)
            ElNotification({
              type: 'success',
              title: 'Thông báo',
              message: response.data.message
            })
            this.$router.push('/blog')
          },
          (error) => {
            ElNotification({
              type: 'error',
              title: 'Thông báo',
              message: error.response.data.message
            })
          }
        )
      // Tại đây bạn có thể thực hiện gửi dữ liệu đến server hoặc xử lý khác
    }
  },
  beforeDestroy() {
    if (this.editor) {
      this.editor.remove()
    }
  },
  components: {
    TinyMCEEditor
  }
}
</script>

<style scoped>
/* Nếu bạn muốn thêm CSS tùy chỉnh bên cạnh Tailwind, hãy thêm ở đây */
</style>
