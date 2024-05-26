<template>
  <nav class="bg-white shadow-lg">
    <div class="container mx-auto px-4 py-3 flex justify-between items-center">
      <a href="#" class="flex items-center space-x-3">
        <img src="https://flowbite.com/docs/images/logo.svg" class="h-8" alt="Flowbite Logo" />
        <span class="text-xl font-semibold text-gray-800">Flowbite</span>
      </a>
      <div class="hidden sm:flex items-center space-x-6">
        <ul class="flex flex-row font-medium space-x-8">
          <li><a href="/" class="text-gray-800 hover:text-blue-500">Trang chủ</a></li>
          <li><a href="/project" class="text-gray-800 hover:text-blue-500">Dự án</a></li>
          <li><a href="#" class="text-gray-800 hover:text-blue-500">Blog</a></li>
          <li><a href="#" class="text-gray-800 hover:text-blue-500">Danh sách bác sĩ</a></li>
        </ul>
        <div class="flex items-center space-x-6">
          <input
            type="text"
            placeholder="Tìm kiếm..."
            class="rounded-full border-gray-300 px-3 py-2 focus:ring-blue-500 focus:border-blue-500"
          />
          <a href="#" class="text-sm text-gray-800 hover:text-blue-500">Đăng ký</a>
          <a href="#" class="text-sm text-gray-800 hover:text-blue-500">Đăng nhập</a>
        </div>
      </div>
      <div class="sm:hidden">
        <button class="flex items-center px-3 py-2 rounded text-gray-500 hover:text-gray-900">
          <svg
            class="w-5 h-5"
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

<script setup></script>

<style scoped>
.container {
  max-width: 1280px;
}

@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }
}
</style>
