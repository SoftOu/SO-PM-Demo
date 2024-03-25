using System;
using System.Collections.Generic;

namespace SOPasswordManager.Repo.DbEntities
{
    public partial class SystemUser
    {
        public SystemUser()
        {
            ProjectUser = new HashSet<ProjectUser>();
        }

        public int SytemUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public int? RoleId { get; set; }
        public bool? IsFirstLogin { get; set; }
        public bool? TwoStepVerification { get; set; }
        public int? VerificationCode { get; set; }

        public UserRole Role { get; set; }
        public ICollection<ProjectUser> ProjectUser { get; set; }
    }
}
