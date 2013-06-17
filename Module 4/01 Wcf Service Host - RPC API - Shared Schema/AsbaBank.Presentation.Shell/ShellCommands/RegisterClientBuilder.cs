using System;
using AsbaBank.Presentation.Shell.ClientServices;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class RegisterClientBuilder : IShellCommand
    {
        public string Usage { get { return String.Format("{0} <Name> <Surname> <Phone Number>", Key); } }
        public string Key  { get { return "RegisterClient"; } }

        public void Execute(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            using (var clientService = new ClientServiceClient())
            {
                clientService.RegisterClient(args[0], args[1], args[2]);
            }    
        }
    }
}