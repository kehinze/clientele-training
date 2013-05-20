using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure.EntityFramework.Mappings;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public enum OnContextCreationEnum
    {
        CreateIfDoesntExist = 1,
        DropAndCreate = 2
    }

    public class AbsaContext : DbContext
    {
        public AbsaContext(string databaseName, OnContextCreationEnum onContextCreationEnum)
            : base(databaseName)
        {
            OnContextCreationEnum(onContextCreationEnum);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new AddressMap());

            base.OnModelCreating(modelBuilder);
        }


        private void OnContextCreationEnum(OnContextCreationEnum onContextCreationEnum)
        {
            if (onContextCreationEnum == EntityFramework.OnContextCreationEnum.CreateIfDoesntExist)
            {
                Database.CreateIfNotExists();
            }
            if (onContextCreationEnum == EntityFramework.OnContextCreationEnum.DropAndCreate)
            {
                Database.Delete();
                Database.Create();
            }
        }
    }
}
