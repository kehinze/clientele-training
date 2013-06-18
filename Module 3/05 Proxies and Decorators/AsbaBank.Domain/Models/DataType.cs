using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Domain.Models
{
    public enum DataTypeEnum
    {
        Unknown = 0,
        Binary = 1,
        Json = 2,
        Xml = 3
    }

    public class DataType
    {
        public DataType(DataTypeEnum dataType)
        {
            Id = (int) dataType;
            Description = dataType.ToString();
        }

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
    }
}
