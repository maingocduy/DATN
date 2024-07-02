<template>
  <div class="container mx-auto mt-8">
    <div class="max-w-7xl mx-auto">
      <h1 class="text-4xl font-bold text-center mt-3 mb-8">Danh sách dự án</h1>

      <!-- Nút Thêm dự án -->
      <div v-if="roleManager" class="text-center mb-8">
        <el-button type="success" @click="gotoCreateProject()" round> Thêm dự án mới </el-button>
      </div>
      <!-- Kết thúc Nút Thêm dự án -->

      <!-- Tabs -->
      <div class="mb-8 text-center">
        <el-button-group>
          <el-button
            :type="currentTab === 'funding' ? 'primary' : 'default'"
            @click="setTab('funding')"
            round
          >
            Các dự án đang gây quỹ
          </el-button>
          <el-button
            :type="currentTab === 'expired' ? 'primary' : 'default'"
            @click="setTab('expired')"
            round
          >
            Các dự án đã kết thúc
          </el-button>
        </el-button-group>
      </div>

      <!-- Loading Spinner -->
      <div v-if="loading" class="text-center">
        <el-loading :fullscreen="true" text="Loading..."></el-loading>
      </div>

      <!-- Error Message -->
      <div v-else-if="error" class="text-center mb-8">
        <el-alert :title="error" type="error" :closable="false" show-icon></el-alert>
      </div>

      <!-- Projects Grid -->
      <div v-else>
        <div class="grid gap-8 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3">
          <el-card
            v-for="project in projects"
            :key="project.id"
            shadow="hover"
            class="project-card"
          >
            <a :href="'/project/' + project.name">
              <img
                v-if="!!project?.images"
                class="rounded-t-lg w-full h-48 object-cover"
                :src="project.images.image_url"
                alt=""
              />
              <img
                v-else
                class="rounded-t-lg w-full h-48 object-cover"
                src="../../../public/Images/doctor.jpg"
                alt="Placeholder"
              />
            </a>
            <div class="p-6">
              <a :href="'/project/' + project.name">
                <h5 class="mb-2 text-2xl font-bold tracking-tight text-gray-900">
                  {{ project.name }}
                </h5>
              </a>
              <el-progress
                :percentage="calculatePercentage(project.contributions, project.budget)"
                status="success"
                :text-inside="true"
                :stroke-width="20"
                class="my-4"
              >
                <template #text>
                  <span>{{ calculatePercentage(project.contributions, project.budget) }}%</span>
                </template>
              </el-progress>
              <div class="text-sm text-gray-600">
                <p>Đã đóng góp: {{ formatCurrencyToVND(project.contributions) }}</p>
                <p>Mục tiêu: {{ formatCurrencyToVND(project.budget) }}</p>
              </div>
              <a
                :href="'/project/' + project.name"
                class="inline-flex items-center px-3 py-2 text-sm font-medium text-white bg-blue-600 rounded-lg hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 transition duration-300 mt-4"
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
          </el-card>
        </div>

        <!-- No Projects Message -->
        <div v-if="projects.length === 0 && !loading && !error" class="flex flex-col items-center">
          <img src="../../../public/Images/Empty.jpg" alt="Hình ảnh rỗng" class="max-w-xs mb-4" />
          <p class="italic">Không có dự án nào.</p>
        </div>
      </div>

      <!-- Pagination -->
      <div class="flex justify-center mt-12 mb-5">
        <el-pagination
          background
          layout="prev, pager, next"
          :current-page="pageNumber"
          :page-size="pageSize"
          :page-count="totalProjects"
          @current-change="handlePageChange"
        >
        </el-pagination>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import Cookies from 'js-cookie'
import {
  ElLoading,
  ElAlert,
  ElCard,
  ElButton,
  ElButtonGroup,
  ElPagination,
  ElProgress
} from 'element-plus'

export default {
  data() {
    return {
      projects: [],
      error: null,
      loading: false,
      pageNumber: 1,
      pageSize: 6,
      totalProjects: 0,
      roleManager: false,
      currentTab: 'funding'
    }
  },
  computed: {
    totalPages() {
      return Math.ceil(this.totalProjects / this.pageSize)
    }
  },
  mounted() {
    this.fetchProjects()
    if (Cookies.get('role') === 'Manager') {
      this.roleManager = true
    }
  },
  methods: {
    gotoCreateProject() {
      this.$router.push('/newProject')
    },
    setTab(tab) {
      this.currentTab = tab
      this.pageNumber = 1 // Reset lại trang khi chuyển tab
      this.fetchProjects()
    },
    async fetchImages(projectId) {
      try {
        const response = await axios.get('api/Project/get_image', {
          params: {
            project_id: projectId
          }
        })
        return response.data // Trả về danh sách ảnh từ API
      } catch (error) {
        console.error('Lỗi khi tải ảnh dự án:', error)
        return [] // Trả về mảng rỗng nếu có lỗi
      }
    },
    async fetchProjects() {
      this.loading = true
      this.error = null
      let apiUrl
      if (this.currentTab === 'expired') {
        apiUrl = `https://localhost:7188/api/Project/get_all_project_out_date?pageNumber=${this.pageNumber}`
      } else {
        apiUrl = `https://localhost:7188/api/Project/get_all_project_in_date?pageNumber=${this.pageNumber}`
      }
      try {
        const response = await axios.get(apiUrl)
        this.projects = await Promise.all(
          response.data.projects.map(async (project) => {
            const images = await this.fetchImages(project.project_id)
            return {
              ...project,
              images: images.length > 0 ? images[0] : null
            }
          })
        )
        console.log(this.projects)
        this.totalProjects = response.data.totalPages
      } catch (error) {
        this.error = 'Không thể tải dữ liệu dự án.'
        console.error('Lỗi khi tải dữ liệu dự án:', error)
      } finally {
        this.loading = false
      }
    },
    calculatePercentage(contributions, budget) {
      if (!budget) return 0
      return Math.min(((contributions / budget) * 100).toFixed(2), 100)
    },
    handlePageChange(page) {
      this.pageNumber = page
      this.fetchProjects()
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
    },
    formatCurrencyToVND(amount) {
      return amount
        ? Math.round(amount)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' VND'
        : '0 VND'
    }
  }
}
</script>

<style scoped>
.project-card {
  transition:
    transform 0.3s ease,
    box-shadow 0.3s ease;
}

.project-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 15px rgba(0, 0, 0, 0.1);
}

.project-card img {
  transition: transform 0.3s ease;
}

.project-card img:hover {
  transform: scale(1.05);
}
</style>
