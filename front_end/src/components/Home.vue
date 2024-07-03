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
          :interval="5000"
          :arrow="projects.length > 3 ? 'always' : 'never'"
          @change="resetInterval"
          autoplay
          class="project-carousel"
          style="width: 90%; height: 100%"
        >
          <el-carousel-item
            v-for="(chunk, chunkIndex) in chunkedProjects"
            :key="chunkIndex"
            style="height: auto"
          >
            <div class="flex-container">
              <div v-for="(project, index) in chunk" :key="index" class="flex-item">
                <el-card shadow="hover" class="project-card">
                  <a
                    :href="'/project/' + project.name"
                    style="
                      display: flex;
                      flex-direction: column;
                      align-items: center;
                      justify-content: center;
                    "
                  >
                    <img
                      v-if="!!project?.images"
                      class="rounded-t-lg w-40 h-20"
                      :src="project.images.image_url"
                      alt=""
                    />
                    <img
                      v-else
                      class="rounded-t-lg w-40 h-20"
                      src="../../../public/Images/doctor.jpg"
                      alt="Placeholder"
                    />
                  </a>
                  <div class="p-6">
                    <a :href="'/project/' + project.name">
                      <h5
                        class="mb-2 text-2xl font-bold tracking-tight text-gray-900"
                        style="
                          display: -webkit-box;
                          -webkit-line-clamp: 1;
                          -webkit-box-orient: vertical;
                          overflow: hidden;
                          text-overflow: ellipsis;
                          word-break: break-word;
                        "
                      >
                        {{ project.name }}
                      </h5>
                    </a>

                    <el-progress
                      :percentage="calculatePercentage(project.contributions, project.budget)"
                      :stroke-width="10"
                      style="width: 100%; display: block"
                    >
                    </el-progress>

                    <div class="text-sm mt-2 text-gray-600">
                      <p>Đã đóng góp: {{ formatCurrencyToVND(project.contributions) }}</p>
                      <p>Mục tiêu: {{ formatCurrencyToVND(project.budget) }}</p>
                    </div>
                  </div>
                </el-card>
              </div>
            </div>
          </el-carousel-item>
        </el-carousel>

        <el-button @click="goToProject" class="view-all-button" round> Xem tất cả dự án </el-button>
      </div>
    </div>

    <!-- Thống kê -->
    <div class="statistics">
      <h2 class="text-5xl">Những con số biết nói</h2>
      <div class="stat-container mt-8 flex flex-wrap justify-around">
        <div class="stat p-4" v-for="(value, key) in stats" :key="key">
          <p class="text-lg">{{ key }}</p>
          <h3 class="text-4xl font-bold">{{ value }}</h3>
        </div>
      </div>
    </div>

    <!-- Giới thiệu -->
    <div class="introduction">
      <div class="intro-text">
        <div style="margin-bottom: 10px">
          <h2 style="line-height: 1">Website Hội Bác Sĩ Tình Nguyện Là Gì?</h2>
        </div>
        <p>
          Trang web của chúng tôi là nơi kết nối những trái tim nhân ái với sứ mệnh chăm sóc sức
          khỏe cộng đồng. Tại đây, bạn có thể quyên góp cho các dự án y tế từ thiện, đồng thời khám
          phá kho tàng kiến thức y tế qua các bài viết blog do các bác sĩ và chuyên gia y tế chia
          sẻ.
        </p>
        <p>
          Hội Bác Sĩ Tình Nguyện không chỉ kêu gọi quyên góp tài chính, mà còn cho phép bạn đăng ký
          tham gia trực tiếp vào các dự án khám bệnh từ thiện. Chúng tôi luôn chào đón mọi sự đóng
          góp và tham gia của bạn, để cùng nhau xây dựng một cộng đồng khỏe mạnh và tràn đầy tình
          thương.
        </p>
        <p>
          Hãy cùng chúng tôi lan tỏa những giá trị nhân văn và mang lại nụ cười cho những mảnh đời
          khó khăn. Tham gia ngay hôm nay và trở thành một phần của sự thay đổi tích cực!
        </p>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { ElCarousel, ElCarouselItem, ElCard, ElButton, ElProgress } from 'element-plus'
import 'element-plus/dist/index.css'

export default {
  name: 'HomePage',
  components: {
    ElCarousel,
    ElCarouselItem,
    ElCard,
    ElButton,
    ElProgress
  },
  data() {
    return {
      products: [
        { image: '../../public/Images/anhbia5.jpg' },
        { image: '../../public/Images/anhslide.jpg' },
        { image: '../../public/Images/anhbia4.jpg' }
      ],
      projects: [],
      overview: [],
      stats: {}
    }
  },
  computed: {
    chunkedProjects() {
      const chunkSize = 3
      const chunks = []
      let i = 0

      while (i < this.projects.length) {
        if (i + chunkSize >= this.projects.length) {
          // Trường hợp cuối danh sách
          chunks.push(this.projects.slice(-chunkSize))
          break
        } else {
          // Các chunk bình thường
          chunks.push(this.projects.slice(i, i + chunkSize))
          i += chunkSize // Di chuyển i lên chunkSize để không có phần tử trùng lặp
        }
      }

      return chunks
    }
  },
  mounted() {
    this.viewAllProjects()
    this.Overview()
  },
  methods: {
    calculatePercentage(contributions, budget) {
      if (!budget) return 0
      return Math.min(((contributions / budget) * 100).toFixed(2), 100)
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
    async viewAllProjects() {
      try {
        const response = await axios.get('api/Project/get_all_project_in_date')
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
  padding: 0.5rem 0.5rem;
  background-color: #f4f4f9;
  font-family: 'Arial', sans-serif;
}
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&family=Roboto+Mono:wght@400;700&family=Merriweather:wght@400;700&display=swap');

.statistics h2 {
  font-family: 'Roboto', Sans-serif;
}

.statistics p {
  font-family: 'Roboto', sans-serif;
}

.statistics h3 {
  font-family: 'Roboto', Sans-serif;
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
  margin-top: 0.5rem;
  background-color: #fff;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  background-position: top left;
  background-repeat: no-repeat;
  background-size: cover;
  background-image: url('../../public/Images/groupProject.png');
}

.project-card {
  text-align: center;
  background-color: #fff;
  width: 100%;
  height: 100%;
}

.project-card img {
  height: 200px;
  width: 100%;
}

.el-card__body {
  padding: unset;
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
  height: 450px;
}

.project-carousel {
  margin-bottom: 20px;
  height: 100%;
}

.flex-container {
  display: flex;
  justify-content: center; /* Center the items */
  gap: 20px;
  width: 100%;
  flex-wrap: wrap;
}

.flex-item {
  flex: 1 1 calc(25% - 20px); /* Adjusted for smaller items */
  max-width: calc(25% - 20px); /* Adjusted for smaller items */
  box-sizing: border-box;
  display: flex; /* Added to center the card content */
  flex-direction: column; /* Added to center the card content */
  align-items: center; /* Added to center the card content */
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
  margin-top: 0.5rem;
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
  color: #fff;
}

.introduction {
  display: flex;
  align-items: center;
  text-align: center;
  background-color: #fff;
  margin-top: 0.5rem;
  border-radius: 8px;
  animation: slideUp 1s ease-out;
  gap: 20px;
  padding: 50px 0px 30px 0px;
  justify-content: center;
  background-image: url('../../public/Images/groupProject.png');
}

.intro-text {
  flex: 1;
  text-align: left;
  font-weight: 400;
  max-width: 600px; /* Giới hạn chiều rộng của phần giới thiệu */
  text-align: justify;
}
.intro-text h2 {
  color: #2e3c4b;
  font-family: 'Roboto', sans-serif;
  font-size: 40px;
  font-style: normal;
  font-weight: 700;
  text-align: center;
}
.intro-text p {
  margin-bottom: 28px;
  text-align: justify;
  color: #7a7a7a;
  font-family: 'Roboto', Sans-serif;
  font-size: 16px;
  font-weight: 400;
  line-height: 24px;
  margin-bottom: 10px;
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

/* Responsive Design */
@media (max-width: 1024px) {
  .flex-item {
    flex: 1 1 calc(33.333% - 20px); /* 3 items per row */
    max-width: calc(33.333% - 20px); /* 3 items per row */
  }
}

@media (max-width: 768px) {
  .flex-item {
    flex: 1 1 calc(50% - 20px); /* 2 items per row */
    max-width: calc(50% - 20px); /* 2 items per row */
  }
}

@media (max-width: 480px) {
  .flex-item {
    flex: 1 1 100%; /* 1 item per row */
    max-width: 100%; /* 1 item per row */
  }

  .project-card img {
    height: 150px; /* Adjust image height for smaller screens */
  }

  .project-title {
    font-size: 1.2rem; /* Adjust font size for smaller screens */
  }

  .view-all-button {
    padding: 0.5rem 1rem; /* Adjust button padding for smaller screens */
  }

  .statistics {
    padding: 30px 0; /* Adjust padding for smaller screens */
    height: auto; /* Adjust height for smaller screens */
  }
}
</style>
