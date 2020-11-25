<template>
  <div class="app row">
    <div class="items col-md-7">
      <input type="text" v-model="filter" class="form-control" placeholder="O que você está buscando ?">
      <br>
      <div class="list-group">
        <div class="list-group-item" v-for="item in filteredItems" v-bind:key="item.id">
          <div class="row">
            <div class="col-md-2">
              <div class="badge badge-info">
                {{ item.category }}
              </div>
            </div>
            <div class="col-md-6">
              <h5>{{ item.description }}</h5>
            </div>
            <div class="col-md-2">
              <h5>{{ formatMoney(item.price) }}</h5>
            </div>
            <div class="col-md-2 text-right">
              <button class="btn btn-outline-info btn-sm" @click="addItem(item)">
                <i class="fa fa-plus"></i>
              </button>
              &nbsp;
              <button class="btn btn-outline-info btn-sm" @click="deleteItem(item)">
                <i class="fa fa-minus"></i>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="order col-md-5">
      <div class="row">
        <div class="col-md">
          <h5>Pedido #{{ order.id }}</h5>
        </div>
        <div class="col-md text-right">
          <h5>{{ formatMoney(order.total) }}</h5>
        </div>
      </div>
      <hr />
      <div v-for="orderItem in order.orderItems" v-bind:key="orderItem.item.id">
        <div class="row">
          <div class="col-md-2">
            {{ orderItem.quantity }}
          </div>
          <div class="col-md-7">
            {{ orderItem.item.description }}
          </div>
          <div class="col-md-3 text-right">
            {{ formatMoney(orderItem.quantity * orderItem.item.price) }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import "bootstrap/dist/css/bootstrap.min.css";
import "font-awesome/css/font-awesome.min.css";
import axios from 'axios/dist/axios';

export default {
  name: "App",
  data() {
    return {
      filter: '',
      items: [],
      order: {
        id: Math.floor(Math.random() * 10000),
        orderItems: [],
        total: 0,
      },
    };
  },
  methods: {
    formatMoney(value) {
      const formatter = new Intl.NumberFormat("pt-BR", {
        style: "currency",
        currency: "BRL",
      });
      return formatter.format(value);
    },
    addItem(item) {
      const existingItem = this.order.orderItems.find(orderItem => orderItem.item.id === item.id);
      if (!existingItem) {
        this.order.orderItems.push({
          quantity: 1,
          item
        });
      } else {
        existingItem.quantity++;        
      }
      this.order.total += item.price;
    },
    deleteItem(item) {
      const existingItem = this.order.orderItems.find(orderItem => orderItem.item.id === item.id);
      if (existingItem) {
        existingItem.quantity--;
        this.order.total -= item.price;

        if (existingItem.quantity === 0) {
          this.order.orderItems.splice(this.order.orderItems.indexOf(existingItem), 1);
        }
      }
    },
    normalizeString(value) {
      return value.normalize("NFD").replace(/[^a-zA-Zs]/g, "").toLowerCase();
    }
  },
  computed: {
    filteredItems() {
      if (!this.filter) return this.items;

      let itemFilter = this.normalizeString(this.filter);
      
      return this.items.filter((item) => {
        let description = this.normalizeString(item.description);
        return description.includes(itemFilter);
      });
    }
  },
  created() {
    axios({
      url: 'https://localhost:5001/api/Items',
      method: 'get'
    }).then(response => {
      if (response.status === 200){
        this.items = response.data;
      }
    });
  },
};
</script>

<style>
.app {
  width: 100vw;
}
.items {
  padding: 30px;
}
.order {
  padding: 30px;
  background: #f1f1f1;
  height: 100vh;
}
</style>
