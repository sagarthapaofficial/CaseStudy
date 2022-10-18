<template>
<div>
<img class="logo" :src="`img/logo.jpg`" alt="Image Text" />
<div class="heading">Brands</div>
<div class="status">{{ state.status }}</div>

<Dropdown
  v-if="state.brands.length > 0"
  v-model="state.selectedBrand"
  :options="state.brands"
  optionLabel="name"
  optionValue="id"
  placeholder="Select Brand"
  @change="loadProducts"
/>

<div style="margin-top:2vh;" v-if="state.products.length > 0">
 <DataTable
  :value="state.products"
  :scrollable="true"
  scrollHeight="35vh"
  selectionMode="single"
  class="p-datatable-striped"
  @row-select="onRowSelect"
>
<Column style="margin-right:-35vw;">
    <template #body="slotProps" >
   <img
    :src="`img/${slotProps.data.graphicName}`"
    :alt="slotProps.data.productName"
    class="product-image"
/>
    </template>
</Column>

<Column field="description" header="Select an Item"></Column>
</DataTable>

<Dialog v-model:visible="state.itemSelected" class="dialog-border">
        <div style="text-align: center">
          <img
            :src="`img/${state.selectedItem.graphicName}`"
            :alt="state.selectedItem.productName"
            class="dialog-image"
          />
        </div>
        <br />
        <div style="margin-bottom: 2vh; font-size: larger; font-weight: bold">
          {{ state.selectedItem.productName }} -
          {{ formatCurrency(state.selectedItem.mrsp) }}
        </div>
        <br />
        {{ state.selectedItem.description }}
        <br />
      <div style="margin-top: 2vh; text-align: center; width:20vh">
          <div style="text-align:left">Qty:</div><br/>
          <span style="margin-left:-25vw">
            <InputNumber
              id="qty"
              :min="0"
              v-model="state.qty"
              :step="1"
              incrementButtonClass="plus"
              showButtons
              buttonLayout="horizontal"
              decrementButtonIcon="pi pi-minus"
              incrementButtonIcon="pi pi-plus"
            />
          </span>
        </div>
        <div style="text-align: center; margin-top: 2vh">
          <Button
            label="Add To Cart"
            @click="addToCart"
            class="p-button-outlined margin-button1"
          />
          &nbsp;
          <Button
            label="View Cart"
            class="p-button-outlined margin-button1"
            v-if="state.cart.length > 0"
            @click="viewCart"
          />
        </div>
        <div
          style="text-align: center"
          v-if="state.dialogStatus !== ''"
          class="dialog-status"
        >
          {{ state.dialogStatus }}
        </div>
      </Dialog>
</div>
</div>
</template>

<script>
import { reactive, onMounted } from "vue";
import {fetcher} from "../util/apiutil";
import {useRouter} from "vue-router";

export default {
setup() {
const router = useRouter();
//this is the main function that runs.
onMounted(() => {
loadbrands();
});

//store variables
let state = reactive({
status: "",
brands: [],
products:[],
selectedItem:{},
dialogStatus:"",
itemSelected:false,
selectedBrand: {},
qty: 0,
cart: [],
});

//This will load all the brands we have available.
 const loadbrands = async () => {
      try {
        state.status = "loading brands...";
        const payload = await fetcher(`Brand`);
        if (payload.error === undefined) {
          state.brands = payload;
          console.log(state.brands);
          state.status = `loaded ${state.brands.length} brands`;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    //this method will load all the products from the database.
    const loadProducts = async () => {
      try {
        state.status = `finding products for brand ${state.selectedBrand}...`;
        let payload = await fetcher(`Product/${state.selectedBrand}`);
        state.products = payload;
        state.status = `loaded ${state.products.length} products`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }

        if (sessionStorage.getItem("cart")) {
      state.cart = JSON.parse(sessionStorage.getItem("cart"));
      }

    };

  //this method will get information about the selected product.
    const onRowSelect = (event) => {
    try {
    state.selectedItem = event.data;
    state.dialogStatus = "";
    state.itemSelected = true;
    } catch (err) {
    console.log(err);
    state.status = `Error has occured: ${err.message}`;
    }
    };



    //Adds the selected item to the card and is added to the session storage.
    const addToCart = () => {
    const index = state.cart.findIndex(
      // is item already on the cart
      (product) => product.id === state.selectedItem.id
    );
    if (state.qty !== 0) {
      index === -1 // add
        ? state.cart.push({
            id: state.selectedItem.id,
            qty: state.qty,
            product: state.selectedItem,
          })
        : (state.cart[index] = {
            // replace
            id: state.selectedItem.id,
            qty: state.qty,
            product: state.selectedItem,
          });
      state.dialogStatus = `${state.qty} product(s) added`;
    } else {
      index === -1 ? null : state.cart.splice(index, 1); // remove
      state.dialogStatus = `product(s) removed`;
    }
    sessionStorage.setItem("cart", JSON.stringify(state.cart));
    state.qty = 0;
  };

    //formats the integer value with the dollar currency
    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };


//this redirects to the car 
const viewCart = () => {
router.push("cart");
};

return {
state,
loadbrands,
loadProducts,
onRowSelect,
addToCart,
formatCurrency,
viewCart,
};
}
}
</script>