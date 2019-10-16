import Vue from 'vue'
import VueRouter from 'vue-router'
import BootstrapVue from 'bootstrap-vue'
import App from './App.vue'
import Vuex from 'vuex'

import GSignInButton from 'vue-google-signin-button'
import FBSignInButton from 'vue-facebook-signin-button'

import './custom.scss'

import { store } from './store/store';

// Font Awesome
import { library } from '@fortawesome/fontawesome-svg-core'
import { faSquare, faCheckSquare, faTable, faEdit, faSortAmountUp, faSortAmountDown, faTrash, faPlus, faGlobe } from '@fortawesome/free-solid-svg-icons'
import { faFacebook, faGoogle } from '@fortawesome/free-brands-svg-icons'
//import { fas } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

// Mixins
import AuthMixin from './components/Mixins/AuthMixin.vue'

// Form Controls
import TextControl from './components/Controls/TextControl.vue'
import TextareaControl from './components/Controls/TextareaControl.vue'
import SelectControl from './components/Controls/SelectControl.vue'
import CheckboxControl from './components/Controls/CheckboxControl.vue'

// Widgets
import PageLoading from './components/Common/PageLoading.vue'
import PageTitle from './components/Common/PageTitle.vue'
import FilteredTable from './components/Common/FilteredTable.vue'
import FilteredTableMap from './components/Common/FilteredTableMap.vue'
import NearbyMap from './components/Common/NearbyMap.vue'

import AddressLookup from './components/Home/AddressLookup.vue'
import AuthForm from './components/Home/AuthForm.vue'
import UserSubscriptionList from './components/Home/UserSubscriptionList.vue'
import BasicMap from './components/Home/BasicMap.vue'

// Pages
import About from './components/About.vue'
import Contact from './components/Contact.vue'
import Developers from './components/Developers.vue'
import Login from './components/Login.vue'
import Logout from './components/Logout.vue'
import Home from './components/Home.vue'
import PropertyList from './components/Property/List.vue'
import PropertyView from './components/Property/View.vue'
import ApplicationUserList from './components/ApplicationUser/List.vue'
import ApplicationUserFields from './components/ApplicationUser/Fields.vue'
import ApplicationUserEdit from './components/ApplicationUser/Edit.vue'
import PoliceDispatchCallList from './components/PoliceDispatchCall/List.vue'
import PoliceDispatchCallView from './components/PoliceDispatchCall/View.vue'
import FireDispatchCallList from './components/FireDispatchCall/List.vue'
import FireDispatchCallView from './components/FireDispatchCall/View.vue'
import CrimeList from './components/Crime/List.vue'
import CrimeView from './components/Crime/View.vue'

Vue.use(VueRouter);
Vue.use(BootstrapVue);
Vue.use(require('vue-moment'));
Vue.use(Vuex);

Vue.use(AuthMixin);

Vue.use(GSignInButton);
Vue.use(FBSignInButton);

library.add(faSquare, faCheckSquare, faTable, faEdit, faSortAmountUp, faSortAmountDown, faTrash, faPlus, faGlobe, faFacebook, faGoogle);
//library.add(fas);

Vue.component('font-awesome-icon', FontAwesomeIcon);
Vue.component('page-loading', PageLoading);
Vue.component('page-title', PageTitle);
Vue.component('filtered-table', FilteredTable);
Vue.component('filtered-table-map', FilteredTableMap);
Vue.component('nearby-map', NearbyMap);

Vue.component('address-lookup', AddressLookup);
Vue.component('auth-form', AuthForm);
Vue.component('user-subscription-list', UserSubscriptionList);
Vue.component('basic-map', BasicMap);

Vue.component('text-control', TextControl);
Vue.component('textarea-control', TextareaControl);
Vue.component('select-control', SelectControl);
Vue.component('checkbox-control', CheckboxControl);

Vue.component('application-user-fields', ApplicationUserFields);

Vue.config.productionTip = false;

const routes = [
  { path: '/', component: Home, meta: { public: true } },
  { path: '/about', component: About, meta: { public: true } },
  { path: '/contact', component: Contact, meta: { public: true } },
  { path: '/developers', component: Developers, meta: { public: true } },
  { path: '/login', component: Login, meta: { public: true } },
  { path: '/logout', component: Logout, meta: { public: true } },
  { path: '/property', component: PropertyList, meta: { public: true } },
  { path: '/property/:id', component: PropertyView, meta: { public: true }, props: true },
  { path: '/policeDispatchCall', component: PoliceDispatchCallList, meta: { public: true } },
  { path: '/policeDispatchCall/:id', component: PoliceDispatchCallView, meta: { public: true }, props: true },
  { path: '/fireDispatchCall', component: FireDispatchCallList, meta: { public: true } },
  { path: '/fireDispatchCall/:id', component: FireDispatchCallView, meta: { public: true }, props: true },
  { path: '/crime', component: CrimeList, meta: { public: true } },
  { path: '/crime/:id', component: CrimeView, meta: { public: true }, props: true },
  { path: '/applicationUser', component: ApplicationUserList },
  { path: '/applicationUser/:id', component: ApplicationUserEdit, props: true },
];

const router = new VueRouter({
  routes // short for `routes: routes`
});

router.beforeEach((to, from, next) => {
  if (router.app.$root.$data && router.app.$root.$data.authenticatedUser && router.app.$root.$data.authenticatedUser.id) {
    // Allow authenticated users through
    next();
  } else if (to.matched.some(record => record.meta.public)) {
    // Allow all users to hit any page marked as public
    next();
  } else {
    // Redirect to the login page, passing the requested page as the redirect param
    next({
      path: '/login',
      query: { redirect: to.fullPath }
    });
  }
});

new Vue({
  store,
  router,
  render: h => h(App),
  data: {
    authenticatedUser: {
      username: null,
      id: null,
      roles: []
    }
  }
}).$mount('#app');
