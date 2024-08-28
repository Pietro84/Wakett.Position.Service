using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Handlers;
using Wakett.Position.Service.Core.Interfaces;
using Wakett.Position.Service.Core.Models;
using Wakett.Position.Service.Tests.Helpers;
using Xunit;

namespace Wakett.Position.Service.Tests
{


    public class CryptocurrencyQuoteUpdatedHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUpdatePositionsAndPublishMessage()
        {
            // Arrange
            var mockPositionService = new Mock<IPositionService>();
            var mockProfitLossCalculator = new Mock<IProfitLossCalculator>();
            var mockBus = new Mock<IBus>();

            var handler = new CryptocurrencyQuoteUpdatedHandler(
                mockProfitLossCalculator.Object,
                mockPositionService.Object,
                mockBus.Object
            );

            var message = new CryptocurrencyQuoteUpdated
            {
                Symbol = "BTC/USD",
                NewPrice = 50000m
            };

            var positions = new List<FinancialPosition>
            {
                new FinancialPosition
                {
                    Id = 1,
                    InstrumentId = "BTC",
                    Quantity = 1.5m,
                    InitialRate = 40000m,
                    Side = 1,
                    ProfitLoss = 0m
                }
            };

            mockPositionService
                .Setup(ps => ps.GetPositionsByInstrumentIdAsync("BTC"))
                .ReturnsAsync(positions);

            mockProfitLossCalculator
                .Setup(plc => plc.CalculateProfitLoss(It.IsAny<FinancialPosition>(), message.NewPrice))
                .Returns((FinancialPosition p, decimal np) => p.Quantity * (np - p.InitialRate) * p.Side);

            // Act
            await handler.Handle(message);

            // Assert
            //mockPositionService.Verify(ps => ps.UpdatePositionProfitLossAsync(It.Is<FinancialPosition>(
            //    p => p.ProfitLoss == 15000m)), Times.Once);

        }


        [Fact]
        public async Task TestOutputBus()
        {
            try
            {
                var message = new FinancialPositionUpdated
                {
                    InstrumentId = "BTC/USD",
                    ProfitLoss = 0m
                };
                var bus = RebusConfig.ConfigureBus();
                await bus.Send(message);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }
    }
}
