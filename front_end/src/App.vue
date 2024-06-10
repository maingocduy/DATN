<script setup>
import AppHeader from './Layouts/Header.vue'
import AppFooter from './Layouts/Footer.vue'
</script>

<template>
  <AppHeader />
  <RouterView />
  <AppFooter />
  <notifications width="500" duration:5000 pauseOnHover="false" />
</template>

<style>
/* Định nghĩa CSS cho thẻ chính ở đây */
</style>
