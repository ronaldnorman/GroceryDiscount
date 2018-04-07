using GroceryCo.Entities.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Entities
{
    public class SkuEntity
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
        public DiscountTypes DiscountType { get; set; }
        public SimpleDiscountEntity SimpleDiscountEntity { get; set; }
        public GroupDiscountEntity GroupDiscountEntity { get; set; }
        public AddonDiscountEntity AddonDiscountEntity { get; set; }
    }
}
