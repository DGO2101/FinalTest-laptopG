namespace FinalTest.Models
{
    public class SalesReportViewModel
    {
        public int SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductCategory { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int? SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public string ShipToAddress { get; set; }
        public string BillToAddress { get; set; }
        // Agrega más propiedades según necesites
    }

}
