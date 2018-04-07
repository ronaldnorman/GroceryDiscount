using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Entities.Discounts
{
    public class AddonDiscountEntity : BaseDiscountEntity
    {
        public decimal BaseCount { get; set; }
        public decimal AddonCount { get; set; }
        public decimal AddonDiscountPercent { get; set; }
    }
}
