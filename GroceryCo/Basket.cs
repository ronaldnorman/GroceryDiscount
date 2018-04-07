using GroceryCo.Entities;
using GroceryCo.Entities.Discounts;
using GroceryCo.Models;
using GroceryCo.Parsers;
using GroceryCo.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo
{
    public class Basket
    {
        private BasketItemEntity basketItem;
        private string basketFilePath;
        private CatalogModel catalogModel;

        // Results stored here line by line
        private List<LineItem> receiptLineItems;

        public Basket(string basketFilePath, CatalogModel catalogModel)
        {
            // Parser factory instantiates the right parser for the right file automatically for extensibility and flexibility
            this.basketFilePath = basketFilePath;
            this.catalogModel = catalogModel;
        }

        /// <summary>
        /// Give access to whoever needs to get access to the results line items.
        /// Of course, Checkout should be run before to load up those line items
        /// </summary>
        public List<LineItem> LineItems
        {
            get
            {
                return this.receiptLineItems;
            }
         }

        /// <summary>
        /// Main basket items processing method. It's idemopotent.
        /// </summary>
        public void Checkout()
        {
            SkuEntity skuEntity;
            IBasketParser basketParser = BasketParser.Create(basketFilePath);
            receiptLineItems = new List<Entities.LineItem>();

            // Iterate through each item in the basket
            while (basketParser.MoveNext())
            {
                basketItem = basketParser.Current;

                skuEntity = this.catalogModel.Find(basketItem.Sku);

                if (skuEntity != null)
                {
                    // Add regular line item 
                    receiptLineItems.Add(new LineItem
                    {
                        LineItemType = DiscountTypes.noDiscount,
                        Label = skuEntity.Label,
                        Price = skuEntity.Price,
                        Qty = basketItem.Qty,
                        Sku = skuEntity.Id,
                        Total = basketItem.Qty * skuEntity.Price
                    });

                    // Add discount line item (automatically determined by the appropriate discount strategy for the item)
                    IDiscountStrategy discountStrategy
                        = DiscountStrategy.Instance.Create(skuEntity.DiscountType);

                    // Create the discount line item if any
                    LineItem discountLineItem =
                        discountStrategy.GenerateDiscountLineItem(skuEntity, basketItem);

                    if (discountLineItem != null)
                    {
                        receiptLineItems.Add(discountLineItem);
                    }
                }
            }

            // Reset discount strategies to get them ready for next basket checkout
            DiscountStrategy.Instance.Reset();
        }

        public void DisplayReceipt()
        {
            Console.WriteLine();
            Console.WriteLine("==============================");
            Console.WriteLine();
            Console.WriteLine(Utils.SafeGetConfigString("BusinessName", string.Empty));
            Console.WriteLine(Utils.SafeGetConfigString("BusinessPhone", String.Empty));
            Console.WriteLine();
            Console.WriteLine(DateTime.Now.ToLongDateString());
            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.WriteLine();

            decimal runningTotal = 0;

            foreach (var lineItem in this.receiptLineItems)
            {
                // Sku
                Console.WriteLine("{0} ", lineItem.Sku);

                // Label
                Console.WriteLine("\t{0} ", lineItem.Label);

                // Qty & Price
                if (lineItem.Qty > 0 && lineItem.Price != 0)
                {
                    Console.Write("\t{0} @ {1:c} ea\t", lineItem.Qty, lineItem.Price);
                }
                else
                {
                    Console.Write("\t\t\t");
                } 

                // Line Total and align right
                Console.Write("{0,6:####.00}", lineItem.Total);

                Console.WriteLine();

                runningTotal += lineItem.Total;
            }

            Console.WriteLine();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Total\t\t\t{0,6:$####.00}", runningTotal);
            Console.WriteLine("==============================");
        }


        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine("Printing receipt... (example of future functionality placeholder)");
            Console.ReadLine();
        }

        public void Save()
        {
            Console.WriteLine();
            Console.WriteLine("Saving receipt... (example of future functionality placeholder)");
            Console.ReadLine();
        }

        public void Email()
        {
            Console.WriteLine();
            Console.WriteLine("Emailing receipt... (example of future functionality placeholder)");
            Console.ReadLine();
        }
    }
}
