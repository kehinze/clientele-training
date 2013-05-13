using System;
using AsbaBank.ApplicationService;
using AsbaBank.Domain.Commands;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class UpdateClientAddressShellCommand : IShellCommand
    {
        public string Usage { get { return String.Format("{0} <Id> <StreetNumber> <Street> <City> <PostCode>", Key); } }
        public string Key { get { return "UpdateAddress"; } }

        private UpdateClientAddress command;

        public void Build(string[] args)
        {
            if (args.Length != 5)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            command = new UpdateClientAddress(Int32.Parse(args[0]), args[1], args[2], args[3], args[4]);
        }

        public void Execute()
        {
            if (command == null)
            {
                throw new InvalidOperationException("The command must first be built before it can be executed.");
            }

            IClientService clientService = Environment.GetClientService();
            clientService.UpdateClientAddress(command);
            command = null;
        }
    }
}