﻿using System;
using AsbaBank.Presentation.Shell.ClientServices;

namespace AsbaBank.Presentation.Shell.ShellCommands
{
    public class UpdateClientAddressBuilder: IShellCommand
    {
        public string Usage { get { return String.Format("{0} <Id> <StreetNumber> <Street> <City> <PostCode>", Key); } }
        public string Key { get { return "UpdateAddress"; } }

        public void Execute(string[] args)
        {
            if (args.Length != 5)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            using (var clientService = new ClientServiceClient())
            {
                clientService.UpdateClientAddress(Int32.Parse(args[0]), args[1], args[2], args[3], args[4]);
            }  
        }
    }
}