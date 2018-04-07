using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCo.Entities;
using System.Xml;

namespace GroceryCo.Parsers
{
    public class XmlBasketParser : IBasketParser
    {
        private string filePath;
        private XmlReader xmlBasketReader;
        private BasketItemEntity currentBasketItem;

        public XmlBasketParser(string filePath)
        {
            this.filePath = filePath;
        }

        private void InitializeXmlReader()
        {
            this.xmlBasketReader = XmlReader.Create(this.filePath);
        }

        private bool ReadNextBasketItem()
        {
            // Initialize if first time
            if (this.xmlBasketReader == null)
            {
                InitializeXmlReader();
            }

            // Return null if we reached the end 
            if (!this.xmlBasketReader.ReadToFollowing("item"))
            {
                this.currentBasketItem = null;

                return false;
            }

            BasketItemEntity basketItem = new BasketItemEntity();

            // Set the SKU
            this.xmlBasketReader.MoveToFirstAttribute();
            basketItem.Sku = this.xmlBasketReader.Value;

            // Set the qty
            this.xmlBasketReader.MoveToNextAttribute();
            decimal qty = 0;
            decimal.TryParse(this.xmlBasketReader.Value, out qty);
            basketItem.Qty = qty;

            // Update the current basket item
            this.currentBasketItem = basketItem;

            return true;
        }

        #region IEnumerable implementation

        /// <summary>
        /// Starts automatically at the first item
        /// </summary>
        public BasketItemEntity Current
        {
            get
            {
                // Initialize if the reader is null
                if (this.currentBasketItem == null)
                {
                    ReadNextBasketItem();
                }

                return this.currentBasketItem;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public bool MoveNext()
        {
            return ReadNextBasketItem();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.xmlBasketReader.Dispose();
                }

                this.xmlBasketReader = null;
                this.currentBasketItem = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
