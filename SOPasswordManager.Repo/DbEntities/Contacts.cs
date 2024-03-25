using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class Contacts
    {
        public Contacts()
        {
            ClientContacts = new HashSet<ClientContacts>();
        }

        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public int? RoleId { get; set; }
        public string Notes { get; set; }

        public ICollection<ClientContacts> ClientContacts { get; set; }
    }
}
