using System;
using System.Collections.Generic;

namespace SOPasswordManager.DbEntities
{
    public partial class ProviderContact
    {
        public int ProviderContactId { get; set; }
        public int? ProviderId { get; set; }
        public int? ProviderContactDetailId { get; set; }

        public Providers Provider { get; set; }
        public ProviderContactDetail ProviderContactDetail { get; set; }
    }
}
