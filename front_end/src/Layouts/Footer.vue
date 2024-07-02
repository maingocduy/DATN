<template>
  <footer class="bottom-0 bg-gray-800">
    <!-- Thêm class flex-shrink-0 để footer không bị co lại khi nội dung ít -->
    <div class="mx-auto w-full max-w-screen-xl p-4 py-6 lg:py-8">
      <div class="md:flex md:justify-between">
        <div class="mb-6 md:mb-0"></div>
        <div class="grid grid-cols-2 gap-8 sm:gap-6 sm:grid-cols-3">
          <!-- Các phần tử của footer -->
          <div>
            <h2 class="mb-6 text-sm font-semibold uppercase text-white">Follow us</h2>
            <ul class="text-gray-400 font-medium">
              <li class="mb-2">
                <a href="https://www.facebook.com/HoiBacSyTinhNguyen" class="hover:underline"
                  >Facebook</a
                >
              </li>
            </ul>
          </div>
        </div>
      </div>
      <hr class="my-6 sm:mx-auto border-gray-700 lg:my-8" />
      <div class="sm:flex sm:items-center sm:justify-between">
        <span class="text-sm sm:text-center text-gray-400"
          >© 2024 <a href="#" class="hover:underline">Mai Ngoc Duy</a>. All Rights Reserved.
        </span>
        <div class="flex mt-4 sm:justify-center sm:mt-0">
          <!-- Các liên kết xã hội -->
          <a
            href="https://www.facebook.com/HoiBacSyTinhNguyen"
            class="text-gray-500 hover:text-white"
          >
            <svg
              class="w-4 h-4"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 8 19"
            >
              <path
                fill-rule="evenodd"
                d="M6.135 3H8V0H6.135a4.147 4.147 0 0 0-4.142 4.142V6H0v3h2v9.938h3V9h2.021l.592-3H5V3.591A.6.6 0 0 1 5.592 3h.543Z"
                clip-rule="evenodd"
              />
            </svg>
            <span class="sr-only">Facebook page</span>
          </a>
        </div>
      </div>
    </div>
  </footer>
</template>

<script>
export default {
  name: 'AppFooter'
}
</script>

<style scoped>
.container {
  max-width: 1280px;
  margin: auto; /* Center the container */
}

/* Adjustments for smaller screens */
@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }
}
</style>
