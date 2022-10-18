<template>
 <div class="container">
<div class="heading">Cart Contents</div>
    <img
      class="logo"
      :src="`img/cart.jpg`"
      style="text-align:center; margin-left:8vh"
    />
    <div class="status">{{ state.status }}</div>
   <div v-if="state.cart.length > 0" id="cart">
      <DataTable
        :value="state.cart"
        responsiveLayout="scroll"
        scrollHeight="35vh"
        dataKey="id"
        class="p-datatable-striped"
        id="cart-contents"
      >
        <ColumnGroup type="header">
          <Row>
            <Column header="Name" />
            <Column header="Quantity" />
            <Column header="Price" />
            <Column header="Extended" />
          </Row>
        </ColumnGroup>
        <Column field="product.productName" />
        <Column field="qty" />
        <Column field="product.mrsp">
          <template #body="slotProps">
            {{ formatCurrency(slotProps.data.product.mrsp) }}
          </template>
        </Column>
        <Column>
          <template #body="slotProps">
            {{
              formatCurrency(slotProps.data.product.mrsp * slotProps.data.qty)
            }}
          </template>
        </Column>
        <ColumnGroup type="footer">
          <Row>
            <Column footer="Subtotal:" :colspan="3" footerStyle="text-align:right"/>
            <Column :footer="formatCurrency(state.subTot)" />
          </Row>
          <Row>
            <Column footer="Tax:" :colspan="3" footerStyle="text-align:right"/>
            <Column :footer="formatCurrency(state.tax)" />
          </Row>
          <Row>
            <Column footer="Total:" :colspan="3" footerStyle="text-align:right"/>
            <Column :footer="formatCurrency(state.total)" />
          </Row>
        </ColumnGroup>
      </DataTable>
    </div>
      <Button
        style="margin-top: 2vh"
        label="Add Cart"
        @click="addCart"
        v-if="state.cart.length > 0"
        class="p-button-outlined margin-button1"
      />
       &nbsp;
      <Button
        label="Empty Cart"
        style="margin-top:2vh;margin-left:15vh"
        class="p-button-outlined margin-button1"
        v-if="state.cart.length > 0"
        @click="clearCart"
      />  
    </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { useRouter } from "vue-router";
import { poster } from "../util/apiutil";
export default {
  setup() {
    const router = useRouter();

    onMounted(() => {
         if (sessionStorage.getItem("cart")) {
        state.cart = JSON.parse(sessionStorage.getItem("cart"));
        state.cart.map((cartItem) => {
          state.qty += cartItem.qty;
          state.subTot += cartItem.product.mrsp * cartItem.qty;
          state.tax = state.subTot * 0.13;
          state.total = state.subTot + state.tax;
        });
      } else {
        state.cart = [];
      }
    });


    let state = reactive({
      status: "",
      extended: 0,
      subTot: 0,
      tax: 0,
      total: 0,
      qty: 0,
      cart: [],
    });

    const clearCart = () => {
      sessionStorage.removeItem("cart");
      state.cart = [];
      state.status = "cart cleared";
    };

     const addCart = async () => {
      let customer = JSON.parse(sessionStorage.getItem("customer"));
      let cart = JSON.parse(sessionStorage.getItem("cart"));
      try {
        state.status = "sending cart info to server";
        let cartHelper = { email: customer.email, selections: cart };

        let payload = await poster("Order", cartHelper);
        if (payload.indexOf("not") > 0) {
          state.status = payload;
        } else {
          clearCart();
          state.status = payload;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error add cart: ${err}`;
      }
    };



    //formats the integer value with the dollar currency
    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };



    return {
      state,
      clearCart,
      formatCurrency,
      addCart,
    };
  },
};
</script>
