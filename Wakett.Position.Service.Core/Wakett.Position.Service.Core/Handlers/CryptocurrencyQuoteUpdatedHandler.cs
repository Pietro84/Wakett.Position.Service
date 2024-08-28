using Rebus.Bus;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Interfaces;
using Wakett.Position.Service.Core.Models;
using Wakett.Position.Service.Core.Services;

namespace Wakett.Position.Service.Core.Handlers
{
    public class CryptocurrencyQuoteUpdatedHandler : IHandleMessages<CryptocurrencyQuoteUpdated>
    {
        private readonly IProfitLossCalculator _profitLossCalculator;
        private readonly IPositionService _positionService;
        private readonly IBus _bus;

        public CryptocurrencyQuoteUpdatedHandler(IProfitLossCalculator profitLossCalculator, IPositionService positionService, IBus bus)
        {
            _profitLossCalculator = profitLossCalculator;
            _positionService = positionService;
            _bus = bus;
        }

        public async Task Handle(CryptocurrencyQuoteUpdated message)
        {

            // Recupera le posizioni finanziarie dal database
            var positions = await _positionService.GetPositionsByInstrumentIdAsync(message.Symbol);
            if(positions != null && positions.Count > 0)
            {
                foreach (var position in positions)
                {
                    // Calcola il nuovo profit/loss
                    position.ProfitLoss = _profitLossCalculator.CalculateProfitLoss(position, message.NewPrice);

                    // Aggiorna la posizione nel database
                    await _positionService.UpdatePositionProfitLossAsync(position);

                    // Pubblica un nuovo messaggio con la posizione finanziaria aggiornata
                    var updatedPositionMessage = new FinancialPositionUpdated
                    {
                        InstrumentId = position.InstrumentId,
                        Quantity = position.Quantity,
                        InitialRate = position.InitialRate,
                        Side = position.Side,
                        ProfitLoss = position.ProfitLoss
                    };

                    await _bus.Publish(updatedPositionMessage);

                }
            }
            
        }
    }

}
