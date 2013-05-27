using System;
using AsbaBank.Contracts.Dtos;
using AsbaBank.Contracts.Queries;
using AsbaBank.Core.Queries;

namespace AsbaBank.Presentation.Shell
{
    public class QueryExampleController
    {
        private readonly IQueryProcessor queryProcessor;

        public QueryExampleController(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        public void ShowAllClients()
        {
            ClientDto[] clients = queryProcessor.Handle(new FetchAllClients());

            foreach (var clientDto in clients)
            {
                Console.WriteLine("{0}, {1}", clientDto.PhoneNumber, clientDto.Surname);
            }
        }
    }
}
