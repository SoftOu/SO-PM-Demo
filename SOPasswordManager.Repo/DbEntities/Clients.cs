using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class Clients
    {
        public Clients()
        {
            ClientContacts = new HashSet<ClientContacts>();
        }

        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }

        public City City { get; set; }
        public County Country { get; set; }
        public ICollection<ClientContacts> ClientContacts { get; set; }
    }
}
