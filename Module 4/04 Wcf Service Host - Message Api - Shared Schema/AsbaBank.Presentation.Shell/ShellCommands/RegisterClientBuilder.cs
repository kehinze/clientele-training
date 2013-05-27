using System;
using AsbaBank.Core.Commands;
using AsbaBank.Presentation.Shell.CommandHandlerServices;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class RegisterClientBuilder : IShellCommand
    {
        public string Usage { get { return String.Format("{0} <Name> <Surname> <Phone Number>", Key); } }
        public string Key  { get { return "RegisterClient"; } }

        public ICommand Build(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            return new RegisterClient
            {
                ClientName = args[0],
                ClientSurname = args[1],
                PhoneNumber = args[2]
            };
        }
    }
}