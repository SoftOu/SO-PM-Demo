using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class ClientUser
    {
        public int ClientUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? ClientId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }

        public UserRole Role { get; set; }
    }
}
