<template>
  <div class="blog-list p-4 mb-10 flex flex-col items-center">
    <div class="w-full max-w-7xl">
      <h2 class="font-bold text-2xl mb-4 text-center">Danh Sách Blog</h2>
      <button
        v-if="blogs.length !== 0"
        class="mb-6 bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600 transition duration-200"
        @click="navigateToAddNew"
      >
        Thêm mới
      </button>
      <div v-if="blogs.length === 0" class="flex flex-col items-center">
        <img src="../../../public/Images/Empty.jpg" alt="Hình ảnh rỗng" class="max-w-xs mb-4" />
        <p class="italic">Không có bài viết nào.</p>
      </div>
      <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6">
        <div v-for="blog in blogs" :key="blog.id" class="bg-white shadow-lg rounded-lg p-4">
          <template v-if="extractFirstImage(blog.content)">
            <img
              :src="extractFirstImage(blog.content)"
              class="w-full h-48 object-cover rounded-t-lg mb-4"
              alt="Hình ảnh của blog"
            />
          </template>
          <h3 class="text-xl font-bold mb-2">{{ blog.title }}</h3>
          <p class="text-gray-600"><strong>Tác giả:</strong> {{ blog.account.username }}</p>
          <p class="text-gray-600">
            <strong>Thời gian tạo:</strong> {{ formatDate(blog.createdAt) }}
          </p>
          <button class="mt-4 text-blue-500 hover:underline" @click="showBlogDetail(blog.blog_id)">
            Xem chi tiết
          </button>
          <el-dialog
            title="Nội dung chi tiết"
            :visible.sync="dialogVisible"
            width="60%"
            :before-close="handleDialogClose"
          >
            <div v-html="currentBlog.content"></div>
            <span slot="footer" class="dialog-footer">
              <el-button @click="dialogVisible = false">Đóng</el-button>
            </span>
          </el-dialog>
        </div>
      </div>
      <div v-if="blogs.length !== 0" class="mt-8 flex justify-center">
        <el-pagination
          @current-change="handlePageChange"
          :current-page="pageNumber"
          :page-size="pageSize"
          layout="prev, pager, next"
          :page-count="totalBlogs"
        />
      </div>
      <button
        v-if="blogs.length === 0"
        class="mt-6 bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600 transition duration-200"
        @click="navigateToAddNew"
      >
        Thêm mới
      </button>
    </div>
  </div>
</template>

<script>
import Cookies from 'js-cookie'
import axios from 'axios'
import { ElNotification } from 'element-plus'

export default {
  name: 'BlogList',
  data() {
    return {
      blogs: [],
      pageNumber: 1,
      pageSize: 6,
      totalBlogs: 0,
      dialogVisible: false,
      currentBlog: {}
    }
  },
  methods: {
    async fetchBlogs() {
      try {
        const response = await axios.post('/api/blog/all_blog_approve', null, {
          params: {
            pageNumber: this.pageNumber
          }
        })
        this.blogs = response.data.blogs
        this.totalBlogs = response.data.totalPages
      } catch (error) {
        console.error('Failed to fetch blogs:', error)
      }
    },
    handlePageChange(newPage) {
      this.pageNumber = newPage
      this.fetchBlogs()
    },
    formatDate(dateString) {
      const options = { year: 'numeric', month: 'numeric', day: 'numeric' }
      const formattedDate = new Date(dateString).toLocaleDateString('vi-VN', options)
      return formattedDate
    },
    showBlogDetail(blog) {
      this.$router.push('/BlogDetail/' + blog)
    },
    handleDialogClose(done) {
      if (done) {
        this.currentBlog = {}
        this.dialogVisible = false
      }
    },
    navigateToAddNew() {
      if (Cookies.get('token')) {
        this.$router.push('/addBlog')
      } else {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Bạn cần phải đăng nhập thì mới dùng được chức năng này'
        })
      }
    },
    extractFirstImage(content) {
      const imgTag = content.match(/<img[^>]+src="([^">]+)"/)
      return imgTag ? imgTag[1] : '../../../public/Images/blog-def.jpg'
    }
  },
  created() {
    this.fetchBlogs()
  }
}
</script>

<style scoped>
.blog-list {
  padding: 20px;
}

.max-w-xs {
  max-width: 20rem;
}

.mt-6 {
  margin-top: 1.5rem;
}

.mb-6 {
  margin-bottom: 1.5rem;
}

.mb-4 {
  margin-bottom: 1rem;
}
</style>
