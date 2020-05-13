using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models
{
    public class CheckoutViewModel
    {
        
       public double Total { get; set; }
        public double Subtotal { get; set; }
        public double Tax { get; set; } = 0.06;
        public int CreditCardNumber { get; set; }
        public int CVV { get; set; }
        public int ExpDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }
}
