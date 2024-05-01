using FinalTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private AdventureWorks2022Context _context;

        public ProductosController(AdventureWorks2022Context context)
        {
            _context = context;
        }
        [HttpGet("productos-mas-vendidos")]
        public IActionResult ObtenerProductosMasVendidos(int numeroProductos)
        {
            var productosMasVendidos = _context.SalesOrderDetails
                .GroupBy(sod => sod.ProductId)
                .Select(g => new
                {
                    ProductID = g.Key,
                    TotalVentas = g.Sum(sod => sod.OrderQty),
                    ContribucionPorcentualVenta = (double)g.Sum(sod => sod.OrderQty) / _context.SalesOrderDetails.Sum(sod => sod.OrderQty) * 100
                })
                .OrderByDescending(g => g.TotalVentas)
                .Take(numeroProductos)
                .ToList();

            var resultado = new List<object>();
            foreach (var producto in productosMasVendidos)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == producto.ProductID);
                if (product != null)
                {
                    var categoria = _context.ProductSubcategories
                        .Include(ps => ps.ProductCategory)
                        .FirstOrDefault(ps => ps.ProductSubcategoryId == product.ProductSubcategoryId);

                    if (categoria != null)
                    {
                        var ventasCategoria = _context.SalesOrderDetails
                            .Where(sod => sod.ProductId == product.ProductId)
                            .Sum(sod => sod.OrderQty);

                        var contribucionPorcentualCategoria = (double)ventasCategoria / _context.SalesOrderDetails.Sum(sod => sod.OrderQty) * 100;

                        resultado.Add(new
                        {
                            ProductoID = producto.ProductID,
                            NombreProducto = product.Name,
                            NombreCategoria = categoria.ProductCategory.Name,
                            VentasTotales = producto.TotalVentas,
                            ContribucionPorcentualVenta = producto.ContribucionPorcentualVenta,
                            //VentasCategoria = ventasCategoria,
                            //ContribucionPorcentualCategoria = contribucionPorcentualCategoria
                        });
                    }
                }
            }

            return Ok(resultado);
        }

        [HttpGet("sales-report")]
        public IActionResult GetSalesReport(int numberOfRows, string color = null, string tamaño = null, string billCity = null)
        {
            var query = _context.SalesOrderHeaders
                .Include(soh => soh.SalesOrderDetails)
                .Include(soh => soh.Customer)
                .Include(soh => soh.SalesPerson)
                .SelectMany(soh => soh.SalesOrderDetails, (soh, sod) => new
                {
                    soh.SalesOrderId,
                    soh.OrderDate,
                    CustomerId = soh.CustomerId,
                    ProductId = sod.ProductId,
                    Product = _context.Products.FirstOrDefault(p => p.ProductId == sod.ProductId), // Buscar el producto por ProductId
                    ProductCategory = _context.Products
                        .Where(p => p.ProductId == sod.ProductId)
                        .Select(p => p.ProductSubcategory.ProductCategory.Name)
                        .FirstOrDefault(),
                    sod.UnitPrice,
                    Quantity = sod.OrderQty,
                    TotalPrice = sod.UnitPrice * sod.OrderQty,
                    soh.SalesPersonId,
                    SalesPersonName = _context.VSalesPersonSalesByFiscalYears
                        .FirstOrDefault(s => s.SalesPersonId == soh.SalesPersonId),
                    soh.ShipToAddress,
                    soh.BillToAddress
                })
                .AsQueryable();

            // Aplicar filtro de color si se proporciona
            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(sod => sod.Product.Color == color);
            }

            // Aplicar filtro de tamaño si se proporciona
            if (!string.IsNullOrEmpty(tamaño))
            {
                query = query.Where(sod => sod.Product.Size == tamaño);
            }

            // Aplicar filtro de ciudad de facturación si se proporciona
            if (!string.IsNullOrEmpty(billCity))
            {
                query = query.Where(soh => soh.BillToAddress.City == billCity);
            }

            var salesReport = query
                .Take(numberOfRows) // Limita el número de filas
                .ToList();

            return Ok(salesReport);
        }




        // GET: api/Product
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }





        // GET: api/<ProductosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProductosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
