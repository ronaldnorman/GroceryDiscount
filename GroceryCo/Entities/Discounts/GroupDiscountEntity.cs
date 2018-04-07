using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Entities.Discounts
{
    public class GroupDiscountEntity : BaseDiscountEntity
    {
        public decimal GroupCount { get; set; }
        public decimal GroupPrice { get; set; }
    }
}
