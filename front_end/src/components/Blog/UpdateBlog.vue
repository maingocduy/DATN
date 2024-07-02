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
      <label for="content" class="block text-gray-700 font-bold mb-2">Nội dung:</label>
      <TinyMCEEditor v-model="editorContent" />
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
import { ElNotification } from 'element-plus'
import TinyMCEEditor from '../../Helper/tinymce.vue'

export default {
  components: {
    TinyMCEEditor
  },
  data() {
    return {
      title: '',
      editorContent: ''
    }
  },
  async created() {
    await this.fetchBlog()
  },
  methods: {
    async fetchBlog() {
      try {
        const response = await axios.get('api/Blog/get_blog_by_id', {
          params: {
            id: this.$route.params.id
          }
        })
        this.title = response.data.title
        this.editorContent = response.data.content
      } catch (error) {
        console.log(error)
      }
    },
    async handleSubmit() {
      if (!this.title || !this.editorContent) {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Bạn nhập thiếu trường bắt buộc'
        })
        return
      }
      try {
        var myContent = tinymce.get('editor').getContent()
        const response = await axios.post('api/Blog/update_blog', {
          blog_id: this.$route.params.id,
          title: this.title,
          content: myContent
        })
        ElNotification({
          type: 'success',
          title: 'Thông báo',
          message: response.data.message
        })
        this.$router.push('/BlogDetail/' + this.$route.params.id)
      } catch (error) {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: error.response.data.message
        })
      }
    }
  }
}
</script>

<style scoped>
/* Nếu bạn muốn thêm CSS tùy chỉnh bên cạnh Tailwind, hãy thêm ở đây */
</style>
