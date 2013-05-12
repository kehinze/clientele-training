using AsbaBank.Infrastructure.Implementations.InMemory;
using AsbaBank.Infrastructure.Interfaces;
using AsbaBank.Infrastructure.Repositories;
using Ninject.Modules;

namespace AsbaBank.Presentation.Shell.Startup
{
    public class AbsaBankModules : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IUnitOfWork>().ToMethod(c => Environment.GetUnitOfWork());
            Kernel.Bind<IClientRepository>().To<ClientRepository>();
        }
    }
}
