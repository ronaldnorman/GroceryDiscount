using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Strategies
{
    public sealed class DiscountStrategy
    {
        Dictionary<DiscountTypes, IDiscountStrategy> discountStrategies = 
            new Dictionary<DiscountTypes, IDiscountStrategy>();

        // We're using a Singleton because we want to maintain state between calls. Specifically, this is for the Group and Addon discount strategies as they need to keep count of items to provide the discount
        #region Singleton
        private static readonly DiscountStrategy instance = new DiscountStrategy();

        private DiscountStrategy()
        {
            // Add all strategies in a dictionary for easy composite operations (reset)
            discountStrategies.Add(DiscountTypes.noDiscount, new DummyDiscountStrategy());
            discountStrategies.Add(DiscountTypes.simpleDiscount, new SimpleDiscountStrategy());
            discountStrategies.Add(DiscountTypes.groupDiscount, new GroupDiscountStrategy());
            discountStrategies.Add(DiscountTypes.addonDiscount, new AddonDiscountStrategy());
        }

        public static DiscountStrategy Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public IDiscountStrategy Create(DiscountTypes discountType)
        {
            // First determine the discount strategy
            switch (discountType)
            {
                case DiscountTypes.noDiscount:
                    return this.discountStrategies[DiscountTypes.noDiscount];
                case DiscountTypes.simpleDiscount:
                    return this.discountStrategies[DiscountTypes.simpleDiscount];
                case DiscountTypes.groupDiscount:
                    return this.discountStrategies[DiscountTypes.groupDiscount];
                case DiscountTypes.addonDiscount:
                    return this.discountStrategies[DiscountTypes.addonDiscount];
                default:
                    return this.discountStrategies[DiscountTypes.noDiscount];
            }
        }

        public void Reset()
        {
            // Make sure to reset each strategy
            foreach (var strategy in discountStrategies.Values)
            {
                strategy.Reset();
            }
        }
    }
}
