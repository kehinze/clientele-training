using System;
using AsbaBank.Core.Commands;
using AsbaBank.Presentation.Shell.CommandHandlerServices;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class UpdateClientAddressBuilder: IShellCommand
    {
        public string Usage { get { return String.Format("{0} <Id> <StreetNumber> <Street> <City> <PostCode>", Key); } }
        public string Key { get { return "UpdateAddress"; } }

        public ICommand Build(string[] args)
        {
            if (args.Length != 5)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            return new UpdateClientAddress
            {
                ClientId = Int32.Parse(args[0]),
                StreetNumber = args[1],
                Street = args[2],
                City = args[3],
                PostalCode = args[4]
            };
        }
    }
}