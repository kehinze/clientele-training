using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        //As some members of our class are not yet familiar with Entity framework, we use a in memeory repository. 
        //This is declared static on the Mvc Applicatioon so that state is not lost between requests. 
        public static InMemoryRepository Repository { get; set; }

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
            Repository = new InMemoryRepository { AutoIncrementIds = true };

            Repository.Add(new Client
            {
                ClientName = "Adrian Freemantle",
                Email = "adrian@synerics.com",
                PhoneNumber = "0125551111",
                Street = "Alon road",
                City = "Sandton",
                StreetNumber = "9",
                PostalCode = "0001"
            });

            Repository.Add(new Account
            {
                AccountNumber = "1048163555",
                Balance = 1000,
                Closed = false,
                ClientId = 1
            });

            Repository.Add(new BankCard
            {
                AccountId = 1,
                ClientId = 1,
                Disabled = false
            });

            Repository.Commit();
        }
    }
}