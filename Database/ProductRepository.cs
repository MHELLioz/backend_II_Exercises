using System.Data;
using Microsoft.Data.SqlClient;
using backend_II_Exercises.Models;

namespace backend_II_Exercises.Database
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            // Lee la cadena de conexión de appsettings.json o de variables del archivo .env
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                ?? throw new InvalidOperationException("Cadena de conexión no configurada.");
        }

        // 1. Obtener todos los productos activos (@method = 'Products')
        public async Task<GeneralResponse<List<Product>>> GetAllActiveProductsAsync(bool isActive)
        {
            var response = new GeneralResponse<List<Product>> { Data = new List<Product>() };

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetAllActiveProducts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@method", "Products");
                command.Parameters.AddWithValue("@isActive", isActive);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Code = reader["code"]?.ToString() ?? "0";
                        response.Message = reader["message"]?.ToString() ?? "";

                        if (response.Code == "1")
                        {
                            response.Data.Add(MapProduct(reader));
                        }
                    }
                }
            }

            return response;
        }

        // 2. Obtener producto por SKU (@method = 'GetProductBySKU')
        public async Task<GeneralResponse<Product>> GetProductBySkuAsync(string sku)
        {
            var response = new GeneralResponse<Product>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetAllActiveProducts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@method", "GetProductBySKU");
                command.Parameters.AddWithValue("@SKU", sku ?? (object)DBNull.Value);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        response.Code = reader["code"]?.ToString() ?? "0";
                        response.Message = reader["message"]?.ToString() ?? "";

                        if (response.Code == "1")
                        {
                            response.Data = MapProduct(reader);
                        }
                    }
                }
            }

            return response;
        }

        // 3. Buscar productos por Nombre (@method = 'GetProductsByName')
        public async Task<GeneralResponse<List<Product>>> GetProductsByNameAsync(string name)
        {
            var response = new GeneralResponse<List<Product>> { Data = new List<Product>() };

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_GetAllActiveProducts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@method", "GetProductsByName");
                command.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Code = reader["code"]?.ToString() ?? "0";
                        response.Message = reader["message"]?.ToString() ?? "";

                        if (response.Code == "1")
                        {
                            response.Data.Add(MapProduct(reader));
                        }
                    }
                }
            }

            return response;
        }

        private static Product MapProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = Convert.ToInt32(reader["ProductId"]),
                SKU = reader["SKU"].ToString() ?? "",
                Name = reader["Name"].ToString() ?? "",
                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                Price = Convert.ToDecimal(reader["Price"]),
                StockQuantity = Convert.ToInt32(reader["StockQuantity"])
            };
        }
    }
}