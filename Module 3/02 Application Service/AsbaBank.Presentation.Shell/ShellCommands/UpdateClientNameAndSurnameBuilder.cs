using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.ApplicationService.Commands;
using AsbaBank.Core.Commands;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class UpdateClientNameAndSurnameBuilder : ICommandBuilder
    {
        public string Usage { get { return String.Format("{0} <clientId> <Name> <Surname>", Key); } }
        public string Key { get { return "UpdateClientNameAndSurname"; } }

        public ICommand Build(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            int clientId;

            if (!int.TryParse(args[0], out clientId))
            {
                throw new ArgumentException("Please enter a valid client Id.");
            }

            return new UpdateClientNameAndSurname(int.Parse(args[0]), args[1], args[2]);
        }
    }
}
