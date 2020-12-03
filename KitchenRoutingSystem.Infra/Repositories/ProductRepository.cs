using Dapper;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Infra.Repositories.Base;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;

        public async Task<Product> Add(Product obj)
        {
            var sql = "INSERT INTO Product (ProductId, Value, Quantity, ProductType, Status) Values (@ProductId, @Value, @Quantity, @ProductType, @Status);";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, obj);
                return obj;
            }
        }

        public async Task<int> Delete(int id)
        {
            var sql = "DELETE FROM Product WHERE ProductId = @Id;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows;
            }
        }

        public async Task<Product> Get(int id)
        {
            var sql = "SELECT * FROM Product WHERE ProductId = @Id;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Product>(sql, new { Number = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var sql = "SELECT * FROM Product;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Product>(sql);
                return result;
            }
        }

        public async Task<int> Update(Product entity)
        {
            var sql = "UPDATE Product SET ProductId = @ProductId, Value = @Value, Quantity = @Quantity, ProductType = @ProductType, Status = @Status  WHERE ProductId = @Id;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows;
            }
        }
    }
}