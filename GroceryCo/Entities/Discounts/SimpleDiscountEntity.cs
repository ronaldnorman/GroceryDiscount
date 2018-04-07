using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Entities.Discounts
{
    public class SimpleDiscountEntity : BaseDiscountEntity
    {
        public decimal DiscountedPrice { get; set; }
    }
}
