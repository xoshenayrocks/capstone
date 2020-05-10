using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.Services
{
    public interface IProductRepo
    {
         Task<IEnumerable<Product>> DisplayProducts();
    }

}
