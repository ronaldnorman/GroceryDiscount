using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Strategies
{
    public class GroupDiscountStrategy : IDiscountStrategy
    {
        Dictionary<string, decimal> skuCounter;
        private readonly int discountAlreadyGivenFlag = -1;

        public GroupDiscountStrategy()
        {
            this.Reset();
        }

        public LineItem GenerateDiscountLineItem(SkuEntity skuEntity, BasketItemEntity basketItem)
        {
            if (Utils.IsCurrentDateTimeWithinRange(skuEntity.GroupDiscountEntity.EffectiveFromDate, skuEntity.GroupDiscountEntity.EffectiveToDate))
            {
                // We already gave the discount as customer already bought more than the group count. 
                // This could change based on future requirements and business rules
                if (skuCounter.ContainsKey(skuEntity.Id))
                {
                    // First check if the group discount was already given for this item
                    if (skuCounter[skuEntity.Id] == discountAlreadyGivenFlag)
                        return null;
                    
                    // Keep a count of items
                    skuCounter[skuEntity.Id] = skuCounter[skuEntity.Id] + basketItem.Qty;
                }
                else
                {
                    // Add the item for the first time to start tracking it its count
                    skuCounter.Add(skuEntity.Id, basketItem.Qty);
                }

                // Now check if we've reached the group count required to give a discount. This should happen only once once. We don't want to keep giving discounts after it goes over 3 for example
                if (skuCounter[skuEntity.Id] >= skuEntity.GroupDiscountEntity.GroupCount)
                {
                    skuCounter[skuEntity.Id] = discountAlreadyGivenFlag;

                    return new LineItem
                    {
                        LineItemType = DiscountTypes.groupDiscount,
                        Label = skuEntity.GroupDiscountEntity.Label,
                        Sku = skuEntity.Id,
                        Total = (skuEntity.GroupDiscountEntity.GroupPrice - (skuEntity.GroupDiscountEntity.GroupCount * skuEntity.Price))
                    };
                }

            }

            return null;
        }

        public void Reset()
        {
            this.skuCounter = null;
            this.skuCounter = new Dictionary<string, decimal>();

            return;
        }


    }
}
