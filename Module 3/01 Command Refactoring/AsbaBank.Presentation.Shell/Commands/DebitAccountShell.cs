using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Presentation.Shell.Commands
{
    public class DebitAccountShell : IShellCommand
    {
        public string Usage { get { return string.Format("{0} <AcccountId> <Amount>", Key); } }
        public string Key { get { return "DebitAccount"; } }

        public ICommand Build(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            decimal amount;

            if (!decimal.TryParse(args[1], out amount))
            {
                throw new ArgumentException(string.Format("The amount you have entered is not a currency. Please enter an amount in the format 0.00."));
            }

            return new DebitAccount(args[0], amount);
        }
    }
}
