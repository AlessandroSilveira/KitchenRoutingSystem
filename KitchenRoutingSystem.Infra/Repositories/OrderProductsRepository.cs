using Dapper;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class OrderProductsRepository : IOrderProductsRepository
    {
        private readonly IConfiguration _configuration;

        public OrderProductsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<OrderProduct> Add(OrderProduct entity)
        {
            var sql = "INSERT INTO OrderProducts (OrderId, ProductId, Value, Quantity) Values (@OrderId, @ProductId, @Value, @Quantity);";

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var affectedRows = await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public async Task<int> Delete(int id)
        {
            var sql = "DELETE FROM OrderProducts WHERE Id = @Id;";

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows;
        }

        public async Task<OrderProduct> Get(string id)
        {
            var sql = "SELECT * FROM OrderProducts WHERE Id = @Id;";
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            connection.Open();

            var result = await connection.QueryAsync<OrderProduct>(sql, new { Number = id });
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<OrderProduct>> GetAll()
        {
            var sql = "SELECT * FROM OrderProducts;";

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var result = await connection.QueryAsync<Order>(sql);
            return (IEnumerable<OrderProduct>)result;
        }

        public async Task<int> Update(OrderProduct entity)
        {           
            var sql = "UPDATE OrderProducts SET OrderId = @OrderId, ProductId = @ProductId, Value = @Value, Quantity = @Quantity WHERE Id = @Id;";

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var affectedRows = await connection.ExecuteAsync(sql, entity);
            return affectedRows;
        }
    }
}
