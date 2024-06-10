import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/blog',
      name: 'blog',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/TiptapView.vue')
    },
    {
      path: '/Momo/:name',
      name: 'Momo',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/momoView.vue')
    },
    {
      path: '/Response',
      name: 'Response',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/ResponseMomoView.vue')
    },
    {
      path: '/project',
      name: 'project',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/projectView.vue')
    },
    {
      path: '/project/:name', // Thêm route cho trang chi tiết dự án và đặt parameter cho id của dự án
      name: 'projectDetail',
      component: () => import('../components/DetailProject.vue')
    },
    {
      path: '/test', // Thêm route cho trang chi tiết dự án và đặt parameter cho id của dự án
      name: 'test',
      component: () => import('../views/test.vue')
    },
    {
      path: '/login', // Thêm route cho trang chi tiết dự án và đặt parameter cho id của dự án
      name: 'login',
      component: () => import('../views/loginView.vue')
    },
    {
      path: '/newProject', // Thêm route cho trang chi tiết dự án và đặt parameter cho id của dự án
      name: 'newProject',
      component: () => import('../views/newProjectView.vue')
    },
    {
      path: '/register', // Thêm route cho trang chi tiết dự án và đặt parameter cho id của dự án
      name: 'register',
      component: () => import('../views/RegisterView.vue')
    },
    {
      path: '/ResponseRegister',
      name: 'ResponseRegister',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/ResponseRegisterView.vue')
    },
    {
      path: '/notify',
      name: 'notify',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/notifyView.vue')
    }
  ]
})

export default router
