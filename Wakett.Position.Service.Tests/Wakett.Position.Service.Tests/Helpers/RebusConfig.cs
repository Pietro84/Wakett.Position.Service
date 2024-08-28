using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Models;

namespace Wakett.Position.Service.Tests.Helpers
{
    public class RebusConfig
    {
        public static IBus ConfigureBus()
        {
            try
            {
                string connectionStringRebus = "server=SEQSQL315\\SEQSQL315;database=Rebus;user id=dbo_Test;password=F90C631D-2F8F-4770-9766-1DA4C261EC9B;integrated security=false;MultipleActiveResultSets=true;Trusted_Connection=False;TrustServerCertificate=True;";
                var activator = new BuiltinHandlerActivator();
                var rebusConfigurer = Configure.With(activator).Routing(r => r.TypeBased().Map<CryptocurrencyQuoteUpdated>("TableNameTestPie"))
                                                .Transport(t => t.UseSqlServer(new SqlServerLeaseTransportOptions(connectionStringRebus), "TableNameTestPie"))
                                                .Subscriptions(s => s.StoreInSqlServer(connectionStringRebus, "SubscriptionsTestPie", true));
                var bus = rebusConfigurer.Start();
                return bus;
            }
            catch (Exception ex)
            {

            }

            return null;

        }


    }
}
