<template>
  <div class="container mx-auto mt-8">
    <div class="max-w-3xl mx-auto bg-white shadow-md p-8 rounded-lg">
      <ul
        class="flex flex-wrap text-sm font-medium text-center text-gray-500 border-b border-gray-200 dark:border-gray-700 dark:text-gray-400"
      >
        <li class="me-2">
          <a
            href="#"
            :class="{
              'inline-block p-4 text-blue-600 bg-gray-100 rounded-t-lg active dark:bg-gray-800 dark:text-blue-500':
                currentPage === 'profile',
              'inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300':
                currentPage !== 'profile'
            }"
            @click="changePage('profile')"
            >Detail</a
          >
        </li>
        <li class="me-2">
          <a
            href="#"
            :class="{
              'inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300':
                currentPage !== 'Member',
              'inline-block p-4 text-blue-600 bg-gray-100 rounded-t-lg active dark:bg-gray-800 dark:text-blue-500':
                currentPage === 'Member'
            }"
            @click="changePage('Member')"
            >Member</a
          >
        </li>
        <li class="me-2">
          <a
            href="#"
            :class="{
              'inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300':
                currentPage !== 'Sponsor',
              'inline-block p-4 text-blue-600 bg-gray-100 rounded-t-lg active dark:bg-gray-800 dark:text-blue-500':
                currentPage === 'Sponsor'
            }"
            @click="changePage('Sponsor')"
            >Sponsor</a
          >
        </li>
        <li class="me-2">
          <a
            href="#"
            :class="{
              'inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300':
                currentPage !== 'contacts',
              'inline-block p-4 text-blue-600 bg-gray-100 rounded-t-lg active dark:bg-gray-800 dark:text-blue-500':
                currentPage === 'contacts'
            }"
            @click="changePage('contacts')"
            >Contacts</a
          >
        </li>
        <li>
          <a
            class="inline-block p-4 text-gray-400 rounded-t-lg cursor-not-allowed dark:text-gray-500"
            >Disabled</a
          >
        </li>
      </ul>

      <div v-if="currentPage === 'profile'">
        <!-- Nội dung trang Profile -->
        <project />
        <!-- Thêm các phần tử và mã HTML cho trang Profile -->
      </div>
      <div v-else-if="currentPage === 'Member'">
        <!-- Nội dung trang Dashboard -->
        <Member />
        <!-- Thêm các phần tử và mã HTML cho trang Dashboard -->
      </div>
      <div v-else-if="currentPage === 'Sponsor'">
        <!-- Nội dung trang Dashboard -->
        <sponsor />
        <!-- Thêm các phần tử và mã HTML cho trang Dashboard -->
      </div>
      <!-- Tương tự cho các trang còn lại -->
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import Member from '../components/DetailProjectCompo/MemberInProject.vue'
import project from '../components/DetailProjectCompo/detail.vue'
import sponsor from '../components/DetailProjectCompo/sponsorInProject.vue'

export default {
  data() {
    return {
      currentPage: 'profile' // Trang hiện tại được chọn mặc định là 'profile'
    }
  },
  methods: {
    changePage(pageName) {
      this.currentPage = pageName
    }
  },
  components: {
    project,
    Member,
    sponsor // Đăng ký ChildComponent trong phần components
  }
}
</script>

<style>
/* Không cần thêm CSS vì chúng ta đã sử dụng các lớp từ Tailwind CSS */
</style>
