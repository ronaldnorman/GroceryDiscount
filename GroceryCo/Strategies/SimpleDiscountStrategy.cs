using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Strategies
{
    public class SimpleDiscountStrategy : IDiscountStrategy
    {
        public SimpleDiscountStrategy()
        {
        }

        public LineItem GenerateDiscountLineItem(SkuEntity skuEntity, BasketItemEntity basketItem)
        {
            if (Utils.IsCurrentDateTimeWithinRange(skuEntity.SimpleDiscountEntity.EffectiveFromDate, skuEntity.SimpleDiscountEntity.EffectiveToDate))
            {
                return new LineItem
                {
                    LineItemType = DiscountTypes.simpleDiscount,
                    Label = skuEntity.SimpleDiscountEntity.Label,
                    Sku = skuEntity.Id,
                    Total = (skuEntity.SimpleDiscountEntity.DiscountedPrice - skuEntity.Price) * basketItem.Qty
                };
            }

            return null;
        }

        public void Reset()
        {
            return;
        }
    }
}
