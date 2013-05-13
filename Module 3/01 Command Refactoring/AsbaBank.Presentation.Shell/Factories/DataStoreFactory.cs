using AsbaBank.Infrastructure.Implementations.InMemory;
using AsbaBank.Infrastructure.Interfaces;

namespace AsbaBank.Presentation.Shell.Factories
{
    public class DataStoreFactory
    {
        private static readonly InMemoryDataStore DataStore;
     
        static DataStoreFactory()
        {
            DataStore = new InMemoryDataStore();
        }

        public static IUnitOfWork GetUnitOfWork()
        {
            return new InMemoryUnitOfWork(DataStore);
        }
    }
}
