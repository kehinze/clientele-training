using System;
using AsbaBank.Core;

namespace AsbaBank.Domain.Commands
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
    }
}
