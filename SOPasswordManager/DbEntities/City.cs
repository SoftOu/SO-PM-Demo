using System;
using System.Collections.Generic;

namespace SOPasswordManager.DbEntities
{
    public partial class City
    {
        public City()
        {
            Clients = new HashSet<Clients>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? CountryId { get; set; }

        public County Country { get; set; }
        public ICollection<Clients> Clients { get; set; }
    }
}
