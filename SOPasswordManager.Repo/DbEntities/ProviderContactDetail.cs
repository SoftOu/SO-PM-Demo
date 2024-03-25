using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class ProviderContactDetail
    {
        public ProviderContactDetail()
        {
            ProviderContact = new HashSet<ProviderContact>();
        }

        public int ProviderContactDetailId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public int? RoleId { get; set; }
        public string Notes { get; set; }

        public ICollection<ProviderContact> ProviderContact { get; set; }
    }
}
