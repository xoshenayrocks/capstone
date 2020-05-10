using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.config;
using Capstone.Models;
using Microsoft.Extensions.Options;
using Dapper;
using System.Linq;
using System.Data.SqlClient;

namespace Capstone.Services
{
    public class ProductRepo : IProductRepo
    {
        private readonly string _connectionstring;

        public ProductRepo(IOptions<DatabaseConfig> config)
        {
            _connectionstring = config.Value.ConnectionStrings;
        }

        public async Task<IEnumerable<Product>> DisplayProducts()
        {
            const string queryString = "Select * from [dbo].Products";

            using (var connection = new SqlConnection(_connectionstring))
            {
                IEnumerable<Product> orderDetail = await connection.QueryAsync<Product>(queryString);

                return orderDetail;
            }
        }
    }
}
