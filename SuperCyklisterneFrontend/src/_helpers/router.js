import Vue from 'vue';
import Router from 'vue-router';

import SideIkkeFundet from '../views/side-ikke-fundet.vue';

Vue.use(Router);

export const router = new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      redirect: '/hjem',
    },
    {
      path: '/hjem',
      name: 'hjem',
      component: () =>
        import(/* webpackChunkName: "hjem" */ '../views/Hjem.vue'),
    },
    {
      path: '/medlemmer',
      name: 'medlemmer',
      component: () =>
        import(/* webpackChunkName: "medlemmer" */ '../views/Medlemmer.vue'),
    },
    {
      path: '/omklubben',
      name: 'omklubben',
      component: () =>
        import(/* webpackChunkName: "omklubben" */ '../views/OmSuperCyklisterne.vue'),
    },
    {
      path: '/minkonto',
      name: 'minkonto', 
      component: () =>
        import(/* webpackChunkName: "minkonto" */ '../views/MinKonto.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () =>
        import(/* webpackChunkName: "login" */ '../views/Login.vue'),
    },
    {
      path: '/*',
      component: SideIkkeFundet,
    },
  ]
});

router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login', '/omklubben', '/medlemmer', '/hjem'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('user');

  if (authRequired && !loggedIn) {
    return next('/hjem');
  }
  next();
})