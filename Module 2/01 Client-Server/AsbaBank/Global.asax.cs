using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AsbaBank.App_Start;
using AsbaBank.Infrastructure;
using AsbaBank.Models;

namespace AsbaBank
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //As some members of our class are not yet familiar with Entity framework, we use a in memory data store. 
        //This is declared static on the Mvc Applicatioon so that state is not lost between requests. 
        public static InMemoryDataStore DataStore { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitSeedData();
        }

        private void InitSeedData()
        {
            DataStore = new InMemoryDataStore();
            var unitOfWork = new InMemoryUnitOfWork(DataStore);

            unitOfWork.GetRepository<Client>()
                .Add(new Client
            {
                ClientName = "Adrian Freemantle",
                Email = "adrian@synerics.com",
                PhoneNumber = "0125551111",
                Street = "Alon road",
                City = "Sandton",
                StreetNumber = "9",
                PostalCode = "0001"
            });

            unitOfWork.GetRepository<Account>()
                .Add(new Account
            {
                AccountNumber = "1048163555",
                Balance = 1000,
                Closed = false,
                ClientId = 1
            });

            unitOfWork.GetRepository<BankCard>()
                .Add(new BankCard
            {
                AccountId = 1,
                ClientId = 1,
                Disabled = false
            });

            unitOfWork.Commit();
        }
    }
}