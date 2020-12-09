using Dapper;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Order> Add(Order obj)
        {
            var sql = "INSERT INTO Order (Number, CreateDate, LastUpdateDate, Products, Total, Notes, Status) Values (@Number, @CreateDate, @LastUpdateDate, @Products, @Total, @Notes, @Status);";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, obj);
                return obj;
            }
        }

        public async Task<int> Delete(Guid id)
        {
            var sql = "DELETE FROM Order WHERE Number = @Id;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows;
            }
        }

        public async Task<Order> Get(string id)
        {
            var sql = "SELECT * FROM Order WHERE Number = @Id;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Order>(sql, new { Number = id });
                return result.FirstOrDefault();
            }
        }


        public async Task<IEnumerable<Order>> GetAll()
        {
            var sql = "SELECT * FROM Order;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Order>(sql);
                return result;
            }
        }

        public async Task<int> Update(Order entity)
        {  
            var sql = "UPDATE Order SET Number = @Number, CreateDate = @CreateDate, LastUpdateDate = @LastUpdateDate, Products = @Products, Total = @Total, @Notes = Notes, @Status = Status  WHERE Number = @Id;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows;
            }
        }

        
    }
}