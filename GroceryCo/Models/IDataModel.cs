using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Models
{
    public interface IDataModel
    {
        SkuEntity Find(string id);
    }
}
