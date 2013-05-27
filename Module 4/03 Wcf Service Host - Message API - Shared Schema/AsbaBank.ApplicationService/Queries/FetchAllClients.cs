using System.Runtime.Serialization;
using AsbaBank.ApplicationService.Dtos;
using AsbaBank.Core.Queries;

namespace AsbaBank.ApplicationService.Queries
{
    [DataContract(Namespace = "Asba.Queries")]
    public class FetchAllClients : IQuery<ClientDto[]>
    {
    }
}
