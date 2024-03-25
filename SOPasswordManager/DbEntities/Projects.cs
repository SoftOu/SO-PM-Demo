using System;
using System.Collections.Generic;

namespace SOPasswordManager.DbEntities
{
    public partial class Projects
    {
        public Projects()
        {
            ProjectData = new HashSet<ProjectData>();
            ProjectUser = new HashSet<ProjectUser>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int? ClientId { get; set; }
        public int? ProviderId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }

        public ICollection<ProjectData> ProjectData { get; set; }
        public ICollection<ProjectUser> ProjectUser { get; set; }
    }
}
