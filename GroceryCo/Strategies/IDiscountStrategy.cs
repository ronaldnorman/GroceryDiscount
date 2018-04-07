using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Strategies
{
    public interface IDiscountStrategy
    {
        LineItem GenerateDiscountLineItem(SkuEntity skuEntity, BasketItemEntity basketItem);
        void Reset();
    }
}
