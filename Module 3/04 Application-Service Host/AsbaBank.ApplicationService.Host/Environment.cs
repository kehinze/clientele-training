using AsbaBank.Core;
using AsbaBank.Infrastructure;
using AsbaBank.Infrastructure.CommandScripts;

namespace AsbaBank.ApplicationService.Host
{
    public static class Environment
    {
        private static readonly InMemoryDataStore DataStore;
        public static readonly ILog Logger;

        static Environment()
        {
            DataStore = new InMemoryDataStore();
            Logger = new ConsoleWindowLogger();
        }

        public static IPublishCommands GetCommandPublisher()
        {
            var commandPublisher = new LocalCommandPublisher();
            var unitOfWork = new InMemoryUnitOfWork(DataStore);
         
            commandPublisher.Subscribe(new ClientService(unitOfWork, Logger));

            return commandPublisher;
        }       

        public static ISerialize GetSerializer()
        {
            return new JsonSerializer();
        }
    }
}