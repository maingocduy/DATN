<template>
  <div v-if="blog.account" class="container mx-auto p-4">
    <header class="bg-gray-100 p-6 mb-6 shadow-md rounded-lg">
      <h1 class="text-4xl font-bold text-gray-800 mb-2">{{ blog.title }}</h1>
      <div v-if="UsernameCookie == blog.account.username">
        <el-tooltip effect="light" content="Sửa Blog" placement="top-start">
          <el-button @click="editBlog" type="warning" :icon="Edit" size="small" circle></el-button>
        </el-tooltip>
        <el-tooltip effect="light" content="Xóa Blog" placement="top-start">
          <el-button
            @click="deleteBlog"
            type="danger"
            :icon="Delete"
            size="small"
            circle
          ></el-button>
        </el-tooltip>
      </div>
      <div class="flex justify-between text-gray-600">
        <p><strong>Người đăng:</strong> {{ blog.account.username }}</p>
        <p><strong>Ngày giờ đăng:</strong> {{ formatDate(blog.createdAt) }}</p>
      </div>
    </header>
    <main class="bg-white p-6 shadow-lg rounded-lg">
      <div id="htmlContainer" className="htmlContainer" v-html="this.blog.content" class=""></div>
    </main>
  </div>
</template>

<script>
import axios from 'axios'
import { View, Edit, Hide, Delete, Document } from '@element-plus/icons-vue'
import { ElMessageBox, ElNotification } from 'element-plus'
import Cookies from 'js-cookie'
export default {
  data() {
    return {
      blog: {},
      Delete: Delete,
      Edit: Edit,
      UsernameCookie: Cookies.get('username')
    }
  },
  created() {
    this.fetchBlog() // Gọi fetchBlog khi component được tạo
  },
  methods: {
    editBlog() {
      this.$router.push('/Blog/Update/' + this.$route.params.id)
    },
    showErrorNotification(messenger) {
      ElNotification({
        title: 'Lỗi',
        message: messenger,
        type: 'error'
      })
    },
    showSuccessNotification(messenger) {
      ElNotification({
        title: 'Thành công',
        message: messenger,
        type: 'success'
      })
    },
    deleteBlog() {
      ElMessageBox.confirm('Bạn có chắc muốn xóa bài viết này không ?', 'Xác nhận', {
        confirmButtonText: 'Chấp nhận',
        cancelButtonText: 'Hủy',
        type: 'warning'
      })
        .then(async () => {
          try {
            const response = await axios.delete('api/blog/delete_blog', {
              params: {
                title: this.blog.title
              }
            })
            if (response.status === 200) {
              this.showSuccessNotification(response.data.message)
              this.$router.push('/blog') // Refresh blogs list after deletion
            } else {
              this.showErrorNotification('Xóa bài viết thất bại.')
            }
          } catch (error) {
            if (error.response && error.response.status === 404) {
              this.showErrorNotification(error.response.data)
            } else {
              this.showErrorNotification('Đã xảy ra lỗi. Vui lòng thử lại sau.')
            }
          }
        })
        .catch(() => {})
    },
    async fetchBlog() {
      const id = this.$route.params.id
      try {
        const response = await axios.get(`/api/Blog/get_blog_by_id?id=${id}`)
        this.blog = response.data
        this.updateHtmlContent() // Sau khi nhận dữ liệu từ backend, gọi updateHtmlContent
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
    },
    updateHtmlContent() {
      // Cập nhật nội dung HTML từ dữ liệu blog
      const htmlContainer = document.getElementById('htmlContainer')
      if (htmlContainer) {
        htmlContainer.innerHTML = this.blog.content // Giả sử 'content' là nội dung HTML được lấy từ backend
      }
    }
  }
}
</script>

<style scoped>
/* Add custom loader styles or other necessary styles */
#htmlContainer {
  all: revert-layer !important;
}
</style>
