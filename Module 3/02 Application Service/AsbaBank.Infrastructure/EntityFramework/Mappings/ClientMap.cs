using System.ComponentModel.DataAnnotations.Schema;
using AsbaBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Infrastructure.EntityFramework.Mappings
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            HasKey(c => c.Id);

            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.Name).IsRequired();
            Property(c => c.PhoneNumber).IsRequired();
            Property(c => c.Surname).IsRequired();

            HasOptional(c => c.Address);
        }
    }
}
