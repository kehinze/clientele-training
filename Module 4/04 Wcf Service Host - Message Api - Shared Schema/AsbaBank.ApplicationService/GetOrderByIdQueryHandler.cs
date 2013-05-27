using System.Linq;

using AsbaBank.Contracts.Dtos;
using AsbaBank.Contracts.Queries;
using AsbaBank.Core.Queries;
using AsbaBank.Domain.Models;

namespace AsbaBank.ApplicationService
{
    public class GetOrderByIdQueryHandler : IQueryHandler<FetchAllClients, ClientDto[]>
    {
        private readonly IQueryable<Client> clients;
        private readonly ClientDtoMapper mapper;
        
        public GetOrderByIdQueryHandler(IQueryable<Client> clients)
        {
            this.clients = clients;
            mapper = new ClientDtoMapper();
        }

        public ClientDto[] Handle(FetchAllClients query)
        {
            return clients.Select(mapper.Expression).ToArray();
        }
    }
}
