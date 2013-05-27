using System.Runtime.Serialization;

using AsbaBank.Contracts.Dtos;
using AsbaBank.Core.Queries;

namespace AsbaBank.Contracts.Queries
{
    [DataContract(Namespace = "Asba.Queries")]
    public class FetchAllClients : IQuery<ClientDto[]>
    {
    }
}
