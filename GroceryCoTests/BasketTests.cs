using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryCo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCo.Models;

namespace GroceryCo.Tests
{
    [TestClass()]
    public class BasketTests
    {
        [TestMethod()]
        public void Checkout_BasicTest()
        {
            // Basket xml file for basict test
            string fullBasketFilePath = @"Data\Basket.xml";

            // Inject the catalog with the desired data model
            CatalogModel catalogModel =
                new Models.CatalogModel(new XmlDataModel(Utils.SafeGetConfigString("XmlCatalogFilePath", String.Empty)));

            // Main object Basket will handle the rest composed injected the file path to the basket and the desired catalog
            Basket basket = new Basket(fullBasketFilePath, catalogModel);

            // Checkout is the main method on basket that processes all the basket items and applies all relevant discounts per the catalog definitions storing all in basket for later access.
            basket.Checkout();

            // We should be getting lots of line items
            Assert.AreNotEqual(basket.LineItems.Count, 0);
        }

        /// <summary>
        /// This test should have one simple discount line item
        /// </summary>
        [TestMethod()]
        public void Checkout_SimpleDiscountCountTest()
        {
            // Basket xml file for basict test
            string fullBasketFilePath = @"Data\Basket2.xml";

            // Inject the catalog with the desired data model
            CatalogModel catalogModel =
                new Models.CatalogModel(new XmlDataModel(Utils.SafeGetConfigString("XmlCatalogFilePath", String.Empty)));

            // Main object Basket will handle the rest composed injected the file path to the basket and the desired catalog
            Basket basket = new Basket(fullBasketFilePath, catalogModel);

            // Checkout is the main method on basket that processes all the basket items and applies all relevant discounts per the catalog definitions storing all in basket for later access.
            basket.Checkout();

            int simpleDiscountLineItemsCount = 
                basket.LineItems.Where(m => m.LineItemType == Entities.DiscountTypes.simpleDiscount).Count();

            // We should have ONE simpleDiscount line item
            Assert.AreEqual(simpleDiscountLineItemsCount, 1);
        }

        /// <summary>
        /// This test should have one group discount line item
        /// </summary>
        [TestMethod()]
        public void Checkout_GroupDiscountCountTest()
        {
            // Basket xml file for basict test
            string fullBasketFilePath = @"Data\Basket2.xml";

            // Inject the catalog with the desired data model
            CatalogModel catalogModel =
                new Models.CatalogModel(new XmlDataModel(Utils.SafeGetConfigString("XmlCatalogFilePath", String.Empty)));

            // Main object Basket will handle the rest composed injected the file path to the basket and the desired catalog
            Basket basket = new Basket(fullBasketFilePath, catalogModel);

            // Checkout is the main method on basket that processes all the basket items and applies all relevant discounts per the catalog definitions storing all in basket for later access.
            basket.Checkout();

            int groupDiscountLineItemsCount =
                basket.LineItems.Where(m => m.LineItemType == Entities.DiscountTypes.groupDiscount).Count();

            // We should have ONE groupDiscount line item
            Assert.AreEqual(groupDiscountLineItemsCount, 1);
        }

        /// <summary>
        /// This test should have one addon discount line item
        /// </summary>
        [TestMethod()]
        public void Checkout_AddonDiscountCountTest()
        {
            // Basket xml file for basict test
            string fullBasketFilePath = @"Data\Basket2.xml";

            // Inject the catalog with the desired data model
            CatalogModel catalogModel =
                new Models.CatalogModel(new XmlDataModel(Utils.SafeGetConfigString("XmlCatalogFilePath", String.Empty)));

            // Main object Basket will handle the rest composed injected the file path to the basket and the desired catalog
            Basket basket = new Basket(fullBasketFilePath, catalogModel);

            // Checkout is the main method on basket that processes all the basket items and applies all relevant discounts per the catalog definitions storing all in basket for later access.
            basket.Checkout();

            int addonDiscountLineItemsCount =
                basket.LineItems.Where(m => m.LineItemType == Entities.DiscountTypes.groupDiscount).Count();

            // We should have ONE groupDiscount line item
            Assert.AreEqual(addonDiscountLineItemsCount, 1);
        }

        /// <summary>
        /// This tests idempotency and multiple calling to the Checkout method
        /// It should produce the same total
        /// </summary>
        [TestMethod()]
        public void Checkout_IdempotencyTest()
        {
            // Basket xml file for basict test
            string fullBasketFilePath = @"Data\Basket2.xml";

            // Inject the catalog with the desired data model
            CatalogModel catalogModel =
                new Models.CatalogModel(new XmlDataModel(Utils.SafeGetConfigString("XmlCatalogFilePath", String.Empty)));

            // Main object Basket will handle the rest composed injected the file path to the basket and the desired catalog
            Basket basket = new Basket(fullBasketFilePath, catalogModel);

            // First call to Checkout, store the total
            basket.Checkout();
            decimal firstCallTotal =
                basket.LineItems.Sum(m => m.Total);

            // Second call to Checkout, store the total
            basket.Checkout();
            decimal secondCallTotal =
                basket.LineItems.Sum(m => m.Total);


            // We should have equal totals for the two runs without any changes
            Assert.AreEqual(firstCallTotal, secondCallTotal);
        }
    }
}