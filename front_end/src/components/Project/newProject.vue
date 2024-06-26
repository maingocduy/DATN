<template>
  <div class="container mx-auto mt-8">
    <div v-if="!projectCreated" class="max-w-3xl mx-auto">
      <h1 class="text-3xl font-bold mb-8 text-center">Tạo dự án mới</h1>
      <div class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-8">
        <!-- Tên dự án -->
        <div class="mb-4">
          <label for="projectName" class="block text-gray-700 text-sm font-bold mb-2">
            Tên dự án
          </label>
          <el-input v-model="projectName" placeholder="Nhập tên dự án" clearable class="w-full" />
        </div>
        <!-- Quỹ -->
        <div class="mb-4">
          <label for="budget" class="block text-gray-700 text-sm font-bold mb-2">Quỹ</label>
          <div class="flex items-center">
            <el-input-number
              v-model="budget"
              placeholder="Nhập số tiền"
              :controls="false"
              :precision="0"
              :min="10000"
              @change=""
              :parser="parseBudget"
              @blur="checkBudget"
              class="w-full"
            />
            <span class="ml-2">VND</span>
          </div>
        </div>
        <!-- Ngày bắt đầu và Ngày kết thúc -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
          <div>
            <label for="startDate" class="block text-gray-700 text-sm font-bold mb-2">
              Ngày bắt đầu
            </label>
            <el-date-picker
              v-model="startDate"
              type="date"
              placeholder="Chọn ngày"
              class="w-full"
              format="DD-MM-YYYY"
              value-format="YYYY-MM-DD"
              @change="checkStartDate"
            />
          </div>
          <div>
            <label for="endDate" class="block text-gray-700 text-sm font-bold mb-2">
              Ngày kết thúc
            </label>
            <el-date-picker
              v-model="endDate"
              type="date"
              placeholder="Chọn ngày"
              format="DD-MM-YYYY"
              value-format="YYYY-MM-DD"
              class="w-full"
              @change="checkEndDate"
            />
          </div>
        </div>
        <!-- Drag to Upload -->
        <div class="mb-4">
          <label for="upload" class="block text-gray-700 text-sm font-bold mb-2">
            Thêm ảnh cho dự án
          </label>
          <el-upload
            class="upload-demo"
            action="https://localhost:7188/api/Cloud/uploadsingle"
            drag
            :multiple="true"
            :on-success="handleUploadSuccess"
            :limit="10"
            :file-list="fileList"
            :on-remove="handleRemove"
          >
            <i class="el-icon-upload"></i>
            <div class="el-upload__text">Kéo tệp vào đây hoặc <em>click để chọn</em></div>
          </el-upload>
        </div>
        <!-- Nút tạo dự án -->
        <div class="flex justify-end">
          <el-button @click="handleCreateProject" type="primary" round size="medium">
            Tạo dự án
          </el-button>
        </div>
      </div>
    </div>

    <!-- Editor cho phần mô tả -->
    <div v-if="projectCreated" class="max-w-3xl mx-auto">
      <h2 class="text-3xl font-bold mb-8 text-center">Mô tả dự án</h2>
      <div class="flex justify-end mt-4">
        <el-button
          @click="saveProject"
          style="margin-top: 5px; margin-bottom: 5px"
          type="primary"
          round
          size="medium"
        >
          Lưu dự án
        </el-button>
      </div>
      <div class="mb-5">
        <TinyMCEEditor v-model="description" />
      </div>
    </div>
  </div>
</template>

<script>
import TinyMCEEditor from '../../Helper/tinymce.vue'
import { ElDatePicker, ElInputNumber, ElButton, ElNotification, ElUpload } from 'element-plus'
import axios from 'axios'

export default {
  components: {
    TinyMCEEditor,
    ElDatePicker,
    ElInputNumber,
    ElButton,
    ElUpload
  },
  data() {
    return {
      projectName: '',
      budget: null,
      description: '',
      startDate: '',
      endDate: '',
      fileList: [], // Danh sách các file đã chọn để tải lên
      projectCreated: false, // Biến để kiểm soát việc hiển thị phần mô tả,
      value1: new Date('2018-06-22')
    }
  },
  methods: {
    formatBudget(value) {
      if (!value) return ''
      // Định dạng số tiền thành VND
      const formattedValue = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(value)
      return formattedValue
    },
    handleCreateProject() {
      if (!this.projectName || !this.budget || !this.startDate || !this.endDate) {
        ElNotification({
          type: 'warning',
          title: 'Thông báo',
          message: 'Vui lòng nhập đầy đủ thông tin.'
        })
        return
      }
      console.log(this.budget)
      console.log(this.fileList)
      console.log(this.projectName)
      console.log(this.startDate)
      console.log(this.endDate)
      this.projectCreated = true
    },
    checkStartDate() {
      // Kiểm tra nếu ngày bắt đầu không hợp lệ (nhỏ hơn ngày hiện tại)
      if (new Date(this.startDate) <= this.getDateFourDays()) {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Ngày bắt đầu phải cách ít nhất 7 ngày so với ngày hôm nay'
        })
        this.startDate = ''
      }
    },
    getTodayWithoutTime() {
      const today = new Date()
      today.setHours(0, 0, 0, 0) // Đặt giờ, phút, giây, millisecond về 0
      return today
    },
    getDateFourDays() {
      const today = this.getTodayWithoutTime()
      today.setDate(today.getDate() + 7)
      return today
    },
    async saveProject() {
      // Kiểm tra xem đã nhập mô tả hay chưa
      var myContent = tinymce.get('editor').getContent()
      this.description = myContent
      if (!this.description) {
        ElNotification({
          type: 'warning',
          title: 'Thông báo',
          message: 'Vui lòng nhập mô tả dự án.'
        })
        return
      }
      try {
        var response = await axios.post('api/Project/add_project', {
          name: this.projectName,
          budget: this.budget,
          description: this.description,
          startDate: this.startDate,
          endDate: this.endDate
        })
        if (this.fileList.length > 0) {
          for (const e of this.fileList) {
            try {
              await axios.post('api/Cloud/UpImageProject', {
                projectName: this.projectName,
                publicID: e.response.result.publicId,
                url: e.response.result.secureUrl
              })
            } catch (error) {
              console.error('Error uploading image:', error)
              // Xử lý lỗi nếu cần thiết
            }
          }
        }
        ElNotification({
          type: 'success',
          title: 'Thông báo',
          message: response.data.message
        })
        this.$router.push('/project')
      } catch (error) {
        ElNotification({
          type: 'warning',
          title: 'Thông báo',
          message: 'Lỗi khi thêm dự án mới! Vui lòng thử lại sau'
        })
      }
    },
    checkBudget() {
      // Kiểm tra nếu số tiền nhỏ hơn 10000
      if (this.budget < 10000 || this.budget == null) {
        ElNotification({
          type: 'warning',
          title: 'Thông báo',
          message: 'Số tiền quỹ phải lớn hơn 10000'
        })
        this.budget = null
      }
    },
    simulateSaveProject(projectData) {
      // Trong ví dụ này, bạn có thể mô phỏng gọi API để lưu dự án và ảnh
      console.log('Đang lưu dự án với các thông tin sau:')
      console.log(projectData)

      // Sau khi lưu thành công, reset form và hiển thị thông báo thành công
      this.resetForm()

      ElNotification({
        type: 'success',
        title: 'Thông báo',
        message: 'Đã lưu dự án thành công.'
      })
    },
    checkEndDate() {
      const startDate = new Date(this.startDate)
      const endDate = new Date(this.endDate)
      if (startDate && endDate && endDate <= startDate) {
        ElNotification({
          type: 'error',
          title: 'Thông báo',
          message: 'Ngày kết thúc phải sau ngày bắt đầu'
        })
        this.endDate = ''
      }
    },
    resetForm() {
      this.projectName = ''
      this.budget = null
      this.description = ''
      this.startDate = ''
      this.endDate = ''
      this.fileList = [] // Reset danh sách ảnh đã chọn
      this.projectCreated = false // Reset trạng thái hiển thị
    },
    handleUploadSuccess(response, file, fileList) {
      // Xử lý khi tệp được tải lên thành công
      console.log('Tệp đã tải lên:', file)
      // Thực hiện xử lý lưu thông tin tệp tải lên vào dữ liệu của bạn
      this.fileList = fileList // Cập nhật danh sách các file đã tải lên
    },
    async handleRemove(file, fileList) {
      // Hành động xóa file trước khi lưu dự án
      console.log('Đang xóa tệp:', file.response.result.publicId)
      const response = await axios.post(
        'https://localhost:7188/api/cloud/delete',
        file.response.result.publicId,
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      )
      this.fileList = fileList
      for (const e of this.fileList) {
        console.log(e.response.result.publicId)
        console.log(e.response.result.secureUrl)
      }
      // Cập nhật lại danh sách file sau khi xóa
    }
  }
}
</script>

<style scoped>
.upload-demo .el-upload {
  width: 100%;
}
</style>
