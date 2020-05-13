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

        public async Task<IEnumerable<Product>> DisplayProducts(MenuViewModel menu)
        {
            const string queryString = @"select * from Products
Where Category = @Category";

            using (var connection = new SqlConnection(_connectionstring))
            {
                var orderDetail = await connection.QueryAsync<Product>(queryString, menu);

                return orderDetail;
            }
        }


        public bool AddToCart(AddToCartViewModel cart, int productId)
        {
            var query = @"Insert into Cart(Qty, ProductId)
Values(@Qty, @ProductId)";


            using (var connection = new SqlConnection(_connectionstring))
            {
                try
                {
                    var orderDetail = connection.Execute(query, cart);
                    return true;
                }
                catch
                {
                    return false;
                }
             
            }
        }

        public async Task<IEnumerable<Cart>> ViewCart(ViewCartModel model)
        {
            const string query = @"select Name, Price, Qty, Cart.ProductId from Cart
join Products on Products.ProductId = Cart.ProductId";
            using (var connection = new SqlConnection(_connectionstring))
            {
                var orderDetail = await connection.QueryAsync<Cart>(query, model);

                return orderDetail;
            }
        }

        public bool DeleteCartItem(int ProductId)
        {
            var query = @"DELETE FROM Cart WHERE ProductId = @ProductId;";


            using (var connection = new SqlConnection(_connectionstring))
            {
                try
                {
                    var orderDetail = connection.Execute(query, new { ProductId });
                    return true;
                }
                catch
                {

                    return false;
                }
            }
        }


        public  async void Clear()
         {

             var query = "Truncate Table Cart";
             using (var connection = new SqlConnection(_connectionstring))
             {
                 var orderDetail = await connection.ExecuteAsync(query);

             }

         }
    }
}
