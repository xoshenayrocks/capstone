using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Capstone.Models;
using Capstone.Services;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepo _productrepository;

        public HomeController(IProductRepo productrepository)
        {
            _productrepository = productrepository;
        }

        public IActionResult Index()
        {
            return View();
        }

 

        public IActionResult Menu()
        {
            var menu = new MenuViewModel();
            return View(menu);
        }

        public async Task<IActionResult> ProductList(string category)
        {
            var model = new ProductListViewModel();
            var menu = new MenuViewModel();
            menu.Category = category;
   
                var list = await _productrepository.DisplayProducts(menu);

            if (menu.Category == "drink")
            {
                model.ProductList = list.Select(products => new Product() { Name = products.Name, ProductId = products.ProductId, Description = products.Description, Price = products.Price }).Where(cat => menu.Category == "drink")
                .ToList();
            }

            if (menu.Category == "food")
            {
                model.ProductList = list.Select(products => new Product() { Name = products.Name, ProductId = products.ProductId, Description = products.Description, Price = products.Price }).Where(cat => menu.Category == "food")
      .ToList();
            }

            return View(model);
        }

        public IActionResult Order()
        {
            return RedirectToAction(nameof(AddToCart));
        }

        [HttpGet]
        public IActionResult AddToCart(int productId)
        {
            var cart = new AddToCartViewModel();
            cart.ProductId = productId;
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(AddToCartViewModel model)
        {
            var qty = model.Qty;

            var list = _productrepository.AddToCart(model, model.ProductId);
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
