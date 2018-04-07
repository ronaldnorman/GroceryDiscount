using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Strategies
{
    public class AddonDiscountStrategy : IDiscountStrategy
    {
        Dictionary<string, decimal> skuCounter;

        public AddonDiscountStrategy()
        {
            this.Reset();
        }

        public LineItem GenerateDiscountLineItem(SkuEntity skuEntity, BasketItemEntity basketItem)
        {
            if (Utils.IsCurrentDateTimeWithinRange(skuEntity.AddonDiscountEntity.EffectiveFromDate, skuEntity.AddonDiscountEntity.EffectiveToDate))
            {
                decimal lastRemainderQtyNotGivenDiscount = 0;

                // Keep a tally on the remainder qty that a discount wasn't applied to because it wasn't sufficient
                if (skuCounter.ContainsKey(skuEntity.Id))
                {
                    lastRemainderQtyNotGivenDiscount = skuCounter[skuEntity.Id];
                }
                else
                {
                    skuCounter.Add(skuEntity.Id, 0);
                }

                // Add the total qty of the minimum to defined to qualify for a discount
                decimal totalAddonAndBase = skuEntity.AddonDiscountEntity.AddonCount + skuEntity.AddonDiscountEntity.BaseCount;

                // Calculate the ratio to apply the discount to
                decimal discountQtyApplyRatio =
                    (skuEntity.AddonDiscountEntity.AddonCount / totalAddonAndBase);

                // Calculate the qty to apply the addon discount to
                decimal qualifyingQtyForDiscount = basketItem.Qty + lastRemainderQtyNotGivenDiscount;

                // Now apply the discount ratio to the qualified quantity
                decimal addonQtyToBeDiscounted = 0;
                decimal regularPrice = 0;
                decimal discountAmount = 0;
                decimal remainderNonQualified = 0;

                if (qualifyingQtyForDiscount >= totalAddonAndBase)
                {
                    // Calculate the discount amount
                    regularPrice = qualifyingQtyForDiscount * skuEntity.Price;
                    addonQtyToBeDiscounted = discountQtyApplyRatio * qualifyingQtyForDiscount;
                    discountAmount = -(addonQtyToBeDiscounted * skuEntity.Price * skuEntity.AddonDiscountEntity.AddonDiscountPercent / 100);

                    // Remainder quantity not given discount so far so we count it on the next call
                    remainderNonQualified = qualifyingQtyForDiscount - addonQtyToBeDiscounted;

                    return new LineItem
                    {
                        LineItemType = DiscountTypes.addonDiscount,
                        Label = skuEntity.AddonDiscountEntity.Label,
                        Sku = skuEntity.Id,
                        Total = discountAmount
                    };
                }
                else
                {
                    remainderNonQualified = qualifyingQtyForDiscount;
                }

                // Store the remainder that could not qualify for next time to accumulate and hopefully qualify again
                skuCounter[skuEntity.Id] = remainderNonQualified;

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
