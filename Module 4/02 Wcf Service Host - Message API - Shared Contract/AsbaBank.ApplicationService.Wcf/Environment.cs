using System.Collections.Generic;
using System.Data.Entity;

using AsbaBank.Core;
using AsbaBank.DataModel;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Infrastructure.EntityFramework;

namespace AsbaBank.ApplicationService.Wcf
{
    static class Environment
    {
        public static readonly ILog Logger;
        private static readonly IContextFactory ContextFactory;

        static Environment()
        {
            Logger = new ConsoleWindowLogger();

            Database.SetInitializer(new AsbaContextInitializer());
            ContextFactory = new ContextFactory<AsbaContext>("AsbaBank");
        }

        public static IPublishCommands GetCommandPublisher()
        {
            var commandPublisher = new CommandPublisher();
            var unitOfWork = new EntityFrameworkUnitOfWork(ContextFactory);

            commandPublisher.Subscribe(new ClientService(unitOfWork, Logger));

            return commandPublisher;
        }

        public static QueryProcessor GetQueryHandlers()
        {
            QueryProcessor handler = new QueryProcessor();
            handler.Subscribe(new GetOrderByIdQueryHandler(ContextFactory.GetContext().Set<Client>()));
            return handler;
        }
    }
}