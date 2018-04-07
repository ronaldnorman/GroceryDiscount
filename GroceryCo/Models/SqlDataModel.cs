using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Models
{
    public class SqlDataModel : IDataModel
    {
        public SkuEntity Find(string id)
        {
            throw new NotImplementedException("Extension point");
        }
    }
}
