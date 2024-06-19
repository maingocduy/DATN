<template>
  <div class="blog-list">
    <el-row type="flex" justify="center" class="blog-list-content">
      <el-col :span="24">
        <h2 class="font-bold text-xl">Danh Sách Blog</h2>
        <!-- Nút Thêm mới -->
        <el-button
          v-if="blogs.length !== 0"
          class="add-new-button"
          type="primary"
          size="medium"
          @click="navigateToAddNew"
          >Thêm mới</el-button
        >
        <el-row :gutter="20">
          <template v-if="blogs.length === 0">
            <div class="no-blogs">
              <div class="no-blogs-content">
                <img src="../../../public/Images/Empty.jpg" alt="Hình ảnh rỗng" />
                <p>Không có bài viết nào.</p>
              </div>
            </div>
          </template>
          <template v-else>
            <el-col :xs="24" :sm="12" :md="8" v-for="blog in blogs" :key="blog.id">
              <el-card shadow="hover" class="blog-card">
                <template v-if="blog.image">
                  <img :src="blog.image" class="blog-image" alt="Hình ảnh của blog" />
                </template>
                <h3>{{ blog.title }}</h3>
                <p><strong>Tác giả:</strong> {{ blog.account.username }}</p>
                <p><strong>Thời gian tạo:</strong> {{ formatDate(blog.createdAt) }}</p>
                <el-button type="text" @click="showBlogDetail(blog)">Xem chi tiết</el-button>
                <el-dialog
                  title="Nội dung chi tiết"
                  :visible.sync="dialogVisible"
                  width="60%"
                  :before-close="handleDialogClose"
                >
                  <div v-html="blog.content"></div>
                  <span slot="footer" class="dialog-footer">
                    <el-button @click="dialogVisible = false">Đóng</el-button>
                  </span>
                </el-dialog>
              </el-card>
            </el-col>
          </template>
        </el-row>
        <div v-if="blogs.length !== 0" class="pagination">
          <el-pagination
            @current-change="handlePageChange"
            :current-page="pageNumber"
            :page-size="pageSize"
            layout="prev, pager, next"
            :total="totalBlogs"
          />
        </div>
        <el-button
          v-if="blogs.length === 0"
          class="add-new-button"
          type="primary"
          size="medium"
          @click="navigateToAddNew"
          >Thêm mới</el-button
        >
      </el-col>
    </el-row>
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
        this.totalBlogs = response.data.totalPages * this.pageSize
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
      this.currentBlog = blog
      this.dialogVisible = true
    },
    handleDialogClose(done) {
      if (done) {
        this.currentBlog = {}
        this.dialogVisible = false
      }
    },
    navigateToAddNew() {
      // Thực hiện điều hướng đến trang thêm mới blog hoặc hiển thị dialog thêm mới tại đây
      if (Cookies.get('token')) {
        this.$router.push('/addBlog')
      } else {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Bạn cần phải đăng nhập thì mới dùng được chức năng này'
        })
      }
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
  min-height: 80vh; /* Điều chỉnh chiều cao tối thiểu cho trang */
  display: flex;
  justify-content: center;
  align-items: center;
}

.blog-list-content {
  text-align: center;
}

.blog-cards {
  margin-top: 20px;
}

.blog-card {
  margin-bottom: 20px;
  height: 100%; /* Đặt chiều cao cho thẻ blog */
}

.pagination {
  margin-top: 20px;
  display: flex;
  justify-content: center;
}

.no-blogs {
  text-align: center;
  width: 100%;
}

.no-blogs-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  max-width: 600px; /* Điều chỉnh chiều rộng tối đa của phần nội dung */
  padding: 20px;
}

.no-blogs-content img {
  max-width: 30%; /* Đảm bảo hình ảnh không vượt quá chiều rộng của phần nội dung */
  height: auto;
  margin-bottom: 10px;
}

.no-blogs-content p {
  margin: 0;
  font-style: italic;
}

.add-new-button {
  margin-bottom: 20px; /* Khoảng cách giữa nút Thêm mới và danh sách blog */
}
</style>
