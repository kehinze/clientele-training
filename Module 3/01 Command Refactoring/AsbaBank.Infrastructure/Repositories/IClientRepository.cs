using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsbaBank.Domain.Models;

namespace AsbaBank.Infrastructure.Repositories
{
    public interface IClientRepository
    {
        void RegisterClient(Client client);
    }
}
