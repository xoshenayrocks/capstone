using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public class ViewCartModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
      public string Name { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double Subtotal { get; set; }
        public IEnumerable<Cart> Cart { get; set; }
    }
}
