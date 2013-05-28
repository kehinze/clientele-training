using System.Collections.Generic;
using System.Linq;
using AsbaBank.Core.Persistence;
using AsbaBank.Domain.Models;

namespace AsbaBank.DataModel
{   
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ClientQueries
    {
        private readonly IEntityQuery entityQuery;
        private readonly ISqlQuery sqlQuery;

        public ClientQueries(IEntityQuery entityQuery, ISqlQuery sqlQuery)
        {
            this.entityQuery = entityQuery;
            this.sqlQuery = sqlQuery;
        }

        public ICollection<Client> All()
        {
            var query = entityQuery.Query<Client>();

            return query.ToList();
        }        

        public ICollection<Client> WithPhoneNumberLike(string phoneNumber)
        {
            var query = entityQuery.Query<Client>()
                                   .Where(c => c.PhoneNumber.Contains(phoneNumber));
           
            return query.ToList();
        }

        public ICollection<ClientDto> WithSurname(string surname)
        {
            var query = entityQuery.Query<Client>()
                                   .Where(c => c.Surname.Equals(surname))
                                   .Select(c => new ClientDto
                                   {
                                       Name = c.Name,
                                       Surname = c.Surname,
                                       PhoneNumber = c.PhoneNumber
                                   });

            return query.ToList();
        }

        public ICollection<ClientDto> WithName(string name)
        {
            var query = sqlQuery.SqlQuery<ClientDto>("SELECT Id, Name, Surname, PhoneNumber FROM Client WHERE Name = {0}", name);

            return query.ToList();
        }

        public ICollection<ClientDto> WithPhoneNumber(string phoneNumber)
        {
            var clients = entityQuery.Query<Client>();

            var query = from c in clients
                        where c.PhoneNumber.Equals(phoneNumber)
                        select new ClientDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Surname = c.Surname,
                            PhoneNumber = c.PhoneNumber
                        };

            return query.ToList();
        }
    }
}
