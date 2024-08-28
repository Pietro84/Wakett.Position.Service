using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Models;

namespace Wakett.Position.Service.Core.Interfaces
{
    public interface IProfitLossCalculator
    {
        decimal CalculateProfitLoss(FinancialPosition position, decimal currentRate);
    }
}
