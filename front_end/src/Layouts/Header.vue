<template>
  <nav class="bg-white shadow-lg">
    <div class="container mx-auto px-4 py-3 flex justify-between items-center">
      <a href="#" class="flex items-center space-x-3">
        <img src="../../public/Images/logo.jpg" class="h-20" alt="Flowbite Logo" />
      </a>
      <div class="hidden sm:flex items-center space-x-6">
        <ul class="flex flex-row font-medium space-x-8">
          <li><a href="/" class="text-gray-800 hover:text-blue-500">Trang chủ</a></li>
          <li><a href="/blog" class="text-gray-800 hover:text-blue-500">Blog</a></li>
          <li><a href="#" class="text-gray-800 hover:text-blue-500">Danh sách bác sĩ</a></li>
          <li><a href="/project" class="text-gray-800 hover:text-blue-500">Dự án</a></li>
        </ul>
      </div>
      <ul class="flex flex-row font-medium space-x-8 sm:hidden">
        <li><a href="/" class="text-gray-800 hover:text-blue-500">Trang chủ</a></li>
        <li><a href="/blog" class="text-gray-800 hover:text-blue-500">Blog</a></li>
        <li><a href="#" class="text-gray-800 hover:text-blue-500">Danh sách bác sĩ</a></li>
        <li><a href="/project" class="text-gray-800 hover:text-blue-500">Dự án</a></li>
      </ul>
      <div class="flex items-center space-x-6">
        <template v-if="isAuthenticated">
          <span class="text-sm text-gray-800">Xin chào, {{ username }} </span>
        </template>
        <template v-else>
          <a href="/register" class="text-sm text-gray-800 hover:text-blue-500">Đăng ký</a>
          <a href="/login" class="text-sm text-gray-800 hover:text-blue-500">Đăng nhập</a>
        </template>
      </div>
      <div class="sm:hidden">
        <button class="flex items-center px-3 py-2 rounded text-gray-500 hover:text-gray-900">
          <svg
            class="w-6 h-6"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M4 6h16M4 12h16m-7 6h7"
            ></path>
          </svg>
        </button>
      </div>
    </div>
  </nav>
</template>

<script>
import { computed, onMounted } from 'vue'
import { useStore } from 'vuex'

export default {
  setup() {
    const store = useStore()
    onMounted(() => {
      store.dispatch('initializeAuth')
    })

    const username = computed(() => store.getters['username'])
    const role = computed(() => store.getters['role'])
    const isAuthenticated = computed(() => store.getters['isAuthenticated'])

    return {
      username,
      role,
      isAuthenticated
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 1280px;
  margin: auto; /* Center the container */
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }
  .hidden {
    display: none; /* Hide the elements initially */
  }
  .sm\:hidden {
    display: block; /* Show the elements for small screens */
  }
  .sm\:flex {
    display: none; /* Hide the elements for small screens */
  }
  .flex {
    flex-wrap: wrap; /* Allow items to wrap on small screens */
    justify-content: center; /* Center items on small screens */
  }
  .space-x-6 {
    justify-content: space-around; /* Adjust spacing between items on small screens */
  }
}
</style>
