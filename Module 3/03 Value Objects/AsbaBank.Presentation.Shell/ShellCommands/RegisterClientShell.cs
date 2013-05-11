using System;

using AsbaBank.Core.Commands;
using AsbaBank.Domain.Commands;
using AsbaBank.Domain.ValueTypes;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class RegisterClientShell : IShellCommand
    {
        public string Usage { get { return String.Format("{0} <Name> <Surname> <Phone Number>", Key); } }
        public string Key  { get { return "RegisterClient"; } }
       
        public ICommand Build(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            var clientName = new PersonName(args[0], args[1]);
            var phoneNumber = new TelephoneNumber(args[2]);
            
            return new RegisterClient(clientName, phoneNumber);
        }
    }
}