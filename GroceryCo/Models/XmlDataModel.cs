using GroceryCo.Entities;
using GroceryCo.Entities.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GroceryCo.Models
{
    public class XmlDataModel : IDataModel
    {
        private string fileUri;

        public XmlDataModel(string fileUri)
        {
            this.fileUri = fileUri;
        }

        public SkuEntity Find(string id)
        {
            // Parse forward to the sku element for that id
            XElement skuElement = FindSkuElement(id);
            if (skuElement == null)
                return null;

            SkuEntity skuEntity = new SkuEntity();

            // Set id
            skuEntity.Id = id;

            // Load properties
            skuEntity.Label = skuElement.Element("label").Value;
            skuEntity.Price = skuElement.Element("price").Value.SafeParse<decimal>(0);
            skuEntity.DiscountType = skuElement.Element("discounts").Attribute("selection").Value.SafeParse<DiscountTypes>(DiscountTypes.noDiscount);

            // Load the right discount entity
            LoadDiscountEntity(ref skuEntity, skuElement.Element("discounts"));

            return skuEntity;
        }

        private XElement FindSkuElement(string id)
        {
            id = id.ToUpper();

            var skuElement =
                from e in StreamSkuElements(id)
                where e.Attribute("id").Value.ToUpper() == id
                select e;

            return skuElement.FirstOrDefault();
        }

        private IEnumerable<XElement> StreamSkuElements(string id)
        {
            // Use a reader instead of an in-memory doc to parse a potentially large xml catalog file
            using (XmlReader xmlReader = XmlReader.Create(this.fileUri))
            {
                id = id.ToUpper();
                while (xmlReader.ReadToFollowing("sku"))
                {
                    XElement skuElement = XElement.ReadFrom(xmlReader) as XElement;
                    if (skuElement != null)
                    {
                        yield return skuElement;
                    }
                }
            }
        }

        private void LoadDiscountEntity(ref SkuEntity skuEntity, XElement discountsElement)
        {
            // Make sure we start with a clean slate
            skuEntity.SimpleDiscountEntity = null;
            skuEntity.GroupDiscountEntity = null;
            skuEntity.AddonDiscountEntity = null;

            string discountTypeStr = skuEntity.DiscountType.ToString();

            switch (skuEntity.DiscountType)
            {
                case DiscountTypes.noDiscount:
                    return;
                case DiscountTypes.simpleDiscount:
                    skuEntity.SimpleDiscountEntity =
                        new Entities.Discounts.SimpleDiscountEntity
                        {
                            EffectiveFromDate = discountsElement.Element(discountTypeStr).Attribute("effectiveFromDate").Value.SafeParse<DateTime>(default(DateTime)),
                            EffectiveToDate = discountsElement.Element(discountTypeStr).Attribute("effectiveToDate").Value.SafeParse<DateTime>(default(DateTime)),
                            Label = discountsElement.Element(discountTypeStr).Element("label").Value,
                            DiscountedPrice = discountsElement.Element(discountTypeStr).Element("discountedPrice").Value.SafeParse<decimal>(0),
                        };
                    return;
                case DiscountTypes.groupDiscount:
                    skuEntity.GroupDiscountEntity =
                        new Entities.Discounts.GroupDiscountEntity
                        {
                            EffectiveFromDate = discountsElement.Element(discountTypeStr).Attribute("effectiveFromDate").Value.SafeParse<DateTime>(default(DateTime)),
                            EffectiveToDate = discountsElement.Element(discountTypeStr).Attribute("effectiveToDate").Value.SafeParse<DateTime>(default(DateTime)),
                            Label = discountsElement.Element(discountTypeStr).Element("label").Value,
                            GroupCount = discountsElement.Element(discountTypeStr).Element("groupCount").Value.SafeParse<decimal>(0),
                            GroupPrice = discountsElement.Element(discountTypeStr).Element("groupPrice").Value.SafeParse<decimal>(0),
                        };
                    return;
                case DiscountTypes.addonDiscount:
                    skuEntity.AddonDiscountEntity =
                        new Entities.Discounts.AddonDiscountEntity
                        {
                            EffectiveFromDate = discountsElement.Element(discountTypeStr).Attribute("effectiveFromDate").Value.SafeParse<DateTime>(default(DateTime)),
                            EffectiveToDate = discountsElement.Element(discountTypeStr).Attribute("effectiveToDate").Value.SafeParse<DateTime>(default(DateTime)),
                            Label = discountsElement.Element(discountTypeStr).Element("label").Value,
                            BaseCount = discountsElement.Element(discountTypeStr).Element("baseCount").Value.SafeParse<decimal>(0),
                            AddonCount = discountsElement.Element(discountTypeStr).Element("addonCount").Value.SafeParse<decimal>(0),
                            AddonDiscountPercent = discountsElement.Element(discountTypeStr).Element("addonDiscountPercent").Value.SafeParse<decimal>(0),
                        };
                    return;
                default:
                    return;
            }
        }
    }
}
