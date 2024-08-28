using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Interfaces;
using Wakett.Position.Service.Core.Models;

namespace Wakett.Position.Service.Core.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<List<FinancialPosition>> GetPositionsByInstrumentIdAsync(string instrumentId)
        {
            return await _positionRepository.GetPositionsByInstrumentIdAsync(instrumentId);
        }

        public async Task UpdatePositionProfitLossAsync(FinancialPosition position)
        {
            await _positionRepository.UpdatePositionProfitLossAsync(position);
        }
    }
}
