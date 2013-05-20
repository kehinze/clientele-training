using System;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Presentation.Shell.Startup;
using Ninject;
using AsbaBank.Infrastructure.Repositories;
using AsbaBank.Infrastructure.Interfaces;

namespace AsbaBank.Presentation.Shell.Commands
{
    public class RegisterClient : ICommand
    {
        public string ClientName { get; private set; }
        public string ClientSurname { get; private set; }
        public string PhoneNumber { get; private set; }

        public RegisterClient(string clientName, string clientSurname, string phoneNumber)
        {
            if (String.IsNullOrEmpty(clientName) || clientName.Length < 3)
            {
                throw new ArgumentException("Please provide a valid client name of at least three characters.");
            }

            if (String.IsNullOrEmpty(clientSurname) || clientSurname.Length < 3)
            {
                throw new ArgumentException("Please provide a valid client name of at least three characters.");
            }

            if (String.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.IsDigitsOnly())
            {
                throw new ArgumentException("Please provide a valid phone number that is 10 digits long.");
            }

            ClientName = clientName;
            ClientSurname = clientSurname;
            PhoneNumber = phoneNumber;
        }
        
        public void Execute()
        {
<<<<<<< HEAD
            using (var kernel = new StandardKernel(new AbsaBankModules()))
=======
            var unitOfWork = Environment.GetUnitOfWork(); 
            var clientRepository = unitOfWork.GetRepository<Client>();

            try
>>>>>>> upstream/master
            {
                var unitOfWork = kernel.Get<IUnitOfWork>();
                var clientRepository = kernel.Get<IClientRepository>();
                var logger = kernel.Get<ILog>();
                try
                {
                    var client = new Client(ClientName, ClientSurname, PhoneNumber);

                    clientRepository.RegisterClient(client);
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
}
