using backend_II_Exercises.Database;
using backend_II_Exercises.Models;

namespace backend_II_Exercises.Services
{
    public class ProductsServices
    {
        private readonly ProductRepository _repository;

        public ProductsServices(ProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<GeneralResponse<List<Product>>> GetAll()
        {
            return await _repository.GetAllActiveProductsAsync(true);
        }

        public async Task<GeneralResponse<Product>> GetByRoute(string sku)
        {
            return await _repository.GetProductBySkuAsync(sku);
        }

        public async Task<GeneralResponse<Product>> GetByQuery(string sku)
        {
            return await _repository.GetProductBySkuAsync(sku);
        }

        public async Task<GeneralResponse<List<Product>>> GetByBody(ProductSearchRequest request)
        {
            if (request == null)
            {
                return new GeneralResponse<List<Product>>
                {
                    Code = "0",
                    Message = "El cuerpo de la solicitud no puede estar vacío."
                };
            }

            // Si envió parámetro de búsqueda por Nombre
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                return await _repository.GetProductsByNameAsync(request.Name);
            }

            // Si envió parámetro por SKU
            if (!string.IsNullOrWhiteSpace(request.SKU))
            {
                var skuResult = await _repository.GetProductBySkuAsync(request.SKU);
                var listResponse = new GeneralResponse<List<Product>>
                {
                    Code = skuResult.Code,
                    Message = skuResult.Message,
                    Data = new List<Product>()
                };

                if (skuResult.Data != null)
                {
                    listResponse.Data.Add(skuResult.Data);
                }

                return listResponse;
            }

            return new GeneralResponse<List<Product>>
            {
                Code = "0",
                Message = "Debe proporcionar al menos un criterio de búsqueda ('name' o 'sku')."
            };
        }
    }
}