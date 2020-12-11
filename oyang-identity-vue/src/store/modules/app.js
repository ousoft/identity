import Cookies from 'js-cookie'
import request from '@/utils/request'
import { setToken, removeToken } from '@/utils/auth'

const state = {
  sidebar: {
    opened: Cookies.get('sidebarStatus') ? !!+Cookies.get('sidebarStatus') : true,
    withoutAnimation: false
  },
  device: 'desktop',
  currentUser: {
    id: '',
    loginName: '',
    isAuthenticated: false,
    roles: [],
    permissions: []
  },
  requestLoading: false
}

const mutations = {
  TOGGLE_SIDEBAR: state => {
    state.sidebar.opened = !state.sidebar.opened
    state.sidebar.withoutAnimation = false
    if (state.sidebar.opened) {
      Cookies.set('sidebarStatus', 1)
    } else {
      Cookies.set('sidebarStatus', 0)
    }
  },
  CLOSE_SIDEBAR: (state, withoutAnimation) => {
    Cookies.set('sidebarStatus', 0)
    state.sidebar.opened = false
    state.sidebar.withoutAnimation = withoutAnimation
  },
  TOGGLE_DEVICE: (state, device) => {
    state.device = device
  },
  setCurrentUser: (state, currentUser) => {
    state.currentUser.id = currentUser.id
    state.currentUser.loginName = currentUser.loginName
    state.currentUser.isAuthenticated = currentUser.isAuthenticated
    state.currentUser.roles = currentUser.roles
    state.currentUser.permissions = currentUser.permissions
  },
  removeCurrentUser: (state) => {
    state.currentUser.id = ''
    state.currentUser.loginName = ''
    state.currentUser.isAuthenticated = false
    state.currentUser.roles = []
    state.currentUser.permissions = []
  },
  requestLoadingStart: state => {
    state.requestLoading = true
  },
  requestLoadingEnd: state => {
    state.requestLoading = false
  }
}

const actions = {
  toggleSideBar({ commit }) {
    commit('TOGGLE_SIDEBAR')
  },
  closeSideBar({ commit }, { withoutAnimation }) {
    commit('CLOSE_SIDEBAR', withoutAnimation)
  },
  toggleDevice({ commit }, device) {
    commit('TOGGLE_DEVICE', device)
  },
  getCurrentUser({ commit }) {
    return new Promise((resolve, reject) => {
      request({
        url: '/api/Account/UserInfo',
        method: 'get'
      })
        .then(response => {
          commit('setCurrentUser', response)
          resolve()
        })
        .catch(error => {
          removeToken()
          reject(error)
        })
    })
  },
  signin({ dispatch }, loginForm) {
    return new Promise((resolve, reject) => {
      request({
        url: '/api/Account/GenerateToken',
        method: 'post',
        data: loginForm
      })
        .then(response => {
          setToken(response)
          return dispatch('getCurrentUser', response)
        })
        .then(() => {
          resolve()
        })
        .catch(error => {
          reject(error)
        })
    })
  },
  logout({ commit }) {
    removeToken()
    commit('removeCurrentUser')
  },
  refreshToken(token) {
    return new Promise((resolve, reject) => {
      request({
        url: '/api/Account/RefreshToken',
        method: 'post',
        data: token
      })
        .then(response => {
          setToken(response)
          resolve()
        })
        .catch(error => {
          reject(error)
        })
    })
  },  
  requestLoadingStart({ commit }) {
    commit('requestLoadingStart')
  }, 
  requestLoadingEnd({ commit }) {
    commit('requestLoadingEnd')
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
