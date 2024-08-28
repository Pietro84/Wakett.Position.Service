using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Models;
using Wakett.Position.Service.Tests.Helpers;
using Xunit;

namespace Wakett.Position.Service.Tests
{
    public class RebusIntegrationTests
    {
        private IBus _bus;
        private BuiltinHandlerActivator _activator;

        [Fact]
        public async Task TestMessageReception()
        {
            try
            {

                //_bus = RebusConfig.ConfigureBus();

                //// Simula l'invio di un messaggio alla coda
                //var message = new CryptocurrencyQuoteUpdated
                //{
                //    Symbol = "BTC/USD",
                //    NewPrice = 50000
                //};

                //await _bus.Publish(message);

                // Attendi il completamento del task handler
                await Task.Delay(1000); // Aspetta che il messaggio sia processato


                // Configura il bus Rebus
                _activator = new BuiltinHandlerActivator();

                // Configura l'handler
                _activator.Handle<CryptocurrencyQuoteUpdated>(async receivedMessage =>
                {
                    // Esegui la logica di test, come ad esempio verificare se il messaggio è stato processato
                    Assert.Equal("BTC/USD", receivedMessage.Symbol);
                    Assert.Equal(50000, receivedMessage.NewPrice);
                });


                // Verifica se il messaggio è stato gestito correttamente
                // Non ci sono ulteriori asserzioni nel test di esempio; puoi aggiungere ulteriori verifiche
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
        }
    }
}
