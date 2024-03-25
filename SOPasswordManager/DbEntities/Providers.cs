using System;
using System.Collections.Generic;

namespace SOPasswordManager.DbEntities
{
    public partial class Providers
    {
        public Providers()
        {
            ProviderContact = new HashSet<ProviderContact>();
        }

        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public string BillingFullName { get; set; }
        public string IdCard { get; set; }
        public string FullAddress { get; set; }
        public string PostalCode { get; set; }

        public ICollection<ProviderContact> ProviderContact { get; set; }
    }
}
