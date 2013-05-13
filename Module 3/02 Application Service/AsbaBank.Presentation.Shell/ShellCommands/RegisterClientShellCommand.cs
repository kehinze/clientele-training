using System;
using AsbaBank.ApplicationService;
using AsbaBank.Domain.Commands;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class RegisterClientShellCommand : IShellCommand
    {
        public string Usage { get { return String.Format("{0} <Name> <Surname> <Phone Number>", Key); } }
        public string Key  { get { return "RegisterClient"; } }

        private RegisterClient command;
       
        public void Build(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            command = new RegisterClient(args[0], args[1], args[2]);
        }

        public void Execute()
        {
            if (command == null)
            {
                throw new InvalidOperationException("The command must first be built before it can be executed.");
            }

            IClientService clientService = Environment.GetClientService();
            clientService.RegisterClient(command);
            command = null;
        }
    }
}