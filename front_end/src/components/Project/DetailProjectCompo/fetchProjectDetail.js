// Import axios hoặc fetch, hoặc bất kỳ thư viện nào bạn sử dụng để thực hiện HTTP request
import axios from 'axios'

// Định nghĩa hàm fetchProjectData
export async function fetchProjectData(projectName) {
  try {
    const response = await axios.post(`https://localhost:7188/api/Project/get_project`, {
      projectName: projectName
    })
    return response.data // hoặc bạn có thể trả về response hoàn toàn nếu bạn muốn
  } catch (error) {
    throw new Error('Không thể lấy dữ liệu dự án.') // hoặc xử lý lỗi theo ý của bạn
  }
}
