<template>
  <div id="home-page">
    <!-- Slide hình ảnh -->
    <el-carousel
      ref="imageCarousel"
      :interval="3000"
      arrow="always"
      height="600px"
      @change="resetInterval"
      autoplay
    >
      <el-carousel-item v-for="(product, index) in products" :key="index">
        <img :src="product.image" :alt="product.name" class="carousel-image slide-animation" />
      </el-carousel-item>
    </el-carousel>

    <!-- Slide dự án -->
    <div class="projects-section">
      <h2>Các dự án đang chuẩn bị triển khai</h2>
      <el-carousel
        ref="projectCarousel"
        :interval="3000"
        arrow="always"
        type="card"
        @change="resetInterval"
        autoplay
        class="project-carousel"
      >
        <el-carousel-item v-for="(project, index) in projects" :key="index">
          <el-card class="project-card slide-animation">
            <template #header>
              <div class="project-title">{{ project.name }}</div>
            </template>
            <p>{{ project.description }}</p>
          </el-card>
        </el-carousel-item>
      </el-carousel>
      <el-button @click="viewAllProjects" class="view-all-button" round>
        Xem tất cả dự án
      </el-button>
    </div>

    <!-- Thống kê -->
    <div class="statistics">
      <div class="stat" v-for="(value, key) in stats" :key="key">
        <el-card class="stat-card">
          <h3>{{ value }}</h3>
          <p>{{ key }}</p>
        </el-card>
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
          image: '../../public/Images/anhbia.jpg'
        },
        {
          image: '../../public/Images/anhbia2.jpg'
        },
        {
          image: '../../public/Images/anhbia4.jpg'
        }
      ],
      projects: [],
      stats: {
        'Dự án': 10,
        'Lượt ủng hộ': 500,
        'Tổng số tiền ủng hộ': '1,000,000 VND'
      }
    }
  },
  mounted() {
    this.viewAllProjects()
  },
  methods: {
    async viewAllProjects() {
      try {
        const response = await axios.get(
          'https://localhost:7188/api/Project/get_all_project_aprove'
        )
        this.projects = response.data.projects
        console.log(this.projects)
      } catch (error) {
        console.error('Error fetching projects:', error)
      }
    }
  }
}
</script>

<style scoped>
#home-page {
  text-align: center;
}

.carousel-image {
  width: 100%;
  height: 100%;
  object-fit: cover; /* Đảm bảo hình ảnh vừa với khu vực carousel */
}

.projects-section {
  margin-top: 2rem;
}

.project-card {
  padding: 1rem;
  text-align: center;
  background-color: #fff;
}

.project-title {
  font-size: 1.25rem;
  font-weight: bold;
}

.view-all-button {
  margin-top: 1rem;
  transition: background-color 0.3s ease;
}

.statistics {
  display: flex;
  justify-content: space-around;
  padding: 2rem 0;
  text-align: center;
  background-color: #f8f9fa;
  animation: fadeIn 2s;
}

.stat-card {
  padding: 1rem;
}

.introduction {
  display: flex;
  align-items: center;
  padding: 2rem 0;
  animation: slideUp 1s ease-out;
}

.intro-image {
  width: 200px;
  height: auto;
  margin-right: 2rem;
}

.intro-text {
  flex: 1;
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
