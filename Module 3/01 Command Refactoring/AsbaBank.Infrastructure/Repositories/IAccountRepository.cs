using AsbaBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsbaBank.Infrastructure.Repositories
{
    public interface IAccountRepository
    {
        Account GetAccount(string accountNumber);
    }
}
