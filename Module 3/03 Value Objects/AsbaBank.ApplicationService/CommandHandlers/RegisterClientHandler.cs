using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Domain.Commands;
using AsbaBank.Domain.Models;

namespace AsbaBank.ApplicationService.CommandHandlers
{
    public class RegisterClientHandler : IHandleCommand<RegisterClient>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog logger;

        public RegisterClientHandler(IUnitOfWork unitOfWork, ILog logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public void Execute(RegisterClient command)
        {
            var clientRepository = unitOfWork.GetRepository<Client>();

            try
            {
                var client = new Client(command.ClientName, command.PhoneNumber);
                clientRepository.Add(client);
                unitOfWork.Commit();

                logger.Verbose("Registered client {0}", client);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}