<template>
  <div class="container">
    <div class="heading">Your Orders</div>
    <img
      class="item-image"
      style="margin-top: 2vh"
      width="115"
      height="95"
      :src="`img/logo.jpg`"
    />
    <div class="status">{{ state.status }}</div>
    <div id="order-list">
      <DataTable
        v-if="state.orders.length > 0"
        :value="state.orders"
        :scrollable="true"
        scrollHeight="60vh"
        dataKey="id"
        class="p-datatable-striped"
        v-model:selection="state.orderSelected"
        selectionMode="single"
        @row-select="onRowSelect"
      >
        <Column header="Order #" field="id" />
        <Column header="Date" field="orderDate">
          <template #body="slotProps">
            {{ formatDate(slotProps.data.orderDate) }}
          </template>
        </Column>
      </DataTable>
      <Dialog v-model:visible="state.selectedAnOrder" class="dialog-border">
        <div style="font-weight: bold" class="order-head">
          Order: #{{ state.orderDetails[0].orderId }}
          &nbsp;&nbsp;
          {{ state.orderDetails[0].orderDate }}
        </div>
        <br />
        <div style="font-weight: bold; text-align: center">Quantities</div>
        <DataTable
          :value="state.orderDetails"
          responsiveLayout="scroll"
          scrollHeight="48vh"
          dataKey="id"
          class="p-datatable-striped"
        >
          <ColumnGroup type="header">
            <Row>
              <Column header="Name" />
              <Column header="S" />
              <Column header="O" />
              <Column header="B" />
              <Column header="Extended" />
            </Row>
          </ColumnGroup>
          <Column field="name" />
          <Column field="qtySold" />
          <Column field="qtyOrdered" />
          <Column field="qtyBackOrdered" />
          <Column>
            <template #body="slotProps">
              {{
                formatCurrency(slotProps.data.sellingPrice * slotProps.data.qtyOrdered)
              }}
            </template>
          </Column>
          <ColumnGroup type="footer">
            <Row>
              <Column
                footer="Subtotal:"
                :colspan="4"
                footerStyle="text-align:right"
              />
              <Column :footer="formatCurrency(state.subTotal)" />
            </Row>
            <Row>
              <Column
                footer="Tax:"
                :colspan="4"
                footerStyle="text-align:right"
              />
              <Column :footer="formatCurrency(state.subTotal * 0.13)" />
            </Row>
            <Row>
              <Column
                footer="Total:"
                :colspan="4"
                footerStyle="text-align:right"
              />
              <Column :footer="formatCurrency(state.subTotal * 1.13)" />
            </Row>
          </ColumnGroup>
        </DataTable>
      </Dialog>
    </div>
  </div>
</template>
<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../util/apiutil";
export default {
  setup() {
    let state = reactive({
      status: "",
      dialogStatus: "",
      orders: [],
      selectedAnOrder: false,
      orderSelected: {},
      orderDetails: [],
      subTotal: 0,
    });

    onMounted(() => {
      loadOrders();
    });

    const loadOrders = async () => {
      try {
        let customer = JSON.parse(sessionStorage.getItem("customer"));
        const payload = await fetcher(`order/${customer.email}`);
        if (payload.error === undefined) {
          state.orders = payload;
          state.status = `loaded ${state.orders.length} orders`;
          state.selectedAnOrder = false;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const formatDate = (date) => {
      let d;
      // see if date is coming from server
      date === undefined ? (d = new Date()) : (d = new Date(Date.parse(date))); // from server
      let _day = d.getDate();
      let _month = d.getMonth() + 1;
      let _year = d.getFullYear();
      let _hour = d.getHours();
      let _min = d.getMinutes();
      if (_min < 10) {
        _min = "0" + _min;
      }
      if (_day < 10) {
        _day = "0" + _day;
      }
      if (_month < 10) {
        _month = "0" + _month;
      }
      return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
    };

    const onRowSelect = async (event) => {
      try {
        let customer = JSON.parse(sessionStorage.getItem("customer"));
        state.orderSelected = event.data;
        const payload = await fetcher(
          `order/${state.orderSelected.id}/${customer.email}`
        );
        state.orderDetails = payload;
        console.log(state.orderDetails);

        var sub = 0;
        state.orderDetails.map((orderItem) => {
          sub += orderItem.sellingPrice * orderItem.qtyOrdered;
        });
        state.subTotal = sub;

        state.dialogStatus = `details for order ${state.orderSelected.id}`;
        state.selectedAnOrder = true;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };

    return {
      state,
      formatDate,
      onRowSelect,
      formatCurrency,
    };
  },
};
</script>
