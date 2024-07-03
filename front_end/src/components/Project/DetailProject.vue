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
        <div class="w-auto">
          <h2
            class="text-3xl font-bold mb-4"
            style="width: 90%; word-wrap: break-word; white-space: normal"
          >
            {{ project.name }}
          </h2>
        </div>
        <div class="mb-4">
          <el-progress
            :percentage="progress"
            :stroke-width="6"
            :format="customFormat"
          ></el-progress>
        </div>
        <div class="mb-4 text-lg">
          <p>
            <strong>Số tiền được đóng góp:</strong> {{ formatCurrencyToVND(project.contributions) }}
          </p>
          <p><strong>Số tiền đích:</strong> {{ formatCurrencyToVND(project.budget) }}</p>
        </div>
        <div v-if="!isProjectExpired" class="project-actions">
          <!-- Kiểm tra nếu tiến độ dự án đạt 100% -->
          <div class="flex space-x-4">
            <div v-if="isRegisterToProjectExpired">
              <el-button type="primary" round @click="showJoinPopup">Tham gia</el-button>
            </div>
            <div v-if="progress < 100">
              <el-button type="success" round @click="handleDonate">Đóng góp</el-button>
              <el-button
                v-if="sponsors.length === 0 && role === 'Manager'"
                type="danger"
                round
                @click="handleDelete"
                >Xóa dự án</el-button
              >
            </div>
            <div v-else class="completion-message">🎉 Đã hoàn thành mục tiêu</div>
          </div>
        </div>
        <!-- Nếu dự án đã hết hạn -->
        <div v-else class="expired-message">Dự án đã kết thúc</div>
      </div>
    </div>

    <!-- Bottom Section -->
    <el-tabs v-model="activeTab">
      <el-tab-pane label="Chi tiết" name="details">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <h3 class="text-2xl font-bold mb-4">Chi tiết dự án</h3>
          <div id="htmlContainer" v-html="project.description"></div>
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
            <el-table-column label="STT" width="60">
              <template #default="{ $index }">
                {{ $index + 1 }}
              </template>
            </el-table-column>
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
              :page-count="totalMembersPages"
            ></el-pagination>
          </div>
        </div>
      </el-tab-pane>
      <el-tab-pane label="Người đóng góp" name="sponsors">
        <div class="p-4 bg-white rounded-lg shadow-md">
          <el-table :data="indexedSponsors" style="width: 100%">
            <el-table-column label="STT" width="60">
              <template #default="{ $index }">
                {{ $index + 1 }}
              </template>
            </el-table-column>
            <el-table-column prop="name" label="Tên" width="180"></el-table-column>
            <el-table-column prop="contact" label="Email"></el-table-column>
            <el-table-column prop="contributionAmount" label="Số tiền đóng góp">
              <template #default="{ row }">
                <span>{{ formatCurrencyToVND(row.contributionAmount) }}</span>
              </template>
            </el-table-column>
            <el-table-column prop="address" label="Địa chỉ">
              <template #default="{ row }">
                {{ row.address === '' ? '--' : row.address }}
              </template></el-table-column
            >
            <el-table-column prop="time_create" label="Thời gian">
              <template #default="{ row }">
                <span>{{ formatDate(row.time_create) }}</span>
              </template>
            </el-table-column>
          </el-table>
          <div class="flex justify-center mt-4">
            <el-pagination
              @current-change="handleSponsorsCurrentChange"
              :current-page="currentPageSponsors"
              :page-count="totalSponsorsPages"
              :page-size="sponsorsPerPage"
              layout="prev, pager, next"
              :disabled="false"
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
          <el-select v-model="joinForm.group_name" clearable placeholder="Chọn nhóm">
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
      <div class="card flex justify-center mb-2">
        <InputOtp v-model="otps" :length="6">
          <template #default="{ attrs, events }">
            <input type="text" v-bind="attrs" v-on="events" class="custom-otp-input rounded" />
          </template>
        </InputOtp>
      </div>
      <button @click="submitOtp" class="btn-submit w-full">Xác thực OTP</button>
      <p class="text-center mt-2">Gửi lại OTP sau: {{ countdownTimer }}</p>
      <div v-if="showResendButton">
        <a href="#" class="text-blue-600 text-center block mt-2" @click.prevent="resendOTP"
          >Gửi lại OTP</a
        >
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
        group_name: ''
      },
      otps: '',
      membersPerPage: 6,
      currentPageMembers: 1,
      totalMembersPages: 0,
      selectedGroup: '',
      sponsorsPerPage: 6,
      currentPageSponsors: 1,
      totalSponsorsPages: 0,
      role: Cookies.get('role'),
      countdownTimer: 60,
      showResendButton: false
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
    format() {
      if (this.progress < 100) {
        return `${this.progress.toFixed(2)}%` // Format percentage to two decimal places
      } else {
        return 'Đã đạt 100%' // Display when progress reaches 100%
      }
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
    },
    isProjectExpired() {
      const endDate = new Date(this.project.endDate)
      const currentDate = new Date()
      return endDate < currentDate
    },
    isRegisterToProjectExpired() {
      const threeDaysBeforeStartDate = new Date(this.project.startDate)
      threeDaysBeforeStartDate.setDate(threeDaysBeforeStartDate.getDate() - 3) // Lấy ngày cách 3 ngày trước ngày startDate

      const currentDate = new Date()

      return threeDaysBeforeStartDate >= currentDate
    }
  },
  mounted() {
    this.initialize(), this.startCountdown()
  },
  methods: {
    async resendOTP() {
      // Gửi lại yêu cầu gửi OTP tới email đã nhập
      try {
        var response = await axios.post('api/Member/re_send_otp', {
          email: this.joinForm.email
        })
        ElNotification({
          type: 'success',
          title: 'Thông báo',
          message: response.data.message
        })
        this.startCountdown()
      } catch (error) {
        // Bắt đầu đếm ngược lại
      }
    },
    startCountdown() {
      this.countdownTimer = 60
      this.showResendButton = false
      const countdownInterval = setInterval(() => {
        if (this.countdownTimer > 0) {
          this.countdownTimer--
        } else {
          clearInterval(countdownInterval)
          this.showResendButton = true
        }
      }, 1000)
    },
    customFormat(percentage) {
      return `${percentage.toFixed(0)}%`
    },
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
        console.log(memberResponse.data)
        this.totalMembersPages = memberResponse.data.totalPages
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
        console.log(sponsorResponse.data)
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
            project_id: this.project.project_id,
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
        if (!this.joinForm) {
          ElNotification({
            title: 'Lỗi',
            message: 'Nhập thiếu trường bắt buộc!',
            type: 'error'
          })
          return
        } else if (!this.isValidEmail(this.joinForm.email)) {
          ElNotification({
            title: 'Lỗi',
            message: 'Email không hợp lệ!',
            type: 'error'
          })
          return
        } else if (!this.isValidPhone(this.joinForm.phone)) {
          ElNotification({
            title: 'Lỗi',
            message: 'Số điện thoại không hợp lệ!',
            type: 'error'
          })
          return
        }
        const response = await axios.post(`https://localhost:7188/api/Member`, {
          project_id: this.project.project_id,
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
          Otp: this.otps,
          project_id: this.project.project_id,
          Email: this.joinForm.email
        })
        if (response.status === 200) {
          this.otpPopupVisible = false
          ElNotification({
            title: 'Thành công',
            message: response.data.messenger,
            type: 'success'
          })
        }
      } catch (error) {
        console.error('Error submitting OTP:', error)
        ElNotification({
          title: 'Lỗi',
          message: error?.response.data.messenger,
          type: 'error'
        })
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
    isValidEmail(email) {
      const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/
      return emailPattern.test(email)
    },

    isValidPhone(phone) {
      const phonePattern = /^[0-9]{10,11}$/
      return phonePattern.test(phone)
    },
    formatCurrencyToVND(amount) {
      return amount
        ? Math.round(amount)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ' VND'
        : '0 VND'
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
    },
    formatDate(dateString) {
      const options = {
        year: 'numeric',
        month: 'numeric',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric'
      }
      const formattedDate = new Date(dateString).toLocaleString('vi-VN', options)
      return formattedDate
    }
  }
}
</script>

<style scoped>
.container {
  width: 100%;
}

.el-input__wrapper input[type='text']:focus {
  box-shadow: unset !important;
}
.el-dialog__wrapper {
  backdrop-filter: blur(5px);
}
.custom-otp-input {
  width: 53px;
  font-size: 36px;
  text-align: center;
  transition: all 0.2s;
  background: transparent;
  padding: 10px;
  margin-right: 8px;
}
.completion-message {
  background-color: #e0f7fa;
  border: 1px solid #00bcd4;
  color: #006064;
  padding: 10px;
  border-radius: 4px;
  margin-top: 10px;
  font-weight: bold;
}

.expired-message {
  background-color: #fce4ec;
  border: 1px solid #f8bbd0;
  color: #c2185b;
  padding: 10px;
  border-radius: 4px;
  margin-top: 10px;
  font-weight: bold;
  width: 40%;
}
.btn-submit {
  background-color: #3b82f6;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition:
    background-color 0.3s,
    transform 0.3s;
  display: inline-block;
  width: 100%;
  text-align: center;
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
