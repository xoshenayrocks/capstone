﻿using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public class Product
    {
       public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
