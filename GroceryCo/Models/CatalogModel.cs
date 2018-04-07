using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Models
{
    public class CatalogModel
    {
        IDataModel dataModel;
        public CatalogModel(IDataModel dataModel)
        {
            this.dataModel = dataModel;
        }

        public SkuEntity Find(string id)
        {
            return this.dataModel.Find(id);
        }


    }
}
