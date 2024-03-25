using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class ClientContacts
    {
        public int ClientContactId { get; set; }
        public int? ClientId { get; set; }
        public int? ContactId { get; set; }

        public Clients Client { get; set; }
        public Contacts Contact { get; set; }
    }
}
