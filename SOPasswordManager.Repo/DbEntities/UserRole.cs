using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class UserRole
    {
        public UserRole()
        {
            ClientUser = new HashSet<ClientUser>();
            SystemUser = new HashSet<SystemUser>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public ICollection<ClientUser> ClientUser { get; set; }
        public ICollection<SystemUser> SystemUser { get; set; }
    }
}
