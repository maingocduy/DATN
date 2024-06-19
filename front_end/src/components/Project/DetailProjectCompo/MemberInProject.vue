<template>
  <table class="table-auto w-full border-collapse border border-gray-200">
    <thead>
      <tr>
        <th class="px-4 py-2 bg-gray-100">Tên</th>
        <th class="px-4 py-2 bg-gray-100">Email</th>
        <th class="px-4 py-2 bg-gray-100">Số điện thoại</th>
        <th class="px-4 py-2 bg-gray-100">Chuyên ngành</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="member in members" class="hover:bg-gray-50">
        <td class="border px-4 py-2">{{ member.name }}</td>
        <td class="border px-4 py-2">{{ member.email }}</td>
        <td class="border px-4 py-2">{{ member.phone }}</td>
        <td class="border px-4 py-2">{{ member.groups.group_name }}</td>
      </tr>
    </tbody>
  </table>
</template>
<script>
import { fetchProjectData } from './fetchProjectDetail.js'
export default {
  data() {
    return {
      project: {},
      error: null,
      loading: false,
      members: {}
    }
  },
  mounted() {
    this.fetchProject()
  },
  methods: {
    async fetchProject() {
      const projectName = this.$route.params.name
      this.project = await fetchProjectData(projectName)
      this.members = this.project.member
    }
  }
}
</script>

<style>
/* Không cần thêm CSS vì chúng ta đã sử dụng các lớp từ Tailwind CSS */
</style>
