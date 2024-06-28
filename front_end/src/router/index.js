// src/router/index.js

import { createRouter, createWebHistory } from 'vue-router'
import AppLayout from '../mainVue/DefaultLayout.vue'
import AdminLayout from '../mainVue/AdminLayout.vue'

// Routes for the main application
const mainRoutes = [
  {
    path: '/',
    component: AppLayout,
    children: [
      {
        path: '',
        name: 'home',
        component: () => import('../views/HomeView.vue')
      },
      {
        path: 'blog',
        name: 'blog',
        component: () => import('../views/BlogTableView.vue')
      },
      {
        path: 'addBlog',
        name: 'addBlog',
        component: () => import('../views/TiptapView.vue')
      },
      {
        path: 'Momo/:name',
        name: 'Momo',
        component: () => import('../views/momoView.vue')
      },
      {
        path: 'Response',
        name: 'Response',
        component: () => import('../views/ResponseMomoView.vue')
      },
      {
        path: 'project',
        name: 'project',
        component: () => import('../views/projectView.vue')
      },
      {
        path: 'project/:name',
        name: 'projectDetail',
        component: () => import('../components/Project/DetailProject.vue')
      },
      {
        path: 'test',
        name: 'test',
        component: () => import('../views/test.vue')
      },
      {
        path: 'login',
        name: 'login',
        component: () => import('../views/loginView.vue')
      },
      {
        path: 'newProject',
        name: 'newProject',
        component: () => import('../views/newProjectView.vue')
      },
      {
        path: 'register',
        name: 'register',
        component: () => import('../views/RegisterView.vue')
      },
      {
        path: 'ResponseRegister',
        name: 'ResponseRegister',
        component: () => import('../views/ResponseRegisterView.vue')
      },
      {
        path: 'AboutUs',
        name: 'AboutUs',
        component: () => import('../views/AboutUsView.vue')
      },
      {
        path: 'BlogTable',
        name: 'BlogTable',
        component: () => import('../views/BlogTableView.vue')
      },
      {
        path: 'BlogDetail/:id',
        name: 'BlogDetail',
        component: () => import('../views/BlogDetailView.vue')
      },
      {
        path: 'Profile/:username',
        name: 'Profile',
        component: () => import('../views/ProfileView.vue')
      },
      {
        path: 'Blog/Update/:id',
        name: 'UpdateBlog',
        component: () => import('../views/UpdateBlogView.vue')
      }
    ]
  }
]

// Routes for the admin application
const adminRoutes = [
  {
    path: '/admin',
    component: AdminLayout,
    children: [
      {
        path: '',
        name: 'adminHome',
        component: () => import('../views/AdminView.vue')
      },
      {
        path: 'ManagerBlog',
        name: 'ManagerBlog',
        component: () => import('../views/ManagerBlogView.vue')
      }
      // Add other admin-specific routes here
    ]
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [...mainRoutes, ...adminRoutes]
})

export default router
