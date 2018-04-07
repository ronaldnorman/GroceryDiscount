using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Parsers
{
    // Basket parser factory. It creates the right parser for the file extension
    public static class BasketParser
    {
        public static IBasketParser Create(string fullFilePath)
        {
            string ext = System.IO.Path.GetExtension(fullFilePath);

            switch (ext.ToLower())
            {
                case ".xml":
                    return new XmlBasketParser(fullFilePath);
                case ".csv":
                    return new CsvBasketParser(fullFilePath);
                default:
                    throw new NotSupportedException("File format not supported.");
            }
                
        }
    }
}
