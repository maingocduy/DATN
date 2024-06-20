<template>
  <div class="container mx-auto mt-8">
    <!-- Top Section -->
    <div class="flex flex-wrap justify-between mb-8">
      <!-- Image Slider -->
      <div class="w-full lg:w-1/2 mb-4 lg:mb-0">
        <el-carousel :interval="3000" arrow="always" height="300px" autoplay>
          <el-carousel-item v-for="(image, index) in projectImages" :key="index">
            <img :src="image" alt="Project Image" class="w-full h-full object-cover rounded-lg" />
          </el-carousel-item>
        </el-carousel>
      </div>
      <!-- Project Info -->
      <div class="w-full lg:w-1/2 lg:pl-8">
        <h2 class="text-3xl font-bold mb-4">{{ project.name }}</h2>
        <div class="mb-4">
          <el-progress :percentage="progress" status="success"></el-progress>
        </div>
        <div class="mb-4 text-lg">
          <p>
            <strong>Số tiền được đóng góp:</strong> {{ formatCurrencyToVND(project.contributions) }}
          </p>
          <p><strong>Số tiền đích:</strong> {{ formatCurrencyToVND(project.budget) }}</p>
        </div>
        <div class="flex space-x-4">
          <el-button type="primary" round>Tham gia</el-button>
          <el-button type="success" round>Đóng góp</el-button>
        </div>
      </div>
    </div>

    <!-- Bottom Section -->
    <el-tabs v-model="activeTab">
      <el-tab-pane label="Chi tiết" name="details">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="text-2xl font-bold mb-4">Chi tiết dự án</h3>
          <div v-html="project.description"></div>
        </div>
      </el-tab-pane>
      <el-tab-pane label="Thành viên tham gia" name="members">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="text-2xl font-bold mb-4">Thành viên tham gia</h3>
          <el-table :data="members" style="width: 100%">
            <el-table-column prop="name" label="Tên" width="180"></el-table-column>
            <el-table-column prop="email" label="Email"></el-table-column>
            <el-table-column prop="phone" label="Số điện thoại"></el-table-column>
            <el-table-column prop="groups.group_name" label="Vai trò"></el-table-column>
          </el-table>
        </div>
      </el-tab-pane>
      <el-tab-pane label="Người đóng góp" name="sponsors">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="text-2xl font-bold mb-4">Người đóng góp</h3>
          <el-table :data="sponsors" style="width: 100%">
            <el-table-column prop="name" label="Tên" width="180"></el-table-column>
            <el-table-column prop="contact" label="Email"></el-table-column>
            <el-table-column prop="contributionAmount" label="Số tiền đóng góp"
              ><template #default="{ row }">
                <span>{{ formatCurrencyToVND(row.contributionAmount) }}</span>
              </template></el-table-column
            >
            <el-table-column prop="address" label="Địa chỉ"></el-table-column>
          </el-table>
        </div>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import axios from 'axios'
import {
  ElCarousel,
  ElCarouselItem,
  ElProgress,
  ElButton,
  ElTabs,
  ElTabPane,
  ElTable,
  ElTableColumn
} from 'element-plus'
import 'element-plus/dist/index.css'

export default {
  name: 'DetailProject',
  components: {
    ElCarousel,
    ElCarouselItem,
    ElProgress,
    ElButton,
    ElTabs,
    ElTabPane,
    ElTable,
    ElTableColumn
  },
  data() {
    return {
      project: [],
      image: [],
      members: [],
      sponsors: [],
      activeTab: 'details'
    }
  },
  computed: {
    projectImages() {
      return this.project.images ? this.project.images : ['../../public/Images/placeholder.jpg']
    },
    progress() {
      return this.project.budget ? (this.project.contributions / this.project.budget) * 100 : 0
    }
  },
  mounted() {
    this.fetchProjectDetails()
  },
  methods: {
    async fetchProjectDetails() {
      try {
        const projectName = this.$route.params.name
        const projectResponse = await axios.post(`https://localhost:7188/api/Project/get_project`, {
          projectName: projectName
        })
        this.project = projectResponse.data
        this.sponsors = this.project.sponsor
        this.members = this.project.member
        this.image = this.project.images

        console.log(this.project)
        console.log(this.sponsors)
        console.log(this.members)
        console.log(this.image)
        // const membersResponse = await axios.get(
        //   `https://localhost:7188/api/Project/${projectId}/members`
        // )
        // this.members = membersResponse.data

        // const sponsorsResponse = await axios.get(
        //   `https://localhost:7188/api/Project/${projectId}/sponsors`
        // )
        // this.sponsors = sponsorsResponse.data
      } catch (error) {
        console.error('Error fetching project details:', error)
      }
    },
    progress() {
      return this.project.budget ? (this.project.contributions / this.project.budget) * 100 : 0
    },
    formatCurrencyToVND(amount) {
      return amount
        ? Math.round(amount)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, ',') + ' ₫'
        : '0 ₫'
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 1200px;
}
</style>
