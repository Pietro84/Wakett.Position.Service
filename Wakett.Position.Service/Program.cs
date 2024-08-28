using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Wakett.Position.Service.Core.Handlers;
using Wakett.Position.Service.Core.Interfaces;
using Wakett.Position.Service.Core.Models;
using Wakett.Position.Service.Core.Services;
using Wakett.Position.Service.Infrastructure.Repositories;

namespace Wakett.Position.Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var bus = serviceProvider.GetRequiredService<IBus>(); //avvia il bus
            bus.Subscribe<CryptocurrencyQuoteUpdated>().Wait();   //simula l'ascolto continuo

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
            BusConfiguration(services);
            // Registrazione delle dipendenze
            services.AddSingleton<IPositionRepository>(new PositionRepository(connectionString));
            services.AddSingleton<IProfitLossCalculator, ProfitLossCalculator>();
            services.AddSingleton<IPositionService, PositionService>();
            services.AddSingleton<Service1>();
        }

        private static void BusConfiguration(IServiceCollection services)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["RebusConnectionString"].ConnectionString;
            string connectionStringRebus = "server=SEQSQL315\\SEQSQL315;database=Rebus;user id=dbo_Test;password=F90C631D-2F8F-4770-9766-1DA4C261EC9B;integrated security=false;MultipleActiveResultSets=true;Trusted_Connection=False;TrustServerCertificate=True;";

            // Configura Rebus
            services.AddRebus(configure => configure
                .Routing(r => r.TypeBased().Map<CryptocurrencyQuoteUpdated>("TableNameTestPie"))
                .Transport(t => t.UseSqlServer(new SqlServerLeaseTransportOptions(connectionStringRebus), "TableNameTestPie"))
                .Subscriptions(s => s.StoreInSqlServer(connectionStringRebus, "SubscriptionsTestPie", true)),true);

            services.AddRebusHandler<CryptocurrencyQuoteUpdatedHandler>();

            services.AddRebus(configure => configure
               .Routing(r => r.TypeBased().Map<FinancialPositionUpdated>("TableNameOutput"))
               .Options(o => o.SetBusName("default"))
               .Transport(t => t.UseSqlServer(new SqlServerLeaseTransportOptions(connectionStringRebus), "TableNameOutput"))
               .Subscriptions(s => s.StoreInSqlServer(connectionStringRebus, "SubscriptionsTestOutput", true)),false);

        }
    }
}
