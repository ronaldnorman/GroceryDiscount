using GroceryCo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Parsers
{
    public interface IBasketParser : IEnumerator<BasketItemEntity>
    {
    }
}
