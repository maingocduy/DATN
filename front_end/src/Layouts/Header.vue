<template>
  <nav class="bg-white shadow-lg">
    <div class="container mx-auto px-4 py-3 flex justify-between items-center">
      <a href="#" class="flex items-center">
        <img
          src="../../public/Images/logo.jpg"
          class="h-16 w-16 object-cover rounded-full"
          alt="Logo"
        />
      </a>
      <div class="flex-1 hidden sm:flex justify-center">
        <ul class="flex flex-row font-medium space-x-8">
          <li><a href="/" class="text-gray-800 hover:text-blue-500">Trang chủ</a></li>
          <li><a href="#" class="text-gray-800 hover:text-blue-500">Giới thiệu</a></li>
          <li><a href="/blog" class="text-gray-800 hover:text-blue-500">Blog</a></li>

          <li><a href="/project" class="text-gray-800 hover:text-blue-500">Dự án</a></li>
          <li>
            <a href="/admin" v-if="roleManager == 'Admin'" class="text-gray-800 hover:text-blue-500"
              >Admin</a
            >
          </li>
        </ul>
      </div>
      <div class="hidden sm:flex items-center space-x-6">
        <template v-if="isAuthenticated">
          <div class="relative z-50" @click="showDropdown = !showDropdown">
            <span class="text-base text-gray-800 cursor-pointer">Xin chào, {{ username }}</span>
            <div
              v-if="showDropdown"
              class="absolute right-0 mt-2 w-48 bg-white border rounded-lg shadow-lg"
            >
              <a href="/profile" class="block px-4 py-2 text-gray-800 hover:bg-gray-100"
                >Thông tin cá nhân</a
              >
              <a href="#" @click="logout" class="block px-4 py-2 text-gray-800 hover:bg-gray-100"
                >Đăng xuất</a
              >
            </div>
          </div>
        </template>
        <template v-else>
          <a href="/register" class="text-base text-gray-800 hover:text-blue-500">Đăng ký</a>
          <a href="/login" class="text-base text-gray-800 hover:text-blue-500">Đăng nhập</a>
        </template>
      </div>
      <div class="sm:hidden">
        <button
          @click="toggleMobileMenu"
          class="flex items-center px-3 py-2 rounded text-gray-500 hover:text-gray-900"
        >
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
    <div class="sm:hidden px-4 pb-3" v-if="isMobileMenuOpen">
      <ul class="flex flex-col space-y-4">
        <li><a href="/" class="text-gray-800 hover:text-blue-500">Trang chủ</a></li>
        <li><a href="/blog" class="text-gray-800 hover:text-blue-500">Blog</a></li>
        <li><a href="#" class="text-gray-800 hover:text-blue-500">Giới thiệu</a></li>
        <li><a href="/project" class="text-gray-800 hover:text-blue-500">Dự án</a></li>
        <li v-if="isAuthenticated">
          <span class="text-base text-gray-800">Xin chào, {{ username }}</span>
        </li>
        <li v-else>
          <a href="/register" class="text-base text-gray-800 hover:text-blue-500">Đăng ký</a>
          <a href="/login" class="text-base text-gray-800 hover:text-blue-500">Đăng nhập</a>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
import { computed, onMounted, ref } from 'vue'
import { useStore } from 'vuex'
import { ElNotification } from 'element-plus'
export default {
  setup() {
    const store = useStore()
    const showDropdown = ref(false)
    const countdown = ref(0)
    const isMobileMenuOpen = ref(false)
    onMounted(() => {
      store.dispatch('initializeAuth')
    })

    const username = computed(() => store.getters['username'])
    const isAuthenticated = computed(() => store.getters['isAuthenticated'])
    const roleManager = computed(() => store.getters['role'])

    const logout = async () => {
      ElNotification({
        type: 'success',
        title: 'Thông báo',
        message: 'Đăng xuất thành công!'
      })

      // Đặt bộ đếm ngược
      countdown.value = 2
      const interval = setInterval(() => {
        if (countdown.value > 0) {
          countdown.value--
        } else {
          clearInterval(interval)
          location.reload()
        }
      }, 1000)
      await store.dispatch('logout')
    }
    const toggleMobileMenu = () => {
      isMobileMenuOpen.value = !isMobileMenuOpen.value
    }

    return {
      username,
      isAuthenticated,
      showDropdown,
      logout,
      isMobileMenuOpen,
      toggleMobileMenu,
      roleManager
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 1280px;
  margin: auto;
}

nav {
  border-bottom: 1px solid #e5e7eb; /* Gray-200 */
}

a {
  transition: color 0.3s;
}

a:hover {
  color: #1d4ed8; /* Blue-700 */
}

button {
  transition: color 0.3s;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .hidden {
    display: none;
  }
  .sm\:hidden {
    display: block;
  }
  .sm\:flex {
    display: none;
  }
  .space-x-6 {
    justify-content: space-around;
  }
}
</style>
