<template>
    <div class="productos">
        <h1>Productos Mas Vendidos</h1>
        <ul>
            <li v-for="producto in productos" :key="producto.ProductoID">
                <b>{{ producto.NombreProducto }}</b> - Ventas: {{ producto.VentasTotales }} - Categoria: {{ producto.NombreCategoria }}
            </li>
        </ul>
        <h1>Reporte de Ventas</h1>
        <ul>
            <li v-for="venta in ventas" :key="venta.SalesOrderId">
                <b>Orden ID:</b> {{ venta.SalesOrderId }} - <b>Fecha:</b> {{ venta.OrderDate }} - <b>Cliente:</b> {{ venta.CustomerId }} - <b>Vendedor:</b> {{ venta.SalesPersonName }}
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        name: 'ListaProductos',
        data() {
            return {
                productos: [],
                ventas: []
            };
        },
        mounted() {
            this.fetchProductos();
            this.fetchSalesReport();
        },
        methods: {
            async fetchProductos() {
                try {
                    const response = await fetch('https://localhost:7008/api/Productos/productos-mas-vendidos?numeroProductos=10');
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    const data = await response.json();
                    this.productos = data;
                } catch (error) {
                    console.error('Error fetching data:', error);
                }
            },
            async fetchSalesReport() {
                try {
                    const response = await fetch('https://localhost:7008/api/Productos/sales-report?numberOfRows=5');
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    const data = await response.json();
                    this.ventas = data;
                } catch (error) {
                    console.error('Error fetching data:', error);
                }
            }
        }
    };
</script>

<style scoped>
    /* Estilos CSS aquí */
</style>
