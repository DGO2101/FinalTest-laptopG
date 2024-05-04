namespace FinalTest.Models
{
    public class ObtenerProductosMasVendidosViewModel
    {
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public string NombreCategoria { get; set; }
        public int VentasTotales { get; set; }
        public double ContribucionPorcentualVenta { get; set; }
        public int VentasCategoria { get; set; }
        // Agrega aquí cualquier otra propiedad que necesites mostrar en la vista
    }

}
