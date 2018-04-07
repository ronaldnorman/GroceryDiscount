using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Entities
{
    public class LineItem
    {
        // This will help us determine when enumerating what time of a line item is this
        public DiscountTypes LineItemType { get; set; }

        public string Sku { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal Total { get; set; }
    }
}
