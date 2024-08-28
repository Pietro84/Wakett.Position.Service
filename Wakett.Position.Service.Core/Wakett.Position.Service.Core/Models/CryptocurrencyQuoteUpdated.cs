using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wakett.Position.Service.Core.Models
{
    public class CryptocurrencyQuoteUpdated
    {
        public string Symbol { get; set; }
        public decimal NewPrice { get; set; }
    }
}
