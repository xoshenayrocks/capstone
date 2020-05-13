using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepo _productrepository;
        public ShoppingCartController(IProductRepo productrepository)
        {
            _productrepository = productrepository;
        }
        public IActionResult AddToCart(AddToCartViewModel model)
        {
    

            return View(model);
        }

     public async Task<IActionResult> ViewCart()
        {
            var model = new ViewCartModel();
            var cart = await _productrepository.ViewCart(model);

            model.Cart = cart.Select(products => new Cart() { Name = products.Name,  Price = products.Price, Qty = products.Qty, ProductId = products.ProductId })
                .ToList();

            return View(model);
        }

        public IActionResult DeleteCartItem(int productId)
        {
            _productrepository.DeleteCartItem(productId);
            return RedirectToAction(nameof(ViewCart));
        }


        public IActionResult Checkout(ViewCartModel model)
        {
            var checkout = new CheckoutViewModel
            {
                Subtotal = model.Subtotal
            };

            return View(checkout);
        }

        public IActionResult CheckoutConfirmation(CheckoutViewModel checkout)
        {
 
            var model = new CheckoutConfirmationViewModel { Name = checkout.Name} ;
            _productrepository.Clear();
            return View(model);
        }

    }

}
