using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Core.Commands
{
    public class CommandRequestAttribute : Attribute
    {
        public string DataType { get; set; }
    }
}
