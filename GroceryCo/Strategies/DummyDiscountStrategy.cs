using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCo.Entities;

namespace GroceryCo.Strategies
{
    public class DummyDiscountStrategy : IDiscountStrategy
    {
        public LineItem GenerateDiscountLineItem(SkuEntity skuEntity, BasketItemEntity basketItem)
        {
            return null;
        }

        public void Reset()
        {
            return;
        }

    }
}
