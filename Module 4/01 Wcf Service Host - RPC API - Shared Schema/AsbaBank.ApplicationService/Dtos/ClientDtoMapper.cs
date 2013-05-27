using System;
using System.Linq.Expressions;
using AsbaBank.Core;
using AsbaBank.Domain.Models;

namespace AsbaBank.ApplicationService.Dtos
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