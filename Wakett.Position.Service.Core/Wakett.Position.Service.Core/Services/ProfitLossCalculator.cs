using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Interfaces;
using Wakett.Position.Service.Core.Models;

namespace Wakett.Position.Service.Core.Services
{
    public class ProfitLossCalculator : IProfitLossCalculator
    {
        public decimal CalculateProfitLoss(FinancialPosition position, decimal currentRate)
        {
            return position.Quantity * (currentRate - position.InitialRate) * position.Side;
        }
    }
}
