import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import Image from 'primevue/image'
import Dropdown from 'primevue/dropdown';
import Column from 'primevue/column';
import DataTable from 'primevue/datatable'
import ColumnGroup from 'primevue/columngroup';
import Row from 'primevue/row';
import Dialog from 'primevue/dialog';
import InputNumber from 'primevue/inputnumber';
import Toolbar from 'primevue/toolbar';
import Menu from 'primevue/menu';
import InputText from 'primevue/inputtext';
import router from "./router";

import 'primevue/resources/primevue.min.css'; //core css
import 'primevue/resources/themes/md-light-indigo/theme.css'; //theme
import 'primeicons/primeicons.css'; //icons
import './style.css';


//We need to import and add the components in this file.
const app = createApp(App)
app.use(PrimeVue);
app.use(router);
app.component('Button', Button);
app.component('Dropdown', Dropdown);
app.component('Image', Image);
app.component('DataTable', DataTable);
app.component('Column', Column);
app.component('ColumnGroup', ColumnGroup);
app.component('Row', Row);
app.component('Dialog', Dialog);
app.component('InputNumber', InputNumber);
app.component('Toolbar', Toolbar);
app.component('Menu', Menu);
app.component('InputText', InputText);
app.mount('#app');
