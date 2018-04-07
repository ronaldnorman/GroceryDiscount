using GroceryCo.Entities;
using GroceryCo.Models;
using GroceryCo.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Clear();

                    // Display UI
                    Console.WriteLine();
                    Console.WriteLine("Welcome to the {0} New Checkout System!", Utils.SafeGetConfigString("BusinessName", "GroceryCo"));
                    Console.WriteLine();
                    Console.WriteLine("Ready to checkout? Input the full absolute file path to your basket: ");
                    string fullBasketFilePath = Console.ReadLine();

                    // Validate the basket file entered by the user
                    if (!ValidateBasketFile(fullBasketFilePath))
                    {
                        continue;
                    }

                    // Inject the catalog with the desired data model
                    CatalogModel catalogModel =
                        new Models.CatalogModel(new XmlDataModel(Utils.SafeGetConfigString("XmlCatalogFilePath", String.Empty)));

                    // Main object Basket will handle the rest composed injected the file path to the basket and the desired catalog
                    Basket basket = new Basket(fullBasketFilePath, catalogModel);

                    // Checkout is the main method on basket that processes all the basket items and applies all relevant discounts per the catalog definitions storing all in basket for later access.
                    basket.Checkout();

                    // Display the checkout receipt
                    basket.DisplayReceipt();

                    // Exit the program if menu selection returned false
                    if (!ShowMenu(basket))
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }

        private static bool ShowMenu(Basket basket)
        {
            Console.WriteLine();
            Console.WriteLine("(N)ew Checkout, (Q)uit, (P)rint, (E)mail, (S)ave?");
            Console.WriteLine();

            while (true)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.N:
                        return true;
                    case ConsoleKey.Q:
                        Console.WriteLine();
                        Console.WriteLine("Quitting...");
                        return false;
                    case ConsoleKey.P:
                        basket.Print();
                        return true;
                    case ConsoleKey.E:
                        basket.Email();
                        return true;
                    case ConsoleKey.S:
                        basket.Save();
                        return true;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Key pressed is not a menu option. Please press any of the letters in parenthesis.");
                        break;

                }
            }

            return true;

        }

        private static bool ValidateBasketFile(string fullBasketFilePath)
        {
            // Perform various file checks and validations, add more checks as needed in the future
            if (!System.IO.File.Exists(fullBasketFilePath))
            {
                Console.WriteLine("File not found. Hit Enter and try again.");
                Console.ReadLine();
                return false;
            }
            else if (!System.IO.Path.HasExtension(fullBasketFilePath))
            {
                Console.WriteLine("File name must have an extension. Hit Enter and try again");
                Console.ReadLine();
                return false;
            }


            // Check for valid extensions
            string ext = System.IO.Path.GetExtension(fullBasketFilePath);
            if (String.IsNullOrEmpty(ext) || ext.ToLower() != ".xml")
            {
                Console.WriteLine("Invalid file format. Only xml files are accepted. Hit Enter and try again.");
                Console.ReadLine();
                return false;
            }

            return true;
        }
    }
}
