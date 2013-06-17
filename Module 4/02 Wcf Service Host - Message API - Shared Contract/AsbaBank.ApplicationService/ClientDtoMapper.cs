using System;
using System.Linq.Expressions;

using AsbaBank.Contracts.Dtos;
using AsbaBank.Core;
using AsbaBank.Domain.Models;

namespace AsbaBank.ApplicationService
{
    public class ClientDtoMapper : Mapper<Client, ClientDto>
    {
        public override Expression<Func<Client, ClientDto>> Expression
        {
            get
            {
                return client => new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Surname = client.Surname,
                    PhoneNumber = client.PhoneNumber,                    
                };
            }
        }
    }
}