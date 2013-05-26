using System;

using AsbaBank.Core.Commands;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class RegisterClient : ICommand
    {
        public string ClientName { get; private set; }
        public string ClientSurname { get; private set; }
        public string PhoneNumber { get; private set; }

        public RegisterClient(string clientName, string clientSurname, string phoneNumber)
        {
            ClientName = clientName;
            ClientSurname = clientSurname;
            PhoneNumber = phoneNumber;
        }
    }

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

            return new RegisterClient(args[0], args[1], args[2]);
        }
    }
}