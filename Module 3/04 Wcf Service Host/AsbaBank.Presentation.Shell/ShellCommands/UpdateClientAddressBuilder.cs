using System;

using AsbaBank.Core.Commands;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class UpdateClientAddress : ICommand
    {
        public int ClientId { get; private set; }
        public string StreetNumber { get; private set; }
        public string Street { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }

        public UpdateClientAddress(int clientId, string streetNumber, string street, string city, string postalCode)
        {
            ClientId = clientId;
            StreetNumber = streetNumber;
            Street = street;
            PostalCode = postalCode;
            City = city;
        }
    }

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

            return new UpdateClientAddress(Int32.Parse(args[0]), args[1], args[2], args[3], args[4]);
        }
    }
}