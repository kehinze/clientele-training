using AsbaBank.Core;
using AsbaBank.Core.Commands;
using AsbaBank.Domain.ValueTypes;

namespace AsbaBank.Domain.Commands
{  
    public class RegisterClient : ICommand
    {
        public PersonName ClientName { get; private set; }
        public TelephoneNumber PhoneNumber { get; private set; }

        public RegisterClient(PersonName clientName, TelephoneNumber phoneNumber)
        {
            Mandate.ParameterNotDefaut(clientName, "clientName");
            Mandate.ParameterNotDefaut(phoneNumber, "phoneNumber");

            ClientName = clientName;
            PhoneNumber = phoneNumber;
        }
    }
}
