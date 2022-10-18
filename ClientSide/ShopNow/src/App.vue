<template>
<div>

<Toolbar>
    <template #start>
    <span style="font-weight:bold;font-size:larger;">ShopNow</span>
    </template>
    <template #end>
    <Menu id="overlay_menu" ref="menu" :model="menuitems" :popup="true" />
    <Button icon="pi pi-bars" @click="toggle" />
    </template>
</Toolbar>

<router-view />

<div class="footer">
best viewed on mobile device — Sagar Thapa &copy;{{
new Date().getFullYear()
}}
</div>
</div>
</template>

<script>
import { ref } from "vue";
export default {
setup() {
const menuitems = ref([
{ label: "Home", to: "/" },
{ label: "Brands", to: "/Brands" },
{ label: "Carts", to: "/carts" },
{ label: "Orders", to: "/orders" },
{ label: "Register", to: "/register" },
{ label: "Login", to: "/login" },
{ label: "Logout", to: "/logout" },
]);
const menu = ref();
const toggle = (event) => {
menu.value.toggle(event);
};
return {
menuitems,
menu,
toggle,
};
},
};
</script>
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>

