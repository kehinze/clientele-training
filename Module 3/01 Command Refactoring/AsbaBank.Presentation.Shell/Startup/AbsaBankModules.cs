using AsbaBank.Infrastructure.Implementations.InMemory;
using AsbaBank.Infrastructure.Implementations.Logger;
using AsbaBank.Infrastructure.Interfaces;
using AsbaBank.Infrastructure.Repositories;
using AsbaBank.Presentation.Shell.Factories;
using Ninject.Modules;

namespace AsbaBank.Presentation.Shell.Startup
{
    public class AbsaBankModules : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IUnitOfWork>().ToMethod(c => DataStoreFactory.GetUnitOfWork());

            Kernel.Bind<IClientRepository>().To<ClientRepository>();
            Kernel.Bind<IAccountRepository>().To<AccountRepository>();
            Kernel.Bind<ILog>().To<ConsoleWindowLogger>();
        }
    }
}
