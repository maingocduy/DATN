<template>
  <div class="container mx-auto mt-8">
    <!-- Top Section -->
    <div class="flex flex-wrap justify-between mb-8">
      <!-- Image Slider -->
      <div class="w-full lg:w-1/2 mb-4 lg:mb-0">
        <el-carousel :interval="3000" arrow="always" height="300px" autoplay>
          <el-carousel-item v-for="(image, index) in projectImages" :key="index">
            <img
              v-if="!project.images || project.images.length === 0"
              class="w-full h-full object-cover rounded-lg"
              src="../../../public/Images/doctor.jpg"
              alt="Placeholder"
            />
            <img
              v-else
              :src="image.image_url"
              alt="Project Image"
              class="w-full h-full object-cover rounded-lg"
            />
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
          <el-button type="primary" round @click="showJoinPopup">Tham gia</el-button>
          <el-button type="success" round @click="handleDonate">Đóng góp</el-button>
          <el-button type="danger" round @click="handleDelete">Xóa dự án</el-button>
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
          <div class="items-center mb-4">
            <el-select
              v-model="selectedGroup"
              placeholder="Chọn nhóm"
              @change="fetchMember"
              style="width: 200px; margin-left: 10px"
            >
              <el-option label="Tất cả" :value="null"></el-option>
              <el-option
                v-for="group in groups"
                :key="group.id"
                :label="group.group_name"
                :value="group.group_name"
              ></el-option>
            </el-select>
          </div>
          <el-table :data="indexedMembers" style="width: 100%">
            <el-table-column prop="index" label="STT" width="60"></el-table-column>
            <el-table-column prop="name" label="Tên" width="180"></el-table-column>
            <el-table-column prop="email" label="Email"></el-table-column>
            <el-table-column prop="phone" label="Số điện thoại"></el-table-column>
            <el-table-column prop="groups.group_name" label="Vai trò"></el-table-column>
          </el-table>
          <div class="flex justify-center mt-4">
            <el-pagination
              @current-change="handleMembersCurrentChange"
              :current-page="currentPageMembers"
              :page-size="membersPerPage"
              layout="prev, pager, next"
              :total="totalMembersPages"
            ></el-pagination>
          </div>
        </div>
      </el-tab-pane>
      <el-tab-pane label="Người đóng góp" name="sponsors">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <el-table :data="indexedSponsors" style="width: 100%">
            <el-table-column prop="index" label="STT" width="60"></el-table-column>
            <el-table-column prop="name" label="Tên" width="180"></el-table-column>
            <el-table-column prop="contact" label="Email"></el-table-column>
            <el-table-column prop="contributionAmount" label="Số tiền đóng góp">
              <template #default="{ row }">
                <span>{{ formatCurrencyToVND(row.contributionAmount) }}</span>
              </template>
            </el-table-column>
            <el-table-column prop="address" label="Địa chỉ"></el-table-column>
          </el-table>
          <div class="flex justify-center mt-4">
            <el-pagination
              @current-change="handleSponsorsCurrentChange"
              :current-page="currentPageSponsors"
              :pager-count="5"
              :page-size="sponsorsPerPage"
              layout="prev, pager, next"
              :total="totalSponsorsPages"
            ></el-pagination>
          </div>
        </div>
      </el-tab-pane>
    </el-tabs>

    <!-- Join Project Popup -->
    <el-dialog title="Tham gia dự án" v-model="joinPopupVisible" width="600px">
      <el-form :model="joinForm" label-width="120px">
        <el-form-item label="Tên">
          <el-input v-model="joinForm.name"></el-input>
        </el-form-item>
        <el-form-item label="Email">
          <el-input v-model="joinForm.email"></el-input>
        </el-form-item>
        <el-form-item label="Số điện thoại">
          <el-input v-model="joinForm.phone"></el-input>
        </el-form-item>
        <el-form-item label="Nhóm">
          <el-select v-model="joinForm.group" clearable placeholder="Chọn nhóm">
            <el-option label="Tất cả" :value="null"></el-option>
            <el-option
              v-for="group in groups"
              :key="group.id"
              :label="group.group_name"
              :value="group.group_name"
            ></el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="joinPopupVisible = false">Hủy</el-button>
        <el-button type="primary" @click="submitJoin">Gửi</el-button>
      </div>
    </el-dialog>

    <!-- OTP Popup -->
    <el-dialog title="Nhập mã OTP" v-model="otpPopupVisible" width="400px">
      <el-form :model="otpForm" label-width="120px">
        <el-form-item label="Mã OTP">
          <el-input v-model="otpForm.otp"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="otpPopupVisible = false">Hủy</el-button>
        <el-button type="primary" @click="submitOtp">Gửi</el-button>
      </div>
    </el-dialog>
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
  ElTableColumn,
  ElDialog,
  ElForm,
  ElFormItem,
  ElInput,
  ElSelect,
  ElOption,
  ElNotification,
  ElMessageBox,
  ElPagination
} from 'element-plus'
import Cookies from 'js-cookie'

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
    ElTableColumn,
    ElDialog,
    ElForm,
    ElFormItem,
    ElInput,
    ElSelect,
    ElOption,
    ElPagination
  },
  data() {
    return {
      project: {},
      members: [],
      sponsors: [],
      groups: [],
      activeTab: 'details',
      joinPopupVisible: false,
      otpPopupVisible: false,
      joinForm: {
        name: '',
        email: '',
        phone: '',
        group: ''
      },
      otpForm: {
        otp: ''
      },
      membersPerPage: 6,
      currentPageMembers: 1,
      totalMembersPages: 0,
      selectedGroup: '',
      sponsorsPerPage: 6,
      currentPageSponsors: 1,
      totalSponsorsPages: 0
    }
  },
  computed: {
    projectImages() {
      if (!this.project.images || this.project.images.length === 0) {
        return [{ image_url: '../../../public/Images/doctor.jpg' }] // Provide a default image
      } else {
        return this.project.images
      }
    },
    progress() {
      return this.project.budget ? (this.project.contributions / this.project.budget) * 100 : 0
    },
    indexedMembers() {
      if (!this.members) return []
      return this.members.map((member, index) => ({
        ...member,
        index: (this.currentPageMembers - 1) * this.membersPerPage + index + 1
      }))
    },
    indexedSponsors() {
      if (!this.sponsors) return [] // Handle undefined or null sponsors gracefully
      return this.sponsors.map((sponsor, index) => ({
        ...sponsor,
        index: (this.currentPageSponsors - 1) * this.sponsorsPerPage + index + 1
      }))
    }
  },
  mounted() {
    this.initialize()
  },
  methods: {
    async initialize() {
      await this.fetchProjectDetails()
      await this.fetchGroups()
      await this.fetchMember()
      await this.fetchSponsor()
    },
    async fetchProjectDetails() {
      try {
        const projectName = this.$route.params.name
        const projectResponse = await axios.post(`api/Project/get_project`, {
          projectName: projectName
        })
        this.project = projectResponse.data
        console.log(this.project)
      } catch (error) {
        console.error('Error fetching project details:', error)
      }
    },
    async fetchMember() {
      try {
        const memberResponse = await axios.post(
          `https://localhost:7188/api/Member/get_all_member`,
          {
            projectId: this.project.project_id,
            groupName: this.selectedGroup,
            pageNumber: this.currentPageMembers
          }
        )

        this.members = memberResponse.data.mems
        console.log(this.members)
        this.totalSponsorsPages = memberResponse.data.totalPages
      } catch (error) {
        console.error('Error fetching members:', error)
      }
    },
    handleMembersCurrentChange(val) {
      this.currentPageMembers = val
      this.fetchMember()
    },
    async fetchSponsor() {
      try {
        const sponsorResponse = await axios.post(
          `https://localhost:7188/api/Sponsor/get_all_sponsor`,
          {
            projectId: this.project.project_id,
            pageNumber: this.currentPageSponsors
          }
        )
        this.sponsors = sponsorResponse.data.spons
        console.log(this.sponsors)
        this.totalSponsorsPages = sponsorResponse.data.totalPages
      } catch (error) {
        console.error('Error fetching sponsors:', error)
      }
    },
    handleSponsorsCurrentChange(val) {
      this.currentPageSponsors = val
      this.fetchSponsor()
    },
    async fetchGroups() {
      try {
        const response = await axios.get(`api/Group`)
        this.groups = response.data
      } catch (error) {
        console.error('Error fetching groups:', error)
      }
    },
    async showJoinPopup() {
      if (Cookies.get('username') && Cookies.get('token') && Cookies.get('role')) {
        try {
          const response = await axios.post('https://localhost:7188/api/Member/JoinProject', {
            projectName: this.project.name,
            username: Cookies.get('username')
          })
          ElNotification({
            title: 'Thành công',
            message: response.data.messenger,
            type: 'success'
          })
        } catch (error) {
          ElNotification({
            title: 'Lỗi',
            message: error?.response.data.messenger,
            type: 'error'
          })
        }
      } else {
        this.joinPopupVisible = true
      }
    },
    async submitJoin() {
      try {
        const response = await axios.post(`https://localhost:7188/api/Member/create`, {
          nameProject: this.project.name,
          ...this.joinForm
        })
        if (response.status === 200) {
          this.joinPopupVisible = false
          this.otpPopupVisible = true
          ElNotification({
            title: 'Thành công',
            message: response.data.message,
            type: 'success'
          })
        }
      } catch (error) {
        console.error('Error submitting join:', error)
      }
    },
    async submitOtp() {
      try {
        const response = await axios.post(`https://localhost:7188/api/Member/enter_otp`, {
          Otp: this.otpForm.otp,
          ProjectName: this.project.name,
          Email: this.joinForm.email
        })
        if (response.status === 200) {
          this.otpPopupVisible = false
          ElNotification({
            title: 'Thành công',
            message: response.data.message,
            type: 'success'
          })
        }
      } catch (error) {
        console.error('Error submitting OTP:', error)
      }
    },
    handleDonate() {
      this.$router.push('/Momo/' + this.$route.params.name)
    },
    async handleDelete() {
      ElMessageBox.confirm('Bạn có chắc muốn xóa dự án này không ?', 'Xác nhận', {
        confirmButtonText: 'Chấp nhận',
        cancelButtonText: 'Hủy',
        type: 'warning'
      })
        .then(async () => {
          try {
            const response = await axios.delete(
              `https://localhost:7188/api/Project/${this.project.name}`
            )
            if (response.status === 200) {
              this.showSuccessNotification(response.data.message)
              this.$router.push('/Project')
            } else {
              this.showErrorNotification('Xóa dự án thất bại.')
            }
          } catch (error) {
            if (error.response && error.response.status === 404) {
              this.showErrorNotification(error.response.data)
            } else {
              this.showErrorNotification('Đã xảy ra lỗi. Vui lòng thử lại sau.')
            }
          }
        })
        .catch(() => {
          this.showErrorNotification('Xóa dự án thất bại!')
        })
    },
    formatCurrencyToVND(amount) {
      return amount
        ? Math.round(amount)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' ₫'
        : '0 ₫'
    },
    showErrorNotification(message) {
      ElNotification({
        title: 'Lỗi',
        message: message,
        type: 'error'
      })
    },
    showSuccessNotification(message) {
      ElNotification({
        title: 'Thành công',
        message: message,
        type: 'success'
      })
    }
  }
}
</script>

<style scoped>
.container {
  width: 100%;
}

.el-dialog__wrapper {
  backdrop-filter: blur(5px);
}

.el-dialog {
  border-radius: 10px;
}

.el-dialog__header {
  background-color: #f5f5f5;
  border-bottom: 1px solid #ebebeb;
}

.el-dialog__title {
  font-weight: bold;
}

.dialog-footer {
  text-align: right;
}

.el-button--primary {
  background-color: #409eff;
  border-color: #409eff;
}

.el-button--primary:hover {
  background-color: #66b1ff;
  border-color: #66b1ff;
}

.el-button--danger {
  background-color: #f56c6c;
  border-color: #f56c6c;
}

.el-button--danger:hover {
  background-color: #ff7878;
  border-color: #ff7878;
}
</style>
