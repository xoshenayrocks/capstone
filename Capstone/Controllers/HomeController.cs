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

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ProductList()
        {
            var model = new ProductListViewModel();
            var list = await _productrepository.DisplayProducts();

            model.ProductList = list.Select(products=> new ProductNameAndId() { Name = products.Name, ProductId = products.Id})
                .ToList();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
