<template>
  <div>
    <button @click="showNotification">Hiển thị thông báo</button>
  </div>
</template>

<script>
export default {
  methods: {
    showNotification() {
      this.$notify({
        type: 'success',
        title: 'Thông báo',
        text: 'Đã hiển thị thông báo thành công!'
      })
    }
  }
}
</script>
