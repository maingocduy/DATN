<template>
  <div class="container mx-auto">
    <h1 class="text-3xl font-bold mb-4">Tạo Blog</h1>
    <form @submit.prevent="createBlog">
      <div class="mb-4">
        <label for="title" class="block text-sm font-medium text-gray-700">Tiêu đề:</label>
        <input
          type="text"
          id="title"
          v-model="title"
          class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md"
        />
      </div>
      <div class="mb-4">
        <label for="editor" class="block text-sm font-medium text-gray-700">Nội dung:</label>
        <ckeditor
          :editor="editor"
          v-model="content"
          :config="editorConfig"
          id="editor"
          class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md"
        />
      </div>
      <button
        type="submit"
        class="inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
      >
        Tạo Blog
      </button>
    </form>
  </div>
</template>

<script>
import { ref } from 'vue'
import { Editor, EditorContent } from '@ckeditor/ckeditor5-vue'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import axios from 'axios'

export default {
  components: {
    ckeditor: Editor,
    EditorContent
  },
  setup() {
    const title = ref('')
    const content = ref('')

    const createBlog = async () => {
      try {
        const response = await axios.post('YOUR_BACKEND_ENDPOINT/create-blog', {
          title: title.value,
          content: content.value
        })
        console.log(response.data)
        // Xử lý kết quả nếu cần
      } catch (error) {
        console.error(error)
      }
    }

    const editor = ref(ClassicEditor)
    const editorConfig = ref({
      // Cấu hình cho CKEditor
    })

    return { title, content, createBlog, editor, editorConfig }
  }
}
</script>

<style>
/* Tailwind CSS styles */
</style>
