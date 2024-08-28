using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wakett.Position.Service.Core.Models
{
    public class FinancialPosition
    {
        public int Id { get; set; }
        public string InstrumentId { get; set; }
        public decimal Quantity { get; set; }
        public decimal InitialRate { get; set; }
        public int Side { get; set; } // +1 for Buy, -1 for Sell
        public decimal ProfitLoss { get; set; }
    }
}
