using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Domain.Models;

namespace AsbaBank.Infrastructure.EntityFramework.Mappings
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            HasKey(c => c.Id);

            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.City);
            Property(c => c.PostalCode).HasMaxLength(20);
            Property(c => c.Street).HasMaxLength(100);
            Property(c => c.StreetNumber).HasMaxLength(100);
        }
    }
}
