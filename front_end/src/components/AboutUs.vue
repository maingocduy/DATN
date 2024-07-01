<template>
  <div>
    <!-- Hero Section -->
    <section class="hero">
      <el-row type="flex" justify="center" align="middle" class="hero-content">
        <el-col :xs="22" :sm="20" :md="16" :lg="12" class="hero-text">
          <h1>Hội Bác Sĩ Tình Nguyện - Vì sức khỏe cộng đồng</h1>
          <p>
            Chúng tôi là một nhóm bác sĩ tình nguyện cam kết mang lại sức khỏe và hạnh phúc cho
            những cộng đồng khó khăn.
          </p>
          <div class="hero-buttons">
            <el-button type="primary" size="large" @click="goToProject">Tham gia ngay</el-button>
          </div>
        </el-col>
      </el-row>
    </section>

    <!-- About Us Section -->
    <section class="about-us">
      <el-row type="flex" justify="center" class="about-us-content">
        <el-col :xs="22" :sm="20" :md="16" :lg="12">
          <h2>Về chúng tôi</h2>
          <p>
            Hội Bác Sĩ Tình Nguyện được thành lập với mục tiêu cung cấp dịch vụ y tế miễn phí cho
            những người khó khăn và kêu gọi sự đóng góp từ cộng đồng để duy trì các hoạt động khám
            chữa bệnh tình nguyện.
          </p>
          <el-row :gutter="20" type="flex" justify="center">
            <el-col :xs="12" :sm="6">
              <el-card shadow="always" class="info-card">
                <div class="card-content">
                  <el-icon><el-icon-s-promotion /></el-icon>
                  <p>Tận tâm</p>
                </div>
              </el-card>
            </el-col>
            <el-col :xs="12" :sm="6">
              <el-card shadow="always" class="info-card">
                <div class="card-content">
                  <el-icon><el-icon-s-check /></el-icon>
                  <p>Chuyên nghiệp</p>
                </div>
              </el-card>
            </el-col>
            <el-col :xs="12" :sm="6">
              <el-card shadow="always" class="info-card">
                <div class="card-content">
                  <el-icon><el-icon-s-opportunity /></el-icon>
                  <p>Minh bạch</p>
                </div>
              </el-card>
            </el-col>
            <el-col :xs="12" :sm="6">
              <el-card shadow="always" class="info-card">
                <div class="card-content">
                  <el-icon><el-icon-s-cooperation /></el-icon>
                  <p>Bền vững</p>
                </div>
              </el-card>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
    </section>
  </div>
</template>

<script>
export default {
  name: 'AboutUs',
  methods: {
    goToProject() {
      this.$router.push('/project')
    }
  }
}
</script>

<style scoped>
.hero {
  background-image: url('../../public/Images/anhAboutUs.jpg');
  background-size: cover;
  background-position: center;
  padding: 100px 0;
}

.hero-content {
  text-align: center;
}

.hero-text {
  color: #333;
  background-color: rgba(255, 255, 255, 0.9);
  padding: 30px;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.hero-text h1 {
  font-size: 2.5rem;
  margin-bottom: 1rem;
  color: #2c3e50;
}

.hero-text p {
  font-size: 1.25rem;
  margin-bottom: 1.5rem;
}

.hero-buttons {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

.about-us {
  padding: 60px 20px;
  background-color: #f5f5f5;
  background-image: url("../../public/Images/groupProject.png");
}

.about-us-content {
  text-align: center;
}

.about-us-content h2 {
  font-size: 2.5rem;
  margin-bottom: 1rem;
  color: #2c3e50;
}

.about-us-content p {
  font-size: 1.5rem;
  margin-bottom: 2rem;
}

.info-card {
  margin-bottom: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 120px; /* Đảm bảo chiều cao đồng nhất */
}

.card-content {
  text-align: center;
}

.card-content p {
  margin-top: 10px;
  font-size: 14px;
}

@media (max-width: 768px) {
  .hero-text h1 {
    font-size: 2rem;
  }

  .hero-text p {
    font-size: 1rem;
  }

  .about-us-content h2 {
    font-size: 2rem;
  }

  .about-us-content p {
    font-size: 1.25rem;
  }

  .card-content p {
    font-size: 1rem;
  }
}

@media (max-width: 480px) {
  .hero-text h1 {
    font-size: 1.75rem;
  }

  .hero-text p {
    font-size: 0.875rem;
  }

  .about-us-content h2 {
    font-size: 1.75rem;
  }

  .about-us-content p {
    font-size: 1rem;
  }

  .card-content p {
    font-size: 0.875rem;
  }
}
</style>
