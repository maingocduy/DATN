<template>
  <div id="home-page">
    <!-- Slide hình ảnh -->
    <el-carousel
      ref="imageCarousel"
      :interval="3000"
      arrow="never"
      @change="resetInterval"
      height="auto"
      autoplay
    >
      <el-carousel-item v-for="(product, index) in products" :key="index" style="height: auto">
        <img :src="product.image" :alt="product.name" class="carousel-image slide-animation" />
      </el-carousel-item>
    </el-carousel>

    <!-- Slide dự án -->
    <div class="projects-section py-8 px-4 md:px-0">
      <h2 class="text-4xl font-sans mb-4">Dự án đang triển khai</h2>
      <p class="text-base text-gray-700 mb-6">Hãy lựa chọn đồng hành cùng dự án mà bạn quan tâm</p>

      <div class="carousel-container">
        <el-carousel
          ref="projectCarousel"
          :interval="3000"
          arrow="always"
          @change="resetInterval"
          autoplay
          class="project-carousel"
          style="width: 500px; height: 900px"
        >
          <el-carousel-item style="height: 500px" v-for="(project, index) in projects" :key="index">
            <el-card shadow="hover" style="height: 500px" class="project-card">
              <a :href="'/project/' + project.name">
                <img
                  v-if="!project.images || project.images.length === 0"
                  class="rounded-t-lg w-36 h-30"
                  src="../../../public/Images/doctor.jpg"
                  alt="Placeholder"
                />
                <img
                  v-else
                  class="rounded-t-lg w-60 h-30"
                  :src="project.images[0].image_url"
                  alt=""
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
              </div>
            </el-card>
          </el-carousel-item>
        </el-carousel>

        <el-button @click="goToProject" class="view-all-button" round> Xem tất cả dự án </el-button>
      </div>
    </div>

    <!-- Thống kê -->
    <div class="statistics">
      <h2 class="text-5xl font-mono">Những con số biết nói</h2>
      <div class="stat-container">
        <div class="stat" v-for="(value, key) in stats" :key="key">
          <p class="text-lg font-mono">{{ key }}</p>
          <h3 class="text-4xl font-bold font-mono">{{ value }}</h3>
        </div>
      </div>
    </div>

    <!-- Giới thiệu -->
    <div class="introduction">
      <img
        src="../../public/Images/309043961_434949368736287_4246824704006557099_n.jpg"
        alt="Introduction Image"
        class="intro-image"
      />
      <div class="intro-text">
        <h2>Giới thiệu</h2>
        <p>Đây là đoạn giới thiệu về dự án...</p>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { ElCarousel, ElCarouselItem, ElCard, ElButton } from 'element-plus'
import 'element-plus/dist/index.css'

export default {
  name: 'HomePage',
  components: {
    ElCarousel,
    ElCarouselItem,
    ElCard,
    ElButton
  },
  data() {
    return {
      products: [
        {
          image: '../../public/Images/anhbia5.jpg'
        },
        {
          image: '../../public/Images/anhslide.jpg'
        },
        {
          image: '../../public/Images/anhbia4.jpg'
        }
      ],
      projects: [],
      overview: [],
      stats: {}
    }
  },
  mounted() {
    this.viewAllProjects(), this.Overview()
  },
  methods: {
    calculatePercentage(contributions, budget) {
      if (!budget) return 0
      return Math.min(((contributions / budget) * 100).toFixed(2), 100)
    },
    async viewAllProjects() {
      try {
        const response = await axios.get('https://localhost:7188/api/Project/get_all_project')
        this.projects = response.data.projects
        console.log(this.projects)
      } catch (error) {
        console.error('Error fetching projects:', error)
      }
    },
    async Overview() {
      try {
        const response = await axios.get('/api/Project/overview')
        this.overview = response.data
        this.stats = {
          'Dự án': this.overview.totalProject,
          'Lượt ủng hộ': this.overview.totalSponsor,
          'Tổng số tiền ủng hộ': this.formatCurrencyToVND(this.overview.totalContribution)
        }
      } catch (error) {
        console.error('Error fetching projects:', error)
      }
    },
    formatCurrencyToVND(amount) {
      return amount
        ? Math.round(amount)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' VND'
        : '0 VND'
    },
    goToProject() {
      this.$router.push('/project')
    }
  }
}
</script>

<style scoped>
#home-page {
  text-align: center;
  padding: 1rem 1rem;
  background-color: #f4f4f9;
  font-family: 'Arial', sans-serif;
}

.carousel-item {
  display: flex;
  align-items: center;
  justify-content: center;
}

.carousel-image {
  width: 100%;
  height: auto;
  object-fit: cover;
  border-radius: 8px;
}

.projects-section {
  margin-top: 1rem;
  background-color: #fff;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.project-card {
  padding: 1rem;
  text-align: center;
  background-color: #fff;
}

.project-title {
  font-size: 1.5rem;
  font-weight: bold;
  color: #333;
}
.carousel-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: auto;
  /* Adjust the height as needed */
}

.project-carousel {
  margin-bottom: 20px; /* Adjust the spacing between carousel and button as needed */
}

.view-all-button {
  margin-top: 20px;
  background-color: #007bff;
  color: #fff;
  border: none;
  padding: 0.75rem 1.5rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.view-all-button:hover {
  background-color: #0056b3;
}

.statistics {
  text-align: center;
  padding: 60px 0px 60px 0px;
  background: url('../../public/Images/homeStatic.jpg');
  background-position: center center;
  background-repeat: no-repeat;
  background-size: cover;
  color: #fff;
  margin-top: 1rem;
  height: 302px;
}

.stat-container {
  display: flex;
  justify-content: space-around;
  flex-wrap: wrap;
  margin-top: 1rem;
}

.stat {
  flex: 1;
  min-width: 150px;
  margin: 0.5rem;
  color: #fff; /* Ensure text color is white for readability */
}

.introduction {
  display: flex;
  align-items: center;
  padding: 2rem 0;
  background-color: #fff;
  margin-top: 2rem;
  border-radius: 8px;
  animation: slideUp 1s ease-out;
}

.intro-image {
  width: 200px;
  height: auto;
  margin-right: 2rem;
  border-radius: 8px;
}

.intro-text {
  flex: 1;
  text-align: left;
}

.descProject {
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
  word-break: break-word;
}

.slide-animation {
  animation: slideEffect 0.5s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideUp {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

@keyframes slideEffect {
  from {
    transform: translateX(100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}
</style>
