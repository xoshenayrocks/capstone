using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.Services
{
    public interface IProductRepo
    {
         Task<IEnumerable<Product>> DisplayProducts(MenuViewModel menu);
        bool AddToCart(AddToCartViewModel cart, int productId);
        Task<IEnumerable<Cart>> ViewCart(ViewCartModel model);
        public bool DeleteCartItem(int ProductId);
    }

}
