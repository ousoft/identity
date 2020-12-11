import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
import { getToken } from '@/utils/auth'


// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 60000 // request timeout
  // headers: {'Content-Type': 'application/x-www-form-urlencoded'},
  // headers: { 'Content-Type': 'application/json' },
})
// service.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
// service.defaults.headers.post['Content-Type'] = 'application/json';

// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent
    store.dispatch('app/requestLoadingStart')

    const token = getToken()
    if (token) {
      // let each request carry token
      // ['X-Token'] is a custom headers key
      // please modify it according to the actual situation
      config.headers['Authorization'] = 'Bearer ' + token
    }
    return config
  },
  error => {
    store.dispatch('app/requestLoadingEnd')
    // do something with request error
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
  */

  /**
   * Determine the request status by custom code
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    store.dispatch('app/requestLoadingEnd')
    return response.data
  },
  error => {
    store.dispatch('app/requestLoadingEnd')
    const response = error.response
    if (response.status === 401) {
      const token = getToken()
      if (token) {
        store.dispatch('app/refreshToken')
      }
      else {
        store.dispatch('app/logout')
      }
    } else if (response.status === 403) {
      store.dispatch('app/logout')
    } else if (response.status === 400) {
      Message({
        message: response.data.message,
        type: 'warning',
      })
    }

    return Promise.reject(error)
  }
)

export default service
