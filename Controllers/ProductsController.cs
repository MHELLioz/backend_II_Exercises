using Microsoft.AspNetCore.Mvc;
using backend_II_Exercises.Models;
using backend_II_Exercises.Services;

namespace backend_II_Exercises.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsServices _services;

        public ProductsController(ProductsServices services)
        {
            _services = services;
        }

        // GET: api/products
        [HttpGet]
        public async Task<GeneralResponse<List<Product>>> GetAll()
        {
            return await _services.GetAll();
        }

        // GET: api/products/PROD-001
        [HttpGet("{sku}")]
        public async Task<GeneralResponse<Product>> GetByRoute([FromRoute] string sku)
        {
            return await _services.GetByRoute(sku);
        }

        // GET: api/products/search?sku=PROD-001
        [HttpGet("search")]
        public async Task<GeneralResponse<Product>> GetByQuery([FromQuery] string sku)
        {
            return await _services.GetByQuery(sku);
        }

        // POST: api/products/search
        // Body: { "name": "Teclado" }  o  { "sku": "PROD-001" }
        [HttpPost("search")]
        public async Task<GeneralResponse<List<Product>>> GetByBody([FromBody] ProductSearchRequest request)
        {
            return await _services.GetByBody(request);
        }
    }
}