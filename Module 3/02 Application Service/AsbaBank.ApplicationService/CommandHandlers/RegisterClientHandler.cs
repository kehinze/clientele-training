using AsbaBank.Core;
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
                var client = new Client(command.ClientName, command.ClientSurname, command.PhoneNumber);
                clientRepository.Add(client);
                unitOfWork.Commit();

                logger.Verbose("Registered client {0} {1} with Id {2}", client.Name, client.Surname, client.Id);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}