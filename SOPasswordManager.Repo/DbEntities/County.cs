using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class County
    {
        public County()
        {
            City = new HashSet<City>();
            Clients = new HashSet<Clients>();
        }

        public int CountryId { get; set; }
        public string CountyName { get; set; }

        public ICollection<City> City { get; set; }
        public ICollection<Clients> Clients { get; set; }
    }
}
