using System;
using System.Runtime.Serialization;
using AsbaBank.Core.Queries;

namespace AsbaBank.Presentation.Shell
{
    [DataContract(Namespace = "Asba.Queries")]
    public class FetchAllClients : IQuery<ClientDto[]>
    {
    }

    public class ClientDto
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string PhoneNumber { get; set; }
    }

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
