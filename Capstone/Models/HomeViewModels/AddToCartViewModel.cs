using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public class AddToCartViewModel
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; } = 1; 
        public string Name { get; set; }
        public IEnumerable<ProductNameAndId> Cart { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
